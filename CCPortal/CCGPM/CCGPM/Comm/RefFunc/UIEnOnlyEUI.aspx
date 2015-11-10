<%@ Page Title="" Language="C#" MasterPageFile="~/Comm/EasyUISite.Master" AutoEventWireup="true"
    CodeBehind="UIEnOnlyEUI.aspx.cs" Inherits="CCOA.Comm.RefFunc.UIEnOnlyEUI" %>

<%@ Register Src="../UC/UCEn.ascx" TagName="UCEn" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div data-options="region:'center'" border="false" style="margin: 0; padding: 0;
        overflow: hidden;">
        <uc1:UCEn ID="" runat="server" />
    </div>
</asp:Content>
