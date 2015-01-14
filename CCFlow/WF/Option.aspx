<%@ Page language="c#" Inherits="BP.Web.WF.WF.Option" Codebehind="Option.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> Process Options </title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<base target="_self">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face=" Times New Roman ">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" height="100%" width="100%" border="1"
					bgcolor="#FFFFFF">
					<TR>
						<TD colspan="2" height="1%">
							<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
					</TR>
					<TR>
						<TD height="100%" width="40%"><br>
							<br>
							<asp:Label id="Label1" runat="server" height="100%" Width="100%">Label</asp:Label>
						</TD>
						<TD width="100%"><asp:TextBox id="TextBox1" runat="server" height="100%" Width="100%" TextMode="MultiLine"></asp:TextBox></TD>
					</TR>
					<TR align="right">
						<TD colspan="2">
							<asp:Button id="Btn_Save"  CssClass=Btn runat="server" Text="  确 定  " onclick="Button1_Click"></asp:Button>&nbsp;&nbsp;
							<asp:Button id="Btn_Cancel" CssClass=Btn runat="server" Text="  取 消  " onclick="Button2_Click"></asp:Button>&nbsp;&nbsp;</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
