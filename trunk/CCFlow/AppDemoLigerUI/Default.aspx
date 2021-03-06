﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.Default" %>
<%@ Import Namespace="CCFlow.WF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=BP.Sys.SystemConfig.SysName %></title>
    <link href="jquery/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <script src="../WF/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/ligerui.min.js" type="text/javascript"></script>
    <script src="Menu/MenuData.js" type="text/javascript"></script>
    <script src="js/AppData.js" type="text/javascript"></script>
    <script src="js/popMessage.js" type="text/javascript"></script>
    <%--<script src="jquery/lib/ligerUI/js/core/base.js" type="text/javascript"></script>--%>
    <script src="jquery/lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="js/GetTreeData.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tab = null;
        var accordion = null;
        var tree = null;
        $(function () {
            // Layout 
            $("#layout1").ligerLayout({ leftWidth: 190, height: '100%', heightDiff: -34, space: 4, onHeightChanged: f_heightChanged });
            var height = $(".l-layout-center").height();
            //Tab
            $("#framecenter").ligerTab({ height: height, onAfterSelectTabItem: function (tabId) {
                if (tabId == "empworks" || tabId == "HungUp") {
                    tab.reload(tabId);
                }
            }, onAfterRemoveTabItem: function (tabId, index) {
                var item_list = tab.getTabidList();
                var preItem = item_list[item_list.length - 1];
                if (preItem == "empworks" || preItem == "HungUp") {
                    tab.reload(preItem);
                }
            }
            });
            // Panel 
            $("#accordion1").ligerAccordion({ height: height - 24, speed: null });

            $(".l-link").hover(function () {
                $(this).addClass("l-link-over");
            }, function () {
                $(this).removeClass("l-link-over");
            });

            tab = $("#framecenter").ligerGetTabManager();
            accordion = $("#accordion1").ligerGetAccordionManager();


            var startUrl = $("#startFlow").val();
            var nodeformtype = $("#nodeformtype").val();
            if (startUrl != null && startUrl != "") {
                //  f_addTab("addstartflow", " Initiate the process ", startUrl);
                //   In the case of  " Form tree "  Pop up 
                if (nodeformtype == "SheetTree") {
                    window.open(startUrl)
                }
                else {
                    f_addTab("addstartflow", " Initiate the process ", startUrl);
                }

            }
            refreshEmpWorks();
            //Application.data.popAlert("unRead", getJson, this); //2013.05.26 H 
            $("#pageloading").hide();
        });

        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 24 > 0)
                accordion.setHeight(options.middleHeight - 24);
        }

        function f_addTab(tabid, text, url) {
            tab.addTabItem({ tabid: tabid, text: text, url: url });
        }
        function checktop() {
            if (top.location != window.location) {
                top.location = "Login.aspx";
            }
        }
        function callBack(js) {
            if (typeof (js) == "string") {
                var pushData = eval('(' + js + ')');
                document.getElementById("empworkCount").innerHTML = "  (" + pushData.message.empwork + ")";
                document.getElementById("ccsmallCount").innerHTML = "  (" + pushData.message.ccnum + ")";
                document.getElementById("hungUpCount").innerHTML = "  (" + pushData.message.hungupnum + ")";
                if (document.getElementById("TaskPoolNum") != null)
                    document.getElementById("TaskPoolNum").innerHTML = "  (" + pushData.message.TaskPoolNum + ")";
            }
        }

        // Refresh the number of jobs 
        function refreshEmpWorks() {
            Application.data.getEmpWorkCounts(callBack, this);
        }
        setInterval(refreshEmpWorks, 10000);

        //POP Pop-up window   2013.05.23 H
        function getJson(jsonData, scope) {
            if (jsonData) {
                if (jsonData.length > 17) {
                    var pushData = eval('(' + jsonData + ')');
                    document.getElementById("messageCount").value = pushData.Total;
                    if (pushData.Total > 1)// If you did not read the record is greater than 1条
                    {
                        // Pop-up window 
                        var pOP_Message = new POP_Message(" System messages unread ", 200, 180, " System Messages :", "", " You have <a href='javascript:void(0)' hidefocus=false id='btCommand'>   " + pushData.Total + "   </a> Unread messages ");

                        pOP_Message.rect(null, null, null, screen.height - 50);
                        pOP_Message.speed = 10;
                        pOP_Message.step = 5;
                        pOP_Message.show();
                    }
                    else // Only 1 Article Information   The whole show 
                    {
                        var title = pushData.Rows[0].Title;
                        var doc = pushData.Rows[0].Doc.replace(/~/g, "'");
                        document.getElementById("messagemyPK").value = pushData.Rows[0].MyPK;
                        // Pop-up window 
                        var pOP_Message = new POP_Message(" System messages unread ", 200, 180, " System Messages :", "", " You have 1 Unread messages <br/><a href='javascript:void(0)' hidefocus=false id='btCommand'>  " + title + "</a>");
                        pOP_Message.rect(null, null, null, screen.height - 50);
                        pOP_Message.speed = 10;
                        pOP_Message.step = 5;
                        pOP_Message.show();
                    }
                }
            }

        }

        function changeCulture(culture) {

            var url = "/WF/GlobalizationHandler.ashx?action=changeCulture";
            $.get(url, { culture: culture },function() {
                
                //window.location.reload();
            });

        }
    </script>
    <style type="text/css">
        body, html
        {
            height: 100%;
        }
        body
        {
            padding: 0px;
            margin: 0;
            overflow: hidden;
        }
        
        .l-link2
        {
            text-decoration: underline;
            color: white;
            margin-left: 2px;
            margin-right: 2px;
        }
        .l-layout-top
        {
            background: #102A49;
            color: White;
        }
        .l-layout-bottom
        {
            background: #E5EDEF;
            text-align: center;
        }
        #pageloading
        {
            position: absolute;
            left: 0px;
            top: 0px;
            background: white url('jquery/lib/images/loading.gif') no-repeat center;
            width: 100%;
            height: 100%;
            z-index: 99999;
        }
        .l-link
        {
            display: block;
            line-height: 25px;
            height: 25px;
            padding-left: 16px;
            text-decoration: none;
            border: 1px solid white;
            border-bottom: 1px #E5E5E5 solid;
            margin: 4px;
        }
        .l-link-over
        {
            background: #FFEEAC;
            border: 1px solid #DB9F00;
        }
        .l-winbar
        {
            background: #2B5A76;
            height: 30px;
            position: absolute;
            left: 0px;
            bottom: 0px;
            width: 100%;
            z-index: 99999;
        }
        .space
        {
            color: #E7E7E7;
        }
        .img-menu
        {
            height: 18px;
            width: 18px;
        }
        /*  Top  */
        .l-topmenu
        {
            margin: 0;
            padding: 0;
            height: 31px;
            line-height: 31px;
            background: url('jquery/lib/images/top.jpg') repeat-x bottom;
            position: relative;
            border-top: 1px solid #1D438B;
        }
        .l-topmenu-logo
        {
            color: #E7E7E7;
            padding-left: 45px;
            font-size: 16px;
            font-weight: bold;
            line-height: 26px;
            background: url('Img/logo.png') no-repeat 10px 5px;
        }
        .l-topmenu-welcome
        {
            position: absolute;
            height: 24px;
            line-height: 24px;
            right: 30px;
            top: 2px;
            color: #070A0C;
        }
        .l-topmenu-welcome a
        {
            color: #E7E7E7;
            text-decoration: underline;
        }
        a
        {
            color: Black;
        }
        #home
        {
            height: 167px;
            width: 376px;
        }
    </style>
</head>
<body style="padding: 0px; background: #EAEEF5;" onload="checktop();">
    <div id="pageloading">
    </div>
    <div id="topmenu" class="l-topmenu">
        <form id="formCulture" action="/AppDemoLigerUI/Default.aspx" method="get"  target="_self">
        <div class="l-topmenu-logo"><asp:Localize runat="server" Text="<%$ Resources:Title,TopMenu.Text %>"></asp:Localize></div>
        <div class="l-topmenu-welcome">
            <span class="space">
                <%=usermsg %></span>&nbsp;&nbsp;<span class="space">|</span>&nbsp;&nbsp;<asp:HyperLink runat="server" Text="<%$ Resources:Link,Logout.Text %>" NavigateUrl="Login.aspx?DoType=Logout"></asp:HyperLink> <span class="space">|</span> 
                        <asp:HyperLink runat="server" Text="<%$ Resources:Link,forum.Text %>" NavigateUrl="<%$ Resources:Link,forum.Href %>"></asp:HyperLink>
                <span class="space">|</span>
        
        <select name="culture"  onchange="document.getElementById('formCulture').submit();" >
           <%
               Dictionary<string, string> clutures = GlobalizationHandler.clutures;
               foreach (var key in clutures.Keys)
               {
                   string val = clutures[key];
                   string cname = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower();
                   string selected = cname == key ? "selected='selected'" : "";

           %>
            <option value="<%=key %>"  <%=selected %>  ><%=val %> </option>
           <% } %>
          </select>
      
        </div>
       </form>
    </div>
    <div id="layout1" style="width: 99.2%; margin: 0 auto; margin-top: 4px;">
        <div position="left" title="<%$ Resources:Title,Menu.Text %>" id="accordion1" runat="server">
        </div>
        <div position="center" id="framecenter">
            <div tabid="home" title=" <%$ Resources:Title,Home.Text %> " style="height: 300px" runat="server">
                <iframe frameborder="0" name="home" id="home" style="width:100%;height:100%;" src="Welcome.aspx"></iframe>
            </div>
        </div>
    </div>
    <div style="height: 32px; line-height: 32px; color: #666; font-family: arial; text-align: center;">
          <asp:Localize  runat="server"  Text="<%$ Resources:Title,co.Text %>"></asp:Localize>
    </div>
    <div style="display: none">
    </div>
    <input type="hidden" id="startFlow" value="<%=mainSrc %>" />
    <input type="hidden" id="nodeformtype" value="<%=nodeformtype %>" />
    <input type="hidden" id="messageCount" value="0" />
    <input type="hidden" id="messagemyPK" value="0" />
</body>
</html>
