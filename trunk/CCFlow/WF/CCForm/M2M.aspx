<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.Comm_M2M"
    CodeBehind="M2M.aspx.cs" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<div id='div_loading' style='z-index: 20001; position: absolute; left: 40%; top: 20%;
                                     height: auto; opacity: 0.8; filter: alpha(opacity=80); padding: 2px;'>
    <div style='text-align: center; margin: 0 auto; color: #444; background: white;'>
        <img src="../Img/loading.gif" alt="" width="24" height="24" align="absmiddle"
            style="margin-left: 8px;" />
        <span style="font-size: 11pt;">loading...</span>
    </div>
</div>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        body
        {
            font-size: smaller;
        }

        .hitkeyword {
            background-color: yellowgreen;
        }

        .autocomplete-suggestions { border: 1px solid #999; background: #FFF; cursor: default; overflow: auto; -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); }
.autocomplete-suggestion { padding: 2px 5px; white-space: nowrap; overflow: hidden; }
.autocomplete-no-suggestion { padding: 2px 5px;}
.autocomplete-selected { background: #F0F0F0; }
.autocomplete-suggestions strong { font-weight: bold; color: #000; }
.autocomplete-group { padding: 2px 5px; }
.autocomplete-group strong { font-weight: bold; font-size: 16px; color: #000; display: block; border-bottom: 1px solid #000; }

    </style>

    <script src="../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../Scripts/jquery.zclip.min.js"></script>
    <script src="../Scripts/jquery.autocomplete.min.js" ></script>
    <script language="javascript">
        var isChange = false, searchitems=[];

        $.fn.pasteEvents = function (delay) {
            if (delay == undefined) delay = 20;
            return $(this).each(function () {
                var $el = $(this);
                $el.on("paste", function () {
                    $el.trigger("prepaste");
                    setTimeout(function () { $el.trigger("postpaste"); }, delay);
                });
            });
        };

        function initSearchTxt() {
            var elsearch = $("#txtsearch");
            elsearch.bind("postpaste", function (e) {
                var val = elsearch.val();
                _search(val,true);
            }).pasteEvents();


            $("#form1").find("label").each(function(index) {
                var text = $(this).text();
                searchitems.push({ data: text, value: text });
            });

            elsearch.autocomplete({
                lookup: searchitems
                , triggerSelectOnValidInput: false
                , onSelect: function (suggestion) { _search(suggestion["data"], true); }
            });
        }

        function initSummary() {
            var groupitems = $("table.groupitems");
            groupitems.each(function () {
                var self = $(this);
                var allcheckbox = self.find("input"), checkedcheckbox = self.find("input[checked='checked']");
                var grouptr= self.parents("tr:first").prev();
                var groupsummary =grouptr.find("span.groupsummary");
                var checklens=checkedcheckbox.length;
                var summarytext = "["+checklens + "/" + allcheckbox.length+"]";
                groupsummary.text(summarytext);
                if (checklens > 0) {
                    grouptr.find("input:first").attr("checked", true);
                    grouptr.find("img").attr("src", "../Img/Min.gif");
                }
            });
        }

        

        function initcopyselected() {
            $('#copydynamic').zclip({
                path: '../Scripts/ZeroClipboard.swf',
                copy: function () {
                    var copytext = "";
                    var groupitems = $("table.groupitems");
                    groupitems.each(function () {
                        var self = $(this);
                        var allcheckbox = self.find("input");
                        
                        allcheckbox.each(function () {
                            var _self = $(this);
                            if (_self.is(":checked")) {
                                var val = _self.parent().attr("val");
                                copytext += val + "\n";
                            }
                        });
                    });
                    return copytext;
                }
            });
        }

        function expandselected(isexpand) {
            var grouptr = $("tr.grouptr");
            grouptr.hide();
            if (isexpand) {
               
                grouptr.each(function () {
                    var self = $(this);
                    var groupitems = self.find("table.groupitems");
                    var allcheckbox = groupitems.find("input");
                    var anychecked = false;
                    allcheckbox.each(function () {
                        var _self = $(this);
                        if (_self.is(":checked")) {
                            anychecked = true;
                            return false;
                        }
                    });
                    if (anychecked) {
                        self.show();
                    }
                });
            } 
        }

        function clearSelected() {
            $("#form1").find(":checked").removeAttr("checked");
            $("#Button1").click();
        }

        function _search(keyword,isPaste) {

            if ($.trim(keyword) == '') return;
            isPaste = isPaste || false;
            var keywordrows = keyword.split("\n");
            var alllabel = $("label");
            $(".tmphit").remove();
            $(".hited").show().removeClass("hited");
            alllabel.each(function (index) {
                var self = $(this);
                //self.removeClass("hited").show();
                var text = self.text();
               // var hideText = self.attr("hideText");
               // if (hideText) {
               //     text = hideText;
               //     self.text(text);
                // }
                
                var ishit = false,isLike=false;
                if (isPaste) {
                    for (var i = 0; i < keywordrows.length; i++) {
                        var keywordrow = keywordrows[i];
                        if (keywordrow == "") continue;
                        var keywordcols = keywordrow.split(" ");
                        ishit = keywordcols.indexOf(text) > -1;
                        
                        if (ishit) break;
                    }
                } else {
                    ishit = text == keyword;
                    isLike= text.toLowerCase().indexOf(keyword.toLowerCase()) > -1;
                }
                
                if (ishit || isLike) {
                    var temphit = $("<span class='tmphit' ></span>");
                    //temphit.html(text.replace(keyword, "<span class='hitkeyword'>" + keyword + "</span>"));
                    var hitindex = text.toLowerCase().indexOf(keyword.toLowerCase());
                    var hithtml = text.substring(0, hitindex) + "<span class='hitkeyword'>" + text.substring(hitindex,hitindex+keyword.length) + "</span>" + text.substring(hitindex + keyword.length, text.length);
                    temphit.html(hithtml);
                    self.after(temphit);
                    self.addClass("hited").hide();//.attr("hideText", text);
                   // var newtext = text.replace(keyword, "<span class='hitkeyword'>" + keyword + "</span>");
                    //  self.text(newtext);

                    var grouptr = self.parents("tr.grouptr:first");
                    if (grouptr.length == 1) {
                        grouptr.show();
                    } else {
                        self.parents("tr:first").next(".grouptr").show();
                    }

                    if (ishit) {
                        self.prev("input").attr("checked", "checked");
                    }

                }
            });
        }

        function search() {
            var keyword = $("#txtsearch").val();
            _search(keyword,false);
        }

        function SaveM2M(force) {
            force = force || false;
            if (isChange == false&&!force)
                return;
            var btn = document.getElementById('Button1');
            btn.click();
            isChange = false;
        }

        function TROver(ctrl) {
            ctrl.style.backgroundColor = 'LightSteelBlue';
        }
        function TROut(ctrl) {
            ctrl.style.backgroundColor = 'white';
        }
        function Del(id, ens) {
            if (window.confirm(' Are you sure you want to delete it ?') == false)
                return;
            var url = 'Do.aspx?DoType=DelDtl&OID=' + id + '&EnsName=' + ens;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }

        function GroupBarClick(gID) {
            var alt = document.getElementById('I' + gID).alert;
             if (alt==null) {
		            alt = 'Max';
		        }
		        var sta = 'block';
		        if (alt == 'Max') {
		            sta = 'block';
		            alt = 'Min';
		        } else {
		            sta = 'none';
		            alt = 'Max';
		        }
		        document.getElementById('I' + gID).src = '../Img/' + alt + '.gif';
		        document.getElementById('I' + gID).alert = alt;
		        document.getElementById('TR' + gID).style.display = sta;
        }

        function pagemesh(isshow){
             if(isshow){
               $("#div_loading").show();
             }else{
               $("#div_loading").hide();
             }
        }

        $(document).ready(function () {

            initSummary();
            expandselected(true);
            initcopyselected();
            
            initSearchTxt();


            pagemesh(false);
        });

    </script>
    <style type="text/css">
        .HBtn
        {
            width: 1px;
            height: 1px;
            display: none;
        }
        
        td
        {
            border: 0.5px solid #D6DDE6;
            padding: 0px;
            text-align: left;
            background-color: #FFFFFF;
            color: #333333;
            margin: 0px;
            font-size: 12px;
        }
        th, .Title
        {
            font-size: 12px;
            border: 1px solid #C2D5E3;
            text-align: center;
            color: #336699;
            white-space: nowrap;
            padding: 0px;
            background-color: InfoBackground;
        }
        .Title:hover
        {
            cursor:pointer;
        }
        .ckbgroup
        {
            cursor: default;
        }
        .ckbgroup:hover
        {
            cursor:default;
        }
    </style>
    <script language="JavaScript" src="../Comm/JScript.js"></script>
    <base target="_self" />
</head>
<body topmargin="0" leftmargin="0" onkeypress="Esc()" style="font-size: smaller">
    <form id="form1" runat="server">
     <ul  style="list-style:none; padding-left:0px">
         <li style="float:left"><asp:Button ID="Button1" runat="server" Text=" Save " CssClass="Btn" Visible="true" OnClick="Button1_Click" /></li>
         <li style="float:left"><button ID="btnClear"    onclick="clearSelected()"   >Clear Selected</button></li>
         <li style="float:left"><textarea id="txtsearch" style="width:300px"  rows="1" ></textarea></li>
         <li style="float:left"><button  type="button" onclick="search()"  >Search</button></li>
         <li style="float:left"><button id="copydynamic" type="button" >Copy Selected</button></li>
         <li style="float:left">Exopand Selected<input type="checkbox"  onclick="expandselected(this.checked)"  checked="checked"/></li>

     </ul>
     
    <uc2:Pub ID="Pub1" runat="server" />
   
    </form>
</body>
</html>
