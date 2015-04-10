<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HolidayRegister.aspx.cs" Inherits="Kintai.Work.HolidayRegister" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Calendar ID="HolidayCalendar" runat="server" OnSelectionChanged="Calendar_SelectionChanged"></asp:Calendar>
    <asp:TextBox ID="HolidayName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="HolidayName" runat="server" ErrorMessage="未入力です" CssClass="error"></asp:RequiredFieldValidator>
    <asp:Button ID="SaveButton" runat="server" Text="Button" OnClick="SaveButton_Click" />
    <p><asp:Label ID="SuccessMessage" runat="server" Text="SuccessMessage" Visible="False"></asp:Label></p>
</asp:Content>
