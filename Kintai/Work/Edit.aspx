<%@ Page Title="登録" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Edit.aspx.cs" Inherits="Kintai.Work.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="info<%: InfoMessage.Text != ""? "": "0" %>">
        <asp:Literal ID="InfoMessage" runat="server"></asp:Literal>
    </div>
    <asp:HiddenField ID="EditFlag" runat="server" />
    <asp:HiddenField ID="WorkDate" runat="server" />
    <div style="background-color: Silver">
        <span style="text-align: left; margin: 0 10px 0 10px">
            <asp:LinkButton ID="BeforeDay" runat="server" OnClick="BeforeDay_Click">前日</asp:LinkButton>
        </span><span style="text-align: left; margin: 0 10px 0 10px">
            <asp:LinkButton ID="NextDay" runat="server" OnClick="NextDay_Click">翌日</asp:LinkButton>
        </span>
    </div>
    <h2>
        <span class="week<%: (int)TargetDate2.DayOfWeek %> holiday<%: isHoliday(TargetDate2) %>">
            <asp:Label ID="TargetDate" runat="server" />
        </span>
    </h2>
    <div>
        <asp:Label ID="Label5" runat="server" Text="区分"></asp:Label>
        <asp:DropDownList ID="WorkTypeDropDownList" runat="server">
            <asp:ListItem Value="1" Text="平日" />
            <asp:ListItem Value="2" Text="休日" />
            <asp:ListItem Value="3" Text="有給" />
            <asp:ListItem Value="4" Text="半休" />
            <asp:ListItem Value="5" Text="休出" />
            <asp:ListItem Value="6" Text="休暇" />
            <asp:ListItem Value="7" Text="なし" />
        </asp:DropDownList>
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="開始時刻"></asp:Label>
        <asp:TextBox ID="BeginTime" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="BeginTime"
            ValidationExpression="[\d]+:[\d]+"><span class="failureNotification">00:00形式で入力してください。</span></asp:RegularExpressionValidator>
        <div>
            <asp:Label ID="Label8" runat="server" Text="" Width="4em"></asp:Label>
            <small>
                <asp:LinkButton ID="BeginTimeInput" runat="server" OnClick="BeginTimeInput_Click">(現在時刻を入力)</asp:LinkButton></small>
        </div>
    </div>
    <div>
        <asp:Label ID="Label2" runat="server" Text="終了時刻"></asp:Label>
        <asp:TextBox ID="EndTime" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="EndTime"
            ValidationExpression="[\d]+:[\d]+"><span class="failureNotification">00:00形式で入力してください。</span></asp:RegularExpressionValidator>
        <div>
            <asp:Label ID="Label7" runat="server" Text="" Width="4em"></asp:Label>
            <small>
                <asp:LinkButton ID="EndTimeInput" runat="server" OnClick="EndTimeInput_Click">(現在時刻を入力)</asp:LinkButton></small>
        </div>
    </div>
    <div>
        <br />
    </div>
    <div>
        <asp:Label ID="Label3" runat="server" Text="休憩時間"></asp:Label>
        <asp:TextBox ID="RestTime" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="RestTime"
            ValidationExpression="[\d]+:[\d]+"><span class="failureNotification">00:00形式で入力してください。</span></asp:RegularExpressionValidator>
        <div>
            <asp:Label ID="Label9" runat="server" Text="" Width="4em"></asp:Label>
            <small>
                <asp:LinkButton ID="RestTime1Link" runat="server" OnClick="RestTime1Link_Click">(1:00)</asp:LinkButton></small>
        </div>
    </div>
    <div>
        <br />
    </div>
    <div>
        <asp:Label ID="Label6" runat="server" Text="労働時間"></asp:Label>
        <asp:Label ID="WorkTime" runat="server" ClientIDMode="Static" />
    </div>
    <div>
        <br />
    </div>
    <div>
        <asp:Label ID="Label4" runat="server" Text="業務内容" Style="vertical-align: top"></asp:Label>
        <asp:TextBox ID="WorkDetail" runat="server" Rows="10" Columns="40" TextMode="MultiLine"></asp:TextBox>
    </div>
    <div style="text-align: right; width: 600px">
        <asp:Button ID="SaveButton" runat="server" Text="保存" OnClick="SaveButton_Click" />
        <asp:Button ID="MailButton" runat="server" Text="メール送信" OnClick="MailButton_Click" />
    </div>
    <script type="text/javascript">
        function updateWorkTime() {
            var val = "";
            var begin = $("#BeginTime").val();
            var end = $("#EndTime").val();
            var rest = $("#RestTime").val();
            if (new RegExp("\\d+:\\d+").test(begin) && new RegExp("\\d+:\\d+").test(end)) {
                var begin2 = begin.split(":");
                var end2 = end.split(":");
                var min = parseInt(end2[0], 10) * 60 + parseInt(end2[1], 10);
                min -= parseInt(begin2[0], 10) * 60 + parseInt(begin2[1], 10);
                if (new RegExp("\\d+:\\d+").test(rest)) {
                    var rest2 = rest.split(":");
                    min -= parseInt(rest2[0], 10) * 60 + parseInt(rest2[1], 10);
                }
                min = Math.max(min, 0);
                val = Math.floor(min / 60).toString() + ":" + ("00" + (min % 60).toString()).substr(-2);
            }
            $("#WorkTime").text(val);
        }
        $("#BeginTime").change(updateWorkTime);
        $("#EndTime").change(updateWorkTime);
        $("#RestTime").change(updateWorkTime);
    </script>
</asp:Content>
