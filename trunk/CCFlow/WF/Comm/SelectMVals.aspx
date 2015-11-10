<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMVals.aspx.cs" Inherits="CCFlow.WF.Comm.SelectMVals" %>

<%@ Register src="UC/Pub.ascx" tagname="Pub" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function SelectAll() {
            var arrObj = document.all;
            if (document.forms[0].checkedAll.checked) {
                for (var i = 0; i < arrObj.length; i++) {
                    if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
                        arrObj[i].checked = true;
                    }
                }
            } else {
                for (var i = 0; i < arrObj.length; i++) {
                    if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox')
                        arrObj[i].checked = false;
                }
            }
        } 
</script>
    <link href="Style/Table.css" rel="stylesheet" type="text/css" />
    <link href="Style/Table0.css" rel="stylesheet" type="text/css" />
		<base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
