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
function getArgsFromHref(sArgName) {
    var sHref = window.location.href;
    var args = sHref.split("?");
    var retval = "";
    if (args[0] == sHref) /* Parameter is empty */
    {
        return retval; /* Do not need any treatment */
    }

    var str = args[1];
    args = str.split("&");
    for (var i = 0; i < args.length; i++) {
        str = args[i];
        var arg = str.split("=");
        if (arg.length <= 1) continue;
        if (arg[0] == sArgName) retval = arg[1];
    }
    while (retval.indexOf('#') >= 0) {
        retval = retval.replace('#', '');
    }
    return retval;
}
var ensName = '';
// Load table data 
function LoadGridData(pageNumber, pageSize) {
    ensName = getArgsFromHref("EnsName");
    // Entity name 
    if (ensName == '') {
        $("body").html("<b style='color:red;'> Please pass the correct parameter name .如:SearchEasyUI.aspx?EnsName=BP.GPM.Depts<b>");
        return;
    }

    var params = {
        method: "getensgriddata",
        EnsName: ensName,
        pageNumber: pageNumber,
        pageSize: pageSize
    };
    queryData(params, function (js, scope) {
        $("#pageloading").hide();
        if (js) {
            if (js == "") js = "[]";

            // System error 
            if (js.status && js.status == 500) {
                $("body").html("<b style='color:red;'> Please pass the correct parameter name .如:SearchEasyUI.aspx?EnsName=BP.GPM.Depts<b>");
                return;
            }

            var pushData = eval('(' + js + ')');
            var fitColumns = true;
            if (pushData.columns.length > 7) {
                fitColumns = false;
            }
            $('#ensGrid').datagrid({
                columns: [pushData.columns],
                data: pushData.data,
                width: 'auto',
                height: 'auto',
                striped: true,
                rownumbers: true,
                singleSelect: true,
                pagination: true,
                remoteSort: false,
                fitColumns: fitColumns,
                pageNumber: pageNumber,
                pageSize: pageSize,
                pageList: [20, 30, 40, 50],
                onDblClickCell: function (index, field, value) {
                    EditEntityForm();
                },
                loadMsg: ' Loading ......'
            });

            var pg = $("#ensGrid").datagrid("getPager");
            if (pg) {
                $(pg).pagination({
                    onRefresh: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    },
                    onSelectPage: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    }
                });
            }

        }
    }, this);
}
// View details 
function CheckEntityForm() {
    EditEntityForm();
}
// See page 
function EditEntityForm() {
    var enName = $("#enName").val();
    var PK = $("#enPK").val();
    if (enName == "") {
        $.messager.alert(' Prompt ', ' Did not find the name of the class !', 'info');
        return;
    }
    var url = "UIEn.aspx?EnName=" + enName;
    var row = $('#ensGrid').datagrid('getSelected');
    if (row) {
        url = "UIEn.aspx?EnName=" + enName + "&PK=" + row[PK];
        var winWidth = document.body.clientWidth;
        // Calculations show that the width 
        winWidth = winWidth * 0.9;
        if (winWidth > 820) winWidth = 820;

        var winheight = document.body.clientHeight;
        // Calculations show that the height 
        winheight = winheight * 0.98
        if (winheight > 780) winheight = 780;

        $("<div id='dialogEnPanel'></div>").append($("<iframe width='100%' height='100%' frameborder=0 src='" + url + "'/>")).dialog({
            title: " Window ",
            width: winWidth,
            height: winheight,
            autoOpen: true,
            modal: true,
            resizable: true,
            onClose: function () {
                $("#dialogEnPanel").remove();
                var pg = $('#ensGrid').datagrid('getPager');
                var curPage = $(pg).pagination.pageNumber;
                LoadGridData(curPage, 20);
            },
            buttons: [{
                text: ' Shut down ',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#dialogEnPanel').dialog("close");
                }
            }]
        });
    } else {
        $.messager.alert(' Prompt ', ' Please select record after viewing !', 'info');
    }
}
function DDL_mvals_OnChange(ctrl, ensName, attrKey) {

    var idx_Old = ctrl.selectedIndex;

    if (ctrl.options[ctrl.selectedIndex].value != 'mvals')
        return;
    if (attrKey == null)
        return;

    var url = 'SelectMVals.aspx?EnsName=' + ensName + '&AttrKey=' + attrKey;
    var val = window.showModalDialog(url, 'dg', 'dialogHeight: 450px; dialogWidth: 450px; center: yes; help: no');
    if (val == '' || val == null) {
        ctrl.selectedIndex = 0;
    }
}
// Public Methods 
function queryData(param, callback, scope, method, showErrMsg) {
    if (!method) method = 'GET';
    $.ajax({
        type: method, // Use GET或POST Method of accessing the background 
        dataType: "text", // Return json Data format 
        contentType: "application/json; charset=utf-8",
        url: "SearchEUI.aspx", // Backstage address to be accessed 
        data: param, // Data to be transmitted 
        async: false,
        cache: false,
        complete: function () { }, //AJAX When the request is complete Hide loading Prompt 
        error: function (XMLHttpRequest, errorThrown) {
            callback(XMLHttpRequest);
        },
        success: function (msg) {//msg For the returned data , Here to do data binding 
            var data = msg;
            callback(data, scope);
        }
    });
}
//3 Seconds after loading 
setTimeout("LoadGridData(1,20)", 500);