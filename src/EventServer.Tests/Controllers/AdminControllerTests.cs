using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using EventServer.Core;
using EventServer.Core.Domain;
using EventServer.Core.Services;
using EventServer.Core.ViewModels;
using EventServer.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using MvcContrib.TestHelper;

namespace EventServer.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        [TestMethod]
        public void Sessions_Return_With_A_Day_property_from_Model()
        {
            //arrange
            var repository = MockRepository.GenerateMock<IRepository>();
            var userProfile = new UserProfile("test", "test", "test"){Id = 1};
            repository.Stub(x => x.Find<UserProfile>()).Return(
                new List<UserProfile> {userProfile}.AsQueryable()
                );

            repository.Stub(x => x.Find<Session>()).Return(new List<Session>
                                                                    {
                                                                        new Session
                                                                        {
                                                                            Status = SessionStatus.Accepted,
                                                                            Category = SessionCategory.Developers,
                                                                            User = userProfile,
                                                                            Title = "Test",
                                                                            Description = "test",
                                                                            Level = SessionLevel.Beginner,
                                                                            Day = 1
                                                                        }
                                                                    }.AsQueryable());
            var currentUserService = MockRepository.GenerateMock<ICurrentUserService>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Stub(x => x.IsUserInRole("test", "Admin")).Return(true);

            AdminController target = new AdminController(repository, currentUserService, membershipService);
            //act
            ActionResult result = target.Sessions();

            //assert
            var model = result.AssertViewRendered().ViewData.Model;
            Assert.IsInstanceOfType(model,typeof(AdminSessionsModel));
            Assert.IsNotNull(((AdminSessionsModel)model).Sessions);
            var session = ((AdminSessionsModel) model).Sessions.FirstOrDefault();
            Assert.IsNotNull(session);
            Assert.AreEqual(1,session.Day);
        }
    }
}