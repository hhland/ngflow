<%@ Page Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.WF_MapDef_GroupField" Title=" Unnamed page " Codebehind="GroupField.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"  >
<script language="javascript" >
   function Del( refNo, refOID )
	{
	 if ( window.confirm(' Are you sure you want to delete it ? ') == false ) 
	      return false;
	   window.location.href='GroupField.aspx?RefNo='+ refNo +'&DoType=DelGF&RefOID=' + refOID;
    }
</script>
<base target=_self />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Pub ID="Pub1" runat="server" />
</asp:Content>

