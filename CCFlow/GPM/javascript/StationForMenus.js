// Initialization 
$(function () {
    // Get job information 
    Application.data.getStations(StationsCallBack, this);
    // Load Menu 
    $("#lbStation").bind('contextmenu', function (e) {
        e.preventDefault();
        $('#mm1').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });
    // Inquiry   Event  
    $('#QueryStation').bind('click', function () {
        QueryStation();
    });
    // Save   Event 
    $('#saveRight').bind('click', function () {
        SaveStationOfMenus();
    });
});
// Load all posts 
function StationsCallBack(js, scope) {
    if (js == "") js = "[]";
    var pushData = eval('(' + js + ')');
    var objSelect = document.getElementById("lbStation");
    if (pushData.length > 0) {
        $.each(pushData, function (index, val) {
            var varItem = new Option("[" + val.No + "]" + val.Name, val.No);
            objSelect.options.add(varItem);
        });
    }
    $("#pageloading").hide();
}

// When the selected job changes 
function LoadMenusOnStationChange() {
    // Load Menu 
    var selectIndex = document.getElementById("lbStation").selectedIndex;
    var options = document.getElementById("lbStation").options;
    if (selectIndex >= 0) {
        $("#pageloading").show();
        Application.data.getStationOfMenusByNo(options[selectIndex].value, 2000, 'false', LoadMenuTreeCallBack, this);
    }
}
// Load Menu 
function LoadMenuTreeCallBack(js, scope) {
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
        onCheck: function (node) {
            if (!treeIsLoading) {
                var pasteSelectIndex = document.getElementById("lbStation").selectedIndex;
                if (pasteSelectIndex >= 0) {
                    $('#saveRight').linkbutton('enable');
                }
            }
        },
        onLoadSuccess: function () {
            treeIsLoading = false;
        },
        onExpand: function (node) {
            if (node) {
                StationMenuChildNodes(node, false);
            }
        },
        onClick: function (node) {
            if (node) {
                StationMenuChildNodes(node, true);
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
// Load the child nodes of the selected menu node 
function StationMenuChildNodes(node, expand) {
    var childNodes = $('#menuTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#menuTree').tree('remove', childNodes[0].target);
        var selectIndex = document.getElementById("lbStation").selectedIndex;
        var options = document.getElementById("lbStation").options;
        if (selectIndex >= 0) {
            Application.data.getStationOfMenusByNo(options[selectIndex].value, node.id, 'true', function (js) {
                if (js && js != '[]') {
                    var pushData = eval('(' + js + ')');
                    $('#menuTree').tree('append', { parent: node.target, data: pushData });
                    if (expand) $('#menuTree').tree('expand', node.target);
                }
            }, this);
        }
    }
}
//  Inquiry   Event 
function QueryStation() {
    var objSearch = $('#searchContent').val();
    // Empty 
    document.getElementById("lbStation").length = 0;
    $("#pageloading").show();
    Application.data.getStationByName(encodeURI(objSearch), function (js, scope) {
        if (js == "") js = "[]";
        var pushData = eval('(' + js + ')');
        var objSelect = document.getElementById("lbStation");
        if (pushData.length > 0) {
            $.each(pushData, function (index, val) {
                var varItem = new Option("[" + val.No + "]" + val.Name, val.No);
                objSelect.options.add(varItem);
            });
        }
        $("#pageloading").hide();
    }, this);
}
// Save   Event 
function SaveStationOfMenus() {
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
            for (var i = 0; i < nodes.length; i++) {
                var nodeText = nodes[i].text;
                if (nodeText == " Loading ...") {
                    continue;
                }
                if (menuIds != '')
                    menuIds += ',';
                menuIds += nodes[i].id;
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
        var selectIndex = document.getElementById("lbStation").selectedIndex;
        var options = document.getElementById("lbStation").options;
        if (selectIndex >= 0) {
            // Save Data 
            Application.data.saveStationOfMenus(options[selectIndex].value, menuIds, menuIdsUn, menuIdsUnExt, function (js, scope) {
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
// Copy rights to other positions 
var copySelectIndex = null;
function CopyRight() {
    copySelectIndex = document.getElementById("lbStation").selectedIndex;
    if (copySelectIndex >= 0) {
        var options = document.getElementById("lbStation").options;
        $("#copyMsg").text(" Copy :" + options[copySelectIndex].text + "  Permissions ");
        Application.data.getStations(StationCallBack, this);
    } else {
        CC.Message.showError(" Prompt ", " Please select copy objects .");
    }
}

// Load positions need to be copied 
function StationCallBack(js, scope) {
    if (js == "") js = "[]";
    var pushData = eval('(' + js + ')');
    var title = $("#copyMsg").text();

    var options = document.getElementById("lbStation").options;
    var copyNo = options[copySelectIndex].value;
    var treeData = "[{ id:'0', text: ' Post ', state: 'open', children:[";
    var children = "";
    if (pushData.length > 0) {
        $.each(pushData, function (index, val) {
            if (val.No != copyNo) {
                if (children.length > 0) children += ",";
                children += "{ id:'" + val.No + "', text:'" + val.Name + "', iconCls: 'icon-seasons'}";
            }
        });
    }
    treeData += children;
    treeData += "]}]";
    pushData = eval('(' + treeData + ')');
    // Load 
    $("#StationTree").tree({
        data: pushData,
        iconCls: 'tree-folder',
        checkbox: true,
        dnd: false,
        onClick: function (node) {
            if (node) {
                if (node.checked == true) {
                    $('#StationTree').tree('uncheck', node.target);
                } else {
                    $('#StationTree').tree('check', node.target);
                }
            }
        }
    });
    // Pop-up window 
    $('#StationDialog').dialog({
        title: title,
        width: 500,
        height: 530,
        closed: false,
        modal: true,
        iconCls: 'icon-rights',
        resizable: true,
        toolbar: [{
            text: ' Empty replication ',
            iconCls: 'icon-save-close',
            handler: function () {
                var nodes = $('#StationTree').tree('getChecked');
                var pastStationNos = "";
                for (var i = 0; i < nodes.length; i++) {
                    if (pastStationNos != "")
                        pastStationNos += ",";
                    pastStationNos += nodes[i].id;
                }
                if (pastStationNos == "") {
                    CC.Message.showError(" Prompted ", " Please select posts !");
                    return;
                }
                // Get copy objects 
                var options = document.getElementById("lbStation").options;
                var copyNo = options[copySelectIndex].value;
                // Save 
                Application.data.clearOfCopyStation(copyNo, pastStationNos, function (js, scope) {
                    if (js == "success") {
                        CC.Message.showError(" Prompted ", " Authorization successful !");
                    } else {
                        CC.Message.showError(" Prompted ", " Authorization failed !" + js);
                    }
                    $('#StationDialog').dialog("close");
                }, this);

            }
        }, '-', {
            text: ' Covering replication ',
            iconCls: 'icon-save-close',
            handler: function () {
                var nodes = $('#StationTree').tree('getChecked');
                var pastStationNos = "";
                for (var i = 0; i < nodes.length; i++) {
                    if (pastStationNos != "")
                        pastStationNos += ",";
                    pastStationNos += nodes[i].id;
                }
                if (pastStationNos == "") {
                    CC.Message.showError(" Prompted ", " Please select posts !");
                    return;
                }
                // Get copy objects 
                var options = document.getElementById("lbStation").options;
                var copyNo = options[copySelectIndex].value;
                // Save 
                Application.data.coverOfCopyStation(copyNo, pastStationNos, function (js, scope) {
                    if (js == "success") {
                        CC.Message.showError(" Prompted ", " Authorization successful !");
                    } else {
                        CC.Message.showError(" Prompted ", " Authorization failed !" + js);
                    }
                    $('#StationDialog').dialog("close");
                }, this);

            }
        }]
    });
}