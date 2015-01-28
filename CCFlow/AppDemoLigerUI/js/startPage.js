var strTimeKey = "";
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

// Load initiate the process list 
function LoadGrid() {
    var grid = $('#maingrid').datagrid({
        title: ' The process can be initiated ',
        iconCls: 'icon-title',
        width: "auto",
        height: "auto",
        nowrap: true,
        fitColumns: true,
        autoRowHeight: false,
        singleSelect: true,
        striped: true,
        collapsible: false,
        url: '/AppDemoLigerUI/Base/DataService.aspx?method=startflow',
        pagination: false,
        rownumbers: true,
        columns: [[
                    { field: 'Name', title: ' Name ', width: 380, align: 'left', formatter: function (value, rec) {
                        var h = "";
                        if (rec.STARTLISTURL) {
                            h = rec.STARTLISTURL + "?FK_Flow=" + rec.NO + "&FK_Node=" + rec.NO + "01&T=" + strTimeKey;
                            return "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')>" + rec.NAME + "</a>";
                        }
                        h = "../WF/MyFlow.aspx?FK_Flow=" + rec.NO + "&FK_Node=" + rec.NO + "01&T=" + strTimeKey;
                        return "<a href='javascript:void(0);' onclick=ShowEasyUiTitleDiv('" + rec.NO + "','" + rec.NAME + "','" + h + "')>" + rec.NAME + "</a>";
                    }
                    },
                    { field: 'IsBatchStart', title: ' Batch launched ', formatter: function (value, rec) {
                        var h = "";
                        if (rec.ISBATCHSTART == "1") {
                            h = "../WF/BatchStart.aspx?FK_Flow=" + rec.NO;
                            h = "<a href='javascript:void(0);' onclick=StartListUrl('" + h + "')> Batch launched </a>";
                        }
                        return h;
                    }
                    },
                    { field: 'RoleType', title: ' Flow chart ', formatter: function (value, rec) {
                        return "<a href='javascript:void(0);' onclick=OpenEasyUiFlowPicture('" + rec.NO + "','" + rec.NAME + "')> Turn on </a>";
                    }
                    },
                    { field: 'HistoryFlow', title: ' History launched ', width: 180, formatter: function (value, rec) {
                        return "<a href='javascript:void(0);' onclick=ShowEasyUiHistoryData('" + rec.NO + "','" + rec.NAME + "')> Check out </a>";
                    }
                    },
                    { field: 'Note', title: ' Description ', width: 280, formatter: function (value, rec) {
                        if (rec.NOTE == null || rec.NOTE == "") {
                            return "Nothing";
                        }
                        return rec.NOTE;
                    }
                    }
                ]]

    });
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
                fit:true,
                url: '/AppDemoLigerUI/Base/DataService.aspx?method=historystartflow&FK_Flow=' + flowNo,
                columns: [[
            { title: ' Title ', field: 'Title', width: 320, align: 'left', formatter: function (value, rec) {
                var h = "../WF/WFRpt.aspx?WorkID=" + rec.OID + "&FK_Flow=" + flowNo + "&FID=" + rec.FID + "&T=" + strTimeKey;
                return "<a href='javascript:void(0);' onclick=WinOpenWindow('" + h + "')>" + rec.TITLE + "</a>";
            }
            },
            { title: ' Start Time ', field: 'FlowStartRDT' },
            { title: ' Participants ', field: 'FlowEmps', width: 300 }
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
            if (ccflow.config.IsWinOpenStartWork == 1) {
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