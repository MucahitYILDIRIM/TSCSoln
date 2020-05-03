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
        private TestBL BL;
        public ActionResult Index()
        {
            BL = new TestBL();
            GetTestResponse response = BL.GetTestList(new GetTestRequest());
            return View(response.TestList);
        }
    }
}