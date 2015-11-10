<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.Comm_MapDef_MapDtlDe"
    CodeBehind="MapDtlDe.aspx.cs" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Form Design </title>
    <script language="JavaScript" src="../Comm/JScript.js"></script>
    <script language="JavaScript" src="../Comm/JS/Calendar/WdatePicker.js" defer="defer"></script>
    <base target="_self" />
    <script language="javascript">
        function Insert(mypk, IDX) {
            var url = 'Do.aspx?DoType=AddF&MyPK=' + mypk + '&IDX=' + IDX;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function AddF(mypk) {
            var url = 'Do.aspx?DoType=AddF&MyPK=' + mypk;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function AddFGroup(mypk) {
            var url = 'Do.aspx?DoType=AddFGroup&FK_MapData=' + mypk;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function CopyF(mypk) {
            var url = 'CopyDtlField.aspx?MyPK=' + mypk;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 600px; dialogWidth: 800px;center: yes; help: no');
            window.location.href = window.location.href;
        }

        function HidAttr(mypk) {
            var url = 'HidAttr.aspx?FK_MapData=' + mypk;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 600px; dialogWidth: 800px;center: yes; help: no');
            //  window.location.href = window.location.href;
        }
        function Edit(mypk, refNo, ftype) {
            var url = 'EditF.aspx?DoType=Edit&MyPK=' + mypk + '&RefNo=' + refNo + '&FType=' + ftype;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 960px; dialogWidth: 1024px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function EditEnum(mypk, refNo) {
            var url = 'EditEnum.aspx?DoType=Edit&MyPK=' + mypk + '&RefNo=' + refNo;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function EditTable(mypk, refno) {
            var url = 'EditTable.aspx?DoType=Edit&MyPK=' + mypk + '&RefNo=' + refno;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }

        function Up(mypk, refNo) {
            var url = 'Do.aspx?DoType=Up&MyPK=' + mypk + '&RefNo=' + refNo + "&IsDtl=1";
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            //window.location.href ='MapDef.aspx?PK='+mypk+'&IsOpen=1';
            window.location.href = window.location.href;
        }
        function Down(mypk, refNo) {
            var url = 'Do.aspx?DoType=Down&MyPK=' + mypk + '&RefNo=' + refNo + "&IsDtl=1";
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function Del(mypk, refNo) {
            if (window.confirm(' Are you sure you want to delete it ?') == false)
                return;

            var url = 'Do.aspx?DoType=Del&MyPK=' + mypk + '&RefNo=' + refNo;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function DtlMTR(MyPK) {
            var url = 'MapDtlMTR.aspx?MyPK=' + MyPK;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 350px; dialogWidth: 550px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function Esc() {
            if (event.keyCode == 27)
                window.close();
            return true;
        }
        function Attachment(fk_mapdtl) {
            window.showModalDialog('Attachment.aspx?IsBTitle=1&PKVal=0&FK_MapData=' + fk_mapdtl + '&FK_FrmAttachment=' + fk_mapdtl + '_AthM&Ath=AthM');
        }
        function MapM2M(fk_mapdtl) {
            window.showModalDialog('MapM2M.aspx?NoOfObj=M2M&PKVal=0&FK_MapData=' + fk_mapdtl + '&FK_FrmAttachment=' + fk_mapdtl + '_AthM&Ath=AthM');
        }

        function EnableAthM(fk_MapDtl) { 
            var url = '../CCForm/AttachmentUpload.aspx?IsBTitle=1&PKVal=0&Ath=AthM&FK_MapData=' + fk_MapDtl + '&FK_FrmAttachment=' + fk_MapDtl + '_AthM';
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
            //  window.location.href = window.location.href;
        }
    </script>
    <script language="javascript" for="document" event="onkeydown" type="text/javascript">
//    if(event.keyCode==13)
//       event.keyCode=9;
    </script>
    <script language="javascript" type="text/javascript">
        // row Primary key information  .
        var rowPK = null;
        // ccform  Provides developers with built-in functions .
        //  Get DDL值.
        function ReqDDL(ddlID) {
            var v = document.getElementById('Pub1_DDL_' + ddlID + "_" + rowPK).value;
            if (v == null) {
                alert(' Not found ID=' + ddlID + ' The drop-down box control .');
            }
            return v;
        }
        //  Get TB值
        function ReqTB(tbID) {
            var v = document.getElementById('Pub1_TB_' + tbID + "_" + rowPK).value;
            if (v == null) {
                alert(' Not found ID=' + tbID + ' The text box control .');
            }
            return v;
        }
        //  Get CheckBox值
        function ReqCB(cbID) {
            var v = document.getElementById('Pub1_CB_' + cbID + "_" + rowPK).value;
            if (v == null) {
                alert(' Not found ID=' + cbID + ' Radio controls .');
            }
            return v;
        }

        ///  Get DDL Obj
        function ReqDDLObj(ddlID) {
            var v = document.getElementById('Pub1_DDL_' + ddlID + "_" + rowPK);
            if (v == null) {
                alert(' Not found ID=' + ddlID + ' The drop-down box control .');
            }
            return v;
        }
        //  Get TB Obj
        function ReqTBObj(tbID) {
            var v = document.getElementById('Pub1_TB_' + tbID + "_" + rowPK);
            if (v == null) {
                alert(' Not found ID=' + tbID + ' The text box control .');
            }
            return v;
        }
        //  Get CheckBox Obj值
        function ReqCBObj(cbID) {
            var v = document.getElementById('Pub1_CB_' + cbID + "_" + rowPK);
            if (v == null) {
                alert(' Not found ID=' + cbID + ' Radio controls .');
            }
            return v;
        }

        //  Set the control value .
        function SetCtrlVal(ctrlID, val) {
            document.getElementById('Pub1_TB_' + ctrlID + "_" + rowPK).value = val;
            document.getElementById('Pub1_DDL_' + ctrlID + "_" + rowPK).value = val;
            document.getElementById('Pub1_CB_' + ctrlID + "_" + rowPK).value = val;
        }
    </script>
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
</head>
<body class="easyui-layout" onkeypress="Esc()">
    <form id="form1" runat="server">
    <div data-options="region:'center',noheader:true">
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
