<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/QingJia/Site1.Master" AutoEventWireup="true" CodeBehind="S4_RenShi.aspx.cs" Inherits="CCFlow.SDKFlowDemo.QingJia.S4_RenShi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<fieldset>
<legend><font color=blue><b> Leave basic information </b></font></legend>
<table border="1" width="90%">
<tr>
<th> Project </th>
<th> Data </th>
<th> Explanation </th>
</tr>

<tr>
<td> People leave account :</td>
<td>
    <asp:TextBox ID="TB_No" ReadOnly=true runat="server" ></asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>


<tr>
<td> Name of leave :</td>
<td>
    <asp:TextBox ID="TB_Name" ReadOnly=true runat="server" > </asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>


<tr>
<td> People leave the department number :</td>
<td>
    <asp:TextBox ID="TB_DeptNo" ReadOnly=true runat="server"  ></asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>

<tr>
<td> People leave the department name :</td>
<td>
    <asp:TextBox ID="TB_DeptName" ReadOnly=true runat="server"  ></asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>


<tr>
<td> Leave a few days :</td>
<td>
    <asp:TextBox ID="TB_QingJiaTianShu" runat="server" Text="0"  ></asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>

<tr>
<td> The reason for leave :</td>
<td>
    <asp:TextBox ID="TB_QingJiaYuanYin" runat="server" Text=" Please enter a reason to leave ..."  ></asp:TextBox>
    </td>
<td>( Read-only )</td>
</tr>
</table>
</fieldset>


<fieldset>
<legend><font color=blue><b> Department manager audit information </b></font></legend>
    <asp:TextBox ID="TB_BMNote" ReadOnly="true" TextMode="MultiLine" runat="server" Height="93px" Width="682px"></asp:TextBox>
</fieldset>

<fieldset>
<legend><font color=blue><b> Human Resources Manager audit information </b></font></legend>
    <asp:TextBox ID="TB_NoteRL" TextMode="MultiLine" runat="server" Height="93px" Width="682px"></asp:TextBox>
</fieldset>


<fieldset>
<legend><font color=blue><b> Functional operating area </b></font></legend>
    <asp:Button ID="Btn_Send" runat="server" Text=" Send " 
        onclick="Btn_Send_Click" />
    
    <asp:Button ID="Btn_Save" runat="server" Text=" Save " onclick="Btn_Save_Click" />

    <asp:Button ID="Btn_Return" runat="server" onclick="Btn_Return_Click" 
        Text=" Return " />

    <asp:Button ID="Btn_Track" runat="server" Text=" Flow chart " 
        onclick="Btn_Track_Click" />
</fieldset>


<fieldset>
<legend>URL By value </legend>
<font color=blue>
<%=this.Request.RawUrl %>
</font>
</fieldset>


</asp:Content>
