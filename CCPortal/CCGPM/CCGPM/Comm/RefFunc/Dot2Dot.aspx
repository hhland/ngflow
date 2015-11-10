<%@ Page Language="C#" MasterPageFile="~/Comm/RefFunc/MasterPage.master" AutoEventWireup="true" Inherits="Comm_RefFunc_Dot2Dot" Title="Untitled Page" Codebehind="Dot2Dot.aspx.cs" %>
<%@ Register src="Dot2Dot.ascx" tagname="Dot2Dot" tagprefix="uc1" %>
<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<base target=_self />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<base target=_self />
    <uc2:Left ID="Left1" runat="server" />
<base target=_self />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <uc1:Dot2Dot ID="Dot2Dot1" runat="server" />
</asp:Content>