<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintSample.aspx.cs" Inherits="CCFlow.WF.PrintSimple" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="UC/PrintSample.ascx" TagName="PrintSample" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
 
    <link href="Style/FormThemes/Table0.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:PrintSample ID="PrintSample1" runat="server" />
    </div>
    </form>
</body>
</html>
