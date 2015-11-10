<%@ Page Title="" Language="C#" MasterPageFile="../Site.Master" AutoEventWireup="true" CodeBehind="S11001.aspx.cs" Inherits="CCFlow.App.F001.S101" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%@ Register src="../../../WF/SDKComponents/Toolbar.ascx" tagname="Toolbar" tagprefix="uc4" %>
<%@ Register src="../../../WF/SDKComponents/FrmCheck.ascx" tagname="FrmCheck" tagprefix="uc1" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<!--  From the toolbar sdk Curry came the introduction of the formation ,  All buttons in the tool section above can be configured by the node properties button area . -->
<div >
    <uc4:Toolbar ID="Toolbar1" runat="server" />
</div>


<!--  Rewrite toolbar  Reserved send 与save  Two methods .-->
<script type="text/javascript">
    function Save() {
        var btnSave = document.getElementById('ContentPlaceHolder1_BtnSave');
        btnSave.click();
    }
    function Send() {
        if (window.confirm(' Will send you to confirm you want to perform ?') == false)
            return;
        var btnSend = document.getElementById('ContentPlaceHolder1_BtnSend');
        btnSend.disable = true; // It can not be the first 2 Press this button once .
        btnSend.click();
    }
</script>

<!--  These two buttons to hide , Use rewrite javascript 的send 与save  Method call them . -->
<div style=" visibility:hidden">
<asp:Button ID="BtnSend" runat="server" Text="[Send]"  onclick="Btn_Send_Click" />
<asp:Button ID="BtnSave" runat="server" Text="[Save]"  onclick="Btn_Save_Click" />
</div>
<!--  Save and Send button to hide . -->


<!--  Custom forms part ........................... -->
<table style="width:100%; text-align:left; background-color:White">
<tr>
<td>
<fieldset>
<legend><font color=green ><b> Leave basic information </b></font></legend>
<table border="1" width="90%">
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
<!-- end  Custom forms part ........................... -->

 </td>
</tr>


<!--  Audit Components  ....................  -->
<% 
    string str = this.Request.QueryString["FK_Node"];
    if (str != "11001")
    {
       /* If this is not the start node , It does not show review button . */
     %>
<tr>
<td> 
<fieldset>
<legend><font color=green ><b> Approval area </b></font></legend>
    <uc1:FrmCheck ID="FrmCheck1" runat="server" />
</fieldset>
</td>
</tr>
<%  } %>

</table>
 
</asp:Content>
