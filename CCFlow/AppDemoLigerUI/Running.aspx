<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Running.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.Running" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jquery/lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../WF/Scripts/easyUI/easyloader.js" type="text/javascript"></script>
    <script src="../WF/Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <link href="../WF/Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="../WF/Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <script src="js/AppData.js" type="text/javascript"></script>
    <script src="js/RunningPage.js" type="text/javascript"></script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div data-options="region:'center'" border="false" style="margin: 0; padding: 0;
        overflow: hidden;">
        <div id="maingrid" fit="true" fitcolumns="true" style="margin: 0; padding: 0;" class="easyui-datagrid">
        </div>
    </div>
    </form>
</body>
</html>
