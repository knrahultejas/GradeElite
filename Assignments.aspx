<%@ Page Title="Student" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Assignments.aspx.cs" Inherits="Assignments" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <br />
    <div style="text-align: center;">
        <h3>List of Assignments</h3>
    </div>
    <br />
    <asp:GridView ID="assignmentsGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
            <asp:HyperLinkField DataNavigateUrlFields="assignmentId,assignmentName" DataNavigateUrlFormatString="AssignmentSubmissions.aspx?assignmentId={0}&assignmentName={1}" DataTextField="assignmentName" HeaderText="Assignment Name" />
            <asp:BoundField DataField="dueDate" HeaderText="Due Date" />
            <asp:BoundField DataField="points" HeaderText="Points"></asp:BoundField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                        CommandArgument='<%# Eval("assignmentId")%>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
</asp:Content>
