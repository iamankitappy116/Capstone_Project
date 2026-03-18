using Xunit;
using Microsoft.EntityFrameworkCore;
using EFExample.Controllers;
using MyModels;
using repo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EFExample.Tests
{
    public class CarsControllerTests
    {
        private RepoContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<RepoContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new RepoContext(options);

            context.cars.AddRange(
                new Car { Id = 1, Name = "BMW", Price = 50000 },
                new Car { Id = 2, Name = "Audi", Price = 60000 }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task Index_ReturnsView_WithListOfCars()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new CarsController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<Car>>(viewResult.Model);

            Assert.Equal(2, model.Count());
        }
    }
}