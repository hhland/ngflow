<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.WF_DtlFrm" Codebehind="DtlCard.aspx.cs" %>
<%@ Register src="../UC/UCEn.ascx" tagname="UCEn" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<link href="../Style/themes/default/easyui.css" rel="stylesheet" type="text/css" />
<link href="../Style/themes/icon.css" rel="stylesheet" type="text/css" />
<script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../Scripts/jquery.easyui.min.js" type="text/javascript"></script>
<script language="javascript">
    function SaveDtlData(iframeId) {
        var iframe = document.getElementById("IF" + iframeId);
        if (iframe) {
            iframe.contentWindow.SaveDtlData()
        }
    }

    var maintabs_regex = "#maintabs";
    
    function createTab(title, FrmW, FrmH, EnsName, RefPKVal, IsReadonly) {
        var url = "DtlCard.ashx?action=addRow";
        $.getJSON(url, { EnsName: EnsName, addRowNum: 1, RefPKVal: RefPKVal}, function (json) {
            
            _createTab(title, FrmW, FrmH, EnsName, RefPKVal,1, IsReadonly);
        });
    }

    function _createTab(prefix,FrmW,FrmH,EnsName,RefPKVal,OID,IsReadonly) {
        var $maintabs = $(maintabs_regex);
        var idx = $maintabs.tabs("tabs").length;
        OID = idx;
        var src="FrmDtl.aspx?FK_MapData=" + EnsName + "&WorkID=" + RefPKVal +"&OID=" + OID + "&IsReadonly=" + IsReadonly;
        var frame = "<iframe id='IF" + idx + "' frameborder='0' style='width:" + FrmW + "px;height:" + FrmH + "px;' src=\"" + src + "\"><" + "/iframe>";
        $maintabs.tabs('add', {
            title: prefix+" "+(idx+1),
            selected: true
            ,content:frame
        });
    }

    function deleteTab(EnsName, RefPKVal) {
        var url = "DtlCard.ashx?action=addRow";
        $.getJSON(url, { EnsName: EnsName, addRowNum: -1, RefPKVal: RefPKVal }, function (json) {

            _deleteTab();
        });
    }

    function _deleteTab() {
        var $maintabs = $(maintabs_regex);
        var selecttab = $maintabs.tabs('getSelected');
        var selectindex = $maintabs.tabs("tabs").length-1; //$maintabs.tabs('getTabIndex', selecttab);
        $maintabs.tabs("close",selectindex);
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:UCEn ID="UCEn1" runat="server" />
</asp:Content>
