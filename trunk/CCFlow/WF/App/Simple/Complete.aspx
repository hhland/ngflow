<%@ Page Title=" Completed list " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true"
    CodeBehind="Complete.aspx.cs" Inherits="CCFlow.App.Complete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function WinOpenCC(ccid, fk_flow, fk_node, workid, fid, sta) {
            var url = '';
            if (sta == '0') {
                url = 'WF/Do.aspx?DoType=DoOpenCC&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&WorkID=' + workid + '&FID=' + fid + '&Sta=' + sta + '&MyPK=' + ccid + "&T=" + dateNow;
            }
            else {
                url = 'WF/WorkOpt/OneWork/Track.aspx?FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&WorkID=' + workid + '&FID=' + fid + '&Sta=' + sta + '&MyPK=' + ccid + "&T=" + dateNow;
            }
            //window.parent.f_addTab("cc" + fk_flow + workid, " Cc " + fk_flow + workid, url);
            var newWindow = window.open(url, 'z');
            newWindow.focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>
         Completed  (<%=BP.WF.Dev2Interface.Todolist_Complete %>)
    </h2>
    <%
        // Gets the job done .
        System.Data.DataTable dt =null;
        try
        {
            if (BP.Sys.SystemConfig.AppSettings["IsAddCC"] == "1")
                dt = BP.WF.Dev2Interface.DB_FlowComplete();
            else
                dt = BP.WF.Dev2Interface.DB_FlowCompleteAndCC();

        }
        catch (Exception ex)
        {
            dt =  BP.WF.Dev2Interface.DB_FlowComplete();

        }
        string path = this.Request.ApplicationPath;

        //  Output 
    %>
    <table border="1" widht='90%'>
        <tr>
        <th> Title </th>
        <th> Process </th>
        <th> Start Time </th>
        <th> Sponsor </th>
        <th> Stay node </th>
        <th> Operating </th>
        </tr>
        <%
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                string workid = dr["WorkID"].ToString();
                string fk_flow = dr["FK_Flow"].ToString();
        %>
        <tr>
            <td>
                <% if (dr["Type"] + "" == "RUNNING")
                   { %>
                <a href="/WF/WFRpt.aspx?FK_Flow=<%=dr["FK_Flow"] %>&WorkID=<%=dr["WorkID"] %>" target="_blank">
                    <%=dr["Title"]%>
                </a>
                <% }
                   else
                   { %>
                <a href='javascript:WinOpenCC("<%=dr["MyPk"] %> "," <%=dr["FK_Flow"] %>  ","<%=dr["FK_Node"] %> "," <%=dr["WorkID"] %> ","<%=dr["FID"] %>","<%=dr["Sta"] %>")'>
                    <%=dr["Title"] %></a>
                <% } %>
            </td>
            <td>
                <%=dr["FlowName"]%>
            </td>
            <td>
                <%=dr["RDT"]%>
            </td>
            <td>
                <%=dr["StarterName"]%>
            </td>
            <td>
                <%=dr["NodeName"]%>
            </td>
            <% if (dr["Type"] + "" == "RUNNING")
               { %>
            <td>
                <a href="/WF/MyFlow.aspx?FK_Flow=<%= dr["FK_Flow"] %>&CopyFormWorkID=<%= dr["WorkID"] %>&CopyFormNode=<%= dr["FK_Node"] %>"
                    target="_blank">Copy Initiate the process </a>
            </td>
            <% }
               else
               { %>
            <td>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>
</asp:Content>
