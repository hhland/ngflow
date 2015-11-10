<%@ Page language="c#" Inherits="BP.Web.Comm.FileManager" Codebehind="FileManager.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="UCSys" Src="UC/UCSys.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="BP.Web.Controls" Assembly="BP.Web.Controls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DocType HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>
			<%=BP.Sys.SystemConfig.SysName%>文件管理者</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

		<LINK href="./Style/Table.css" type="text/css" rel="stylesheet">
		<LINK href="./Style/Table0.css" type="text/css" rel="stylesheet">

		<script language="javascript">
		 function DoAction(url, msg )
         {
           if ( confirm("提示: \n \n 将要执行["+msg+"] 确认吗？" )==false )
		      return;
	        window.location.href=url;
         }
		</script>
		<base target="_self" >
	    <style type="text/css">
            .style1
            {
                width: 100%;
            }
        </style>
	    <link href="Table.css" rel="stylesheet" type="text/css" />
	</HEAD>
	<body class="Body<%=BP.Web.WebUser.Style%>"  topmargin="0" leftmargin="0">
		<form id="form1" runat="server">
		<div style=" background-color:#FFF; width:600px; margin:0 auto;height:100%">
        <table class="style1">
            <tr>
                <td>
                    <uc1:UCSys ID="Title" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:UCSys ID="UCSys1" runat="server" />
                </td>
            </tr>
        </table>
        </div>
        </form>
	</body>
</HTML>
