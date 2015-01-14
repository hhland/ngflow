<%@ Page Title="1.  Basic Information " Language="C#" MasterPageFile="RptGuide.Master" AutoEventWireup="true"
    CodeBehind="S1_Edit.aspx.cs" Inherits="CCFlow.WF.MapDef.Rpt.NewOrEdit" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class='Table' cellpadding='0' cellspacing='0' style='width: 100%;'>
        <tr>
            <td class="GroupTitle">
                 Serial number :
            </td>
            <td>
                <asp:TextBox ID="TB_No" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="GroupTitle">
                 Name :
            </td>
            <td>
                <asp:TextBox ID="TB_Name" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="GroupTitle">
                 Remark :
            </td>
            <td>
                <asp:TextBox ID="TB_Note" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <cc1:LinkBtn ID="Btn_Save1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-save'"
        Text=" Save " OnClick="Btn_Save_Click" />
    <cc1:LinkBtn ID="Btn_SaveAndNext1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-save'"
        Text=" Save and Continue " OnClick="Btn_SaveAndNext1_Click" />
    <cc1:LinkBtn ID="Btn_Cancel1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-undo'"
        Text=" Cancel " OnClick="Btn_Cancel_Click" />
</asp:Content>
