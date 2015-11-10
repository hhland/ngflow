// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                   { display: ' Title ', name: 'Title', width: 340, align: 'left', render: function (rowdata, rowindex) {
                       var title = "<a href=javascript:WinOpenIt('../WF/MyFlow.aspx?FK_Flow=" + rowdata.FK_FLOW
                       + "&FK_Node=" + rowdata.FK_NODE + "&FID=" + rowdata.FID +"&T="+ dateNow+ "&WorkID=" + rowdata.WORKID
                       + "&IsRead=0','" + rowdata.MYPK
                       + "');><img align='middle' alt='' src='Img/Menu/Mail_UnRead.png' border=0 id='" + rowdata.MYPK
                       + "'/>" + rowdata.TITLE + "</a>";
                       return title;
                   }
                   },
                   { display: ' Process Name ', name: 'FlowName' },
                   { display: ' The current node ', name: 'NodeName' },
                   { display: ' Sponsor ', name: 'StarterName' },
                   { display: ' Launch date ', name: 'RDT' },
                   { display: ' Accepted ', name: 'SDTOfFlow' },
                   { display: ' The term ', name: 'SDTOfNode' },
                   { display: ' Status ', name: 'WFState', render: function (rowdata, rowindex) {
                       var datePattern = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
                       if (datePattern.test(rowdata.SDTOFNODE)) {
                           var d1 = new Date(dateNow.replace(/-/g, "/"));
                           var d2 = new Date(rowdata.SDTOFFLOW.replace(/-/g, "/"));

                           if (Date.parse(d1) <= Date.parse(d2)) {
                               return " Normal ";
                           }
                           return "<font color=red> Overdue </font>";
                       }
                   } }
                   ],
            pageSize: 20,
            data: pushData,
            rownumbers: true,
            height: "99%",
            width: "99%",
            columnWidth: 100,
            onReload: LoadGrid,
            onDblClickRow: function (rowdata, rowindex) {
                WinOpenIt("../WF/MyFlow.aspx?FK_Flow=" + rowdata.FK_FLOW
                       + "&FK_Node=" + rowdata.FK_NODE + "&FID=" + rowdata.FID + "&WorkID=" + rowdata.WORKID
                       + "&IsRead=0&T="+dateNow, rowdata.MYPK);
            }
        });
    }
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
    $("#pageloading").hide();
}
// Open the form 
function WinOpenIt(url, imgId) {
    var isReadImg = document.getElementById(imgId);
    if (isReadImg) isReadImg.src = "Img/Menu/Mail_Read.png";
    var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
    newWindow.focus();
    return;
}
// Load Pending List 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getHungUpList(callBack, this);
}
var dateNow = "";
$(function () {
    dateNow = "";
    var date = new Date();
    dateNow += date.getFullYear(); //年
    dateNow += date.getMonth() + 1; //月  May be less than the actual month 1
    dateNow += date.getDate(); //日
    dateNow += date.getHours(); //HH
    dateNow += date.getMinutes(); //MM
    dateNow += date.getSeconds(); //SS

    LoadGrid();
});