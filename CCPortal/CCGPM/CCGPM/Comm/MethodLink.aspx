<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_MethodLink" Codebehind="MethodLink.aspx.cs" %>
<%@ Register Src="UC/ucsys.ascx" TagName="ucsys" TagPrefix="uc2" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Esc 键关闭窗口.</title>
		<meta content="Microsoft FrontPage 5.0" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="Style.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="JScript.js"></script>
		<script language="JavaScript" src="Menu.js"></script>
		<script language="JavaScript" src="ActiveX.js"></script>
		<base target="_self" />
		
		<LINK href="./Style/Table.css" type="text/css" rel="stylesheet">
		
		<script language="javascript" for="document" event="onkeydown">
<!--
 if (window.event.srcElement.tagName="TEXTAREA") 
     return false;
  if(event.keyCode==13)
     event.keyCode=9;
-->


</script>
		<script language="javascript" >
function ShowIt(m)
{
   var url='Method.aspx?M='+m;
   var a=window.showModalDialog( url, 'OneVs' ,'dialogHeight: 400px; dialogWidth: 500px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no'); 
}
</script>
	</HEAD>
	<body    onkeypress=Esc() leftMargin=0 
topMargin=0>
    <form id="form1" runat="server">
    <div>
        <uc2:ucsys ID="Ucsys1" runat="server" />
        &nbsp;</div>
    </form>
</body>
</html>
