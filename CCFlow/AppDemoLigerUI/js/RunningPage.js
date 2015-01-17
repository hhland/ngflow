// Send revocation 
function UnSend(fkFlow, workId) {
    $.messager.confirm(' Prompt ', ' Are you sure you want to cancel this transmission ?', function (yes) {
        if (yes) {
            Application.data.unSend(fkFlow, workId, function (js) {
                if (js) {
                    //                    var msg = eval('(' + js + ')');
                    //$.ligerDialog.alert(msg.message);
                    $.messager.alert(' Prompt ', js);
                    LoadGrid();
                }
            });
        }
    });
}
// Reminders 
function Press(url) {
    window.showModalDialog(url, 'sd', 'dialogHeight: 200px; dialogWidth: 400px;center: yes; help: no;');
}
function winOpen(url) {
    // window.showModalDialog(url, '_blank', 'dialogHeight: 500px; dialogWidth: 850px;center: yes; help: no;scroll:no');
    window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');

}

// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").datagrid({
            title: ' Reminders list ',
            height: "auto",
            width: "auto",
            nowrap: true,
            fitColumns: true,
            singleSelect: true,
            autoRowHeight: false,
            striped: true,
            idField: 'Title',
            collapsible: false,
            url: '/AppDemoLigerUI/Base/DataService.aspx?method=Running',
            pagination: true,
            rownumbers: true,
            columns: [[
                   { title: ' Title ', field: 'Title', width: 340, align: 'left', formatter: function (value, rec) {

                       var h = "../WF/WFRpt.aspx?WorkID=" + rec.WORKID + "&FK_Flow=" + rec.FK_FLOW + "&FID=" + rec.FID + "&T=" + dateNow;
                       return "<a href='javascript:void(0);' onclick=winOpen('" + h + "')><img align='middle' border=0 width='20' height='20' src='Img/Menu/Runing.png'/>" + rec.TITLE + "</a>";

                   }
                   },
                   { title: ' The current node ', field: 'NodeName' },
                   { title: ' Sponsor ', field: 'StarterName' },
                   { title: ' Launch date ', field: 'RDT' },
                   { title: ' Operating ', field: 'opt', width: 200,
                       formatter: function (value, rec) {

                           var h2 = "../WF/WorkOpt/Press.aspx?FID=" + rec.FID + '&WorkID=' + rec.WORKID + '&FK_Flow=' + rec.FK_FLOW + "&T=" + dateNow;

                           return "<a href='javascript:void(0);' onclick=UnSend('" + rec.FK_FLOW + "','" + rec.WORKID + "') ><img align='middle' width='20' height='20' src='../WF/Img/Action/UnSend.png' border=0 /> Undo Send </a>&nbsp;&nbsp;&nbsp;<a href='javascript:void(0);' onclick=Press('" + h2 + "')><img width='20' height='20' align='middle' src='../WF/Img/Action/Press.png' border=0 /> Reminders </a>";

                       }
                   }]]
        });
        var p = $('#maingrid').datagrid('getPager');
        $(p).pagination({
            pageSize: 20,
            pageList: [20, 50, 100],
            beforePageText: 'Page ',
            afterPageText: '    Total  {pages} ',
            displayMsg: ' Current display  {from} - {to}  Records    Total  {total}  Records '
        });
    }
    else {
        $.messager.alert(' Prompt ', '<p class="warn"> Failed to load ! </p>');
    }
    $("#pageloading").hide();
}
// In the list of passers-loaded 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getRunning(callBack, this);
   
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