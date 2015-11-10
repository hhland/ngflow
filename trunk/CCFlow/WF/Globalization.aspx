<%@ Page Title="" Language="C#" MasterPageFile="~/WF/Comm/EUILayout.Master" AutoEventWireup="true" CodeBehind="Globalization.aspx.cs" Inherits="CCFlow.WF.Globalization" %>
<%@ Import Namespace="CCFlow.WF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        function saveLangRow(gridid, index, key) {
            var dg = $("#" + gridid);
            var editors = dg.datagrid("getEditors", index);
            var newRow = { isEditing: 0 ,key:key};
            for (var i = 0; i < editors.length; i++) {
                var editor = editors[i];
                var field = editor["field"];
                var el = dg.datagrid("getEditor", { index: index, field: field });
                var val = $(el.target).val();
                newRow[field] = val;
            }

            dg.datagrid('updateRow', { index: index, row: newRow });
           
            dg.datagrid("endEdit", index);
        }

        function cancelLangRowEdit(gridid, index) {
            var dg=$("#" + gridid);
            dg.datagrid('updateRow', { index: index, row: { isEditing:0 } });
            dg.datagrid("cancelEdit", index);
        }

        function beginLangRowEdit(gridid, index) {
            var dg = $("#" + gridid);
            dg.datagrid("beginEdit", index);
        }

        function onEndLangEdit(gridid, index, row, changes) {
            var url = "GlobalizationHandler.ashx?action=saveLang";
            $.post(url, row, function (str) {
                var json = eval("(" + str + ")");
                var msg=json["msg"];
                $.messager.show({
                    title: 'save result',
                    msg: msg
                });
            });
        }

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%
        string[] keys = GlobalizationHandler._globalResKeys();
        Dictionary<string, string> cluts = GlobalizationHandler.clutures;
    %>
   
     
    <div class="easyui-tabs" data-options="region:'center'" style="background:#eee;">
        <% foreach (var key in keys)
            {
                string gridid = "grid" + key;
               %>
           <div data-options="title:'<%=key %>'">
               <table class="easyui-datagrid" id="<%=gridid %>"  data-options="fit:true,pagination:false,rownumbers:true,pageSize:100000
              ,url:'GlobalizationHandler.ashx?action=clutureLangs&key=<%=key %>',onDblClickRow:function(index,row){ $('#<%=gridid %>').datagrid('beginEdit',index); }
                   ,onBeforeEdit:function(index,row){ $('#<%=gridid %>').datagrid('updateRow',{index:index,row:{isEditing:1}} ); }
                   ,onAfterEdit:function(index,row,changes){ onEndLangEdit('<%=gridid %>',index,row,changes); }
                   ,toolbar:[{text:'reload',iconCls: 'icon-reload',handler:function(){ $('#<%=gridid %>').datagrid('reload'); } }]
            ">
            <thead>
                <th>
                    <th data-options="field:'name',width:180">Name</th>
                    <th data-options="field:'op',width:80,formatter:function(value,row,index){ if(row['isEditing']) {return '<a href=javascript:saveLangRow(\'<%=gridid %>\','+index+',\'<%=key %>\') >save</a>&nbsp;&nbsp;<a href=javascript:cancelLangRowEdit(\'<%=gridid %>\','+index+')>cancel</a>'; } else {return '<a href=javascript:beginLangRowEdit(\'<%=gridid %>\','+index+',\'<%=key %>\') >edit</a>' ;} }">Oper</th>
                    <%
                        foreach (var clut in cluts.Keys)
                        {
                            string title = cluts[clut];
                            %>
                         <th data-options="field:'<%=clut %>',title:'<%=title %>(<%=clut %>)',width:220,editor:'textarea'"></th>
                       <%
                        }
                        %>
              </thead>
             </table>
           </div>
           
        <%
           } %>
        
   
    </div>

</asp:Content>
