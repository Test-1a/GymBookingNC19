using GymBookingNC19.Core;
using GymBookingNC19.Core.Models;
using GymBookingNC19.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GymBookingNC19.Controllers
{
    [TestClass]
    public class GymClassesControllerTests
    {

        [TestInitialize]
        public void SetUp()
        {
            var repository = new Mock<IGymClassesRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.GymClasses).Returns(repository.Object);

            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var mockUsermanager = new Mock<UserManager<ApplicationUser>>
                (userStore.Object, null, null, null, null, null, null, null, null);

            var controller = new GymClassesController(mockUsermanager.Object, mockUoW.Object);

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
