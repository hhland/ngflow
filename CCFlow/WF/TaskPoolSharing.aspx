<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="TaskPoolSharing.aspx.cs" Inherits="CCFlow.WF.TaskPoolSmal" %>
<%@ Register src="UC/TaskPoolSharing.ascx" tagname="TaskPoolSharing" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:TaskPoolSharing ID="TaskPoolSharing1" runat="server" />
</asp:Content>
