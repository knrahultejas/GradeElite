<%@ Page EnableSessionState="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Grader.aspx.cs" Inherits="Grader" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <h3 style="text-align: center;">Assignment Submissions</h3>
    <br />
    <h3 style="color: peru">
        <asp:Label ID="noSubmissionsLBL" runat="server" Text=""></asp:Label></h3>
    <br />
    <h3 style="text-align: center;">Test Cases for Assignment</h3>
    <div id="tcGridViewDiv" runat="server">
        <asp:GridView ID="testCaseGV" runat="server" AutoGenerateColumns="False" Style="color: #333333; width: 98%; margin-left: 10px;">
            <Columns>
                <asp:BoundField DataField="testCaseId" HeaderText="TestCase Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ApplyFormatInEditMode="True">
                    <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                    <ItemStyle CssClass="hiddencol"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="sampleInput" HeaderText="Sample Input" ApplyFormatInEditMode="True" />
                <asp:BoundField DataField="sampleOutput" HeaderText="Sample Output" ApplyFormatInEditMode="True" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Button ID="deleteTestCase" CssClass="btn-danger" runat="server" Text="Delete" OnClick="deleteTestCase_Click"></asp:Button>
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
    </div>
    <h5>
        <asp:Label ID="noTestCasesLBL" runat="server" Text=""></asp:Label></h5>
    <div class="clearfix">
        <button type="button" class="btn btn-success float-right" data-toggle="modal" data-target=".myModal"><i class="fa fa-plus"></i>Add Test Case</button>
    </div>
    <br />
    <br />
    <h3 style="text-align: center;">Student Submissions for Assignment</h3>
    <br />
    <asp:GridView ID="studentGridView" runat="server" AutoGenerateColumns="False" Style="color: #333333; width: 98%; margin-left: 10px;">
        <Columns>
            <asp:BoundField DataField="assignmentId" HeaderText="Assignment Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" ApplyFormatInEditMode="True">
                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                <ItemStyle CssClass="hiddencol"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="id" HeaderText="User Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol">
                <HeaderStyle CssClass="hiddencol"></HeaderStyle>
                <ItemStyle CssClass="hiddencol"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="userName" HeaderText="Student Name" ApplyFormatInEditMode="True" />
            <asp:BoundField DataField="assignmentName" HeaderText="Assignment Name" ApplyFormatInEditMode="True" />
            <asp:BoundField DataField="points" HeaderText="Points" ApplyFormatInEditMode="True" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="updatePoints" CssClass="btn-success" runat="server" Text="Update Grade" OnClick="updateGrade"></asp:Button>
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
    <br />

    <asp:PlaceHolder runat="server" ID="updateGradePH" Visible="false">
        <hr />
        <div class="container">
            <asp:Label ID="assIdLBL" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="userIdLBL" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text="Name:"></asp:Label>
            <asp:Label ID="nameLBL" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="Label3" runat="server" Text="Assignment Name:"></asp:Label>
            <asp:Label ID="assLBL" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="Label5" runat="server" Text="Points:"></asp:Label>
            <asp:TextBox ID="pointsTB" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="update" runat="server" Text="Update" OnClick="update_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="closeUpdate" runat="server" Text="Close" OnClick="closeUpdate_Click" />
        </div>
        <br />
    </asp:PlaceHolder>
    <hr />
    <h4>Click the button to download Excel with plagarim details.
    </h4>
    <asp:Button ID="plagarismButton" CssClass="btn-success" runat="server" Text="Check Plagarism" OnClick="plagarismButton_Click"></asp:Button>

    <div class="row">
        <div class="col-md-4">
        </div>
        <div class="col-md-4 modal fade myModal">
            <div class="modal-content">
                <!-- Modal body -->
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-6">
                                <label for="date">Sample Input:</label>
                                <asp:FileUpload ID="sampleInputFileUpload" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <label for="date">Sample Output:</label>
                                <asp:FileUpload ID="sampleOutputFileUpload" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal footer -->
                <div class="modal-footer">
                    <asp:Button ID="addTestCase" class="btn btn-success" runat="server" Text="Add" OnClick="addTestCase_Click" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
