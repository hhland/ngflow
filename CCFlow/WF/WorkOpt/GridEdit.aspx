<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GridEdit.aspx.cs" Inherits="CCFlow.WF.WorkOpt.GridEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/CreateControl.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OnOpenReport() {
            // Set up  DefaultAction  Property  false, The default implementation of the control itself is not open behavior 
            ReportDesigner.DefaultAction = false;
 
            var LoadURL  =location.href+"&method=load";
            var success = ReportDesigner.Report.LoadFromURL(encodeURI(LoadURL));
            if (success == true) {
                ReportDesigner.Reload();
            }
            else {
                alert(" Failed to load report !");
            }
        }

        function OnSaveReport() {
            // Set up  DefaultAction  Property  false, The default behavior of the control is not performed to save itself 
            ReportDesigner.DefaultAction = false;

            ReportDesigner.Post();  // Designer will submit the data to the report design object 

            var LoadURL = location.href+"&method=save";
            var success = ReportDesigner.Report.SaveToURL(encodeURI(LoadURL));
            if (success)
                alert(" Save the report success !");
            else
                alert(" Failed to save the report !");
        }
    </script>
</head>
<body style="margin:0px;height:800px">
    <script type="text/javascript">
//        var file = "../../DataUser/CyclostyleFile/<%=Request.QueryString["grf"] %>";
        // Modify a report , After the completion of the report design , Save the report in web On the server 
        // The first two parameters are loaded and saved in the specified template URL,
        // The third parameter specifies the report data URL, In order to load data at design time to see the effects in time 
        // Here you do not specify any parameters ,在 OpenReport 与 SaveReport  The specific processing parameters 
        CreateDesignerEx("100%", "100%", "", "", "",  "<param name='OnOpenReport' value='OnOpenReport'><param name='OnSaveReport' value='OnSaveReport'>");
    </script>
</body>
</html>
