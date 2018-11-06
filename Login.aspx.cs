using System;
using System.Web.UI;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public partial class Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Login button click event
    protected void LogIn(object sender, EventArgs e)
    {
        var username = UserName.Text;
        var password = "";
        var pass = Password.Text;
        if (pass != null && pass != "")
        {
            //Encryption - SHA1 is used to encrrypt password
            SHA1CryptoServiceProvider encrypt = new SHA1CryptoServiceProvider();
            byte[] encryptText = encrypt.ComputeHash(Encoding.Default.GetBytes(pass));
            foreach (byte tempData in encryptText)
            {
                password = password + tempData.ToString();
            }
        }

        var connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        MySqlConnection connection = new MySqlConnection(connectionString);
        try
        {
            connection.Open();
            MySqlCommand cmd = new MySqlCommand("select id, userName, role, isBlocked from user where userName=@user and password=@pass", connection);
            cmd.Parameters.AddWithValue("user", @username);
            cmd.Parameters.AddWithValue("pass", @password);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                var role = "student";
                var isBlocked = "0";
                foreach (DataRow row in dt.Rows)
                {
                    Session["userID"] = row.ItemArray[0].ToString();
                    role = row.ItemArray[2].ToString();
                    isBlocked = row.ItemArray[3].ToString();
                    Session["username"] = row.ItemArray[1].ToString();
                    Session["role"] = row.ItemArray[2].ToString();
                    Session["isBlocked"] = row.ItemArray[3].ToString();
                }
                connection.Close();
                if (isBlocked == "1")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "Authentication Error", "<script language='javascript'>alert('You donot have permission to loggin. Please contact Admin.')</script>");
                }
                else
                {
                    //Assigns roles to the user
                    if (role == "admin")
                    {
                        Response.Redirect("~/AdminView", false);
                    }
                    if (role == "instructor")
                    {
                        Response.Redirect("~/InstructorView", false);
                    }
                    if (role == "student")
                    {
                        Response.Redirect("~/StudentView", false);
                    }
                }
            }
            else
            {
                connection.Close();
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username and Password')</script>");
            }
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
}