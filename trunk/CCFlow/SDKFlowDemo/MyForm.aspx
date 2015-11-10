<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/WinOpen.master" AutoEventWireup="true" Inherits="SDKFlows_MyForm" Codebehind="MyForm.aspx.cs" %>

<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
&nbsp;&nbsp;&nbsp;&nbsp;  This is a personalized demo form . 
     Here you are free to play any , To meet ccflow Customer demand style sheets can not be completed , Please note that your arguments passed to receive and process these parameters . These parameters are .<br />
&nbsp;&nbsp;  Process ID , The work ID, Node ID .....  You can receive these parameters to deal with them .<br />
    <br />
    <uc1:Pub ID="Pub1" runat="server" />
    <br />
    <br />
    <br />
</asp:Content>

