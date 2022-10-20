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
using System.Web.Services;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Learing_web
{
    public partial class watch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uname"] == null)
            {
                Response.Redirect("~/default.aspx");
            }
            
        }
        
        public  void onchickvideo(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);
            

            con.Open();
            SqlCommand cmdSql = new SqlCommand("INSERT INTO catalog VALUES(@aid, @vid,@ontime) ", con);
            
            cmdSql.Parameters.AddWithValue("@aid", Session["aid"]);
            cmdSql.Parameters.AddWithValue("@vid", Request["vid"]);
            cmdSql.Parameters.AddWithValue("@ontime", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
            cmdSql.ExecuteNonQuery();
            
            con.Close();

            
             
        }
        public void onchickalreadyw(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);


            con.Open();
            SqlCommand cmdSql = new SqlCommand("UPDATE watchcheck SET alreadywatch = 1 WHERE aid = " + Session["aid"]+" AND vid="+ Request["vid"]+";", con);

           
            cmdSql.ExecuteNonQuery();

            con.Close();



        }

    }
}