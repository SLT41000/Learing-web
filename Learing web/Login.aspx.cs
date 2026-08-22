using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.WebConfiguration;
using System.Configuration;
using System.Drawing;

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
            // Use parameterized query to prevent SQL injection
            var dt = DbHelper.ExecuteQuery(
                "SELECT * FROM account WHERE uname=@uname AND password=@password",
                DbHelper.Param("@uname", userbox.Value),
                DbHelper.Param("@password", pbox.Value)
            );

            if (dt.Rows.Count > 0)
            {
                // Get account ID from the already-loaded DataTable
                string a = dt.Rows[0][0].ToString();

                Session["aid"] = a;

                memberdata[] m = getmdata();
                string[] member = new string[m.Length];

                for (int i = 0; i < m.Length; i++)
                {
                    member[i] = m[i].mid.ToString();
                    Session["mid-" + m[i].mid] = true;
                }
                Session["mid"] = member;

                Viddata[] vid = addwatchcheck(m);

                // Use parameterized INSERT for watchcheck
                foreach (Viddata v in vid)
                {
                    DbHelper.ExecuteNonQuery(
                        "INSERT INTO watchcheck (aid, vid, mid, alreadywatch) VALUES (@aid, @vid, @mid, @alreadywatch)",
                        DbHelper.Param("@aid", Session["aid"]),
                        DbHelper.Param("@vid", v.vid),
                        DbHelper.Param("@mid", v.mid),
                        DbHelper.Param("@alreadywatch", 0)
                    );
                }

                Session["uname"] = userbox.Value;
                Response.Redirect("default.aspx");
            }
            else
            {
                Label1.Text = "Your username and password is incorrect";
                Label1.ForeColor = System.Drawing.Color.White;
            }
        }

        protected void BSingin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Signin.aspx");
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void pbox_TextChanged(object sender, EventArgs e)
        {
        }

        protected Viddata[] addwatchcheck(memberdata[] mid)
        {
            // Check if watchcheck entries already exist for this account
            var existing = DbHelper.ExecuteQuery(
                "SELECT vid, mid FROM watchcheck WHERE aid=@aid",
                DbHelper.Param("@aid", Session["aid"])
            );

            if (existing.Rows.Count > 0)
            {
                // Return empty array if entries already exist
                return new Viddata[0];
            }

            // Get all videos for the member's subjects
            var retVal = new List<Viddata>();

            foreach (memberdata m in mid)
            {
                var videos = DbHelper.ExecuteQuery(
                    "SELECT v.vid FROM video AS v WHERE v.mid=@mid",
                    DbHelper.Param("@mid", m.mid)
                );

                foreach (DataRow row in videos.Rows)
                {
                    retVal.Add(new Viddata
                    {
                        vid = row[0].ToString(),
                        mid = m.mid
                    });
                }
            }

            return retVal.ToArray();
        }

        public memberdata[] getmdata()
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
