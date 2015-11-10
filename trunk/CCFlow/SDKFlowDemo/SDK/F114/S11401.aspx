<%@ Page Title=" Split point " Language="C#" MasterPageFile="~/SDKFlowDemo/SDK/Site.Master" AutoEventWireup="true" CodeBehind="S11401.aspx.cs" Inherits="CCFlow.SDKFlowDemo.SDK.F114.S11401" %>
<%@ Register src="../../../WF/SDKComponents/DocMainAth.ascx" tagname="DocMainAth" tagprefix="uc1" %>
<%@ Register src="../../../WF/SDKComponents/DocMultiAth.ascx" tagname="DocMultiAth" tagprefix="uc2" %>
<%@ Register src="../../../WF/SDKComponents/Toolbar.ascx" tagname="Toolbar" tagprefix="uc3" %>
<%@ Register src="../../../WF/SDKComponents/FrmCheck.ascx" tagname="FrmCheck" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<!--  Load EUI Required document . -->
<script src="/WF/Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
<script src="/WF/Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
<script src="/WF/Scripts/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
<link href="/WF/Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
<link href="/WF/Scripts/easyUI/themes/default/easyui.css" rel="stylesheet"  type="text/css" />

<script type="text/javascript">
  //  Lock all controls .
    var doType = "";
    var paras = "";
    function LockPage() {
        if (doType != "Send")
            return;
        var arrObj = document.all;
        for (var i = 0; i < arrObj.length; i++) {
            if (typeof arrObj[i].type == "undefined")
                continue;

            if (arrObj[i].type == "button")
                arrObj[i].disabled = 'disabled';

            if (arrObj[i].type == "text")
                arrObj[i].disabled = 'disabled';
        }
    }
    function ShowMsg(title, msg) {
        $('#Msg').dialog(
        {
            title: title,
            modal: true,
            width: 500,
            height: 300,
            content: msg,
            buttons: [{ text: ' Shut down ', handler: function () {
                $('#Msg').dialog("close");
            }
            }]
        }
        );
    }
   
    function Send() {
        if (confirm(" Are you sure you want to send it ?") == false)
            return;
        doType = "Send";
        LoadFrm();
        // Form submission .
        $('#form1').submit();
        LockPage();
    }
    function Save() {
        doType = "Save";
        LoadFrm();
        // Form submission .
        $('#form1').submit();
    }

    //  Use form Submit Data .
    function LoadFrm() {
        GetParas();
        $('#form1').form({
            url: "/SDKFlowDemo/SDK/F114/Serv.ashx?DoType=" + doType + paras,
            data: paras,
            onSubmit: function (param) {
            },
            success: function (data) {
                ShowMsg(' Sent successfully ', data);
            }
        });
    }
    function GetParas() {
        paras = "";
        // Get other parameters 
        var sHref = window.location.href;
        var args = sHref.split("?");
        var retval = "";
        if (args[0] != sHref) /* Parameter is not empty */
        {
            var str = args[1];
            args = str.split("&");
            for (var i = 0; i < args.length; i++) {
                str = args[i];
                var arg = str.split("=");
                if (arg.length <= 1)
                    continue;
                // It does not contain added 
                if (paras.indexOf('&' + arg[0]) == -1) {
                    paras += "&" + arg[0] + "=" + arg[1];
                }
            }
        }
    }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="Msg" />

<table  height="100%" cellspacing="1" cellpadding="0" width="800px" align="center"
    border="0">
<tr>
<td>
  <uc3:Toolbar ID="Toolbar1" runat="server" />

<fieldset>
<legend> Single attachment - Package </legend>
 <uc1:DocMainAth ID="DocMainAth2" runat="server" />
</fieldset>


<fieldset>
<legend> More Accessories - Package </legend>
    <uc2:DocMultiAth ID="DocMultiAth2" runat="server" />
</fieldset>
 


<fieldset>
<legend> Audit Components </legend>
<uc5:FrmCheck ID="FrmCheck1" runat="server" />
</fieldset>

    </td>
</tr>
</table>

</asp:Content>
