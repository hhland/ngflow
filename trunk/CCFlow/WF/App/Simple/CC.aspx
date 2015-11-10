<%@ Page Title=" Cc " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true" CodeBehind="CC.aspx.cs" Inherits="CCFlow.App.CC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2>  Cc   </h2>
<%
    string ShowType = this.Request.QueryString["ShowType"];
    if (ShowType == null)
        ShowType = "0";
    
    // Get a copy to work .
    System.Data.DataTable dt = null;
    if (ShowType == "0")
    {
        /* Cc unread */
        dt = BP.WF.Dev2Interface.DB_CCList_UnRead(BP.Web.WebUser.No);
    }
    if (ShowType == "1")
    {
        /* Read the Cc */
        dt = BP.WF.Dev2Interface.DB_CCList_Read(BP.Web.WebUser.No);
    }
    if (ShowType == "2")
    {
        /* Deleted Cc */
        dt = BP.WF.Dev2Interface.DB_CCList_Delete(BP.Web.WebUser.No);
    }
    
   //  Output 
   %>
    [<a href='CC.aspx?ShowType=0'> Unread </a>][<a href='CC.aspx?ShowType=1'> Read </a>][<a href='CC.aspx?ShowType=2'> Delete </a>]
   <hr>
    <table border=1 widht='90%'>
   <tr>
   <th> Cc </th>
   <th> Title </th>
   <th> Process </th>
   <th> Start Time </th>
   <th> Details </th>
   </tr>
   
      <%
    foreach (System.Data.DataRow dr in dt.Rows)
    {
        string workid = dr["WorkID"].ToString();
        string fid = dr["FID"].ToString();
        string fk_flow = dr["FK_Flow"].ToString();
        string fk_node = dr["FK_Node"].ToString();

        string url = "/WF/WFRpt.aspx?FK_Flow="+fk_flow+"&FK_Node="+fk_node+"&WorkID="+workid+"&FID="+fid;
        
        %>
        <tr>
        <td><%=dr["Rec"]%></td>
        <td><%=dr["FlowName"]%></td>
        <td><%=dr["RDT"]%></td>
        <td><%=dr["NodeName"]%></td>
        <td><a href='<%=url %>' target="_blank" > Detailed </a></td>
        </tr>

        <tr>
        <td colspan=5 >
         Title :<%=dr["Title"] %>
        <hr />
         Cc content :
        <hr>
        
        <%=dr["Doc"]%>
        
        </td>

        </tr>


   <% } %> 
   </table>


</asp:Content>
