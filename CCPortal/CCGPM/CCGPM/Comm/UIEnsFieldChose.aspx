<%@ Page language="c#" Inherits="BP.Web.WF.Comm.UIEnsFieldChose" Codebehind="UIEnsFieldChose.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>³Û³ÒÈí¼þ</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<base target="_self">
        <meta http-equiv="Pragma" contect="no-cache">
		
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" height="100%" border="1">
				<TR>
					<TD>
						<asp:Label id="Label1" runat="server">Label</asp:Label>
					</TD>
				</TR>
				<TR>
					<TD>
						<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
				</TR>
				<TR valign="top" height="100%" bgcolor="infobackground">
					<TD height="100%">
						<cc1:CBL id="CBL1" runat="server"></cc1:CBL></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
