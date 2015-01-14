<%@ Page Title=" Subgroup analysis " Language="C#" MasterPageFile="~/WF/Rpt/Single.Master" AutoEventWireup="true"
    CodeBehind="Group.aspx.cs" Inherits="CCFlow.WF.Rpt.Group" %>

<%@ Register Src="UC/Group.ascx" TagName="Group" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Comm/JS/Calendar/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/EasyUIUtility.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OpenUrl(wintitle, url) {
            var dlg = $('#win').dialog({ title: wintitle, onClose: function () { $('#winFrame').attr('src', ''); } });
            dlg.dialog('open');
            $('#winFrame').attr('src', "../../Comm/RefFunc/Dot2DotSingle.aspx?EnsName=BP.WF.Rpt.MapRpts&EnName=BP.WF.Rpt.MapRpt&AttrKey=BP.WF.Rpt." + rpt + "&No=" + rptNo);
        }

        function WinOpen(url) {
            var newWindow = window.open(url, 'z', 'scroll:1;status:1;help:1;resizable:1;dialogWidth:680px;dialogHeight:420px');
            newWindow.focus();
            return;
        }

        function WinOpen(url, winName) {
            var newWindow = window.open(url, winName, 'width=700,height=400,top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
            newWindow.focus();
            return;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Group ID="Group1" runat="server" />
</asp:Content>
