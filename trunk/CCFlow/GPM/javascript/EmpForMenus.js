// Whether the tree is still loaded 
var treeIsLoading = true;
// Load department staff 
function DeptEmpCallBack(js, scope) {

    if (js == "") js = "[]";
    var pushData = eval('(' + js + ')');
    var title = $("#copyMsg").text();
    // Load department staff tree 
    $("#deptEmpTree").tree({
        data: pushData,
        iconCls: 'tree-folder',
        checkbox: true,
        dnd: false,
        onBeforeExpand: function (node) {
            if (node) {
                DeptEmpChildNodes(node);
            }
        },
        onClick: function (node) {
            if (node) {
                DeptEmpChildNodes(node);
            }
        }
    });
    // Pop-up window 
    $('#deptEmpDialog').dialog({
        title: title,
        width: 500,
        height: 530,
        closed: false,
        modal: true,
        iconCls: 'icon-rights',
        resizable: true,
        toolbar: [{
            text: ' Empty replication ',
            iconCls: 'icon-save',
            handler: function () {
                var nodes = $('#deptEmpTree').tree('getChecked');
                var pastUsers = "";
                for (var i = 0; i < nodes.length; i++) {
                    var isLeaf = $('#deptEmpTree').tree('isLeaf', nodes[i].target);
                    if (isLeaf) {
                        if (pastUsers != "")
                            pastUsers += ",";
                        pastUsers += nodes[i].id;
                    }
                }

                if (pastUsers == "") {
                    CC.Message.showError(" Prompted ", " Please select the user !");
                    return;
                }
                // Get copy objects 
                var options = document.getElementById("lbEmp").options;
                var copyNo = options[copySelectIndex].value;
                // Save 
                Application.data.clearOfCopyUserPower(copyNo, pastUsers, function (js, scope) {
                    if (js == "success") {
                        CC.Message.showError(" Prompted ", " Authorization successful !");
                    } else {
                        CC.Message.showError(" Prompted ", " Authorization failed !" + js);
                    }
                    $('#deptEmpDialog').dialog("close");
                }, this);
            }
        }, '-', {
            text: ' Covering replication ',
            iconCls: 'icon-save-close',
            handler: function () {
                var nodes = $('#deptEmpTree').tree('getChecked');
                var pastUsers = "";
                for (var i = 0; i < nodes.length; i++) {
                    var isLeaf = $('#deptEmpTree').tree('isLeaf', nodes[i].target);
                    if (isLeaf) {
                        if (pastUsers != "")
                            pastUsers += ",";
                        pastUsers += nodes[i].id;
                    }
                }

                if (pastUsers == "") {
                    CC.Message.showError(" Prompted ", " Please select the user !");
                    return;
                }
                // Get copy objects 
                var options = document.getElementById("lbEmp").options;
                var copyNo = options[copySelectIndex].value;
                // Save 
                Application.data.coverOfCopyUserPower(copyNo, pastUsers, function (js, scope) {
                    if (js == "success") {
                        CC.Message.showError(" Prompted ", " Authorization successful !");
                    } else {
                        CC.Message.showError(" Prompted ", " Authorization failed !" + js);
                    }
                    $('#deptEmpDialog').dialog("close");
                }, this);
            }
        }]
    });

}

// Load the child nodes of the selected node 
function DeptEmpChildNodes(node) {
    var childNodes = $('#deptEmpTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#deptEmpTree').tree('remove', childNodes[0].target);
        Application.data.getDeptEmpChildNodes(node.id, function (js, scope) {
            if (js) {
                var pushData = eval('(' + js + ')');
                $('#deptEmpTree').tree('append', { parent: node.target, data: pushData });
                $('#deptEmpTree').tree('expand', node.target);
            }
        }, this);
    }
}

// Copy rights 
var copySelectIndex = null;
function CopyRight() {
    copySelectIndex = document.getElementById("lbEmp").selectedIndex;
    if (copySelectIndex >= 0) {
        var options = document.getElementById("lbEmp").options;
        $("#copyMsg").text(" Copy :" + options[copySelectIndex].text + "  Permissions ");
        Application.data.getDeptEmpTree(DeptEmpCallBack, this);

    } else {
        CC.Message.showError(" Prompt ", " Please select copy objects .");
    }
}

// Load Catalog 
function showMenusTree(js, scope) {
    if (js == "") js = [];
    var pushData = eval('(' + js + ')');
    treeIsLoading = true;
    // Loading system directory 
    $("#menuTree").tree({
        data: pushData,
        iconCls: 'tree-folder',
        collapsed: true,
        checkbox: true,
        lines: true,
        onCheck: function () {
            if (!treeIsLoading) {
                var pasteSelectIndex = document.getElementById("lbEmp").selectedIndex;
                if (pasteSelectIndex >= 0) {
                    $('#saveRight').linkbutton('enable');
                }
            }
        },
        onLoadSuccess: function () {
            treeIsLoading = false;
            //            $(this).find('span.tree-checkbox').unbind().click(function () {
            //                return false;
            //            });
        },
        onExpand: function (node) {
            if (node) {
                EmpMenuChildNodes(node, false);
            }
        },
        onClick: function (node) {
            if (node) {
                EmpMenuChildNodes(node, true);
            }
        }
    });
    $("#menuTree").bind('contextmenu', function (e) {
        e.preventDefault();
        $('#treeMM').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });
    $("#pageloading").hide();
    $('#saveRight').linkbutton('disable');
}
// Load the child nodes of the selected node 
function EmpMenuChildNodes(node, expand) {
    var childNodes = $('#menuTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#menuTree').tree('remove', childNodes[0].target);

        var selectIndex = document.getElementById("lbEmp").selectedIndex;
        var options = document.getElementById("lbEmp").options;
        if (selectIndex >= 0) {
            Application.data.getEmpOfMenusByEmpNo(options[selectIndex].value, node.id, "true", function (js) {
                if (js && js != '[]') {
                    var pushData = eval('(' + js + ')');
                    $('#menuTree').tree('append', { parent: node.target, data: pushData });
                    if (expand) $('#menuTree').tree('expand', node.target);
                }
            }, this);
        }
    }
}
// Save User Menu permissions 
function SaveUserOfMenus() {
    var saveRight = $('#saveRight').linkbutton("options");
    if (saveRight && saveRight.disabled == false) {
        var menuIds = "";
        var menuIdsUn = "";
        var menuIdsUnExt = "";
        var nodes = [];
        var nodesUn = [];
        var nodesUnExt = [];
        // Treatment is not fully selected items 
        $("#menuTree").find('.tree-checkbox2').each(function () {
            var node = $(this).parent();
            nodesUn.push($.extend({}, $.data(node[0], 'tree-node'), {
                target: node[0],
                checked: node.find('.tree-checkbox').hasClass('tree-checkbox2')
            }));
        });
        if (nodesUn.length > 0) {
            for (var i = 0; i < nodesUn.length; i++) {
                if (menuIdsUn != '')
                    menuIdsUn += ',';
                menuIdsUn += nodesUn[i].id;
            }
        }
        // Handle fully selected items 
        nodes = $('#menuTree').tree('getChecked');
        if (nodes.length > 0) {
            for (var k = 0; k < nodes.length; k++) {
                var nodeText = nodes[k].text;
                if (nodeText == " Loading ...") {
                    continue;
                }
                if (menuIds != '')
                    menuIds += ',';
                menuIds += nodes[k].id;
            }
        }
        // Treatment is not expanded and includes selected items 
        $("#menuTree").find('.collaboration').each(function () {
            var node = $(this).parent();
            nodesUnExt.push($.extend({}, $.data(node[0], 'tree-node'), {
                target: node[0],
                checked: node.find('.tree-checkbox').hasClass('tree-checkbox0')
            }));
        });
        if (nodesUnExt.length > 0) {
            for (var i = 0; i < nodesUnExt.length; i++) {
                var childNodes = $('#menuTree').tree('getChildren', nodesUnExt[i].target);
                if (nodesUnExt[i].checked == true) {
                    // Undeployed need to traverse a child in the background 
                    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
                        if (menuIdsUnExt != '')
                            menuIdsUnExt += ',';
                        menuIdsUnExt += nodesUnExt[i].id;
                    }
                    // As incompletely option to save 
                    if (menuIdsUn != '')
                        menuIdsUn += ',';
                    menuIdsUn += nodesUnExt[i].id;
                }
            }
        }
        var selectIndex = document.getElementById("lbEmp").selectedIndex;
        var options = document.getElementById("lbEmp").options;
        if (selectIndex >= 0) {
            // Save Data 
            $("#empNo").val(options[selectIndex].value);
            $("#menuIds").val(menuIds);
            $("#menuIdsUn").val(menuIdsUn);

            Application.data.saveUserOfMenus(options[selectIndex].value, menuIds, menuIdsUn, menuIdsUnExt, function (js, scope) {
                if (js == "success") {
                    CC.Message.showError(" Prompted ", " Saved successfully !");
                    $('#saveRight').linkbutton('disable');
                } else {
                    CC.Message.showError(" Prompted ", " Failed to save !" + js);
                }
            }, this);
        }
    }
}
// When people change the selected 
function LoadMenusOnEmpChange() {
    //            var saveRight = $('#saveRight').linkbutton("options");
    //            if (saveRight && saveRight.disabled == false) {
    //                $.messager.confirm(' Prompt ', ' Selected personnel changes menu , Whether to save ?', function (r) {
    //                    if (r) {
    //                        SaveUserOfMenus();
    //                    } else {
    //                        $('#saveRight').linkbutton('disable');
    //                    }
    //                });
    //            }
    // Load Menu 
    var selectIndex = document.getElementById("lbEmp").selectedIndex;
    var options = document.getElementById("lbEmp").options;
    if (selectIndex >= 0) {
        $("#pageloading").show();
        Application.data.getEmpOfMenusByEmpNo(options[selectIndex].value, 2000, "false", showMenusTree, this);
    }
}
// Inquiry 
function QueryEmps() {
    var objSearch = $('#searchConten').val();
    // Empty 
    document.getElementById("lbEmp").length = 0;
    $("#pageloading").show();
    Application.data.getEmpsByNoOrName(encodeURI(objSearch), function (js, scope) {
        if (js == "") js = "[]";
        var pushData = eval('(' + js + ')');
        var objSelect = document.getElementById("lbEmp");
        if (pushData.length > 0) {
            $.each(pushData, function (index, val) {
                var varItem = new Option("[" + val.No + "]" + val.Name, val.No);
                objSelect.options.add(varItem);
            });
        }
        $("#pageloading").hide();
    }, this);
}
// Load personnel information 
function EmpsCallBack(js, scope) {
    if (js == "") js = "[]";
    var pushData = eval('(' + js + ')');
    var objSelect = document.getElementById("lbEmp");
    if (pushData.length > 0) {
        $.each(pushData, function (index, val) {
            var varItem = new Option("[" + val.No + "]" + val.Name, val.No);
            objSelect.options.add(varItem);
        });
    }
    $("#pageloading").hide();
}
// Initialization 
$(function () {
    // Get personnel information 
    Application.data.getEmps(EmpsCallBack, this);
    // Binding events 
    $("#lbEmp").bind('contextmenu', function (e) {
        e.preventDefault();
        $('#mm1').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });
    $('#saveRight').bind('click', function () {
        SaveUserOfMenus();
    });
});