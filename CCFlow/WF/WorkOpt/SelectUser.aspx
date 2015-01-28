<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectUser.aspx.cs" Inherits="CCFlow.WF.WorkOpt.SelectUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Select staff </title>
    <style type="text/css">
        body
        {
            font-size: 12px;
        }
        table.t2
        {
            display: table;
            border-collapse: collapse;
            border: 1px solid #cad9ea;
            color: #666;
            width: 100%;
        }
        table.t2 td
        {
            border: 1px solid #cad9ea;
        }
        select.listbox
        {
            border-style: none;
        }
        table.t1
        {
            display: table;
            border-collapse: collapse;
        }
    </style>
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getReturnText() {
            var length = $('#lbRight option').length;
            var text = "";
            if (length > 0) {
                $('#lbRight option').each(function (i, selected) {
                    if (text.length > 0) text += ",";
                    text += $(selected).text();
                });
            }
            return text;
        }
        function getReturnValue() {
            var length = $('#lbRight option').length;
            var value = "";
            if (length > 0) {
                $('#lbRight option').each(function (i, selected) {
                    if (value.length > 0) value += ",";
                    value += $(selected).val();
                });
            }
            return value;
        }
        $(function () {
            // Initialization department tree 
            $('#ddlDept').combotree({
                onSelect: function (node) {
                    $("#pageloading").show();
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
            // Operating 
            $.ajax({
                type: "GET",
                url: "SelectUser.aspx?method=getdepts&s=" + Math.random(),
                dataType: 'text',
                success: function (data) {
                    $("#pageloading").hide();
                    if (data != "") {
                        var depts = eval('(' + data + ')');
                        $('#ddlDept').combotree('loadData', depts);
                    }
                }
            });
            $("#pageloading").hide();
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
                url: "SelectUser.aspx?method=getusers&s=" + Math.random(),
                dataType: 'html',
                data: { "DeptId": deptId, "SearchChild": searchChild, "StationId": stationId, "KeyWord": txtKeyWords },
                success: function (data) {
                    $("#pageloading").hide();
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
        // Determine the value selected 
        function SubmitValue() {
            var returnVal = getReturnValue();
            var returnText = getReturnText();
            if (window.opener != undefined) {
                window.top.returnValue = returnVal + "~" + returnText;
            } else {
                window.returnValue = returnVal + "~" + returnText;
            }
            window.close();
        }
        // Shut down 
        function CloseWin() {
            window.close();
        }
    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div region="center" border="true" style="margin: 0; padding: 0; overflow: hidden;">
        <div id="Div1" style="background-color: #fafafa; height: 30px;">
            <a id="submitWin" href="#" style="float: left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'"
                onclick="SubmitValue()"> Determine </a> 
            <a id="closeWin" href="#" style="float: left;" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'" 
                onclick="CloseWin()"> Cancel </a> 
        </div>
        <div style="margin-left: 10px; text-align: left; margin-top: 5px;">
            <table width="590px" border="0" class="t1">
                <tr>
                    <td>
                        <input id="ddlDept" class="easyui-combotree" style="width: 180px;" />
                        <input type="checkbox" id="cbContainChild" value="1" /> Search sub-sector &nbsp; &nbsp;
                        <asp:TextBox ID="txtKeyword" runat="server" MaxLength="10" Width="90px"></asp:TextBox>
                        <input type="button" id="btnSearch" value=" Search for " onclick="search()" /><input type="button"
                            id="btnSearchAll" value=" All " onclick="searchAll()" />
                    </td>
                </tr>
            </table>
            <div style="width: 590px; height: 350px; border: 2 inset; scrollbar-face-color: #DBEBFE;
                scrollbar-shadow-color: #B8D6FA; scrollbar-highlight-color: #FFFFFF; scrollbar-3dlight-color: #DBEBFE;
                scrollbar-darkshadow-color: #458CE4; scrollbar-track-color: #FFFFFF; scrollbar-arrow-color: #458CE4;
                margin-top: 5px;">
                <table width="430px" id="mytab" border="1" class="t2">
                    <tr>
                        <td align="left" width="40%">
                            <asp:ListBox ID="lbLeft" runat="server" Height="320px" Width="100%" ToolTip="Press Ctrl Select key "
                                CssClass="listbox" SelectionMode="Multiple" ondblclick="L2R()"></asp:ListBox>
                        </td>
                        <td width="15%" align="center">
                            <p>
                                <input type="button" value=" → " class="SmallButtonA" onclick="L2R()" />
                            </p>
                            <p>
                                <input type="button" value=" ← " class="SmallButtonA" onclick="R2L()" />
                            </p>
                        </td>
                        <td width="40%">
                            <asp:ListBox ID="lbRight" runat="server" Height="320px" Width="100%" CssClass="listbox"
                                SelectionMode="Multiple" ondblclick="R2L()"></asp:ListBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
