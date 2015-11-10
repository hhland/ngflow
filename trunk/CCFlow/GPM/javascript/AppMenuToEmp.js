var curModel = "emp";
// Load Menu 
function showMenusTree(js, scope) {
    if (js == "") js = [];
    var pushData = eval('(' + js + ')');

    // Loading system directory 
    $("#menuTree").tree({
        data: pushData,
        iconCls: 'tree-folder',
        collapsed: true,
        lines: true,
        onExpand: function (node) {
            if (node) {
                LoadMenuChildNodes(node, false);
            }
        },
        onClick: function (node) {
            if (node) {
                LoadMenuChildNodes(node, true);
            }
        }
    });

    $("#pageloading").hide();
}
// Load child nodes 
function LoadMenuChildNodes(node, expand) {
    var childNodes = $('#menuTree').tree('getChildren', node.target);
    if (childNodes && childNodes.length > 0 && childNodes[0].text == ' Loading ...') {
        $('#menuTree').tree('remove', childNodes[0].target);
        Application.data.getMenusOfMenuForEmp(node.id, "true", function (js) {
            if (js && js != '[]') {
                var pushData = eval('(' + js + ')');
                $('#menuTree').tree('append', { parent: node.target, data: pushData });
                if (expand) $('#menuTree').tree('expand', node.target);
            }
            GetTemplatePanel(node);
        }, this);
    } else {
        GetTemplatePanel(node);
    }
}

// Get menu data 
function LoadMenuTree() {
    Application.data.getMenusOfMenuForEmp(2000, "false", showMenusTree, this);
}

// Which mode to assign permissions 
function DistributeRight(model) {
    curModel = model;
    if (curModel == "group") {
        $("#curModelText").html(" The currently selected by the group permissions to assign permissions ");
    }
    if (curModel == "station") {
        $("#curModelText").html(" Assign permissions according to the currently selected post ");
    }
    if (curModel == "emp") {
        $("#curModelText").html(" The currently selected by staff to assign permissions ");
    }
    var selected = $('#menuTree').tree('getSelected');
    // If you have a choice on the menu item Refresh 
    if (selected) GetTemplatePanel(selected);
}


// Get permission to menu 
function GetTemplatePanel(node) {
    if (node) {
        Application.data.getTemplateData(node.id, curModel, function (js, scope) {
            if (js) {
                var data = eval('(' + js + ')');
                var panel = document.getElementById('templatePanel');
                panel.innerHTML = ' Loading  ......';
                try { eval(data); } catch (e) { alert('Data Format Error!'); }
                // Default assignment menu templates by staff 
                var template = " <#macro userlist data> "
                             + " <#list data.bmList as bmList> "
                             + " <caption>${bmList.Name}</caption><hr>"
                             + " <#list data.empList as empList> "
                             + " <#if (empList.FK_Dept==bmList.No)>"
                             + "   <#if (empList.isCheck==1)>"
                             + "       <input type='checkbox' checked='true'  name='ckgroup'  id='${empList.No}'> ${empList.Name} "
                             + "   <#else>"
                             + "       <input type='checkbox'   name='ckgroup'  id='${empList.No}'> ${empList.Name} "
                             + "   </#if>"
                             + " </#if>"
                             + " </#list> "
                             + " <br><br> "
                             + " </#list>"
                             + " </#macro>";
                // Allocation template by post 
                if (curModel == "station") {
                    template = " <#macro stationlist data> "
                             + " <#list data.station as stationlist> "
                             + " <#if (stationlist.isCheck==1)>"
                             + "     <input type='checkbox' checked='true' name='ckgroup'  id='${stationlist.No}'> ${stationlist.Name} <hr>"
                             + " <#else>"
                             + "     <input type='checkbox' name='ckgroup' id='${stationlist.No}'> ${stationlist.Name} <hr>"
                             + " </#if>"
                             + " </#list>"
                             + " </#macro>";
                }
                // Assign permissions set by the template 
                if (curModel == "group") {
                    template = " <#macro grouplist data> "
                             + " <#list data.group as groupList> "
                             + " <#if (groupList.isCheck==1)>"
                             + "     <input type='checkbox' checked='true' name='ckgroup'  id='${groupList.No}'> ${groupList.Name} <hr>"
                             + " <#else>"
                             + "     <input type='checkbox' name='ckgroup' id='${groupList.No}'> ${groupList.Name} <hr>"
                             + " </#if>"
                             + " </#list>"
                             + " </#macro>";
                }
                // Get content 
                var source = easyTemplate(template, data);
                panel.innerHTML = source;
            }
        }, this);
    }
}

// Save options 
function SaveMenuForEmp() {
    var saveNos = "";
    var selected = $('#menuTree').tree('getSelected');
    var ckgroup = document.getElementsByName("ckgroup");

    if (selected) {
        // Get selections 
        for (var i = 0; i < ckgroup.length; i++) {
            if (ckgroup[i].checked) {
                saveNos += ckgroup[i].id + ",";
            }
        }

        if (saveNos.length > 0)
            saveNos = saveNos.substring(0, saveNos.length - 1);

        var childNodes = $('#menuTree').tree('getChildren', selected.target);
        if (childNodes && childNodes.length > 0) {
            // Message alerts 
            $.messager.confirm(' Prompt ', " Under the selected menu contains submenus , Whether to authorize sub-menu ?", function (r) {
                if (r) {
                    SaveMenuData(selected.id, saveNos, true);
                }
                else {
                    SaveMenuData(selected.id, saveNos, false);
                }
            });
        }
        else {
            SaveMenuData(selected.id, saveNos, false);
        }

    } else {
        CC.Message.showError(" Prompted ", " Please select the menu !");
    }
}
function SaveMenuData(menuNo, ckNos, saveChildNode) {
    $("#pageloading").show();
    // Save menu Permissions 
    Application.data.saveMenuForEmp(menuNo, ckNos, curModel, saveChildNode, function (js, scope) {
        if (js) {
            CC.Message.showError(" Prompted ", " Saved successfully !");
        }
        $("#pageloading").hide();
    }, this);
}
// Select 
function CheckedAll() {
    var checkedSta = false;
    var ckbAll = document.getElementById("ckbAllText");

    var selected = $('#menuTree').tree('getSelected');
    // If you do not select the menu item to return 
    if (!selected) return;

    if (ckbAll.innerHTML == " Select ") {
        checkedSta = true;
        ckbAll.innerHTML = " Empty ";
    } else {
        ckbAll.innerHTML = " Select ";
    }
    var ckgroup = document.getElementsByName("ckgroup");
    for (var i = 0; i < ckgroup.length; i++) {
        ckgroup[i].checked = checkedSta;
    }
}

// Initialization 
$(function () {
    $("#pageloading").show();
    // Load Menu 
    LoadMenuTree();
});