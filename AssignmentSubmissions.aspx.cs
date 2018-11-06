using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AssignmentSubmissions : System.Web.UI.Page
{

    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string assignID = "";
    string assignmentName = "";
    string userID = "";
    string userName = "";
    string sampleInput = "";
    string sampleOutput = "";
    string path = "";
    string submissionPath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && Session["role"].ToString() == "student")
        {
            //get values from session
            assignID = Request.QueryString["assignmentId"];
            assignmentName = Request.QueryString["assignmentName"];
            userID = Session["userID"].ToString();
            userName = Session["username"].ToString();
            LoadStudentGridView();
            if (!IsPostBack)
            {
                checkAssignmentSubmission();
            }
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    //checking if there are any assignments or not
    protected void checkAssignmentSubmission()
    {
        var result = 0;
        string points = "";

        MySqlConnection connection1 = new MySqlConnection(connectionString);
        connection1.Open();
        try
        {
            //quering from db
            MySqlCommand mysqlcmd = connection1.CreateCommand();
            mysqlcmd.CommandText = "SELECT count(*) FROM studentassignments WHERE assignmentId=@assignmentId and studentId=@userId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignID);
            mysqlcmd.Parameters.AddWithValue("userId", userID);
            result = Convert.ToInt32(mysqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection1.State == ConnectionState.Open)
            {
                connection1.Close();
            }
        }
        //if there are assignments, then show submissions and populate gridview
        if (result > 0)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = "SELECT points,filePath FROM studentassignments where assignmentId=@assignmentId and studentId=@userId";
                comm.Parameters.AddWithValue("@assignmentId", assignID);
                comm.Parameters.AddWithValue("@userId", userID);
                MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                DataTable dt = ds.Tables[0];
                points = dt.Rows[0][0].ToString();
                submissionPath = dt.Rows[0][1].ToString();
                Session["submissionPath"] = submissionPath;
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
        if (points != null && submissionPath != "")
        {
            resultPH.Visible = true;
            if (points != "")
            {
                gradeLBL.Text = "Marks: " + points;
            }
            uploadAssignment.Text = "Resubmit Assignment";
        }
    }

    //get data and populate student gridview
    protected void LoadStudentGridView()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select a.assignmentName, a.assignmentId, s.points, a.dueDate From assignments a join studentassignments s on a.assignmentId = s.assignmentId where s.studentId = @userId and s.assignmentId = @assignmentId";
            // cmd.CommandText = "SELECT assignmentId,assignmentName,dueDate,points FROM assignments where assignmentId=@assignmentId";
            cmd.Parameters.AddWithValue("@assignmentId", assignID);
            cmd.Parameters.AddWithValue("@userId", userID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            DataTable dt = ds.Tables[0];
            studentGridView.DataSource = ds.Tables[0].DefaultView;
            studentGridView.DataBind();
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

    //download assignment file from database
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

    //main method to grade uploaded assignment
    protected void GradeAssignment(string path, string userId, string assignmentID, string userName, string assignmentName)
    {
        ///////////////////
        string sampleInput = null, sampleOutput = null;
        DataTable dt = null;
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand comm = connection.CreateCommand();
            comm.CommandText = "SELECT assignmentId,sampleInput,sampleOutput FROM testcases where assignmentId=@assignmentId";
            comm.Parameters.AddWithValue("@assignmentId", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(comm);
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
        if (dt.Rows.Count == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Submission Failed. Please retry.')</script>");
            return;
        }
        double sum = 0;
        int numberOfTestcases = 0;
        Literal resultHTML = new Literal();
        string htmlSample = "";

        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string output = "";
            sampleInput = dt.Rows[i][1].ToString();
            sampleOutput = dt.Rows[i][2].ToString();
            string exeOutput = "";
            if (sampleInput != null && sampleInput != "")
            {
                //get executed output and store in variable
                exeOutput = CheckAssignmentZip(userId, assignmentID, assignmentName, ToStream(sampleInput));

                if (exeOutput == sampleOutput)
                {
                    output = "100%";
                    sum = sum + 100;
                    numberOfTestcases++;
                }
                else
                {
                    output = "test case failed..!!";
                    sum = sum + 0;
                    numberOfTestcases++;
                }
            }
            
            //Prepating HTML to show executed output and actual output
            string resultInnerHTML = "<hr /><div class='container'>" +
                           "<div class='container' style='width: 100%;'>" +
                           "<div class='panel panel-default'>" +
                           "<div class='panel-heading' style='align-content: center; font-weight: bold'>Test Case "+ (i+1)+"</div>" +
                           "<div class='panel-body'>" +
                           "<h4>" + output + "</h4>" +
                           "<asp:LinkButton runat='server' title='Sample Input' data-toggle='popover' data-trigger='hover' data-content='" + sampleInput + "'>Sample Input</asp:LinkButton><br />" +
                           "<asp:LinkButton runat='server' title='Sample Output' data-toggle='popover' data-trigger='hover' data-content='" + sampleOutput + "'>Sample Output</asp:LinkButton><br />" +
                           "<asp:LinkButton runat='server' title='Executed Output' data-toggle='popover' data-trigger='hover' data-content='" + exeOutput + "'>Executed Output</asp:LinkButton><br />" +
                           "</div></div></div></div>";
            htmlSample = htmlSample + resultInnerHTML;
        }
        resultHTML.Text = htmlSample;
        testcasePHMain.Controls.Add(resultHTML);

        MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();
        string maxpoints = "";
        try
        {
            MySqlCommand mysqlcmd = conn.CreateCommand();
            mysqlcmd.CommandText = "SELECT points FROM assignments WHERE assignmentId=@assignmentId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignmentID);
            maxpoints = mysqlcmd.ExecuteScalar().ToString();
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
        }
        double percentage = (((sum / numberOfTestcases) / 100) * Convert.ToInt32(maxpoints));
        percentLBL.Text = percentage.ToString() + " out of " + maxpoints;
        outputPH.Visible = true;
        /////////////////////////////
        var result = 0;
        MySqlConnection conn1 = new MySqlConnection(connectionString);
        conn1.Open();
        try
        {
            MySqlCommand mysqlcmd = conn1.CreateCommand();
            mysqlcmd.CommandText = "SELECT count(*) FROM studentassignments WHERE assignmentId=@assignmentId AND studentId=@userId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignmentID);
            mysqlcmd.Parameters.AddWithValue("userId", userId);
            result = Convert.ToInt32(mysqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (conn1.State == ConnectionState.Open)
            {
                conn1.Close();
            }
        }

        if (result <= 0)
        {
            MySqlConnection conn2 = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            conn2.Open();
            try
            {
                cmd = conn2.CreateCommand();
                cmd.CommandText = "INSERT INTO studentassignments(assignmentId,studentId,points,comments,zipFileName,filePath) VALUES(@assignmentId,@studentId,@points,@comments,@zipFileName,@filePath)";
                cmd.Parameters.AddWithValue("@assignmentID", assignID);
                cmd.Parameters.AddWithValue("@studentId", userID);
                cmd.Parameters.AddWithValue("@points", (int)percentage);
                cmd.Parameters.AddWithValue("@comments", "");
                cmd.Parameters.AddWithValue("@zipFileName", userID + "_" + assignID + ".zip");
                cmd.Parameters.AddWithValue("@filePath", path);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Submission Failed. Please retry.')</script>");
                Response.Redirect("~/Error");
            }
            finally
            {
                if (conn2.State == ConnectionState.Open)
                {
                    conn2.Close();
                }
            }
        }
        if (result > 0)
        {
            MySqlConnection conn3 = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            conn3.Open();
            try
            {
                cmd = conn3.CreateCommand();
                cmd.CommandText = "UPDATE studentassignments SET points=@points,comments=@comments,zipFileName=@zipFileName,filePath=@filePath where assignmentId=@assignmentID and studentId=@studentId";
                cmd.Parameters.AddWithValue("@assignmentID", assignID);
                cmd.Parameters.AddWithValue("@studentId", userID);
                cmd.Parameters.AddWithValue("@points", (int)percentage);
                cmd.Parameters.AddWithValue("@comments", "");
                cmd.Parameters.AddWithValue("@zipFileName", userID + "_" + assignID + ".zip");
                cmd.Parameters.AddWithValue("@filePath", path);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Submission Failed. Please retry.')</script>");
                Response.Redirect("~/Error");
            }
            finally
            {
                if (conn3.State == ConnectionState.Open)
                {
                    conn3.Close();

                }
                //reload gridview 
                LoadStudentGridView();
                checkAssignmentSubmission();
            }
        }
    }

    //this method is used to check the uploaded zip file and unzip it and return executed output
    protected string CheckAssignmentZip(string userID, string assignID, string assignmentName, Stream inputStream)
    {
        string path = Server.MapPath("~/Submissions/" + userID + "_" + assignID + ".zip");
        string fileName = userID + "_" + assignID + ".zip";
        string finalOutput = "";
        try
        {
        if (File.Exists(path))
        {
//unzipping the file
            ZipFile.ExtractToDirectory(path, path.Replace(".zip", ""));
            string unZipPath = Server.MapPath("~/Submissions/" + userID + "_" + assignID);
            string subFolderName = assignmentName;
            DirectoryInfo folder = new DirectoryInfo(unZipPath);
            DirectoryInfo[] subfolders = folder.GetDirectories();
            if (subfolders.Length > 0)
            {
                subFolderName = subfolders[0].Name;
            }

            string destinationDirectory = path.Replace(".zip", "") + @"\" + subFolderName + @"\dist";

            if (Directory.Exists(destinationDirectory))
            {
                string jarName = assignmentName;
                CopyStream(inputStream, destinationDirectory + @"\input.txt");
                DirectoryInfo folder1 = new DirectoryInfo(destinationDirectory);
                FileInfo[] files = folder1.GetFiles();

                foreach (var item in files)
                {
                    if (item.Name.Contains(".jar"))
                    {
                        jarName = item.Name;
                    }
                }

                //Checking if JAR file is there and executing JAR file
                string output = ExecuteJAR(path.Replace(".zip", "") + @"\" + subFolderName + @"\dist", jarName);
                output = output.Remove(output.TrimEnd().LastIndexOf(Environment.NewLine));
                finalOutput = DeleteLines(output, 8).Trim();
                finalOutput.ToString().Trim();
                if (Directory.Exists(path.Replace(".zip", "")))
                {
                    DeleteFilesAndFoldersRecursively(path.Replace(".zip", ""));
                }
            }
            else
            {
                return "Compilation Error. Please submit after building the project.";
            }
        }
        }
        catch (Exception)
        {
            Response.Redirect("~/Error");
        }
        return finalOutput;
    }

    //
    public void CopyStream(Stream stream, string destPath)
    {
        using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
        {
            stream.CopyTo(fileStream);
        }
    }

    public string DeleteLines(string s, int linesToRemove)
    {
        return s.Split(Environment.NewLine.ToCharArray(),
                       linesToRemove + 1
            ).Skip(linesToRemove)
            .FirstOrDefault();
    }

    //Execute JAR using command prompt. Invoking a Process
    protected string ExecuteJAR(String pathOfFile, String nameOfFile)
    {
        String output = "";
        String command = "java -jar " + nameOfFile;
        Process exeProcess = new Process();
        try
        {
            //Giving Properties to Process we invoked
            ProcessStartInfo processStartInformation = new ProcessStartInfo();
            processStartInformation.WorkingDirectory = pathOfFile;
            processStartInformation.FileName = "cmd.exe";
            processStartInformation.RedirectStandardInput = true;
            processStartInformation.RedirectStandardOutput = true;
            processStartInformation.RedirectStandardError = true;
            processStartInformation.CreateNoWindow = true;
            processStartInformation.UseShellExecute = false;
            exeProcess.StartInfo = processStartInformation;
            exeProcess = Process.Start(processStartInformation);
            exeProcess.StandardInput.AutoFlush = true;
            exeProcess.StandardInput.WriteLine(command);
            exeProcess.WaitForExit(3000);

            if (!exeProcess.HasExited)
            {
                exeProcess.Kill();
            }
            output = exeProcess.StandardOutput.ReadToEnd();
        }
        catch (Exception ex)
        {
            output = ex.ToString();
        }
        finally
        {
            if (exeProcess != null)
            {
                if (!exeProcess.HasExited)
                {
                    exeProcess.Kill();
                }
                exeProcess.Dispose();
            }
        }
        return output;
    }

    public void DeleteFilesAndFoldersRecursively(string target_dir)
    {
        foreach (string file in Directory.GetFiles(target_dir))
        {
            File.Delete(file);
        }

        foreach (string subDir in Directory.GetDirectories(target_dir))
        {
            DeleteFilesAndFoldersRecursively(subDir);
        }

        Thread.Sleep(1); // This makes the difference between whether it works or not. Sleep(0) is not enough.
        Directory.Delete(target_dir);
    }

    protected Stream ToStream(string data)
    {
        // convert string to stream
        byte[] byteArray = Encoding.UTF8.GetBytes(data);
        MemoryStream stream = new MemoryStream(byteArray);
        return stream;
    }

    protected void DownloadZip(object sender, EventArgs e)
    {
        string path = Session["submissionPath"].ToString();
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=Assignment.zip");
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "&lt;script type=\"text/javascript\"&gt;alert('File not Found');</script>");
        }
    }

    //invoked when upload button is clicked
    protected void uploadAssignment_Click(object sender, EventArgs e)
    {
        if (uploadZIPButton.HasFile)
        {
            path = Server.MapPath("~/Submissions/" + userID + "_" + assignID + ".zip");
            uploadZIPButton.SaveAs(path);
            //Invoking t grade assignment
            GradeAssignment(path, userID, assignID, userName, assignmentName);
            LoadStudentGridView();
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a file to upload')</script>");
        }
    }
}