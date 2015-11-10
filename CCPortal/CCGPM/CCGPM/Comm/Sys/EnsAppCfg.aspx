<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_Sys_EnsAppCfg" Codebehind="EnsAppCfg.aspx.cs" %>

<%@ Register src="../UC/UCSys.ascx" tagname="UCSys" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>属性设置</title>
    <link href="../Style/Table0.css" rel="stylesheet" type="text/css" />
		<script language="JavaScript" src="../JScript.js"></script>

    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <% string ensName = this.Request.QueryString["EnsName"]; %>
    <table width="100%">
   <caption ><a href="EnsAppCfg.aspx?EnsName=<%=ensName %>&DoType=Base" >基本设置</a> | <a href="EnsAppCfg.aspx?EnsName=<%=ensName %>&DoType=Adv" >高级设置</a>| <a href="EnsAppCfg.aspx?EnsName=<%=ensName %>&DoType=ExpImp" >导入导出</a> </caption>
     <tr>
    <td>
        <uc1:UCSys ID="UCSys1" runat="server" />
    </td>
    </tr>
    </table>
    
    </form>
</body>
</html>
