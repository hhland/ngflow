<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DemoUserList.aspx.cs" Inherits="CCFlow.SDKFlowDemo.BPFramework.Output.DemoUserList" %>

<%@ Register src="../../Pub.ascx" tagname="Pub" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Not paged data output </title>
     <link href="/WF/Comm/Style/Table.css" rel="stylesheet" 
        type="text/css" />
    <link href="/WF/Comm/Style/Table0.css" rel="stylesheet" 
        type="text/css" />
    <script type="text/javascript">
        function Del(no) {
            if (window.confirm(' Are you sure you want to delete the number of (' + no + ') The staff do ?') == false)
                return;
            var url = 'DemoUserList.aspx?DoType=Del&RefNo=' + no;
            window.location.href = url;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc1:Pub ID="Pub1" runat="server" />
    
    </div>
    </form>
</body>
</html>
