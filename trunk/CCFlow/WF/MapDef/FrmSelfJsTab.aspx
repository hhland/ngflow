<%@ Page Title="" Language="C#" MasterPageFile="~/WF/MapDef/WinOpen.master" AutoEventWireup="true" CodeBehind="FrmSelfJsTab.aspx.cs" Inherits="CCFlow.WF.MapDef.FrmSelfJsTab" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%
    string  FK_MapData = Request.Params["FK_MapData"] ;
     
 %>

<script type="text/javascript">

    function saveSelfjs() {
        var url = "/WF/MapDef/FrmSelfJs.ashx?action=save&FK_MapData=<%=FK_MapData %>";
        var content = $("#txtselfjs").val();
        $.post(url, { content: content }, function (str) {
            window.location.reload();
        });
        
    }

    function loadSelfjs() {
        var url = "/DataUser/JsLibData/<%=FK_MapData %>_Self.js";
        $("#txtselfjs").load(url);
    }

    $(document).ready(function () {

        loadSelfjs();

    });

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<% 
   
        var OperAttrKey = Request.Params["OperAttrKey"] ;
        var FK_MapData = Request.Params["FK_MapData"] ;
        var MyPK = Request.Params["MyPK"] ;
        var __event = Request.Params["event"];
        var docid = Request.Params["docid"];
    string[] events =  {"onblur","onchange","onclick","ondbclick","onkeypress","onkeyup","onsubmit" };    
%>
<div class="easyui-layout" data-options="fit:true">
     <div class="easyui-tabs" data-options="region:'center'" >
		   <% 
         
         
         foreach (string _event in events)
        {
            string src = "/WF/MapDef/FrmSelfjs.aspx?MyPK+" + MyPK + "&FK_MapData=" + FK_MapData
                + "&OperAttrKey=" + OperAttrKey+"&event="+_event+"&docid="+docid
                ;
            string selected = _event == __event ? "true" : "false";

            %>
			<div data-options="title:'<%=_event %>',closable:false,selected:<%=selected %>">
               <iframe src="<%=src %>" style=" height:99%;width:99%" ></iframe>
            </div>
            <%} %>
		    
            <div data-options="title:'<%=FK_MapData %>_self.js',closable:false">
               <textarea id="txtselfjs" style="width:99%;height:96%"></textarea>
               <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" onclick="saveSelfjs()" >save</a>
            </div>

		</div>
</div>

</asp:Content>
