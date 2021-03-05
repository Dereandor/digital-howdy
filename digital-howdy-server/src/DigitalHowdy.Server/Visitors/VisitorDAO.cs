using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbReader;
using DigitalHowdy.Server.Database;

namespace DigitalHowdy.Server.Visitors
{
    public class VisitorDAO : IVisitorDAO
    {
        private IDbConnection _dbConnection;
        private ISqlQueries _sqlQueries;

        public VisitorDAO(IDbConnection dbConnection, ISqlQueries sqlQueries)
        {
            _dbConnection = dbConnection;
            _sqlQueries = sqlQueries;
        }

        public async Task<IEnumerable<VisitorDTO>> GetAllVisitors(string query = "")
        {
            IEnumerable<VisitorDTO> results;

            if (query == "")
            {
                results = await _dbConnection.ReadAsync<VisitorDTO>(_sqlQueries.GetAllVisitors);
            }
            else
            {
                results = await _dbConnection.ReadAsync<VisitorDTO>(_sqlQueries.GetVisitorsByQuery, new { query = query });
            }


            return results;
        }



        public async Task<VisitorDTO> GetVisitorByPhone(string phone)
        {
            var results = await _dbConnection.ReadAsync<VisitorDTO>(_sqlQueries.GetVisitorByPhone, new { phone = phone });

            return results.FirstOrDefault();
        }

        public async Task<VisitorDTO> InsertVisitor(VisitorInsertDTO insertVisitor)
        {
            var organizations = await _dbConnection.ReadAsync<OrganizationDTO>(_sqlQueries.GetOrganizationByName, new { name = insertVisitor.OrganizationName });
            var organization = organizations.FirstOrDefault();

            if (organization == null)
            {
                await _dbConnection.ExecuteAsync(_sqlQueries.InsertOrganization, new { name = insertVisitor.OrganizationName });
                insertVisitor.OrganizationId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);
                organization = new OrganizationDTO
                {
                    Id = insertVisitor.OrganizationId,
                    Name = insertVisitor.OrganizationName
                };
            }
            else
            {
                insertVisitor.OrganizationId = organization.Id;
            }

            var visitor = await GetVisitorByPhone(insertVisitor.Phone);
            if (visitor == null)
            {
                await _dbConnection.ExecuteAsync(_sqlQueries.InsertVisitor, insertVisitor);
                var visitorId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);

                visitor = new VisitorDTO
                {
                    Id = visitorId,
                    Name = insertVisitor.Name,
                    Phone = insertVisitor.Phone,
                    Organization = organization
                };
            }
            else
            {
                var updateVisitor = new VisitorUpdateDTO
                {
                    Id = visitor.Id,
                    OrganizationName = visitor.Organization.Name
                };

                await UpdateVisitor(updateVisitor);
            }

            return visitor;
        }

        public async Task<bool> UpdateVisitor(VisitorUpdateDTO updateVisitor)
        {
            var organizationName = updateVisitor.OrganizationName;
            var organizations = await _dbConnection.ReadAsync<OrganizationDTO>(_sqlQueries.GetOrganizationByName, new { name = organizationName });
            var organization = organizations.FirstOrDefault();

            long organizationId;

            if (organization == null)
            {
                await _dbConnection.ExecuteAsync(_sqlQueries.InsertOrganization, new { name = organizationName });
                organizationId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);
            }
            else
            {
                organizationId = organization.Id;
            }

            var affectedRows = await _dbConnection.ExecuteAsync(_sqlQueries.UpdateVisitor, new { id = updateVisitor.Id, organizationId = organizationId });

            return (affectedRows == 1);
        }


    }
}
