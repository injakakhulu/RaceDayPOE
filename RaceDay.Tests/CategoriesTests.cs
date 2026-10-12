
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RaceDay.API.Data;
using RaceDay.API.Controllers;
using Xunit;

namespace RaceDay.Tests
{
    public class CategoriesTests
    {
        [Fact]
        public async Task GetCategories_ReturnsOk()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);
            var controller = new CategoriesController(context);

            var result = await controller.GetCategories();

            Assert.IsType<OkObjectResult>(result);
        }
        ///////////////////////
        ///

        [Fact]
        public async Task GetCategories_ReturnsAddedCategory()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new RaceDayDbContext(options);

            context.Categories.Add(new RaceDay.API.Models.Category
            {
                EventId = 1,
                CategoryName = "5km Open",
                CategoryType = "Running",
                Distance = 5,
                MinimumAge = 18,
                MaximumAge = 60
            });

            await context.SaveChangesAsync();

            var controller = new CategoriesController(context);
            var result = await controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<IEnumerable<RaceDay.API.Models.Category>>(
                okResult.Value);

            Assert.Single(categories);
            Assert.Equal("5km Open", categories.First().CategoryName);
        }

        /////////////////////////////////////

    }
}
