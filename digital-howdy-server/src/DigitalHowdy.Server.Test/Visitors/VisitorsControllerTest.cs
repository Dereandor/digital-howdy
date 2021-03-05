using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.Host.Visitors;
using DigitalHowdy.Server.Visitors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DigitalHowdy.Server.Test.Visitors
{
    public class VisitorsControllerTest
    {
        private readonly VisitorsController _controller;

        public VisitorsControllerTest()
        {
            var mock = new Mock<IVisitorDAO>();
            var testData = CreateTestData();
            mock.Setup(vd => vd.GetAllVisitors("")).ReturnsAsync(testData);
            _controller = new VisitorsController(null, mock.Object);
        }


        [Fact]
        public async Task GetAllVisitors_Should_ReturnOk3Visitors()
        {
            var result = await _controller.GetAllVisitors();

            var okResult = result as OkObjectResult;

            var data = okResult.Value as IEnumerable<VisitorDTO>;

            Assert.NotNull(okResult);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            Assert.Equal(3, data.Count());
        }

        private IEnumerable<VisitorDTO> CreateTestData()
        {
            var visitors = new List<VisitorDTO>
            {
                new VisitorDTO
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
                new VisitorDTO
                {
                    Id = 2,
                    Name = "Bernard Bernardsen",
                    Phone = "23456789",
                    Organization = new OrganizationDTO
                    {
                        Id = 2,
                        Name = "DigitalHowdy"
                    }
                },
                new VisitorDTO
                {
                    Id = 3,
                    Name = "Sara Bernardsen",
                    Phone = "56784567",
                    Organization = new OrganizationDTO
                    {
                        Id = 2,
                        Name = "DigitalHowdy"
                    }
                }
            };

            return visitors;
        }
    }
}