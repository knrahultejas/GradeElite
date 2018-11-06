<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div style="align-content: center;">
        <div class="jumbotron" style="width:70%; align-content:center;">
            <h3><span style="color: green;"></span></h3>
            <h3><span style="color: green;"></span></h3>
            <fieldset>
                <legend>GradeElite Login</legend>
                <div class="form-group">
                    <label for="inputEmail" class="control-label">Email</label>
                    <div>
                        <asp:TextBox runat="server" ID="UserName" CssClass="form-control"  type="email" class="form-control" placeholder="Email" name="email" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="password" class="control-label">Password</label>
                    <div>
                        <asp:TextBox runat="server" ID="Password" CssClass="form-control"  type="password" class="form-control" placeholder="Password" name="password" required="required" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-lg-10 col-lg-offset-2">
                        <asp:Button ID="Button1" runat="server" Text="submit" type="submit" class="btn btn-primary" name="submit" OnClick="LogIn" />
                    </div>
                </div>
            </fieldset>
            <h3><span style="color: red;"></span></h3>
        </div>
    </div>
</asp:Content>

