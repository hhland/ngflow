<%@ Page language="c#" Inherits="BP.Web.Comm.UI.DBIO" Codebehind="DBIO.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SystemClass</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../Style/Table0.css" type="text/css" rel="stylesheet">

	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="1">
					<TR>
						<TD>
							<cc1:BPToolBar id="BPToolBar1" runat="server"></cc1:BPToolBar></TD>
					</TR>
					<TR>
						<TD>
							<cc1:CBL id="CBL1" runat="server"></cc1:CBL></TD>
					</TR>
					<TR>
						<TD>注册系统实体：就是规范化实体描述与物理数据库的对应是否符合，把不符合的修改为符合。</TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
