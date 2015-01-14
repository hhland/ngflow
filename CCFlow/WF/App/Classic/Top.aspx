<%@ Page Language="C#" AutoEventWireup="true" Inherits="AppDemo_Top1" CodeBehind="Top.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="./css/basic.css" rel="stylesheet" type="text/css" />
    <link href="./css/base.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
        function relogin() {
            //if(confirm(" Are you sure you want to re-visit ?"))
            //parent.location.href = "login.aspx?flag=logout";
            parent.location.href = "<%=webloginurl %>";
        }
    </script>
    <script type="text/javascript" language="javascript">
        var timer;
        function startTimer() {
            return;
            timer = setInterval("alert()", 60000);  //1 Points to perform a 
        }
        function alert() {
            var arr = " <%=tempcount%>";
            if (arr > 0) {
                ifrmTop.location.href = "treelist\\setsession.ashx?sname=0";
                openwin();
            }
        }
        function refwindow(num) {
            return;
            var num = num;
            num++;
            var mytimeout = window.setTimeout("refwindow(1)", 12000);
            if (num == 2) {
                // Flashing a few prompts the user to have information 
                var cdemo = window.setTimeout("window.focus()", 1000);
            }
        }
        var posx = screen.width - 330;
        var posy = screen.height - 300;
        function openwin() {
            divnew.style.display = "";
        } 
    </script>
</head>
<body onload="startTimer()">
    <form id="form1" runat="server">
    <div class="JS_haed">
        <div class="JS_haed_n">
        <div style="float:left;color:Yellow;font-size:18px;vertical-align:middle"> <font><b> Gallop workflow management system CCFlow5.0</b></font></div>
            <div class="JS_admin fr">
                <p > 
                     Date :<%=System.DateTime.Now.ToLongDateString() %><br />
                     Account number :<span><%=BP.Web.WebUser.No.ToString() %></span>  Full name :<span> <a href="javascript:void(0)" onclick="window.top.main.location.href = './../WF/Tools.aspx'">
                        <%=BP.Web.WebUser.Name.ToString() %></a></span><br />
                     Department :<span><%=BP.Web.WebUser.FK_DeptName.ToString()%></span>
                </p>
                <asp:ImageButton ImageUrl="Img/Top/Exit.jpg"  CssClass="login-btn" runat="server"
                    ID="imgBtn" OnClick="Unnamed1_Click" />
            </div>
            <div id="divnew" name="divnew" style="display: none">
                <img alt=" There are new pieces received , See the to-do list " src="./Img/newinfo.gif" onclick="divnew.style.display = 'none'" /></div>
            <div style="display: none">
                <iframe src="" id="ifrmTop" name="ifrmTop"></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
