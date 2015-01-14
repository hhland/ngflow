<%@ Page Title="" Language="C#" MasterPageFile="../SDKComponents/Site.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_WorkOpt_ThreadDtl" Codebehind="ThreadDtl.aspx.cs" %>
<%@ Register src="../SDKComponents/ThreadDtl.ascx" tagname="ThreadDtl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ThreadDtl ID="ThreadDtl1" runat="server" />
</asp:Content>

