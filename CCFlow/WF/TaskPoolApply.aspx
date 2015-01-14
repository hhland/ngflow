<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="TaskPoolApply.aspx.cs" Inherits="CCFlow.WF.TaskPoolApplySmall" %>
<%@ Register src="UC/TaskPoolApply.ascx" tagname="TaskPoolApply" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:TaskPoolApply ID="TaskPoolApply1" runat="server" />
</asp:Content>
