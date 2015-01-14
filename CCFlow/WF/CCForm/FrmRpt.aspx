<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.Comm_FrmRpt" Codebehind="FrmRpt.aspx.cs" %>
<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title></title>
		<Meta http-equiv="Page-Enter" Content="revealTrans(duration=0.5, transition=8)" />
		<script language="javascript"  type="text/javascript" >
		    var isChange = false;
		    function SaveDtlData() {
		        if (isChange == false)
		            return;
		        var btn = document.getElementById('Button1');
		        btn.click();
		        isChange = false;
		    }
		    function SaveDtlDataTo(url) {

		        if (isChange == true) {
		            alert(' Please execute the save , Exit .');
		            return true;
		        }
		        //SaveDtlData();
		        window.location.href = url;
		    }

function TROver(ctrl)
{
   ctrl.style.backgroundColor='LightSteelBlue';
}

function TROut(ctrl)
{
  ctrl.style.backgroundColor='white';
}

function Del(id, ens, refPk, pageIdx) {
    if (window.confirm(' Are you sure you want to delete it ?') == false)
        return;

    var url = 'Do.aspx?DoType=DelDtl&OID=' + id + '&EnsName=' + ens;
    var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
    window.location.href = 'Dtl.aspx?EnsName=' + ens + '&RefPKVal=' + refPk + '&PageIdx=' + pageIdx;
}

function DtlOpt(workId, fk_mapdtl) {
    var url = 'DtlOpt.aspx?WorkID=' + workId + '&FK_MapDtl=' + fk_mapdtl;
    var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
    window.location.href = 'Dtl.aspx?EnsName=' + fk_mapdtl + '&RefPKVal=' + workId;
}
    </script>
    <style type="text/css">
        .HBtn
        {
        	 width:1px;
        	 height:1px;
        	 display:none;
        }
    </style>
	<script language="JavaScript" src="../Comm/JScript.js"></script>
   <script language="JavaScript" src="../Comm/JS/Calendar/WdatePicker.js"  defer="defer" ></script>
   <script type="text/javascript">
       function SetVal() {
           // document.getElementById('KVs').value = this.GenerPageKVs();
           //  kvs = this.GenerPageKVs();
       }
   </script>

   <script language="javascript" type="text/javascript" >
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
<body onkeypress="Esc()" style="font-size:smaller"  onblur="SetVal();" onload="SetVal();"   topmargin="0" leftmargin="0" > 
    <form id="form1" runat="server">
     <asp:Button ID="Button1" runat="server" Text=""  CssClass="HBtn" Visible="true"
         onclick="Button1_Click" />
     <uc2:Pub ID="Pub1" runat="server" />
     <uc2:Pub ID="Pub2" runat="server" />
    </form>
</body>
</html>