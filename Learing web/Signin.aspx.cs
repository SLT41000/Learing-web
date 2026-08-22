using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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

            if (!IsPostBack)
            {
                blindingMembertype();
            }
        }

        protected void RadioButtonMembertype_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void blindingMembertype()
        {
            var dt = DbHelper.ExecuteQuery("SELECT * FROM member");
            CheckBoxList1.DataSource = dt;
            CheckBoxList1.DataTextField = "type";
            CheckBoxList1.DataValueField = "id";
            CheckBoxList1.DataBind();
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (userbox.Value == "" || pbox.Value == "")
            {
                Label1.Text = "Username or password shouldn't be empty";
                Label1.ForeColor = System.Drawing.Color.White;
                return;
            }

            // Check if username already exists using parameterized query
            var existing = DbHelper.ExecuteQuery(
                "SELECT * FROM account WHERE uname=@uname",
                DbHelper.Param("@uname", userbox.Value)
            );

            if (existing.Rows.Count > 0)
            {
                Label1.Text = "This User already exists!";
                Label1.ForeColor = System.Drawing.Color.White;
                return;
            }

            // Must select at least one member type
            string selectedTypes = "";
            foreach (ListItem li in CheckBoxList1.Items)
            {
                if (li.Selected)
                    selectedTypes += li.Text + ' ';
            }

            if (selectedTypes.Trim() == "")
            {
                Label1.Text = "Member must select at least one subject";
                Label1.ForeColor = System.Drawing.Color.White;
                return;
            }

            // Insert new account using parameterized query
            DbHelper.ExecuteNonQuery(
                "INSERT INTO account (uname, password) VALUES (@uname, @password)",
                DbHelper.Param("@uname", userbox.Value),
                DbHelper.Param("@password", pbox.Value)
            );

            // Get the new account's ID using parameterized query
            var newAcct = DbHelper.ExecuteQuery(
                "SELECT * FROM account WHERE uname=@uname AND password=@password",
                DbHelper.Param("@uname", userbox.Value),
                DbHelper.Param("@password", pbox.Value)
            );

            string a = newAcct.Rows[0][0].ToString();

            // Insert account-member relationships using parameterized queries
            foreach (ListItem li in CheckBoxList1.Items)
            {
                if (li.Selected)
                {
                    DbHelper.ExecuteNonQuery(
                        "INSERT INTO accountmember (aid, mid) VALUES (@aid, @mid)",
                        DbHelper.Param("@aid", int.Parse(a)),
                        DbHelper.Param("@mid", int.Parse(li.Value))
                    );
                }
            }

            Label1.Text = "Sign Up successful! You can now login.";
            Label1.ForeColor = System.Drawing.Color.Green;
        }

        protected void BLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void alert(string message)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
        }
    }
}
