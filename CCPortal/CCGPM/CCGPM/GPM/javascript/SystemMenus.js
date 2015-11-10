var toolbar = [{ 'text': '打开新窗口', 'iconCls': 'icon-save-close', 'handler': 'OpenNewWindow' }
    , { 'text': '增加同级', 'iconCls': 'icon-new', 'handler': 'addSampleNode' }
    , { 'text': '增加下级', 'iconCls': 'icon-new', 'handler': 'addChildNode' }
    , { 'text': '上移', 'iconCls': 'icon-remove', 'handler': 'tranUp' }
    , { 'text': '下移', 'iconCls': 'icon-remove', 'handler': 'tranDown' }
    , { 'text': '删除', 'iconCls': 'icon-cancel', 'handler': 'delNode' }
    , { 'text': '修改', 'iconCls': 'icon-edit', 'handler': 'editNode'}];

var appName = "";
var curMenuNo = "";
var isModelWindow = true;
function winOpen(menuNo) {
    var strTimeKey = "";
    curMenuNo = menuNo;
    var date = new Date();
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月 月比实际月份要少1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS
    var val = window.showModalDialog("/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Menus&PK=" + menuNo + "&T=" + strTimeKey, "属性", "dialogWidth=800px;dialogHeight=460px;dialogTop=140px;dialogLeft=260px");
    if (isModelWindow) {
        LoadGrid();
    } else {
        LoadGrid2();
    }
    //window.open("/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Menus&PK=" + menuNo + "&T=" + strTimeKey, "属性", "width=800,height=600,top=40,left=160,toolbar=no,menubar=no,scrollbars=no,location=no, status=no");
}
//新建选项卡
function AddTab(title, url) {
    window.parent.addTab(title, url);
}
//打开新窗口
function OpenNewWindow() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        if (row.MenuType == "5") {
            $.messager.alert('提示:', '功能点菜单没有子级！', 'info');
        }
        else {
            var url = "AppMenu.aspx?FK_App=" + appName + "&No=" + row.No;
            var title = row.Name + "菜单";
            AddTab(title, url);
        }

    }
    else {
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//新增同级
function addSampleNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        if (row.No == no) {
            $.messager.alert('提示:', '当前菜单不允许新建同级！', 'info');
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
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//新增子级
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
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//上移
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
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//下移
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
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//删除
function delNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        var msg = "您确定删除所选项？";
        var isLeaf = 0;
        if (row.children) isLeaf = row.children.length;
        if (isLeaf > 0) {
            msg = "子项将一起删除，您确定删除？";
        }
        if (row.Flag.indexOf("Flow") > -1) {
            $.messager.alert('提示:', '该菜单为流程菜单，不能进行删除！', 'info');
            return;
        }
        //消息提醒
        $.messager.confirm('提示', msg, function (r) {
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
        $.messager.alert('提示:', '请先选择项！', 'info');
    }
}
//修改
function editNode() {
    var row = $('#menuGrid').treegrid('getSelected');
    if (row) {
        winOpen(row.No);
    }
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
            loadMsg: '数据加载中......',
            columns: [[
                   { field: 'Name', title: '菜单名', width: 280, formatter: function (value, rowData, rowIndex) {
                       return "<a href='javascript:void(0)' onclick=winOpen('" + rowData.No + "');>" + rowData.Name + "</a>";
                   }
                   },
                   { field: 'No', title: '编号', width: 60 },
                   { field: 'Flag', title: '标识', width: 100 },
                   { field: 'Url', title: 'Url', width: 260, align: 'left' },
                   { field: 'WebPath', title: '图标名称', width: 200 },
                   { field: 'MyFilePath', title: '图标路径', width: 180 }
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
//初始页面
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
            loadMsg: '数据加载中......',
            columns: [[
                   { field: 'Name', title: '菜单名', width: 280, formatter: function (value, rowData, rowIndex) {
                       return "<a href='javascript:void(0)' onclick=winOpen('" + rowData.No + "');>" + rowData.Name + "</a>";
                   }
                   },
                   { field: 'No', title: '编号', width: 100 },
                   { field: 'Flag', title: '标识', width: 100 },
                   { field: 'Url', title: 'Url', width: 260, align: 'left' },
                   { field: 'MyFileName', title: '图标名称', width: 80 },
                   { field: 'MyFilePath', title: '图标路径', width: 80 }
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