<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/1ShengJu/Site1.Master" AutoEventWireup="true" CodeBehind="S2_WatherSubFlow.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleComDemo.ShengJu.S2_WatherSubFlow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2> Provincial Bureau monitors the progress of each of the Council sub-processes (<font color=red > If the process is not completed various cities , It can not be submitted .</font>)</h2>
 <asp:Button ID="Btn_Send" runat="server" Text=" Submitted for review " onclick="Btn_Send_Click" />
 <%
     /* To obtain data from the sub-process business master table , To view the sub-processes of change  . */
    
     string sql = "SELECT a.*, b.wf_no, b.wf_send_user,b.region_id FROM WF_GenerWorkFlow a,  tab_wf_commonkpiopti b WHERE a.WorkID=b.WorkID and a.PWorkID=" + this.Request.QueryString["WorkID"];
     System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
    
   %>

   <fieldset>
   <legend> Data source acquisition sql</legend>
   <%=sql %>

  <br>
  <hr />
   Explanation : Business Process Engine Table Table and associated by , You can control the state of the formation of business process data .
   </fieldset>

     <table border=1 >
     <caption> Sub-processes operating information </caption>
     <tr>
     <th>WorkID</th>
     <th> Process Status </th>
     <th>BillNo</th>
     <th> Title </th>
     <th>region_id</th>
     <th> Processors </th>
     <th> Operating </th>

     </tr>
     <%
     foreach (System.Data.DataRow dr in dt.Rows)
     {
  %>
     <tr>
     <td><% =dr["WorkID"].ToString() %></td>
     <td><% =dr["WFState"].ToString()%></td>
     <td><% =dr["wf_no"].ToString()%></td>
     <td><% =dr["Title"].ToString() %></td>
     <td><% =dr["region_id"].ToString()%></td>
     <td><% =dr["wf_send_user"].ToString()%></td>
     <td><a href='Do.aspx?DoType=DelSubFlow&WorkID=<% =dr["WorkID"].ToString()%>&FK_Flow=<%=dr["FK_Flow"] %>' > Delete sub-processes </a></td>
     </tr>

  <% } %>
     </table>
</asp:Content>
