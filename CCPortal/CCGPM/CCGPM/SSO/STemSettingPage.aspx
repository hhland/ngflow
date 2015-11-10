<%@ Page Title="" Language="C#" MasterPageFile="~/SSO/MasterPage.master" AutoEventWireup="true" Inherits="SSO_STemSettingPage" Codebehind="STemSettingPage.aspx.cs" %>
<%@ Register Src="STemSetting.ascx" TagName="STemSetting" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="Style/default.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <uc2:STemSetting ID="ToolBarss1" runat="server" />
</asp:Content>
