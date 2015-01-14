<%@ Page Title=" Launch " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="CCFlow.App.Start" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2> Launch </h2>
<%
   // Gets the collection process can be initiated .
   System.Data.DataTable dt= BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(BP.Web.WebUser.No);
   //  Output set .
   %>
   <table border="1">
   <tr>
   <th> Category </th>
   <th> Process </th>
   </tr>
     <%
    foreach (System.Data.DataRow dr in dt.Rows)
    {
        %>
        <tr>
        <td><%=dr["FK_FlowSortText"] %></td>

        <td> <a target=_blank href='/WF/MyFlow.aspx?FK_Flow=<%=dr["No"] %>' ><%=dr["Name"].ToString() %> </a>  </td>
        </tr>
   <% } %> 
   </table>
</asp:Content>
