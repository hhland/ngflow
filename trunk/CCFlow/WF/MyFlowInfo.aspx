<%@ Page Title="" Language="C#" MasterPageFile="SDKComponents/Site.Master" AutoEventWireup="true" Inherits="CCFlow.WF.WF_MyFlowInfoSmall" Codebehind="MyFlowInfo.aspx.cs" %>
<%@ Register src="UC/MyFlowInfo.ascx" tagname="MyFlowInfo" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="./Comm/JScript.js" type="text/javascript" ></script>
    <link href="Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <!--
    
    <link href="Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    -->
    <style>
        fieldset {
        margin: 5px;
       table-layout: auto;
padding: 2px;
text-align: left;
}

caption {
vertical-align: middle;
text-align: left;
height: 32px;
line-height: 32px;
font-size: 14px;
background: url('/WF/Comm/Style/BG_Title.png') repeat-x;
height: 30px;
line-height: 30px;
padding-left: 10px;
font-family: 黑体;
font-weight: bolder;
}

.datagrid-cell {
    white-space:normal
}

    </style>
    <link href="Comm/JS/EasyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Comm/JS/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="Comm/JS/EasyUI/jquery.easyui.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript" >
    if (window.opener && !window.opener.closed) {
        if (window.opener.name == "main") {
            window.opener.location.href = window.opener.location.href;
            window.opener.top.leftFrame.location.href = window.opener.top.leftFrame.location.href;
        }
    }

    function formatEmps() {
        $emps = $("span.info:eq(1)");
        var txtemps = $emps.text();
        var bri = txtemps.indexOf("(");
        var textsub = txtemps.substring(0, bri);
        var emps = txtemps.substring(bri);
        var url = "WorkOpt/OneWork/DeptEmpInfoList.ashx";
        var html = "<span>" + textsub + "</span>";
       // html += "<div id='pp'>";
            html+="<table id='tt'   style='width:100%;'><thead><tr>"
            + "<th data-options=\"field:'dept',width:100\" ><%=GetGlobalResourceTitle("DepartmentName.Text")%><"+"/th>"
            + "<th data-options=\"field:'name',width:80\" ><%=GetGlobalResourceTitle("Name.Text")%><" + "/th>"
            + "<th data-options=\"field:'email',width:80\" ><%=GetGlobalResourceTitle("Email.Text")%><" + "/th>"
            + "<th data-options=\"field:'tel',width:80\" ><%=GetGlobalResourceTitle("Tel.Text")%><" + "/th>"
            + "<"+"/tr>"
            + "<"+"/thead>"
            + "<tbody id='dd'><"+"/tbody>"
            + "<" + "/table>";
           // html+="</div>"
        $emps.html(html);
        $emps.find("#dd").load(url, { emps: emps,pformat:"emps" },function() {
            
            $emps.find("#tt").datagrid({
                height: 160
                , width: "100%"
                , striped: true
                , fitColumns: true
                ,nowrap:false
            });
        });
    }

    function formatLang() {
        $("#linkAllotTask").text("<%=GetGlobalResourceLink("AllotTask.Text")%>");
        $("#linkUnDo").text("<%=GetGlobalResourceLink("UnDo.Text")%>");
        $("#linkRpt").text("<%=GetGlobalResourceLink("Rpt.Text")%>");
        $("#linkNewProcess").text("<%=GetGlobalResourceLink("NewProcess.Text")%>");
        var wsr=$("#workSendResult");
        var wsr_text = "<%=GetGlobalResourceMsg("WorkSendResult.Pattern")%>".replace("{0}", wsr.attr("toNode")).replace("{1}", wsr.attr("EmpsExt"));
        wsr.text(wsr_text);
        var nwss = $("#nextWorkStartSuccessfully");
        var nwss_text = "<%=GetGlobalResourceMsg("NextWorkStartSuccessfully.Pattern")%>".replace("{0}", wsr.attr("toND"));
        nwss.text(nwss_text);
    }

    $(document).ready(function () {

        formatEmps();
        formatLang();
    });
</script>
 <fieldset>
     <legend></legend>
 </fieldset>
<table style=" text-align:left; width:100%;">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td>
<uc2:MyFlowInfo ID="MyFlowInfo1" runat="server"  />
 </td>
</tr>
</table>
 
</asp:Content>
