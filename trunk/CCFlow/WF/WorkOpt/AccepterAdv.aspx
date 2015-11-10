<%@ Page Title=" Recipient selector " Language="C#" MasterPageFile="~/WF/Rpt/SiteEUI.Master" AutoEventWireup="true"
    CodeBehind="AccepterAdv.aspx.cs" Inherits="CCFlow.WF.WorkOpt.AccepterAdv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .wordPanel
        {
            width: 100%;
            height: 100%;
            overflow: auto;
        }
        a:link
        {
            text-decoration: none;
        }
        a:visited
        {
            text-decoration: none;
        }
        a:hover
        {
            text-decoration: none;
        }
        a:active
        {
            text-decoration: none;
        }
        .cs-navi-tab
        {
            padding: 2px;
            display: block;
            line-height: 16px;
            height: 16px;
            padding-left: 16px;
            text-decoration: none;
            border: 1px solid white;
            border-bottom: 1px #E5E5E5 solid;
        }
        .cs-navi-tab:hover
        {
            background: #FFEEAC;
            border: 1px solid #DB9F00;
        }
    </style>
    <script language="javascript" type="text/javascript">
        // Save 
        function Save() {

        }

        // Cancel 
        function CloseWin() {
            window.close();
        }
        // Initialization spare vocabulary 
        function InitReserveWords(NodeID) {
            $("#leftWordPanel").html('');
            $("#rightWordPanel").html('');

            var params = {
                method: 'getreservewords',
                FK_Node: NodeID
            };
            queryData(params, function (js, scope) {
                var pushData = eval('(' + js + ')');

                // Left the vocabulary 
                var leftWords = pushData.LeftWords;
                for (var i = leftWords.length - 1; i >= 0; i--) {
                    $("<a id='leftWord" + i + "' class='cs-navi-tab' href=javascript:AddWordToContent('" + leftWords[i].word + "','leftWord" + i + "');>" + leftWords[i].word + "</a>").prependTo("#leftWordPanel");
                }
                // The right of the words 
                var rightWords = pushData.RightWords;
                for (var j = leftWords.length-1; j >= 0; j--) {
                    $("<a class='cs-navi-tab' href=javascript:AddWordToContent('" + rightWords[j].word + "');>" + rightWords[j].word + "</a>").prependTo("#rightWordPanel");
                }

            }, this);
        }
        function AddWordToContent(word, ctrID) {
            //            $("p").remove("#second");
            //$("#" + ctrID).remove();     // Delete matching elements 
            var content = $("#wordContent").val();
            content += word;
            $("#wordContent").val(content);
        }

        // Initialization Tab页
        function InitTabPage() {
            var curNodeID = Application.common.getArgsFromHref("FK_Node");
            var workID = Application.common.getArgsFromHref("WorkID");

            var params = {
                method: 'getdeliverynode',
                FK_Node: curNodeID,
                WorkID: workID
            };
            queryData(params, function (js, scope) {
                var pushData = eval('(' + js + ')');
                
                for (var i = 0; i < pushData.length; i++) {
                    var tabContent = "<div id='" + pushData[i].NodeID + "_Tab'  class='easyui-layout'>";
                    tabContent += "     <div id='" + pushData[i].NodeID + "_Emps' data-options=\"region:'west',split:false,collapsible:false\" title=' Alternative entry and punctuation ' style='width: 180px;overflow: hidden;'>sdd</div>";
                    tabContent += "     <div id='" + pushData[i].NodeID + "_Checked' data-options=\"region:'east',split:false,collapsible:false\" title=' Alternative entry ' style='width: 180px;'>sdff</div>";
                    tabContent += "</div>";
                    $('#tabs').tabs('add', {
                        title: pushData[i].Name,
                        id: pushData[i].NodeID,
                        content: tabContent,
                        closable: false
                    });
                }
                // Choose Tab页, Get people and alternative vocabulary 
                $('#tabs').tabs({
                    onSelect: function (title, index) {
                        var tab = $('#tabs').tabs('getSelected');
                        InitReserveWords(tab[0].id);
                    }
                });
            }, this);
        }
        $(function () {
            $("#pageloading").hide();
            InitTabPage();
        });

        // Public Methods 
        function queryData(param, callback, scope, method, showErrMsg) {
            if (!method) method = 'GET';
            $.ajax({
                type: method, // Use GET或POST Method of accessing the background 
                dataType: "text", // Return json Data format 
                contentType: "application/json; charset=utf-8",
                url: "AccepterAdv.aspx", // Backstage address to be accessed 
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="pageloading">
    </div>
    <div data-options="region:'north',split:false" style="height: 34px; overflow: hidden;">
        <div style="padding: 3px; background: #fafafa;">
            <a href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-save'" onclick="Save()"> Save </a> 
            <a href="#" class="easyui-linkbutton" data-options="plain:true,iconCls:'icon-cancel'" onclick="CloseWin()"> Cancel </a>
        </div>
    </div>
    <div data-options="region:'west',split:false,collapsible:false" title=" Alternative entry and punctuation " style="width: 180px;
        overflow: hidden;">
        <div id="leftWordPanel" class="wordPanel">
        </div>
    </div>
    <div region="center" border="true" style="margin: 0; padding: 0; overflow: hidden;">
        <div id="tabs" class="easyui-tabs" fit="true" border="false">
        </div>
    </div>
    <div data-options="region:'east',split:false,collapsible:false" title=" Alternative entry " style="width: 180px;">
        <div id="rightWordPanel" class="wordPanel">
        </div>
    </div>
    <div data-options="region:'south',split:false,collapsible:false" title=" Handle comments " style="height: 160px;">
        <textarea id="wordContent" rows="5" cols="20" style="width: 99%; height: 95%;"></textarea>
    </div>
</asp:Content>
