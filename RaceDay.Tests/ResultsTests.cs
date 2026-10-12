
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.API.Data;
using RaceDay.API.Controllers;
using Xunit;

namespace RaceDay.Tests
{
    public class ResultsTests
    {
        [Fact]
        public async Task GetMyResults_ReturnsEmptyList_WhenNoResultsExist()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);
            var controller = new ResultsController(context);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            httpContext.Session.SetInt32("UserId", 1);

            var result = await controller.GetMyResults();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var results = Assert.IsAssignableFrom<IEnumerable<RaceDay.API.Models.Result>>(
                okResult.Value);

            Assert.Empty(results);
        }
    }
}
