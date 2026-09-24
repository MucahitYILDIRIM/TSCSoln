using System.Linq;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.WebUI;

namespace TSCSoln.Tests.WebUI
{
    [TestClass]
    public class BundleConfigTests
    {
        [TestMethod]
        public void RegisterBundles_AddsExactlyFiveBundles()
        {
            BundleCollection bundles = new BundleCollection();

            BundleConfig.RegisterBundles(bundles);

            Assert.AreEqual(5, bundles.Count());
        }

        [TestMethod]
        public void RegisterBundles_AddsExpectedBundlePaths()
        {
            BundleCollection bundles = new BundleCollection();

            BundleConfig.RegisterBundles(bundles);

            var paths = bundles.Select(b => b.Path).ToList();
            CollectionAssert.AreEquivalent(
                new[]
                {
                    "~/bundles/jquery",
                    "~/bundles/jqueryval",
                    "~/bundles/modernizr",
                    "~/bundles/bootstrap",
                    "~/Content/css"
                },
                paths);
        }
    }
}
