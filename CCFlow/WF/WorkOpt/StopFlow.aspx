<%@ Page Title=" Suspend the process " Language="C#" MasterPageFile="~/WF/SDKComponents/Site.Master" AutoEventWireup="true" CodeBehind="StopFlow.aspx.cs" Inherits="CCFlow.WF.WorkOpt.StopFlow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
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
    <link href="../Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



 <table style=" text-align:left; width:100%">
<caption> Hello :<%=BP.WF.Glo.GenerUserImgSmallerHtml(BP.Web.WebUser.No,BP.Web.WebUser.Name) %></caption>
<tr>
<td valign=top style=" text-align:center">
    <br>
    <br>


<table style="width:500px" >

<tr>
<th valign=top colspan=2>
  End Process ( Please enter a reason for ending the process )
</th>
</tr>

<tr>
<td valign=top>
 Explanation :
<ul style=" margin:3px ">
<li>1. End of the process identification process has been completed , After the node is not executed .</li>
<li>2. After the process has completed the process has been completed can check .</li>
<li>3. To run down the recovery process needs to inform the administrator .</li>
</ul>
</td>
<td valign=top style="width:300px">
    <asp:TextBox ID="TextBox1" runat="server" Height="110px" TextMode="MultiLine" 
        Width="100%"></asp:TextBox>
    </td>
</tr>
<tr>
<td></td>
<td align=right>
    <asp:Button ID="Btn_OK" runat="server" Text=" Determine " onclick="Btn_OK_Click" />
    <asp:Button ID="Btn_Cancel" runat="server"  
        OnClientClick="window.location.go(-1);" Text=" Cancel " onclick="Btn_Cancel_Click" />
    </td>
</tr>
</table>


<br>
<br>
<br>
<br>
<br>
<br>



 
    </td>
</tr>
</table>

</asp:Content>
