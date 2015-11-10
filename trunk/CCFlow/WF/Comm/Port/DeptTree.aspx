<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptTree.aspx.cs" Inherits="CCFlow.WF.Comm.Port.DeptTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../Scripts/CommonUnite.js" type="text/javascript"></script>
    <script type="text/javascript">
        // Load grid After the callback function 
        function LoadDataGridCallBack(js, scorp) {
            $("#pageloading").hide();
            if (js == "") js = "[]";

            // System error 
            if (js.status && js.status == 500) {
                $("body").html("<b> Access page fault , Please contact the administrator .<b>");
                return;
            }

            var pushData = eval('(' + js + ')');

            $("#tt").tree({
                idField: 'id',
                iconCls: 'tree-folder',
                data: pushData,
                checkbox: true,
                collapsed: true,
                animate: true,
                width: 300,
                height: 400,
                lines: true,
                onExpand: function (node) {
                    if (node) {
                    }
                },
                onClick: function (node) {
                    if (node) {
                    }
                }
            });
        }
        // Load DeptTree
        function LoadDeptTreeData() {
            var FK_Node = Application.common.getArgsFromHref("FK_Node");
            var params = {
                method: "getTreeDateMet",
                FK_Node: FK_Node
            };
            queryData(params, LoadDataGridCallBack, this);
        }

        // Initialization 
        $(function () {
            LoadDeptTreeData();
        });

        // Get the selected item id
        function getChecked() {
            var nodes = $('#tt').tree('getChecked');
            var getId = '';
            for (var i = 0; i < nodes.length; i++) {
                if (getId != '') getId += ',';
                getId += nodes[i].id;
            }

            //  Get URL Value pass 
            var FK_Node = Application.common.getArgsFromHref("FK_Node");
            var params = {
                method: "insertMet",
                getId: getId,
                FK_Node: FK_Node
            };
            queryData(params, function (js, scope) {
                if (js == "true")
                    $.messager.alert(" Prompt ", " Saved successfully !", "info");
            }, this);
        }
        // Shut down 
        function cancelMet() {
            window.close();
        }

        // Public Methods 
        function queryData(param, callback, scope, method, showErrMsg) {
            if (!method) method = 'GET';
            $.ajax({
                type: method, // Use GET或POST Method of accessing the background 
                dataType: "text", // Return json Data format 
                contentType: "application/json; charset=utf-8",
                url: "DeptTree.aspx", // Backstage address to be accessed 
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
    </script>
</head>
<body class="easyui-layout" style="overflow-y: hidden" fit="true">
    <div region="north" split="false" border="false" title=" Select node department " iconcls='icon-department'
        style="height: 53px;">
        <div id="tb" style="background-color: #fafafa; height: 25px;">
            <a id="isOk" style="float: left; background-color: #fafafa; margin-left: 10px;" href="#"
                class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'" onclick="getChecked()">
                 Save </a>&nbsp; &nbsp;<a id="delMeeting" href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'"
                    onclick="cancelMet()"> Shut down </a>
        </div>
    </div>
    <div region="center" border="true" style="margin: 0; padding: 0; overflow: auto;">
        <ul class="easyui-tree" id="tt" style="margin-left:10px;">
        </ul>
    </div>
</body>
</html>
