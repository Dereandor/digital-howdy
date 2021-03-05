using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using DbReader;
using DigitalHowdy.Server.Authentication;
using DigitalHowdy.Server.Authorization;
using DigitalHowdy.Server.Database;

namespace DigitalHowdy.Server.Admins
{
    public class AdminDAO : IAdminDAO
    {
        private readonly IDbConnection _dbConnection;
        private readonly ISqlQueries _sqlQueries;
        private readonly IPasswordManager _passwordManager;
        private readonly ITokenManager _tokenManager;
        private readonly IPasswordPolicy _passwordPolicy;

        public AdminDAO(IDbConnection dbConnection, ISqlQueries sqlQueries, IPasswordManager passwordManager, ITokenManager tokenManager, IPasswordPolicy passwordPolicy)
        {
            _dbConnection = dbConnection;
            _sqlQueries = sqlQueries;
            _passwordManager = passwordManager;
            _tokenManager = tokenManager;
            _passwordPolicy = passwordPolicy;
        }

        public async Task<IEnumerable<AdminDTO>> GetAllAdmins()
        {
            var results = await _dbConnection.ReadAsync<AdminDTO>(_sqlQueries.GetAllAdmins);

            return results;
        }

        public async Task<AdminDTO> InsertAdmin(AdminInsertDTO insertAdmin)
        {
            try
            {
                await _dbConnection.ExecuteAsync(_sqlQueries.InsertAdmin, insertAdmin);
                var adminId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);
                var insertedAdmin = new AdminDTO
                {
                    Id = adminId,
                    Username = insertAdmin.Username
                };

                return insertedAdmin;
            }
            catch (MySqlConnector.MySqlException e)
            {
                if (e.Number == 7262 || e.Number == 1062)
                {
                    throw new AdminExistsException("This user is already registered as an admin");
                }
                else
                {
                    throw e;
                }
                
            }
        }

        public async Task<bool> DeleteAdmin(long adminId)
        {
            var affectedRows = await _dbConnection.ExecuteAsync(_sqlQueries.DeleteAdmin, new { id = adminId });

            return (affectedRows == 1);
        }

        public async Task<string> LoginAdmin(AdminLoginDTO adminLogin)
        {
            var hashedPassword = await _dbConnection.ExecuteScalarAsync<string>(_sqlQueries.GetAdminPasswordByUsername, new { username = adminLogin.Username });

            if (hashedPassword == null || !_passwordManager.VerifyPassword(adminLogin.HashedPassword, hashedPassword))
            {
                throw new AuthenticationFailedException("Invalid username or password.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, adminLogin.Username)
            };

            var token = _tokenManager.GenerateToken(claims);
            return token;
        }

        public async Task<AdminDTO> RegisterAdmin(AdminRegisterDTO adminRegister)
        {
            _passwordPolicy.ApplyPolicy(adminRegister.Password, adminRegister.ConfirmPassword);
            var hashedPassword = _passwordManager.HashPassword(adminRegister.Password);
            var insertAdmin = new AdminInsertDTO
            {
                Username = adminRegister.Username,
                HashedPassword = hashedPassword
            };

            var insertedAdmin = await InsertAdmin(insertAdmin);

            return insertedAdmin; 
        }
    }
}