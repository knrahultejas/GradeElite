<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeFile="AdminView.aspx.cs" Inherits="AdminView" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="container">
        <div style="align-content: center;">
            <h2>Admin</h2>
        </div>
        <div style="align-content: center;">
            <asp:GridView ID="userListGV" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="table-striped" AutoGenerateColumns="False" Style="color: #333333; width: 98%; margin-left: 10px;">
                <Columns>
                    <asp:BoundField DataField="userName" HeaderText="User Name" />
                    <asp:BoundField DataField="role" HeaderText="Role" />
                    <asp:BoundField DataField="isBlocked" HeaderText="Blocked Status" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="black" Height="35px" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#eeeeee" ForeColor="#333333" Height="35px" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
        <hr />
        <div>
            <div class="form-group">
                <label for="inputEmail" class="control-label">Add User</label>
                <div>
                    <asp:TextBox type="text" class="form-control" ID="addEmail" placeholder="Email" name="addU" runat="server"></asp:TextBox>
                    <asp:DropDownList ID="roleDD" runat="server">
                        <asp:ListItem Selected="True">student</asp:ListItem>
                        <asp:ListItem>instructor</asp:ListItem>
                        <asp:ListItem>admin</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />
                <div>
                    <asp:Button ID="addUser" runat="server" class="btn btn-primary" Text="Add User" OnClick="addUser_Click" />
                </div>
            </div>
        </div>
        <hr />
        <div>
            <div class="form-group">
                <label for="inputEmail" class="control-label">Block User</label>
                <div>
                    <asp:TextBox type="text" class="form-control" ID="blockEmail" placeholder="Email" name="addU" runat="server"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Button ID="blockUser" runat="server" class="btn btn-primary" Text="Block User" OnClick="blockUser_Click" />
                </div>
            </div>
        </div>
        <hr />
        <div>
            <div class="form-group">
                <label for="inputEmail" class="control-label">Unblock User</label>
                <div>
                    <asp:TextBox type="text" class="form-control" ID="unblockEmail" placeholder="Email" name="addU" runat="server"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Button ID="unblockUser" runat="server" class="btn btn-primary" Text="Unblock User" OnClick="unblockUser_Click" />
                </div>
            </div>
        </div>
        <hr />
        <div>
            <div class="form-group">
                <label for="inputEmail" class="control-label">Delete User</label>
                <div>
                    <asp:TextBox type="text" class="form-control" ID="deleteEmail" placeholder="Email" name="addU" runat="server"></asp:TextBox>
                </div>
                <br />
                <div>
                    <asp:Button ID="deleteUser" runat="server" class="btn btn-danger" Text="Delete User" OnClick="deleteUser_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
