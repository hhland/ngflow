<%@ Page Language="C#" AutoEventWireup="true" Inherits="Comm_GradeEns" Codebehind="GradeEns.aspx.cs" %>

<%@ Register src="UC/UCSys.ascx" tagname="UCSys" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分级实体维护</title>
    <script language='javascript' >
    function DoDel(ensName,refpkval,currPK)
    {
       if (window.confirm('您确定要删除吗？')==false)
            return;
            
        var url="Do.aspx?EnsName="+ensName+"&RefPK="+refpkval+"&DoType=DelGradeEns";
        window.showModalDialog(url , '','dialogHeight: 1px; dialogWidth:1px; center: no; help: no');
        window.location.href='GradeEns.aspx?EnsName='+ensName+'&RefPK='+currPK;
    }
    </script>
    <style type="text/css">
        .style1
        {
            width: 376px;
        }
    </style>
    <link href="./Style/Table.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table  class="Table" >
    <tr>
    <td colspan=2>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </td>
    </tr>
    
       <tr>
    <td colspan=2 class="GroupTitle"  align='right' >
          <uc1:UCSys ID="UCBar" runat="server" />
    </td>
    </tr>
    
    
    <tr>
    <td valign=top width='10%' >
       <asp:TreeView ID="TreeView1" runat="server" BorderColor="Silver" 
        ImageSet="WindowsHelp">
    </asp:TreeView>
    </td>
    <td valign=top width='90%'>
          <uc1:UCSys ID="UCSys1" runat="server" />
    </tr>
    </table>
    </form>
</body>
</html>
