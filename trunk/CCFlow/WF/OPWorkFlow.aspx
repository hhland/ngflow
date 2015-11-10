<%@ Page language="c#" Inherits="BP.Web.WF.StopWorkFlow" Codebehind="OPWorkFlow.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> Process Operation </title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="./Comm/Style.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="./Comm/JScript.js"></script>
	</HEAD>
	<body class="Body<%=BP.Web.WebUser.Style%>">
		<form id="Form1" method="post" runat="server">
			<FONT face=" Times New Roman ">
				<BR>
				<BR>
				<div align="center">
					<TABLE id="Table1" width="80%" style=" HEIGHT: 247px" cellSpacing="1" cellPadding="1" border="1">
						<TR>
							<TD align="center" bgColor="activeborder">
                                <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                                <STRONG>
									 Please explain why , Then perform the appropriate action .</STRONG>
							</TD>
						</TR>
						<TR>
							<TD style="HEIGHT: 195px" align="center"><cc1:tb id="TB1" runat="server" Height="188px" TextMode="MultiLine" Width="100%"> Enter a reason гогого</cc1:tb></TD>
						</TR>
						<TR>
							<TD align="center">
								<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
						</TR>
						<TR>
							<td>
								<asp:Label id="Label1" runat="server">Label</asp:Label>
							</td>
						</TR>
						<TR>
							<td>
								&nbsp;</td>
						</TR>
					</TABLE>
				</div>
			</FONT>
		</form>
	</body>
</HTML>
