<%@ Page language="c#" Inherits="BP.Web.WF.WorkList" Codebehind="WorkList.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCFlow" Src="./UCFlow.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"  >
<HTML>
	<HEAD>
		<title> Work List </title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="JavaScript" src="./Comm/JScript.js"></script>
		<script language="JavaScript" src="./Comm/Menu.js"></script>
		<script language="javascript" for="document" event="onkeydown">
<!--
  if(event.keyCode==13)
     event.keyCode=9;
     
-->
		</script>
		<script language="JavaScript" src="Flow.js"></script>
		<style>A:link { TEXT-DECORATION: none }
	A:visited { TEXT-DECORATION: none }
	A:hover { TEXT-DECORATION: none }
		</style>
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face=" Times New Roman ">
				<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="0" border="1">
					<TR>
						<TD>
							<uc1:UCFlow id="UCFlow1" runat="server"></uc1:UCFlow></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
