<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUploadEUI.aspx.cs"
    Inherits="CCFlow.WF.FileUploadEUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> File upload </title>
    <script src="/WF/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="/WF/Scripts/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
    <link href="/WF/Scripts/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <script src="/WF/Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <link href="/WF/Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="/WF/Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            loadGrid();
        });

        function loadGrid() {
            var loadType = $('#LoadType').combobox('getValue');
            $("#maingrid").datagrid({
                nowrap: true,
                title: ' File List ',
                fitColumns: true,
                fit: true,
                singleSelect: true,
                autoRowHeight: false,
                striped: true,
                toolbar: '#tb',
                collapsible: false,
                url: location.href + "?type=load&LoadType=" + loadType,
                rownumbers: true,
                onDblClickRow: function (idx, rowData) {
                    if (rowData.Name != '') {
                        if (rowData.Type == 'doc' || rowData.Type == 'docx' || rowData.Type == 'xlsx') {
                            window.open("WebOffice/OfficeView.aspx?IsEdit=1&Path=" + encodeURI("/DataUser/") + $("#LoadType").combobox('getValue') + "/" + rowData.Name);

                        }

                    }
                },
                columns: [[
                   { title: ' Name  ', field: 'Name', width: 160, align: 'left', formatter: function (value, rec) {
                       return rec.Name;
                   }
                   },
                   { title: ' Type ', field: 'Type' },
                   { title: ' Size (KB)', field: 'Size' }
                   ]]
            });
        }

        function remove() {
            var row = $('#maingrid').datagrid('getSelected');
            if (row == null) {
                $.messager.alert(' Prompt ', ' Please select the file to be deleted !');
                return;
            }
            pageLoadding(' File deletion !');
            var loadType = $('#LoadType').combobox('getValue');
            $.ajax({
                type: "post", // Use GET或POST Method of accessing the background 
                dataType: "text", // Return json Data format 
                contentType: "application/json; charset=utf-8",
                url: location.href + "?type=delete&LoadType=" + loadType + "&name=" + row.Name, // Backstage address to be accessed 
                data: "", // Data to be transmitted 
                async: true,
                cache: false,
                complete: function () { }, //AJAX When the request is complete Hide loading Prompt 
                error: function (XMLHttpRequest, errorThrown) {
                    loaddingOut(" Delete failed !");
                },
                success: function (msg) {//msg For the returned data , Here to do data binding 
                    if (msg == "true") {
                        loaddingOut(" Deleted successfully !");
                        loadGrid();
                    } else {

                        loaddingOut(" Delete failed !");
                        $.messager.alert(' Prompt ', msg);
                    }
                }
            });


        }
        function add() {
            $('#addWin').window('open');
            $('#addWin').parent().appendTo($("form:first"));
        }

        function checkForm() {
            if ($('#fileUpload').val() == '') {
                $.messager.alert(' Prompt ', ' Please select the file !');
                return false;
            }
            pageLoadding(' Preservation ...');
            return true;
        }

        function pageLoadding(msg) {
            $.jBox.tip(msg, 'loading');
        }
        function loaddingOut(msg) {
            $.jBox.tip(msg, 'success');
        }
    </script>
    <style type="text/css">
        .btn
        {
            border: 0;
            background: #4D77A7;
            color: #FFF;
            font-size: 12px;
            padding: 6px 10px;
            margin: 5px 0;
        }
    </style>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'center'">
        <div id="maingrid" style="margin: 0; padding: 0;">
        </div>
    </div>
    <div id="tb" style="padding: 5px; height: auto">
        <a onclick="add();" class="easyui-linkbutton" iconcls="icon-add" plain="true"></a>
        <a onclick="remove()" class="easyui-linkbutton" iconcls="icon-remove" plain="true">
        </a>
        <asp:DropDownList runat="server" class="easyui-combobox" name="LoadType" data-options="onSelect: function(rec){loadGrid();},panelHeight: 'auto'"
            ID="LoadType" Style="width: 100px;">
            <asp:ListItem Text=" Tao Hong file " Value="OfficeOverTemplate"></asp:ListItem>
            <asp:ListItem Text=" Document Templates " Value="OfficeTemplate"></asp:ListItem>
            <asp:ListItem Text=" Official signature " Value="OfficeSeal"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div id="addWin" class="easyui-window" title=" Add to " data-options="modal:true,closed:true,iconCls:'icon-add',maximizable:false,minimizable:false"
        style="width: 500px; height: 200px; padding: 10px;">
         File :
        <br />
        <asp:FileUpload ID="fileUpload" runat="server" />
        <br />
        <asp:Button ID="btnConfirm" runat="server" class="btn" OnClientClick="return checkForm()"
            Text=" Determine " OnClick="btnConfirm_Click" />
    </div>
    </form>
</body>
</html>
