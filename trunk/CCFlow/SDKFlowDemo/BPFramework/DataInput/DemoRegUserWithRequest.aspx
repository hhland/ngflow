<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoRegUserWithRequest.aspx.cs" Inherits="CCFlow.SDKFlowDemo.DemoRegUserWithRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../WF/Comm/Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="../../../WF/Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border=1 align=center width="50%">
    <caption> User Registration ( Use  BP.Sys.PubClass.CopyFromRequest  Method demonstrates how to write data to the data table  )</caption>
     <tr>
    <td> Account number </td>
    <td>
        <asp:TextBox ID="TB_No" runat="server"></asp:TextBox>
        </td>
    <td> Must be a combination of letters or numbers underscore </td>
    </tr>
    <tr>
    <td> Full name </td>
    <td>
        <asp:TextBox ID="TB_Name" runat="server"></asp:TextBox>
        </td>
    <td></td>
    </tr>
    <tr>
    <td> Password </td>
    <td>
        <asp:TextBox ID="TB_Pass" runat="server"></asp:TextBox>
        </td>
    <td> Can not be empty </td>
    </tr>
     <tr>
    <td> Weight lost password </td>
    <td>
        <asp:TextBox ID="TB_Pass1" runat="server"></asp:TextBox>
         </td>
    <td> The two need to be consistent </td>
    </tr>
    <tr>
    <td> Sex </td>
    <td>
        <asp:DropDownList ID="DDL_XB" runat="server">
            <asp:ListItem Value="1">Male</asp:ListItem>
            <asp:ListItem Value="0">Female</asp:ListItem>
        </asp:DropDownList>
        </td>
    <td></td>
    </tr>
    
    <tr>
    <td> Age </td>
    <td>
        <asp:TextBox ID="TB_Age" runat="server" >0</asp:TextBox>
        </td>
    <td> Enter int Types of data </td>
    </tr>

    <tr>
    <td> Address </td>
    <td>
        <asp:TextBox ID="TB_Addr" runat="server"></asp:TextBox>
        </td>
    <td></td>
    </tr>

     <tr>
    <td> Phone </td>
    <td>
        <asp:TextBox ID="TB_Tel" runat="server"></asp:TextBox>
         </td>
    <td></td>
    </tr>

    
     <tr>
    <td> Mail </td>
    <td>
        <asp:TextBox ID="TB_Email" runat="server"></asp:TextBox>
         </td>
    <td></td>
    </tr>

       <tr>
    <td colspan=3><asp:Button ID="Btn_Reg" runat="server" Text=" Sign up " 
            onclick="Btn_Reg_Click" /></td>
    </tr>


     <tr>
    <td colspan=3>
     Explanation :
    1,  Corresponding fields on this form class BP.Demo.BPUser, Named according to the format field BP Agreed regulatory framework , Text type field TB+"_" +  Field name .
    
    <br>
    2,  After submission system , According to background Post Form data passed over Entity  The assignment of data to the entity classes .

    </td>
    </tr>


    </table>

    </div>
    </form>
</body>
</html>
