<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow_Comm_Sys_EnsAppCfg" Codebehind="EnsAppCfg.aspx.cs" %>

<%@ Register src="../UC/UCSys.ascx" tagname="UCSys" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Property set </title>
    <link href="../Style/Table0.css" rel="stylesheet" type="text/css" />
    <base target=_self />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:UCSys ID="UCSys1" runat="server" />
        <uc1:UCSys ID="UCSys2" runat="server" />
    </form>
</body>
</html>
