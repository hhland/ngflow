<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchAdv.ascx.cs" Inherits="CCFlow.WF.Rpt.SearchAdv" %>
<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="../../Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<%@ Register Src="../../Comm/UC/UCSys.ascx" TagName="UCSys" TagPrefix="uc3" %>
<link href="../Comm/Style/CommStyle.css" rel="stylesheet" type="text/css" />
<link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
<script language="JavaScript" src="../Comm/JScript.js" type="text/javascript" />
<script language="javascript" type="text/javascript">
</script>
<div id="searchWin" class="easyui-layout" data-options="fit:true">
    <div data-options="region:'west',title:' My query ',split:true,tools:'#qtools'" style="padding: 5px;
        width: 200px; height: 200px">
        <ul class="navlist">
            <uc1:Pub ID="Pub1" runat="server" />
        </ul>
    </div>
    <div id="qtools">
        <a href="javascript:void(0)" class="icon-add" onclick="javascript:self.location='<%=this.NormalSearchUrl %>';"
            title=" Default Query ( Can 【 Save Query 】到[ My query ]中)"></a>
    </div>
    <div data-options="region:'center',title:'<%=this.CurrentQuery %>'" style="padding: 5px;">
        <table class='Table' cellspacing='0' cellpadding='0' border='0' style='width: 100%'>
            <uc1:Pub ID="Pub2" runat="server" />
        </table>
        <br />
        <br />
        <asp:HiddenField ID="hiddenAttrKey" runat="server" />
        <div style="text-align: right; width: 100%">
            <cc1:LinkBtn ID="btnSearch" data-options="iconCls:'icon-search',plain:false" runat="server"
                OnClick="btnSearch_Click"> Inquiry </cc1:LinkBtn>
            <cc1:LinkBtn ID="btnSaveSearchDirect" data-options="iconCls:'icon-save',plain:false"
                runat="server" OnClick="btnSaveSearch_Click" Visible="false"> Save the query </cc1:LinkBtn>
            <a id="btnSaveSearch" href="javascript:void(0)" onclick="showAskName()" class="easyui-linkbutton"
                data-options="iconCls:'icon-save'" runat="server" visible="false"> Save Query </a>
            <cc1:LinkBtn ID="btnDeleteModel" data-options="iconCls:'icon-delete',plain:false"
                runat="server" Visible="false" OnClick="btnDeleteModel_Click" OnClientClick="return confirm(' Are you sure you want to delete this query ?');"> Delete </cc1:LinkBtn>
            <cc1:LinkBtn ID="btnExport" data-options="iconCls:'icon-excel',plain:false" runat="server"
                OnClick="btnExport_Click"> Export </cc1:LinkBtn>
        </div>
        <div id='attrwin' class='easyui-dialog' title=" Select the query field " data-options="closed:true"
            style='width: 240px; height: 280px'>
            <div class='easyui-layout' data-options='fit:true'>
                <div data-options="region:'center',noheader:true,border:false">
                    <cc1:LB ID="LB_Attrs" AutoPostBack="false" Height="100%" Width="100%" onchange="setAttr(this.value)"
                        runat="server">
                    </cc1:LB>
                </div>
                <div data-options="region:'south',noheader:true,border:false" style='height: 40px;
                    padding: 10px; text-align: right'>
                    <cc1:LinkBtn ID="btnOK" data-options="iconCls:'icon-save',plain:false" runat="server"
                        OnClick="btnOK_Click"> Determine </cc1:LinkBtn>
                    <a href='javascript:void(0)' onclick="$('#attrwin').dialog('close');" class='easyui-linkbutton'
                        data-options="iconCls:'icon-undo'"> Cancel </a>
                </div>
            </div>
        </div>
        <div id='askname' title=" Save the query " class='easyui-dialog' data-options="closed:true" style='width: 240px;
            height: 140px; padding: 20px;'>
             Query Name :<cc1:TB ID="TB_SearchName" runat="server" Width="180"></cc1:TB><br />
            <br />
            <div style='text-align: right; width: 100%'>
                <cc1:LinkBtn ID="btnSaveSearchOK" data-options="iconCls:'icon-save',plain:false"
                    runat="server" OnClientClick="setSearchName()" OnClick="btnSaveSearch_Click"> Determine </cc1:LinkBtn>
                <a href='javascript:void(0)' onclick="$('#askname').dialog('close');" class='easyui-linkbutton'
                    data-options="iconCls:'icon-undo'"> Cancel </a>
            </div>
        </div>
    </div>
    <div data-options="region:'south',split:true,title:' Query Results ',collapsed:<%=this.ResultCollapsed.ToString().ToLower() %>"
        style="height: 360px;">
        <uc3:UCSys ID="UCSys1" runat="server" />
        <uc1:Pub ID="Pub3" runat="server" />
    </div>
</div>
<script type="text/javascript">
    var hn = '<%= this.hiddenAttrKey.ClientID %>';
    var lb = '<%= this.LB_Attrs.ClientID %>';
    var tn = '<%=this.TB_SearchName.ClientID %>';

    function selectAttr(condIdx, selectedColumn) {

        if (condIdx != undefined) {
            $('#' + hn).val(condIdx);
        }

        $('#attrwin').dialog('open');

        if (selectedColumn != undefined) {
            $('#' + lb).val(selectedColumn);
            $('#' + hn).val(condIdx + '^' + selectedColumn);
        }
    }

    function setAttr(attr) {
        var hattr = $('#' + hn).val();
        $('#' + hn).val(hattr.split('^')[0] + '^' + attr);
    }

    function setSearchName() {
        $('#' + hn).val($('#' + tn).val());
    }

    function showAskName() {
        $('#askname').dialog('open');
        $('#' + tn).focus();
    }
</script>
