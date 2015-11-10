<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OneFlow.aspx.cs" Inherits="CCFlow.WF.Rpt.OneFlow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> A process data management </title>
    <link href="../Comm/Style/CommStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var currShow;

        // In the right frame displays the specified url Pages 
        function OpenUrlInRightFrame(ele, url) {
            if (ele != null && ele != undefined) {
                //if (currShow == $(ele).text()) return;

                currShow = $(ele).parents('li').text(); // There carriage 

                $.each($('ul.navlist'), function () {
                    $.each($(this).children('li'), function () {
                        $(this).children('div').css('font-weight', $(this).text() == currShow ? 'bold' : 'normal');
                    });
                });

                $('#context').attr('src', url);
            }
        }
    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'center',border:false">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'west',split:true" style="width: 200px;">
                <% if (BP.Web.WebUser.No == "admin")
                   { %>
                <div class="easyui-panel" title=" Report Design / Monitor " data-options="collapsible:true,border:false" style="height: auto">
                    <ul class="navlist">
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../MapDef/Rpt/S1_Edit.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=this.RptNo%>')">
                                    <span class="nav">1.  Basic Information </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../MapDef/Rpt/S2_ColsChose.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=this.RptNo%>')">
                                    <span class="nav">2.  Set the report display columns </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../MapDef/Rpt/S3_ColsLabel.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=this.RptNo%>')">
                                    <span class="nav">3.  Set the report displays the column order </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../MapDef/Rpt/S5_SearchCond.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=this.RptNo%>')">
                                    <span class="nav">4.  Setting the report query conditions </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../MapDef/Rpt/S6_Power.aspx?FK_MapData=<%=this.FK_MapData%>&FK_Flow=<%=this.FK_Flow%>&RptNo=<%=this.RptNo%>')">
                                    <span class="nav">5.  Set Report Permissions </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../Admin/FlowDB/FlowDB.aspx?s=d34&FK_Flow=<%=this.FK_Flow%>&ExtType=StartFlow&RefNo=')">
                                    <span>6.  Process Monitoring </span></a></div>
                        </li>
                    </ul>
                </div>
                
                <%} %>
                <div class="easyui-panel" title=" Report Viewer " data-options="collapsible:true,border:false" style="height: auto">
                    <ul class="navlist">
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../../WF/Rpt/Search.aspx?RptNo=<%= this.RptNo%>')">
                                    <span>1.  Inquiry </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../../WF/Rpt/SearchAdv.aspx?RptNo=<%= this.RptNo%>')">
                                    <span>2.  Advanced Search </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../../WF/Rpt/Group.aspx?RptNo=<%= this.RptNo%>')">
                                    <span>3.  Subgroup analysis </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../../WF/Rpt/D3.aspx?RptNo=<%= this.RptNo%>')">
                                    <span>4.  Cross report </span></a></div>
                        </li>
                        <li>
                            <div>
                                <a href="javascript:void(0)" onclick="OpenUrlInRightFrame(this, '../../WF/Rpt/Contrast.aspx?RptNo=<%= this.RptNo%>')">
                                    <span>5.  Comparative analysis </span></a></div>
                        </li>
                    </ul>
                </div>
            </div>
            <div data-options="region:'center',noheader:true" style="overflow-y: hidden">
                <iframe id="context" scrolling="auto" frameborder="0" src=""
                    style="width: 100%; height: 100%;"></iframe>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
