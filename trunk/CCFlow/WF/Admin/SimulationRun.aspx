<%@ Page Title=" Process simulation run " Language="C#" MasterPageFile="~/WF/Admin/WinOpen.master" AutoEventWireup="true" CodeBehind="SimulationRun.aspx.cs" Inherits="CCFlow.WF.Admin.SimulationRun" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<fieldset>
<legend> Simulation run </legend>
<div style=" text-align:center ">
<table>
<tr>
<td>
    <uc1:Pub ID="Pub1" runat="server" />
    </td>
    <td>
    <uc1:Pub ID="Pub2" runat="server" />
    </td>
    </tr>
</table>

    </div>
    </fieldset>
</asp:Content>
