<%@ Page Title="2.  Set the report display columns " Language="C#" MasterPageFile="RptGuide.master" AutoEventWireup="true"
    CodeBehind="S2_ColsChose.aspx.cs" Inherits="CCFlow.WF.MapDef.Rpt.ColsChose" %>

<%@ Register Src="../Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Pub ID="Pub2" runat="server" />    
    <br />
    <br />
    <cc1:LinkBtn ID="Btn_Save1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-save'"
        Text=" Save " OnClick="Btn_Save_Click" />
    <cc1:LinkBtn ID="Btn_SaveAndNext1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-save'"
        Text=" Save and Continue " OnClick="Btn_SaveAndNext1_Click" />
    <cc1:LinkBtn ID="Btn_Cancel1" runat="server" IsPlainStyle="false" data-options="iconCls:'icon-undo'"
        Text=" Cancel " OnClick="Btn_Cancel_Click" />
</asp:Content>
