using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.WebConfiguration;
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
            Certificate.Visible = false;
            login.Visible = true;
            sslog.InnerHtml = "";
            hidden.InnerHtml = "";
            catalog.Visible = false;
            Response.Redirect("~/default.aspx");
        }

        protected void hidecata(string[] mid)
        {
            // Remove subjects the user is NOT enrolled in from the dropdown
            for (int i = 1; i < 7; i++)
            {
                bool enrolled = false;
                for (int j = 0; j < mid.Length; j++)
                {
                    if (mid[j] == i.ToString())
                    {
                        enrolled = true;
                        break;
                    }
                }

                if (!enrolled)
                {
                    ListItem removeItem = classDrpDwn.Items.FindByValue(i.ToString());
                    if (removeItem != null)
                        classDrpDwn.Items.Remove(removeItem);
                }
            }
        }

        protected memberdata[] getmdata()
        {
            var retVal = new List<memberdata>();
            string a = Session["aid"].ToString();

            DbHelper.ReadQuery(
                "SELECT mid FROM accountmember WHERE accountmember.aid=@aid",
                reader =>
                {
                    memberdata m = new memberdata();
                    m.mid = reader[0].ToString();
                    retVal.Add(m);
                },
                DbHelper.Param("@aid", a)
            );

            return retVal.ToArray();
        }
    }
}
