<%@ Control Language="C#" AutoEventWireup="true" Inherits="WF_MapDef_UC_MExt" Codebehind="MExt.ascx.cs" %>
<%@ Register src="../Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<script type="text/javascript">
    function DoDel(mypk,fk_mapdata,extType) {
        if (window.confirm(' Are you sure you want to delete it ?') == false)
            return;
        window.location.href = 'MapExt.aspx?DoType=Del&FK_MapData=' + fk_mapdata + '&ExtType=' + extType + '&MyPK=' + mypk;
    }
</script>
<table width='900px'>
<tr>
<td valign='top' width="20%" align=left><uc1:Pub ID="Left" runat="server" /></td>

<td valign='top' width="80%" align=left><uc1:Pub ID="Pub2" runat="server" /></td>
</tr>
</table>
    
    