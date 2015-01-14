<%@ Page language="c#" Inherits="CCFlow.Web.Comm.UI.HelperOfTB" Codebehind="HelperOfTB.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> Gets or sets the default value </title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="Table.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="JScript.js"></script>
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face=" Times New Roman ">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="1" style="WIDTH:100%; HEIGHT:0px"
					bgcolor="InfoBackground">
					<TR>
						<TD style="WIDTH:100%; HEIGHT:0px">
							<cc1:BPToolBar id="BPToolBar1" runat="server" AutoPostBack="True"></cc1:BPToolBar></TD>
					</TR>
					<TR>
						<TD style="WIDTH:100%; HEIGHT:0px">
							<cc1:BPToolBar id="BPToolBar2" runat="server"></cc1:BPToolBar></TD>
					</TR>
					<TR align="left" style="WIDTH:100%; HEIGHT:0px">
						<TD align="left" style="WIDTH:100%; HEIGHT:0px">
							<cc1:CBL id="CBL1" runat="server" Width="100%"></cc1:CBL></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>