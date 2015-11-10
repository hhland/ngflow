var querySta = "unread";
var dateNow = "";
// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                   { display: ' Title ', name: 'Title', width: 380, align: 'left', render: function (rowdata, rowindex) {
                       var title = "<a href=javascript:WinOpenIt('" + rowdata.MYPK + "','" + rowdata.FK_FLOW + "','" + rowdata.FK_NODE
                       + "','" + rowdata.WORKID + "','" + rowdata.FID + "','" + rowdata.STA + "');><img align=middle src='Img/Menu/CCSta/" + rowdata.STA
                       + ".png' id=" + rowdata.MYPK + ">" + rowdata.TITLE + "</a> Date :" + rowdata.RDT;
                       return title;
                   }
                   },
                   { display: ' Process Name ', name: 'FlowName' },
                   { display: ' The current node ', name: 'NodeName' },
                   { display: ' Content ', name: 'Doc', width: 200 },
                   { display: ' Cc ', name: 'Rec' },
                   { display: ' Cc date ', name: 'RDT' }
                   ],
            pageSize: 20,
            data: pushData,
            rownumbers: true,
            height: "99%",
            width: "99%",
            columnWidth: 100,
            onReload: LoadGrid,
            onDblClickRow: function (rowdata, rowindex) {
                WinOpenIt(rowdata.MYPK, rowdata.FK_FLOW, rowdata.FK_NODE, rowdata.WORKID, rowdata.FID, rowdata.STA);
            }
        });
    }
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
    $("#pageloading").hide();
}
// Load CC list 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getCCFlowList(querySta, callBack, this);
}
// Open the form 
function WinOpenIt(ccid, fk_flow, fk_node, workid, fid, sta) {
    var url = '';
    if (sta == '0') {
        url = '../WF/Do.aspx?DoType=DoOpenCC&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&WorkID=' + workid + '&FID=' + fid + '&Sta=' + sta + '&MyPK=' + ccid + "&T=" + dateNow;
    }
    else {
        url = '../WF/WorkOpt/OneWork/Track.aspx?FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&WorkID=' + workid + '&FID=' + fid + '&Sta=' + sta + '&MyPK=' + ccid + "&T=" + dateNow;
    }
    //window.parent.f_addTab("cc" + fk_flow + workid, " Cc " + fk_flow + workid, url);
    var newWindow = window.open(url, 'z');
    newWindow.focus();
    if (querySta == "all" || querySta == "unread")

        LoadGrid();
}
// Toolbar event 
function itemclick(item) {
    if (item.text == " Whole ") {
        querySta = "all";
    }
    else if (item.text == " Unread ") {
        querySta = "unread";
    }
    else if (item.text == " Read ") {
        querySta = "isread";
    }
    else if (item.text == " Deleted ") {
        querySta = "delete";
    }
    LoadGrid();
}

$(function () {

    dateNow = "";
    var date = new Date();
    dateNow += date.getFullYear(); //年
    dateNow += date.getMonth() + 1; //月  May be less than the actual month 1
    dateNow += date.getDate(); //日
    dateNow += date.getHours(); //HH
    dateNow += date.getMinutes(); //MM
    dateNow += date.getSeconds(); //SS

    var toolBarManager = $("#toptoolbar").ligerToolBar({ items: [
                { text: ' Whole ', click: itemclick, icon: 'database' },
                { line: true },
                { text: ' Unread ', click: itemclick, icon: 'outbox' },
                { line: true },
                { text: ' Read ', click: itemclick, icon: 'mailbox' },
                { line: true },
                { text: ' Deleted ', click: itemclick, icon: 'attibutes' }
            ]
    });
    LoadGrid();
});