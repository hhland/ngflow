<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="uc1" TagName="UCEn" Src="UC/UCEn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.GroupEns" Codebehind="GroupEns.aspx.cs" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>驰骋软件,值得信赖.</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Menu.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="Menu.js"></script>
		<script language="JavaScript" src="JScript.js"></script>
		
		<LINK href="Table.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="Body<%=BP.Web.WebUser.Style%>" onkeypress=Esc() 
onclick="HideMenu( 'WebMenu1' )" leftMargin=0 background=BJ1.gif topMargin=0  >
		<form id="Form1" method="post" runat="server">
			<TABLE class="Table<%=BP.Web.WebUser.Style%>" id=Table1 cellSpacing=1 
cellPadding=0 width="100%" height='100%' align=left border=1 >
				<TR>
					<TD colSpan="2" height="0"><asp:label id="Label1" runat="server">Label</asp:label></TD>
				</TR>
				<TR>
					<TD colSpan="2">
						<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
				</TR>
				<TR valign="top" height='100%'>
					<TD vAlign="top" noWrap width='20%'>
						<TABLE id="Table2" bgcolor="buttonface" cellSpacing="1" cellPadding="1" width="100%" border="1">
							<TR>
								<TD>分类条件</TD>
							</TR>
							<TR>
								<TD>
									<asp:checkboxlist id="CheckBoxList1" runat="server"></asp:checkboxlist></TD>
							</TR>
							<TR>
								<TD>分析项目</TD>
							</TR>
							<TR>
								<TD>
									<cc1:ddl id="DDL_GroupField" runat="server" onselectedindexchanged="CheckBoxList1_SelectedIndexChanged"></cc1:ddl></TD>
							</TR>
							<TR>
								<TD>分析方式</TD>
							</TR>
							<TR>
								<TD><cc1:ddl id="DDL_GroupWay" runat="server" onselectedindexchanged="CheckBoxList1_SelectedIndexChanged"></cc1:ddl>
									<cc1:ddl id="DDL_Order" runat="server"></cc1:ddl>
								</TD>
							</TR>
							<TR>
								<TD>
									<asp:CheckBox id="CheckBox1" runat="server" Text="显示编号列"></asp:CheckBox>
								</TD>
							</TR>
							<TR>
								<TD>取前:<asp:TextBox id="TB_Top" runat="server" Width="60px">10000</asp:TextBox>名</TD>
							</TR>
						</TABLE>
					</TD>
					<TD valign="top" ><uc1:ucsys id="UCSys1" runat="server"></uc1:ucsys>
					</TD>
				</TR>
			</TABLE>
			<%--<cc1:webmenu id="WebMenu1" style="Z-INDEX: 101; LEFT: 560px; POSITION: absolute; TOP: 100px"
				runat="server" Width="152px"></cc1:webmenu><FONT face="宋体"></FONT>--%></form>
	</body>
</HTML>
