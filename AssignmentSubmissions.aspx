<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AssignmentSubmissions.aspx.cs" Inherits="AssignmentSubmissions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <style>
        #loader {
            position: absolute;
            left: 50%;
            top: 50%;
            z-index: 2;
            width: 150px;
            height: 150px;
            margin: -75px 0 0 -75px;
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* Add animation to "page content" */
        .animate-bottom {
            position: relative;
            -webkit-animation-name: animatebottom;
            -webkit-animation-duration: 1s;
            animation-name: animatebottom;
            animation-duration: 1s
        }

        @-webkit-keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0
            }

            to {
                bottom: 0px;
                opacity: 1
            }
        }

        @keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0
            }

            to {
                bottom: 0;
                opacity: 1
            }
        }

        .overlay {
            background: #e9e9e9;
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            opacity: 0.5;
            z-index: 1;
        }
    </style>
    <%--  <div class="overlay" id="gradingLoad" style="display: block;">
        <div id="loader" ></div>
    </div>--%>
    <div>
        <br />
        <br />
        <div style="text-align: center">
            <h3>Assignment Details</h3>
        </div>
        <br />
        <asp:PlaceHolder runat="server" ID="notSubmittedLBL" Visible="false">
             <h3>Not yet submitted the assignment</h3>
        </asp:PlaceHolder>
       
        <asp:GridView ID="studentGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                <asp:BoundField DataField="assignmentName" HeaderText="Name" />
                <asp:BoundField DataField="dueDate" HeaderText="Due Date" />
                <asp:BoundField DataField="points" HeaderText="Your Grade" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                            CommandArgument='<%# Eval("assignmentId") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" Height="35px" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#eeeeee" ForeColor="#333333" Height="35px" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <h3>&nbsp;</h3>
        <div class="container">
            <asp:PlaceHolder ID="resultPH" runat="server" Visible="false">
                <div class="panel panel-default">
                    <div class="panel-heading" style="align-content: center; font-weight: bold">Graded Percentage</div>
                    <div class="panel-body">
                        <asp:Label ID="gradeLBL" runat="server" Text=""></asp:Label><br />
                        <asp:LinkButton ID="downloadSubmission" runat="server" OnClick="DownloadZip">Click to download submitted assignment</asp:LinkButton>
                    </div>
                </div>
                <hr />
            </asp:PlaceHolder>
        </div>
        <h3>&nbsp;Upload your assignment</h3>
        <asp:FileUpload ID="uploadZIPButton" class="btn btn-success" runat="server" Width="316px" />
        <br />
        <br />
        <asp:Button ID="uploadAssignment" class="btn btn-success" runat="server" Text="Submit Assignment" OnClick="uploadAssignment_Click" />
        <br />
        <hr />
        <div>
            <asp:PlaceHolder ID="testcasePHMain" runat="server"> 
                <div class="container">
                    <asp:PlaceHolder ID="outputPH" runat="server" Visible="false">
                        <div class="panel panel-default">
                            <div class="panel-heading" style="align-content: center; font-weight: bold">Total average percentage</div>
                            <div class="panel-body">
                                <asp:Label ID="percentLBL" runat="server" Text="No test case provided" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <hr />
            </asp:PlaceHolder>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('[data-toggle="popover"]').popover();
        });
    </script>
</asp:Content>

