var strTimeKey = "";
// Initiate the process 
function StartFlow(url) {

}
// Open flowchart 
//function OpenFlowPicture(flowNo, flowName) {
//    var pictureUrl = "../WF/Chart.aspx?FK_Flow=" + flowNo + "&DoType=Chart&T=" + strTimeKey;
//    var win = $.ligerDialog.open({
//        height: 500, width: 800, url: pictureUrl, showMax: true, isResize: true, modal: true, title: flowName + " Flow chart ", slide: false, buttons: [{
//            text: ' Shut down ', onclick: function (item, Dialog, index) {
//                win.hide();
//            }
//        }]
//    });
//}
// Initiate workflow 
function StartListUrl(url) {
    var v = window.showModalDialog(url, 'sd', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
    if (v == null || v == "")
        return;
}
// Open the form 
function WinOpenIt(tabid, text, url) {
    if (ccflow.config.IsWinOpenStartWork == 1) {
        window.parent.f_addTab(tabid + strTimeKey, text, url);
    } else {
        var winWidth = 850;
        var winHeight = 680;
        if (screen && screen.availWidth) {
            winWidth = screen.availWidth;
            winHeight = screen.availHeight;
        }
        //var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
        window.showModalDialog(url, "", "scrollbars=yes;resizable=yes;center=yes;dialogWidth=" + winWidth + ";dialogHeight=" + winHeight + ";dialogTop=50px;dialogLeft=50px;");
    }
}
function WinOpenWindow(url) {
    var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
    newWindow.focus();
}
// Enter a title box 
//function ShowTitleDiv(tabid, text, url) {
//    // Run 
//    Application.data.createEmptyCase(tabid, "", function (js, scope) {
//        if (js == "addform") {
//            if (ccflow.config.IsWinOpenStartWork == 1) {
//                WinOpenIt(tabid, text, url);
//            } else {
//                window.parent.f_addTab(tabid + strTimeKey, text, url);
//            }
//        } else {
//            // Open floor 
//            $.ligerDialog.open({
//                target: $("#divTitle"),
//                title: ' New - ' + text + " Process ",
//                width: 510,
//                height: 180,
//                isResize: true,
//                modal: true,
//                buttons: [{ text: ' Determine ', onclick: function (i, d) {
//                    var title = $("#TB_Title").val();

//                    if (title == "") {
//                        $.ligerDialog.warn(' The title may not be empty !');
//                        return;
//                    }
//                    // Run 
//                    Application.data.createEmptyCase(tabid, title, function (js, scope) {
//                        $("#TB_Title").val("");
//                        d.hide();
//                        WinOpenIt(tabid, text, js);
//                    }, this);
//                }
//                }, { text: ' Cancel ', onclick: function (i, d) {
//                    $("#TB_Title").val("");
//                    d.hide();
//                }
//                }]
//            });
//        }
//    }, this);
//}
// Display history launched 
//function ShowHistoryData(flowNo, flowName) {
//    // Open floor 
//    $.ligerDialog.open({
//        target: $("#showHistory"),
//        title: flowName + '- History initiate List ',
//        width: 810,
//        height: 500,
//        isResize: true,
//        modal: true,
//        buttons: [{ text: ' Shut down ', onclick: function (i, d) {
//            d.hide();
//        }
//        }]
//    });
//    LoadHistoryGrid(flowNo);
//}
// Initiate data loading history 
//function LoadHistoryGrid(flowNo) {
//    $("#pageloading").show();
//    Application.data.getHistoryStartFlow(flowNo, function (json) {
//        if (json) {
//            var pushData = eval('(' + json + ')');
//            var grid = $("#historyGrid").ligerGrid({
//                columns: [
//                   { display: ' Title ', name: 'Title', width: 320, align: 'left', render: function (rowdata, rowindex) {
//                       var h = "../WF/WFRpt.aspx?WorkID=" + rowdata.OID + "&FK_Flow=" + flowNo + "&FID=" + rowdata.FID + "&T=" + strTimeKey;
//                       return "<a href='javascript:void(0);' onclick=WinOpenWindow('" + h + "')>" + rowdata.Title + "</a>";
//                   }
//                   },
//                   { display: ' Start Time ', name: 'FlowStartRDT' },
//                   { display: ' Participants ', name: 'FlowEmps', width: 300 }
//                   ],
//                pageSize: 20,
//                data: pushData,
//                rownumbers: true,
//                width: 780,
//                height: 430,
//                columnWidth: 100,
//                onDblClickRow: function (rowdata, rowindex) {
//                    WinOpenWindow("../WF/WFRpt.aspx?WorkID=" + rowdata.OID + "&FK_Flow=" + flowNo + "&FID=" + rowdata.FID + "&T=" + strTimeKey);
//                }
//            });
//        }
//        $("#pageloading").hide();
//    }, this);
//}
// Callback 
//function callBack(jsonData, scope) {
//    if (jsonData) {
//        var pushData = eval('(' + jsonData + ')');
//        var grid = $("#maingrid").ligerGrid({
//            columns: [
//                   { display: ' Name ', name: 'Name', width: 380, align: 'left', render: function (rowdata, rowindex) {
//                       var h = "";
//                       if (rowdata.StartListUrl) {
//                           h = rowdata.StartListUrl + "?FK_Flow=" + rowdata.No + "&FK_Node=" + rowdata.No + "01&T=" + strTimeKey;
//                           return "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')>" + rowdata.Name + "</a>";
//                       }
//                       h = "../WF/MyFlow.aspx?FK_Flow=" + rowdata.No + "&FK_Node=" + rowdata.No + "01&T=" + strTimeKey;
//                       return "<a href='javascript:void(0);' onclick=ShowTitleDiv('" + rowdata.No + "','" + rowdata.Name + "','" + h + "')>" + rowdata.Name + "</a>";
//                       
//                   }
//                   },
//                   { display: ' Batch launched ', name: 'IsBatchStart', render: function (rowdata, rowindex, value) {
//                       var h = "";
//                       if (rowdata.IsBatchStart == "1") {
//                           h = "../WF/BatchStart.aspx?FK_Flow=" + rowdata.No;
//                           h = "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')> Batch launched </a>";
//                       }
//                       return h;

//                   }
//                   },
//                   { display: ' Flow chart ', name: 'RoleType', render: function (rowdata, rowindex, value) {
//                       return "<a href='javascript:void(0);' onclick=OpenFlowPicture('" + rowdata.No + "','" + rowdata.Name + "')> Turn on </a>";
//                   }
//                   }, {
//                       display: ' History launched ', name: 'HistoryFlow', width: 180, render: function (rowdata, rowindex) {
//                           return "<a href='javascript:void(0);' onclick=ShowHistoryData('" + rowdata.No + "','" + rowdata.Name + "')> Check out </a>";
//                       }
//                   },
//                   { display: ' Description ', name: 'Note', width: 280, render: function (rowdata, rowindex) {
//                       if (rowdata.Note == null || rowdata.Note == "") {
//                           return "Nothing";
//                       }
//                       return rowdata.Note;
//                   }
//                   }
//                   ],
//            pageSize: 20,
//            data: pushData,
//            rownumbers: true,
//            height: "99%",
//            width: "99%",
//            columnWidth: 120,
//            onReload: LoadGrid,
//            groupColumnName: 'FK_FlowSortText',
//            groupColumnDisplay: ' Type ',
//            onDblClickRow: function (rowdata, rowindex) {
//                OpenFlowPicture(rowdata.No, rowdata.Name);
//                //WinOpenIt( "../WF/MyFlow.aspx?FK_Flow=" + rowdata.No + "&FK_Node=" + rowdata.No + "01&T=" + strTimeKey);
//            }
//        });
//    }
//    else {
//        $.ligerDialog.warn(' Error loading data , Please retry close !');
//    }
//    $("#pageloading").hide();
//}
function callBack(jsonData, scope) {
    if (jsonData) {

        
        var grid = $('#maingrid').treegrid({
            fitColumns: true,
            idField: 'NO',
            treeField: 'NAME',
            url: '/AppDemoLigerUI/Base/DataService.aspx?method=startflowTree',
            method: 'get',

            columns: [[

                    { field: 'NAME', title: ' Name ', width: 380, align: 'left', formatter: function (value, rec) {
                        if (rec.FK_FLOWSORT == null)
                            return value;
                        var h = "";
                        if (rec.STARTLISTURL) {
                            h = rec.STARTLISTURL + "?FK_Flow=" + rec.NO + "&FK_Node=" + rec.NO + "01&T=" + strTimeKey;
                            return "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')>" + rec.NAME + "</a>";
                        }
                        h = "../WF/MyFlow.aspx?FK_Flow=" + rec.NO + "&FK_Node=" + rec.NO + "01&T=" + strTimeKey;
                        return "<a href='javascript:void(0);' onclick=ShowEasyUiTitleDiv('" + rec.NO + "','" + rec.NAME + "','" + h + "')>" + rec.NAME + "</a>";
                    }
                    },
                    { field: 'ISBATCHSTART', title: ' Batch launched ', formatter: function (value, rec) {
                        if (rec.FK_FLOWSORT == null)
                            return value;
                        var h = "";
                        if (rec.ISBATCHSTART == "1") {
                            h = "../WF/BatchStart.aspx?FK_Flow=" + rec.NO;
                            h = "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')> Batch launched </a>";
                        }
                        return h;
                    }
                    },
                    { field: 'ROLETYPE', title: ' Flow chart ', formatter: function (value, rec) {
                        if (rec.FK_FLOWSORT == null)
                            return value;
                        return "<a href='javascript:void(0);' onclick=OpenEasyUiFlowPicture('" + rec.NO + "','" + rec.NAME + "')> Turn on </a>";
                    }
                    },
                    { field: 'HISTORYFLOW', title: ' History launched ', width: 180, formatter: function (value, rec) {
                        if (rec.FK_FLOWSORT == null)
                            return value;
                        return "<a href='javascript:void(0);' onclick=ShowEasyUiHistoryData('" + rec.NO + "','" + rec.NAME + "')> Check out </a>";
                    }
                    },
                    { field: 'NOTE', title: ' Description ', width: 280, formatter: function (value, rec) {
                        if (rec.FK_FLOWSORT == null)
                            return value;
                        if (rec.NAME == null || rec.NAME == "") {
                            return "Nothing";
                        }
                        return rec.NAME;
                    }
                    }
                ]],
            onLoadSuccess: function (row, data) {
                $('#maingrid').treegrid('expandAll');
            }

        });
    }
    else {
        $.messager.alert(' Prompt ', ' Error loading data , Please retry close !');
    }
    $("#pageloading").hide();
}

// Initiate data loading history 
function LoadEasyUiHistoryGrid(flowNo) {
    $("#pageloading").show();
    Application.data.getHistoryStartFlow(flowNo, function (json) {
        if (json) {
            var grid = $("#historyGrid").datagrid({
                pagination: true,
                nowrap: true,
                fitColumns: true,
                autoRowHeight: false,
                striped: true,
                collapsible: false,
                url: '/AppDemoLigerUI/Base/DataService.aspx?method=historystartflow&FK_Flow=' + flowNo,
                columns: [[
            { title: ' Title ', field: 'TITLE', width: 320, align: 'left', formatter: function (value, rec) {
                var h = "../WF/WFRpt.aspx?WorkID=" + rec.OID + "&FK_Flow=" + flowNo + "&FID=" + rec.FID + "&T=" + strTimeKey;
                return "<a href='javascript:void(0);' onclick=WinOpenWindow('" + h + "')>" + rec.TITLE + "</a>";
            }
            },
            { title: ' Start Time ', field: 'FLOWSTARTRDT' },
            { title: ' Participants ', field: 'FLOWEMPS', width: 300 }
            ]],
                rownumbers: true,
                width: 780,
                height: 430
            });
        }
        $("#pageloading").hide();
    }, this)
}
// Open flowchart 
function OpenEasyUiFlowPicture(flowNo, flowName) {
    var pictureUrl = "/DataUser/FlowDesc/" + flowNo + "." + flowName + "/Flow.png";
    //$.jBox("iframe:" + pictureUrl, {
    //    title: flowName + " Flow chart ",
    //    width: 800,
    //    height: 500,
    //    buttons: { ' Shut down ': true }
    //});
    document.getElementById("FlowPic").src = pictureUrl;
    $("#flowPicDiv").dialog({
        height: 500,
        width: 800,
        showMax: true,
        isResize: true,
        modal: true,
        title: flowName + " Flow chart ",
        slide: false,
        buttons: [{ text: ' Shut down ', handler: function () {
            $('#flowPicDiv').dialog('close');
        }
        }]
    });
}
// Display history launched 
function ShowEasyUiHistoryData(flowNo, flowName) {
    // Open floor 
    $("#showHistory").dialog({
        title: flowName + '- History initiate List ',
        width: 810,
        height: 500,
        modal: true,
        buttons: [{ text: ' Shut down ', handler: function () {
            $('#showHistory').dialog('close');
        }
        }]
    });
    LoadEasyUiHistoryGrid(flowNo);
}
// Enter a title box 
function ShowEasyUiTitleDiv(tabid, text, url) {
    // Run 
    Application.data.createEmptyCase(tabid, "", function (js, scope) {
        if (js == "addform") {
            if (ccflow.config.IsWinOpenStartWork == 0) {
                WinOpenIt(tabid, text, url);
            } else {
                window.parent.f_addTab(tabid + strTimeKey, text, url);
            }
        } else {
            // Open floor 
            $('#divTitle').show();
            $("#divTitle").dialog({
                title: ' New - ' + text + " Process ",
                width: 510,
                height: 180,
                resizable: true,
                buttons: [{ text: ' Determine ', handler: function () {
                    var title = $("#TB_Title").val();

                    if (title == "") {
                        $.messager.alert(' Prompt ', ' The title may not be empty !');
                        return;
                    }
                    // Run 
                    Application.data.createEmptyCase(tabid, title, function (js, scope) {
                        $("#TB_Title").val("");
                        $('#divTitle').dialog('close');
                        WinOpenIt(tabid, text, js);
                    });
                }
                }, { text: ' Cancel ', handler: function (i, d) {
                    $("#TB_Title").val("");
                    //d.hide();
                    $('#divTitle').dialog('close');
                }
                }]
            });
        }
    }, this);
}
// Load initiate the process list 
function LoadGrid() {
    $("#pageloading").show();
    $("#divTitle").hide();
    Application.data.getStartFlowTree(callBack, this);
}

$(function () {
    strTimeKey = "";
    var date = new Date();
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS
    LoadGrid();
});