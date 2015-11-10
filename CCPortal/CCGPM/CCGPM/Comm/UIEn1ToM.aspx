<%@ Reference Control="~/comm/uc/ucen.ascx" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.UI.UIEn1ToM" Codebehind="UIEn1ToM.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register src="UC/ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>感谢您选择:<%=BP.Sys.SystemConfig.DeveloperShortName%>
			一对多的关系维护</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
        <meta http-equiv="Pragma" contect="no-cache">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="JScript.js"></script>
		<base target="_self">
	    <link href="./Style/Table.css" rel="stylesheet" type="text/css" />
	   
	</HEAD>
	<body  onkeypress=Esc() leftMargin=0 
topMargin=0 >
		<form id="aspnetForm" method="post" runat="server">
			<div align="center" >
				<TABLE   align="left" cellSpacing="1" cellPadding="1" width="100%" border="1"  >
					<TR>
						<td width="100%" height="1%">
							<asp:Label id="Label1" runat="server">Label</asp:Label>
						</td>
					</TR>
					<TR >
						<TD  width="100%"  height="1%" class=Toolbar >
							<uc2:ToolBar ID="ToolBar1" runat="server" />
                        </TD>
					</TR>
					<TR  valign=top >
						<TD width="100%"  height="100%"> 
								<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys>
                                <cc1:Tree ID="Tree1" runat="server"></cc1:Tree>
						</TD>
					</TR>
				</TABLE>
			</div>
		</form>
	</body>
</HTML>
