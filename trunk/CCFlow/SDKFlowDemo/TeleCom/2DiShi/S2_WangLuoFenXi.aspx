<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/2DiShi/Site.Master" AutoEventWireup="true" CodeBehind="S2_WangLuoFenXi.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleCom._2DiShi.S2_WangLuoFenXi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    //  Function execution .
    function CallSubFlow(shebeiID, fk_flow, fk_node, parentWorkid, fid) {
        var url = "CallSubFlow.aspx?SheBeiID=" + shebeiID + '&ParentWorkID=' + parentWorkid + '&FID=' + fid;
        var newWindow = window.open(url, 'z', 'scrollbars=yes,resizable=yes,toolbar=false,location=false');
        newWindow.focus();
        return;
    }

    //  Processing work to be done .
    function DealSubFlow(SheBeiID,workid) {
        var url = 'DealSubFlow.aspx?SheBeiID=' + SheBeiID + '&WorkID=' + workid + '&FK_Flow=027&FK_Node=2701&FID=0';
        var newWindow = window.open(url, 'z', 'scrollbars=yes,resizable=yes,toolbar=false,location=false');
        newWindow.focus();
        return;
    }

    //  Open flow trajectories .
    function Chart(fk_flow, fk_node, workid, fid) {
        var url = "/WF/Chart.aspx?FK_Flow=" + fk_flow + '&FK_Node=' + fk_node + '&WorkID=' + workid + '&FID=' + fid;
       // alert(' To enable the child thread :' + url);
        var newWindow = window.open(url, 'z', 'scrollbars=yes,resizable=yes,toolbar=false,location=false');
        newWindow.focus();
        return;
    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3> Urban Network Analysis node :( Each device must initiate a sub-process , Only sub-processes of each device is fully processed to complete this work to the downward movement of the nodes )</h3>
<br />
<asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Send_Click" 
           style="width: 40px"  />
        <asp:Button ID="Btn_Save" runat="server" Text=" Save " onclick="Btn_Save_Click" />
        <asp:Button ID="Btn_Track" runat="server" Text=" Trajectories " onclick="Btn_Track_Click" />
        <hr />
        <%
            Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
            BP.Demo.tab_wf_commonkpiopti pti = new BP.Demo.tab_wf_commonkpiopti();
            pti.Retrieve(BP.Demo.tab_wf_commonkpioptiAttr.WorkID, workid);
         %>
             <fieldset>
             <legend> Cities flow master table data </legend>
        <table border=1 >
        <tr>
        <td> Document Number :<%=pti.wf_no %></td>
        <td> City :<%=pti.region_id %></td>
        <td> Person in charge :<%=pti.wf_send_user%></td>
        </tr>
        </table>
            </fieldset>
        <%
            string fk_flow = this.Request.QueryString["FK_Flow"];
            int fk_node = int.Parse(this.Request.QueryString["FK_Node"]);
            Int64 fid = Int64.Parse(this.Request.QueryString["FID"]);

            string sql = "select * from tab_wf_commonkpioptivalue  where  ParentWorkID=" + workid;
            System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            
             %>

             <fieldset>
             <legend> Local Municipal Equipment Information :( Different status of the device , Not the same as the operation performed )</legend>
             <table border="1" width="70%">
             <tr>
             <th> Person in charge </th>
             <th> Device Address </th>
             <th>ParentWorkID</th>
             <th>WorkID</th>
             <th> Process Status </th>
             <th> Child thread stays node </th>
             <th> Child thread stays node ID</th>
             <th> Operating </th>
             </tr>
             <%
                 
                 // Traverse device information table .
                 foreach (System.Data.DataRow dr in dt.Rows)
                 {
                     string shebeiID = dr[BP.Demo.tab_wf_commonkpioptivalueAttr.OID].ToString();
                     string fuzeren = dr[BP.Demo.tab_wf_commonkpioptivalueAttr.fuzeren].ToString();
                     string addr = dr[BP.Demo.tab_wf_commonkpioptivalueAttr.addr].ToString();
                     
                     int sheBeiWorkID = int.Parse(dr[BP.Demo.tab_wf_commonkpioptivalueAttr.WorkID].ToString());
                     int parentWorkID = int.Parse(dr[BP.Demo.tab_wf_commonkpioptivalueAttr.ParentWorkID].ToString());
                     
                     BP.WF.Data.GERpt gerpt=null;
                     BP.WF.WFState wfState = BP.WF.WFState.Draft;
                     
                     string nodeStop = "无";
                     int flowEndNodeID = 0;
                     if (sheBeiWorkID != 0)
                     {
                         gerpt = BP.WF.Dev2Interface.Flow_GenerGERpt("027", sheBeiWorkID);
                         wfState = gerpt.WFState;
                         nodeStop = gerpt.FlowEndNodeText;
                         flowEndNodeID = gerpt.FlowEndNode;
                     }
                   %>
                   <tr>
                   <td><%= fuzeren%></td>
                   <td><%=addr%></td>
                   <td><%=parentWorkID%></td>
                   <td><%=sheBeiWorkID%></td>
                   <td><%=wfState.ToString()%></td>
                   <td><%=nodeStop%> </td>
                   <td><%=flowEndNodeID%> </td>
                   <td>
                  <% if (sheBeiWorkID == 0)
                     {  /*  This device does not originate sub-processes  */ %>
                   <a  href="javascript:CallSubFlow('<%= shebeiID %>', '027', '<%=fk_node %>', '<%=workid %>', '<%=fid %>');" > Start equipment maintenance processes </a>
                   <% }  %>


                    <% if (sheBeiWorkID != 0)
                       {  /* This device does not originate sub-processes */ %>

                      <a  href="javascript:Chart( '027', '<%=fk_node %>', '<%=subFlowWorkID %>', '<%=fid %>');" > Trajectories </a>

                   <% }  %>


                     <% if (sheBeiWorkID != 0 && gerpt.FlowEndNode == 2701)
                        {  /* If the node is the start node stays , Just let it handle the work ..*/ %>
                      <a  href="javascript:DealSubFlow('<%=shebeiID %>','<%=sheBeiWorkID %>');" ><b> Working deal </b></a>
                     <% }  %>
                   </td>
                   </tr>
               <% } %>
             </table>
            </fieldset>
</asp:Content>