<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.Admin.WF_Admin_FlowFrms" Codebehind="FlowFrms.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
    function New() {
        window.location.href = window.location.href;
    }

    function WinField(fk_mapdata, nodeid, fk_flow) {
        var url = "../MapDef/Sln.aspx?FK_MapData=" + fk_mapdata + "&FK_Node=" + nodeid+'&FK_Flow='+fk_flow;
        WinOpen(url);
    }

    function WinFJ(fk_mapdata, nodeid, fk_flow) {
        var url = "../MapDef/Sln.aspx?FK_MapData=" + fk_mapdata + "&FK_Node=" + nodeid + '&FK_Flow=' + fk_flow+'&DoType=FJ';
        WinOpen(url);
    }

    function AddIt(fk_mapdata, fk_node, fk_flow) {
        var url = 'FlowFrms.aspx?DoType=Add&FK_MapData=' + fk_mapdata + '&FK_Node=' + fk_node + '&FK_Flow=' + fk_flow;
        window.location.href = url;
    }
    function DelIt(fk_mapdata, fk_node,fk_flow) {
        if (window.confirm(' Are you sure you want to remove it ?') == false)
            return;
        var url = 'FlowFrms.aspx?DoType=Del&FK_MapData=' + fk_mapdata + '&FK_Node=' + fk_node + '&FK_Flow=' + fk_flow;
        window.location.href = url;
    }
</script>
<base target="_self" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
    <table width='100%' height='100%' align=center  border="0px" >
<tr>
<td align=left valign=top width='20%'  border="0px">
    <uc1:Pub ID="Left" runat="server" />
    </td>
    <td align="Left" valign=top width='80%'  border="0px">
    <uc1:Pub ID="Pub1" runat="server" />
    </td>
    </tr>
    </table>
</asp:Content>

