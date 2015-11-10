<%@ Page language="c#" AutoEventWireup="false" Inherits="BP.Web.Comm.Item3" Codebehind="Item3.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCEn" Src="UC/UCEn.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>驰骋软件,值得信赖.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="JavaScript" src="JScript.js"></script>
		<script language="JavaScript" src="Menu.js"></script>
		<LINK href="Table.css" type="text/css" rel="stylesheet">
		
		<script language="JavaScript" src="JScript.js"></script>
		<script language="javascript" for="document" event="onkeydown">
<!--
  if (window.event.srcElement.tagName="TEXTAREA") 
     return false;

  if(event.keyCode==13)
     event.keyCode=9;
-->
		</script>
	 
		<base target="_self" />
	</HEAD>
	<body   topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" align="center" class='Table' cellSpacing="1"  height='10px' cellPadding="1" width='95%'
				border="1">
				<TR>
					<TD class=TD >
						<asp:Label id="Label1" Font-Size=xX-Small  runat="server">Label</asp:Label></TD>
				</TR>
				<TR>
					<TD>
						<uc1:UCEn id="UCEn1" runat="server"></uc1:UCEn></TD>
				</TR>
				<TR>
					<TD>
						<asp:Label id="Label2" runat="server" Font-Bold="True"></asp:Label></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
