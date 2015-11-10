<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/3SheBei/Site.Master" AutoEventWireup="true" CodeBehind="S3_FangAnShenHe.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleCom._3SheBei.S3_FangAnShenHe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Click" />
    <asp:Button ID="Btn_Save" runat="server" Text=" Save " onclick="Btn_Click"/>
    <asp:Button ID="Btn_Return" runat="server" Text=" Return " onclick="Btn_Click" />
    <asp:Button ID="Btn_Track" runat="server" Text=" Locus " onclick="Btn_Click" />
    <asp:Button ID="Btn_Forward" runat="server" Text=" Forwarding " onclick="Btn_Click" />
    <hr />
        <fieldset>
        <legend> Basic information about the device processes </legend>
        <table border=1>
          <tr>
           <td> Field </td>
           <td> Field Description </td>
           <td>值</td>
         </tr>
        <%
            int workID = int.Parse(this.Request.QueryString["WorkID"]);
            
            BP.Demo.tab_wf_commonkpioptivalue en = new BP.Demo.tab_wf_commonkpioptivalue();
            en.WorkID = workID;
            en.Retrieve(BP.Demo.tab_wf_commonkpioptivalueAttr.WorkID,workID); // According Equipment ID  Check out the information on the device .
            foreach (BP.En.Attr  item in en.EnMap.Attrs)
            {
            %>
           <tr>
           <td> <% =item.Key  %> </td>
           <td> <% = item.Desc %> </td>
           <td><% =en.GetValStrByKey(item.Key) %></td>
            </tr>
            <% }%>
            </table>
        </fieldset>
        <hr />
               <br> Staff in the next step of :
              <asp:TextBox ID="TB_NextWorker" Text="fuhui" runat="server"></asp:TextBox>
    <hr />

</asp:Content>
