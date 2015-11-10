<%@ Page language="c#" Inherits="BP.Web.Comm.UI.UIDataHelpEnsValues" Codebehind="UIDatahelpensvalues.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>获取或设置默认值</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<!-- 
		<Meta http-equiv="Expires" Content="0"  >
		<Meta http-equiv="Page-Enter" Content="revealTrans(duration=0.5, transition=8)"> 
		<Meta http-equiv="Page-Exit" Content="revealTrans(duration=0.5, transition=8)">
		-->
		
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body  class="Body<%=BP.Web.WebUser.Style%>"  onkeypress=Esc() leftMargin=0 
topMargin=0  >
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" border="1" style="WIDTH:100%; HEIGHT:0px">
					<TR>
						<TD style="WIDTH:100%; HEIGHT:0px">
							<cc1:BPToolBar id="BPToolBar1" runat="server" AutoPostBack="True"></cc1:BPToolBar></TD>
					</TR>
					<TR>
						<TD style="WIDTH:100%; HEIGHT:0px">
							<cc1:CBL id="CBL1" runat="server" Width="100%"></cc1:CBL></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
