using AwesomeDevEvents.API.Controllers;
using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace TestProjectApi
{
    public class DevEventsPost
    {
        private DevEventsDbContext _dbContext;
        private DevEventsController _controller;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<DevEventsDbContext> options = new DbContextOptionsBuilder<DevEventsDbContext>()
              .UseInMemoryDatabase(databaseName: "test_db")
              .Options;

            _dbContext = new DevEventsDbContext(options);

            _controller = new DevEventsController(_dbContext);
        }

        [Test]
        public void ValidateReturn_Success_DevEventsPost()
        {
            //arrange
            DevEvent devEvent = new DevEvent()
            {
                Title = "Test",
                Description = "Tests",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                IsDeleted = false,

            };
            //act
            var result = _controller.Post(devEvent) as ObjectResult;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);

            Assert.AreEqual("Test", devEvent.Title);
            Assert.AreEqual("Tests", devEvent.Description);
            Assert.IsInstanceOf<DateTime>(devEvent.StartDate);
            Assert.IsInstanceOf<DateTime>(devEvent.EndDate);
        }

        [Test]
        public void VerifySave_InMemory_DevEventsPost()
        {
            TearDown();

            var devEvent = new DevEvent()
            {
                Title = "Marcos",
                Description = "Tests",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                IsDeleted = false
            };
            _dbContext.DevEvents.Add(devEvent);
            _dbContext.SaveChanges();

            var savedDevEvent = _dbContext.DevEvents.FirstOrDefault(e => e.Title == "Marcos");
            Assert.IsNotNull(savedDevEvent);
            Assert.AreEqual("Marcos", savedDevEvent.Title);
            Assert.AreEqual("Tests", savedDevEvent.Description);
            Assert.AreEqual(devEvent.StartDate, savedDevEvent.StartDate);
            Assert.AreEqual(devEvent.EndDate, savedDevEvent.EndDate);
            Assert.AreEqual(false, savedDevEvent.IsDeleted);
        }


        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
}
