<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimulateLogin.aspx.cs"
    Inherits="GMP2.SSO.SimulateLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../GPM/jquery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../GPM/jquery/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(function () {
            //获取iframe的window对象
            //var iframWin = document.getElementById("transIframe").contentWindow;
            //通过获取到的window对象操作HTML元素，这和普通页面一样
            //iframWin.document.getElementById("exit").style.visibility = "visible";
            //var userName = iframWin.document.getElementById("txtUserName");
            //var userPwd = iframWin.document.getElementById("txtPassword");

        });
        function getUserPars() {
            var iframWin = document.getElementById("transIframe").contentWindow;
            //通过获取到的window对象操作HTML元素，这和普通页面一样
            //iframWin.document.getElementById("exit").style.visibility = "visible";
            var userName = iframWin.document.getElementById("txtUserName");
            userName.value = "admin";
            var userPwd = iframWin.document.getElementById("txtPassword");
            userPwd.value = "pub";
            iframWin.theForm.submit();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" id="submit" value="获取" onclick="getUserPars()" />
    </div>
    </form>
</body>
</html>
