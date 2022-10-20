using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Drawing;
using System.Configuration;

namespace Learing_web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uname"] != null)
            {
                Response.Redirect("~/default.aspx");
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select * from account where uname=@uname and password=@password", con);
            cmd.Parameters.AddWithValue("@uname", userbox.Value);
            cmd.Parameters.AddWithValue("@password", pbox.Value);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();



            if (dt.Rows.Count > 0)
            {

                List<memberdata> retVal = new List<memberdata>();
                var cmdSql1 = new SqlCommand("SELECT *  FROM account \nWHERE uname = '" + userbox.Value + "' AND password='" + pbox.Value + "'", con);
                SqlDataReader reader = cmdSql1.ExecuteReader();
                string a = "";
                while (reader.Read())
                {
                    a = reader[0].ToString();

                }
                reader.Close();











                Session["aid"] = readaid();
                memberdata[] m = getmdata();
                string[] member = new string[m.Length] ;
                
                for (int i = 0; i < m.Length; i++)
                {
                     
                    Session["mid-"+ m[i].mid] = true;
                }
                Session["mid"] = member;

                Viddata[] vid = addwatchcheck(m);
                
                for (int i = 0; i < vid.Length; i++)
                {
                    SqlCommand cmdSql = new SqlCommand("INSERT INTO watchcheck (aid, vid, mid, alreadywatch)\r\nVALUES (N'" + Session["aid"] +"', N'"+ vid[i].vid + "', N'"+ vid[i].mid + "', N'"+0+"');", con);
                    cmdSql.ExecuteNonQuery();
                    
                }
                
                Session["uname"] = userbox.Value;
                

                Response.Redirect("default.aspx");
            }
            else
            {
                Label1.Text = "Your username and password is incorrect";
                Label1.ForeColor = System.Drawing.Color.White;
            }
            con.Close();
        }
        protected void BSingin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Signin.aspx");
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected string readaid()
        {  
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);
            con.Open();
            var cmdSql1 = new SqlCommand("SELECT *  FROM account \nWHERE uname = '" + userbox.Value + "' AND password='" + pbox.Value + "'", con);
            SqlDataReader reader = cmdSql1.ExecuteReader();
            string a = "";
            while (reader.Read())
            {
                a = reader[0].ToString();

            }
            reader.Close();
            con.Close();
            return a;

        }

        protected void pbox_TextChanged(object sender, EventArgs e)
        {
            

            
        }


        protected Viddata[] addwatchcheck(memberdata[] mid)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select * from watchcheck where aid=@aid ", con);
            cmd.Parameters.AddWithValue("@aid", Session["aid"]);
            
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();

            List<Viddata> retVal = new List<Viddata>();

            if (dt.Rows.Count == 0)
            {
                
                for (int i = 0; i < mid.Length; i++)
                {
                    var cmdSql1 = new SqlCommand("SELECT v.vid FROM video AS v WHERE v.mid=" + mid[i].mid, con);
                    SqlDataReader reader = cmdSql1.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        Viddata a = new Viddata();
                        a.vid= reader[0].ToString();
                        a.mid = mid[i].mid;
                        retVal.Add(a);
                    }
                    reader.Close();


                }



               
                con.Close();






               





            }
            
            return retVal.ToArray();

        }


        public memberdata[] getmdata()
        {
            List<memberdata> retVal = new List<memberdata>();

            string a = Session["aid"].ToString();
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            var cmdSql2 = new SqlCommand("SELECT mid  FROM accountmember  where accountmember.aid=" + a, con);

            con.Open();

            SqlDataReader reader = cmdSql2.ExecuteReader();
            
            while (reader.Read())
            {
                memberdata m = new memberdata();
                m.mid = reader[0].ToString();
                
                retVal.Add(m);

            }
            
            
            reader.Close();
            con.Close();
           
               
            return retVal.ToArray();







        }


    }
}