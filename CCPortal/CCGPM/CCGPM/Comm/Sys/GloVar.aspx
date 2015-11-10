<%@ Page Title="" Language="C#" MasterPageFile="~/Comm/WinOpen.master" AutoEventWireup="true" Inherits="Comm_Sys_GloVar" Codebehind="GloVar.aspx.cs" %>
<%@ Register src="../UC/UCSys.ascx" tagname="UCSys" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <LINK href="../Style/Table0.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="../JScript.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:UCSys ID="UCSys1" runat="server" />
</asp:Content>

