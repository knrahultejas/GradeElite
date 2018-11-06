using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ionic.Zip;
using System.Text;
using System.Runtime.InteropServices;

public partial class Grader : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string assignID = "";
    string userID = "";
    string assignmentName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && (Session["role"].ToString() == "admin" || Session["role"].ToString() == "instructor"))
        {
            //Getting data from query
            assignID = Request.QueryString["assignmentId"];
            userID = Session["userID"].ToString();
            assignmentName = Request.QueryString["assignmentName"];
            if (!Page.IsPostBack)
            {
                LoadStudentGridView();
                LoadTestCaseGridView();
            }
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    //Load Student gridview. Get data  from database and populate gridview
    private void LoadStudentGridView()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select u.userName, a.assignmentName, u.id, s.assignmentId, s.points, s.comments, s.zipFileName, s.filePath From assignments a join studentassignments s on a.assignmentId = s.assignmentId join user u on u.Id = s.studentId where s.assignmentId = @assignmentId";
            cmd.Parameters.AddWithValue("@assignmentId", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            if (ds.Tables.Count == 0)
            {
                noSubmissionsLBL.Text = "There are no submissions yet.";
            }
            else
            {
                noSubmissionsLBL.Text = "";
                studentGridView.DataSource = ds.Tables[0].DefaultView;
                studentGridView.DataBind();
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

    //This is responsible for loading the test data and populating the gridview
    private void LoadTestCaseGridView()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select testCaseId,sampleInput,sampleOutput from testcases where assignmentId = @assignmentId";
            cmd.Parameters.AddWithValue("@assignmentId", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                noTestCasesLBL.Text = "There are no test cases for this assignment.";
                tcGridViewDiv.Visible = false;
            }
            else
            {
                tcGridViewDiv.Visible = true;
                noTestCasesLBL.Text = "";
                testCaseGV.DataSource = ds.Tables[0].DefaultView;
                testCaseGV.DataBind();
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

    //Gets values from page and sets to update division
    protected void updateGrade(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int rowIndex = gvr.RowIndex; // Get the current row
        string assignmentID = studentGridView.Rows[rowIndex].Cells[0].Text;
        string userId = studentGridView.Rows[rowIndex].Cells[1].Text;
        string userName = studentGridView.Rows[rowIndex].Cells[2].Text;
        string assignmentName = studentGridView.Rows[rowIndex].Cells[3].Text;
        string points = studentGridView.Rows[rowIndex].Cells[4].Text;

        nameLBL.Text = userName.Split('@')[0];
        assLBL.Text = assignmentName;
        pointsTB.Text = points;
        assIdLBL.Text = assignmentID;
        userIdLBL.Text = userId;

        updateGradePH.Visible = true;
    }

    //When you want to update the grade, we use this button event to save grades in DB
    protected void update_Click(object sender, EventArgs e)
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "update studentassignments SET points =" + pointsTB.Text + " where assignmentId=" + assIdLBL.Text + " and studentId=" + userIdLBL.Text;
            cmd.ExecuteNonQuery();
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
            updateGradePH.Visible = false;
        }
        Response.Redirect("~/InstructorView");
    }

    //Invoked when we click on add test case button. This is used to save test cases to the db.
    protected void addTestCase_Click(object sender, EventArgs e)
    {
        string si = null;
        string so = null;
        if (sampleInputFileUpload.HasFile)
        {
            StreamReader readerSi = new StreamReader(sampleInputFileUpload.PostedFile.InputStream);
            si = readerSi.ReadToEnd();
        }
        if (sampleOutputFileUpload.HasFile)
        {
            StreamReader readerSo = new StreamReader(sampleOutputFileUpload.PostedFile.InputStream);
            so = readerSo.ReadToEnd();
        }
        if (si == null)
        {
            si = "";
        }
        if (so == null)
        {
            so = "";
        }
        MySqlConnection conn = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        conn.Open();
        ////
        try
        {
            cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO testcases(assignmentId,sampleInput,sampleOutput) VALUES(@assignId,@si,@so)";
            cmd.Parameters.AddWithValue("@assignId", assignID);
            cmd.Parameters.AddWithValue("@si", si);
            cmd.Parameters.AddWithValue("@so", so);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            LoadTestCaseGridView();
        }
    }

    //Used to check the plagarism
    protected void plagarismButton_Click(object sender, EventArgs e)
    {
        System.Data.DataTable dt = null;
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select a.assignmentName, a.assignmentId, s.studentId, u.userName, s.filePath From studentassignments s join assignments a on a.assignmentId = s.assignmentId join user u on s.studentId = u.id where s.assignmentId = @assignID";
            cmd.Parameters.AddWithValue("@assignID", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            dt = ds.Tables[0];
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
        //Create excel sheet
        Microsoft.Office.Interop.Excel.Application application = null;
        Microsoft.Office.Interop.Excel._Workbook workbook = null;
        Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
        Microsoft.Office.Interop.Excel.Range range = null;
        object misvalue = System.Reflection.Missing.Value;
        try
        {
            //Start Excel and get Application object.
            application = new Microsoft.Office.Interop.Excel.Application();
            application.Visible = true;

            //Get a new workbook.
            workbook = (Microsoft.Office.Interop.Excel._Workbook)(application.Workbooks.Add(""));
            worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;

            if (dt.Rows.Count > 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    worksheet.Cells[1, i + 2] = dt.Rows[i][3].ToString().Split('@')[0]; 
                    worksheet.Cells[i + 2, 1] = dt.Rows[i][3].ToString().Split('@')[0];
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (i != j)
                        {
                            string assignmetnId = dt.Rows[i][1].ToString();
                            string iStudentId = dt.Rows[i][2].ToString();
                            string iUserName = dt.Rows[i][3].ToString();
                            string iFilePath = dt.Rows[i][4].ToString();
                            string jStudentId = dt.Rows[j][2].ToString();
                            string jUserName = dt.Rows[j][3].ToString();
                            string jFilePath = dt.Rows[j][4].ToString();
                            string file1 = GetFileContent(iFilePath, iStudentId, assignmetnId);
                            string file2 = GetFileContent(jFilePath, jStudentId, assignmetnId); ;
                            Plagarism plagarism = new Plagarism();

                            //This is used to calculate plagarism percentage.
                            double percentage = plagarism.CalculateSimilarity(file1, file2);
                            worksheet.Cells[i + 2, j + 2] = percentage;
                            if (percentage == 1)
                            {
                                worksheet.Cells[i + 2, j + 2] = 100;
                                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[i + 2, j + 2];
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                            if (percentage > 90)
                            {
                                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[i + 2, j + 2];
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                            }
                            if (percentage == 100)
                            {
                                Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[i + 2, j + 2];
                                rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            }
                        }
                    }
                }
                string destination = assignmentName + "_Plagarism.xlsx";
                //Saving the excel sheet
                workbook.SaveAs(destination, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close();
                application.Quit();
            }
        }
        catch
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (range != null) Marshal.ReleaseComObject(range);
            if (worksheet != null) Marshal.ReleaseComObject(worksheet);
            if (workbook != null) Marshal.ReleaseComObject(workbook);
            if (application != null) Marshal.ReleaseComObject(application);
        }
    }

    //Gets the file content and returns the string 
    protected string GetFileContent(string path, string userId, string assignmentId)
    {
        string data = "";
        using (ZipArchive zip = System.IO.Compression.ZipFile.Open(path, ZipArchiveMode.Read))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                if (entry.Name.Contains(".java"))
                {
                    using (var stream = entry.Open())
                    using (var reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }
            }
        }
        return data;
    }

    protected void closeUpdate_Click(object sender, EventArgs e)
    {
        updateGradePH.Visible = false;
    }

    //To remove test cases from DB
    protected void deleteTestCase_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int rowIndex = gvr.RowIndex; // Get the current row
        string testCaseId = testCaseGV.Rows[rowIndex].Cells[0].Text;
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "delete from testcases where testCaseId = @testCaseId";
            cmd.Parameters.AddWithValue("@testCaseId", testCaseId);
            cmd.ExecuteNonQuery();
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
            LoadTestCaseGridView();
        }
    }
}