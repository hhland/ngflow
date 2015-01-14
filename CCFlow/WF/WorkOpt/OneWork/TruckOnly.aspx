<%@ Page Title="" Language="C#" MasterPageFile="../../WinOpen.master" AutoEventWireup="true" CodeBehind="TruckOnly.aspx.cs" Inherits="CCFlow.WF.WorkOpt.OneWork.TruckOnly" %>
<%@ Register src="TruakUC.ascx" tagname="TruakUC" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    function WinOpen(url, winName) {
        var newWindow = window.open(url, winName, 'height=800,width=1030,top=' + (window.screen.availHeight - 800) / 2 + ',left=' + (window.screen.availWidth - 1030) / 2 + ',scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
        newWindow.focus();
        return;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:TruakUC ID="TruakUC1" runat="server" />
</asp:Content>
