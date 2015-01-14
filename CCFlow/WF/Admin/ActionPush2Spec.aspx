<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ActionPush2Spec.aspx.cs"
    Inherits="CCFlow.WF.Admin.ActionPush2Spec" %>

<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Other designated person to push message </title>
    <link href="../Comm/Style/CommStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Comm/JScript.js" type="text/javascript"></script>
    <script src="../Scripts/EasyUIUtility.js" type="text/javascript"></script>
    <script type="text/javascript">
        var txtId;
        var hiddenId;

        function showWin(url, tid, hid) {
            txtId = tid;
            hiddenId = hid;

            var selected = $('#' + hiddenId).val();
            if (selected != null && selected.length > 0) {
                url += '?In=' + selected + '&tk=' + Math.random();
            }

            // Here manually set up dialog The origin position , Otherwise open , Occasionally not among display , For unknown reasons 
            //$('#ewin').dialog({ left: (document.body.offsetWidth - 520) / 2, top: (document.body.offsetHeight - 360) / 2 });

            OpenEasyUiDialog(url, 'eudlgframe', ' Select staff ', 520, 360, 'icon-user', true, function () {
                var innerWin = document.getElementById('eudlgframe').contentWindow;
                $('#' + txtId).val(innerWin.getReturnText());
                $('#' + hiddenId).val(innerWin.getReturnValue());
            });
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
