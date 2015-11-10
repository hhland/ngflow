<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_StartSmall" Codebehind="Start.aspx.cs" %>

<%@ Register src="UC/EmpWorks.ascx" tagname="EmpWorks" tagprefix="uc1" %>

<%@ Register src="UC/Start.ascx" tagname="Start" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="./Comm/JScript.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc2:Start ID="Start1" runat="server" />
</asp:Content>

