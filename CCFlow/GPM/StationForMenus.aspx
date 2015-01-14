<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StationForMenus.aspx.cs"
    Inherits="GMP2.GPM.StationForMenus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Assign permissions by post </title>
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css" />
    <link href="themes/default/tree.css" rel="stylesheet" type="text/css" />
    <link href="themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="themes/icon.css" />
    <script type="text/javascript" src="jquery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="jquery/jquery.easyui.min.js"></script>
    <script src="javascript/CC.MessageLib.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <script src="javascript/StationForMenus.js" type="text/javascript"></script>
</head>
<body class="easyui-layout">
    <div data-options="region:'north',border:false" style="height: 40px; background: #E0ECFF;
        padding: 10px">
        <div class="toolbar">
            <div style="float:left;"> Query :<input type="text" id="searchContent" name="searchContent" /></div>
            <a href="#" style="float:left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-search'" id="QueryStation">  Inquiry </a>
            <div class="datagrid-btn-separator"></div>
            <a href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save',disabled:true" id="saveRight"> Save </a>
            <a href="#" id="rightC" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-add'" onclick="CopyRight()"> Copy rights to other positions </a>
        </div>
    </div>
    <div data-options="region:'west',split:true" title=" Post " style="width: 200px; padding1: 1px;
        overflow: hidden;">
        <div style="width: 100%; height: 100%; overflow: auto;">
            <select size="4" name="lbStation" id="lbStation" onchange="LoadMenusOnStationChange()"
                style="height: 100%; width: 100%;">
            </select>
            <div id="mm1" class="easyui-menu" style="width: 150px;">
                <div id="rightCcontent" onclick="CopyRight()">
                     Copy rights to other positions </div>
            </div>
        </div>
        <div id="StationDialog">
            <div style="height: 420px; overflow: auto">
                <ul id="StationTree" class="easyui-tree" data-options="animate:true,dnd:false">
                </ul>
            </div>
            <div style="font-weight: bold; background: #E0ECFF; height: 40px;">
                <span style="color: Red; font-weight: bold; margin-top: 5px;"> Explanation : 【 Empty replication 】 First post selected permissions empty , Then the selected positions authorized permission .<br />
                    【 Covering replication 】 Prior permission to retain the selected post , Copy permissions to the selected incremental jobs . </span>
            </div>
        </div>
        <span id="copyMsg" style="color: Red; font-weight: bold; display: none"></span>
    </div>
    <div data-options="region:'center'" title=" Menu " style="overflow: hidden;">
        <div style="overflow: auto; height: 100%;">
            <ul id="menuTree" class="easyui-tree" data-options="animate:false,dnd:false">
            </ul>
        </div>
    </div>
</body>
</html>
