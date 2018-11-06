<%@ Page EnableEventValidation="true" Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="InstructorView.aspx.cs" Inherits="InstructorView" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin-top: 30px">
        <div class="row">
            <div class="col-md-9">
                <div class="clearfix">
                    <button type="button" class="btn btn-success float-right" data-toggle="modal" data-target=".myModal"><i class="fa fa-plus"></i>Assignments</button>
                </div>
                <hr>
                <div class="tab-content">
                    <div id="home" class="container tab-pane active">
                        <br>
                        <h3>HOME</h3>
                        <p>Welcome to Java course site</p>
                        <style type="text/css">
                            .hiddencol {
                                display: none;
                            }
                        </style>
                        <div>
                            <asp:GridView ID="assignmentsGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                                    <asp:BoundField DataField="assignmentName" HeaderText="Name" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                                    <asp:HyperLinkField DataNavigateUrlFields="assignmentId,assignmentName" DataNavigateUrlFormatString="Grader.aspx?assignmentId={0}&assignmentName={1}" DataTextField="assignmentName" HeaderText="Assignment Name" />
                                    <asp:BoundField DataField="dueDate" HeaderText="Due Date" />
                                    <asp:BoundField DataField="points" HeaderText="Points"></asp:BoundField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                                                CommandArgument='<%# Eval("assignmentId") %>'></asp:LinkButton>
                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Button ID="deleteAssignment" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="deleteAssignment_Click"></asp:Button>
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
                    </div>
                </div>
            </div>
            <div class="col-md-3">
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
            </div>
            <div class="col-md-4 modal fade myModal">
                <div class="modal-content">
                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="usr">Name:</label>
                            <asp:TextBox ID="assignmentName" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="fle">Assignment:</label>
                            <asp:FileUpload ID="fileUploadButton" runat="server" />
                        </div>
                        <div class="form-group">
                            <label for="pts">Grade Points:</label>
                            <asp:TextBox ID="assignmentPoints" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="date">Due Date:</label>
                            <asp:TextBox type="date" ID="assignmentDueDate" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <asp:Button ID="Button1" class="btn btn-success" runat="server" Text="Add" OnClick="createAssignment_Click" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>
