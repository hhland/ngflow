<%@ Control Language="C#" AutoEventWireup="true" Inherits="App_control_Login123" Codebehind="Login.ascx.cs" %>

<table style="font-size:14px;">
    <tr style="height:50px;">
        <td> Username :</td><td><asp:TextBox ID="txtUserName" runat="server" onfocus="this.style.borderColor='#1B6BB0'" onblur="this.style.borderColor='#C8E0F8'" style=" height:26px; line-height:26px; width:240px; border:1px solid #C8E0F8;font-size:large;font-weight:bolder"></asp:TextBox></td>
    </tr>
    <tr style="height:50px; ">
        <td> Password :</td><td><asp:TextBox ID="txtPassword" runat="server" onfocus="this.style.borderColor='#1B6BB0'" onblur="this.style.borderColor='#C8E0F8'" style=" height:26px; line-height:26px; width:240px; border:1px solid #C8E0F8;font-size:large;font-weight:bolder" TextMode="Password"></asp:TextBox></td>
    </tr>
</table>
<asp:LinkButton ID="lbtnSubmit" runat="server" Text=" " onclick="btnSubmit_Click" style="position:absolute; display:block; left:72px; bottom:17px; width:95px; height:30px;"></asp:LinkButton>
<div style="position:absolute; display:block; left:206px; bottom:17px; width:95px; height:30px; cursor:pointer" onclick="form1.reset()"></div>


