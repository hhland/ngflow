<%@ Page Language="C#" AutoEventWireup="true" Inherits="AppDemo_Lef1t" Codebehind="Left.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Navigation </title>
    <link href="css/basic.css" rel="stylesheet" type="text/css" />
    <link href="css/base.css" rel="stylesheet" type="text/css" />
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <script language="JavaScript">
        function myrefresh() {
            window.location.reload();
        }
        setTimeout('myrefresh()', 100000); // Designation 1 Second refresh  
   </script> 
</head>
<body>
    <div class="JS_zhut">
        <div class="JS_zhut_n">
            <div  class="left fl">
                <ul id="Ul1" class="menu" runat="server">
                    <li  runat="server" id="Li2s" style=" background:url(Img/Menu/Start.jpg) -1px 0px"><a  href="/WF/Start.aspx" target="main"> Launch </a></li>
                    <li runat="server" id="get" style=" background:url(Img/Menu/EmpWorks.jpg) -1px 0px"><a  href="/WF/EmpWorks.aspx" target="main"> Upcoming  (<%= BP.WF.Dev2Interface.Todolist_EmpWorks %>)</a></li>
                    <li runat="server" id="Li1" style=" background:url('Img/Menu/Ruing.jpg') -1px 0px"><a  href="/WF/Runing.aspx" target="main"> In transit (<%= BP.WF.Dev2Interface.Todolist_Runing %>)</a></li>
                    <li runat="server" id="Li3" style=" background:url(Img/Menu/CC.jpg) -1px 0px"><a  href="/WF/CC.aspx" target="main"> Cc  (<%= BP.WF.Dev2Interface.Todolist_CCWorks %>)</a></li>
                    <li runat="server" id="Li7" style=" background:url(Img/Menu/Batch.jpg) -1px 0px"><a  href="/WF/Batch.aspx" target="main"> Batch </a></li>
                    <% if (BP.WF.Glo.IsEnableTaskPool)
                       { %>
                    <li runat="server" id="Li8" style=" background:url(Img/Menu/Sharing.jpg) -1px 0px"><a  href="/WF/TaskPoolSharing.aspx" target="main"> Shared mission </a></li>
                    <%} %>

                    <li runat="server" id="Li5" style=" background:url(Img/Menu/HungUp.jpg) -1px 0px"><a  href="/WF/HungUpList.aspx" target="main"> Pending  (<%= BP.WF.Dev2Interface.Todolist_HungUpNum%>)</a></li>
                    <li runat="server" id="over" style=" background:url(Img/Menu/Search.jpg) -1px 0px"><a  href="/WF/FlowSearch.aspx" target="main"> Inquiry  </a></li>
                    <%--<li runat="server" id="cld" style=" background:url(Img/Menu/Rili.jpg) -1px 0px"><a  href="../WF/Calendar.aspx" target="main"> Calendar </a></li>--%>
                    <li runat="server" id="Li2" style=" background:url(Img/Menu/GetTask.jpg) -1px 0px"><a  href="/WF/GetTask.aspx" target="main"> Retrieve approval </a></li>
                    <li runat="server" id="Li6" style=" background:url(Img/Menu/Emps.jpg) -1px 0px"><a  href="/WF/Emps.aspx" target="main"> Member  </a></li>
                    <li runat="server" id="end" style=" background:url(Img/Menu/Setting.jpg) -1px 0px"><a  href="/WF/Tools.aspx" target="main"> Set up  </a></li>
                    <li runat="server" id="Li4" style=" background:url(Img/Menu/Setting.jpg) -1px 0px"><a  href="/WF/Manager/Tools.aspx" target="main"> Set up UI </a></li>
                </ul>
            </div>
        </div>
    </div>
</body>
</html>

