
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Data;
using RaceDay.API.Controllers;
using Xunit;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDay.API.Data;
using RaceDay.API.Controllers;
using Xunit;

namespace RaceDay.Tests
{
    public class EnrolmentsTests
    {
        [Fact]
        public async Task GetMyEnrolments_ReturnsEmptyList_WhenNoEnrolmentsExist()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);
            var controller = new EnrolmentsController(context);

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            httpContext.Session.SetInt32("UserId", 1);

            var result = await controller.GetMyEnrolments();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var enrolments = Assert.IsAssignableFrom<IEnumerable<RaceDay.API.Models.Enrolment>>(
                okResult.Value);

            Assert.Empty(enrolments);
        }
    }
}
