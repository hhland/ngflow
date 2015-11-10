<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" CodeBehind="S7_PowerOfDept.aspx.cs" Inherits="CCFlow.WF.MapDef.Rpt.S7_PowerFlowDeptEmp" %>
<%@ Register src="../Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 Select the type of control :
<asp:DropDownList ID="DDL_DeptPowerType" runat="server">
    <asp:ListItem Value="0"> The department can only view </asp:ListItem>
    <asp:ListItem Value="1"> View this sector and lower sector </asp:ListItem>
    <asp:ListItem Value="2"> The process according to the specified department personnel access control </asp:ListItem>
    <asp:ListItem Value="3"> Does not control , Anyone can view the data in any sector .</asp:ListItem>
    </asp:DropDownList>
    
SELECT * FROM WF_DeptFlowSearch
<uc1:Pub ID="Pub2" runat="server" />
     
    <br />
    <asp:Button ID="Btn_Save" runat="server" Text="SaveAndClose" OnClick="Btn_Save_Click" />
    <asp:Button ID="Btn_SaveAndNext" runat="server" Text="SaveAndNext" OnClick="Btn_Save_Click" />
    <asp:Button ID="Btn_Close" runat="server" Text="Cancel" 
    onclick="Btn_Close_Click" />

</asp:Content>
