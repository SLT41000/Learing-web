using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Configuration;

namespace Learing_web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service2.svc or Service2.svc.cs at the Solution Explorer and start debugging.
    public class Service2 : IService2
    {
        public Catalogdata[] Submit_Click(string aid)
        {
            var retVal = new List<Catalogdata>();
            using (var con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString))
            {
                using (var cmd = new SqlCommand(
                    "SELECT c.vid, c.ontime, v.vname FROM catalog AS c INNER JOIN video AS v ON (c.vid = v.vid) WHERE c.aid = @aid",
                    con))
                {
                    cmd.Parameters.AddWithValue("@aid", aid);
                    var sda = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    sda.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Catalogdata v = new Catalogdata();
                        v.vid = row[0].ToString();
                        v.ontime = row[1].ToString();
                        v.vname = row[2].ToString();
                        retVal.Add(v);
                    }
                }
            }
            return retVal.ToArray();
        }
    }
}
