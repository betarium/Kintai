<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangeProfile.aspx.cs" Inherits="Kintai.Account.ChangeProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="TextBox1" runat="server" CssClass="error">エラー</asp:RequiredFieldValidator>
    <div style="text-align: right">
        <asp:Button ID="SaveButton" runat="server" Text="保存" OnClick="SaveButton_Click" />
    </div>
</asp:Content>
