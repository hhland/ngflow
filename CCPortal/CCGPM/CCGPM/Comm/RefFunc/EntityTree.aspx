<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master"
 AutoEventWireup="true" Inherits="Comm_RefFunc_EntityTree"
 Codebehind="EntityTree.aspx.cs" %>
<%@ Register src="../UC/Pub.ascx" tagname="Pub" tagprefix="uc2" %>
<%@ Register src="../UC/UIEn.ascx" tagname="UIEn" tagprefix="uc3" %>
<%@ Register src="Left.ascx" tagname="Left" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="JavaScript" src="../JScript.js"></script>
    <script language="javascript">
        function ShowEn(url, wName, h, w) {
            var s = "dialogWidth=" + parseInt(w) + "px;dialogHeight=" + parseInt(h) + "px;resizable:yes";
            var val = window.showModalDialog(url, null, s);
            window.location.href = window.location.href;
        }
        function GroupBarClick(rowIdx) {
            var alt = document.getElementById('Img' + rowIdx).alert;
            var sta = 'block';
            if (alt == 'Max') {
                sta = 'block';
                alt = 'Min';
            } else {
                sta = 'none';
                alt = 'Max';
            }
            document.getElementById('Img' + rowIdx).src = './Img/' + alt + '.gif';
            document.getElementById('Img' + rowIdx).alert = alt;
            var i = 0
            for (i = 0; i <= 40; i++) {
                if (document.getElementById(rowIdx + '_' + i) == null)
                    continue;
                document.getElementById(rowIdx + '_' + i).style.display = sta;
            }
        }
</script>
    <style type="text/css">
        .style1
        {
            width: 157px;
        }
        .style2
        {
            width: 711px;
        }
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="1px" width='100%' >
<tr>
<td valign=top class="style1">
<%
    string ensName = this.Request.QueryString["EnsName"];
    string enName = this.Request.QueryString["EnName"];
    string pk = this.Request.QueryString["No"];
    if (string.IsNullOrEmpty(pk))
        pk = "0001";
    string url = "EntityTree.aspx?EnsName=" + ensName + "&EnName=" + enName + "&No=" + pk;
  %>
  <b>
[<a href="<%=url %>&DoType=AddSameLevelNode">+同级节点</a>][<a href="<%=url %>&DoType=AddSubNode">+子节点</a>]
[<a href="<%=url %>&DoType=DoUp">上移</a>][<a href="<%=url %>&DoType=DoDown">下移</a>]
</b>
<hr />
    <asp:TreeView ID="TreeView1" runat="server" ImageSet="XPFileExplorer" 
        NodeIndent="15">
        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
        <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" 
            HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
        <ParentNodeStyle Font-Bold="False" />
        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" 
            HorizontalPadding="0px" VerticalPadding="0px" />
    </asp:TreeView>
    </td>
<td valign=top class="style2"  >
    <uc2:Pub ID="Pub1" runat="server" />
    </td>
</tr>
</table>
</asp:Content>

