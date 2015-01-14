function WinOpenIt(url,target) {
    window.showModalDialog(url,target , 'height=800px,width=950px,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
   
}
// Open flowchart 
function OpenFlowPicture(flowNo, flowName) {
    var pictureUrl = "../WF/Chart.aspx?FK_Flow=" + flowNo + "&DoType=Chart&T=" + strTimeKey;
    var win = $.ligerDialog.open({
        height: 500, width: 800, url: pictureUrl, showMax: true, isResize: true, modal: true, title: flowName + " Flow chart ", slide: false, buttons: [{
            text: ' Shut down ', onclick: function (item, Dialog, index) {
                win.hide();
            }
        }]
    });
}
// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                   { display: ' Process Name ', name: 'Name', width: 420, align: 'left', render: function (rowdata, rowindex) {
                       var h = "../WF/GetTask.aspx?FK_Flow=" + rowdata.No + "&FK_Node=" + parseInt(rowdata.No) + "01&T=" + strTimeKey;
                       return "<a href='javascript:void(0);' onclick=WinOpenIt('" + h + "','_blank') >" + rowdata.Name + "</a>";
                   }

                   },
                     { display: ' Flow chart ', render: function (rowdata, rowindex) {
                         var h = "../WF/Chart.aspx?FK_Flow=" + rowdata.No + "&DoType=Chart&T=" + strTimeKey;
                         return "<a href='javascript:void(0);' onclick=OpenFlowPicture('" + rowdata.No + "','" + rowdata.Name + "')> Turn on </a>";
                        }
                      },
                     { display: ' Description ', name: 'Note' }
                   ],
            pageSize: 20,
            data: pushData,
            rownumbers: true,
            height: "99%",
            width: "99%",
            columnWidth: 120,
            onReload: LoadGrid,
            groupColumnName: 'FK_FlowSortText',
            groupColumnDisplay: ' Process Category ',
            onDblClickRow: function (rowdata, rowindex) {
                WinOpenIt("../WF/MyFlow.aspx?FK_Flow=" + rowdata.FK_Flow + "&FK_Node=" + rowdata.FK_Node
                           + "&FID=" + rowdata.FID + "&IsRead=0&WorkID=" + rowdata.WorkID + "&T=" + strTimeKey, rowdata.WorkID, rowdata.FlowName);
            }
        });
    }
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
    $("#pageloading").hide();
}
// Load   Retrieve approval   List 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getTask(callBack, this);
}
var strTimeKey = "";
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