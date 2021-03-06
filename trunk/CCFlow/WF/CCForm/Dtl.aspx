﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.Comm_Dtl"
    CodeBehind="Dtl.aspx.cs" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
 
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />  
    <script src="../Comm/JS/Calendar/WdatePicker.js" type="text/javascript"></script>
    <meta http-equiv="Page-Enter" content="revealTrans(duration=0.5, transition=8)" />
    <link href="../Style/FormThemes/Table0.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript">

        var EnsName="<% =Request.Params["EnsName"] %>";
        var isChange = false;

        
function updateDtl(oid, fieldname, fieldval) {
    var url = "/WF/CCForm/Dtl.ashx?action=updateDtl";
    var param = { oid: oid, fieldname: fieldname, fieldval: fieldval, EnsName: EnsName };
    
    $.post(url, param, function (str) {
        var json = eval("(" + str + ")");
        var eff = json["eff"];
        if (eff != 1) {
            var errormsg = json["errormsg"];
            alert(errormsg);
        }
    });
}

        function bindRowEvent() {
            $("tr.dtlrow").each(function (index) {
                var _self = $(this);
                var oid = _self.attr("oid");
                _self.find("input").each(function (i) { 
                     var __self=$(this);
                     var id=__self.attr("id")||"";
                     var tempid=id.substring("Pub1_".length+1);
                     var si=tempid.indexOf("_"),ei=tempid.lastIndexOf("_");
                     var fieldname=tempid.substring(si+1,ei);
                     
                     fieldname=fieldname.substring();
                     //alert(fieldname);
                     __self.change(function(){
                         var fieldval=$("#"+id).val();
                         updateDtl(oid,fieldname,fieldval);
                         //isChange=true;
                     });
                });
            });
            
        }

        function SaveDtlData() {
            if (isChange == false)
                return;
            var btn = document.getElementById('Button1');
            btn.click();
            isChange = fals0
        }
        function SaveDtlDataTo(url) {

            if (isChange == true) {
                alert(' Please execute the save , Exit .');
                return true;
            }
            //SaveDtlData();
            window.location.href = url;
        }

        function TROver(ctrl) {
            ctrl.style.backgroundColor = 'LightSteelBlue';
        }

        function TROut(ctrl) {
            ctrl.style.backgroundColor = 'white';
        }

        function Del(id, ens, refPk, pageIdx) {
            if (window.confirm(' Are you sure you want to delete it ?') == false)
                return;

            var url = 'Do.aspx?DoType=DelDtl&OID=' + id + '&EnsName=' + ens;
            //		        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');

            $.post(url, null, function () {

                window.location.href = 'Dtl.aspx?EnsName=' + ens + '&RefPKVal=' + refPk + '&PageIdx=' + pageIdx;

            }
		        );
        }

        function DtlOpt(workId, fk_mapdtl) {
            var url = 'DtlOpt.aspx?WorkID=' + workId + '&FK_MapDtl=' + fk_mapdtl;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = 'Dtl.aspx?EnsName=' + fk_mapdtl + '&RefPKVal=' + workId;
        }


        $(document).ready(function () {

            bindRowEvent();

        });

    </script>
   <%-- <style type="text/css">
        .HBtn
        {
            width: 1px;
            height: 1px;
            display: none;
        }
    </style>--%>
    <script language="JavaScript" src="../Comm/JScript.js"></script>
    <script language="JavaScript" src="../Comm/JS/Calendar/WdatePicker.js" defer="defer"></script>
    <script src="MapExt.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SetVal() {
            // document.getElementById('KVs').value = this.GenerPageKVs();
            //  kvs = this.GenerPageKVs();
        }
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
</head>
<body onkeypress="Esc()" style="font-size: smaller;" class="easyui-layout" onblur="SetVal();"
    onload="SetVal();" topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <div id="mainPanle" region="center" border="false" style="position: fixed">
        <asp:Button ID="Button1" runat="server" Text="" CssClass="HBtn" Visible="true" OnClick="Button1_Click" />
        <uc2:Pub ID="Pub1" runat="server" />
        <uc2:Pub ID="Pub2" runat="server" />
    </div>
    </form>
</body>
</html>
