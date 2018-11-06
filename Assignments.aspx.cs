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

public partial class Assignments : System.Web.UI.Page
{
    //Connection string, to get the connection string from config file
    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Checking the session variables of a user name is present or not
        if (Session["username"] != null && Session["role"].ToString() == "student")
        {
            //Loading the DataGridview. Populating it
            LoadGridData();
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    //populate gridview
    private void LoadGridData()
    {
        //open sql connection
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            //quering frm database
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM assignments";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            assignmentsGridView.DataSource = ds.Tables[0].DefaultView;
            assignmentsGridView.DataBind();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    //method used to download file
    protected void DownloadFile(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes;
        string fileName, contentType;
        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "select assignmentName, Data, ContentType from assignments where assignmentId=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Connection = con;
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    //convert data to byte form
                    sdr.Read();
                    bytes = (byte[])sdr["Data"];
                    contentType = sdr["ContentType"].ToString();
                    fileName = sdr["assignmentName"].ToString();
                }
                con.Close();
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        if (ContentType != null)
        {
            //append file type to downloaded file
            switch (ContentType.ToLower())
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    fileName = fileName + ".docx";
                    break;
                case ".html":
                case "text/html":
                    fileName = fileName + ".html";
                    break;
                case ".txt":
                case "text/plain":
                    fileName = fileName + ".txt";
                    break;
                case ".doc":
                case ".rtf":
                case "application/msword":
                    fileName = fileName + ".doc";
                    break;

                case ".xls":
                    fileName = fileName + ".xls";
                    break;
            }
        }
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
}