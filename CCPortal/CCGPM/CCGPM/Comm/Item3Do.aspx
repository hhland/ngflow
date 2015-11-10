<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_Item3Do" Codebehind="Item3Do.aspx.cs" %>

<%@ Register Src="UC/ucsys.ascx" TagName="ucsys" TagPrefix="uc1" %>
		<base target="_self" />
		<LINK href="Menu.css" type="text/css" rel="stylesheet">
		<LINK href="Table.css" type="text/css" rel="stylesheet">
		
		<script language="JavaScript" src="JScript.js"></script>
		<script language="JavaScript" src="Menu.js"></script>
		

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>chichengsoft.com : Esc 关闭窗口</title>
</head>
<body  onkeypress="Esc()"  >
    <form id="form1" runat="server">
    <div>
    
    <table border=1  align=center width='80%' >
    <tr>
    <td class=BigDoc valign=middle >
    
    <uc1:ucsys ID="Ucsys1" runat="server" />
    
    </td>
    </tr>
    
    </table>
        
    
    </div>
    </form>
</body>
</html>
