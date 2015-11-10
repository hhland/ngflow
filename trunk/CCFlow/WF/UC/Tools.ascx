<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.Tools" Codebehind="Tools.ascx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<%@ Register src="ToolsWap.ascx" tagname="ToolsWap" tagprefix="uc2" %>

<table border=0 width='100%' align='left'  >
<caption> System Settings 
</caption>

<tr>
<td  valign=top width='20%' align='center' >
    <uc1:Pub ID="Left" runat="server" />
    <br>
    <br>
    <br>
    <br>
    <br>
    </td>
<td  valign=top  align='left'  width='80%' >
    <uc2:ToolsWap ID="ToolsWap1" runat="server" />
    </td>
</tr>
</table>
