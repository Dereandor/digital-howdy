using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.Admins;
using DigitalHowdy.Server.Host.Admins;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DigitalHowdy.Server.Test.Admins
{
    public class AdminsControllerTest
    {
        private readonly AdminsController _controller;

        public AdminsControllerTest()
        {
            var mock = new Mock<IAdminDAO>();
            var testData = CreateTestData();
            mock.Setup(ad => ad.GetAllAdmins()).ReturnsAsync(testData);
            mock.Setup(ad => ad.RegisterAdmin(It.Is<AdminRegisterDTO>(ard => ard.Username == "erikeri"))).ReturnsAsync(new AdminDTO { Id = 4, Username = "erikeri" });
            mock.Setup(ad => ad.RegisterAdmin(It.Is<AdminRegisterDTO>(ard => ard.Username == "antonant"))).ThrowsAsync(new AdminExistsException("This user is already registered as an admin"));
            mock.Setup(ad => ad.DeleteAdmin(1)).ReturnsAsync(true);
            mock.Setup(ad => ad.DeleteAdmin(4)).ReturnsAsync(false);

            _controller = new AdminsController(null, mock.Object);
        }

        [Fact]
        public async Task GetAllAdmins_Should_Return3Admins()
        {
            var response = await _controller.GetAllAdmins();

            var okResult = response as OkObjectResult;

            Assert.NotNull(okResult);

            var data = okResult.Value as IEnumerable<AdminDTO>;

            Assert.NotNull(data);
            Assert.Equal(3, data.Count());
        }

        [Fact]
        public async Task RegisterAdmin_Should_ReturnCreated()
        {
            var newAdmin = new AdminRegisterDTO { Username = "erikeri", Password = "Asdf1234", ConfirmPassword = "Asdf1234" };

            var response = await _controller.RegisterAdmin(newAdmin);

            var createdResult = response as CreatedResult;

            Assert.NotNull(createdResult);

            var data = createdResult.Value as AdminDTO;

            Assert.NotNull(data);
            Assert.Equal(4, data.Id);
            Assert.Equal("erikeri", data.Username);
        }

        [Fact]
        public async Task RegisterAdmin_Should_ReturnConflict_AlreadyExists()
        {
            var newAdmin = new AdminRegisterDTO { Username = "antonant", Password = "Asdf1234", ConfirmPassword = "Asdf1234" };

            var response = await _controller.RegisterAdmin(newAdmin);

            var conflictResult = response as ConflictObjectResult;

            Assert.NotNull(conflictResult);

            var error = conflictResult.Value;

            Assert.NotNull(error);
        }

        [Fact]
        public async Task DeleteAdmin_Should_ReturnOK()
        {
            var response = await _controller.DeleteAdmin(1);

            var okResult = response as OkResult;

            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task DeleteAdmin_Should_ReturnNotFound_NotExists()
        {
            var response = await _controller.DeleteAdmin(4);

            var notFoundResult = response as NotFoundResult;

            Assert.NotNull(notFoundResult);
        }

        private IEnumerable<AdminDTO> CreateTestData()
        {
            var admins = new List<AdminDTO>
            {
                new AdminDTO
                {
                    Id = 1,
                    Username = "antonant"
                },
                new AdminDTO
                {
                    Id = 2,
                    Username = "berntber"
                },
                new AdminDTO
                {
                    Id = 3,
                    Username = "danieldan"
                }
            };

            return admins;
        }
    }
}