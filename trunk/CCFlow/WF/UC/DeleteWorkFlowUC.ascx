<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteWorkFlowUC.ascx.cs" Inherits="CCFlow.WF.UC.DeleteWorkFlowUC" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<%@ Register src="../Comm/UC/ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
            <fieldset>
            <legend> Job Remove </legend>
<div align="center" >
            <div  align="center" style='height:30px;'  >
               <uc2:toolbar ID="ToolBar1" runat="server" />
            </div>
            <div >
            <hr />
                <uc1:pub ID="Pub1" runat="server" />
            </div>
</div>
            </fieldset>

<script type="text/javascript" >
    function OnChange(ctrl) {
//        var text = ctrl.options[ctrl.selectedIndex].text;
//        var user = text.substring(0, text.indexOf('='));
//        var nodeName = text.substring(text.indexOf('>') + 1, 1000);
//        var objVal = ' Hello ' + user + ':';
//        objVal += "  \t\n ";
//        objVal += "  \t\n ";
//        objVal += "    You are dealing with  [" + nodeName + "]  Working with errors , You need to re-apply ． ";
//        objVal += "\t\n   \t\n 礼! ";
//        objVal += "  \t\n ";
//        document.getElementById('ContentPlaceHolder1_ReturnWork1_Pub1_TB_Doc').value = objVal;
    }
</script>
