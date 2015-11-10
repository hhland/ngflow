<%@ Page Title="" Language="C#" MasterPageFile="SDKComponents/Site.Master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_JumpWay" Codebehind="JumpWay.aspx.cs" %>

<%@ Register src="UC/JumpWay.ascx" tagname="JumpWay" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:JumpWay ID="JumpWay1" runat="server" />
</asp:Content>

