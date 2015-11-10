<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/Telecom/1ShengJu/Site1.Master" AutoEventWireup="true" CodeBehind="S1_Start.aspx.cs" Inherits="CCFlow.SDKFlowDemo.TelecomDemo.Parent.S1_Start" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Btn_Send" runat="server" Text=" Initiate work orders " onclick="Btn_Send_Click" />
    <asp:Button ID="Btn_Save" runat="server" Text=" Save Ticket " onclick="Btn_Save_Click" />
    <asp:Button ID="Btn_FlowChat" runat="server" Text=" Flow chart " onclick="Btn_Chat_Click" />

    <hr />

    <fieldset>
    <legend> Ticket Master table information </legend>
     Sponsor :<asp:TextBox ID="TB_FaQiRen" Text="zhangshan"  runat="server"></asp:TextBox>
     The time required to complete :<asp:TextBox ID="TB_SDT" Text="2013-01-04" runat="server"></asp:TextBox>
    <br />
     Index Name :<asp:TextBox ID="TB_ZBName" Text=" Drop rate " runat="server"></asp:TextBox>
     Index value :<asp:TextBox ID="TB_ZBVal" Text="10%" runat="server"></asp:TextBox>

    <br />
     Document Number :<asp:TextBox ID="TB_wf_no" Text="10%" runat="server"></asp:TextBox>
     Start Time :<asp:TextBox ID="TB_wf_send_time" Text="10%" runat="server"></asp:TextBox>
    </fieldset>

    <fieldset>
    <legend> Ticket information from the table ( Each message is a sub-process , People who deal with cities responsible for the sub-process is the first node )</legend>
         <table border=1 width='100%' >
          <tr>
         <th> Cities </th>
         <th> Person in charge of cities </th>
         <th> Document Number </th>
         <th> Status </th>
         <th> Editing Device Information </th>
         </tr>
 
         <tr>
         <td><input value=' Jinan ' type=text  /></td>
         <td><input value='zhoutianjiao' type=text  /></td>
         <td><input value='111-222-333' type=text  /></td>
         <td> Initialization </td>
         <td> Equipment </td>
         </tr>
         
         <tr>
         <td colspan=5 >
         <table border="1" width='60%' >
         <caption > Device Information ( From the table from the table , Each message is a sub-sub-processes )</caption>
         <tr>
         <th> Device Number </th>
         <th> Equipment Leader </th>
         <th> Device Location </th>
         </tr>

         <tr>
         <td>abc-abc</td>
         <td>guobaogeng</td>
         <td> Jinan High-tech Zone xx路xx号</td>
         </tr>

          <tr>
         <td>abc-123</td>
         <td>fuhui</td>
         <td> Jinan Licheng District xx路xx号</td>
         </tr>
         </table>

         </td>
         </tr>

     </table>
    </fieldset>

    <h3> Provincial Bureau initiate work orders - After sending the Council mission and start , Upcoming let PUC work in state .</h3>
</asp:Content>
