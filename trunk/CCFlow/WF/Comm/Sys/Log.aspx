<%@ Register TagPrefix="uc1" TagName="UCSys" Src="../UC/UCSys.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Page language="c#" Inherits="CCFlow.Web.Comm.Sys.UILog" Codebehind="Log.aspx.cs" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Log</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="JavaScript" src="../JScript.js"></script>
		<LINK href="../Style/Table0.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face=" Times New Roman ">
				<TABLE id="Table1" style="Z-INDEX: 101; LEFT: 0px; POSITION: absolute; TOP: 0px"  cellSpacing="1"
					cellPadding="1" width="100%"  height="100%" border="1">
					<TR>
						<TD height='1%' >
							<asp:Label id="Label1" runat="server">Label</asp:Label></TD>
					</TR>
					<TR>
						<TD height='1%' >
							<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
					</TR>
					<TR>
						<TD height='100%' valign=top >
							<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
