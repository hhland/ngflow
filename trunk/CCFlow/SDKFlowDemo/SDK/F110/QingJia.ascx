<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QingJia.ascx.cs" Inherits="CCFlow.App.F001.Apply" %>

<%@ Register src="../../../WF/SDKComponents/FrmCheck.ascx" tagname="FrmCheck" tagprefix="uc1" %>

<%@ Register src="../../../WF/SDKComponents/DocMainAth.ascx" tagname="DocMainAth" tagprefix="uc2" %>
<%@ Register src="../../../WF/SDKComponents/DocMultiAth.ascx" tagname="DocMultiAth" tagprefix="uc3" %>

<div id="WFStrl">
<asp:Button ID="Btn_Send" runat="server" Text=" Send " onclick="Btn_Send_Click" />
<asp:Button ID="Btn_Return" runat="server" Text=" Return " onclick="Btn_Return_Click" />
<asp:Button ID="Btn_CC" runat="server" Text=" Cc " onclick="Btn_CC_Click" />
<asp:Button ID="Btn_Track" runat="server" Text=" Locus " onclick="Btn_Track_Click" />
<hr>
</div>

<table style="width:80%;aligen:center">
<caption> Leave Application Form </caption>

<tr>
<td>
<fieldset>
<legend> Form area </legend>
 Leave of absence : 
 Time from : 
To: 
</fieldset>
 </td>
</tr>

<tr>
<td> 

<% 
    string str = this.Request.QueryString["FK_Node"];
    if (str != "11001" && 1==3)
    {
       /* If this is not the start node , It does not show review button . */
     %>

<fieldset>
<legend> Approval area </legend>
    <uc1:FrmCheck ID="FrmCheck1" runat="server" />
</fieldset>

<%} %>
</td>
</tr>


<tr>
<td> 
<fieldset>
<legend> Single attachment </legend>
    <uc2:DocMainAth ID="DocMainAth1" runat="server" />
</fieldset>
</td>
</tr>

<tr>
<td> 

<fieldset>
<legend> More Accessories </legend>
    <uc3:DocMultiAth ID="DocMultiAth1" runat="server" />
</fieldset>
</td>
</tr>

</table>






