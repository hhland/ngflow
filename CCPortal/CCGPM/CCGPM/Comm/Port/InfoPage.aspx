<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.Info" Codebehind="InfoPage.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCSys" Src="../UC/UCSys.ascx" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ÏûÏ¢¿ò</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Style.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../JScript.js"></script>
		<script language="javascript" src="../Menu.js"></script>
		<script language="javascript" src="../ActiveX.js"></script>
		<LINK href="./Table.css" type="text/css" rel="stylesheet">
		<base  target=_self /> 
		<Meta http-equiv="Pragma" Content="No-cach">
	</HEAD>
	<body class="Body" onkeypress=Esc()  >
		<form id="ErrPage" method="post" runat="server">
		<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys>
		</form>
	</body>
</HTML>
