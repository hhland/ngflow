<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpForMenus.aspx.cs" Inherits="GMP2.GPM.EmpForMenus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title> User menu Permissions </title>
    <link rel="stylesheet" type="text/css" href="themes/default/easyui.css" />
    <link href="themes/default/tree.css" rel="stylesheet" type="text/css" />
    <link href="themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="themes/icon.css" />
    <script type="text/javascript" src="jquery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="jquery/jquery.easyui.min.js"></script>
    <script src="javascript/CC.MessageLib.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <script src="javascript/EmpForMenus.js" type="text/javascript"></script>
</head>
<body class="easyui-layout">
    <form runat="server">
    <div id="pageloading" style="opacity: 0.6; background-color: rgb(204, 204, 204);">
    </div>
    <div data-options="region:'north',border:false" style="height: 40px; background: #E0ECFF;
        padding: 10px">
        <div class="toolbar">
            <div style=" float:left;"> Query :<input type="text" id="searchConten" name="searchConten" /></div>
            <a href="#" style=" float:left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-search'" onclick="QueryEmps()">
                 Inquiry </a><div class="datagrid-btn-separator">
                </div><a href="#" id="saveRight" class="easyui-linkbutton"
                    data-options="plain:true,iconCls:'icon-save',disabled:true"> Save </a>
            <%--<a href="#" class="easyui-linkbutton"> Clear Permissions </a>--%>
            <a href="#" id="rightC" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-add'" onclick="CopyRight()"> Copy rights to others </a>
        </div>
    </div>
    <div data-options="region:'west',split:true" title=" User " style="width: 200px; padding1: 1px;
        overflow: hidden;">
        <div style="width: 100%; height: 100%; overflow: auto;">
            <select size="4" name="lbEmp" id="lbEmp" onchange="LoadMenusOnEmpChange()" style="height: 100%;
                width: 100%;">
            </select>
            <div id="mm1" class="easyui-menu" style="width: 150px;">
                <div id="rightCcontent" onclick="CopyRight()">
                     Copy rights to others </div>
            </div>
        </div>
        <div id="deptEmpDialog">
            <div style="height: 420px; overflow: auto">
                <ul id="deptEmpTree" class="easyui-tree" data-options="animate:true,dnd:false">
                </ul>
            </div>
            <div style="font-weight: bold; background: #E0ECFF;">
                <span style="color: Red; font-weight: bold;"> Explanation : 【 Empty replication 】 The selected staff privileges ( Does not include the group permissions ) First empty ,<br />
                     Then copy the permissions granted to selected personnel .<br />
                    【 Covering replication 】 Prior permission to retain the selected staff , Copy permissions to selected personnel increments . </span>
            </div>
        </div>
        <span id="copyMsg" style="color: Red; font-weight: bold; display: none;"></span>
    </div>
    <div data-options="region:'center'" title=" Menu " style="overflow: hidden;">
        <div style="overflow: auto; height: 100%;">
            <ul id="menuTree" class="easyui-tree" data-options="animate:false,dnd:false">
            </ul>
        </div>
    </div>
    <input type="hidden" id="empNo" name="empNo" runat="server" value="" />
    <input type="hidden" id="menuIds" name="menuIds" runat="server" value="" />
    <input type="hidden" id="menuIdsUn" name="menuIdsUn" runat="server" value="" />
    </form>
</body>
</html>
