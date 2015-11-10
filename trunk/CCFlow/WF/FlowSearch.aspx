<%@ Page Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true"
 Inherits="CCFlow.WF.Face_FlowSearch" Title=" Query and analysis " Codebehind="FlowSearch.aspx.cs" %>
<%@ Register src="UC/FlowSearch.ascx" tagname="FlowSearch" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language=javascript>
    function Dtl(fk_flow) {
        WinOpen('DtlSearch.aspx?FK_Flow=' + fk_flow, 'ss');
    }
</script>
    <script language="JavaScript" src="./Comm/JScript.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:FlowSearch ID="FlowSearch1" runat="server" />
</asp:Content>

