
function OpenIt(url) {
    window.open(url, 'card', 'width=700,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false');
}

// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                   { display: ' Title ', name: 'Title', width: 380, align: 'left', render: function (rowdata, rowindex) {
                       var h = "../WF/WFRpt.aspx?WorkID=" + rowdata.OID + "&FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FLOWENDNODE+"&T="+dateNow;
                       return "<a href='javascript:void(0);' onclick=OpenIt('" + h + "') >" + rowdata.TITLE + "</a>";
                   }
                   },
                     { display: ' Sponsor ', name: 'FlowStarter' },
                     { display: ' Launch date ', name: 'FlowStartRDT' },
                       { display: ' Status ', name: 'WFState', render: function (rowdata, rowindex) {
                           if (rowdata.WFSTATE == "0") {
                               return " Unfinished ";
                           }
                           else if (rowdata.WFSTATE == "1") {
                               return " Completed ";
                           }
                           else {
                               return " Unknown ";
                           }
                       }
                       },
                    { display: ' Participants ', name: 'FlowEmps',width:300 }
                   ],
            pageSize: 20,
            data: pushData,
            rownumbers: true,
            height: "99%",
            width: "99%",
            columnWidth: 120,
            groupColumnName: 'FlowName',
            groupColumnDisplay: ' Process Type ',
            onDblClickRow: function (rowdata, rowindex) {
                OpenIt("../WF/WFRpt.aspx?WorkID=" + rowdata.OID + "&FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FLOWENDNODE+"&T="+dateNow);
            }
        });
        $("#pageloading").hide();
    }
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
}
// Load Data 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.keySearch(false, 9, 'all', callBack, this);
}
// Tools menu events 
function itemclick(item) {
    if (item.text == " By Job ID查") {
        queryType = "workid";
    } else if (item.text == " Title field investigation process by keyword ") {
        queryType = "title";
    }
    else if (item.text == " Check all fields keywords ") {
        queryType = "all";
    }
    $.ligerDialog.open({
        target: $("#showKey"),
        title: ' Enter the query ',
        width: 380,
        height: 100,
        isResize: true,
        modal: true,
        buttons: [{ text: ' Determine ', onclick: function (i, d) {
            ckbOwner = $("#cbkQueryType");
            ckbOwner = ckbOwner[0].checked;
            contentKey = $("#txtKey").val();
            $("#pageloading").show();
            Application.data.keySearch(ckbOwner, contentKey, queryType, callBack, this);
            d.hide();
        }
        }, { text: ' Shut down ', onclick: function (i, d) {
            d.hide();
        }
        }]
    });
}
// Variable 
var ckbOwner = false;
var contentKey = "";
var queryType = "all";
var dateNow = "";
// Initial 
$(function () {
    dateNow = "";
    var date = new Date();
    dateNow += date.getFullYear(); //年
    dateNow += date.getMonth() + 1; //月  May be less than the actual month 1
    dateNow += date.getDate(); //日
    dateNow += date.getHours(); //HH
    dateNow += date.getMinutes(); //MM
    dateNow += date.getSeconds(); //SS
    // Tools Events 
    var toolBarManager = $("#toptoolbar").ligerToolBar({ items: [
                { text: ' By Job ID查', click: itemclick, icon: 'search2' },
                { line: true },
                { text: ' Title field investigation process by keyword ', click: itemclick, icon: 'search2' }
//                { line: true },
//                { text: ' Check all fields keywords ', click: itemclick, icon: 'search2' }
            ]
    });
    LoadGrid();
});