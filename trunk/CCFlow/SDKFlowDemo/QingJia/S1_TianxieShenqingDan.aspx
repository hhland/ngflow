<%@ Page Title="" Language="C#" MasterPageFile="~/SDKFlowDemo/QingJia/Site1.Master" AutoEventWireup="true" CodeBehind="S1_TianxieShenqingDan.aspx.cs" Inherits="CCFlow.SDKFlowDemo.QingJia.S1_TianxieShenqingDan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<fieldset>
<legend> Leave basic information </legend>
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
<td>( Read-only ) Current Account Login Login BP.Web.WebUser.No</td>
</tr>


<tr>
<td> Name of leave :</td>
<td>
    <asp:TextBox ID="TB_Name" ReadOnly=true runat="server" > </asp:TextBox>
    </td>
<td>( Read-only ) Name of the currently logged on BP.Web.WebUser.Name</td>
</tr>


<tr>
<td> People leave the department number :</td>
<td>
    <asp:TextBox ID="TB_DeptNo" ReadOnly=true runat="server"  ></asp:TextBox>
    </td>
<td>( Read-only ) People currently logged in department number BP.Web.WebUser.FK_Dept</td>
</tr>

<tr>
<td> People leave the department name :</td>
<td>
    <asp:TextBox ID="TB_DeptName" ReadOnly=true runat="server"  ></asp:TextBox>
    </td>
<td>( Read-only ) The name of the currently logged-person department BP.Web.WebUser.FK_DeptName</td>
</tr>


<tr>
<td> Leave a few days :</td>
<td>
    <asp:TextBox ID="TB_QingJiaTianShu" runat="server" Text="0"  ></asp:TextBox>
    </td>
<td> Please enter a number </td>
</tr>


<tr>
<td> The reason for leave :</td>
<td>
    <asp:TextBox ID="TB_QingJiaYuanYin" runat="server" Text=" Please enter a reason to leave ..."  ></asp:TextBox>
    </td>
<td></td>
</tr>

</table>
</fieldset>

<fieldset>
<legend> Functional operating area </legend>
    <asp:Button ID="Btn_Send" runat="server" Text=" Send ( Send to a department manager for approval )" 
        onclick="Btn_Send_Click" />
    <asp:Button ID="Btn_Save" runat="server" onclick="Btn_Save_Click" Text=" Save " />

    <% 
     //   int workid = int.Parse(this.Request.QueryString["WorkID"]);
     //   var url = "javascript:window.open('/WF/WFRpt.aspx?WorkID=" + workid + "')";
         %>
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
