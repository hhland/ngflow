<%@ Page Title=" The current form element Copy Other forms up to " Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="EleBatch.aspx.cs" Inherits="CCFlow.WF.MapDef.EleCopy" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="80%" >
    <tr>
    <td width="30%" valign=top >
    <fieldset>
    <legend> Processing content </legend>
        <uc1:Pub ID="Left" runat="server" />
        </fieldset>
        </td>
    <td><uc1:Pub ID="Pub1" runat="server" /></td>
    </tr>
    </table>
    

</asp:Content>
