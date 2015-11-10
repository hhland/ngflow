<%@ Page Language="C#" MasterPageFile="../SDKComponents/Site.master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_Forward_UI" Title=" Transfer " Codebehind="Forward.aspx.cs" %>
<%@ Register src="../UC/ForwardUC.ascx" tagname="ForwardUC" tagprefix="uc266" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />

     <script type="text/javascript" >
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc266:ForwardUC ID="ForwardUC1" runat="server" />
</asp:Content>