using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbReader;
using DigitalHowdy.Server.Database;

namespace DigitalHowdy.Server.Employees
{
    public class EmployeeDAO : IEmployeeDAO
    {
        private IDbConnection _dBConnection;

        private ISqlQueries _sqlQueries;

        public EmployeeDAO(IDbConnection dbConnection, ISqlQueries sqlQueries)
        {
            _dBConnection = dbConnection;
            _sqlQueries = sqlQueries;

        }

        public async Task<bool> DeleteEmployee(long id)
        {
            var results = await _dBConnection.ExecuteAsync(_sqlQueries.DeleteEmployee, new { id = id });

            return results != 0;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployee()
        {
            var results = await _dBConnection.ReadAsync<EmployeeDTO>(_sqlQueries.GetAllEmployee);

            return results;
        }

        public async Task<EmployeeDTO> GetEmployeeById(long id)
        {
            var results = await _dBConnection.ReadAsync<EmployeeDTO>(_sqlQueries.GetEmployeeById, new { id = id });

            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeeByName(string name)
        {
            var results = await _dBConnection.ReadAsync<EmployeeDTO>(_sqlQueries.GetEmployeeByName, new { name = name });

            return results;
        }

        public async Task<EmployeeDTO> InsertEmployee(EmployeeDTO newEmployee)
        {
            await _dBConnection.ExecuteAsync(_sqlQueries.InsertEmployee, newEmployee);
            var employeeId = await _dBConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);

            newEmployee.Id = employeeId;

            return newEmployee;

        }

        public async Task<bool> UpdateEmployeePhone(long id, string phone)
        {
            var results = await _dBConnection.ExecuteAsync(_sqlQueries.UpdateEmployeePhone, new { Id = id, phone = phone });

            return results != 0;
        }

        public async Task<IEnumerable<EmployeeNameDTO>> GetAllEmployeeNamesAndId()
        {
            var results = await _dBConnection.ReadAsync<EmployeeNameDTO>(_sqlQueries.GetAllEmployeeNamesAndId);

            return results;

        }
    }
}