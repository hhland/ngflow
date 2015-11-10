<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchStart.ascx.cs" Inherits="CCFlow.WF.UC.BatchStart" %>
<script language="JavaScript" src="./../Comm/JScript.js" type="text/javascript" ></script>
<script language="JavaScript" src="./../Comm/JS/Calendar/WdatePicker.js" defer="defer" type="text/javascript" ></script>
<script language="javascript" type="text/javascript" >
　　    function SelectAllBS(ctrl) {
        var arrObj = document.all;
        if (ctrl.checked) {
            for (var i = 0; i < arrObj.length; i++) {
                if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
                    if (arrObj[i].name.indexOf('IDX_') > 0)
                        arrObj[i].checked = true;
                }
            }
        } else {
            for (var i = 0; i < arrObj.length; i++) {
                if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
                    if (arrObj[i].name.indexOf('IDX_') > 0)
                        arrObj[i].checked = false;
                }
            }
        }
    }
</script>