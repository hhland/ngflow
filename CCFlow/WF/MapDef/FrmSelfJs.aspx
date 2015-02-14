<%@ Page Title="" Language="C#" MasterPageFile="~/WF/MapDef/WinOpen.master" AutoEventWireup="true" CodeBehind="FrmSelfJs.aspx.cs" Inherits="CCFlow.WF.MapDef.FrmSelfJs" %>
<%@ Import Namespace="System.IO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _event = "<%=Request.Params["event"] %>";
        var OperAttrKey = "<%=Request.Params["OperAttrKey"] %>";
        var FK_MapData = "<%=Request.Params["FK_MapData"] %>";
        var MyPK = "<%=Request.Params["MyPK"] %>";
        var fileurl = "/DataUser/JSLibData/" + FK_MapData + "_Self.js";
        var frmselfjsurl = "/WF/MapDef/FrmSelfJs.ashx";
        var mapexturl = "/WF/MapDef/MapExt.ashx";
        var winid = "<%=Request.Params["winid"] %>";
        var docid = "<%=Request.Params["docid"] %>";
        var r = Math.random();
        function loadfile() {
            var url = frmselfjsurl + "?action=read&FK_MapData=" + FK_MapData + "&OperAttrKey=" + OperAttrKey
                + "&event=" + _event+"&r="+r;
            $("#content_selfjs").load(url);
        }

        function loadjslib() {
            var url = frmselfjsurl + "?action=jslib&event=" + _event+"&r="+r;
            $.get(url,{},function(grid) {
                var rows = grid["rows"]||[];
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    $("#dg_jslib").datagrid("appendRow", row);
                }
            });
        }

        $(document).ready(function() {
            loadfile();
        });

        function _saveRegion(content) {
            
            var url = frmselfjsurl + "?action=save&FK_MapData="+FK_MapData;
            $.post(url, { region: content,OperAttrKey:OperAttrKey,event:_event },
            function(json) {
                
                var msg = json["msg"];
                $.messager.show({title:"save",msg:msg});
               
            },"json");
        }

        function saveDoc(doc) {
            //var url = mapexturl + "?action=updateDoc&MyPK="+MyPK;
            //$.post(url, { doc: doc },function (str){});
           
            $(window.parent.document).find("[name$='Btn_Save']").click();
        }

        function saveRegion() {
            //var url = frmselfjsurl + "?action=save";
            var content = $("#content_selfjs").val();
            $.messager.show({ title: "saved", msg: " saved" });
            _saveRegion(content);
            
            var $docel = $(window.parent.document).find("[name$='" + docid + "'");
            if ($.trim($docel.val()) == "") {
                var funname = OperAttrKey + "_" + _event;
               var funcall = "javascript:"+funname+ "(ele);";
               if (_event == "onclick") {
                   funcall = funname + "(this);";
               }
                $docel.val(funcall);
                var msg = "the " + _event + " content is blank,are you want to fill it with '" + funcall + "' and save it now?";
                if (confirm(msg)) {
                    saveDoc(funcall);     
                }
               
            }
        }

        function preview() {
            var url = "/WF/CCForm/Frm.aspx?FK_MapData="+FK_MapData+"&IsTest=1&WorkID=0&FK_Node=999999&s=2&T="+Math.random();
            WinOpen(url, "preview form:"+FK_MapData);
        }

        function test() {
            if (confirm("save before test?")) {
                saveRegion();
            }
            preview();
        }

        function closeWin() {
            $(window.parent.document).find("#" + winid).window("destroy",true);
        }


        function prepentJslib(filename,funcname) {
            var src = "/DataUser/JsLib/" + _event + "/" + filename;
            var js = "document.writeln(\"<script src='"+src+"'><"+"/script>\") \n\n";
            js = "importScript(\"" + src + "\"); // "+funcname+" \n";
           // alert(js);
           
            $("#content_selfjs").prepend(js);
        }

    </script>
    
    <style>
        .datagrid-cell {
    white-space:normal
}
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout" data-options="fit:true">
       <div data-options="region:'west',title:'/DataUser/JSLib/<%=Request.Params["event"] %> ',split:true" style="width:280px;">
          <table class="easyui-datagrid" 
        data-options="fit:true,fitColumns:true,singleSelect:true">
    <thead>
        <tr>
           
            <th data-options="field:'name',width:100">Name</th>
            <th data-options="field:'op',width:30">opera</th>
        </tr>
    </thead>
    <tbody>
        <%
            string _event = Request.Params["event"];
            foreach (FileInfo jslibfile in jslibfiles)
            {
                StreamReader sr=new StreamReader(jslibfile.OpenRead());
                string content = sr.ReadToEnd();
                int funstart = content.IndexOf("function") + "function".Length, funend = content.IndexOf("{");
                    ;

                    string funcname = "";
                    if (funstart >= 0&&funend>=0)
                    {
                       funcname= content.Substring(funstart,funend- funstart );         
                    }
               
                %>
                <tr>
                    <td><span title="<%=content %>"><%=jslibfile.Name %></span></td>
                    <td>
                        <span style="cursor: pointer" title="import" onclick="prepentJslib('<%=jslibfile.Name %>','<%=funcname %>')"><img src="/WF/Scripts/easyUI/themes/icons/edit_add.png" /></span>
                        
                        <a  target="_blank" title="view" href="/DataUser/JSLib/<%=_event %>/<%=jslibfile.Name %>" ><img src="/WF/Scripts/easyUI/themes/icons/search.png" /></a>
                        
                    </td>
                </tr>
                <%
            }
            %>
    </tbody>
</table>
       </div>
    <div data-options="region:'center',title:'Final Java Script'" style="padding:5px;background:#eee;">
        
        <textarea id="content_selfjs" name="content" style="width: 99%;height: 99%"></textarea>
        
    </div>
     <div data-options="region:'south',title:''" style="height:30px;">
         <a target="_blank" href="/WF/CCForm/Frm.aspx?FK_MapData=<%=Request.Params["FK_MapData"] %>&IsTest=1&WorkID=0&FK_Node=999999&s=2&T=xxx" 
         id="mb" class="easyui-linkbutton" 
        data-options="iconCls:'icon-edit'">Test</a>
        <a href="javascript:saveRegion()" id="A1" class="easyui-linkbutton" 
        data-options="iconCls:'icon-save'">Save</a>
        <!--
        <a href="javascript:saveRegion();closeWin();" id="A2" class="easyui-linkbutton" 
        data-options="iconCls:'icon-save'">Save and Close</a>
        -->
     </div>
    </div>
</asp:Content>
