using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSCSoln.Entities.Test
{
    public class GetTestRequest
    {

    }

    public class GetTestResponse
    {
        public List<GetTestDTO> TestList { get; set; }
    }
    public class GetTestDTO
    {
        public int SQ_TEST_ID { get; set; }
        public string CH_TEST_NAME { get; set; }
    }
}
