<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthPlanCollect.aspx.cs"
    Inherits="CCFlow.AppDemoLigerUI.MonthPlanCollect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script type="text/javascript" language="javascript" src="Lodop/LodopFuncs.js"></script>
    <object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
        height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="install_lodop32.exe"></embed>
    </object>
    <link href="jquery/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="jquery/tablestyle.css" rel="stylesheet" type="text/css" />
    <link href="jquery/lib/ligerUI/skins/ligerui-icons.css" rel="stylesheet" type="text/css" />
    <script src="jquery/lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="js/AppData.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var LODOP; // Declared as a global variable      
        function PrintOneURL() {
            LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
            LODOP.PRINT_INIT(" Print month plan ");
            LODOP.ADD_PRINT_TBURL(30, 20, 730, "100%", 'MonthPlanRePort.aspx');
            LODOP.SET_PRINT_STYLEA(0, "HOrient", 3);
            LODOP.SET_PRINT_STYLEA(0, "VOrient", 3);
            LODOP.SET_SHOW_MODE("HIDE_PAPER_BOARD", 1);
            LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1); // Forward displayed when the transverse 
            LODOP.ADD_PRINT_TEXT(580, 660, 165, 22, "Page #/Total &");
            LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
            LODOP.SET_PRINT_STYLEA(0, "Horient", 3);
            LODOP.SET_PRINT_STYLEA(0, "Vorient", 3);
            LODOP.SET_PRINT_STYLEA(0, "TableHeightScope", 2);
            LODOP.PREVIEW();
        }
        // Toolbar event 
        function itemclick(item) {
            if (item.text == " New Schedule ") {
                Application.data.createMonthPlan(function (js, scope) {
                    if (js == "success") {
                        LoadGrid();
                    }
                }, this);
            } else if (item.text == " Export Excel") {
                $.ligerDialog.open({ title: ' Please wait ...', width: 200, content: ' Exporting , Please wait .....', url: "MonthPlanRePort.aspx?exporttype=xls" });
                setTimeout(function () {
                    $.ligerDialog.hide();
                }, 3000);

            } else if (item.text == " Export Word") {
                $.ligerDialog.open({ title: ' Please wait ...', width: 200, content: ' Exporting , Please wait .....', url: "MonthPlanRePort.aspx?exporttype=doc" });
                setTimeout(function () {
                    $.ligerDialog.hide();
                }, 3000);
            } else if (item.text == " Print ") {
                PrintOneURL();
            } else {
                var grid = $("#maingrid").ligerGetGridManager();
                var rows = grid.getCheckedRows();

                // Determine whether the selected line 
                if (rows.length == 0) {
                    $.ligerDialog.warn(' Please select the row and try again !');
                    return;
                }

                if (item.text == " Send ") {

                    $.ligerDialog.confirm(' Are you sure you want to send it to perform ?', function (yes) {
                        if (yes) {
                            var str = "";
                            $(rows).each(function () {
                                if (str != "") str += "^";
                                str += this.FK_Flow + "," + this.WorkID;
                            });
                            // Send execution method 
                            Application.data.workFlowManage("send", str, function (js, scope) {
                                if (js.statusText == null && js.statusText != "error") {
                                    LoadGrid();
                                    document.getElementById("msgDialog").innerHTML = js;
                                    ShowMsgDialog();
                                }
                            }, this);
                        }
                    });
                } else if (item.text == " Modification plan ") {

                    var topRow = rows[0];
                    var strTimeKey = "";
                    var date = new Date();
                    strTimeKey += date.getFullYear(); //年
                    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
                    strTimeKey += date.getDate(); //日
                    strTimeKey += date.getHours(); //HH
                    strTimeKey += date.getMinutes(); //MM
                    strTimeKey += date.getSeconds(); //SS
                    WinOpenIt("../WF/MyFlow.aspx?FK_Flow=" + topRow.FK_Flow + "&FK_Node=" + topRow.FK_Node
                           + "&FID=" + topRow.FID + "&WorkID=" + topRow.WorkID + "&IsRead=0&T=" + strTimeKey, topRow.WorkID, topRow.FlowName);

                } else if (item.text == " Delete Schedule ") {

                    $.ligerDialog.confirm(' Are you sure you want to delete the selected item ?', function (yes) {
                        if (yes) {
                            var str = "";
                            $(rows).each(function () {
                                if (str != "") str += "^";
                                str += this.FK_Flow + "," + this.WorkID;
                            });
                            // Delete method 
                            Application.data.workFlowManage("delete", str, function (js, scope) {
                                if (js.statusText == null && js.statusText != "error") {
                                    LoadGrid();
                                    document.getElementById("msgDialog").innerHTML = js;
                                    ShowMsgDialog();
                                }
                            }, this);
                        }
                    });
                }
            }
        }
        // Pop-up message box 
        function ShowMsgDialog() {
            // Open floor 
            $.ligerDialog.open({
                target: $("#msgDialog"),
                title: ' News ',
                width: 810,
                height: 500,
                isResize: true,
                modal: true,
                buttons: [{ text: ' Shut down ', onclick: function (i, d) {
                    d.hide();
                }
                }]
            });
        }
        // Open the form 
        function WinOpenIt(url, workId, text) {
            var isReadImg = document.getElementById(workId);
            if (isReadImg) isReadImg.src = "Img/Menu/Mail_Read.png";
            if (ccflow.config.IsWinOpenEmpWorks.toUpperCase() == "TRUE") {
                var val = window.showModalDialog(url, " Approval Process ", "dialogWidth=900px;dialogHeight=650px;dialogTop=50px;dialogLeft=60px");
                LoadGrid();
            } else {
                window.parent.f_addTab(workId, text, url);
            }
        }
        // Load List 
        function LoadGridCallBack(jsonData, scope) {
            if (jsonData) {
                var strTimeKey = "";
                var date = new Date();
                strTimeKey += date.getFullYear(); //年
                strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
                strTimeKey += date.getDate(); //日
                strTimeKey += date.getHours(); //HH
                strTimeKey += date.getMinutes(); //MM
                strTimeKey += date.getSeconds(); //SS
                var month = date.getMonth() + 1;
                var dateNow = date.getFullYear() + "-" + month + "-" + date.getDate();

                var pushData = eval('(' + jsonData + ')');
                var grid = $("#maingrid").ligerGrid({
                    columns: [
                   { display: ' Title ', name: 'Title', width: 340, align: 'left', render: function (rowdata, rowindex) {
                       var title = "";
                       if (rowdata.ISREAD == 0) {
                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlow.aspx?FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FK_NODE
                           + "&FID=" + rowdata.FID + "&WorkID=" + rowdata.WORKID + "&IsRead=0&T=" + strTimeKey
                           + "','" + rowdata.WORKID + "','" + rowdata.FLOWNAME + "');\" ><img align='middle' alt='' id='" + rowdata.WORKID
                           + "' src='Img/Menu/Mail_UnRead.png' border=0/>" + rowdata.TITLE + "</a>";
                       } else {
                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlow.aspx?FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FK_NODE
                           + "&FID=" + rowdata.FID + "&T=" + strTimeKey + "&WorkID=" + rowdata.WORKID + "','" + rowdata.WORKID
                           + "','" + rowdata.FLOWNAME + "');\"  ><img border=0 align='middle' id='" + rowdata.WORKID + "' alt='' src='Img/Menu/Mail_Read.png'/>" + rowdata.TITLE + "</a>";
                       }
                       return title;
                   }
                   },
                   { display: ' Work Item ', name: 'GongZuoXiangMu' },
                   { display: ' The current node ', name: 'NodeName' },
                   { display: ' Sponsor ', name: 'JiHuaBianZhiRen' },
                   { display: ' Launch date ', name: 'RDT' },
                   { display: ' Accepted ', name: 'ADT' },
                   { display: ' The term ', name: 'SDT' },
                   { display: ' Status ', name: 'FlowState', render: function (rowdata, rowindex) {
                       var datePattern = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
                       if (datePattern.test(rowdata.SDT)) {
                           var d1 = new Date(dateNow.replace(/-/g, "/"));
                           var d2 = new Date(rowdata.SDT.replace(/-/g, "/"));

                           if (Date.parse(d1) <= Date.parse(d2)) {
                               return " Normal ";
                           }
                           return "<font color=red> Overdue </font>";
                       }
                   }
                   }
                   ],
                    data: pushData,
                    rownumbers: true,
                    checkbox: true,
                    selectRowButtonOnly: true,
                    height: "99%",
                    width: "99%",
                    columnWidth: 100,
                    pageSize: 50,
                    onReload: LoadGrid,
                    toolbar: { items: [
                    { text: ' Send ', click: itemclick, icon: 'msn' },
                    { line: true },
                    { text: ' New Schedule ', click: itemclick, icon: 'add' },
                    { text: ' Modification plan ', click: itemclick, icon: 'modify' },
                    { text: ' Delete Schedule ', click: itemclick, icon: 'delete' },
                    { line: true },
                        //{ text: ' Export Excel', click: itemclick, icon: 'excel' },
                    {text: ' Export Word', click: itemclick, icon: 'Word' },
                    { text: ' Print ', click: itemclick, icon: 'print' }
                    ]
                    }
                });
            }
            else {
                $.ligerDialog.warn(' Error loading data , Please retry close !');
            }
            $("#pageloading").hide();
        }

        // Load List 
        function LoadGrid() {
            $("#pageloading").show();
            Application.data.monthPlanCollect(LoadGridCallBack, this);
        }
        // Page initialization 
        $(function () {
            LoadGrid();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div id="maingrid" style="margin: 0; padding: 0;">
    </div>
    <div id="msgDialog" style="display: none">
    </div>
    </form>
</body>
</html>
