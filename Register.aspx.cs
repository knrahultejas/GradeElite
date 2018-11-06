using System;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

public partial class Register : Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //It is used to update user details
    protected void UpdateUser_Click(object sender, EventArgs e)
    {
        string username = UserName.Text;
        string role = Session["role"].ToString();

        var password = "";
        var pass = Password.Text;
        if (pass != null && pass != "")
        {
            SHA1CryptoServiceProvider encrypt = new SHA1CryptoServiceProvider();
            byte[] encryptText = encrypt.ComputeHash(Encoding.Default.GetBytes(pass));
            foreach (byte tempData in encryptText)
            {
                password = password + tempData.ToString();
            }
        }

        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        try
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET password = @password where userName=@userName";
            cmd.Parameters.AddWithValue("@userName", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Response.Redirect("~/Error");
        }
        finally {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                Response.Redirect("~/Logout.aspx");
            }
        }      
    }
}