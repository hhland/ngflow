﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/TeleCom/2DiShi/Site.Master" AutoEventWireup="true" CodeBehind="CallSubFlow.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TeleCom._2DiShi.CallSubFlow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h3> Sub-sub-processes ( Equipment maintenance processes - Start node )</h3>
<asp:Button ID="Btn_Send" runat="server" Text=" Send sub-sub-processes " 
        onclick="Btn_Send_Click" />

  <asp:CheckBox ID="CB_IsShiShi" Text=" The need to develop programs ( This value determines the direction of the sub-sub-processes )" runat="server" />
               <br> Staff next node :
              <asp:TextBox ID="TB_FZR" Text="fuhui" runat="server"></asp:TextBox>( This value determines the sub-sub-processes of handling people )
 This step function to send a single sub-sub-processes .
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
             

</asp:Content>
