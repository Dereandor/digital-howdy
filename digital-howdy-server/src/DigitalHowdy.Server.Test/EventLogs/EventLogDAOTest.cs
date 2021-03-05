using System;
using System.Linq;
using System.Threading.Tasks;
using DigitalHowdy.Server.EventLogs;
using DigitalHowdy.Server.Test.Utils;
using LightInject;
using LightInject.xUnit2;
using Xunit;

namespace DigitalHowdy.Server.Test.EventLogs
{
    [Collection("Integration")]
    public class EventLogDAOTest
    {
        [Theory, InjectData]
        public async Task GetAllEventLogs_Should_Return3Logs(EventLogDAO eventLogDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();
            var results = await eventLogDAO.GetAllEvents();

            Assert.Equal(3, results.Count());
        }

        [Theory, InjectData]
        public async Task InsertEventLog_Should_ReturnNewEventLog(EventLogDAO eventLogDAO, TestDataCreator testData)
        {
            await testData.CreateTestData();

            var eventLog = new EventLogDTO
            {
                Date = DateTime.Parse("2020-10-21 11:22:33"),
                Description = "User antonant checked in visitor Sara Bernardsen"
            };

            var inserted = await eventLogDAO.InsertEvent(eventLog);

            Assert.Equal(4, inserted.Id);
            Assert.Equal("2020-10-21 11:22:33", inserted.Date.ToString("yyyy-MM-dd hh:mm:ss"));
            Assert.Equal("User antonant checked in visitor Sara Bernardsen", inserted.Description);
        }

        public static void Configure(ServiceContainer container)
        {
            container.RegisterFrom<TestCompositionRoot>();
        }
    }
}