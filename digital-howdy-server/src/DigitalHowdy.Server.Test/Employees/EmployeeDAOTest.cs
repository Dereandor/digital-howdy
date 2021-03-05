using DigitalHowdy.Server.Employees;
using Xunit;
using LightInject;
using LightInject.xUnit2;
using System.Threading.Tasks;
using DigitalHowdy.Server.Test.Utils;
using System.Linq;

namespace DigitalHowdy.Server.Test.Employees
{
    [Collection("Integration")]
    public class EmployeeDAOTest
    {
        [Theory, InjectData]
        public async Task GetAllEmployee_Should_Return4Employees(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await employeeDAO.GetAllEmployee();

            Assert.Equal(4, results.Count());
        }

        [Theory, InjectData]
        public async Task GetEmployeeById_Should_ReturnOneEmployee(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await employeeDAO.GetEmployeeById(1);

            Assert.NotNull(results);
            Assert.Equal("Anton Antonsen", results.Name);
            Assert.Equal("11223344", results.Phone);
            Assert.Equal("antant@test.com", results.Email);
        }

        [Theory, InjectData]
        public async Task GetEmployeeById_Should_ReturnNull_NotExists(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await employeeDAO.GetEmployeeById(10);

            Assert.Null(results);
        }

        [Theory, InjectData]
        public async Task GetEmployeeByName_Should_ReturnOneEmployee(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await employeeDAO.GetEmployeeByName("Anton");

            Assert.NotNull(results);
            Assert.Equal("Anton Antonsen", results.ElementAt(0).Name);
            Assert.Equal("11223344", results.ElementAt(0).Phone);
            Assert.Equal("antant@test.com", results.ElementAt(0).Email);
        }

        [Theory, InjectData]
        public async Task DeleteEmployee_Should_DeleteOneEmployee(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await employeeDAO.DeleteEmployee(4);

            Assert.True(results);

            var all = await employeeDAO.GetAllEmployee();

            Assert.Equal(3, all.Count());

            var deleted = await employeeDAO.GetEmployeeById(4);

            Assert.Null(deleted);
        }

        [Theory, InjectData]
        public async Task InsertEmployee_Should_CreateNewEmployee(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var newEmployee = new EmployeeDTO
            {
                Name = "Frank Frankson",
                Phone = "55667788",
                Email = "frafra@test.com"
            };

            var created = await employeeDAO.InsertEmployee(newEmployee);

            Assert.NotNull(created);
            Assert.Equal("Frank Frankson", created.Name);
        }

        [Theory, InjectData]
        public async Task UpdateEmployeePhone_Should_UpdatePhoneOfOneEmployee(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var results = await employeeDAO.UpdateEmployeePhone(2, "99887766");

            Assert.True(results);

            var newPhone = await employeeDAO.GetEmployeeById(2);

            Assert.Equal("99887766", newPhone.Phone);
        }

        [Theory, InjectData]
        public async Task GetAllEmployeeNamesAndId_Should_Return4ResultsWithName(EmployeeDAO employeeDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var results = await employeeDAO.GetAllEmployeeNamesAndId();

            Assert.NotNull(results);
            Assert.Equal(4, results.Count());
            Assert.Equal("Anton Antonsen", results.ElementAt(0).Name);

        }

        public static void Configure(ServiceContainer container)
        {
            container.RegisterFrom<TestCompositionRoot>();
        }
    }
}