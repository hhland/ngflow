<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeBehind="RETemplete.aspx.cs" Inherits="CCFlow.WF.MapDef.RegularExpressionTemplete" %>

<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function DoIt(fk_mapdata, keyOfEn, forCtrl, reid,reName) {
            if (window.confirm(' Are you sure you clear the current settings you load this expression ?') == false)
                return;
            document.getElementById("selectRe").value = reName;
            window.location.href = 'RETemplete.aspx?FK_MapData=' + fk_mapdata + '&KeyOfEn=' + keyOfEn + '&REID=' + reid + '&ForCtrl=' + forCtrl;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="width: 150px;">
        <legend> About the event template </legend> Using the Event template 
        <ul>
            <li> Can help you quickly define the form field events .</li>
        </ul>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <uc1:Pub ID="Pub1" runat="server" />
</asp:Content>
