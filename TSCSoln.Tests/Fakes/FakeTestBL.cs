using System.Collections.Generic;
using TSCSoln.Business.Test;
using TSCSoln.Entities.Test;

namespace TSCSoln.Tests.Fakes
{
    internal class FakeTestBL : ITestBL
    {
        public GetTestResponse Response { get; set; } = new GetTestResponse { TestList = new List<GetTestDTO>() };
        public int CallCount { get; private set; }
        public GetTestRequest LastRequest { get; private set; }

        public GetTestResponse GetTestList(GetTestRequest request)
        {
            CallCount++;
            LastRequest = request;
            return Response;
        }
    }
}
