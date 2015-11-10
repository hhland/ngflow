<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocRelease.ascx.cs" Inherits="CCFlow.SDKFlowDemo.SDK.F111.DocRelease" %>
<%@ Register src="/WF/SDKComponents/FrmCheck.ascx" tagname="FrmCheck" tagprefix="uc2" %>
<%@ Register src="/WF/SDKComponents/DocMainAth.ascx" tagname="DocMainAth" tagprefix="uc1" %>
<%@ Register src="/WF/SDKComponents/DocMultiAth.ascx" tagname="DocMultiAth" tagprefix="uc3" %>

<asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Send_Click" />
<asp:Button ID="Btn_Return" runat="server" Text=" Return " onclick="Btn_Return_Click" />
<asp:Button ID="Btn_Track" runat="server" Text=" Locus " onclick="Btn_Track_Click" />
<asp:Button ID="Btn_AskForHelp" runat="server" Text=" Plus sign " onclick="Btn_AskForHelp_Click" />
<asp:Button ID="Btn_CC" runat="server" Text=" Cc " onclick="Btn_CC_Click" />

<hr>

<div  id="Div_Msg" >
</div>

<div style="text-align:center">
</div>

<table style="width:100%">
<caption><font color="red"><H1> Shenzhen People Congress documents </H1> </font> </caption>
<tr>
<td> Units issued </td>
<td> Units issued </td>
</tr>

<tr>
<td> Text </td>
<td colspan=1>
    <uc1:DocMainAth ID="DocMainAth1" runat="server" />
    </td>
</tr>

<tr>
<td colspan=2>
 Accessory :<br />
    <uc3:DocMultiAth ID="DocMultiAth1" runat="server" />
    </td>
</tr>

 
<tr>
<td colspan="2" >
    <uc2:FrmCheck ID="FrmCheck1" runat="server" />
    </td>
</tr>


</table>

