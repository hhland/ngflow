<%@ Page Title=" Reply endorsement " Language="C#" MasterPageFile="../SDKComponents/Site.Master" AutoEventWireup="true" CodeBehind="AskForRe.aspx.cs" Inherits="CCFlow.WF.WorkOpt.AskForRe" %>
<%@ Register src="../Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td valign=top style=" text-align:center">
    <br>
    <br>
    

 <table style=" text-align:left; width:500px">
 <tr>
 <th> Fill Reply Comment </th>
 </tr>
<tr>
<td valign=top style="text-align:center">
        <uc1:Pub ID="Pub1" runat="server" />
         </td>
         </tr>
         </table>

    
    
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    <br>
    
    </td>
</tr>
</table>
</asp:Content>
