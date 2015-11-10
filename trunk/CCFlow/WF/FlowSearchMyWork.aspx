<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_FlowSearchMyWork" Codebehind="FlowSearchMyWork.aspx.cs" %>
<%@ Register src="UC/FlowSearchMyWork.ascx" tagname="FlowSearchMyWork" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<script language="JavaScript" src="./Comm/JS/Calendar/WdatePicker.js" defer="defer" ></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:FlowSearchMyWork ID="FlowSearchMyWork1" runat="server" />
</asp:Content>

