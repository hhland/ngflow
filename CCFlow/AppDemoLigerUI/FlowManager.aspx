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
            if (window.confirm(' Are you sure you want to delete it ?') == false) {
                $("#pageloading").hide();
                return;
            }

            $.post(location.href, { 'action': 'Delete', 'key': oid, 'FK_Flow': fk_flow }, function (jsonData) {
                if (jsonData == "True") {
                    $("#pageloading").hide();
                    $.ligerDialog.success(" Deleted successfully !");
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

            $("#sh_StartDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 130,  label: 'Start Time', labelWidth: 65 });
            $("#sh_EndDate").ligerDateEditor({ format: "yyyy-MM-dd", width: 130, label: 'End Time', labelWidth: 65 });
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
                      { display: ' Title ', name: 'Title', width: 340, align: 'left', render: function (rowdata, rowindex) {
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
                      }, { display: ' Type ', name: 'FLOWNAME' },
                   { display: ' The current node ', name: 'NODENAME' },
                   { display: ' Sponsor ', name: 'STARTERNAME' },
                            { display: ' Status ', name: 'WFState', render: function (rowdata, rowindex) {
                                if (rowdata.WFSTATE == "0") {
                                    return " Blank ";
                                } else if (rowdata.WFSTATE == "3") {
                                    return " Completed ";
                                } else if (rowdata.WFSTATE == "2") {
                                    return " Run ";
                                } else if (rowdata.WFSTATE == "1") {
                                    return " Draft ";
                                } else if (rowdata.WFSTATE == "4") {
                                    return " Pending ";
                                } else if (rowdata.WFSTATE == "5") {
                                    return " Return ";
                                } else if (rowdata.WFSTATE == "6") {
                                    return " Transfer ";
                                } else if (rowdata.WFSTATE == "7") {
                                    return " Delete ( Logic )";
                                } else if (rowdata.WFSTATE == "8") {
                                    return " Plus sign ";
                                } else if (rowdata.WFSTATE == "9") {
                                    return " Freeze ";
                                } else {
                                    return rowdata.WFSTATE;

                                }
                            }
                            },
                   { display: ' Launch date ', name: 'RDT' },
                   { display: ' Trajectories ', name: 'Node', render: function (rowdata, rowindex) {
                       var op = null;
                       op = "<a href=\"javascript:OpenIt('../WF/Chart.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "'); \" > Turn on </a>";

                       return op;
                   }
                   },
               { display: ' Report ', name: 'BillNo', render: function (rowdata, rowindex) {
                   var op = null;
                   op = "<a href=\"javascript:OpenIt('../WF/WFRpt.aspx?WorkID=" + rowdata.WORKID + "&FK_Flow=" + rowdata.FK_FLOW + "&FID=" + rowdata.FID + "'); \" > Turn on </a>";

                   return op;
               }
               },
                { display: ' Operating ', name: 'WorkID', render: function (rowdata, rowindex) {
                    var op = null;
					 if (rowdata.WFSTATE == "3")
                        op = "<a href=\"javascript:OpenIt('FlowRollBack.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "&FK_Node=" + rowdata.FK_NODE + "&FID=" + rowdata.FID + "');\"> Rollback </a>";

                    else
                    op = "<a href=\"javascript:DelIt('Delete','" + rowdata.WORKID + "','" + rowdata.FK_FLOW + "')\"> Delete </a> - <a href=\"javascript:OpenIt('FlowShift.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "&FK_Node="+rowdata.FK_NODE+"&FID="+rowdata.FID+"');\"> Dispatch </a> - <a href=\"javascript:OpenIt('FlowSkip.aspx?FK_Flow=" + rowdata.FK_FLOW + "&WorkID=" + rowdata.WORKID + "');\"> Jump </a>";

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
                    $.ligerDialog.warn(' Error loading data , Please retry close !');
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
                 <td >Type :</td><td><input type="text" id="sh_FlowName" /></td>
                <td>Keyword <input type="text" id="sh_Title" /></td>
                <td><input type="text" id="sh_StartDate" /></td>
                <td><input type="text" id="sh_EndDate" /></td>
                <td style="width: 35px"> Status </td>
                <td style="width: 75px">
                    <select name="sh_State" id="sh_State">
                        <option value="0"> Whole </option>
                        <option value="1"> Draft </option>
                        <option value="2"> Run </option>
                        <option value="3"> Completed </option>
                        <option value="4"> Pending </option>
                        <option value="5"> Return </option>
                        <option value="6"> Forwarding ( Transfer )</option>
                        <option value="7"> Delete ( Logic )</option>
                        <option value="8"> Plus sign </option>
                        <option value="9"> Freeze </option>
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
