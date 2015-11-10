<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowManager.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.FlowManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Process Scheduling </title>
    <link href="../AppDemoLigerUI/jquery/tablestyle.css" rel="stylesheet" type="text/css" />
    <link href="../AppDemoLigerUI/jquery/lib/ligerUI/skins/Aqua/css/ligerui-all.css"
        rel="stylesheet" type="text/css" />
    <link href="../AppDemoLigerUI/jquery/lib/ligerUI/skins/ligerui-icons.css" rel="stylesheet"
        type="text/css" />
    <script src="../AppDemoLigerUI/jquery/lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.ligerDefaults.GridString = {
            errorMessage: ' <%=GetGlobalResourceMsg("errorMessage.Text")%> ',
            pageStatMessage: ' <%=GetGlobalResourceMsg("pageStatMessage.Pattern")%> ',
            pageTextMessage: '<%=GetGlobalResourceMsg("pageTextMessage.Text")%>',
            loadingMessage: ' <%=GetGlobalResourceMsg("loadingMessage.Text")%>',
            findTextMessage: ' <%=GetGlobalResourceMsg("findTextMessage.Text")%> ',
            noRecordMessage: ' <%=GetGlobalResourceMsg("noRecordMessage.Text")%> ',
            isContinueByDataChanged: ' <%=GetGlobalResourceMsg("isContinueByDataChanged.Text")%> ',
            cancelMessage: ' <%=GetGlobalResourceMsg("cancelMessage.Text")%> ',
            saveMessage: ' <%=GetGlobalResourceMsg("saveMessage.Text")%> ',
            applyMessage: ' <%=GetGlobalResourceMsg("applyMessage.Text")%> ',
            draggingMessage: '<%=GetGlobalResourceMsg("draggingMessage.Pattern")%>'
        };
    </script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/plugins/ligerButton.js" type="text/javascript"></script>
    <script src="../AppDemoLigerUI/jquery/lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <%-- <script src="../AppDemoLigerUI/js/QiYeList.js" type="text/javascript"></script>
    --%>
    <script type="text/javascript">
        function OpenIt(url) {
            //            window.open(url, 'card', 'width=700,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false');
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px; dialogTop: 100px; dialogLeft: 110px; center: yes; help: no');
            if (b != null)
                loadData();
        }

        function DelIt(queryType, oid, fk_flow) {
            $("#pageloading").show();
            if (window.confirm(' <%=GetGlobalResourceMsg("SureDelete.Text")%>') == false) {
                $("#pageloading").hide();
                return;
            }

            $.post(location.href, { 'action': 'Delete', 'key': oid, 'FK_Flow': fk_flow }, function (jsonData) {
                if (jsonData == "True") {
                    $("#pageloading").hide();
                    $.ligerDialog.success(" <%=GetGlobalResourceMsg("DeletedSuccessfully.Text")%>");
                    loadData();
                }
                else {
                    $.ligerDialog.warn(jsonData);
                    $("#pageloading").hide();
                }
            });

        }

        $(function () {
            initSearch();
            loadData();

        

        });
        // Initialization inquiry 
        function initSearch() {
            $("#sh_FlowName").ligerComboBox(
                {
                    autocomplete: true,
                    url: 'Base/DataServices.ashx?action=GetFlow',
                    valueField: 'No',
                    textField: 'Name',
                    selectBoxWidth: 120,
                    width: 120,
                    cancelable: true,
                    label: ' Type :'
                }
            );
            $("#sh_State").ligerComboBox(
                {
                    autocomplete: true,
                    selectBoxWidth: 120,
                    width: 120,
                    cancelable: true
                }
            );


            var d = new Date();
            var year = d.getFullYear();
            var month = d.getMonth() + 1; //  Remember the current month is to +1的
            var today = year + "-" + month + "-01";

            $("#sh_StartDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 130,  label: '<%=GetGlobalResourceLabel("StartTime.Text")%>', labelWidth: 65 });
            $("#sh_EndDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 130, label: '<%=GetGlobalResourceLabel("EndTime.Text")%>', labelWidth: 65 });
        }
        // Load Data 
        function loadData() {
            $("#pageloading").show();
            var strTimeKey = "";
            var date = new Date();
            strTimeKey += date.getFullYear(); //年
            strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
            strTimeKey += date.getDate(); //日
            strTimeKey += date.getHours(); //HH
            strTimeKey += date.getMinutes(); //MM
            strTimeKey += date.getSeconds(); //SS
            var dateNow = date.getFullYear()  +""+ date.getMonth() + 1 +""+ date.getDate();

            var flow = $("#sh_FlowName").ligerComboBox().getValue();
             
            var startDate = $("#sh_StartDate").val();
            var endData = $("#sh_EndDate").val();
            var state = $("#sh_State").val();
            var title = $("#sh_Title").val();
            var param = '';
            if (flow != '') {
                param += "FK_Flow='" + flow + "'";
            }
            if (title != '') {
                if (param == '') {
                    param += encodeURI(" title like '%" + title + "%'");
                } else {
                    param += encodeURI(" and title like '%" + title + "%' ");
                }

            }


            if (startDate != '') {
                if (param == '') {
                    param += " RDT>='" + startDate + "'";
                } else {
                    param += " and RDT>='" + startDate + "'";
                }
            }

            if (endData != '') {
                if (param == '') {
                    param += " RDT<='" + endData + "'";
                } else {
                    param += " and RDT<='" + endData + "'";
                }
            }
            if (state != null) {
                if (state > 0) {
                    if (param == '') {
                        param += " WFState='" + state + "'";
                    } else {
                        param += " and WFState='" + state + "'";
                    }
                }
            }
         


            $.post(location.href, { 'action': 'loadData', 'key': param }, function (jsonData) {
                if (jsonData) {
                    var pushData = eval('(' + jsonData + ')');
                    var grid = $("#maingrid").ligerGrid({
                        columns: [
                            { display: ' <%=GetGlobalResourceTitle("Title.Text")%> ', name: 'Title', width: 340, align: 'left', render: function (rowdata, rowindex) {
                                    var title = "";
                                    if (rowdata.ISREAD == 0) {
                                        //                                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlowSmall.aspx?FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FLOWENDNODE
                                        //                           + "&FID=" + rowdata.FID + "&WorkID=" + rowdata.OID + "&IsRead=0&T=" + strTimeKey
                                        //                           + "','" + rowdata.OID + "','" + rowdata.FLOWNAME + "');\" ><img align='middle' alt='' id='" + rowdata.OID
                                        //                           + "' src='Img/Menu/Mail_UnRead.png'/>" + rowdata.TITLE + "</a>";


                                        var h = "/WF/WFRpt.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "&T=" + dateNow;
                                        title = "<a href='javascript:void(0);' onclick=winOpen('" + h + "')><img align='middle' alt='' id='" + rowdata.OID + "' src='/AppDemoLigerUI/Img/Menu/Mail_UnRead.png' border=0 />" + rowdata.TITLE + "</a>";
                                    } else {
                                        //                                           title = "<a href=\"javascript:WinOpenIt('../WF/MyFlowSmall.aspx?FK_Flow=" + rowdata.FK_FLOW + "&FK_Node=" + rowdata.FLOWENDNODE
                                        //                           + "&FID=" + rowdata.FID + "&T=" + strTimeKey + "&WorkID=" + rowdata.OID + "','" + rowdata.OID
                                        //                           + "','" + rowdata.FLOWNAME + "');\"  ><img align='middle' id='" + rowdata.OID + "' alt='' src='Img/Menu/Mail_Read.png'/>" + rowdata.TITLE + "</a>";
                                        var h = "/WF/WFRpt.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "&T=" + dateNow;
                                        title = "<a href='javascript:void(0);' onclick=winOpen('" + h + "')><img align='middle' alt='' id='" + rowdata.OID + "' src='/AppDemoLigerUI/Img/Menu/Mail_Read.png'/>" + rowdata.TITLE + "</a>";

                                    }
                                    return title;
                                }
                            }, { display: ' <%=GetGlobalResourceTitle("Type.Text")%> ', name: 'FLOWNAME' },
                            { display: ' <%=GetGlobalResourceTitle("CurrentNode.Text")%> ', name: 'NODENAME' },
                            { display: ' <%=GetGlobalResourceTitle("Sponsor.Text")%> ', name: 'STARTERNAME' },
                            { display: ' <%=GetGlobalResourceTitle("Status.Text")%> ', name: 'WFState', render: function (rowdata, rowindex) {
                                    if (rowdata.WFSTATE == "0") {
                                        return " <%=GetGlobalResourceLabel("Blank.Text")%> ";
                                    } else if (rowdata.WFSTATE == "3") {
                                        return " <%=GetGlobalResourceLabel("Completed.Text")%> ";
                                    } else if (rowdata.WFSTATE == "2") {
                                        return " <%=GetGlobalResourceLabel("Run.Text")%> ";
                                    } else if (rowdata.WFSTATE == "1") {
                                        return " <%=GetGlobalResourceLabel("Draft.Text")%> ";
                                    } else if (rowdata.WFSTATE == "4") {
                                        return " <%=GetGlobalResourceLabel("Pending.Text")%> ";
                                    } else if (rowdata.WFSTATE == "5") {
                                        return " <%=GetGlobalResourceLabel("Return.Text")%> ";
                                    } else if (rowdata.WFSTATE == "6") {
                                        return " <%=GetGlobalResourceLabel("Transfer.Text")%> ";
                                    } else if (rowdata.WFSTATE == "7") {
                                        return " <%=GetGlobalResourceLabel("DeleteLogic.Text")%>";
                                    } else if (rowdata.WFSTATE == "8") {
                                        return " <%=GetGlobalResourceLabel("PlusSign.Text")%> ";
                                    } else if (rowdata.WFSTATE == "9") {
                                        return " <%=GetGlobalResourceLabel("Freeze.Text")%> ";
                                    } else {
                                        return rowdata.WFSTATE;

                                    }
                                }
                            },
                            { display: ' <%=GetGlobalResourceTitle("LaunchDate.Text")%> ', name: 'RDT' },
                            { display: ' <%=GetGlobalResourceTitle("Trajectories.Text")%> ', name: 'Node', render: function (rowdata, rowindex) {
                                    var op = null;
                                    op = "<a href=\"javascript:OpenIt('../WF/Chart.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "'); \" > <%=GetGlobalResourceLink("TurnOn.Text")%> </a>";

                                    return op;
                                }
                            },
                            { display: ' <%=GetGlobalResourceTitle("Report.Text")%> ', name: 'BillNo', render: function (rowdata, rowindex) {
                                    var op = null;
                                    op = "<a href=\"javascript:OpenIt('../WF/WFRpt.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "'); \" > <%=GetGlobalResourceLink("TurnOn.Text")%> </a>";

                                    return op;
                                }
                            },
                            { display: ' <%=GetGlobalResourceTitle("Operating.Text")%> ', name: 'WorkID', render: function (rowdata, rowindex) {
                                    var op = null;
                                    if (rowdata.WFSTATE == "3")
                                        op = "<a href=\"javascript:OpenIt('FlowRollBack.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "&FK_Node=" + rowdata.FK_NODE + "&FID=" + rowdata.FID + "');\"> <%=GetGlobalResourceLink("Rollback.Text")%> </a>";

                                    else
                                        op = "<a href=\"javascript:DelIt('Delete','" + rowdata.WORKID + "','" + rowdata.FK_FLOW + "')\"> <%=GetGlobalResourceLink("Delete.Text")%> </a> - <a href=\"javascript:OpenIt('FlowShift.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "&FK_Node="+rowdata.FK_NODE+"&FID="+rowdata.FID+"');\"> <%=GetGlobalResourceLink("Dispatch.Text")%> </a> - <a href=\"javascript:OpenIt('FlowSkip.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "');\"> <%=GetGlobalResourceLink("Jump.Text")%> </a>";

                                    return op;
                                }
                            }
                        ],

                        pageSize: 20,
                        data: pushData,
                        rownumbers: true,
                        height: "99%",
                        width: "99%",
                        columnWidth: 120,
                        onDblClickRow: function (rowdata, rowindex) {

                        }
                    });
                    $("#pageloading").hide();
                }
                else {
                    $.ligerDialog.warn(' <%=GetGlobalResourceMsg("ErrorLoadingData.Text")%>');
                }
            });
        }

        function winOpen(url) {
            // window.showModalDialog(url, '_blank', 'dialogHeight: 500px; dialogWidth: 850px;center: yes; help: no;scroll:no');
            window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');

        }
    </script>
    
    <style>
        #search input.l-text-field,#search div.l-text-label {
            margin-top: 3px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div id="search">
        <table width="90%" cellpadding="0" cellspacing="0">
            <tr>
                <td ><asp:Localize runat="server" Text="<%$ Resources:Label,Type.Text %>" /></td><td><input type="text" id="sh_FlowName" /></td>
                <td><asp:Localize runat="server" Text="<%$ Resources:Label,Keyword.Text %>" /><input type="text" id="sh_Title" /></td>
                <td><input type="text" id="sh_StartDate" /></td>
                <td><input type="text" id="sh_EndDate" /></td>
                <td style="width: 35px"> <asp:Localize runat="server" Text="<%$ Resources:Label,Status.Text %>" /> </td>
                <td style="width: 75px">
                    <select name="sh_State" id="sh_State">
                        <option value="0"> <asp:Localize runat="server" Text="<%$ Resources:Label,Whole.Text %>" /> </option>
                        <option value="1"> <asp:Localize runat="server" Text="<%$ Resources:Label,Draft.Text %>" /> </option>
                        <option value="2"> <asp:Localize runat="server" Text="<%$ Resources:Label,Run.Text %>" /> </option>
                        <option value="3"> <asp:Localize runat="server" Text="<%$ Resources:Label,Completed.Text %>" /> </option>
                        <option value="4"> <asp:Localize runat="server" Text="<%$ Resources:Label,Pending.Text %>" /> </option>
                        <option value="5"> <asp:Localize runat="server" Text="<%$ Resources:Label,Return.Text %>" /> </option>
                        <option value="6"> <asp:Localize runat="server" Text="<%$ Resources:Label,ForwardingTransfer.Text %>" /></option>
                        <option value="7"> <asp:Localize runat="server" Text="<%$ Resources:Label,DeleteLogic.Text %>" /></option>
                        <option value="8"> <asp:Localize runat="server" Text="<%$ Resources:Label,PlusSign.Text %>" /> </option>
                        <option value="9"> <asp:Localize runat="server" Text="<%$ Resources:Label,Freeze.Text %>" /> </option>
                    </select>
                </td>
                <td style="width: 30px;padding-left: 10px">
                    <input type="button" id="btnReturn" runat="server" value=" Search" onclick="loadData()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="maingrid" style="margin: 0; padding: 0">
    </div>
    <%--<div id="maingrid" style="margin: 0; padding: 0">
    </div>
    <div style="display: none;">
    </div>--%>
    </form>
    <%-- Key sub :<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;<asp:Button ID="Btn_Search" runat="server" onclick="Btn_Search_Click"
        Text=" Inquiry " />
&nbsp;

<table>
<tr>
<th> No. </th>
<th> Serial number </th>
<th> Company Name </th>
<th> Operating </th>
</tr>

 <asp:DataList ID="dtNews" runat="server" Height="148px" Width="692px">
                                            <ItemTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" class="frmNewTable">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                             <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("no") %>'></asp:Label></a>
                                                        </td>
                                                        <td>
                                                            <a href='QiYeInfo.aspx?QYBH=<%# Eval("no") %>'>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="line_01" colspan="3">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
</tr>

<%--<%
    int idx = 0;
    foreach (System.Data.DataRow dr in dt.Rows)
    {
        idx++;
        string qybh = dr["No"].ToString();
        %><TR><%="<TD>" + idx + "</TD><TD>" + dr["No"] + "</TD><TD><a href='QiYeInfo.aspx?QYBH=" + qybh + "'>" + dr["Name"] + "</a></TD>"
        %></TR><%
    }
</table>--%>
</body>
</html>
