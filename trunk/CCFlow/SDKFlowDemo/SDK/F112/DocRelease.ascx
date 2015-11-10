<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocRelease.ascx.cs" Inherits="CCFlow.SDKFlowDemo.SDK.F112.DocRelease" %>
<%@ Register src="/WF/SDKComponents/FrmCheck.ascx" tagname="FrmCheck" tagprefix="uc2" %>
<%@ Register src="/WF/SDKComponents/DocMainAth.ascx" tagname="DocMainAth" tagprefix="uc1" %>
<%@ Register src="/WF/SDKComponents/DocMultiAth.ascx" tagname="DocMultiAth" tagprefix="uc3" %>
<%@ Register src="/WF/SDKComponents/Toolbar.ascx" tagname="Toolbar" tagprefix="uc4" %>

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
            closeable: false,
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
    function ShowMsgUrl(title, url) {
        $('#Msg').dialog(
        {
            href: url,
            closeable: false,
            title: title,
            modal: true,
            width: 500,
            height: 300,
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
    function LoadFrm() {
        GetParas();
        $('#form1').form({
            url: "/SDKFlowDemo/SDK/F112/Serv.ashx?DoType=" + doType + paras,
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
                if (paras.indexOf('&'+arg[0]) == -1) {
                    paras += "&" + arg[0] + "=" + arg[1];
                }
            }
        }
    }
</script>

<!--  Process Controllers . -->
<%--<input type="Button" title=" Send " id="Btn_Send" value=" Send " onclick="Send();" /> 
<input type="Button" title=" Save " id="Btn_Save" value=" Save " onclick="Save();" />
<input type="Button" title=" Return " id="Btn_Return" value=" Return " onclick="Return();" />
<input type="Button" title=" Locus " id="Btn_Track" value=" Locus " onclick="Track();" />
<input type="Button" title=" Plus sign " id="Btn_AskForHelp" value=" Plus sign " onclick="AskForHelp();" />--%>
<%--<asp:Button ID="Btn_CC" runat="server" Text=" Cc " onclick="Btn_CC_Click" />--%>
<uc4:Toolbar ID="Toolbar1" runat="server" />
<hr>

<div class="EasyUI-Dialog" id="Msg"></div>

 <!--  Form area  -->
<table height="100%" cellspacing="1" cellpadding="0" width="800px" align="center" bgcolor="#000000"
        border="0">
        <tbody>
<tr>
 <td valign="top" bgcolor="#ffffff" >
<table width="800px" style="font-size:15px" cellpadding="0" align="center" border="0">
    <tr>
        <td colspan="4" align="center">
        <h1 style="text-align:center;font-color:red"> Office of Shenzhen Municipal People Congress Wen Cheng Pibiao </h1>
        </td>
    </tr>
    <tr>
        <td >
             Urgency :
        </td>
        <td>
            <asp:DropDownList ID="DDL_JinJiChengDu" runat="server" class="easyui-combobox">
                <asp:ListItem Value="1"> Urgent </asp:ListItem>
                <asp:ListItem Value="2"> General </asp:ListItem>
                <asp:ListItem Value="3"> Not Urgent </asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
             Dense :
        </td>
        <td>
            <asp:DropDownList ID="DDL_MiMiDengJi" runat="server" class="easyui-combobox">
                <asp:ListItem Value="1"> Secret </asp:ListItem>
                <asp:ListItem Value="2"> Secrecy </asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td >
             Communications Unit :
        </td>
        <td colspan="3">
            <asp:DropDownList ID="DDL_Port_DeptType" runat="server" Width="210px" class="easyui-combobox">
                <asp:ListItem Value="23">2</asp:ListItem>
                <asp:ListItem Value="232">323</asp:ListItem>
                <asp:ListItem Value="23">2323</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="DDL_Port_OutDept" runat="server" Width="210px" class="easyui-combobox" data-options="valueField:'No',textField:'Name'">
                <asp:ListItem Value="23">3332</asp:ListItem>
                <asp:ListItem Value="34">434</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td >
             Number to text :
        </td>
        <td>
            <asp:TextBox ID="TB_LaiWenZiHao" runat="server" class="easyui-validatebox"></asp:TextBox>
        </td>
        <td>
             Read organizer :
        </td>
        <td>
            <asp:DropDownList ID="DDL_FK_LEADER" runat="server" class="easyui-combobox" Width="200px">
                <asp:ListItem>3232</asp:ListItem>
                <asp:ListItem Value="4">444</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td >
             Document Title :
        </td>
        <td colspan="3">
            <asp:TextBox ID="TB_WenJianBiaoTi" runat="server" Width="627px" class="easyui-validatebox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td >
             Date of receipt :
        </td>
        <td>
            <asp:TextBox ID="TB_ShouWenRiQi" runat="server" Width="199px" class="easyui-datebox textbox"></asp:TextBox>
        </td>
        <td>
             Office of the text number :
        </td>
        <td>
            <asp:TextBox ID="TB_BanWenBianHao" runat="server" class="easyui-validatebox"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td >
             Text :
        </td>
        <td colspan="3">
            <uc1:DocMainAth ID="DocMainAth1" runat="server" />
        </td>
    </tr>
    <tr>
        <td >
             Accessory :
        </td>
        <td colspan="3">
            <uc3:DocMultiAth ID="DocMultiAth1" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <a class="btn blue" onclick="openWin(620,600,callBack);" data-toggle="modal" href="#"> Handle comments </a>
        </td>
    </tr>
    <tr>
        <td > Summary :</td>
        <td colspan="3">
            <asp:TextBox ID="TB_nrzynbyj" runat="server" Height="81px" class="easyui-validatebox"
                TextMode="MultiLine" Width="675px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="4">
            <uc2:FrmCheck ID="FrmCheck1" runat="server" />
        </td>
    </tr>
</table>


                    
                </td>
            </tr>
        </tbody>
    </table>