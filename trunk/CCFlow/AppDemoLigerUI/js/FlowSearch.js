// Callback 
function callBack(jsonData, scope) {

    var grid = $('#maingrid').treegrid({
        fitColumns: true,
        idField: 'No',
        treeField: 'Name',
        url: '/AppDemoLigerUI/Base/DataService.aspx?method=flowsearch',
        method: 'get',
        columns: [[{ title: ' Name ', field: 'Name', width: 360, align: 'left', formatter: function (value, rec) {
            if (rec.Element == "flow") {
                var reVal = "../WF/Rpt/Search.aspx?RptNo=ND" + Number(rec.No) + "MyRpt";
                reVal = "<a href=javascript:WinOpen('" + reVal + "')  class='s12'>" + rec.Name + "</a>";
                return reVal;
            }
            return rec.Name;
        }
        }, { title: ' Invoice ', field: 'NumOfBill', formatter: function (value, rec) {
            if (rec.Element == "flow") {
                var reVal = "";
                if (rec.NumOfBill == 0) {
                    reVal = "Nothing";
                }
                else {
                    reVal = "../WF/Rpt/Bill.aspx?EnsName=BP.WF.Bills&FK_Flow=" + rec.No + "&T=" + dateNow;
                    reVal = "<a href=\"javascript:WinOpen('" + reVal + "');\"  ><img src='Img/Menu/bill.png' align='middle' width='16' height='16' border=0/> Invoice </a>";
                }
                return reVal;
            }
        }
        }, { title: ' Process Query - Analysis ', field: 'opt', formatter: function (value, rec) {
            if (rec.Element == "flow") {
                var reVal = "../WF/Rpt/Search.aspx?RptNo=ND" + Number(rec.No) + "MyRpt";
                reVal = "<a href=javascript:WinOpen('" + reVal + "')  class='s12'> Inquiry </a>";
                var reVal1 = "../WF/Rpt/Group.aspx?FK_Flow=" + rec.No + "&DoType=Dept";
                reVal1 = " - <a href=javascript:WinOpen('" + reVal1 + "')  class='s12'> Analysis </a>";
                return reVal + reVal1;
            }
        }
        }]],
        onLoadSuccess: function (row, data) {
            $('#maingrid').treegrid('expandAll');
        }
    });
    $("#pageloading").hide();
}
// Open flowchart 
//function OpenFlowPicture(flowNo, flowName) {
//    var pictureUrl = "../WF/Chart.aspx?FK_Flow=" + flowNo + "&DoType=Chart&T=" + dateNow;
//    var win = $.ligerDialog.open({
//        height: 500, width: 800, url: pictureUrl, showMax: true, isResize: true, modal: true, title: flowName + " Flow chart ", slide: false, buttons: [{
//            text: ' Shut down ', onclick: function (item, Dialog, index) {
//                win.hide();
//            }
//        }]
//    });
//}
// Open flowchart 
function OpenEasyUiFlowPicture(flowNo, flowName) {
    var pictureUrl = "/WF/Chart.aspx?FK_Flow=" + flowNo + "&DoType=Chart&T=" + dateNow;
    $("#opengrid").dialog({
        height: 500,
        width: 800,
        href: pictureUrl,
        showMax: true,
        isResize: true,
        modal: true,
        title: flowName + " Flow chart ",
        slide: false,
        buttons: [{ text: ' Shut down ', handler: function () {
            $('#opengrid').dialog('close');
        }
        }]
    });
}
function WinOpen(url) {
    var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
    newWindow.focus();
    return;
}
// Load query list 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.flowSearch(callBack, this);
}
var dateNow = "";

$(function () {

    var date = new Date();
    dateNow += date.getFullYear(); //年
    dateNow += date.getMonth() + 1; //月  May be less than the actual month 1
    dateNow += date.getDate(); //日
    dateNow += date.getHours(); //HH
    dateNow += date.getMinutes(); //MM
    dateNow += date.getSeconds(); //SS


    LoadGrid();
});