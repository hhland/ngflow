<%@ Page Title=" Import field generated form " Language="C#" MasterPageFile="~/WF/MapDef/WinOpen.master"
    AutoEventWireup="true" CodeBehind="ImpTableField.aspx.cs" Inherits="CCFlow.WF.MapDef.ImpTableField" %>

<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Comm/Style/CommStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var idPrefix = '<%=this.Pub1.ClientID %>';

        function CheckAll(checked) {
            $.each($(":checkbox"), function () {
                this.checked = checked;
            });
        }

        function editLT(link) {
            var colname = link.substring('A_LogicType_'.length);
            var colLT = $('#' + idPrefix + '_LBL_LogicType_' + colname).text();
            //alert(colLT);

            $('#typeWin').dialog({ title: ' Change  ' + colname + '  Logic Type ' });
            $('.navlist').empty();
            $('ul.navlist').append("<li><div>" + (colLT == ' General ' ? "<a><span class='nav'>* General *</span></a>" : ("<a href='javascript:void(0)' onclick=\"setLT('" + colname + "',$(this).text())\"><span class='nav'> General </span></a>")) + "</div></li>");
            $('ul.navlist').append("<li><div>" + (colLT == ' Enumerate ' ? "<a><span class='nav'>* Enumerate *</span></a>" : ("<a href='javascript:void(0)' onclick=\"setLT('" + colname + "',$(this).text())\"><span class='nav'> Enumerate </span></a>")) + "</div></li>");
            $('ul.navlist').append("<li><div>" + (colLT == ' Foreign key ' ? "<a><span class='nav'>* Foreign key *</span></a>" : ("<a href='javascript:void(0)' onclick=\"setLT('" + colname + "',$(this).text())\"><span class='nav'> Foreign key </span></a>")) + "</div></li>");
            $('#typeWin').dialog('open');
        }

        function setLT(colname, colLT) {
            $('#' + idPrefix + '_LBL_LogicType_' + colname).text(colLT);
            $('#typeWin').dialog('close');
        }

        // Move 
        function up(obj, idxTBColumnIdx) {
            var objParentTR = $(obj).parent().parent();
            var prevTR = objParentTR.prev();
            var currTrId;
            var prevTrId;
            if (prevTR.length > 0 && !isNaN(prevTR.children(":eq(0)").text())) {
                prevTR.insertAfter(objParentTR);
                currTrId = Number(objParentTR.children(":eq(0)").text());
                prevTrId = Number(prevTR.children(":eq(0)").text());
                objParentTR.children(":eq(0)").text(prevTrId);
                prevTR.children(":eq(0)").text(currTrId);
                objParentTR.children(":eq(" + idxTBColumnIdx + ")").children(":first").val(prevTrId);
                prevTR.children(":eq(" + idxTBColumnIdx + ")").children(":first").val(currTrId);
            } else {
                return;
            }
        }
        // Down 
        function down(obj, idxTBColumnIdx) {
            var objParentTR = $(obj).parent().parent();
            var nextTR = objParentTR.next();
            var currTrId;
            var nextTrId;
            if (nextTR.length > 0 && !isNaN(nextTR.children(":eq(0)").text())) {
                nextTR.insertBefore(objParentTR);
                currTrId = Number(objParentTR.children(":eq(0)").text());
                nextTrId = Number(nextTR.children(":eq(0)").text());
                objParentTR.children(":eq(0)").text(nextTrId);
                nextTR.children(":eq(0)").text(currTrId);
                objParentTR.children(":eq(" + idxTBColumnIdx + ")").children(":first").val(nextTrId);
                nextTR.children(":eq(" + idxTBColumnIdx + ")").children(":first").val(currTrId);
            } else {
                return;
            }
        }

        $(document).ready(function () {
            $("#maintable tr:gt(0)").hover(
                function () { $(this).addClass("tr_hover"); },
                function () { $(this).removeClass("tr_hover"); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Pub ID="Pub1" runat="server" />
    <div id="typeWin" class="easyui-dialog" title=" Change Logic Type " style="width: 260px; height: 200px;"
        data-options="iconCls:'icon-edit',modal:true,closed:true">
        <ul class="navlist">
        </ul>
    </div>
</asp:Content>
