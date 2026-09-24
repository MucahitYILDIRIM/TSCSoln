using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.WebUI;

namespace TSCSoln.Tests.WebUI
{
    [TestClass]
    public class FilterConfigTests
    {
        [TestMethod]
        public void RegisterGlobalFilters_AddsExactlyOneFilter()
        {
            GlobalFilterCollection filters = new GlobalFilterCollection();

            FilterConfig.RegisterGlobalFilters(filters);

            Assert.AreEqual(1, filters.Count());
        }

        [TestMethod]
        public void RegisterGlobalFilters_AddsHandleErrorAttribute()
        {
            GlobalFilterCollection filters = new GlobalFilterCollection();

            FilterConfig.RegisterGlobalFilters(filters);

            Assert.IsInstanceOfType(filters.Single().Instance, typeof(HandleErrorAttribute));
        }
    }
}
