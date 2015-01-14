<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimulationRunEUI.aspx.cs"
    Inherits="CCFlow.WF.Admin.SimulationRunEUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Process Simulator </title>
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../Scripts/CommonUnite.js" type="text/javascript"></script>
    <script type="text/javascript">
        // Load grid After the callback function 
        var selectId;
        var whichTaps;
        var getTaps = 0;
        var getText;

        function LoadDataCallBack(js, scorp) {
            $("#pageloading").hide();
            if (js == "") js = "[]";

            if (js.status && js.status == 500) {
                $("body").html("<b> Access page fault , Please contact the administrator .<b>");
                return;
            }
            getSelectedTabInd();
            var pushData = eval('(' + js + ')');
            $(whichTaps).tree({
                idField: 'id',
                iconCls: 'tree-folder',
                data: pushData,
                checkbox: true,
                collapsed: true,
                animate: true,
                width: 300,
                height: 400,
                lines: true,
                onContextMenu: function (e, row) {
                    e.preventDefault();
                    $(whichTaps).tree("select", row.id);
                    $('#whatToDo').menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    });
                    selectId = row.id;
                },
                onExpand: function (node) {
                    if (node) {
                    }
                },
                onClick: function (node) {
                    if (node) {
                        $(whichTaps).tree("check", node.target);
                    }
                }
            });
        }

        function LoadData() {
            var params = {
                method: "getMyData",
                getTaps: getTaps
            };
            queryData(params, LoadDataCallBack, this);
        }
        setTimeout("closeCurDlg()", 5000);

        function closeCurDlg() {
            $('#Curdlg').dialog('close');
        }
        var getEmps;
        // Initialization 
        $(function () {
            $('#dlg').dialog('close');
            $('#Curdlg').dialog('close');
            // Automatically increase Panel
            var urlEmps = Application.common.getArgsFromHref("IDs");
            getEmps = urlEmps.split(',');
            for (var i = 0; i < getEmps.length; i++) {
                if (getEmps[i] != '') {
                    index++;
                    $('#tt').tabs('add', {
                        title: ' Number sponsor ---' + getEmps[i],
                        content: '<div style="padding:10px">Content' + index + '</div>',
                        closable: true
                    });
                }
            }


        });

        function bindMet(resetStatus) {
            if (resetStatus.className == 'model') {
                if (resetStatus.id == 'modelOne') {
                    document.getElementById("modelTwo").checked = false;
                } else {
                    document.getElementById("modelOne").checked = false;
                }
            }
            else {
                if (resetStatus.id == 'TimeSOne') {
                    document.getElementById("TimeSTwo").checked = false;
                } else {
                    document.getElementById("TimeSOne").checked = false;
                }
            }
        }

        // Public Methods 
        function queryData(param, callback, scope, method, showErrMsg) {
            if (!method) method = 'GET';
            $.ajax({
                type: method, // Use GET或POST Method of accessing the background 
                dataType: "text", // Return json Data format 
                contentType: "application/json; charset=utf-8",
                url: "SimulationRunEUI.aspx", // Backstage address to be accessed 
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
        // Operating tabs
        var index = 0;
        function addPanel() {
            $("#dlg").dialog({
                iconCls: 'icon-user',
                buttons: [{
                    text: ' Determine ',
                    iconCls: 'icon-ok'
                }, {
                    text: ' Cancel ',
                    handler: function () {
                        $('#dlg').dialog('close');
                    }
                }]
            });
            $('#dlg').dialog('open');
        }
        function removePanel() {
            var tab = $('#tt').tabs('getSelected');
            if (tab) {
                var index = $('#tt').tabs('getTabIndex', tab);
                $('#tt').tabs('close', index);
            }
        }
    </script>
</head>
<body>
    <div id="tt" class="easyui-tabs" data-options="tools:'#tab-tools'" style="width: auto;
        height: 600px">
        <div title=" About Process Simulator " style="padding: 10px; width: 700px; height: 250px">
        </div>
    </div>
    <div id="tab-tools">
        <a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-config'"
            onclick="addPanel()"></a><a href="javascript:void(0)" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-delete'"
                onclick="removePanel()"></a>
    </div>
    <div id="dlg" class="easyui-dialog" title=" Simulator parameter settings " style="width: 400px; height: 200px;
        padding: 10px">
        <div>
            <div>
                 Run mode </div>
            <input type="radio" id="modelOne" class="model" onclick="bindMet(this)" checked="checked" /> Serial 
            <input type="radio" id="modelTwo" class="model" onclick="bindMet(this)" /> Parallel </div>
        <div style="margin-top: 20px;">
            <div>
                 Interval </div>
            <input type="radio" id="TimeSOne" class="TimeS" onclick="bindMet(this)" /> Speed mode 
            <input type="radio" id="TimeSTwo" class="TimeS" onclick="bindMet(this)" checked="checked" /> Interval three seconds </div>
    </div>
    <div id="CurDlg" class="easyui-dialog" title=" The current node " style="width: 400px; height: 200px;
        padding: 10px">
    </div>
</body>
</html>
