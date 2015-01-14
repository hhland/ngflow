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
var parentNo = 0;
// Load node tree 
function LoadTreeNodes() {
    ensName = getArgsFromHref("EnsName");
    parentNo = getArgsFromHref("ParentNo");
    // Entity name 
    if (ensName == '') {
        $("body").html("<b style='color:red;'> Please pass the correct parameter name .如:Tree.aspx?EnsName=BP.GPM.Depts<br/> Idea : If the root node ParentNo Not 0, Need to pass the root ParentNo Value .<b>");
        return;
    }
    // Parent Number 
    if (parentNo == '') {
        parentNo = 0;
    }

    $("#pageloading").show();
    var params = {
        method: "gettreenodes",
        EnsName: ensName,
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
            lines: true
        });
        $("#enTree").bind('contextmenu', function (e) {
            e.preventDefault();
            $('#treeMM').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        });
    }, this);
}

// Tree node operations 
function treeNodeManage(dowhat, nodeNo, callback, scope) {
    var params = {
        method: "treesortmanage",
        EnsName: ensName,
        dowhat: dowhat,
        nodeNo: nodeNo
    };
    queryData(params, callback, scope);
}

// Create the same directory 
function CreateSampleNode() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        treeNodeManage("sample", node.id, function (js) {
            if (js) {
                var parentNode = $('#enTree').tree('getParent', node.target);
                var pushData = eval('(' + js + ')');
                $('#enTree').tree('append', {
                    parent: (parentNode ? parentNode.target : null),
                    data: [{
                        id: pushData.No,
                        text: pushData.Name,
                        iconCls: 'tree_folder'
                    }]
                });
            }

        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node !', 'info');
    }
}
// Create a lower directory 
function CreateSubNode() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        treeNodeManage("children", node.id, function (js) {
            if (js) {
                var pushData = eval('(' + js + ')');
                $('#enTree').tree('append', {
                    parent: (node ? node.target : null),
                    data: [{
                        id: pushData.No,
                        text: pushData.Name,
                        iconCls: 'tree_folder'
                    }]
                });
            }

        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node !', 'info');
    }
}

// Modification 
function EditNode() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        var enName = $("#enName").val();
        if (enName == "") {
            $.messager.alert(' Prompt ', ' Did not find the name of the class !', 'info');
            return;
        }
        var url = "UIEn.aspx?EnName=" + enName + "&PK=" + node.id;
        window.showModalDialog(url, ' Editor ', 'dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px; scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes; help: no');


        var params = {
            method: "gettreenodename",
            EnsName: ensName,
            nodeNo: node.id
        };

        queryData(params, function (js) {
            if (js != "") {
                $('#enTree').tree('update', { target: node.target, text: js });
            }
        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node !', 'info');
    }
}

// Delete Node 
function DeleteNode() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        // Delete 
        treeNodeManage("delete", node.id, function (js) {
            $('#enTree').tree('remove', node.target);
        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node .', 'info');
    }
}

// Move 
function DoUp() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        treeNodeManage("doup", node.id, function (js) {
            LoadTreeNodes();
            $('#enTree').tree('expandAll');
        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node .', 'info');
    }
}
// Down 
function DoDown() {
    var node = $('#enTree').tree('getSelected');
    if (node) {
        treeNodeManage("dodown", node.id, function (js) {
            LoadTreeNodes();
            $('#enTree').tree('expandAll');
        }, this);
    } else {
        $.messager.alert(' Prompt ', ' Please select the node .', 'info');
    }
}

// Public Methods 
function queryData(param, callback, scope, method, showErrMsg) {
    if (!method) method = 'GET';
    $.ajax({
        type: method, // Use GET或POST Method of accessing the background 
        dataType: "text", // Return json Data format 
        contentType: "application/json; charset=utf-8",
        url: "Tree.aspx", // Backstage address to be accessed 
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