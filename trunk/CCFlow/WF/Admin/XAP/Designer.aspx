<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.Admin.XAP.Designer" Codebehind="Designer.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title> NMP Workflow Engine Designer </title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    width : 100%;
	    overflow:hidden;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    width: 100%;
	    text-align:center; 
    }

   </style>

    <script src="../../../Silverlight.js" type="text/javascript"></script>
    <script type="text/javascript">
        var slver = "5.1.30514.0";
        

        function checkSL() {
            var isinstalled = Silverlight.isInstalled(slver);
            if (!isinstalled) {
                document.getElementById("silverlightControlHost").style.display = "none";
                document.getElementById("silverlightError").style.display = "inline";
            }
        }

        function maximizeWindow() {
            window.moveTo(0, 0);
            window.resizeTo(screen.width, window.screen.availHeight);

            checkSL();
            
            
        }
        function GetBrowserWidth() {
            return window.screen.width;
        }
        function GetBrowserHeight() {
            return window.screen.height;
        }

      
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Silverlight  Application unhandled error  " + appSource + "\n";

            errMsg += " Code : " + iErrorCode + "    \n";
            errMsg += " Category : " + errorType + "       \n";
            errMsg += " News : " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += " File : " + args.xamlFile + "     \n";
                errMsg += "行: " + args.lineNumber + "     \n";
                errMsg += " Location : " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "行: " + args.lineNumber + "     \n";
                    errMsg += " Location : " + args.charPosition + "     \n";
                }
                errMsg += " Method name : " + args.methodName + "     \n";
            }
            alert(errMsg);
        }
  

</script>
</head>
<body onload="javascript:maximizeWindow()">
    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
		<object data="data:application/x-silverlight" type="application/x-silverlight-2" width="100%" height="100%">
			<param name="source" value="../../../ClientBin/CCFlowDesigner.xap"/>
			<param name="onerror" value="onSilverlightError" />
			<param name="background" value="white" />
            <param name="windowless" value="false" />
            <param name="AllowHtmlPopupWindow" value="true" />
			<param name="minRuntimeVersion" value="5.1.30514.0" />
			<param name="autoUpgrade" value="true" />
            <param name="InitParams" value="design_userno=admin,meysam.azizian,ahmadreza.ghetmiri"/>
			<a href="<%=BP.WF.Glo.SilverlightDownloadUrl %>">" style="text-decoration: none;">
     			<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>
			</a>
		</object>
    </div>
    <div id="silverlightError" style="display:none">
        <a href="<%=BP.WF.Glo.SilverlightDownloadUrl %>">" style="text-decoration: none;">
     			<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>
			</a>
    </div>
    <iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
    </form>
</body>
</html>
