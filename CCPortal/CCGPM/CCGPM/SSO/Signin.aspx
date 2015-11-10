<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>

<%@ Page Language="c#" Inherits="BP.Web.SignInPG" CodeBehind="Signin.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>
        <%=BP.Sys.SystemConfig.SysName%>
    </title>
    <meta name="vs_showGrid" content="True">
    <meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <script language="javascript">
        function winfix() {
            if (document.layers) {
                width = screen.availWidth - 10;
                height = screen.availHeight - 20;
            } else {
                var width = screen.availWidth - 2;
                var height = screen.availHeight;
            }
            self.resizeTo(width, height);
            self.moveTo(0, 0);
        }
        var mode;
        mode = 1;
        function show() {
            // window.open('Note.htm')
            if (mode == 0) {
                help.innerHTML = "";
                mode = 1;
            }
            else {
                help.innerHTML = "<BR><b>1)</b>本系统与主体系统紧密结合,请采用系统的用户名登陆.";
                help.innerHTML += "<BR><b>2)</b>如果您是首次登陆，密码请用pub.登陆系统后请注意修改密码．为了保证你的数据安全，请注意保存好自己的密码．";
                help.innerHTML += "<BR><b>3)</b>使用过程中有问题，请发布到BBS或者反映给系统管理员．";
                help.innerHTML += "<BR><BR>&nbsp;&nbsp;&nbsp;&nbsp;工作愉快．";
                mode = 0;
            }
        }
        function SetHomePage() {
            document.body.style.behavior = "url(#default#homepage)";
            document.body.setHomePage(window.location.href);
            //  document.body.all.homepageLink.click();
        }
        function setFocus(ctl) {
            if (document.forms[0][ctl] != null) {
                document.forms[0][ctl].focus();
            }
        }		
		 
    </script>
    <script language="JavaScript" src="../../Comm/JScript.js"></script>
    <style type="text/css">
        BODY
        {
            background-position: center 50%;
            background-attachment: fixed;
            background-repeat: no-repeat;
            background-color: #F2f5fa;
            font: 12px/12px Arial, sans-serif, Verdana, Tahoma;
        }
        .border
        {
            border: 1px solid #1B85E2;
        }
    </style>
</head>
<body bgproperties="fixed" topmargin="0" leftmargin="0" rightmargin="0" bottommargin="0"
    class="Login<%=BP.Sys.SystemConfig.SysNo%>" onload="winfix();">
    <form id="SignIn" method="post" runat="server" defaultfocus="TB_No">
    <div style="background: url(Img/bgtop.png); height: 75px">
    </div>
    <table align="center" id="Table3" border="0" style="width: 70%; height: 80%">
        <tr>
            <td align="center" height="400px">
                <table height="200px" width="300px" cellpadding="0" cellspacing="0" class="border">
                    <tr style="text-align: left; height: 30px; background-color:  #4682B4;">
                        <td colspan="2">
                            <img src="Img/icon-11.png" width="30" height="30" align='middle' />
                            <font size="3"><b>单点登录系统</b></font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:Lab ID="Lab1" runat="server">
										<font size="2">&nbsp;&nbsp;<b>用户名</b></font></cc1:Lab>
                        </td>
                        <td>
                            <cc1:TB ID="TB_No" Style="background-image: url(Img/text.png);" runat="server" ShowType="TB"
                                Width="165px" Height="28px" BorderStyle="None" Font-Size="14px"></cc1:TB>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:Lab ID="Lab2" runat="server">
										<font size="2">&nbsp;&nbsp;<b>密&nbsp;&nbsp;码</b></font></cc1:Lab>
                        </td>
                        <td>
                            <cc1:TB Style="background-image: url(Img/text.png);" ID="TB_Pass" runat="server"
                                Width="164px" Height="28px" BorderStyle="none" TextMode="Password" BackColor="White"></cc1:TB>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <cc1:Btn ID="Btn1" runat="server" Style="background: url(Img/btlogin.png); width: 108px;
                                height: 33px; border: none;" OnClick="Btn1_Click"></cc1:Btn>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div style="background: url(Img/bg_bottom.png); height: 70px; text-align: center;
        line-height: 70px;">
        <span style="color: #fff; font-size: 12px;">版权所有：济南驰骋信息技术有限公司-单点登录系统</span>
    </div>
    </form>
</body>
</html>
