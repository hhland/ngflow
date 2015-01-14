<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppMenu.aspx.cs" Inherits="GMP2.GPM.AppMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> System corresponding menu </title>
    <link id="appstyle" href="themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link href="themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="jquery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="jquery/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <script src="javascript/SystemMenus.js" type="text/javascript"></script>
</head>
<body>
    <table id="menuGrid" fit="true" class="easyui-datagrid">
    </table>
    <div id="mm" class="easyui-menu" style="width: 120px;">
        <div data-options="iconCls:'icon-new'">
            <span> Create a model menu </span>
            <div style="width: 120px;">
                <div onclick="NewFlowModel()">
                     Process type model </div>
                <%--<div class="menu-sep"></div>--%>
                <div onclick="NewFormModel()">
                     Form type model </div>
            </div>
        </div>
        <div onclick="OpenNewWindow()" data-options="iconCls:'icon-save-close'">
             Open a new window </div>
        <div onclick="addSampleNode()" data-options="iconCls:'icon-add'">
             Increase at the same level </div>
        <div onclick="addChildNode()" data-options="iconCls:'icon-add'">
             Increase subordinate </div>
        <div onclick="tranUp()" data-options="iconCls:'icon-remove'">
             Move </div>
        <div onclick="tranDown()" data-options="iconCls:'icon-remove'">
             Down </div>
        <div onclick="delNode()" data-options="iconCls:'icon-cancel'">
             Delete </div>
        <div onclick="editNode()" data-options="iconCls:'icon-edit'">
             Modification </div>
    </div>
    <div id="DIV_toolbar">
        <a href="javascript:void(0)" id="mb1" style=" float:left;" class="easyui-menubutton" data-options="menu:'#mm1',iconCls:'icon-new'">
             Create a model menu </a> 
        <a href="#" style="float: left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save-close'"
                onclick="OpenNewWindow()"> Open a new window </a>
        <a href="#" style="float: left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-new'"
                onclick="addSampleNode()"> Increase at the same level </a>
        <div id="mm1" style="width: 150px;">
            <div onclick="NewFlowModel()">
                 Process type model </div>
            <div onclick="NewFormModel()">
                 Form type model </div>
        </div>
    </div>
    <div id="modelDialog">
        <div style="height: 450px; overflow: auto">
            <ul id="mainTree" class="easyui-tree" data-options="animate:true,dnd:false">
            </ul>
        </div>
    </div>
</body>
</html>
