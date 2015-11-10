<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" Inherits="CCFlow.WF.Comm.RefFunc.Dot2Dot" Title="" Codebehind="Dot2Dot.aspx.cs" %>
<%@ Register src="Dot2Dot.ascx" tagname="Dot2Dot" tagprefix="uc1" %>
<%@ Register src="RefLeft.ascx" tagname="RefLeft" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <base target=_self />

    <style>
         .autocomplete-suggestions { border: 1px solid #999; background: #FFF; cursor: default; overflow: auto; -webkit-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); -moz-box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); box-shadow: 1px 4px 3px rgba(50, 50, 50, 0.64); }
.autocomplete-suggestion { padding: 2px 5px; white-space: nowrap; overflow: hidden; }
.autocomplete-no-suggestion { padding: 2px 5px;}
.autocomplete-selected { background: #F0F0F0; }
.autocomplete-suggestions strong { font-weight: bold; color: #000; }
.autocomplete-group { padding: 2px 5px; }
.autocomplete-group strong { font-weight: bold; font-size: 16px; color: #000; display: block; border-bottom: 1px solid #000; }
    </style>

    <script type="text/javascript" src="/WF/Scripts/jquery.autocomplete.min.js"></script>
    <script type="text/javascript">
        var isChange = false, searchitems = [];
        function initSearchTxt() {

            var elsearch = $("<input id='txtsearch' style='width:200px'  />");
            var btnsearch = $("<button type='button' onclick='search();' >search</button>");
            $("#toolbar").append(elsearch).append(btnsearch);
            


            $("#content").find("label").each(function (index) {
                var text = $(this).text();
                searchitems.push({ data: text, value: text });
            });

            elsearch.autocomplete({
                lookup: searchitems
                , triggerSelectOnValidInput: false
                , onSelect: function (suggestion) { _search(suggestion["data"], false); }
            });

        }


        function _search(keyword, isPaste) {

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

                var ishit = false, isLike = false;
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
                    isLike = text.toLowerCase().indexOf(keyword.toLowerCase()) > -1;
                }

                if (ishit || isLike) {
                    

                    var grouptd = self.parents("td:first");
                    if (grouptd.hasClass("GroupTitle")) {
                        var nexttr=grouptd.parents("tr:first").next("tr");
                        var nexttd = nexttr.find("td:eq(0)");
                        while (!nexttd.hasClass("GroupTitle")) {
                            nexttr.find("input").attr("checked", "checked");
                            nexttr = nexttr.next("tr");
                            nexttd = nexttr.find("td:eq(0)");
                        }
                    } 

                    if (ishit) {
                        self.prev("input").attr("checked", "checked");
                    }

                }
            });
        }

        function search() {
            var keyword = $("#txtsearch").val();
            _search(keyword, false);
        }

        $(document).ready(function () {

            initSearchTxt();
        });

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc2:RefLeft ID="RefLeft1" runat="server" />
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <uc1:Dot2Dot ID="Dot2Dot1" runat="server" />
</asp:Content>