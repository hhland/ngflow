<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.UIEn" ValidateRequest="false" Codebehind="UIEn.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCEn" Src="UC/UCEn.ascx" %>
<%@ Register src="UC/ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat=server>
	<title>  Gallop Software , Trustworthy .</title>
		<meta content="Microsoft FrontPage 12.0" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
	   	<META   HTTP-EQUIV="pragma"   CONTENT="no-cache">   
         <META   HTTP-EQUIV="Cache-Control"   CONTENT="no-cache,   must-revalidate">   
         <META   HTTP-EQUIV="expires"   CONTENT="Wed,   26   Feb   1978   08:21:57   GMT">   
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="JavaScript" src="JScript.js"></script>
		<script language="JavaScript" src="ShortKey.js"></script>
		<script language="JavaScript" src="Menu.js"></script>
		<script language=javascript >
		function calendar(ctrl)
        { 
          var url ='./Pub/CalendarHelp.htm';           
	      val=window.showModalDialog( url , '','dialogHeight: 335px; dialogWidth: 340px; center: yes; help: no'); 
	       if ( val==undefined)
	        return;
	      ctrl.value=val;
        }
		</script>
		<LINK href="./Style/Table.css" type="text/css" rel="stylesheet">
		<LINK href="./Style/Table0.css" type="text/css" rel="stylesheet">

		<base  target="_self" /> 
	</HEAD>
	<body onkeypress="Esc()" leftMargin="0" topMargin="0" onkeydown='DoKeyDown();'>
		<form id="Form1" method="post" runat="server">
			 <div class="Toolbar" >
                            <uc2:ToolBar ID="ToolBar2" runat="server" />
                            <uc2:ToolBar ID="ToolBar1" runat="server" />

             </div>
            <div >
             <uc1:ucen id="UCEn1" runat="server"></uc1:ucen>
             </div>
		</form>
	</body>
</HTML>