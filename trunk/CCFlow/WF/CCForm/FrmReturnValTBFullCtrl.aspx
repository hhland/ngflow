<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" Inherits="CCFlow.WF.CCForm.WF_ReturnValTBFullCtrl" Codebehind="FrmReturnValTBFullCtrl.aspx.cs" %><%@ Register src="UC/ReturnValTBFullCtrl.ascx" tagname="ReturnValTBFullCtrl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <style>
   
   .Title
   {
     text-align:left !important;
       
   }
   
   th
   {
       font-size:10px !important;  
    }
   
   </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <uc1:ReturnValTBFullCtrl ID="ReturnValTBFullCtrl1" runat="server" />
</asp:Content>

