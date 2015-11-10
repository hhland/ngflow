<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Page language="c#" AutoEventWireup="false" 
Inherits="BP.Web.Comm.Item3Ens" Codebehind="Item3Ens.aspx.cs" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>驰骋软件,值得信赖.</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<Meta http-equiv="Page-Enter" Content="revealTrans(duration=0.5, transition=8)">
		
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="JavaScript" src="JScript.js"></script>
		<script language="JavaScript" src="Menu.js"></script>
		<base target="_self" />
		<LINK href="./Style/Menu.css" type="text/css" rel="stylesheet">
		<LINK href="./Style/Table.css" type="text/css" rel="stylesheet">
		
		<script language=javascript>
		
		function Del(enName,refno)
		{
		   if (window.confirm('您确定要删除它吗？')==false)
		       return;
		
		    var url='Item3Do.aspx?EnName='+enName+'&RefPK='+refno;
            var b=window.showModalDialog( url , 'ass' ,'dialogHeight: 400px; dialogWidth: 600px; dialogTop: 200px; dialogLeft: 180px; center: yes; help: no'); 
            window.location.reload();
		}
		</script>
		<script language="javascript" for="document" event="onkeydown">
<!--
  if (window.event.srcElement.tagName="TEXTAREA") 
     return false;

  if(event.keyCode==13)
     event.keyCode=9;
     

-->
		</script>
<script language=javascript>
<!--
  function Edit( url )
  {
	 window.showModalDialog(url , 'card','dialogHeight: 600px; dialogWidth: 900px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no'); 
	 window.location.reload();
  }
//-->
</script>
	</HEAD>
	<body  topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1"  class='Table' cellSpacing="1" cellPadding="1" height='100%'
				width='95%' >
				<TR>
					<TD style="height: 18px">
						<asp:Label id="Label1" runat="server">Label</asp:Label></TD>
				</TR>
				<TR style="BORDER-TOP: activeborder solid">
					<TD height='100%' valign="top" style="BORDER-TOP: activeborder 1px solid">
						<uc1:UCSys id="UCSys1" runat="server"></uc1:UCSys>
                        <uc1:UCSys ID="UCSys2" runat="server" />
                    </TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
