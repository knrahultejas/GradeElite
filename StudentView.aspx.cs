using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class StudentView : Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && Session["role"].ToString() == "student")
        {
           
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }
}