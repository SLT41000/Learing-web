using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Learing_web
{
    public partial class Signin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["uname"] != null)
            {
                Response.Redirect("~/default.aspx");
            }
        }

        protected void RadioButtonMembertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        protected void blindingMembertype()
        {
            SqlConnection SqlCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);
            string cmd = "SELECT *  FROM member";
            SqlDataAdapter adpt = new SqlDataAdapter(cmd, SqlCon);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            CheckBoxList1.DataSource = dt;
            CheckBoxList1.DataBind();
            CheckBoxList1.DataTextField = "type";
            CheckBoxList1.DataValueField = "id";
            CheckBoxList1.DataBind();
           
        }
        protected void alert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);

            SqlCommand cmd = new SqlCommand("select * from account where uname=@uname", con);
            cmd.Parameters.AddWithValue("@uname", userbox.Value);
            
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();

            if (userbox.Value=="" || pbox.Value=="")
            {
                Label1.Text = "Username or password should't empty";
                Label1.ForeColor = System.Drawing.Color.White;
            }

            else if (dt.Rows.Count > 0)
            {

                Label1.Text = "This User already exit!";
                Label1.ForeColor = System.Drawing.Color.White;
            }
            else
            { string strchklist = "";
                foreach (ListItem li in CheckBoxList1.Items)
                {
                    if (li.Selected)
                    {
                        strchklist += li.Text + ' ';
                    }
                    
                    
                }
                if(strchklist == "")
                {
                    Label1.Text = "Memmeb must select";
                    Label1.ForeColor = System.Drawing.Color.White;
                    return;
                }

                SqlConnection SqlCon = new SqlConnection(WebConfigurationManager.ConnectionStrings["strconn"].ConnectionString);
                SqlCommand cmdSql = new SqlCommand("INSERT INTO account VALUES(@uname, @password ) ", SqlCon);
                SqlCon.Open();
                cmdSql.Parameters.AddWithValue("@uname", userbox.Value);
                cmdSql.Parameters.AddWithValue("@password", pbox.Value);
                cmdSql.ExecuteNonQuery();
                var cmdSql1 = new SqlCommand("SELECT *  FROM account \nWHERE uname = '" + userbox.Value + "' AND password='" + pbox.Value + "'", SqlCon);
                SqlDataReader reader = cmdSql1.ExecuteReader();

                string a = "";
                while (reader.Read())
                {
                    a = reader[0].ToString();

                }


                reader.Close();




                foreach (ListItem li in CheckBoxList1.Items)
                {
                    if (li.Selected)
                    {
                        SqlCommand cmdSql2 = new SqlCommand("INSERT INTO accountmember VALUES(" + a + ", " + li.Value + " ) ", SqlCon);

                        cmdSql2.ExecuteNonQuery();
                    }
                }

                

                    
                    
                
                
                SqlCon.Close();


                Label1.Text = null ;

            }

            
            
        }

        protected void BLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }
    }
}