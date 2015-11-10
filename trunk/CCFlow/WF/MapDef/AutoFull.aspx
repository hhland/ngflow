<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.WF_MapDef_AutoFull" Codebehind="AutoFull.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../../../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table  width='900px' >
<tr>
<td valign=top width="20%" align=left ><uc1:Pub ID="Left" runat="server" /></td>
<td  valign=top  width="80%" align=left ><uc1:Pub ID="Pub1" runat="server" /></td>
</tr>
</table>
</asp:Content>