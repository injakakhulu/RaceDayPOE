
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Data;
using RaceDay.API.Controllers;
using Xunit;

namespace RaceDay.Tests
{
    public class EventsTests
    {
        [Fact]
        public async Task GetEvents_ReturnsOk()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);
            var controller = new EventsController(context);

            var result = await controller.GetEvents();

            Assert.IsType<OkObjectResult>(result);
        }
        ///////////////////////////
        ///

        [Fact]
        public async Task GetEvents_ReturnsAddedEvent()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);

            context.Events.Add(new RaceDay.API.Models.Event
            {
                EventName = "Test Fun Run",
                Description = "Unit test event",
                Location = "Johannesburg",
                EventDate = DateTime.UtcNow.AddDays(7),
                Distance = 5,
                EventType = "Running",
                OrganiserId = 1,
                CreatedAt = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var controller = new EventsController(context);
            var result = await controller.GetEvents();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var events = Assert.IsAssignableFrom<IEnumerable<RaceDay.API.Models.Event>>(
                okResult.Value);

            Assert.Single(events);
            Assert.Equal("Test Fun Run", events.First().EventName);
        }
        ///////////////////
        ///

        [Fact]
        public async Task GetEvent_ReturnsNotFound_WhenEventDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);
            var controller = new EventsController(context);

            var result = await controller.GetEvent(999);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        //////////////////////////////


    }
}
