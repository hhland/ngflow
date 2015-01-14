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


// Pop-up layer   
function WinOpenIt(url) {
    $.ligerDialog.open({ height: 400, url: url, width: 450, showMax: true, showToggle: true, showMin: true, isResize: true, modal: false, title: ' Message Details ' });
}
var pushData = "";
// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        if (jsonData.length > 17) {
            pushData = eval('(' + jsonData + ')');
            var grid = $("#maingrid").ligerGrid({
                columns: [
                   { display: ' Theme ', name: 'Title', width: 300, align: 'left', render: function (rowdata, rowindex) {
                       if (rowdata.ISREAD == "1") {
                           return "<img align='middle' id='" + rowdata.MYPK + "' border=0 width='20' height='20' src='Img/Menu/Mail_Read.png'/><a  href='javascript:void(0)' onmouseover=OpenDocDiv('" + escape(rowdata.DOC) + "',event)  onmouseout=CloseDocDiv() >" + rowdata.TITLE + "</a>";
                       }
                       else {
                           return "<img align='middle' id='" + rowdata.MYPK + "' border=0 width='20' height='20' src='Img/Menu/Mail_UnRead.png'/><a  href='javascript:void(0)' onmouseover=OpenDocDiv('" + escape(rowdata.DOC) + "',event)   onmouseout=CloseDocDiv() >" + rowdata.TITLE + "</a>";
                       }
                   }
                   },
                   { display: ' Sender ', name: 'Sender' },
                   { display: ' Send date ', name: 'RDT', width: 150, render: function (rowdata, rowindex) {
                       var week = "";
                       if (rowdata.WEEKRDT != "") {
                           switch (rowdata.WEEKRDT) {
                               case "1":
                                   week = " On Monday ";
                                   break;
                               case "2":
                                   week = " On Tuesday ";
                                   break;
                               case "3":
                                   week = " On Wednesday ";
                                   break;
                               case "4":
                                   week = " Thursday ";
                                   break;
                               case "5":
                                   week = " Fri. ";
                                   break;
                               case "6":
                                   week = " On Saturday ";
                                   break;
                               case "7":
                                   week = " Sun. ";
                                   break;
                           }
                       }
                       var date = new Date(Date.parse(rowdata.RDT.replace(/\-/g, "/")));
                       var month = (date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1);
                       var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                       var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
                       var minu = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
                       return date.getFullYear() + "/" + month + "/" + day + "(" + week + ")" + hour + ":" + minu;
                   }
                   }
                   ],
                pageSize: 20,
                data: pushData,
                rownumbers: true,
                height: "99%",
                width: "99%",
                columnWidth: 120,
                onReload: LoadGrid,
                detail: { onShowDetail: GetDetail },
                groupColumnName: 'GroupBy',
                groupColumnDisplay: ' Date '
            });
        }
    }
    else {
        $.ligerDialog.warn(' No system message !');
    }
    $("#pageloading").hide();
}

// Float    Turn on 
function OpenDocDiv(row, e) {
    var x = e.clientX;
    var y = e.clientY + 10;
    document.getElementById('divDoc').style.top = y + "px";
    document.getElementById('divDoc').style.position = "absolute";
    document.getElementById('divDocContent').innerHTML = unescape(row).replace(/~/g, "'");
    document.getElementById('divDoc').style.display = 'block';
    return false;
}
//  Go away   Shut down 
function CloseDocDiv() {
    document.getElementById('divDoc').style.display = 'none';
    return false;
}

// Show   Details 
function GetDetail(row, detailPanel) {
    if ($('#griddetail1') != null && $('#griddetail1').length > 0) {
        $('#griddetail1').css('display', 'none');
        $('#griddetail1').remove();
    }
    var grid = document.createElement('div');
    $(".l-grid-detailpanel").css('height', 100);
    $(grid).css('margin', 20);
    grid.innerHTML = row.Doc.replace(/~/g, "'");
    $(detailPanel).append(grid);

    var isReadImg = document.getElementById(row.MyPK);
    if (isReadImg) isReadImg.src = "Img/Menu/Mail_Read.png";
    Application.data.upMsgSta(row.MyPK, upMsgSta, this);
}
// Load   Inbox   List 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.popAlert(type, callBack, this);
}
var strTimeKey = "";
var type = "";
$(function () {

    strTimeKey = "";
    var date = new Date();
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS

    var Request = new QueryString();
    type = Request["type"];

    LoadGrid();
});
// Modify the data state   2013.05.23 H
function upMsgSta(my_PK, jsonData, scope) {
    if (jsonData) { }
}
