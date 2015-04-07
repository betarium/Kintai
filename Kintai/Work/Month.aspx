<%@ Page Title="月間" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Month.aspx.cs" Inherits="Kintai.Work.Month" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: Silver">
        <span style="text-align: left; margin: 0 10px 0 10px">
            <asp:LinkButton ID="BeforeMonth" runat="server" OnClick="BeforeMonth_Click">前月</asp:LinkButton>
        </span><span style="text-align: left; margin: 0 10px 0 10px">
            <asp:LinkButton ID="NextMonth" runat="server" OnClick="NextMonth_Click">翌月</asp:LinkButton>
        </span>
    </div>
    <h2>
        <asp:Label ID="TargetMonthHead" runat="server" Text="" />
    </h2>
    <asp:HiddenField ID="TargetMonth" runat="server" />
    <div>
        <table border="1">
            <tr>
                <th>
                    日付
                </th>
                <th>
                    区分
                </th>
                <th>
                    出社
                </th>
                <th>
                    退社
                </th>
                <th>
                    勤務
                </th>
                <th>
                    休憩
                </th>
                <th>
                    労働
                </th>
                <th>
                    <br />
                </th>
            </tr>
            <asp:Repeater ID="DateList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="week<%# (int)((Kintai.WorkTimeEntity)Container.DataItem).WorkDate.Value.DayOfWeek %>">
                            <%# ((Kintai.WorkTimeEntity)Container.DataItem).WorkDate.Value.ToString("d(ddd)")%>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.WorkTypeToString(((Kintai.WorkTimeEntity)Container.DataItem).WorkType) %>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.MinutesToTimeString(((Kintai.WorkTimeEntity)Container.DataItem).BeginTime) %>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.MinutesToTimeString(((Kintai.WorkTimeEntity)Container.DataItem).EndTime)%>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.MinutesToTimeString(((Kintai.WorkTimeEntity)Container.DataItem).OfficeTime)%>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.MinutesToTimeString(((Kintai.WorkTimeEntity)Container.DataItem).RestTime)%>
                        </td>
                        <td>
                            <%# Kintai.Work.Utility.MinutesToTimeString(((Kintai.WorkTimeEntity)Container.DataItem).WorkTime)%>
                        </td>
                        <td>
                            <asp:Button ID="EditButton" runat="server" Text="編集" OnClick="EditButton_Click" CommandArgument='<%# ((Kintai.WorkTimeEntity)Container.DataItem).WorkDate.Value.ToString(Kintai.Work.Utility.DATE_FORMAT_YYYYMMDD) %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <th>
                    合計
                </th>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <th>
                    <asp:Label ID="TotalOfficeTime" runat="server" />
                </th>
                <th>
                    <asp:Label ID="TotalRestTime" runat="server" />
                </th>
                <th>
                    <asp:Label ID="TotalWorkTime" runat="server" />
                </th>
                <td>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <table>
            <tr>
                <th>
                    実総労働時間
                </th>
                <td>
                    <asp:Label ID="TotalWorkTime2" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    休憩時間
                </th>
                <td>
                    <asp:Label ID="TotalRestTime2" runat="server" />
                </td>
            </tr>
            <tr>
                <th>
                    残業時間
                </th>
                <td>
                    <asp:Label ID="TotalOverTime2" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
