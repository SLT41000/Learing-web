using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Learing_web
{
    public partial class Catalog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uname"] == null)
            {
                Response.Redirect("~/default.aspx");
            }
        }

        protected Catalogdata[] Submit_Click()
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            SqlCommand cmd = new SqlCommand("SELECT c.vid,c.ontime,v.vname FROM catalog AS c INNER JOIN video as v ON (c.vid=v.vid) WHERE c.aid="+ Session["aid"].ToString(), con);

            
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