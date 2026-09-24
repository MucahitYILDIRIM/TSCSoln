using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSCSoln.Business.Test;
using TSCSoln.Entities.Test;

namespace TSCSoln.WebUI.Controllers
{
    public class TestController : Controller
    {
        private ITestBL BL;
        private readonly Func<ITestBL> blFactory;

        public TestController()
            : this(() => new TestBL())
        {
        }

        public TestController(Func<ITestBL> blFactory)
        {
            this.blFactory = blFactory;
        }

        public ActionResult Index()
        {
            BL = blFactory();
            GetTestResponse response = BL.GetTestList(new GetTestRequest());
            return View(response.TestList);
        }
    }
}