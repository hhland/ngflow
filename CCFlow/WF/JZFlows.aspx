<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="JZFlows.aspx.cs" Inherits="CCFlow.WF.JZFlows" %>
<%@ Register src="UC/JZFlows.ascx" tagname="Flows" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<uc1:Flows ID="Flows1" runat="server" />
</asp:Content>
