<%@ Page Title="" Language="C#" MasterPageFile="~/Comm/RefFunc/MasterPage.master" AutoEventWireup="true" Inherits="Comm_RefFunc_UIEn" Codebehind="UIEn.aspx.cs" %>
<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc1" %>
<%@ Register src="../UC/UIEn.ascx" tagname="UIEn" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<base target=_self />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:Left ID="Left1" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <uc2:UIEn ID="UIEn1" runat="server" />
<br>
</asp:Content>

