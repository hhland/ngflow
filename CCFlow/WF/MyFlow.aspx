<%@ Page Language="C#" MasterPageFile="WinOpenEUI.master" AutoEventWireup="true"
    Inherits="CCFlow.WF.WF_MyFlowSmall" Title=" Working deal " CodeBehind="MyFlow.aspx.cs" %>

<%@ Register Src="UC/MyFlowUC.ascx" TagName="MyFlowUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Comm/JScript.js" type="text/javascript"></script>
    <script src="CCForm/MapExt.js" type="text/javascript"></script>
    <script src="Style/Frm/jquery.idTabs.min.js" type="text/javascript"></script>
    <script src="Comm/JS/Calendar/WdatePicker.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
    <link href="Scripts/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OpenOfiice(fk_ath, pkVal, delPKVal, FK_MapData, NoOfObj, FK_Node) {
            var date = new Date();
            var t = date.getFullYear() + "" + date.getMonth() + "" + date.getDay() + "" + date.getHours() + "" + date.getMinutes() + "" + date.getSeconds();

            var url = 'WebOffice/AttachOffice.aspx?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal + "&FK_MapData=" + FK_MapData + "&NoOfObj=" + NoOfObj + "&FK_Node=" + FK_Node + "&T=" + t;
            //var url = 'WebOffice.aspx?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal;
            // var str = window.showModalDialog(url, '', 'dialogHeight: 1250px; dialogWidth:900px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
            //var str = window.open(url, '', 'dialogHeight: 1200px; dialogWidth:1110px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
            window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:MyFlowUC ID="MyFlowUC1" runat="server" />
</asp:Content>
