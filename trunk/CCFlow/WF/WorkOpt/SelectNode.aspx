<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectNode.aspx.cs" Inherits="CCFlow.WF.WorkOpt.SelectNode" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Insertion step / Process </title>
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var selectType;

        $(function () {
            $('#tab').tabs({
                onSelect: function (title) {
                    selectType = title == ' Step ' ? 'node' : 'flow';
                }
            });
        });

        // Get selected value 
        //1. If the selected node step ,则val Format :idx,nodeid,nodetext
        //2. If you check the flow ,则val Format :flowno,flowname
        function getSelectedValue() {
            return selectType == 'node' ? $('#<%=lbNodes.ClientID %>').val() : $('#<%=lbFlows.ClientID %>').val();
        }

        // Gets the currently selected type 
        // Selected nodes steps node, Process for flow
        function getSelectedType() {
            return selectType;
        }
    </script>
</head>
<body class="easyui-layout">
    <form id="form1" runat="server">
    <div data-options="region:'center',border:false" style="padding: 5px">
        <div id="tab" class="easyui-tabs" data-options="fit:true">
            <div title=" Step " data-options="closed:<%=this.ShowFlowOnly.ToString().ToLower() %>">
                <cc1:LB ID="lbNodes" runat="server" style="width:100%;height:100%;border:0">
                </cc1:LB>
            </div>
            <div title=" Process " data-options="closed:<%=this.ShowNodeOnly.ToString().ToLower() %>">
                <cc1:LB ID="lbFlows" runat="server" style="width:100%;height:100%;border:0">
                </cc1:LB>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
