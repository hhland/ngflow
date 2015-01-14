<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.WFRpt" CodeBehind="WFRpt.ascx.cs" %>
<%@ Register Src="UCEn.ascx" TagName="UCEn" TagPrefix="uc1" %>
<%-- <table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td>--%>
<link id="myFlowcss" href="" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript" src="/DataUser/PrintTools/LodopFuncs.js"></script>
<script type="text/javascript">
    $(function () {


        var userStyle = "<%=BP.WF.Glo.GetUserStyle %>";
        $('#myFlowcss').attr('href', 'Style/FormThemes/' + userStyle + '/MyFlow.css');


         


        var screenHeight = document.documentElement.clientHeight;


        var topBarHeight = 40;


        var allHeight = topBarHeight;


        if ("<%=BtnWord %>" == "2")
            allHeight = allHeight + 30;

        var frmHeight = "<%=Height %>";
        if (screenHeight > parseFloat(frmHeight) + allHeight) {
            $("#divCCForm").height(screenHeight - allHeight);
        }
        
       
    });

</script>
<script language="javascript" type="text/javascript">
    var LODOP; // Declared as a global variable  
    function printFrom() {
        //        var html;
        //        $.ajax({
        //            type: "post",
        //            url: url,
        //            data: null,
        //            async: false,
        //            beforeSend: function (XMLHttpRequest, fk_mapExt) {
        //                //ShowLoading();
        //            },
        //            success: function (data, textStatus) {
        //                html = data;
        //            },
        //            complete: function (XMLHttpRequest, textStatus) {
        //                //    alert('HideLoading');
        //                //HideLoading();
        //            },
        //            error: function () {
        //                alert('error when load data.');
        //                // Error processing request 
        //            }
        //        });
        var url = "PrintSample.aspx?FK_Flow=<%=this.FK_Flow%>&FK_Node=<%=this.FK_Node %>&FID=<%=this.FID %>&WorkID=<%=this.WorkID %>&AtPara=";

        LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        LODOP.PRINT_INIT(" Print Form ");
        // LODOP.ADD_PRINT_URL(30, 20, 746, "100%", location.href);
        LODOP.ADD_PRINT_HTM(0, 0, "100%", "100%", document.getElementById("divCCForm").innerHTML);
        // LODOP.ADD_PRINT_URL(0, 0, "100%", "100%", url);
        LODOP.SET_PRINT_STYLEA(0, "HOrient", 3);
        LODOP.SET_PRINT_STYLEA(0, "VOrient", 3);
        //		LODOP.SET_SHOW_MODE("MESSAGE_GETING_URL",""); // The statement hide the progress bar or modify the message 
        //		LODOP.SET_SHOW_MODE("MESSAGE_PARSING_URL","");// The statement hide the progress bar or modify the message 
        //  LODOP.PREVIEW();

        LODOP.PREVIEW();
        return false;

    }


</script>
<div style="width: <%=Width %>px; margin: 0 auto; background: white; text-align: left;">
    <asp:Button ID="Btn_Print" runat="server" Text=" Print " CssClass="Btn" Visible="true"
        OnClientClick="return printFrom()" />
</div>
<div style="width: <%=Width %>px; margin: 0 auto; background: white; border-top: 1px solid #4D77A7;">
    <br />
</div>
<uc1:UCEn ID="UCEn1" runat="server" />
<%--<td>
<tr>
</table>
--%>
<script type="text/javascript">
    function ReinitIframe(frmID, tdID) {
        try {

            var iframe = document.getElementById(frmID);
            var tdF = document.getElementById(tdID);
            iframe.height = iframe.contentWindow.document.body.scrollHeight;
            iframe.width = iframe.contentWindow.document.body.scrollWidth;
            if (tdF.width < iframe.width) {
                tdF.width = iframe.width;
            } else {
                iframe.width = tdF.width;
            }

            tdF.height = iframe.height;
            return;

        } catch (ex) {

            return;
        }
        return;
    }
</script>
<style type="text/css">
    .ActionType
    {
        width: 16px;
        height: 16px;
    }
</style>
