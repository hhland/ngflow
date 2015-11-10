<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Page language="c#" Inherits="BP.Web.WF.Comm.GroupEnsDtl" Codebehind="GroupEnsDtl.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>驰骋软件,值得信赖.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Menu.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="Menu.js"></script>
		<LINK href="Table.css" type="text/css" rel="stylesheet">
		<LINK href="Search.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="Body<%=BP.Web.WebUser.Style%>" onkeypress=Esc() 
  leftMargin=0 background=BJ1.gif topMargin=0 >
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="1">
					<TR>
						<TD>
							<asp:Label id="Label1" runat="server">Label</asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys></TD>
					</TR>
				</TABLE>
			</FONT>
			
		</form>
	</body>
</HTML>
