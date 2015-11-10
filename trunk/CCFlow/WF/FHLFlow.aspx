<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_FHLFlow" Codebehind="FHLFlow.aspx.cs" %>
<%@ Register src="UC/FHLFlow.ascx" tagname="FHLFlow" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:FHLFlow ID="FHLFlow1" runat="server" />
</asp:Content>

