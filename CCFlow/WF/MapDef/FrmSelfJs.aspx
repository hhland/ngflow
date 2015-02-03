<%@ Page Title="" Language="C#" MasterPageFile="~/WF/MapDef/WinOpen.master" AutoEventWireup="true" CodeBehind="FrmSelfJs.aspx.cs" Inherits="CCFlow.WF.MapDef.FrmSelfJs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var _event = "<%=Request.Params["event"] %>";
        var OperAttrKey = "<%=Request.Params["OperAttrKey"] %>";
        var FK_MapData = "<%=Request.Params["FK_MapData"] %>";
        var fileurl = "/DataUser/JSLibData/" + FK_MapData + "_Self.js";
        var frmselfjsurl = "/WF/MapDef/FrmSelfJs.ashx";
        var winid = "<%=Request.Params["winid"] %>";
        var docid = "<%=Request.Params["docid"] %>";
        function loadfile() {
            var url = frmselfjsurl + "?action=read&FK_MapData=" + FK_MapData + "&OperAttrKey=" + OperAttrKey
                + "&event=" + _event;
            $("#content_selfjs").load(url);
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

        function saveRegion() {
            //var url = frmselfjsurl + "?action=save";
            var content = $("#content_selfjs").val();
            $.messager.show({ title: "saved", msg: " saved" });
            _saveRegion(content);
            
            var $docel = $(window.parent.document).find("[name$='" + docid + "'");
            if ($.trim($docel.val()) == "") {
               var funcall = OperAttrKey + "_" + _event + "(this);";
                $docel.val(funcall);
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout" data-options="fit:true">
       <div data-options="region:'west',title:'Function List',split:true" style="width:160px;"></div>
    <div data-options="region:'center',title:'Final Java Script'" style="padding:5px;background:#eee;">
        
        <textarea id="content_selfjs" name="content" style="width: 99%;height: 99%"></textarea>
        
    </div>
     <div data-options="region:'south',title:''" style="height:30px;">
         <a href="javascript:test()" id="mb" class="easyui-linkbutton" 
        data-options="iconCls:'icon-edit'">Test</a>
        <a href="javascript:saveRegion()" id="A1" class="easyui-linkbutton" 
        data-options="iconCls:'icon-save'">Save</a>
        <a href="javascript:saveRegion();closeWin();" id="A2" class="easyui-linkbutton" 
        data-options="iconCls:'icon-save'">Save and Close</a>
        
     </div>
    </div>
</asp:Content>
