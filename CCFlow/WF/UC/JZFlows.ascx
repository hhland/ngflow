<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JZFlows.ascx.cs" Inherits="CCFlow.WF.UC.UCFlows" %>
<script type="text/javascript">
    function StartListUrl(appPath, url, fk_flow, pageid) {
        var v = window.showModalDialog(url, 'sd', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
        //alert(v);
        if (v == null || v == "")
            return;

        window.location.href = appPath + '../MyFlow.aspx?FK_Flow=' + fk_flow + v;
    }
    function WinOpenIt(url) {
        var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
        newWindow.focus();
        return;
    }

    function WinOpen(url, winName) {
        var newWindow = window.open(url, winName, 'height=800,width=1030,top=' + (window.screen.availHeight - 800) / 2 + ',left=' + (window.screen.availWidth - 1030) / 2 + ',scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
        newWindow.focus();
        return;
    }
</script>