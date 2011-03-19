using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;
using EventServer.Infrastructure.Repositories;
using EventServer.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using EventServer.Core.Services;
using System.Collections.Generic;
using EventServer.Core.Domain;

namespace EventServer.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mockTwitterService = MockRepository.GenerateMock<ITwitterService>();
            mockTwitterService.Stub(x => x.GetaRecentTweets()).Return(new List<Tweet>(){new Tweet()});


            var controller = new HomeController(new InMemoryRepository(), null, mockTwitterService);

            // Act
            var model = controller.Index().As<ViewResult>().ViewData.Model;

            // Assert
            Assert.IsTrue(model is HomeIndexModel);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var mockRepo = new Rhino.Mocks.MockRepository();
            var mockTwitterService = mockRepo.StrictMock<ITwitterService>();
            
            var controller = new HomeController(new InMemoryRepository(), null, mockTwitterService);

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}