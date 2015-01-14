<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.FHLFlow"
    CodeBehind="FHLFlow.ascx.cs" %>
<%@ Register Src="./../Comm/UC/ToolBar.ascx" TagName="ToolBar" TagPrefix="uc1" %>
<%@ Register Src="UCEn.ascx" TagName="UCEn" TagPrefix="uc2" %>
<script type="text/javascript">
    function ReinitIframe(frmID, tdID) {
        try {
            var iframe = document.getElementById(frmID);
            var tdF = document.getElementById(tdID);
            iframe.height = iframe.contentWindow.document.body.scrollHeight;
            iframe.width = iframe.contentWindow.document.body.scrollWidth;
            if (tdF.width < iframe.width) {
                tdF.width = iframe.width;
            } else {
                iframe.width = tdF.width;
            }
            tdF.height = iframe.height;
            return;
        } catch (ex) {
            return;
        }
        return;
    }
    function GroupBarClick(rowIdx) {
        var alt = document.getElementById('Img' + rowIdx).alert;
        var sta = 'block';
        if (alt == 'Max') {
            sta = 'block';
            alt = 'Min';
        } else {
            sta = 'none';
            alt = 'Max';
        }
        document.getElementById('Img' + rowIdx).src = './Img/' + alt + '.gif';
        document.getElementById('Img' + rowIdx).alert = alt;
        var i = 0
        for (i = 0; i <= 40; i++) {
            if (document.getElementById(rowIdx + '_' + i) == null)
                continue;
            if (sta == 'block') {
                document.getElementById(rowIdx + '_' + i).style.display = '';
            } else {
                document.getElementById(rowIdx + '_' + i).style.display = sta;
            }
        }
    }
</script>
<script language="javascript">
    function Do(warning, url) {
        if (window.confirm(warning) == false)
            return;
        window.location.href = url;
        // WinOpen(url);
    }
</script>
<style type="text/css">
    .Bar
    {
        width: 500px;
        text-align: center;
    }
    
    #tabForm, D
    {
        width: 960px;
        text-align: left;
        margin: 0 auto;
        margin-bottom: 5px;
    }
    
    #divFreeFrm
    {
        position: relative;
        left: 25PX;
        width: 960px;
    }
</style>
<div id="tabForm">
    <uc1:ToolBar ID="ToolBar1" runat="server" />
    <hr>
</div>
<br>
<br>
<br>
<div id="D">
    <uc2:UCEn ID="UCEn1" runat="server" />
</div>
