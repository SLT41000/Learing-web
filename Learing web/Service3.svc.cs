using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Configuration;

namespace Learing_web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service3" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service3.svc or Service3.svc.cs at the Solution Explorer and start debugging.
    public class Service3 : IService3
    {
        public Getnaarl[] getalrdy(string aid)
        {
            var retVal = new List<Getnaarl>();
            using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString))
            {
                using (var cmd = new SqlCommand(
                    "SELECT v.vname FROM watchcheck AS w INNER JOIN video AS v ON (v.vid = w.vid) WHERE w.aid = @aid AND w.alreadywatch = 1",
                    conn))
                {
                    cmd.Parameters.AddWithValue("@aid", aid);
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Getnaarl a = new Getnaarl();
                            a.vname = rdr[0].ToString();
                            retVal.Add(a);
                        }
                    }
                }
            }
            return retVal.ToArray();
        }
    }
}
