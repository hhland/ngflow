// To-do list is loaded 
function LoadGrid() {
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

    var grid = $("#maingrid").datagrid({
        title: ' To-do list ',
        height: "auto",
        width: "auto",
        nowrap: true,
        fitColumns: true,
        autoRowHeight: false,
        singleSelect: true,
        striped: true,
        collapsible: false,
        url: 'Base/DataService.aspx?method=getempworks',
        pagination: false,
        rownumbers: true,
        columns: [[
                   { title: ' Title ', field: 'Title', width: 360, align: 'left', formatter: function (value, rec) {
                       var title = "";

                       if (rec.ISREAD == 0) {
                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlow.aspx?FK_Flow=" + rec.FK_FLOW + "&FK_Node=" + rec.FK_NODE
                           + "&FID=" + rec.FID + "&WorkID=" + rec.WORKID + "&AtPara=" + rec.ATPARA + "&IsRead=0&T=" + strTimeKey
                           + "','" + rec.WORKID + "','" + rec.FLOWNAME + "');\" ><img align='middle' alt='' id='" + rec.WORKID
                           + "' src='Img/Menu/Mail_UnRead.png' border=0 width=20 height=20 />" + rec.TITLE + "</a>";
                       } else {
                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlow.aspx?FK_Flow=" + rec.FK_FLOW + "&FK_Node=" + rec.FK_NODE
                           + "&FID=" + rec.FID + "&T=" + strTimeKey + "&WorkID=" + rec.WORKID + "&AtPara=" + rec.ATPARA + "','" + rec.WORKID
                           + "','" + rec.FLOWNAME + "');\"  ><img align='middle' border=0 width=20 height=20 id='" + rec.WORKID + "' alt='' src='Img/Menu/Mail_Read.png'/>" + rec.TITLE + "</a>";
                       }
                       return title;
                   }
                   },
                   { title: ' Process Name ', field: 'FLOWNAME' },
                   { title: ' The current node ', field: 'NODENAME' },
                   { title: ' Sponsor ', field: 'STARTERNAME' },
                   { title: ' Launch date ', field: 'RDT' },
                   { title: ' Accepted ', field: 'ADT' },
                   { title: ' The term ', field: 'SDT' },
                   { title: ' Status ', field: 'FlowState', formatter: function (value, rec) {
                       //var datePattern = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
                       if (rec.SDTOFNODE != "") {
                           var d1 = new Date();
                           var d2 = new Date(rec.SDTOFNODE.replace(/-/g, "/"));
                           // if ((Date.parse(d1) > Date.parse(d2))) {

                           if (d1 > d2) {
                               return "<font color=red> Overdue </font>";
                           }
                           return " Normal ";
                       }
                   }
                   }
                   ]]
    });
//    var p = $('#maingrid').datagrid('getPager');
//    $(p).pagination({
//        pageSize: 20,
//        pageList: [20, 50, 100],
//        beforePageText: '第',
//        afterPageText: '页    共 {pages} 页',
//        displayMsg: ' Current display  {from} - {to}  Records    共 {total}  Records '
//    });
    $("#pageloading").hide();
}
// Open the form 
function WinOpenIt(url, workId, text) {
    var isReadImg = document.getElementById(workId);
    if (isReadImg) isReadImg.src = "Img/Menu/Mail_Read.png";
    if (ccflow.config.IsWinOpenEmpWorks.toUpperCase() == "TRUE") {
        var winWidth = 850;
        var winHeight = 680;
        if (screen && screen.availWidth) {
            winWidth = screen.availWidth;
            winHeight = screen.availHeight - 36;
        }
        //var newWindow = window.open(url, "_blank", "height=" + winHeight + ",width=" + winWidth + ",top=0,left=0,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no");
        //newWindow.focus();
        try {
            var vReturnValue = window.showModalDialog(url, "_blank", "scrollbars=yes;resizable=yes;center=yes;dialogWidth=" + winWidth + ";dialogHeight=" + winHeight + ";dialogTop=0px;dialogLeft=0px;");

        } catch (ex) {
            window.open(url, "_blank", "height=" + winHeight + ",width=" + winWidth + ",top=0,left=0,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no");
        }
        LoadGrid();

    } else {
        window.parent.f_addTab(workId, text, url);
    }
}

$(function () {
    $("#pageloading").show();
    LoadGrid();
});