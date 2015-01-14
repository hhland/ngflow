<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemLoginLog.aspx.cs"
    Inherits="GMP2.GPM.SystemLoginLogPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link id="appstyle" href="themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="themes/default/datagrid.css" rel="stylesheet" type="text/css" />
    <link href="themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="jquery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="jquery/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="javascript/CC.MessageLib.js" type="text/javascript"></script>
    <script src="javascript/AppData.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        if ($.fn.pagination) {
            $.fn.pagination.defaults.beforePageText = '第';
            $.fn.pagination.defaults.afterPageText = '共{pages}页';
            $.fn.pagination.defaults.displayMsg = ' Show {from}到{to},共{total} Record ';
        }
        if ($.fn.datagrid) {
            $.fn.datagrid.defaults.loadMsg = ' Being processed , Please wait ...';
        }
        if ($.fn.treegrid && $.fn.datagrid) {
            $.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
        }
        if ($.messager) {
            $.messager.defaults.ok = ' Determine ';
            $.messager.defaults.cancel = ' Cancel ';
        }
        // Log Query System 
        function LoadGrid(date1, date2) {
            Application.data.getSystemLoginLogs(function (js, scope) {
                if (js) {
                    if (js == "") js = "[]";
                    var pushData = eval('(' + js + ')');
                    $('#logGrid').datagrid({
                        data: pushData,
                        width: 'auto',
                        striped: true,
                        singleSelect: true,
                        loadMsg: ' Loading ......',
                        columns: [[
                       { field: 'OID', title: ' Serial number ', width: 100 },
                       { field: 'FK_EMP', title: ' Login Account ', width: 100, align: 'left' },
                       { field: 'EMP_Name', title: ' Username ', width: 100, align: 'left' },
                       { field: 'Dept_Name', title: ' Department name ', width: 100, align: 'left' },
                       { field: 'Sys_Name', title: ' System Name ', width: 200, align: 'left' },
                       { field: 'LoginDateTime', title: ' Login Time ', width: 160, align: 'center' },
                       { field: 'IP', title: 'IP Address ', width: 200, align: 'left' },
                       ]]
                    });
                    $("#rowCount").html(" Total inquiry :" + pushData.total + "  Records ");
                }
            }, this, date1, date2);
        }

        $(function () {
            var date = new Date();
            var month = date.getMonth() + 1;
            var dateNow = date.getFullYear() + "-" + month + "-" + date.getDate();
            LoadGrid(dateNow, dateNow);
        });
        // Inquiry 
        function btnSearch_onclick() {
            var startDate = $("#editDate1").datebox('getValue');
            var endDate = $("#editDate2").datebox('getValue');
            if (startDate == "" || endDate == "") {
                CC.Message.showError(" Prompted ", " Start time and end time can not be empty !");
                return;
            }
            LoadGrid(startDate, endDate);
        }
    </script>
</head>
<body>
    <div style="background-color: #b5cbf7; height: 45px;">
        <div id="divEditDate" style="margin-left: 5px; margin-bottom: 5px; vertical-align: middle;">
            <br />
            <div style="float: left;">
                 Start Time :<input type="text" name="mdate" size="20" id="editDate1" class="easyui-datetimebox" data-options="required:true" />
            </div>
            <div style="float: left;">
                 End Time :
                <input type="text" name="mdate" size="20" id="editDate2" class="easyui-datetimebox" data-options="required:true" />
            </div>
            <div style="float: left;">
                <input type="button" id="btnSearch" value="查  询" onclick="btnSearch_onclick();" style="float: left;
                    display: inline;" />
            </div>
            <div style="float: right; font-family:arial, Times New Roman ,sans-serif; font-size:12px; color:Red;">&nbsp;&nbsp;<span id="rowCount"></span></div>
        </div>
    </div>
    <table id="logGrid" class="easyui-datagrid">
    </table>
</body>
</html>
