using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSCSoln.DataAccess;
using TSCSoln.Entities.Test;

namespace TSCSoln.Business.Test
{
    public interface ITestBL
    {
        GetTestResponse GetTestList(GetTestRequest request);
    }

    public class TestBL : ITestBL
    {
        private ITestDal dal;
        public TestBL()
            : this(new TestDal())
        {
        }

        public TestBL(ITestDal dal)
        {
            this.dal = dal;
        }
        public GetTestResponse GetTestList(GetTestRequest request)
        {
            return dal.PRC_GET_TESTS(request);
        }
    }
}
