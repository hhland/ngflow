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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout" data-options="fit:true">
        <div data-options="region:'center',title:'<%=this.Title %>'" style="padding: 5px">
            <uc1:TruakUC ID="TruakUC1" runat="server" />
        </div>
    </div>
</asp:Content>
