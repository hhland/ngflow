<%@ Page Title=" Pending " Language="C#" MasterPageFile="../SDKComponents/Site.master"
 AutoEventWireup="true" Inherits="CCFlow.WF.WF_Hurry" Codebehind="HungUp.aspx.cs" %>
<%@ Register src="../UC/Pub.ascx" tagname="Pub" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <table style="text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td  style=" text-align:center">
    <br>



 <table style=" text-align:left; width:500px">
 <tr>
 <td>
    <uc2:Pub ID="Pub1" runat="server" />
    </td>
 </tr>
 </table>





    <br>
    </td>
</tr>
</table>
</asp:Content>

