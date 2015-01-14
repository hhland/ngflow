// Set up 
function SetEntity() {
    var fk_MapData = $("#EnNo").val();
    var url = "../MapDef/Rpt/Frm_ColsChose.aspx?FK_MapData=" + fk_MapData;
    $("<div id='dialogEnPanel'></div>").append($("<iframe width='100%' height='100%' frameborder=0 src='" + url + "'/>")).dialog({
        title: " Window ",
        width: 800,
        height: 620,
        autoOpen: true,
        modal: true,
        resizable: true,
        onClose: function () {
            $("#dialogEnPanel").remove();
            var pg = $('#ensGrid').datagrid('getPager');
            var curPage = $(pg).pagination.pageNumber;
            LoadGridData(curPage, 20);
        },
        buttons: [{ text: ' Shut down ',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dialogEnPanel').dialog("close");
            }
        }]
    });
}
// New 
function CreateEntityForm() {
    var fk_MapData = $("#EnNo").val();
    var params = {
        method: "getnewid",
        FK_MapData: fk_MapData
    };
    queryData(params, function (js, scope) {
        if (js) {
            OpenDialog(js, 'create');
        }
    }, this);
}
// Modification 
function EditEntityForm() {
    var row = $('#ensGrid').datagrid('getSelected');
    if (row) {
        OpenDialog(row["OID"], 'edit');
    } else {
        $.messager.alert(' Prompt ', ' Please try again after selecting records !', 'info');
    }
}
// Delete Record 
function DeleteEntity() {
    var row = $('#ensGrid').datagrid('getSelected');
    if (row) {
        $.messager.confirm(' Confirm ', ' You acknowledge that you want to delete records ?', function (r) {
            if (r) {
                var fk_MapData = $("#EnNo").val();
                var params = {
                    method: 'deleteentity',
                    FK_MapData: fk_MapData,
                    OID: row.OID
                };
                queryData(params, function (js, scope) {
                    var grid = $('#ensGrid');
                    var options = grid.datagrid('getPager').data("pagination").options;
                    var curPage = options.pageNumber;
                    LoadGridData(curPage, 20);
                }, this);
            }
        });
    } else {
        $.messager.alert(' Prompt ', ' Please try again after selecting records !', 'info');
    }
}
// Pop-up page 
function OpenDialog(oid, showModel) {
    var fk_MapData = $("#EnNo").val();
    var dialogModel = showModel;
    var date = new Date();
    var strTimeKey = "";
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS
    var url = "../CCForm/Frm.aspx?FK_MapData=" + fk_MapData + "&WorkID=" + oid + "&T=" + strTimeKey;
    var winWidth = document.body.clientWidth;
    // Calculations show that the width 
    winWidth = winWidth * 0.9;
    if (winWidth > 820) winWidth = 820;

    var winheight = document.body.clientHeight;
    // Calculations show that the height 
    winheight = winheight * 0.98
    if (winheight > 780) winheight = 780;

    $("<div id='dialogEnPanel' style='z-index: 9999;'></div>").append($("<iframe id='dialogFrame' width='100%' height='100%' onload='focus()' frameborder=0 src='" + url + "'/>")).dialog({
        title: " Window ",
        width: winWidth,
        height: winheight,
        autoOpen: true,
        modal: true,
        resizable: true,
        onClose: function () {
            // Do not save it deleted 
            if (dialogModel == "create") {
                var fk_MapData = $("#EnNo").val();
                var params = {
                    method: 'deleteentity',
                    FK_MapData: fk_MapData,
                    OID: oid
                };
                queryData(params, function (js, scope) { }, this);
            }
            $("#dialogFrame").remove();
            $("#dialogEnPanel").remove();
            var pg = $('#ensGrid').datagrid('getPager');
            var curPage = $(pg).pagination.pageNumber;
            LoadGridData(curPage, 20);
        },
        buttons: [{ text: ' Save ',
            iconCls: 'icon-ok',
            handler: function () {
                var frame = document.getElementById("dialogFrame");
                var contentWidow = frame.contentWindow;
                contentWidow.SaveDtlData();
                dialogModel = "edit";
                $('#dialogEnPanel').dialog("close");
            }
        }, { text: ' Shut down ',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dialogEnPanel').dialog("close");
            }
        }]
    });
}
// Load table data 
function LoadGridData(pageNumber, pageSize) {
    var fk_Mapdata = $("#EnNo").val();
    var params = {
        method: "getensgriddata",
        FK_MapData: fk_Mapdata,
        pageNumber: pageNumber,
        pageSize: pageSize
    };
    queryData(params, function (js, scope) {
        $("#pageloading").hide();
        if (js) {
            if (js == "") js = "[]";

            // System error 
            if (js.status && js.status == 500) {
                $("body").html("<b style='color:red;'> Please pass the correct parameter name .<b>");
                return;
            }

            var pushData = eval('(' + js + ')');
            var fitColumns = true;
            if (pushData.columns.length > 7) {
                fitColumns = false;
            }
            $('#ensGrid').datagrid({
                columns: [pushData.columns],
                data: pushData.data,
                width: 'auto',
                height: 'auto',
                striped: true,
                rownumbers: true,
                singleSelect: true,
                pagination: true,
                remoteSort: false,
                fitColumns: fitColumns,
                pageNumber: pageNumber,
                pageSize: pageSize,
                pageList: [20, 30, 40, 50],
                onDblClickCell: function (index, field, value) {
                    EditEntityForm();
                },
                loadMsg: ' Loading ......'
            });

            var pg = $("#ensGrid").datagrid("getPager");
            if (pg) {
                $(pg).pagination({
                    onRefresh: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    },
                    onSelectPage: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    }
                });
            }

        } else {
            $.messager.confirm(' Confirmation dialog ', ' This table does not set the display column , Is now set ?', function (r) {
                if (r) {
                    SetEntity();
                }
            });
        }
    }, this);
}

$(function () {
    LoadGridData(1, 20);
});

// Public Methods 
function queryData(param, callback, scope, method, showErrMsg) {
    if (!method) method = 'GET';
    $.ajax({
        type: method, // Use GET或POST Method of accessing the background 
        dataType: "text", // Return json Data format 
        contentType: "application/json; charset=utf-8",
        url: "SearchEUI.aspx", // Backstage address to be accessed 
        data: param, // Data to be transmitted 
        async: false,
        cache: false,
        complete: function () { }, //AJAX When the request is complete Hide loading Prompt 
        error: function (XMLHttpRequest, errorThrown) {
            $("body").html("<b> Access page fault , Incoming parameter error .<b>");
            //callback(XMLHttpRequest);
        },
        success: function (msg) {//msg For the returned data , Here to do data binding 
            var data = msg;
            callback(data, scope);
        }
    });
}