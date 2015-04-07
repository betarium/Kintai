<%@ Page Title="帳票出力" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Export.aspx.cs" Inherits="Kintai.Work.Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        帳票出力
        <asp:Label ID="TargetMonthHead" runat="server" Text="" />
    </h2>
    <br />
    <div>
        対象年月
        <asp:TextBox ID="TargetMonth" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TargetMonth"
            ValidationExpression="[\d]+-[\d]+"><span class="failureNotification">YYYY-MM形式で入力してください。</span></asp:RegularExpressionValidator>
    </div>
    <br />
    <div style="height: 3em;">
        <asp:LinkButton ID="DownloadLink" runat="server" OnClick="DownloadLink_Click">ダウンロード</asp:LinkButton>
    </div>
    <div>
        <asp:Button ID="ReportButton" runat="server" Text="帳票の保存" OnClick="ReportButton_Click" />
    </div>
</asp:Content>
