<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.UCReturnWork"
    CodeBehind="ReturnWork.ascx.cs" %>
<%@ Register Src="../Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<%@ Register Src="./../Comm/UC/ToolBar.ascx" TagName="ToolBar" TagPrefix="uc3" %>
<div align="center">
    <div align="center" style='height: 30px;'>
        <uc3:ToolBar ID="ToolBar1" runat="server" />
    </div>
    <div style='height: 4px;'>
    </div>
    <div>
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
</div>
<script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
<script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
<script type="text/javascript">
    function OnChange(ctrl) {
        var text = ctrl.options[ctrl.selectedIndex].text;
        var user = text.substring(0, text.indexOf('='));
        var nodeName = text.substring(text.indexOf('>') + 1, 1000);
        var objVal = ' Hello ' + user + ':';
        objVal += "  \t\n ";
        objVal += "  \t\n ";
        objVal += "    You are dealing with  [" + nodeName + "]  Working with errors , You need to re-apply ． ";
        objVal += "\t\n   \t\n BR, ";
        objVal += "  \t\n ";

        try {
            document.getElementById("<%=TBClientID %>").value = objVal;
        } catch (e) {
        }
    }

    function TBHelp(ctrl, enName, attrKey) {
        var url = "../Comm/HelperOfTBEUI.aspx?EnsName=" + enName + "&AttrKey=" + attrKey;
        var str = window.showModalDialog(url, 'sd', 'dialogHeight: 500px; dialogWidth:400px; dialogTop: 150px; dialogLeft: 200px; center: no; help: no');
        if (str == undefined)
            return;

        $("*[id$=" + ctrl + "]").focus().val(str);

    }
</script>
