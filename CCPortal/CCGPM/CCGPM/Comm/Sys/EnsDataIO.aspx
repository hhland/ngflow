<%@ Page Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" Inherits="Comm_Sys_EnsDataIO" Title="无标题页" Codebehind="EnsDataIO.aspx.cs" %>

<%@ Register src="../UC/Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<LINK href="../Style/Table0.css" type="text/css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Pub ID="Pub1" runat="server" />
    <uc1:Pub ID="Pub2" runat="server" />
    
</asp:Content>

