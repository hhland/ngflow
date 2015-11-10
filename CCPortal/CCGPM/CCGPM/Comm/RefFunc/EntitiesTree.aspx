<%@ Page Title="" Language="C#" MasterPageFile="~/Comm/WinOpen.master" AutoEventWireup="true" Inherits="Comm_RefFunc_EntitiesTree" Codebehind="EntitiesTree.aspx.cs" %>
<%@ Register src="../UC/Pub.ascx" tagname="Pub" tagprefix="uc2" %>
<%@ Register src="../UC/UIEn.ascx" tagname="UIEn" tagprefix="uc3" %>
<%@ Register src="../RefFunc/Left.ascx" tagname="Left" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Style/Table0.css" rel="stylesheet" type="text/css" />
    <script language="JavaScript" src="../JScript.js"></script>
    <script language="javascript">
        function ShowEn(url, wName, h, w) {
            var s = "dialogWidth=" + parseInt(w) + "px;dialogHeight=" + parseInt(h) + "px;resizable:yes";
            var val = window.showModalDialog(url, null, s);
            window.location.href = window.location.href;
        }
        function ImgClick() {
        }
		function OpenAttrs(ensName)
		{
	       var url= './Sys/EnsAppCfg.aspx?EnsName='+ensName;
           var s =  'dialogWidth=680px;dialogHeight=480px;status:no;center:1;resizable:yes'.toString() ;
		   val=window.showModalDialog( url , null ,  s);
           window.location.href=window.location.href;
       }
       function DDL_mvals_OnChange(ctrl, ensName, attrKey) {
           var idx_Old = ctrl.selectedIndex;
           if (ctrl.options[ctrl.selectedIndex].value != 'mvals')
               return;
           if (attrKey == null)
               return;

           var url = 'SelectMVals.aspx?EnsName=' + ensName + '&AttrKey=' + attrKey;
           var val = window.showModalDialog(url, 'dg', 'dialogHeight: 450px; dialogWidth: 450px; center: yes; help: no');
           if (val == '' || val == null) {
               ctrl.selectedIndex = 0;
           }
       }

       function GroupBarClick(rowIdx) {
           var alt = document.getElementById('Img' + rowIdx).alert;
           var sta = 'block';
           if (alt == 'Max') {
               sta = 'block';
               alt = 'Min';
           } else {
               sta = 'none';
               alt = 'Max';
           }
           document.getElementById('Img' + rowIdx).src = './Img/' + alt + '.gif';
           document.getElementById('Img' + rowIdx).alert = alt;
           var i = 0
           for (i = 0; i <= 40; i++) {
               if (document.getElementById(rowIdx + '_' + i) == null)
                   continue;
               document.getElementById(rowIdx + '_' + i).style.display = sta;
           }
       }
</script>
 <style type="text/css">
        .style1
        {
            width: 100px;
            height:900px;
        }
    </style>
   <script type="text/javascript">

       var pk = this.request('PK');
       function AddSameLevelNode() {
           var url = "EntitiesTree.aspx?EnsName=" + this.request('EnsName') + "&EnName=" + this.request('EnName') + "&PK=" + pk + '&DoType=AddSameLevelNode';
           window.document.location = url;
       }
       function AddSubNode() {
           alert(pk);
           var url = "EntitiesTree.aspx?EnsName=" + this.request('EnsName') + "&EnName=" + this.request('EnName') + "&PK=" + pk + '&DoType=AddSubNode';
           window.document.location = url;
       }
       function DoUp() {
           var url = "EntitiesTree.aspx?EnsName=" + this.request('EnsName') + "&EnName=" + this.request('EnName') + "&PK=" + pk + '&DoType=DoUp';
           window.document.location = url;
       }
       function DoDown() {
           var url = "EntitiesTree.aspx?EnsName=" + this.request('EnsName') + "&EnName=" + this.request('EnName') + "&PK=" + pk + '&DoType=DoDown';
           window.document.location = url;
       }

       function request(paras) {
           var url = location.href;
           var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
           var paraObj = {}
           for (i = 0; j = paraString[i]; i++) {
               paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
           }
           var returnValue = paraObj[paras.toLowerCase()];
           if (typeof (returnValue) == "undefined") {
               return "";
           } else {
               return returnValue;
           }
       }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <table border="1px" width='100%' >
<tr>
<td>
[<a href="javascript:AddSameLevelNode();" >+同级部门</a>][<a href="javascript:AddSubNode()" >+下级部门</a>]
[<a href="javascript:DoUp();" >上移</a>][<a href="javascript:DoDown();" >下移</a>]</td>
</tr>
<tr>
<td valign="top">
    <uc2:Pub ID="Pub1" runat="server" />
</td>
</tr>
</table>
</asp:Content>

