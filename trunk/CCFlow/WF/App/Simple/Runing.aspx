<%@ Page Title=" In-transit list " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true"
    CodeBehind="Runing.aspx.cs" Inherits="CCFlow.App.Runing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //  Revocation .
        function UnSend(appPath, fk_flow, workid) {
            if (window.confirm(' Are you sure you want to cancel this transmission ?') == false)
                return;
            var url = appPath + 'WF/Do.aspx?DoType=UnSend&WorkID=' + workid + '&FK_Flow=' + fk_flow;
            window.location.href = url;
            return;
        }
        function Press(appPath, fk_flow, workid) {
            var url = appPath + 'WF/WorkOpt/Press.aspx?WorkID=' + workid + '&FK_Flow=' + fk_flow;
            var v = window.showModalDialog(url, 'sd', 'dialogHeight: 220px; dialogWidth: 430px;center: yes; help: no');
        }
        function CopyAndStart(appPath, fk_flow, CopyFormNode, CopyFormWorkID) {
            var url = appPath + 'WF/MyFlow.aspx?CopyFormWorkID=' + CopyFormWorkID + '&CopyFormNode=' + CopyFormNode + '&FK_Flow=' + fk_flow;
            var v = window.open(url, 'sd', 'dialogHeight: 220px; dialogWidth: 430px;center: yes; help: no');
        }

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
         In transit  (<%=BP.WF.Dev2Interface.Todolist_Runing %>)
    </h2>
    <%
        // Get in the way of work .
        System.Data.DataTable dt = null;
            if (BP.Sys.SystemConfig.AppSettings["IsAddCC"] == "1")
                dt = BP.WF.Dev2Interface.DB_GenerRuning();
            else
                dt = BP.WF.Dev2Interface.DB_GenerRuningAndCC();
        
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
            <th> Current treatment of people </th>
            <th> Operating </th>
        </tr>
        <%
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                string workid = dr["WorkID"].ToString();
                string fk_flow = dr["FK_Flow"].ToString();
                string nodeID = dr["FK_Node"].ToString();

        %>
        <tr>
            <td>
                <% if ( 1==1 || dr["Type"] + "" == "RUNNING")
                   { %>
                <a href="/WF/WFRpt.aspx?FK_Flow=<%= dr["FK_Flow"] %>&WorkID=<%= dr["WorkID"] %>"
                    target="_blank">
                    <%= dr["Title"] %>
                </a>
                <% }
                   else
                   { %>
                <a href='javascript:WinOpenCC("<%=dr["MyPk"] %> "," <%=dr["FK_Flow"] %>  ","<%=dr["FK_Node"] %> "," <%=dr["WorkID"] %> ","<%=dr["FID"] %>","<%=dr["Sta"] %>")'>
                    <%=dr["Title"] %></a>
                <% } %>
            </td>

            <td><%= dr["FlowName"] %></td>
            <td><%= dr["RDT"] %></td>
            <td><%= dr["StarterName"] %></td>
            <td><%= dr["NodeName"] %></td>
            <td><%= dr["TodoEmps"] %></td>
            <% if (1 == 1 ||  dr["Type"] + "" == "RUNNING")
               { %>
            <td>
                [<a href="javascript:UnSend('<%= path %>','<%= fk_flow %>','<%= workid %>')"> Send revocation </a>]
                -[<a href="javascript:Press('<%= path %>','<%= fk_flow %>','<%= workid %>')"> Reminders </a>]
                -[<a href="javascript:CopyAndStart('<%= path %>','<%= fk_flow %>','<%= nodeID %>','<%= workid %>')">Copy Launch </a>]
            </td>
            <% }
               else
               {%>
            <td>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>
</asp:Content>
