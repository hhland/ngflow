<%@ Page Title="" Language="C#" MasterPageFile="SDKComponents/Site.Master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_MyFlowInfoSmall" Codebehind="MyFlowInfo.aspx.cs" %>
<%@ Register src="UC/MyFlowInfo.ascx" tagname="MyFlowInfo" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="./Comm/JScript.js" type="text/javascript" ></script>
    <link href="Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript" >
    if (window.opener && !window.opener.closed) {
        if (window.opener.name == "main") {
            window.opener.location.href = window.opener.location.href;
            window.opener.top.leftFrame.location.href = window.opener.top.leftFrame.location.href;
        }
    }
</script>
 
<table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td>
<uc2:MyFlowInfo ID="MyFlowInfo1" runat="server"  />
 </td>
</tr>
</table>
 
</asp:Content>
