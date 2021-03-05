using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.Host.Employees;
using DigitalHowdy.Server.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DigitalHowdy.Server.Test.Employees
{
    public class EmployeesControllerTest
    {
        private readonly EmployeesController _controller;

        public EmployeesControllerTest()
        {
            var mock = new Mock<IEmployeeDAO>();
            var testData = CreateTestData();
            mock.Setup(ed => ed.GetAllEmployee()).ReturnsAsync(testData);
            mock.Setup(ed => ed.GetEmployeeById(1)).ReturnsAsync(testData.ElementAt(0));
            mock.Setup(ed => ed.GetEmployeeById(5)).ReturnsAsync(default(EmployeeDTO));
            mock.Setup(ed => ed.GetEmployeeByName("Anton")).ReturnsAsync(testData);
            mock.Setup(ed => ed.InsertEmployee(It.Is<EmployeeDTO>(e => e.Phone == "11223344"))).ReturnsAsync(new EmployeeDTO{
                    Id = 5,
                    Name = "Joe Joeson",
                    Phone = "11223344",
                    Email = "joejoe@test.com"
                });
            _controller = new EmployeesController(null, mock.Object);
        }

        [Fact]
        public async Task GetEmployee_ShouldReturn_ReturnOk4Results()
        {
            var result = await _controller.GetEmployee(null);

            var okResult = result as OkObjectResult;

            var data = okResult.Value as IEnumerable<EmployeeDTO>;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            Assert.Equal(4, data.Count());
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnOk1Result()
        {
            var result = await _controller.GetEmployeeById(1);

            var okResult = result as OkObjectResult;

            var data = okResult.Value as EmployeeDTO;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            Assert.Equal(1, data.Id);
            Assert.Equal("Anton Antonsen", data.Name);
        }

        [Fact]
        public async Task GetEmployeeById_Should_ReturnNotFound()
        {
            var result = await _controller.GetEmployeeById(5);

            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetEmployee_Should_ReturnOneResultByName()
        {
            var result = await _controller.GetEmployee("Anton");

            var okResult = result as OkObjectResult;

            var data = okResult.Value as IEnumerable<EmployeeDTO>;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            Assert.Equal(1, data.FirstOrDefault().Id);
            Assert.Equal("Anton Antonsen", data.FirstOrDefault().Name);
        }

        [Fact]
        public async Task InsertEmployee_Should_ReturnCreated()
        {
            var employee = new EmployeeDTO
            {
                Name = "Joe Joeson",
                Phone = "11223344",
                Email = "joejoe@test.com"
            };

            var response = await _controller.InsertEmployee(employee);

            var createdResult = response as CreatedResult;

            Assert.NotNull(createdResult);

            var createdEmployee = createdResult.Value as EmployeeDTO;

            Assert.NotNull(createdEmployee);
            Assert.Equal(5, createdEmployee.Id);
            Assert.Equal("Joe Joeson", createdEmployee.Name);
            Assert.Equal("11223344", createdEmployee.Phone);
            Assert.Equal("joejoe@test.com", createdEmployee.Email);
        }

        private IEnumerable<EmployeeDTO> CreateTestData()
        {
            var employees = new List<EmployeeDTO>
            {
                new EmployeeDTO{
                    Id = 1,
                    Name = "Anton Antonsen",
                    Phone = "11223344",
                    Email = "antant@test.com"
                },
                new EmployeeDTO{
                    Id = 2,
                    Name = "Bernt Berntsen",
                    Phone = "22334455",
                    Email = "berber@test.com"
                },
                new EmployeeDTO{
                    Id = 3,
                    Name = "Daniel Danielsen",
                    Phone = "33445566",
                    Email = "dandan@test.com"
                },
                new EmployeeDTO{
                    Id = 4,
                    Name = "Erik Eriksen",
                    Phone = "44556677",
                    Email = "erieri@test.com"
                }
            };
            return employees;
        }
    }
}