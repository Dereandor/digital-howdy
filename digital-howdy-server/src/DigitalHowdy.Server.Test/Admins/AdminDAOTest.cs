using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.Admins;
using DigitalHowdy.Server.Authentication;
using DigitalHowdy.Server.Test.Utils;
using LightInject;
using LightInject.xUnit2;
using Xunit;

namespace DigitalHowdy.Server.Test.Admins
{
    [Collection("Integration")]
    public class AdminDAOTest
    {
        [Theory, InjectData]
        public async Task Should_Return3Admins(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await adminDAO.GetAllAdmins();

            Assert.Equal(3, results.Count());
        }

        [Theory, InjectData]
        public async Task Should_DeleteAdmin_IfExists(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var result = await adminDAO.DeleteAdmin(2);

            Assert.True(result);

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(2, admins.Count());
        }

        [Theory, InjectData]
        public async Task Should_NotDeleteAdmin_IfNotExists(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var result = await adminDAO.DeleteAdmin(4);

            Assert.False(result);

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(3, admins.Count());
        }

        [Theory, InjectData]
        public async Task Should_InsertAdmin_IfNotExists(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminInsertDTO
            {
                Username = "erikeri",
                HashedPassword = "asdf1234"
            };

            var insertedAdmin = await adminDAO.InsertAdmin(newAdmin);

            Assert.NotNull(insertedAdmin);
            Assert.Equal(4, insertedAdmin.Id);
            Assert.Equal("erikeri", insertedAdmin.Username);
        }

        [Theory, InjectData]
        public async Task Should_FailOnInsert_IfAlreadyExists(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminInsertDTO
            {
                Username = "danieldan",
                HashedPassword = "asdf1234"
            };

            await Assert.ThrowsAsync<AdminExistsException>(async () => await adminDAO.InsertAdmin(newAdmin));

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(3, admins.Count());
        }

        [Theory, InjectData]
        public async Task RegisterAdmin_Should_RegisterAdmin(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminRegisterDTO
            {
                Username = "erieri",
                Password = "Asdf1234",
                ConfirmPassword = "Asdf1234"
            };

            var insertedAdmin = await adminDAO.RegisterAdmin(newAdmin);

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(4, admins.Count());
            Assert.Equal(4, insertedAdmin.Id);
            Assert.Equal("erieri", insertedAdmin.Username);
        }

        [Theory, InjectData]
        public async Task RegisterAdmin_Should_Fail_BreaksPasswordPolicy(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminRegisterDTO
            {
                Username = "erieri",
                Password = "asdf1234",
                ConfirmPassword = "asdf1234"
            };

            await Assert.ThrowsAsync<ValidationErrorException>(async () => await adminDAO.RegisterAdmin(newAdmin));

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(3, admins.Count());
        }

        [Theory, InjectData]
        public async Task RegisterAdmin_Should_Fail_BreaksPasswordPolicy2(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminRegisterDTO
            {
                Username = "erieri",
                Password = "Asdf1234",
                ConfirmPassword = "Asdf12345"
            };

            await Assert.ThrowsAsync<ValidationErrorException>(async () => await adminDAO.RegisterAdmin(newAdmin));

            var admins = await adminDAO.GetAllAdmins();

            Assert.Equal(3, admins.Count());
        }

        [Theory, InjectData]
        public async Task LoginAdmin_Should_Succeed(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminRegisterDTO
            {
                Username = "erieri",
                Password = "Asdf1234",
                ConfirmPassword = "Asdf1234"
            };

            await adminDAO.RegisterAdmin(newAdmin);

            var adminLogin = new AdminLoginDTO
            {
                Username = "erieri",
                HashedPassword = "Asdf1234"
            };

            var token = await adminDAO.LoginAdmin(adminLogin);

            Assert.NotNull(token);
        }

        [Theory, InjectData]
        public async Task LoginAdmin_Should_Fail_WrongPassword(AdminDAO adminDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var newAdmin = new AdminRegisterDTO
            {
                Username = "erieri",
                Password = "Asdf1234",
                ConfirmPassword = "Asdf1234"
            };

            await adminDAO.RegisterAdmin(newAdmin);

            var adminLogin = new AdminLoginDTO
            {
                Username = "erieri",
                HashedPassword = "Asdf12345"
            };

            await Assert.ThrowsAsync<AuthenticationFailedException>(async () => await adminDAO.LoginAdmin(adminLogin));
        }

        public static void Configure(ServiceContainer container)
        {
            container.RegisterFrom<TestCompositionRoot>();
        }
    }
}