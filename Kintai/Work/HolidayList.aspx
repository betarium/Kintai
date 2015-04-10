<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HolidayList.aspx.cs" Inherits="Kintai.Work.HolidayList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table border="1">
        <tr>
            <th>日付</th>
            <th>名前</th>
        </tr>
        <tr>
            <td>1001-12-31</td>
            <td>テストの日</td>
        </tr>
        <asp:Repeater ID="DataList" runad="server">
            <tr>
                <td></td>
                <td></td>
            </tr>
        </asp:Repeater>
    </table>

    </asp:Table>
</asp:Content>
