<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StartSubFlow.ascx.cs" Inherits="CCFlow.WF.SDKComponents.Templelte.StartSubFlow" %>
<h3> Sponsored sub-processes demo</h3>

 Process ID :<asp:TextBox ID="TB_FlowNo" runat="server"></asp:TextBox>
<p>
     Sub-processes are sent to the second node , If you let the process automatically find is empty ,<asp:TextBox ID="TB_NodeID" runat="server"></asp:TextBox>
</p>
 The second sub-process node accepts people , If you let the process automatically find is empty , More than one person separated by commas ,<asp:TextBox ID="TB_ToEmps" runat="server"></asp:TextBox>
<br />
<br />
<asp:Button ID="Btn_Start" runat="server" Text=" Promoter Process " 
    onclick="Btn_Start_Click" />
