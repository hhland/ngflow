<%@ Page Title=" Upcoming " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true" CodeBehind="ToDoList.aspx.cs" Inherits="CCFlow.App.ToDoList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2> Upcoming  (<%=BP.WF.Dev2Interface.Todolist_EmpWorks %>) </h2>
<%
   // Get Upcoming .
   System.Data.DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();
   //  Output 
   %> <table border="1" widht="90%" >
   <tr>
   <th> Whether read ?</th>
   <th> Title </th>
   <th> Process </th>
   <th> Start Time </th>
   <th> Sponsor </th>
   <th> Stay node </th>
   <th>( Send / Cc )</th>
   <th> Type </th>
   </tr>
      <%
    //  Generate timke  The old way to open a browser interface , Cache mode interface .      
    string t = DateTime.Now.ToString("MMddhhmmss");
    foreach (System.Data.DataRow dr in dt.Rows)
    {
        // Parameters of the process engine .
        string paras = dr["AtPara"] as string;
        if (paras == null)
            paras = "";
        // This work is read ? You can personalize the display of work according to the state read unread effect Developers .
        int isRead = int.Parse(dr["IsRead"].ToString());
        
        %>
        <tr>
        <td><%=dr["isRead"]%></td>
       
       <% if (isRead == 0)
          {%>
        <td><b> <a target="_blank" href='/WF/MyFlow.aspx?FK_Flow=<%=dr["FK_Flow"] %>&FK_Node=<%=dr["FK_Node"] %>&WorkID=<%=dr["WorkID"] %>&FID=<%=dr["FID"] %>&IsRead=<%=isRead%>&Paras=<%=paras %>&T=<%=t %>' ><%=dr["Title"].ToString()%> </a>  </b></td>
        <% }
          else
          { %>
        <td><a target="_blank" href='/WF/MyFlow.aspx?FK_Flow=<%=dr["FK_Flow"] %>&FK_Node=<%=dr["FK_Node"] %>&WorkID=<%=dr["WorkID"] %>&FID=<%=dr["FID"] %>&IsRead=<%=isRead%>&Paras=<%=paras %>&T=<%=t %>' ><%=dr["Title"].ToString()%> </a>  </td>
        <%} %>

        <td><%=dr["FlowName"]%></td>
        <td><%=dr["RDT"]%></td>
        <td><%=dr["StarterName"]%></td>
        <td><%=dr["NodeName"]%></td>

       
   <td><%=dr["Sender"]%></td>


        <% if (paras.Contains("IsCC")) { %>

        <td> Cc </td>

        <% } else { %>
        
        <td> Send </td>
        
        <%} %>


        </tr>
   <% } %> 
   </table>

</asp:Content>
