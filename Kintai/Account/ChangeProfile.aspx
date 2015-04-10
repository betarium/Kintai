<%@ Page Title="プロファイル" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChangeProfile.aspx.cs" Inherits="Kintai.Account.ChangeProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Account/ChangePassword.aspx">パスワード変更</asp:HyperLink>
    </div>
    <div class="info<%: InfoMessage.Text != ""? "": "0" %>">
        <asp:Literal ID="InfoMessage" runat="server"></asp:Literal>
    </div>
    <h3>プロファイル変更</h3>
    <asp:Label runat="server" AssociatedControlID="FullName">氏名</asp:Label>
    <asp:TextBox ID="FullName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="FullName"
        runat="server" CssClass="error">入力してください。</asp:RequiredFieldValidator>
    <div style="text-align: right; width: 600px">
        <asp:Button ID="SaveButton" runat="server" Text="保存" OnClick="SaveButton_Click" />
    </div>
</asp:Content>
