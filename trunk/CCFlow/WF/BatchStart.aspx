<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="BatchStart.aspx.cs" Inherits="CCFlow.WF.BatchStartMy" %>
<%@ Register src="UC/BatchStart.ascx" tagname="BatchStart" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function NoSubmit(ev) {
        if (window.event.srcElement.tagName == "TEXTAREA")
            return true;

        if (ev.keyCode == 13) {
            window.event.keyCode = 9;
            ev.keyCode = 9;
            return true;
        }
        return true;
    }
</script>
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:BatchStart ID="BatchStart1" runat="server" />
</asp:Content>
