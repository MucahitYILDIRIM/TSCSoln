using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.Business.Test;
using TSCSoln.Entities.Test;
using TSCSoln.Tests.Fakes;

namespace TSCSoln.Tests.Business
{
    [TestClass]
    public class TestBLTests
    {
        [TestMethod]
        public void GetTestList_DelegatesToDalOnce_WithSameRequest()
        {
            FakeTestDal dal = new FakeTestDal();
            TestBL bl = new TestBL(dal);
            GetTestRequest request = new GetTestRequest();

            bl.GetTestList(request);

            Assert.AreEqual(1, dal.CallCount);
            Assert.AreSame(request, dal.LastRequest);
        }

        [TestMethod]
        public void GetTestList_ReturnsDalResponseUnchanged()
        {
            GetTestResponse expected = new GetTestResponse
            {
                TestList = new List<GetTestDTO>
                {
                    new GetTestDTO { SQ_TEST_ID = 1, CH_TEST_NAME = "One" },
                    new GetTestDTO { SQ_TEST_ID = 2, CH_TEST_NAME = "Two" }
                }
            };
            FakeTestDal dal = new FakeTestDal { Response = expected };
            TestBL bl = new TestBL(dal);

            GetTestResponse actual = bl.GetTestList(new GetTestRequest());

            Assert.AreSame(expected, actual);
            Assert.AreEqual(2, actual.TestList.Count);
        }

        [TestMethod]
        public void GetTestList_NullRequest_IsPassedThroughToDal()
        {
            FakeTestDal dal = new FakeTestDal();
            TestBL bl = new TestBL(dal);

            bl.GetTestList(null);

            Assert.AreEqual(1, dal.CallCount);
            Assert.IsNull(dal.LastRequest);
        }
    }
}
