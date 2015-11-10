<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/3SheBei/Site.Master" AutoEventWireup="true" CodeBehind="S1_WangLuoFenXi.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleCom._3SheBei.S1_WangLuoFenXi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Click" />
    <asp:Button ID="Btn_Save" runat="server" Text=" Save " onclick="Btn_Click"/>
    <asp:Button ID="Btn_Return" runat="server" Text=" Return " onclick="Btn_Click" />
    <asp:Button ID="Btn_Track" runat="server" Text=" Locus " onclick="Btn_Click" />
    <asp:Button ID="Btn_Forward" runat="server" Text=" Forwarding " onclick="Btn_Click" />
    <hr />
    
<h3> Initiate a separate sub-sub process window </h3>
<hr />
        <fieldset>
        <legend> Sub-sub-processes basic information </legend>
        <table border=1>
          <tr>
           <td> Field </td>
           <td> Field Description </td>
           <td>值</td>
         </tr>
        <%
            int shebeiID = int.Parse(this.Request.QueryString["SheBeiID"]);
            
            BP.Demo.tab_wf_commonkpioptivalue en = new BP.Demo.tab_wf_commonkpioptivalue();
            en.OID = shebeiID;
            en.Retrieve(); // According Equipment ID  Check out the information on the device .
            
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
               <asp:CheckBox ID="CB_IsShiShi" Text=" The need to develop programs ( This value determines the direction of the sub-sub-processes )" runat="server" />
               <br> The person in charge of the device :
              <asp:TextBox ID="TB_FZR" Text="fuhui" runat="server"></asp:TextBox>( This value determines the sub-sub-processes of handling people )
</asp:Content>
