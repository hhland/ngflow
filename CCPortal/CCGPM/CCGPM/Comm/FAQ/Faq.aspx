<%@ Register TagPrefix="uc1" TagName="UCSys" Src="../UC/UCSys.ascx" %>
<%@ Page language="c#" Inherits="BP.Web.Comm.Port.UIFAQ" Codebehind="Faq.aspx.cs" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>FAQ</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Style.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="../JScript.js"></script>
		<LINK href="../Table.css" type="text/css" rel="stylesheet">
		<LINK href="../CSS/Link.css" type="text/css" rel="stylesheet">
		<script language=javascript>
     function DeleteDtl( oid)
     {
        alert(oid);
     }
     function Replay( msg )
     {
		   document.getElementById( 'TB_Docs' ).Text=msg;
     }
     function Edit( msg, id )
     {
     
     alert( msg);
     alert (id);
     
		   document.getElementById( 'TB_Docs' ).Text=msg;
		  alert( document.getElementById( 'TB_Docs' ).Text ;
		   
		   document.getElementById( 'TB_Dtl' ).Text=id;
      }
		    
 
      
		    
</script>

	</HEAD>
	<body leftMargin="0"  topMargin="0">
		<form id="Form1" method="post" runat="server">
			<FONT face="ËÎÌו">
				<TABLE id="Table1" style="LEFT: 0px; POSITION: absolute; TOP: 0px" height="100%" cellSpacing="1"
					cellPadding="1" width="100%" border="1">
					<TR>
						<TD><asp:label id="Label1" runat="server">Label</asp:label></TD>
					</TR>
					<TR>
						<TD vAlign="top" height="99%"><uc1:ucsys id="UCSys1" runat="server"></uc1:ucsys></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
