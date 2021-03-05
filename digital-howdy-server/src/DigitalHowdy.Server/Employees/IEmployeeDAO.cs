using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHowdy.Server.Employees
{
    public interface IEmployeeDAO
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployee();
        Task<EmployeeDTO> GetEmployeeById(long id);
        Task<IEnumerable<EmployeeDTO>> GetEmployeeByName(string name);
        Task<bool> UpdateEmployeePhone(long id, string phone);
        Task<EmployeeDTO> InsertEmployee(EmployeeDTO employee);
        Task<IEnumerable<EmployeeNameDTO>> GetAllEmployeeNamesAndId();
    }
}