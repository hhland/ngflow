<%@ Page Title="" Language="C#" MasterPageFile="~/SSO/MasterPage.master" AutoEventWireup="true"
    CodeBehind="InfoBarSetting.aspx.cs" Inherits="GMP2.SSO.InfoBarSetting" %>

<%@ Register Src="InfoBar.ascx" TagName="InfoBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Style/main.css" rel="stylesheet" type="text/css" />
    <link href="./../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="Style/default.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc2:InfoBar ID="InfoBar1" runat="server" />
</asp:Content>
