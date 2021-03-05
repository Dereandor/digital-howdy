using DigitalHowdy.Server.Visitors;
using Xunit;
using LightInject;
using LightInject.xUnit2;
using System.Threading.Tasks;
using DigitalHowdy.Server.Test.Utils;
using System.Linq;

namespace DigitalHowdy.Server.Test.Visitors
{
    [Collection("Integration")]
    public class VisitorDAOTest
    {
        [Theory, InjectData]
        public async Task GetAllVisitors_Should_Return3Visitors(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitorDAO.GetAllVisitors();

            Assert.Equal(3, results.Count());
        }

        [Theory, InjectData]
        public async Task GetAllVisitors_Should_Return2Visitors_WithQuery(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitorDAO.GetAllVisitors("bernard");

            Assert.Equal(2, results.Count());

            Assert.Equal("Bernard Bernardsen", results.ElementAt(0).Name);
            Assert.Equal("Sara Bernardsen", results.ElementAt(1).Name);
        }

        [Theory, InjectData]
        public async Task GetAllVisitors_Should_Return1Visitor_WithQuery(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitorDAO.GetAllVisitors("NTNU");

            Assert.Single(results);
        }

        [Theory, InjectData]
        public async Task GetAllVisitors_Should_Return3Visitors_WithQuery(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await visitorDAO.GetAllVisitors("o");

            Assert.Equal(3, results.Count());
        }

        [Theory, InjectData]
        public async Task GetVisitorByPhone_Should_Return1Visitor_WithPhone(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var result = await visitorDAO.GetVisitorByPhone("12345678");

            Assert.NotNull(result);
            Assert.Equal("Bob Bobsson", result.Name);
            Assert.Equal("12345678", result.Phone);
            Assert.NotNull(result.Organization);
            Assert.Equal(1, result.Organization.Id);
            Assert.Equal("NTNU", result.Organization.Name);
        }

        [Theory, InjectData]
        public async Task GetVisitorByPhone_Should_ReturnNoVisitor_ByPhone(VisitorDAO visitorDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var result = await visitorDAO.GetVisitorByPhone("84739576");

            Assert.Null(result);
        }

        public static void Configure(ServiceContainer container)
        {
            container.RegisterFrom<TestCompositionRoot>();
        }
    }
}