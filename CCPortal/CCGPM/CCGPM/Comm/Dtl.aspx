<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_Dtl" Codebehind="Dtl.aspx.cs" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="UC/ucsys.ascx" TagName="ucsys" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
		<Meta http-equiv="Page-Enter" Content="revealTrans(duration=0.5, transition=8)" />
		<LINK href="Menu.css" type="text/css" rel="stylesheet" />
		<LINK href="./Style/Table.css" type="text/css" rel="stylesheet" />
		
		<script language="JavaScript" src="JScript.js"></script>
		
</head>
<body topmargin="0" leftmargin="0" onkeypress="Esc()"> 
    <form id="form1" runat="server">
    <table  style="width: 100%" id="Table1" align="left" cellSpacing="1" cellPadding="1" border="1" width="100%" >
     <tr>
    <td >
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></td>
    </tr>
    <tr>
    <td >
        <cc1:BPToolBar ID="BPToolBar1" runat="server">
        </cc1:BPToolBar>
    </td>
    </tr>
      <tr>
    <td>
        <uc1:ucsys ID="Ucsys1" runat="server" />
        <uc1:ucsys ID="Ucsys2" runat="server" />
    </td>
    </tr>
    </table>
    </form>
</body>
</html>
