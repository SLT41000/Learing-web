using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Learing_web
{
    public partial class home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            

                

                if (Session["uname"] != null)
                {
                    sslog.InnerHtml = (string)Session["uname"];
                    Certificate.Visible = true;
                    catalog.Visible = true;
                    logout.Visible = true;
                    signin.Visible = false;
                    login.Visible = false;

                    memberdata[] m = getmdata();
                    string[] mid = new string[m.Length];
                    for (int i = 0; i < m.Length; i++)
                    {
                        mid[i] = m[i].mid.ToString();

                    }

                    hidecata(mid);


                

            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        protected void BLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void BSingin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Signin.aspx");
        }
        
        protected void Blogout_Click(object sender, EventArgs e)
        {
            
                Session.Clear();
                logout.Visible = false;
                signin.Visible = true;
            Certificate.Visible=false;
                login.Visible = true;
                sslog.InnerHtml = "";
            hidden.InnerHtml = "";
            catalog.Visible = false;
            Response.Redirect("~/default.aspx");

        }

        protected void hidecata(string[] mid)
        {
            for(int i = 1; i < 7; i++)
            {
                for (int j = 0; j < mid.Length; j++)
                {
                    if (mid[j] == i.ToString())
                    {
                       break;
                    }
                    else if(j == mid.Length-1)
                    {
                        ListItem removeItem = classDrpDwn.Items.FindByValue(i.ToString());
                        classDrpDwn.Items.Remove(removeItem);
                    }
                }
                
            }
           
        }
        protected memberdata[] getmdata()
        {
            List<memberdata> retVal = new List<memberdata>();
            String a=Session["aid"].ToString();
            
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