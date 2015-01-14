<%@ Page Title=" Process Jump " Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" CodeBehind="FlowSkip.aspx.cs" Inherits="CCFlow.WF.Admin.WatchOneFlow.FlowSkip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<fieldset>
<legend> Process Jump 
    
    </legend>
     Jump to the node :<asp:DropDownList ID="DDL_SkipToNode" runat="server">
    </asp:DropDownList>

    <br> Go to ( Please enter the personnel numbers ):
 <asp:TextBox ID="TB_SkipToEmp" 
    Rows=5 runat="server"></asp:TextBox>

     <br>
 Jump reason :
     <br>
<asp:TextBox ID="TB_Note" TextMode="MultiLine" 
    Rows=5 runat="server" Width="308px"></asp:TextBox>
   
     <br>

    <asp:Button ID="Btn_OK" runat="server" Text=" OK Go " onclick="Btn_OK_Click" />
    <asp:Button ID="Btn_Cancel" runat="server" Text=" Cancel " 
        onclick="Btn_Cancel_Click" />

</fieldset>
</asp:Content>
