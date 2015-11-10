var toolbar = [{ 'text': ' Open a new window ', 'iconCls': 'icon-save-close', 'handler': 'OpenNewWindow' }
    , { 'text': ' Increase at the same level ', 'iconCls': 'icon-new', 'handler': 'addSampleNode' }
    , { 'text': ' Increase subordinate ', 'iconCls': 'icon-new', 'handler': 'addChildNode' }
    , { 'text': ' Move ', 'iconCls': 'icon-remove', 'handler': 'tranUp' }
    , { 'text': ' Down ', 'iconCls': 'icon-remove', 'handler': 'tranDown' }
    , { 'text': ' Delete ', 'iconCls': 'icon-cancel', 'handler': 'delNode' }
    , { 'text': ' Modification ', 'iconCls': 'icon-edit', 'handler': 'editNode', menus: [{ text: 'Add', iconCls: 'icon-add', handler: function () { alert('add') } }, { text: 'Cut', iconCls: 'icon-cut', disabled: true, handler: function () { alert('cut') } }, '-', { text: 'Save', iconCls: 'icon-save', handler: function () { alert('save') } }]}];

var appName = "";
var curMenuNo = "";
var isModelWindow = true;
function winOpen(menuNo) {
    var strTimeKey = "";
    curMenuNo = menuNo;
    var date = new Date();
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS
    var val = window.showModalDialog("../WF/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Menus&PK=" + menuNo + "&T=" + strTimeKey, " Property ", "dialogWidth=800px;dialogHeight=460px;dialogTop=140px;dialogLeft=260px");
    if (isModelWindow) {
        LoadGrid();
    } else {
        LoadGrid2();
    }
}
// New tab 
function AddTab(title, url) {
    window.parent.addTab(title, url);
}
// Open a new window 
function OpenNewWindow() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        if (row.MenuType == "5") {
            $.messager.alert(' Prompt :', ' Function Point menu no children !', 'info');
        }
        else {
            var url = "AppMenu.aspx?FK_App=" + appName + "&No=" + row.No;
            var title = row.Name + " Menu ";
            AddTab(title, url);
        }

    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// New sibling 
function addSampleNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        if (row.No == no) {
            $.messager.alert(' Prompt :', ' The current menu does not allow new sibling !', 'info');
        }
        else {
            Application.data.nodeManage(row.No, "sample", function (js, scop) {
                curMenuNo = row.No;
                if (isModelWindow) {
                    LoadGrid();
                } else {
                    LoadGrid2();
                }
                //$('#test').treegrid('expandTo', '009');
                //$('#menuGrid').treegrid('select', '009');
            }, this);
        }

    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// New child 
function addChildNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        Application.data.nodeManage(row.No, "children", function (js, scop) {
            curMenuNo = row.No;
            if (isModelWindow) {
                LoadGrid();
            } else {
                LoadGrid2();
            }
            //$('#test').treegrid('expandTo', '009');
            //$('#menuGrid').treegrid('select', '009');
        }, this);
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// Move 
function tranUp() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        Application.data.nodeManage(row.No, "doup", function (js, scop) {
            curMenuNo = row.No;
            if (isModelWindow) {
                LoadGrid();
            } else {
                LoadGrid2();
            }
            //$('#test').treegrid('expandTo', '009');
            //$('#menuGrid').treegrid('select', '009');
        }, this);
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// Down 
function tranDown() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        Application.data.nodeManage(row.No, "dodown", function (js, scop) {
            curMenuNo = row.No;
            if (isModelWindow) {
                LoadGrid();
            } else {
                LoadGrid2();
            }
            //$('#test').treegrid('expandTo', '009');
            //$('#menuGrid').treegrid('select', '009');
        }, this);
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// Delete 
function delNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        var msg = " Are you sure to delete the selected item ?";
        var isLeaf = 0;
        if (row.children) isLeaf = row.children.length;
        if (isLeaf > 0) {
            msg = " Subkey will be deleted together , Are you sure you delete ?";
        }
        if (row.Flag.indexOf("Flow") > -1) {
            $.messager.alert(' Prompt :', ' The menu for the process menu , Can not be deleted !', 'info');
            return;
        }
        // Message alerts 
        $.messager.confirm(' Prompt ', msg, function (r) {
            if (r) {
                curMenuNo = null;
                Application.data.nodeManage(row.No, "delete", function (js, scop) {
                    if (isModelWindow) {
                        LoadGrid();
                    } else {
                        LoadGrid2();
                    }
                    //$('#test').treegrid('expandTo', '009');
                    //$('#menuGrid').treegrid('select', '009');
                }, this);
            }
        });
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// Modification 
function editNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        winOpen(row.No);
    }
}
// New Process mode menu 
function NewFlowModel() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        Application.data.getFlowTree("0", function (js, scorp) {
            curMenuNo = row.No;
            LoadMainTree("flow", js, " Process tree ");
        }, this);
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}
// New Forms Mode menu 
function NewFormModel() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        Application.data.getFormTree("0", function (js, scorp) {
            curMenuNo = row.No;
            LoadMainTree("form", js, " Form Library ");
        }, this);
    }
    else {
        $.messager.alert(' Prompt :', ' Please select item !', 'info');
    }
}

// Load the child nodes of the selected node 
function FlowSortChildNodes(node) {
    var childNodes = $('#mainTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#mainTree').tree('remove', childNodes[0].target);
        Application.data.getFlowTree(node.id, function (js, scope) {
            if (js) {
                var pushData = eval('(' + js + ')');
                $('#mainTree').tree('append', { parent: node.target, data: pushData });
                $('#mainTree').tree('expand', node.target);
            }
        }, this);
    }
}
// Load the selected child nodes form library 
function FormSortChildNodes(node) {
    var childNodes = $('#mainTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#mainTree').tree('remove', childNodes[0].target);
        Application.data.getFormTree(node.id, function (js, scope) {
            if (js) {
                var pushData = eval('(' + js + ')');
                $('#mainTree').tree('append', { parent: node.target, data: pushData });
                $('#mainTree').tree('expand', node.target);
            }
        }, this);
    }
}

// Loading process , Form library tree 
function LoadMainTree(model, js, title) {
    if (js == "") js = "[]";
    var pushData = eval('(' + js + ')');

    // Loading process categories and processes 
    $("#mainTree").tree({
        data: pushData,
        iconCls: 'tree-folder',
        checkbox: true,
        dnd: false,
        onBeforeExpand: function (node) {
            if (node) {
                if (model == "flow") {
                    FlowSortChildNodes(node);
                }
                if (model == "form") {
                    FormSortChildNodes(node);
                }
            }
        },
        onClick: function (node) {
            if (node) {
                if (model == "flow") {
                    FlowSortChildNodes(node);
                }
                if (model == "form") {
                    FormSortChildNodes(node);
                }
            }
        }
    });
    // Pop-up window 
    $('#modelDialog').dialog({
        title: title,
        width: 500,
        height: 530,
        closed: false,
        modal: true,
        iconCls: 'icon-rights',
        resizable: true,
        toolbar: [{
            text: ' Save and Close ',
            iconCls: 'icon-save-close',
            handler: function () {
                var nodes = $('#mainTree').tree('getChecked');
                var pastNos = "";
                var pastSortNos = "";
                for (var i = 0; i < nodes.length; i++) {
                    // Subkey 
                    if (nodes[i].iconCls == "icon-4") {
                        if (pastNos != "")
                            pastNos += ",";
                        pastNos += nodes[i].id;
                    }
                    // Category 
                    if (nodes[i].iconCls == "icon-tree_folder") {
                        if (pastSortNos != "")
                            pastSortNos += ",";
                        pastSortNos += nodes[i].id;
                    }
                }
                if (pastNos == "" && pastSortNos == "") {
                    $.messager.alert(' Prompt :', ' Please select the content and then save !', 'info');
                    return;
                }
                // Save Process \ Forms Library menu 
                Application.data.saveFlowFormMenu(model, curMenuNo, pastSortNos, pastNos, function (js, scope) {
                    if (isModelWindow) {
                        LoadGrid();
                    } else {
                        LoadGrid2();
                    }
                    $('#modelDialog').dialog("close");
                }, this);

            }
        }, '-', {
            text: ' Cancel ',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#modelDialog').dialog("close");
            }
        }]
    });
}

function LoadGrid() {
    Application.data.getAppMenus(appName, function (js, scope) {
        if (js == "") js = [];
        var pushData = eval('(' + js + ')');
        $('#menuGrid').treegrid({
            data: pushData,
            idField: 'No',
            treeField: 'Name',
            toolbar: toolbar,
            striped: true,
            loadMsg: ' Loading ......',
            columns: [[
                   { field: 'Name', title: ' Menu name ', width: 280, formatter: function (value, rowData, rowIndex) {
                       return "<a href='javascript:void(0)' onclick=winOpen('" + rowData.No + "');>" + rowData.Name + "</a>";
                   }
                   },
                   { field: 'No', title: ' Serial number ', width: 60 },
                   { field: 'Flag', title: ' Mark ', width: 100 },
                   { field: 'Url', title: 'Url', width: 260, align: 'left' },
                   { field: 'WebPath', title: ' Icon Name ', width: 200 },
                   { field: 'MyFilePath', title: ' Icon Path ', width: 180 }
                   ]],
            onContextMenu: function (e, node) {
                e.preventDefault();
                // select the node
                $('#tt').tree('select', node.target);
                // display context menu
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onDblClickCell: function (index, field, value) {
                winOpen(field.No);
            }
        });
        if (curMenuNo != "" && curMenuNo != null) {
            var nodeData = $('#menuGrid').treegrid('find', curMenuNo);
            if (nodeData) $('#menuGrid').treegrid('select', curMenuNo);
        }
    }, this);
}
var no = "";
// The initial page 
$(function () {
    appName = Application.common.getArgsFromHref("FK_App");
    no = Application.common.getArgsFromHref("No");
    if (no == "") {
        LoadGrid();
    }
    else {
        isModelWindow = false;
        LoadGrid2();
    }
});
function LoadGrid2() {
    Application.data.getAppChildMenus(appName, no, function (js, scope) {
        if (js == "") js = [];
        var pushData = eval('(' + js + ')');
        $('#menuGrid').treegrid({
            data: pushData,
            idField: 'No',
            treeField: 'Name',
            toolbar: toolbar,
            striped: true,
            loadMsg: ' Loading ......',
            columns: [[
                   { field: 'Name', title: ' Menu name ', width: 280, formatter: function (value, rowData, rowIndex) {
                       return "<a href='javascript:void(0)' onclick=winOpen('" + rowData.No + "');>" + rowData.Name + "</a>";
                   }
                   },
                   { field: 'No', title: ' Serial number ', width: 100 },
                   { field: 'Flag', title: ' Mark ', width: 100 },
                   { field: 'Url', title: 'Url', width: 260, align: 'left' },
                   { field: 'MyFileName', title: ' Icon Name ', width: 80 },
                   { field: 'MyFilePath', title: ' Icon Path ', width: 80 }
                   ]],
            onContextMenu: function (e, node) {
                e.preventDefault();
                // select the node
                $('#tt').tree('select', node.target);
                // display context menu
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onDblClickCell: function (index, field, value) {
                winOpen(field.No);
            }
        });
        if (curMenuNo != "" && curMenuNo != null) {
            var nodeData = $('#menuGrid').treegrid('find', curMenuNo);
            if (nodeData) $('#menuGrid').treegrid('select', curMenuNo);
        }
    }, this);
}