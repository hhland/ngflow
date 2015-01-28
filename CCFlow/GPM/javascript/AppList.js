var toolbar = [{ 'text': ' New ', 'iconCls': 'icon-new', 'handler': 'addApp' }, { 'text': ' Editing System Menu ', 'iconCls': 'icon-config', 'handler': 'EditMenus'}];

function addApp() {
    var url = "../WF/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Apps";
    window.showModalDialog(url, " Property ", "dialogWidth=800px;dialogHeight=500px;dialogTop=140px;dialogLeft=260px");
    LoadGrid();
}
function winOpen(url) {
    window.showModalDialog(url, " Property ", "dialogWidth=800px;dialogHeight=500px;dialogTop=140px;dialogLeft=260px");
    LoadGrid();
}
function AddTab(title, url) {
    window.parent.addTab(title, url);
}
// Edit Menu 
function EditMenus() {
    var row = $('#appGrid').datagrid('getSelected');
    if (row) {
        var url = "AppMenu.aspx?FK_App=" + row.No;
        AddTab(row.Name + ' System Menu ', url);
    }
    else {
        CC.Message.showError(" Prompted ", " Please select item !");
    }
}
function LoadGrid() {
    Application.data.getApps(function (js, scope) {
        if (js) {
            if (js == "") js = "[]";
            var pushData = eval('(' + js + ')');
            $('#appGrid').datagrid({
                data: pushData,
                width: 'auto',
                toolbar: toolbar,
                striped: true,
                rownumbers: true,
                singleSelect: true,
                loadMsg: ' Loading ......',
                columns: [[
                       {field: 'No', title: ' Serial number ', width: 60 },
                       { field: 'Name', title: ' Name ', width: 200, formatter: function (value, rec) {
                           var url = "../WF/Comm/RefFunc/UIEn.aspx?EnsName=BP.GPM.Apps&PK=" + rec.No
                           + "&No=" + rec.No
                           + "&AppModel=" + rec.AppModel
                           + "&FK_AppSort=" + rec.FK_AppSort
                           + "&OpenWay=" + rec.OpenWay;
                           return "<a href='javascript:void(0)' onclick=winOpen('" + url + "')>" + value + "</a>";
                       }
                       },
                       { field: 'AppModelText', title: ' Application Type ', width: 80, align: 'left' },
                       { field: 'FK_AppSortText', title: ' Category ', width: 60, align: 'left' },
                       { field: 'Url', title: ' Connection ', width: 160, align: 'left' },
                       { field: 'OpenWayText', title: ' Open ', width: 60 },
                       { field: 'Idx', title: ' Display Order ', width: 60 },
                       { field: 'MyFilePath', title: ' Whether to enable ', width: 60, formatter: function (value, rec) {
                           if (value == 1)
                               return "Yes";
                           else
                               return "No";
                       }
                       },
                       { field: 'RefMenuNo', title: ' Context Menu No. ', width: 90 },
                       { field: 'control', title: ' Operating ', width: 180, formatter: function (value, rec) {
                           var url = "AppMenu.aspx?FK_App=" + rec.No;
                           var title = "<a href='javascript:void(0)' onclick=AddTab('" + rec.Name + " System Menu ','" + url + "') > Editing System Menu </a>";
                           return title;
                       }
                       }
                       ]]
            });
        }
    }, this);

}
// The initial page 
$(function () {
    LoadGrid();
});