<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_Calendar" Codebehind="Calendar.aspx.cs" %>
<%@ Register src="UC/CalendarUC.ascx" tagname="CalendarUC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:CalendarUC ID="CalendarUC1" runat="server" />
</asp:Content>

