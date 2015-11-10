<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../WF/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            font-size: 13px;
            line-height: 130%;
            padding: 60px;
        }
        .panel
        {
            width: 360px;
            border: 1px solid #0050D0;
        }
        .head
        {
            padding: 5px;
            background: #A3C0E8;
            cursor: pointer;
            height: 13px;
        }
        .content
        {
            padding: 10px;
            text-indent: 2em;
            border-top: 1px solid #0050D0;
            display: block;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#panel1 div.head").click(function () {
                $(this).next("div.content").slideToggle();
            });
            $("#panel2 h5.head").click(function () {
                $(this).next("div.content").slideToggle();
            })
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display: inline;">
        <div id="panel1" class="panel" style="float:left">
            <div class="head">
                <h5>
                     Profile</h5>
            </div>
            <div class="content" style="font-family: 'Segoe UI'; font-size: medium">NAK NMP 
                WORKFLOW<br />
                 <br />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
