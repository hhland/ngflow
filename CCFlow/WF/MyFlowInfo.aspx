<%@ Page Title="" Language="C#" MasterPageFile="SDKComponents/Site.Master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_MyFlowInfoSmall" Codebehind="MyFlowInfo.aspx.cs" %>
<%@ Register src="UC/MyFlowInfo.ascx" tagname="MyFlowInfo" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="./Comm/JScript.js" type="text/javascript" ></script>
    <link href="Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Comm/JS/jquery-1.7.2.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript" >
    if (window.opener && !window.opener.closed) {
        if (window.opener.name == "main") {
            window.opener.location.href = window.opener.location.href;
            window.opener.top.leftFrame.location.href = window.opener.top.leftFrame.location.href;
        }
    }

    $(document).ready(function () {

        $("span.info").each(function (index) {

            var text = $(this).text();
            var html = $(this).html();
            var maxlen = 280;
            if ($(this).find("a").size() > 0) { }
            else if (text != null && text.length > maxlen) {

                var textsub = text.substring(0, maxlen);
                var bri = html.indexOf("<br>");
                if (bri >= 0) {
                    //如果有换行符，就截取第一行
                    textsub = html.substring(0, bri);
                }
                var _html = "<span title='" + text + "'>" + textsub + "...<" + "/span>";
                $(this).html(_html);
            }
        });

    });
</script>
 
<table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td>
<uc2:MyFlowInfo ID="MyFlowInfo1" runat="server"  />
 </td>
</tr>
</table>
 
</asp:Content>
