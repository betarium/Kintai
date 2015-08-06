<%@ Page Title="勤怠" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Kintai._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        勤怠
    </h2>
    <div>
        <asp:Panel ID="MessagePanel" runat="server">
        <div style="background-color: Yellow; padding: 1em 1em 1em 1em; border: 3px solid silver">
            <asp:Label ID="TopMessage" runat="server" />
        </div>
        </asp:Panel>
        <h3>
            <asp:Label ID="TargetDate" runat="server" />
        </h3>
        <asp:Panel ID="InputPanel" runat="server">
            <div>
                <dl style="width: 180px">
                    <dt style="float: left; width: 100px; background-color: #f0f0f0">勤務開始時刻</dt>
                    <dd style="float: right; width: 40px;">
                        <asp:Label ID="BeginTime" runat="server" />
                    </dd>
                </dl>
                <dl style="width: 180px">
                    <dt style="float: left; width: 100px; background-color: #f0f0f0">勤務終了時刻</dt>
                    <dd style="float: right; width: 40px;">
                        <asp:Label ID="EndTime" runat="server" />
                    </dd>
                </dl>
            </div>
            <div style="clear: both">
            </div>
            <div style="width: 300px; text-align: right">
                <% if (BeginTime.Text == "" && EndTime.Text == "")
                   { %>
                <asp:Button ID="BeginButton" runat="server" Text="出社" OnClick="BeginButton_Click" />&nbsp;
                <% } %>
                <% if (EndTime.Text == "")
                   { %>
                <asp:Button ID="EndButton" runat="server" Text="退勤" OnClick="EndButton_Click" />&nbsp;
                <% } %>
                &nbsp;&nbsp;<asp:Button ID="EditButton" runat="server" Text="編集" OnClick="EditButton_Click" />
            </div>
        </asp:Panel>
    </div>
    <div>
        <asp:LoginView ID="HeadLoginView" runat="server" EnableViewState="false">
            <AnonymousTemplate>
                <a href="/Account/Login.aspx">ログイン</a>してください。<br />
                <br />
                アカウントがない場合は<a href="/Account/Register.aspx">こちら</a>からユーザー登録を行ってください。
            </AnonymousTemplate>
        </asp:LoginView>
    </div>
</asp:Content>
