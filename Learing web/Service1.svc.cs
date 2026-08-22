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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public screenData[] getscreendata()
        {
            var retVal = new List<screenData>();
            using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString))
            {
                using (var cmd = new SqlCommand("SELECT * FROM video v", conn))
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            screenData v = new screenData();
                            v.vid = rdr[0].ToString();
                            v.mid = rdr[1].ToString();
                            v.vname = rdr[2].ToString();
                            v.description = rdr[3].ToString();
                            v.vlink = rdr[4].ToString();
                            retVal.Add(v);
                        }
                    }
                }
            }
            return retVal.ToArray();
        }
    }
}
