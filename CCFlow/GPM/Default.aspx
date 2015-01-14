<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GMP2.GPM.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CCFlowBPM</title>
    <link id="appstyle" href="themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="jquery/jquery-1.7.2.min.js" type="text/javascript"></script>    
    <script src="jquery/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            font: 12px/12px Arial, sans-serif, Verdana, Tahoma;
            padding: 0;
            margin: 0;
        }
        a:link
        {
            text-decoration: none;
        }
        a:visited
        {
            text-decoration: none;
        }
        a:hover
        {
            text-decoration: underline;
        }
        a:active
        {
            text-decoration: none;
        }
        .cs-north
        {
            height: 30px;
        }
        .cs-north-bg
        {
            width: 100%;
            height: 100%;
            background: url(Images/Banal.jpg) repeat-x bottom;
        }
        .cs-north-logo
        {
            position: absolute;
            top: 8px;
            color: #E7E7E7;
            height: 16px;
            padding-left: 45px;
            font-size: 14px;
            font-weight: bold;
            background: url('Images/logo.png') no-repeat left;
        }
        .cs-west
        {
            width: 200px;
            padding: 0px;
        }
        .cs-south
        {
            height: 25px;
            background: url('themes/pepper-grinder/images/ui-bg_fine-grain_15_ffffff_60x60.png') repeat-x;
            padding: 0px;
            text-align: center;
        }
        .cs-navi-tab
        {
            padding: 5px;
            display: block;
            line-height: 18px;
            height: 18px;
            padding-left: 16px;
            text-decoration: none;
            border: 1px solid white;
            border-bottom: 1px #E5E5E5 solid;
        }
        .cs-navi-tab:hover
        {
            background: #FFEEAC;
            border: 1px solid #DB9F00;
        }
        .cs-tab-menu
        {
            width: 120px;
        }
        .cs-home-remark
        {
            padding: 10px;
        }
        .wrapper
        {
            float: right;
            height: 30px;
            margin-left: 10px;
        }
        .ui-skin-nav
        {
            float: right;
            padding: 0;
            margin-right: 10px;
            list-style: none outside none;
            height: 20px;
            visibility: hidden;
        }
        
        .ui-skin-nav .li-skinitem
        {
            float: left;
            font-size: 12px;
            line-height: 30px;
            margin-left: 10px;
            text-align: center;
        }
        .ui-skin-nav .li-skinitem span
        {
            cursor: pointer;
            width: 10px;
            height: 10px;
            display: inline-block;
        }
        .ui-skin-nav .li-skinitem span.cs-skin-on
        {
            border: 1px solid #FFFFFF;
        }
        
        .ui-skin-nav .li-skinitem span.gray
        {
            background-color: gray;
        }
        .ui-skin-nav .li-skinitem span.pepper-grinder
        {
            background-color: #BC3604;
        }
        .ui-skin-nav .li-skinitem span.blue
        {
            background-color: blue;
        }
        .ui-skin-nav .li-skinitem span.cupertino
        {
            background-color: #D7EBF9;
        }
        .ui-skin-nav .li-skinitem span.dark-hive
        {
            background-color: black;
        }
        .ui-skin-nav .li-skinitem span.sunny
        {
            background-color: #FFE57E;
        }
        .img-menu
        {
            height: 18px;
            width: 18px;
        }
        .space
        {
            color: #E7E7E7;
        }
        .l-topmenu-welcome
        {
            position: absolute;
            height: 24px;
            line-height: 24px;
            right: 30px;
            top: 3px;
            color: #070A0C;
        }
    </style>
    <script type="text/javascript">
        function addTab(title, url) {
            if ($('#tabs').tabs('exists', title)) {
                $('#tabs').tabs('select', title); // Select and refresh 
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != ' Home ') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            } else {
                var content = createFrame(url);
                $('#tabs').tabs('add', {
                    title: title,
                    content: content,
                    closable: true
                });
            }
            tabClose();
        }
        function createFrame(url) {
            var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
            return s;
        }

        function tabClose() {
            /* Double-click Close TAB Tab */
            $(".tabs-inner").dblclick(function () {
                var subtitle = $(this).children(".tabs-closable").text();
                $('#tabs').tabs('close', subtitle);
            })
            /* Right binding for Tab */
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });

                var subtitle = $(this).children(".tabs-closable").text();

                $('#mm').data("currtab", subtitle);
                $('#tabs').tabs('select', subtitle);
                return false;
            });
        }
        // Binding context menu event 
        function tabCloseEven() {
            // Refresh 
            $('#mm-tabupdate').click(function () {
                var currTab = $('#tabs').tabs('getSelected');
                var url = $(currTab.panel('options').content).attr('src');
                if (url != undefined && currTab.panel('options').title != ' Home ') {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: createFrame(url)
                        }
                    })
                }
            })
            // Close the current 
            $('#mm-tabclose').click(function () {
                var currtab_title = $('#mm').data("currtab");
                $('#tabs').tabs('close', currtab_title);
            })
            // All off 
            $('#mm-tabcloseall').click(function () {
                $('.tabs-inner span').each(function (i, n) {
                    var t = $(n).text();
                    if (t != ' Home ') {
                        $('#tabs').tabs('close', t);
                    }
                });
            });
            // In addition to the current outside closed TAB
            $('#mm-tabcloseother').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                var nextall = $('.tabs-selected').nextAll();
                if (prevall.length > 0) {
                    prevall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).text();
                        if (t != ' Home ') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                if (nextall.length > 0) {
                    nextall.each(function (i, n) {
                        var t = $('a:eq(0) span', $(n)).text();
                        if (t != ' Home ') {
                            $('#tabs').tabs('close', t);
                        }
                    });
                }
                return false;
            });
            // Close the current on the right side TAB
            $('#mm-tabcloseright').click(function () {
                var nextall = $('.tabs-selected').nextAll();
                if (nextall.length == 0) {
                    //msgShow(' Prompted ',' Not really behind ~~','error');
                    alert(' Not really behind ~~');
                    return false;
                }
                nextall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });
            // Close the current on the left side TAB
            $('#mm-tabcloseleft').click(function () {
                var prevall = $('.tabs-selected').prevAll();
                if (prevall.length == 0) {
                    alert(' Coming to an end , Front no matter ~~');
                    return false;
                }
                prevall.each(function (i, n) {
                    var t = $('a:eq(0) span', $(n)).text();
                    $('#tabs').tabs('close', t);
                });
                return false;
            });

            // Drop out 
            $("#mm-exit").click(function () {
                $('#mm').menu('hide');
            })
        }

        $(function () {
            // The initial menu bar 
            Application.data.getLeftMenu(function (js) {
                var isSelect = false;
                var pushData = eval('(' + js + ')');
                var leftMenu = $(".cs-west div");
                for (var i = 0, l = pushData.length; i < l; i++) {
                    // If selected 
                    isSelect = false;
                    if (i == 0) isSelect = true;

                    var menu = $("<div></div>");
                    // Add Child 
                    if (pushData[i].Children.length > 0) {
                        var cMenu = pushData[i].Children;
                        for (var j = 0, k = cMenu.length; j < k; j++) {
                            menu.append("<a class='cs-navi-tab' href=javascript:addTab('" + cMenu[j].Name + "','" + cMenu[j].Url
                            + "');><img class='img-menu' align='middle' border=0 src='" + cMenu[j].Img
                            + "' />&nbsp;&nbsp;" + cMenu[j].Name + "</a>");
                        }
                    }
                    // Add a group 
                    $('.easyui-accordion').accordion('add', {
                        title: pushData[i].Name,
                        content: menu,
                        iconCls: 'icon-reload',
                        selected: isSelect
                    });
                }
            }, this);

            //tab Action event page 
            tabCloseEven();

            var themes = {
                'black': 'themes/black/easyui.css',
                'pepper-grinder': 'themes/pepper-grinder/easyui.css',
                'bootstrap': 'themes/bootstrap/easyui.css',
                'default': 'themes/default/easyui.css',
                'gray': 'themes/gray/easyui.css',
                'metro': 'themes/metro/easyui.css',
                'cupertino': 'themes/cupertino/easyui.css'
            };

            var skins = $('.li-skinitem span').click(function () {
                var $this = $(this);
                if ($this.hasClass('cs-skin-on')) return;
                skins.removeClass('cs-skin-on');
                $this.addClass('cs-skin-on');
                var skin = $this.attr('rel');
                $('#appstyle').attr('href', themes[skin]);
                setCookie('cs-skin', skin);
                skin == 'dark-hive' ? $('.cs-north-logo').css('color', '#FFFFFF') : $('.cs-north-logo').css('color', '#000000');
            });

            if (getCookie('cs-skin')) {
                var skin = getCookie('cs-skin');
                $('#appstyle').attr('href', themes[skin]);
                $this = $('.li-skinitem span[rel=' + skin + ']');
                $this.addClass('cs-skin-on');
                skin == 'dark-hive' ? $('.cs-north-logo').css('color', '#FFFFFF') : $('.cs-north-logo').css('color', '#000000');
            }
        });


        function setCookie(name, value) {// Two parameters , One is cookie  Name , One is the value 
            var Days = 30; //此 cookie  Will be stored  30 天
            var exp = new Date();    //new Date("December 31, 9998");
            exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
            document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
        }

        function getCookie(name) {//取cookies Function         
            var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
            if (arr != null) return unescape(arr[2]); return null;
        }
        function closeWin() {
            window.close();
        }
        function ReFreshCurrentPage() {
            var currTab = $('#tabs').tabs('getSelected');
            var url = $(currTab.panel('options').content).attr('src');
            if (url != undefined && currTab.panel('options').title != ' Home ') {
                $('#tabs').tabs('update', {
                    tab: currTab,
                    options: {
                        content: createFrame(url)
                    }
                })
            }   
        }
    </script>
</head>
<body class="easyui-layout">
    <div region="west" border="true" split="true" title=" Function Panel " class="cs-west">
        <div class="easyui-accordion" fit="true" border="false">
        </div>
    </div>
    <div id="mainPanle" region="center" border="true" border="false">
        <div id="tabs" class="easyui-tabs" fit="true" border="false" data-options="tools:'#tab-tools'" >
            <div title="BPM System Menu ">
                <iframe src="AppList.aspx" id="BPMMenu" style="border:0px; width:100%; height:99%;" ></iframe>
            </div>
        </div>
        <div id="tab-tools">
		    <a href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-reload'" onclick="ReFreshCurrentPage()"></a>
	    </div>
    </div>
    <div id="mm" class="easyui-menu cs-tab-menu">
        <div id="mm-tabupdate">
             Refresh </div>
        <div class="menu-sep">
        </div>
        <div id="mm-tabclose">
             Shut down </div>
        <div id="mm-tabcloseother">
             Close other </div>
        <div id="mm-tabcloseall">
             Collapse all </div>
    </div>
</body>
</html>
