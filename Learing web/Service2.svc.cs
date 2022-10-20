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
            
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            SqlCommand cmd = new SqlCommand("SELECT c.vid,c.ontime,v.vname FROM catalog AS c INNER JOIN video as v ON (c.vid=v.vid) WHERE c.aid=" +aid, con);


            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();





            List<Catalogdata> retVal = new List<Catalogdata>();

            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                Catalogdata v = new Catalogdata();
                v.vid = reader[0].ToString();
                v.ontime = reader[1].ToString();
                v.vname = reader[2].ToString();


                retVal.Add(v);

            }

            reader.Close();




            con.Close();
            return retVal.ToArray();



        }
    }
}
