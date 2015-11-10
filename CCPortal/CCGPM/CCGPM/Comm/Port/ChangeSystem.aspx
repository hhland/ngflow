<%@ Register TagPrefix="uc1" TagName="UCSys" Src="../UC/UCSys.ascx" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.ChangeSystem" Codebehind="ChangeSystem.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ChangeSystem</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../Style.css" type="text/css" rel="stylesheet">
		<LINK href="../Table.css" type="text/css" rel="stylesheet">
		<script language=javascript>
<!--
//-->
</script>

	</HEAD>
	<body class="Body<%=BP.Web.WebUser.Style%>"  topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" align="left" height='100%' cellSpacing="1" cellPadding="1" width="100%"
				border="1">
				<TR Width="100%">
					<TD Width="100%" colSpan="2" height='1%'>
						<asp:Label id="Label1" runat="server">Label</asp:Label><FONT face="宋体"></FONT></TD>
				</TR>
				<TR>
					<TD bgcolor=ActiveBorder style="WIDTH: 30%" valign="top">
						<P>&nbsp;&nbsp;&nbsp;</P>
						<P><FONT face="宋体">&nbsp;&nbsp;&nbsp; 如左边的系统列表, <FONT face="宋体">这些系统之间是整合在一起，共享基础数据，（部门、员工、工作岗位、职务、等信息。）数据的完整性、及时性得到了保证。
						</P>
						<P>&nbsp;&nbsp;&nbsp; 从一个系统转到另外一个系统，只需要一键切换。</P>
						<P>&nbsp;&nbsp;&nbsp; 没有连接的系统就是本系统.</P>
						</FONT></FONT>
					</TD>
					<TD vAlign="top" width="70%">
						<P><FONT face="宋体">
								<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys></P>
						</FONT><FONT face="宋体"></FONT></TD>
				</TR>
			</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
