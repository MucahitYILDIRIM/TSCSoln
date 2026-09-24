using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.Entities.Test;
using TSCSoln.Tests.Fakes;
using TSCSoln.WebUI.Controllers;

namespace TSCSoln.Tests.WebUI
{
    [TestClass]
    public class TestControllerTests
    {
        [TestMethod]
        public void Index_ReturnsDefaultView_WithTestListAsModel()
        {
            List<GetTestDTO> list = new List<GetTestDTO>
            {
                new GetTestDTO { SQ_TEST_ID = 10, CH_TEST_NAME = "Ten" }
            };
            FakeTestBL bl = new FakeTestBL { Response = new GetTestResponse { TestList = list } };
            TestController controller = new TestController(() => bl);

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreSame(list, result.Model);
        }

        [TestMethod]
        public void Index_CallsBusinessLayerOnce_WithNonNullRequest()
        {
            FakeTestBL bl = new FakeTestBL();
            TestController controller = new TestController(() => bl);

            controller.Index();

            Assert.AreEqual(1, bl.CallCount);
            Assert.IsNotNull(bl.LastRequest);
        }

        [TestMethod]
        public void Index_EmptyTestList_ReturnsEmptyModel()
        {
            FakeTestBL bl = new FakeTestBL();
            TestController controller = new TestController(() => bl);

            ViewResult result = (ViewResult)controller.Index();

            var model = result.Model as List<GetTestDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count);
        }

        [TestMethod]
        public void BusinessLayer_IsCreatedLazilyOnEachIndexCall()
        {
            int created = 0;
            TestController controller = new TestController(() => { created++; return new FakeTestBL(); });

            Assert.AreEqual(0, created);

            controller.Index();
            controller.Index();

            Assert.AreEqual(2, created);
        }
    }
}
