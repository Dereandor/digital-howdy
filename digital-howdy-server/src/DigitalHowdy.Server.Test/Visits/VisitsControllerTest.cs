using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.Host.Visits;
using DigitalHowdy.Server.Visits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using DigitalHowdy.Server.Visitors;
using DigitalHowdy.Server.Employees;

namespace DigitalHowdy.Server.Test.Visits
{
    public class VisitsControllerTest
    {
        private readonly VisitsController _controller;

        public VisitsControllerTest()
        {
            var mock = new Mock<IVisitDAO>();
            var testData = CreateTestData();

            var visitor = new VisitorDTO
            {
                Id = 7,
                Name = "New Visitor",
                Phone = "12345678",
                Organization = new OrganizationDTO
                {
                    Id = 2,
                    Name = "NTNU"
                }
            };

            var visitor2 = new VisitorDTO
            {
                Id = 8,
                Name = "New Visitor",
                Phone = "12345678",
                Organization = new OrganizationDTO
                {
                    Id = 2,
                    Name = "NTNU"
                }
            };

            var visit1 = new VisitDTO
            {
                Id = 4,
                Visitor = new VisitorDTO
                {
                    Id = 4,
                    Name = "Fernando Ferntsen",
                    Phone = "98765432",
                    Organization = new OrganizationDTO
                    {
                        Id = 2,
                        Name = "NTNU"
                    }
                },
                Employee = new EmployeeDTO
                {
                    Id = 4,
                    Name = "Erik Eriksen",
                    Phone = "44556677",
                    Email = "erieri@test.com"
                },
                StartDate = new DateTime(2022, 1, 2, 3, 1, 2), 
                Reference = 342516
            };

            var visit2 = new VisitDTO
            {
                Id = 7,
                Visitor = visitor,
                Employee = new EmployeeDTO
                {
                    Id = 7,
                    Name = "Bob Bobssonsson",
                    Phone = "76543212",
                    Email = "bobbob@test.com"
                },
                StartDate = new DateTime(2022, 1, 2, 3, 1, 2), 
                Reference = 342517
            };

            mock.Setup(vd => vd.GetAllVisits("", false, false)).ReturnsAsync(testData);
            mock.Setup(vd => vd.GetVisitById(1)).ReturnsAsync(testData.ElementAt(0));
            mock.Setup(vd => vd.GetVisitById(5)).ReturnsAsync(default(VisitDTO));
            mock.Setup(vd => vd.InsertVisit(It.Is<VisitInsertDTO>(iv => iv.Reference == 342516)))
            .ReturnsAsync(visit1);
            mock.Setup(vd => vd.InsertVisit(It.Is<VisitInsertDTO>(iv => iv.VisitorId == 7)))
            .ReturnsAsync(visit2);
            mock.Setup(vd => vd.InsertVisit(It.Is<VisitInsertDTO>(iv => iv.VisitorId == 8))).Throws(new EmployeeNotFoundException("Employee does not exist!"));
            mock.Setup(vd => vd.UpdateEndDateVisit(It.Is<VisitUpdateDTO>(uv => uv.EndDate == new DateTime(2026, 1, 2, 3, 1, 2)))).ReturnsAsync(true);
            mock.Setup(vd => vd.DeleteVisit(3)).ReturnsAsync(true);
            mock.Setup(vd => vd.DeleteVisit(5)).ReturnsAsync(false);

            var visitorMock = new Mock<IVisitorDAO>();
            visitorMock.Setup(vd => vd.InsertVisitor(It.Is<VisitorInsertDTO>(v => v.Name == "Greg Gregson"))).ReturnsAsync(visitor);
            visitorMock.Setup(vd => vd.InsertVisitor(It.Is<VisitorInsertDTO>(v => v.Name == "Joe Joeson"))).ReturnsAsync(visitor2);

            _controller = new VisitsController(null, null, mock.Object, visitorMock.Object, null, null);
        }

        [Fact]
        public async Task GetAllVisits_Should_Return3Visits()
        {
            var result = await _controller.GetAllVisits();

            var okResult = result as OkObjectResult;

            var data = okResult.Value as IEnumerable<VisitDTO>;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(3, data.Count());
        }

        [Fact]
        public async Task GetVisitById_Should_ReturnVisitWithId1()
        {
            var result = await _controller.GetVisitById(1);

            var okResult = result as OkObjectResult;

            var data = okResult.Value as VisitDTO;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(1, data.Id);
            Assert.Equal(123654, data.Reference);
        }

        [Fact]
        public async Task GetVisitById_Should_ReturnVisitNotFoud()
        {
            var result = await _controller.GetVisitById(5);

            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task RegisterVisit_Should_ReturnOkRegister()
        {
            var formData = new FormData
            {
                Name = "Greg Gregson",
                Phone = "12345678",
                Organization = "NTNU",
                Employee = new EmployeeDTO {
                    Id = 7,
                    Name = "Beth Bethsson",
                    Phone = "2345678"
                }
            };

            var result = await _controller.RegisterVisit(formData);

            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);

            var data = okResult.Value as VisitReferenceDTO;

            Assert.NotNull(data);
            Assert.Equal(7, data.Id);
            Assert.Equal(342517, data.Reference);
        }

        [Fact]
        public async Task RegisterVisit_Should_ReturnNotFoundRegister()
        {
            var formData = new FormData
            {
                Name = "Joe Joeson",
                Phone = "12345678",
                Organization = "NTNU",
                Employee = new EmployeeDTO {
                    Id = 8,
                    Name = "Jack Jackson",
                    Phone = "2345678"
                }
            };

            var result = await _controller.RegisterVisit(formData);

            var notFoundResult = result as NotFoundObjectResult;

            Assert.NotNull(notFoundResult);

            Assert.Equal("Employee does not exist!", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateEndDateVisit_Should_UpdateReturnTrue()
        {
            var newUpdate = new VisitUpdateDTO 
            {
                Id = 1,
                EndDate = new DateTime(2026, 1, 2, 3, 1, 2)
            };

            var result = await _controller.UpdateEndDateVisit(newUpdate);

            var okResult = result as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteVisit_Should_DeleteVisitWithId3()
        {
            var result = await _controller.DeleteVisit(3);

            var okResult = result as OkResult;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteVisit_Should_DeleteVisitNotFound()
        {
            var result = await _controller.DeleteVisit(5);

            var notFoundResult = result as NotFoundResult;

            Assert.NotNull(notFoundResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        private IEnumerable<VisitDTO> CreateTestData()
        {
            var visits = new List<VisitDTO>
            {
                new VisitDTO
                {
                    Id = 1,
                    Visitor = new VisitorDTO
                    {
                        Id = 1,
                        Name = "Bob Bobsson",
                        Phone = "12345678",
                        Organization = new OrganizationDTO
                        {
                            Id = 1,
                            Name = "NTNU"
                        }
                    },
                    Employee = new EmployeeDTO
                    {
                        Id = 1,
                        Name = "Anton Antonsen",
                        Phone = "11223344",
                        Email = "Antant@test.com"
                    },
                    StartDate = new System.DateTime(2021, 4, 12, 4, 1, 2),
                    Reference = 123654
                },
                new VisitDTO
                {
                    Id = 2,
                    Visitor = new VisitorDTO
                    {
                        Id = 2,
                        Name = "Bernard Bernardsen",
                        Phone = "2345678",
                        Organization = new OrganizationDTO
                        {
                            Id = 2,
                            Name = "DigitalHowdy"
                        }
                    },
                    Employee = new EmployeeDTO
                    {
                        Id = 2,
                        Name = "Bernt Berntsen",
                        Phone = "22334455",
                        Email = "berber@test.com"
                    },
                    StartDate = new System.DateTime(2022, 1, 2, 5, 6, 1),
                    Reference = 987123
                },
                new VisitDTO
                {
                    Id = 3,
                    Visitor = new VisitorDTO
                    {
                        Id = 3,
                        Name = "Sara Bernardsen",
                        Phone = "56784567",
                        Organization = new OrganizationDTO
                        {
                            Id = 2,
                            Name = "DigitalHowdy"
                        }
                    },
                    Employee = new EmployeeDTO
                    {
                        Id = 3,
                        Name = "Daniel Danielsen",
                        Phone = "33445566",
                        Email = "dandan@test.com"
                    },
                    StartDate = new System.DateTime(2023, 5, 3, 1, 2, 3)
                }
            };

            return visits;
        }
    }
}