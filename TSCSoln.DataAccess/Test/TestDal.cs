using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSCSoln.Entities.Test;
using System.Data.SqlClient;
using System.Configuration;

namespace TSCSoln.DataAccess
{
    public class TestDal
    {
        private SqlConnection con;
        public TestDal()
        {
            con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }
        
        public GetTestResponse PRC_GET_TESTS(GetTestRequest request)
        {
            GetTestResponse response = new GetTestResponse();
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("PRC_GET_TESTS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter DataAdapter = new SqlDataAdapter(cmd);
                DataAdapter.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            response.TestList = Utility.DataTableToList<GetTestDTO>(dt);
            return response;
        }


    }
}
