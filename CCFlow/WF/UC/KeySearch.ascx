<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.KeySearch" Codebehind="KeySearch.ascx.cs" %>
<%@ Register src="../Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<script type="text/javascript">
    function OpenIt(fk_flow, fk_node, workid) {
        var url = './WFRpt.aspx?WorkID=' + workid + '&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node;
        var newWindow = window.open(url, 'card', 'width=700,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false');
        newWindow.focus();
        return;
    }
    function NoSubmit(ev) {
        if (window.event.srcElement.tagName == "TEXTAREA")
            return true;
        if (ev.keyCode == 13) {
            window.event.keyCode = 9;
            ev.keyCode = 9;
            return true;
        }
        return true;
    }
</script>

<table width="100%">
<caption> Keyword global query </caption>
<tr>
<td>
<b>&nbsp; Enter any keywords :</b><asp:TextBox ID="TextBox1" runat="server" BorderStyle=Inset
 BorderColor=AliceBlue
 Font-Bold="True" 
        Font-Size="Large" Width="259px"></asp:TextBox> 
<asp:CheckBox ID="CheckBox1" runat="server" Font-Bold="True" 
    ForeColor="#0033CC" Text=" I participated in the inquiry process only " />
        <br />
    &nbsp;<asp:Button ID="Btn_ByWorkID" runat="server" Text="Search by Job ID"   
         onclick="Button1_Click"  />
        <asp:Button ID="Btn_ByTitle" runat="server" Text=" Title field investigation process by keyword "  
          onclick="Button1_Click"/>


        <asp:Button ID="Btn_ByAll" runat="server" Text=" Check all fields keywords " Visible=false Font-Bold="True" 
         onclick="Button1_Click" />
<%-- Explanation : In order to improve the efficiency of your query right choice query .--%>
        <hr />
    <uc1:Pub ID="Pub1" runat="server" />
</td>
</tr>
</table>

