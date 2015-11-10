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

var treeEnsName = '';
var ensName = '';
var parentNo = 0;
// Load node tree 
function LoadTreeNodes() {
    treeEnsName = getArgsFromHref("TreeEnsName");
    parentNo = getArgsFromHref("ParentNo");
    // Entity name 
    if (treeEnsName == '') {
        $("body").html("<b style='color:red;'> Please pass the correct parameter name .Like:TreeEns.aspx?TreeEnsName=BP.GPM.Depts&EnsName=BP.Port.Emps&RefPK=FK_Dept<br/> Idea : If the root node ParentNo Not 0, Need to pass the root ParentNo Value .<b>");
        return;
    }
    // Parent Number 
    if (parentNo == '') {
        parentNo = 0;
    }

    $("#pageloading").show();
    var params = {
        method: "gettreenodes",
        TreeEnsName: treeEnsName,
        ParentNo: parentNo,
        isLoadChild: true
    };
    queryData(params, function (js, scope) {
        $("#pageloading").hide();
        // System error 
        if (js.readyState && js.readyState == 4 && js.readyState == 0) js = "[]";
        // System error 
        if (js.status && js.status == 500) {
            $("body").html(js.responseText);
            return;
        }
        // Capture error 
        if (js.indexOf("error:") > -1) {
            $("body").html("<br/><b style='color:red;'>" + js + "<b>");
            return;
        }
        var pushData = eval('(' + js + ')');
        // Load category tree 
        $("#enTree").tree({
            data: pushData,
            iconCls: 'tree-folder',
            collapsed: true,
            lines: true,
            onClick: function (node) {
                if (node) {
                    LoadGridData();
                }
            }
        });
    }, this);
}

// Data is loaded to the right 
function LoadGridData() {
    ensName = getArgsFromHref("EnsName");
    var RefPK = getArgsFromHref("RefPK");
    var node = $('#enTree').tree('getSelected');
    if (node) {
        var params = {
            method: "getensgriddata",
            EnsName: ensName,
            RefPK: RefPK,
            FK: node.id
        };
        queryData(params, function (js, scope) {
            if (js) {
                if (js == "") js = "[]";

                // System error 
                if (js.status && js.status == 500) {
                    $(".datagrid-view").html("<b style='color:red;'> Please pass the correct parameter name .Like:TreeEns.aspx?TreeEnsName=BP.GPM.Depts&EnsName=BP.Port.Emps&RefPK=FK_Dept<br/> Idea : If the root node ParentNo Not 0, Need to pass the root ParentNo Value .<b>");
                    return;
                }
                var pushData = eval('(' + js + ')');
                var fitColumns = true;
                if (pushData.columns.length > 6) {
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
                    pageSize: 10,
                    pageList: [10, 15, 20, 50],
                    onDblClickCell: function (index, field, value) {
                        EditEntityForm();
                    },
                    toolbar: [{ 'text': ' New ', 'iconCls': 'icon-new', 'handler': 'CreateEntityForm' }, { 'text': ' Modification ', 'iconCls': 'icon-config', 'handler': 'EditEntityForm'}],
                    loadMsg: ' Loading ......'
                });
            }
        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node !', 'info');
    }
}
// New Page 
function CreateEntityForm() {
    var enName = $("#enName").val();
    var RefPK = getArgsFromHref("RefPK");
    var node = $('#enTree').tree('getSelected');
    if (node) {
        var PK = $("#enPK").val();
        if (enName == "") {
            $.messager.alert(' Prompt ', ' Did not find the name of the class !', 'info');
            return;
        }
        if (RefPK == "") {
            $.messager.alert(' Prompt ', ' Foreign key value is not found !', 'info');
            return;
        }
        var url = "UIEn.aspx?EnName=" + enName + "&" + RefPK + "=" + node.id;
        window.showModalDialog(url, '', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes; help: no');
        LoadGridData();
    }
}
// Edit page 
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
    }
    window.showModalDialog(url, '', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes; help: no');
    LoadGridData();
}
// Public Methods 
function queryData(param, callback, scope, method, showErrMsg) {
    if (!method) method = 'GET';
    $.ajax({
        type: method, // Use GET或POST Method of accessing the background 
        dataType: "text", // Return json Data format 
        contentType: "application/json; charset=utf-8",
        url: "TreeEns.aspx", // Backstage address to be accessed 
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
setTimeout("LoadTreeNodes()", 3000);