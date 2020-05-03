using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSCSoln.DataAccess;
using TSCSoln.Entities.Test;

namespace TSCSoln.Business.Test
{
    public class TestBL
    {
        private TestDal dal;
        public TestBL()
        {
           dal = new TestDal();
        }
        public GetTestResponse GetTestList(GetTestRequest request)
        {
            return dal.PRC_GET_TESTS(request);
        }
    }
}
