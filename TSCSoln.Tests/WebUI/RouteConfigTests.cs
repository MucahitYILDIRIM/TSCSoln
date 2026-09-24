using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.WebUI;

namespace TSCSoln.Tests.WebUI
{
    [TestClass]
    public class RouteConfigTests
    {
        [TestMethod]
        public void RegisterRoutes_AddsExactlyTwoRoutes()
        {
            RouteCollection routes = new RouteCollection();

            RouteConfig.RegisterRoutes(routes);

            Assert.AreEqual(2, routes.Count);
        }

        [TestMethod]
        public void RegisterRoutes_IgnoresAxdResourceRoute()
        {
            RouteCollection routes = new RouteCollection();

            RouteConfig.RegisterRoutes(routes);

            Route ignoreRoute = routes[0] as Route;
            Assert.IsNotNull(ignoreRoute);
            Assert.AreEqual("{resource}.axd/{*pathInfo}", ignoreRoute.Url);
            Assert.IsInstanceOfType(ignoreRoute.RouteHandler, typeof(StopRoutingHandler));
        }

        [TestMethod]
        public void RegisterRoutes_AddsDefaultRoute_WithExpectedUrlAndDefaults()
        {
            RouteCollection routes = new RouteCollection();

            RouteConfig.RegisterRoutes(routes);

            Route defaultRoute = routes["Default"] as Route;
            Assert.IsNotNull(defaultRoute);
            Assert.AreEqual("{controller}/{action}/{id}", defaultRoute.Url);
            Assert.AreEqual("Home", defaultRoute.Defaults["controller"]);
            Assert.AreEqual("Index", defaultRoute.Defaults["action"]);
            Assert.AreEqual(UrlParameter.Optional, defaultRoute.Defaults["id"]);
        }
    }
}
