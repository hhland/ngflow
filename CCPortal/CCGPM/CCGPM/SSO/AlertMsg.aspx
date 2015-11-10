<%@ Page Language="C#" AutoEventWireup="true" Inherits="SSO_AlertMsg" Codebehind="AlertMsg.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <ul>
        <%foreach (BP.GPM.InfoPush pl in InfoPushs)
          {%>
        <li><a href="<%=pl.Url %>">
            <img alt="" src="<%=pl.ICON%>" width="16px" height="16px" border="0" />
            <%=pl.Name%>(<%=GetNum(pl)%>)</a>&nbsp;&nbsp;</li>
        <% } %>
    </ul>
    </form>
</body>
</html>
