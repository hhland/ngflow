<%@ Page Title=" Job transfer " 
Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true"
 CodeBehind="FlowShift.aspx.cs" Inherits="CCFlow.WF.Admin.WatchOneFlow.FlowShift" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
  <fieldset>
  <legend> Select this work should work transferred staff </legend>

   Please enter the personnel numbers :<asp:TextBox ID="TB_Emp" runat="server" Width="159px">
  </asp:TextBox>
  <br>
   The reason :
  <br>

  <asp:TextBox ID="TB_Note" TextMode=MultiLine runat="server" Width="336px" 
          Height="91px"></asp:TextBox>
   Explanation : You can only enter a personnel number .
&nbsp;<hr>

      <asp:Button ID="Btn_OK" runat="server" Text=" Determining transfer " onclick="Btn_OK_Click" />
      <asp:Button ID="Btn_Cancel" runat="server" Text=" Cancel and close " 
          onclick="Btn_Cancel_Click" />

  </fieldset>
</asp:Content>
