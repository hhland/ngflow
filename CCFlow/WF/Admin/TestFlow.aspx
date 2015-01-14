<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.Admin.WF_Admin_TestFlow"
    CodeBehind="TestFlow.aspx.cs" %>

<%@ Register Src="../Comm/UC/ucsys.ascx" TagName="ucsys" TagPrefix="uc2" %>
<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<script type="text/javascript" src="../Comm/JScript.js" />

    <script type="text/javascript" language="javascript">
        function Del(mypk, fk_flow, refoid) {
            if (window.confirm('Are you sure?') == false)
                return;

            var url = 'Do.aspx?DoType=Del&MyPK=' + mypk + '&RefOID=' + refoid;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function WinOpen(url) {
            var b = window.open(url, 'ass', 'width=700,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false');
            b.focus();
        }
        function WinOpen(url, w, h, name) {
            var b = window.open(url, name, 'width=' + w + ',height=' + h + ',scrollbars=yes,resizable=yes,toolbar=false,location=false,center: yes');
        }
        function WinOpenWAP_Cross(url) {
            var b = window.open(url, 'ass', 'width=50,top=50,left=50,height=20,scrollbars=yes,resizable=yes,toolbar=false,location=false');
        }
    </script>
    <script language="javascript" >
        function ShowIt(m) {
            var url = '../Comm/Method.aspx?M=' + m;
            var a = window.showModalDialog(url, 'OneVs', 'dialogHeight: 400px; dialogWidth: 500px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no');
        }
        function Open(no) {
            if (window.confirm(' Are you sure you want this process numbered :'+no+' The data do ?') == false)
                return;
            var url = '../Comm/RefMethod.aspx?Index=3&EnsName=BP.WF.Template.Ext.FlowSheets&No='+no;
            var a = window.showModalDialog(url, 'OneVs', 'dialogHeight: 400px; dialogWidth: 500px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no');
        }

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
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />

</head>
<body leftmargin="0" topmargin="0" bgcolor="white">
    <form id="form1" runat="server">
    <uc2:ucsys ID="Ucsys1" runat="server" />
    <br>
    <a href="javascript:ShowIt('BP.WF.DTS.ClearDB');"  >
    <img src='../Img/Btn/Delete.gif'  border=0 /> Clear all processes running data ( This function is to run in a test environment )</a>
    <br><font size=2 color=Green> Clear all processes running data , Including the work to be done .</font>
    <br>
    <%  string flowNo = this.Request.QueryString["FK_Flow"];  %>
     <a href="javascript:Open('<%=flowNo %>');"  >
    <img src='../Img/Btn/Delete.gif'  border=0 /> Delete this process data ( This function is to run in a test environment )</a>
    <br><font size=2 color=Green> Clear this process to run data , Including the work to be done .</font><br>
    </form>
    <p>
</body>
</html>
