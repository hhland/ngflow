<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_EnsSimple" Codebehind="EnsSimple.aspx.cs" %>

<%@ Register src="UC/UCSys.ascx" tagname="UCSys" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <LINK href="./Style/Table.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="JScript.js"></script>
		 <script language="JavaScript" src="./JS/Calendar/WdatePicker.js" defer="defer" ></script>
		    <script language="javascript">
　　 function selectAll()
　　 {
　　   var arrObj=document.all;
　　   if(document.aspnetForm.checkedAll.checked)
　　   {
　　     for(var i=0;i<arrObj.length;i++)
　　     {
　　         if(typeof arrObj[i].type != "undefined" && arrObj[i].type=='checkbox') 
　　          {
　　             
　　          
　　           if (arrObj[i].name.indexOf('IDX_') > 0 )
　　              arrObj[i].checked =true;
　         　 }
　　      }
　　    }else{
　　     for(var i=0;i<arrObj.length;i++)
　　      {
　     　   if(typeof arrObj[i].type != "undefined" && arrObj[i].type=='checkbox') 
　     　   {
　　           if (arrObj[i].name.indexOf('IDX_') > 0 )
　     　             arrObj[i].checked =false;
　     　    }
　     　 }
　　    }
　　 }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <table  >
     <tr>
     <td>
         <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
         </td>
    </tr>
    
     <tr>
     <td>
        <uc1:UCSys ID="UCSys1" runat="server" />
        </td>
     </table>
    </form>
</body>
</html>
