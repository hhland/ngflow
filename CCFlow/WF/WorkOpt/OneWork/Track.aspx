<%@ Page Title=" Process Logs " Language="C#" MasterPageFile="OneWork.master" AutoEventWireup="true"
    CodeBehind="Track.aspx.cs" Inherits="CCFlow.WF.WorkOpt.OneWork.TruckUI" %>

<%@ Register Src="TruakUC.ascx" TagName="TruakUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("table.Table tr:gt(0)").hover(
                function () { $(this).addClass("tr_hover"); },
                function () { $(this).removeClass("tr_hover"); });
        });

        function WinOpen(url, winName) {
            var newWindow = window.open(url, winName, 'height=800,width=1030,top=' + (window.screen.availHeight - 800) / 2 + ',left=' + (window.screen.availWidth - 1030) / 2 + ',scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
            newWindow.focus();
            return;
        }

        function showEmpWindow(depemps) {
            //
            var url = "DeptEmpInfoList.ashx";
            var arrdepems = depemps.split("<br>");
            var p_depmes = "";
            for (var i = 0; i < arrdepems.length; i++) {
                var depemp = $.trim(arrdepems[i]);
                if (depemp == "") continue;
                p_depmes += depemp + "@";
            }
            $("#dd").load(url, { depemps: p_depmes, pformat: "depemps" }, function (str) {   
                $("#win_emp").window("open");
            });
        }

        $(document).ready(function () {

            $("TD[nowrap]").each(function (index) {

                var text = $(this).text();
                var html = $(this).html();
                var maxlen = 50;
                var bri = html.indexOf("<br>");
                if ($(this).find("a").size() > 0) { }
                else if (bri >= 0) {
                    textsub = html.substring(0, bri);
                    var depemps = html.substring(bri);
                    var _html = "<a  title='" + text + "' href=\"javascript:showEmpWindow('"+depemps+"');\" >" + textsub + "...<" + "/a>";
                    $(this).html(_html);
                }
                else if (text != null && text.length > maxlen) {

                    var textsub = text.substring(0, maxlen);
                    
                    
                    var _html = "<span title='" + text + "' >" + textsub + "...<" + "/span>";
                    $(this).html(_html);
                }
            });

        });

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout" data-options="fit:true">
        <div data-options="region:'center',title:'<%=this.Title %>'" style="padding: 5px">
            <uc1:TruakUC ID="TruakUC1" runat="server" />
        </div>
    </div>
    
    <div id="win_emp" class="easyui-window" title="Emp info" style="width:600px;height:400px"
        data-options="iconCls:'icon-search',modal:true,closed:true">
       <table style='width:100%'>
            <thead><tr>
                  <th style='width:30%'>Department Name</th>
                  <th style='width:20%'>Name</th>
                  <th style='width:20%'>Email</th>
                  <th>Tel</th>
                  </tr>
              </thead>
                  <tbody id="dd"></tbody>
        </table>
               
    </div>
</asp:Content>
