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
            List<Getnaarl> retVal = new List<Getnaarl>();
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);
            SqlCommand cmd = new SqlCommand(
              "SELECT v.vname,w.aid FROM watchcheck AS w INNER JOIN video as v ON (v.vid=w.vid AND w.aid=" + aid + " AND w.alreadywatch=1);"


            , conn);
            conn.Open();


            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Getnaarl a = new Getnaarl();
                a.vname = rdr[0].ToString();




                retVal.Add(a);

            }

            rdr.Close();
            conn.Close();

            return retVal.ToArray();
        }
    }
}
