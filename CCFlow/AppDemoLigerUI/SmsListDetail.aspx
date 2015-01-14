<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SmsListDetail.aspx.cs"
    Inherits="CCFlow.AppDemoLigerUI.SmsListDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="jquery/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/Gray/css/all.css" rel="stylesheet" type="text/css" />
    <link href="jquery/tablestyle.css" rel="stylesheet" type="text/css" />
    <link href="jquery/lib/ligerUI/skins/ligerui-icons.css" rel="stylesheet" type="text/css" />
    <script src="jquery/lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="js/AppData.js" type="text/javascript"></script>
    <script src="js/SmslistDetail.js" type="text/javascript"></script>
    <style type="text/css">
        .l-page-top
        {
            height: 30px;
            background: #f8f8f8;
            margin-bottom: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style=" text-align:left">
        <tr style=" font-size:15; height:40px;">
         <td> Theme :</td>
         <td id="tdTitle"></td>
        </tr>
            <tr>
                <td style=" width:60px;">
                     Sender :
                </td>
                <td id="lbSender">
                </td>
            </tr>
            <tr>
                <td>
                     Transmission time :
                </td>
                <td id="lbRDT">
                </td>
            </tr>
            <tr>
                <td>
                     Recipient :
                </td>
                <td id="lbSendTo">
                </td>
            </tr>
            <tr style=" height:30px;">
                <td colspan='2'  style=" font-size:14;">
                     Content :
                </td>
            </tr>
            <tr>
                <td colspan='2'>
                    <div id="divDoc" style="width: 80%; height:80%; margin-left:20px;">
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
