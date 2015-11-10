<%@ Page Title=" Return to work " Language="C#" MasterPageFile="../SDKComponents/Site.Master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_ReturnWorkSmall" Codebehind="ReturnWork.aspx.cs" %>
<%@ Register src="../UC/ReturnWork.ascx" tagname="ReturnWork" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="../Comm/JScript.js"></script>
    <script type="text/javascript">
        function NoSubmit(ev) {
            if (window.event.srcElement.tagName == "TEXTAREA")
                return true;
            if (ev.keyCode == 13) {
                window.event.keyCode = 9;
                ev.keyCode = 9;
                return true;
            }
            return true;
        }
</script>
    <link href="../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td>

    <uc1:ReturnWork ID="ReturnWork1" runat="server" />

    </td>
</tr>
</table>

</asp:Content>

