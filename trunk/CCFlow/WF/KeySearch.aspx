<%@ Page Title=" Keyword Search " Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_KeySearch" Codebehind="KeySearch.aspx.cs" %>
<%@ Register src="UC/KeySearch.ascx" tagname="KeySearch" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:KeySearch ID="KeySearch1" runat="server" />
</asp:Content>

