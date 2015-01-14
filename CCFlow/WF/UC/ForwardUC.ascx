<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.Forward_UC"
    CodeBehind="ForwardUC.ascx.cs" %>
<%@ Register Src="../Comm/UC/ToolBar.ascx" TagName="ToolBar" TagPrefix="uc1" %>
<%@ Register Src="../Pub.ascx" TagName="Pub" TagPrefix="uc2" %>
<script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
<script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
<script type="text/javascript">
    //    http: //blog.163.com/zhi_qingfang@126/blog/static/11747756320132218144825/
    function TBHelp(ctrl, enName, attrKey) {
        var explorer = window.navigator.userAgent;
        var url = "../Comm/HelperOfTBEUI.aspx?EnsName=" + enName + "&AttrKey=" + attrKey;
        var str = "";
        if (explorer.indexOf("Chrome") >= 0) {// Google 
            window.open(url, "sd", "left=200,height=500,top=150,width=400,location=yes,menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");
        }
        else {//IE, Firefox 
            str = window.showModalDialog(url, "sd", "dialogHeight:500px;dialogWidth:400px;dialogTop:150px;dialogLeft:200px;center:no;help:no");
            if (str == undefined) return;
            $("*[id$=" + ctrl + "]").focus().val(str);
        }


    }
</script>
<table border="0" style="width: 100%; height: 100%" align="center">
    <caption>
         Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
    <tr>
        <td valign="top">
            <br />
            <table border="0" style="width: 100%; height: 100%" align="center">
                <tr>
                    <td colspan="1" valign="top" class="ToolBar" style="text-align: left">
                        <uc1:ToolBar ID="ToolBar1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" style="text-align: left" valign="top">
                        <uc2:Pub ID="Top" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 300px" style="text-align: left">
                        <uc2:Pub ID="Pub1" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <br />
        </td>
    </tr>
</table>
