<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="BatchStartOne.aspx.cs" Inherits="CCFlow.WF.BatchStart" %>
<%@ Register src="UC/BatchStart.ascx" tagname="BatchStart" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:BatchStart ID="BatchStart1" runat="server" />
</asp:Content>

