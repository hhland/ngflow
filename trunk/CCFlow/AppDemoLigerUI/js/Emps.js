
// Phone 
function WinOpen(url) {
    window.showModalDialog(url, '_blank', 'height=600,width=950,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');

}

// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        var pushData = eval('(' + jsonData + ')');
        var grid = $("#maingrid").ligerGrid({
            columns: [
                      { display: ' Department ', name: 'DeptName', width: 140, align: 'left' },
                      { display: ' Serial number ', name: 'No', width: 120 },
                      { display: ' Full name ', name: 'Name', width: 100 },
                      { display: ' Office ', name: 'DutyName', width: 100 },
                      { display: ' Directly under the leadership ', name: 'Leader', width: 100 },
                      { display: 'Tel', name: 'Tel', width: 150, align: 'center', render: function (rowdata, rowindex, value) {
                          if (rowdata.TEL == "") {
                              return "";
                          }
                          else {
                              var h = "../../WF/Msg/SMS.aspx?Tel=" + rowdata.TEL + "&T=" + dateNow;
                              return "<a href='javascript:void(0)' onclick=WinOpen('" + h + "') ><img src='Img/Menu/mail.png' border=0  align=middle />" + rowdata.TEL + "</a>";
                          }

                      }
                      },
                      { display: 'Email', name: 'Email', width: 160, align: 'center', render: function (rowdata, rowindex, value) {
                          if (rowdata.EMAIL == "") {
                              return "";
                          }
                          else {
                              var h = "";
                              return "<a href='Mailto:" + rowdata.EMAIL + "'><img src='Img/Menu/tel.png' border=0 align=middle/>" + rowdata.EMAIL + "</a>";

                          }
                      }
                      },
                        { display: ' Signature ', name: 'QianMing', render: function (rowdata, rowindex, value) {
                            if (rowdata.QIANMING == "") {
                                return "";
                            }
                            else {
                                return "<img src='../.." + rowdata.QIANMING + "' border=0/>";
                            }

                        }
                        }
                   ],
            usePager: false,
            data: pushData,
            rownumbers: true,
            alternatingRow: false,
            tree: { columnName: 'DeptName' },
            columnWidth: 120,
            onReload: LoadGrid
        });
    }
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
    $("#pageloading").hide();
}
// Load   Contacts    List 
function LoadGrid() {
    $("#pageloading").show();
    Application.data.getEmps(callBack, this);
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