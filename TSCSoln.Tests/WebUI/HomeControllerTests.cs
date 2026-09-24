using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.WebUI.Controllers;

namespace TSCSoln.Tests.WebUI
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_ReturnsDefaultView()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void About_ReturnsDefaultView_WithDescriptionMessage()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.About() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreEqual("Your application description page.", result.ViewData["Message"]);
        }

        [TestMethod]
        public void Contact_ReturnsDefaultView_WithContactMessage()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Contact() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreEqual("Your contact page.", result.ViewData["Message"]);
        }
    }
}
