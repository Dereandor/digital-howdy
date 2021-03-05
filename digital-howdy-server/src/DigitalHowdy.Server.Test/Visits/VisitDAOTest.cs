using DigitalHowdy.Server.Visits;
using Xunit;
using LightInject;
using LightInject.xUnit2;
using System.Threading.Tasks;
using DigitalHowdy.Server.Test.Utils;
using System.Linq;
using System;
using DigitalHowdy.Server.Employees;

namespace DigitalHowdy.Server.Test.Visits
{
    [Collection("Integration")]
    public class VisitDAOTest
    {
        [Theory, InjectData]
        public async Task GetAllVisits_Should_Return3Visits(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitDAO.GetAllVisits();

            Assert.Equal(3, results.Count());
        }

        [Theory, InjectData]
        public async Task GetAllVisits_Should_Return2Visits_ByQuery(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitDAO.GetAllVisits("an");

            Assert.Equal(2, results.Count());
        }

        [Theory, InjectData]
        public async Task GetVisitById_Should_ReturnVisitId1(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var result = await visitDAO.GetVisitById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Visitor.Id);
            Assert.Equal("Bob Bobsson", result.Visitor.Name);
            Assert.Equal("12345678", result.Visitor.Phone);
            Assert.Equal(1, result.Visitor.Organization.Id);
            Assert.Equal("NTNU", result.Visitor.Organization.Name);
            Assert.Equal(3, result.Employee.Id);
            Assert.Equal("Daniel Danielsen", result.Employee.Name);
            Assert.Equal("33445566", result.Employee.Phone);
            Assert.Equal("dandan@test.com", result.Employee.Email);
            Assert.Equal("2020-10-21 11:22:33", result.StartDate.ToString("yyyy-MM-dd hh:mm:ss"));
            Assert.Equal(123456, result.Reference);
        }

        [Theory, InjectData]
        public async Task InsertVisit_Should_InsertVisit(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var newVisit = new VisitInsertDTO
            {
                VisitorId = 1,
                EmployeeId = 4,
                StartDate = new DateTime(2021, 7, 1, 4, 23, 42),
                Reference = 0
            };

            var inserted = await visitDAO.InsertVisit(newVisit);
            var retrieved = await visitDAO.GetVisitById(4);

            Assert.Equal("2021-07-01 04:23:42", retrieved.StartDate.ToString("yyyy-MM-dd hh:mm:ss"));
            Assert.NotNull(retrieved);
        }

        [Theory, InjectData]
        public async Task InsertVisit_Should_Fail_EmployeeNotExist(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var newVisit = new VisitInsertDTO
            {
                VisitorId = 1,
                EmployeeId = 5,
                StartDate = new DateTime(2021, 7, 1, 4, 23, 42),
                Reference = 696969
            };

            await Assert.ThrowsAsync<EmployeeNotFoundException>(() => visitDAO.InsertVisit(newVisit));
        }

        [Theory, InjectData]
        public async Task UpdateEndDateVisit_Should_UpdateEndDate(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var visitUpdate = new VisitUpdateDTO
            {
                Id = 1,
                EndDate = new DateTime(2023, 11, 21, 15, 53, 23)
            };

            var result = await visitDAO.UpdateEndDateVisit(visitUpdate);

            Assert.True(result);

            var retrieved = await visitDAO.GetVisitById(1);

            Assert.NotNull(retrieved);
        }

        [Theory, InjectData]
        public async Task DeleteVisit_Should_DeleteVisit(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            await visitDAO.DeleteVisit(4);

            var retrieved = await visitDAO.GetVisitById(4);

            Assert.Null(retrieved);
        }

        [Theory, InjectData]
        public async Task RemoveVisitReference_Should_DeleteReferenceFromVisit(VisitDAO visitDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var results = await visitDAO.RemoveVisitReference(1);

            Assert.False(results);

        }

        public static void Configure(ServiceContainer container)
        {
            container.RegisterFrom<TestCompositionRoot>();
        }
    }
}