// Obtain from a url Passed to the parameter list 
function QueryString() {
    var name, value, i;

    var str = location.href;
    var num = str.indexOf("?")

    str = str.substr(num + 1);
    var arrtmp = str.split("&");

    for (i = 0; i < arrtmp.length; i++) {
        num = arrtmp[i].indexOf("=");
        if (num > 0) {
            name = arrtmp[i].substring(0, num);
            value = arrtmp[i].substr(num + 1);
            this[name] = value;
        }
    }
}

var fk_flow; // Process ID 
var name; // Process Name 
var dateNow; // Date 
$(function () {
    dateNow = "";
    var date = new Date();
    dateNow += date.getFullYear(); //年
    dateNow += date.getMonth() + 1; //月  May be less than the actual month 1
    dateNow += date.getDate(); //日
    dateNow += date.getHours(); //HH
    dateNow += date.getMinutes(); //MM
    dateNow += date.getSeconds(); //SS

    var Request = new QueryString();
    fk_flow = Request["FK_Flow"];
    name = Request["Name"];
    strAuth = Request["Auth"];
        // Tools Events 
        var toolBarManager = $("#toptoolbar").ligerToolBar({ items: [
                    { text: ' Initiate the process ', click: itemclick, icon: 'plus' }
                ]
        });
    LoadGrid();
});

function itemclick() {
    var h;

    h = "../WF/MyFlow.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_flow + "01&T=" + dateNow;
    WinOpenIt(fk_flow, ' Launch ' + name + ' Process ', h);
}
// Load List 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getStoryHistory(fk_flow, callBack, this);
}

// Callback 
function callBack(jsonData, scope) {
    if (jsonData.length) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                   { display: ' Title ', name: 'Title', width: 380, align: 'left', render: function (rowdata, rowindex) {
                       //                       var title = "<a href=javascript:WinOpenIt('" + rowdata.MYPK + "','" + rowdata.FK_FLOW + "','" + rowdata.FK_NODE
                       //                       + "','" + rowdata.WORKID + "','" + rowdata.FID + "','" + rowdata.STA + "');><img align=middle src='Img/Menu/CCSta/" + rowdata.STA
                       //                       + ".png' id=" + rowdata.MYPK + ">" + rowdata.TITLE + "</a> Date :" + rowdata.RDT;
                       //                       return Title;   

                       var h = "../WF/WFRpt.aspx?WorkID=" + rowdata.OID + "&FK_Flow=" + fk_flow + "&FID=" + rowdata.FID + "&T=" + dateNow;
                       return "<a href='javascript:void(0);' onclick=WinOpenWindow('" + h + "')>" + rowdata.TITLE + "</a>";
                                   
                       return rowdata.TITLE;
                   }
                   },
                   { display: ' Launch date ', name: 'FlowStartRDT' },
                   { display: ' End node ', name: 'FlowEndNode' },
                   { display: ' Status ', name: 'WFState', render: function (rowdata, rowindex) {
                       switch (rowdata.WFSTATE) {
                           case 0:
                                return " Blank ";
                                //return "<img src='../WF/Img/WFState/Cancel.png'   class='imgWFstate' />" + "    Blank ";
                               break;
                           case 1:
                              // return " Draft ";
                               return "<img src='../WF/Img/WFState/Draft.png'   class='imgWFstate' />" + "    Draft ";
                               break;
                           case 2:
                               //return " Run "; 
                               return "<img src='../WF/Img/WFState/Runing.png'   class='imgWFstate' />" + "    Run ";
                               break;
                           case 3:
                               return " Completed ";
                               //return "<img src='../WF/Img/WFState/RebackOverFlow.png'   class='imgWFstate' />" + "     Completed ";
                               break;
                           case 4:
                              // return " Pending ";
                               return "<img src='../WF/Img/WFState/HungUp.png'   class='imgWFstate' />" + "    Pending ";
                               break;
                           case 5:
                               //return " Return "; 
                               return "<img src='../WF/Img/WFState/RebackOverFlow.png'  class='imgWFstate' />" + "    Return ";
                               break;
                           case 6:
                               return " Forwarding ";
                               break;
                           case 7:
                               //return " Delete ";
                               return "<img src='../WF/Img/WFState/Cancel.png'   class='imgWFstate' />" + "    Delete ";
                               break;
                           default:
                               return "";
                               break;
                       }
                   }
                   }
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
// Open the form 
function WinOpenIt(tabid, text, url) {
    if (ccflow.config.IsWinOpenStartWork == 1) {
        window.parent.f_addTab(tabid + dateNow, text, url);
    } else {
        var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
        newWindow.focus();
    }
}
function WinOpenWindow(url) {
    var newWindow = window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
    newWindow.focus();
}