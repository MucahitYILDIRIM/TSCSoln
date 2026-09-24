using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TSCSoln.DataAccess;
using TSCSoln.Entities.Test;

namespace TSCSoln.Tests.DataAccess
{
    [TestClass]
    public class TestDalTests
    {
        [TestMethod]
        public void Constructor_MalformedConnectionString_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new TestDal("this is not a connection string"));
        }

        // An empty connection string makes SqlConnection.Open() throw
        // InvalidOperationException immediately (no network access). PRC_GET_TESTS
        // swallows the exception and returns an empty list.
        [TestMethod]
        public void PRC_GET_TESTS_WhenConnectionFails_ReturnsEmptyList()
        {
            TestDal dal = new TestDal(string.Empty);

            GetTestResponse response = dal.PRC_GET_TESTS(new GetTestRequest());

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.TestList);
            Assert.AreEqual(0, response.TestList.Count);
        }

        [TestMethod]
        public void PRC_GET_TESTS_NullRequest_WhenConnectionFails_ReturnsEmptyList()
        {
            TestDal dal = new TestDal(string.Empty);

            GetTestResponse response = dal.PRC_GET_TESTS(null);

            Assert.IsNotNull(response.TestList);
            Assert.AreEqual(0, response.TestList.Count);
        }
    }
}
