<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" 
Inherits="CCFlow.WF.WF_CC" Codebehind="CC.aspx.cs" %>
<%@ Register src="UC/CC.ascx" tagname="CC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:CC ID="CC1" runat="server" />
</asp:Content>

