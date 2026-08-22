using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.WebConfiguration;
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

        /// <summary>
        /// Records that the user started watching a video (check-in).
        /// </summary>
        public void onchickvideo(object sender, EventArgs e)
        {
            string vid = Request["vid"];
            if (string.IsNullOrEmpty(vid))
                return;

            // Use parameterized query
            DbHelper.ExecuteNonQuery(
                "INSERT INTO catalog (aid, vid, ontime) VALUES (@aid, @vid, @ontime)",
                DbHelper.Param("@aid", Session["aid"]),
                DbHelper.Param("@vid", vid),
                DbHelper.Param("@ontime", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"))
            );
        }

        /// <summary>
        /// Marks a video as already watched.
        /// </summary>
        public void onchickalreadyw(object sender, EventArgs e)
        {
            string vid = Request["vid"];
            if (string.IsNullOrEmpty(vid))
                return;

            // Use parameterized UPDATE query
            DbHelper.ExecuteNonQuery(
                "UPDATE watchcheck SET alreadywatch = 1 WHERE aid = @aid AND vid = @vid",
                DbHelper.Param("@aid", Session["aid"]),
                DbHelper.Param("@vid", vid)
            );
        }
    }
}
