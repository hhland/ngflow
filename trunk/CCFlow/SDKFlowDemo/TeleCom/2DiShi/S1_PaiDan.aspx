<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/2DiShi/Site.Master" AutoEventWireup="true" CodeBehind="S1_PaiDan.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleCom._2DiShi.S1_PaiDan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Send_Click" 
           style="width: 40px"  />
        <asp:Button ID="Btn_Save" runat="server" Text=" Save " onclick="Btn_Save_Click" />
        <asp:Button ID="Btn_Track" runat="server" Text=" Trajectories " onclick="Btn_Track_Click" />
        <hr />
        <%
            BP.Demo.tab_wf_commonkpiopti pti = new BP.Demo.tab_wf_commonkpiopti();
            pti.Retrieve(BP.Demo.tab_wf_commonkpioptiAttr.WorkID, this.WorkID);
             %>
             <fieldset>
             <legend> Subprocess master table data </legend>
        <table border=1 >
        <caption></caption>
        <tr>
        <td> Document Number :<%=pti.wf_no %></td>
        <td> City :<%=pti.region_id %></td>
        <td> Person in charge :<%=pti.wf_send_user%></td>
        </tr>
        </table>
            </fieldset>
        <%
            Int64 workid = Int64.Parse(this.Request.QueryString["WorkID"]);
            BP.Demo.tab_wf_commonkpioptivalues shebeis = new BP.Demo.tab_wf_commonkpioptivalues();
            shebeis.Retrieve(BP.Demo.tab_wf_commonkpioptivalueAttr.ParentWorkID, workid);
             %>

             <fieldset>
             <legend> Device Information : Person in charge of the recipient is a child thread , Several detail records there are several sub-thread , These devices are the parent process information over the data collection .</legend>
             <table border=1 width="70%">
             <tr>
             <td> Person in charge </td>
             <td>fk_flow</td>
             <td> Device Address </td>
             </tr>
             <%
               foreach (BP.Demo.tab_wf_commonkpioptivalue shebei in shebeis)
               {
                   %>
                   <tr>
                   <td><%=shebei.fuzeren %></td>
                   <td><%=shebei.fk_flow %></td>
                   <td><%=shebei.addr %></td>
                   </tr>
                  
               <% } %>
             </table>

            </fieldset>


</asp:Content>
