<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectUser_Jq.aspx.cs"
    Inherits="wwwroot.App_Ctrl.SelectUser_Jq" ClientIDMode="Static" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        /* Cite this page , Use EasyUi的Dialog The size of 520px X 360px,带buttons栏*/
        select.listbox
        {
            border-style: none;
        }
    </style>
    <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../Scripts/QueryString.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getReturnText() {
            var length = $('#lbRight option').length;
            var text = new Array();
            if (length > 0) {
                $('#lbRight option').each(function (i, selected) {
                    text.push($(selected).text());
                });
            }
            return text;
        }
        function getReturnValue() {
            var length = $('#lbRight option').length;
            var value = new Array();
            if (length > 0) {
                $('#lbRight option').each(function (i, selected) {
                    value.push($(selected).val());
                });
            }
            return value;
        }
        $(function () {
            // Initialization department tree 
            $('#ddlDept').combotree({
                url: "SearchUsers.ashx?method=getdepts&s=" + Math.random(),
                onSelect: function (node) {
                    search();
                },
                onLoadSuccess: function (node, data) {
                    // Get the root 
                    var root = $('#ddlDept').combotree("tree").tree('getRoot');
                    if (root) {
                        // Set the selected value 
                        $('#ddlDept').combotree("setValue", root.id);
                        // Execute the query 
                        search();
                    }
                }
            });
        });

        function searchAll() {
            // Get the root 
            var node = $('#ddlDept').combotree("tree").tree('getRoot');
            if (node) {
                // Set the selected value 
                $('#ddlDept').combotree("setValue", node.id);
            }
            $("#cbContainChild").prop("checked", true);
            $("#ddlStation").val("0");
            $("#txtKeyword").val("")
            search();
        }

        function search() {
            // Get user variables 
            var deptTree = $('#ddlDept').combotree('tree'); //  Get the tree object 
            var n = deptTree.tree('getSelected'); //  Get the selected node 
            var deptId = 0;
            if (n) {
                deptId = n.id;
            }
            var searchChild = $("#cbContainChild").prop("checked");
            var stationId = $("#ddlStation").val();
            var txtKeyWords = escape($("#txtKeyword").val());
            // Get to be written list Controls 
            var lb = document.getElementById("lbLeft"); // $("#lbLeft");
            var rb = document.getElementById("lbRight"); // $("#lbLeft");
            // Operating 
            $.ajax({
                type: "POST",
                url: "SearchUsers.ashx?method=getusers&s=" + Math.random(),
                dataType: 'html',
                data: { "DeptId": deptId, "SearchChild": searchChild, "StationId": stationId, "KeyWord": txtKeyWords },
                success: function (data) {
                    if (data != "") {
                        var emps = eval('(' + data + ')');
                        lb.options.length = 0;

                        for (var emp in emps) {
                            var value = emps[emp].No;
                            var text = emps[emp].Name;
                            if (list_exists_item(rb, value)) text = "*" + text;
                            lb.options.add(new Option(text, value));
                        }
                    }
                    else {
                        lb.options.length = 0;
                    }
                }
            });
            return false;
        }
        function list_exists_item(lst_ctrl, str_value) {
            for (var i = 0; i < lst_ctrl.options.length; i++) {
                var option = lst_ctrl.options[i];
                if (option.value == str_value) return true;
            }
            return false;
        }
        function list_find_index(lst_ctrl, str_value) {
            for (var i = 0; i < lst_ctrl.options.length; i++) {
                var option = lst_ctrl.options[i];
                if (option.value == str_value) return i;
            }
            return -1;
        }
        function list_find_option(lst_ctrl, str_value) {
            for (var i = 0; i < lst_ctrl.options.length; i++) {
                var option = lst_ctrl.options[i];
                if (option.value == str_value) return option;
            }
            return null;
        }
        function add_repeat_tag_to_option(option) {
            if (option.text.substr(0, 1) != "*")
                option.text = "*" + option.text;
        }
        function remove_repeat_tag_from_option(option) {
            if (option.text.substr(0, 1) == "*")
                option.text = option.text.substr(1);
        }
        function L2R() {
            // Get two names 
            var lst_from_name = "lbLeft";
            var lst_to_name = "lbRight";
            // Get two list Object 
            var lst_from = document.getElementById(lst_from_name);
            var lst_to = document.getElementById(lst_to_name);
            // Verify selection             
            var from_selIndex = lst_from.selectedIndex;
            if (from_selIndex == -1) {
                alert(" Please choose to add staff !");
                return;
            }
            // First add 
            var numOfRepeat = 0;
            var numOfSelect = 0;
            for (var i = 0; i < lst_from.options.length; i++) {
                var option = lst_from.options[i];
                if (option.selected) {
                    numOfSelect++;
                    if (list_exists_item(lst_to, option.value)) {
                        numOfRepeat++;
                    }
                    else {
                        lst_to.options.add(new Option(option.text, option.value));
                    }
                }
            }
            // Then delete 
            for (var i = lst_from.options.length - 1; i >= 0; i--) {
                var option = lst_from.options[i];
                if (option.selected) {
                    lst_from.options.remove(i);
                }
            }
            // Prompt 
            if (numOfRepeat > 0) {
                alert(" Added successfully " + String(numOfSelect - numOfRepeat) + ", Other " + numOfRepeat + " One has been added !");
            }
        }
        function R2L() {
            // Get two names 
            var lst_from_name = "lbLeft";
            var lst_to_name = "lbRight";
            // Get two list Object 
            var lst_from = document.getElementById(lst_from_name);
            var lst_to = document.getElementById(lst_to_name);
            // Verify selection             
            var to_selIndex = lst_to.selectedIndex;
            if (to_selIndex == -1) {
                alert(" Please choose to cancel employee !");
                return;
            }
            // First add 
            for (var i = 0; i < lst_to.options.length; i++) {
                var option = lst_to.options[i];
                if (option.selected) {
                    if (list_exists_item(lst_from, option.value)) {
                        var option_r = list_find_option(lst_from, option.value);
                        //add_repeat_tag_to_option(option_r);
                        remove_repeat_tag_from_option(option_r);
                    }
                    else {
                        lst_from.options.add(new Option(option.text, option.value));
                    }
                }
            }
            // Then delete 
            for (var i = lst_to.options.length - 1; i >= 0; i--) {
                var option = lst_to.options[i];
                if (option.selected) {
                    lst_to.options.remove(i);
                }
            }
        }
    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'north',noheader:true,border:false" style="overflow-y: hidden;
        height: 30px; padding: 5px">
        <input id="ddlDept" class="easyui-combotree" style="width: 140px;" />
        <input type="checkbox" id="cbContainChild" value="1" /><label for="cbContainChild"> Sub-sector </label>
        <asp:DropDownList ID="ddlStation" runat="server" Width="100px" onchange="search()">
        </asp:DropDownList>
        <asp:TextBox ID="txtKeyword" runat="server" MaxLength="10" Width="60px"></asp:TextBox>
        <input type="button" id="btnSearch" value=" Search for " onclick="search()" />&nbsp;<input
            type="button" id="btnSearchAll" value=" All " onclick="searchAll()" />
    </div>
    <div data-options="region:'center',noheader:true,border:false" style="overflow: hidden;
        padding: 5px">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'west',noheader:true" style="width: 200px">
                <asp:ListBox ID="lbLeft" runat="server" Height="100%" Width="100%" ToolTip="Press Ctrl Select key "
                    CssClass="listbox" SelectionMode="Multiple" ondblclick="L2R()"></asp:ListBox>
            </div>
            <div data-options="region:'center',noheader:true,border:false" style="text-align: center">            
                <br />
                <br />
                <br />
                <br />
                <a href="javascript:void(0)" class="easyui-linkbutton" onclick="L2R()" data-options="iconCls:'icon-right'">
                </a>
                <br />
                <br />
                <a href="javascript:void(0)" class="easyui-linkbutton" onclick="R2L()" data-options="iconCls:'icon-left'">
                </a>
            </div>
            <div data-options="region:'east',noheader:true" style="width: 200px">
                <asp:ListBox ID="lbRight" runat="server" Height="100%" Width="100%" CssClass="listbox"
                    SelectionMode="Multiple" ondblclick="R2L()"></asp:ListBox>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
