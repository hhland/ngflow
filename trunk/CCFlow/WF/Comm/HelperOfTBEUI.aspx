<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelperOfTBEUI.aspx.cs"
    Inherits="CCFlow.WF.Comm.HelperOfTBEUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Word Choice </title>
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
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
                    if (whichTaps != '#historyWordT') {
                        e.preventDefault();
                        $(whichTaps).tree("select", row.id);
                        $('#whatToDo').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                        selectId = row.id;
                    }
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

        function getSelectedTabInd() {
            var getTab = $('#tabs').tabs('getSelected');
            var getTaps = $('#tabs').tabs('getTabIndex', getTab);
            if (getTaps == 0) {
                whichTaps = "#pulicWordsGrid";
            }
            else if (getTaps == 1) {
                whichTaps = "#myWordsGrid";
            } else {
                whichTaps = "#historyWordT";
            }
        }

        function LoadData() {
            var params = {
                method: "getMyData",
                getTaps: getTaps
            };
            queryData(params, LoadDataCallBack, this);
        }

        // Initialization 
        $(function () {
            LoadData();
            $('#tabs').tabs({
                border: false,
                onSelect: function (title) {

                    if (title == ' My vocabulary ') {
                        getTaps = 1;
                    }
                    else if (title == ' Global vocabulary ') {
                        getTaps = 0;
                    } else {
                        getTaps = 2;
                    }
                    LoadData();
                }
            });
        });
        // Add the same node 
        function insertSameNode() {
            $.messager.prompt(' Prompt ', ' Please enter the vocabulary content ', function (r) {
                if (r) {
                    var params = {
                        method: "insertSameNodeMet",
                        selectId: selectId,
                        setText: encodeURI(r),
                        getTaps: getTaps
                    };
                    queryData(params, LoadDataCallBack, this);
                    LoadData();
                    selectId = "";
                }
            });
        }
        // Add a child node 
        function insertSonNode() {
            $.messager.prompt(' Prompt ', ' Please enter the vocabulary content ', function (r) {
                if (r) {
                    var params = {
                        method: "insertSonNodeMet",
                        selectId: selectId,
                        setText: encodeURI(r),
                        getTaps: getTaps
                    };
                    queryData(params, LoadDataCallBack, this);
                    LoadData();
                    selectId = "";
                }
            });
        }
        // Edit Node 
        function editNode() {
            $.messager.prompt(' Prompt ', ' Please enter the vocabulary content ', function (r) {
                if (r) {
                    var params = {
                        method: "editNodeMet",
                        selectId: selectId,
                        setText: encodeURI(r),
                        getTaps: getTaps
                    };
                    queryData(params, LoadDataCallBack, this);
                    LoadData();
                    selectId = "";
                }
            });
        }
        // Delete Node 
        function delNode() {
            $.messager.confirm(' Prompt ', ' OK to delete it ?', function (r) {
                if (r) {
                    var params = {
                        method: "delNodeMet",
                        selectId: selectId,
                        getTaps: getTaps
                    };
                    queryData(params, LoadDataCallBack, this);
                    LoadData();
                    selectId = "";
                }
            });
        }

        // Shut down 
        function closeMet() {
            window.close();
        }
        // Save Selection 
        function saveChecked() {
            var saveHistoryWord = ''; // History Glossary ---- Added    Last used previously reserved 30 Article vocabulary 
            var setMark = ',';
            var nodes = $('#tabs').tree('getChecked');
            getText = '';
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].attributes["IsParent"] == 0) {
                    getText += nodes[i].text;
                    if (i == nodes.length - 1) {
                        setMark = '';
                    }
                    saveHistoryWord += (nodes[i].text + setMark);
                }
            }
            if (getText == '') {
                $.messager.alert(" Prompt ", " You have not selected items !", "info");
                return;
            }
            // Compatible 
            var explorer = window.navigator.userAgent;
            if (explorer.indexOf("Chrome") >= 0) {// Google 
                window.close();
                window.opener.document.getElementById("ContentPlaceHolder1_ForwardUC1_Top_TB_Doc").value = getText;
            }
            else {//IE, Firefox 
                window.returnValue = getText;
                window.close(); // Close the child window 
            }
            // Compatible 

            var params = {
                method: "saveHistoryWordMet",
                saveHistoryWord: saveHistoryWord
            };

            queryData(params, LoadDataCallBack, this);
            selectId = "";
        }
        // Public Methods 
        function queryData(param, callback, scope, method, showErrMsg) {
            if (!method) method = 'GET';
            $.ajax({
                type: method, // Use GET或POST Method of accessing the background 
                dataType: "text", // Return json Data format 
                contentType: "application/json; charset=utf-8",
                url: "HelperOfTBEUI.aspx", // Backstage address to be accessed 
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
<body>
    <div class="easyui-tabs" id="tabs" data-options="tabWidth:112" style="width: auto;
        height: auto;">
        <div title=" Global vocabulary " style="height: 400px;">
            <div id=" GlobalWord" style="background-color: #fafafa; height: 25px;">
                <a id="A1" style="float: left; background-color: #fafafa; margin-left: 10px;" href="#"
                    class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'" onclick="saveChecked()">
                     Determine </a>&nbsp; &nbsp;<a id="A2" href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'"
                        onclick="closeMet()"> Shut down </a>
            </div>
            <div region="center" border="true">
                <ul class="easyui-tree" id="pulicWordsGrid" style="margin-left: 3px; margin-top: 5px;">
                </ul>
            </div>
        </div>
        <div title=" My vocabulary " style="height: 400px;">
            <div id="privateWord" style="background-color: #fafafa; height: 25px;">
                <a id="isOk" style="float: left; background-color: #fafafa; margin-left: 10px;" href="#"
                    class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'" onclick="saveChecked()">
                     Determine </a>&nbsp; &nbsp;<a id="close" href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'"
                        onclick="closeMet()"> Shut down </a>
            </div>
            <div region="center" border="true">
                <ul class="easyui-tree" id="myWordsGrid" style="margin-left: 3px; margin-top: 5px;">
                </ul>
            </div>
        </div>
        <div title=" History Glossary " style="height: 400px;">
            <div id="historyWord" style="background-color: #fafafa; height: 25px;">
                <a id="historyWordA" style="float: left; background-color: #fafafa; margin-left: 10px;"
                    href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'"
                    onclick="saveChecked()"> Determine </a>&nbsp; &nbsp;<a id="A4" href="#" class="easyui-linkbutton"
                        data-options="plain:true,iconCls:'icon-cancel'" onclick="closeMet()"> Shut down </a>
            </div>
            <div region="center" border="true">
                <ul class="easyui-tree" id="historyWordT" style="margin-left: 3px; margin-top: 5px;">
                </ul>
            </div>
        </div>
    </div>
    <div id="whatToDo" class="easyui-menu" style="width: 120px;">
        <div data-options="iconCls:'icon-add'" onclick="insertSameNode()">
             New sibling vocabulary </div>
        <div data-options="iconCls:'icon-add'" onclick="insertSonNode()">
             New child vocabulary </div>
        <div data-options="iconCls:'icon-save'" onclick="editNode()">
             Modify vocabulary </div>
        <div data-options="iconCls:'icon-save'" onclick="delNode()">
             Delete vocabulary </div>
    </div>
</body>
</html>
