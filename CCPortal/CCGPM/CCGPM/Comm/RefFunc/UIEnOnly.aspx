<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UIEnOnly.aspx.cs" Inherits="CCOA.Comm.RefFunc.UIEnOnly" %>
<%@ Register src="../UC/UIEn.ascx" tagname="UIEn" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <script language="javascript" type="text/javascript">
           function SelectAll(cb_selectAll) {
               var arrObj = document.all;
               if (cb_selectAll.checked) {
                   for (var i = 0; i < arrObj.length; i++) {
                       if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
                           arrObj[i].checked = true;
                       }
                   }
               } else {
                   for (var i = 0; i < arrObj.length; i++) {
                       if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox')
                           arrObj[i].checked = false;
                   }
               }
           }
     </script>
     <script language="JavaScript" src="../JScript.js"></script>
        <script language="JavaScript" src="../JS/Calendar/WdatePicker.js" defer="defer"></script>
		<script language="JavaScript" src="../ActiveX.js"></script>
		<script language="JavaScript" src="../Menu.js"></script>
		<script language="JavaScript" src="../ShortKey.js"></script>
		<script language="javascript">
		    function ShowEn(url, wName, h, w) {
		        var s = "dialogWidth=" + parseInt(w) + "px;dialogHeight=" + parseInt(h) + "px;resizable:yes";
		        var val = window.showModalDialog(url, null, s);
		        window.location.href = window.location.href;
		    }
		    function ImgClick() {
		    }
		    function OpenAttrs(ensName) {
		        var url = '../Sys/EnsAppCfg.aspx?EnsName=' + ensName;
		        var s = 'dialogWidth=680px;dialogHeight=480px;status:no;center:1;resizable:yes'.toString();
		        val = window.showModalDialog(url, null, s);
		        window.location.href = window.location.href;
		    }
		</script>
		<base target=_self />
    <link href="../Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../Style/Table0.css" rel="stylesheet" type="text/css" />
</head>
<body leftMargin=0 topMargin=0 onkeypress="javascript:Esc();">
    <form id="form1" runat="server">
    <div style=" padding-left:0px">
    <uc1:UIEn ID="UIEn1" runat="server" />
    </div>
    </form>
</body>
</html>
