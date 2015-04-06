<%@ Page Title="勤怠" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Kintai._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        勤怠
    </h2>
    <p>
    </p>
    <asp:LoginView ID="HeadLoginView" runat="server" EnableViewState="false">
        <AnonymousTemplate>
            <p>
                最初に<a href="/Account/Register.aspx">こちら</a>からユーザー登録を行ってください。
            </p>
        </AnonymousTemplate>
    </asp:LoginView>
</asp:Content>
