<%@ Page Title=" Log in " Language="C#" MasterPageFile="SiteMenu.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CCFlow.SDKFlowDemo.SDK.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2> User Login </h2>
   <fieldset>
    <legend> ccflow Log in API at Button Later events  </legend>
    <br>
   Username :  <asp:TextBox ID="TB_No" runat="server"></asp:TextBox>
   Password : <asp:TextBox ID="TB_Pass" runat="server"></asp:TextBox>  <asp:Button ID="Button1" runat="server" Text="  Log in  " 
    onclick="Button1_Click" />
    <br>
    <br>
    </fieldset>

    <fieldset>
    <legend> Explanation :</legend>
    <ul>
     <li>1,  Login interface is written in your . </li>
     <li>2,  Password verification , Business logic to process login before you have to develop . </li>
     <li>3,  After successful authentication, , Calls ccflow Login API. </li>
    </ul>
    </fieldset>

</asp:Content>
