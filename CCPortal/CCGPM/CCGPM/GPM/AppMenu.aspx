<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppMenu.aspx.cs" Inherits="GMP2.GPM.AppMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统对应的菜单</title>
    <link id="appstyle" href="themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link href="themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="jquery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="jquery/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <script src="javascript/SystemMenus.js" type="text/javascript"></script>
</head>
<body>
    <table id="menuGrid" class="easyui-datagrid">
    </table>
    <div id="mm" class="easyui-menu" style="width: 120px;">
       <div onclick="OpenNewWindow()" data-options="iconCls:'icon-save-close'">
            打开新窗口</div>
<%--        <a href="javascript:void(0)"  onclick="addTab('新窗口', 'AppMenu.aspx?FK_App=CCFLOW')"  
        data-options="iconCls:'icon-add'"  onclick></a>--%>
        <div onclick="addSampleNode()" data-options="iconCls:'icon-add'">
            增加同级</div>
        <div onclick="addChildNode()" data-options="iconCls:'icon-add'">
            增加下级</div>
        <div onclick="tranUp()" data-options="iconCls:'icon-remove'">
            上移</div>
        <div onclick="tranDown()" data-options="iconCls:'icon-remove'">
            下移</div>
        <div onclick="delNode()" data-options="iconCls:'icon-cancel'">
            删除</div>
        <div onclick="editNode()" data-options="iconCls:'icon-edit'">
            修改</div>
    </div>
</body>
</html>
