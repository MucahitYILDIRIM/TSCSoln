using System.Collections.Generic;
using TSCSoln.DataAccess;
using TSCSoln.Entities.Test;

namespace TSCSoln.Tests.Fakes
{
    internal class FakeTestDal : ITestDal
    {
        public GetTestResponse Response { get; set; } = new GetTestResponse { TestList = new List<GetTestDTO>() };
        public int CallCount { get; private set; }
        public GetTestRequest LastRequest { get; private set; }

        public GetTestResponse PRC_GET_TESTS(GetTestRequest request)
        {
            CallCount++;
            LastRequest = request;
            return Response;
        }
    }
}
