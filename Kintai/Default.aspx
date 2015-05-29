<%@ Page Title="勤怠" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Kintai._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        勤怠
    </h2>
    <div>
        <asp:Panel ID="InputPanel" runat="server">
            <dl style="width: 180px">
                <dt style="float: left; width: 100px; background-color: #f0f0f0">
                    勤務開始時刻</dt>
                <dd style="float: right; width: 40px;">
                    <asp:Label ID="BeginTime" runat="server" />
                </dd>
            </dl>
            <dl style="width: 180px">
                <dt style="float: left; width: 100px; background-color: #f0f0f0">
                    勤務終了時刻</dt>
                <dd style="float: right; width: 40px;">
                    <asp:Label ID="EndTime" runat="server" />
                </dd>
            </dl>
            <!--
            <div style="width: 300px; text-align: right">
                <asp:Button ID="BeginButton" runat="server" Text="出社" />&nbsp;
                <asp:Button ID="EndButton" runat="server" Text="退勤" />
            </div>
            -->
        </asp:Panel>
    </div>
    <div>
        <asp:LoginView ID="HeadLoginView" runat="server" EnableViewState="false">
            <AnonymousTemplate>
                アカウントがない場合は<a href="/Account/Register.aspx">こちら</a>からユーザー登録を行ってください。
            </AnonymousTemplate>
        </asp:LoginView>
    </div>
</asp:Content>
