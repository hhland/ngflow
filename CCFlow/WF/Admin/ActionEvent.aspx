<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActionEvent.aspx.cs" Inherits="CCFlow.WF.Admin.ActionEvent" %>

<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Event interface </title>
    <link href="../Comm/Style/CommStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DoDel(nodeid, xmlEvent) {
            if (window.confirm(' You sure you want to delete it ?') == false)
                return;
            parent.window.location.href = 'Action.aspx?NodeID=' + nodeid + '&DoType=Del&RefXml=' + xmlEvent + '&tk=' + Math.random();
        }
    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'center',border:false">
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
