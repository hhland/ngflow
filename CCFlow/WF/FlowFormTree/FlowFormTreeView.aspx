<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowFormTreeView.aspx.cs" Inherits="CCFlow.WF.SheetTree.FlowFormTreeView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Style/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Style/themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link href="../Style/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Scripts/FlowFormTreeData.js" type="text/javascript"></script>
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
            height: 35px;
        }
        .cs-north-bg
        {
            width: 100%;
            height: 100%;
            background: url(Images/Banal.png) repeat-x bottom;
        }
        .cs-north-logo
        {
            position: absolute;
            top: 5px;
            padding-left: 60px;
        }
        .cs-west
        {
            width: 260px;
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
        function addTab(id, title, url) {
            if ($('#tabs').tabs('exists', title)) {
                $('#tabs').tabs('select', title); // Select and refresh 
                var currTab = $('#tabs').tabs('getSelected');
                //                var url = $(currTab.panel('options').content).attr('src');
                //                if (url != undefined && currTab.panel('options').title != ' Home ') {
                //                    $('#tabs').tabs('update', {
                //                        tab: currTab,
                //                        options: {
                //                            content: createFrame(url)
                //                        }
                //                    })
                //                }
            } else {
                var content = createFrame(url);
                $('#tabs').tabs('add', {
                    title: title,
                    id: id,
                    content: content,
                    closable: true
                });
            }
            tabClose();
        }

        // Determine whether there is tab 
        function TabFormExists() {
            var currTab = $('#tabs').tabs('getSelected');
            if (currTab) return true;

            return false;
        }
        function ChangTabFormTitle() {
            
        }
        function createFrame(url) {
            var s = '<iframe scrolling="auto" frameborder="0" Onblur="OnTabChange(this)"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
            return s;
        }
        //tab Switching event 
        function OnTabChange(scope) {
        }

        function tabClose() {
            /* Double-click Close TAB Tab */
            $(".tabs-inner").dblclick(function () {
                var currTab = $('#tabs').tabs('getSelected');
                if (currTab) {
                    var currtab_title = currTab.panel('options').title;
                    $('#tabs').tabs('close', currtab_title);
                }
            })
            /* Right binding for Tab */
            $(".tabs-inner").bind('contextmenu', function (e) {
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
                var subtitle = "";
                var currTab = $('#tabs').tabs('getSelected');
                if (currTab) {
                    subtitle = currTab.panel('options').title;
                }

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
        // Get parameters 
        var RequestArgs = function () {
            this.WorkID = Application.common.getArgsFromHref("WorkID");
            this.CWorkID = Application.common.getArgsFromHref("CWorkID");

            // Parent process 
            this.PWorkID = Application.common.getArgsFromHref("PWorkID");
            this.PFlowNo = Application.common.getArgsFromHref("PFlowNo");

            this.FK_Flow = Application.common.getArgsFromHref("FK_Flow");
            this.FK_Node = Application.common.getArgsFromHref("FK_Node");
            if (this.FK_Node) {
                while (this.FK_Node.substring(0, 1) == '0') this.FK_Node = this.FK_Node.substring(1);
                this.FK_Node = this.FK_Node.replace('#', '');
            }
            this.NodeID = Application.common.getArgsFromHref("NodeID");
            this.UserNo = Application.common.getArgsFromHref("UserNo");
            this.FID = Application.common.getArgsFromHref("FID");
            this.SID = Application.common.getArgsFromHref("SID");

            this.DoFunc = Application.common.getArgsFromHref("DoFunc");
            this.CFlowNo = Application.common.getArgsFromHref("CFlowNo");
            this.WorkIDs = Application.common.getArgsFromHref("WorkIDs");
            this.IsLoadData = 1;
        }
        // Mass participation 
        var urlExtFrm = function () {
            var extUrl = "";
            var args = new RequestArgs();
            if (args.WorkID != "")
                extUrl += "&WorkID=" + args.WorkID;

            extUrl += "&CWorkID=" + args.CWorkID;

            if (args.FK_Flow != "")
                extUrl += "&FK_Flow=" + args.FK_Flow;

            if (args.FK_Node != "")
                extUrl += "&FK_Node=" + args.FK_Node;
            if (args.NodeID != "")
                extUrl += "&NodeID=" + args.NodeID;
            if (args.UserNo != "")
                extUrl += "&UserNo=" + args.UserNo;
            if (args.FID != "")
                extUrl += "&FID=" + args.FID;
            if (args.SID != "")
                extUrl += "&SID=" + args.SID;
            if (args.IsLoadData != "")
                extUrl += "&IsLoadData=" + args.IsLoadData;

            if (args.PWorkID != "")
                extUrl += "&PWorkID=" + args.PWorkID;
            if (args.PFlowNo != "")
                extUrl += "&PFlowNo=" + args.PFlowNo;

            // Get other parameters 
            var sHref = window.location.href;
            var args = sHref.split("?");
            var retval = "";
            if (args[0] != sHref) /* Parameter is not empty */
            {
                var str = args[1];
                args = str.split("&");
                for (var i = 0; i < args.length; i++) {
                    str = args[i];
                    var arg = str.split("=");
                    if (arg.length <= 1) continue;
                    // It does not contain added 
                    if (extUrl.indexOf(arg[0]) == -1) {
                        extUrl += "&" + arg[0] + "=" + arg[1];
                    }
                }
            }
            return extUrl;
        }

        $(function () {
            $("#pageloading").show();
            // Initial Toolbar 
            var args = new RequestArgs();
            // Form tree 
            Application.data.getFlowFormTree(args.FK_Flow, args.FK_Node, function (js) {
                var isSelect = false;
                var pushData = eval('(' + js + ')');
                // Load category tree 
                $("#flowFormTree").tree({
                    data: pushData,
                    iconCls: 'tree-folder',
                    collapsed: true,
                    lines: true,
                    onClick: function (node) {
                        if (node.attributes.NodeType == "form|0" || node.attributes.NodeType == "form|1") {/* Normal Forms and Required Forms */
                            var urlExt = urlExtFrm();
                            var url = "../CCForm/Frm.aspx?FK_MapData=" + node.id + "&IsEdit=0&IsPrint=0" + urlExt;
                            addTab(node.id, node.text, url);
                        } else if (node.attributes.NodeType == "tools|0") {
                            var urlExt = urlExtFrm();
                            var url = node.attributes.Url;
                            while (url.indexOf('|') >= 0) {
                                url = url.replace('|', '/');
                            }
                            if (url.indexOf('?') > 0) {
                                url = url + "&FK_MapData=" + node.id + "&" + urlExt;
                            }
                            else {
                                url = url + "?FK_MapData=" + node.id + "&" + urlExt;
                            }
                            addTab(node.id, node.text, url);
                        } else if (node.attributes.NodeType == "tools|1") {
                            var urlExt = urlExtFrm();
                            var url = node.attributes.Url;
                            while (url.indexOf('|') >= 0) {
                                url = url.replace('|', '/');
                            }
                            if (url.indexOf('?') > 0) {
                                url = url + "&FK_MapData=" + node.id + "&" + urlExt;
                            }
                            else {
                                url = url + "?FK_MapData=" + node.id + "&" + urlExt;
                            }
                            WinOpenPage("_blank", url, node.text);
                        }
                    }
                });
                $("#pageloading").hide();
            }, this);

            //tab Action event page 
            tabCloseEven();
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
        // Open the form 
        function WinOpenPage(target, url, title) {
            window.open(url, target, "left=0,top=0,width=" + (screen.availWidth - 10) + ",height=" + (screen.availHeight - 50) + ",scrollbars,resizable=yes,toolbar=yes,menubar=yes'");
        }
        function WinOpen(url, winName) {
            var newWindow = window.open(url, winName, 'width=700,height=400,top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
            newWindow.focus();
            return;
        }
        function closeWin() {
            window.close();
        }
    </script>
</head>
<body class="easyui-layout">
    <div id="pageloading">
    </div>
    <div region="west" border="true" split="true" title=" Data Tree " class="cs-west">
        <ul id="flowFormTree" class="easyui-tree" data-options="animate:true,dnd:false">
        </ul>
    </div>
    <div id="mainPanle" region="center" border="true" border="false">
        <div id="tabs" class="easyui-tabs" fit="true" border="false" data-options="tools:'#tab-tools'">
        </div>
    </div>
    <div id="msgPanel">
        <div id="content" style="height: 440px; overflow: auto; padding:10px 10px 10px 10px;"></div>
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
