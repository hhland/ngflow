<%@Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.Default" Codebehind="Default.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title> Welcome ccflow</title>
    <base target="_blank" />
    <style type="text/css">
        .style1
        {
            text-decoration: underline;
        }
    </style>
    <script language="javascript" type="text/javascript">
		    function toHome() {
		        if (window.location.href.tolower().indexof('wap') != -1)
		            window.location.href = './WF/Login.aspx';
		        else
		            window.location.href = './WF/WAP/Login.aspx';
		    }
		</script>
</head>
<body onload>
    <form id="form1" runat="server">
    <div>
        
         <fieldset style="vertical-align:middle; width:80%; text-align:left;">
    <legend><h3><asp:Localize  runat="server"  Text="<%$ Resources:Title,welcome.Text %>" ></asp:Localize></h3></legend>
    <div style="float:left;" > 
    <ul>
    <li> <a href="Default.aspx?IsCheckUpdate=1&IsLogin=1" > <asp:Localize runat="server"  Text="<%$ Resources:Link,loginUpdate.Text%>" ></asp:Localize></a></li>
    <li> <a href="Default.aspx?IsCheckUpdate=0&IsLogin=1" > <asp:Localize runat="server" Text="<%$ Resources:Link,loginNormal.Text%>" ></asp:Localize></a></li>
    </ul>
    </div>

    <div style="float:left;" > 
    <ul>
    <li> <a href='./AppDemo/Login.aspx' > <asp:Localize runat="server" Text="<%$ Resources:Link,loginApp.Text%>"></asp:Localize></a></li>
    <li> <a href='./WF/Login.aspx' > <asp:Localize runat="server"   Text="<%$ Resources:Link,loginBlog.Text%>"></asp:Localize></a></li>
    </ul>
    </div>
       
    <div style="float:left;" > 
    <ul>
    <li> <asp:HyperLink runat="server" 
         NavigateUrl="<%$ Resources:Link,website.Href %>" Target="_blank"
        Text="<%$ Resources:Link,website.Text %>"></asp:HyperLink></li>
    <li> <asp:HyperLink runat="server" Target="_blank" NavigateUrl="<%$ Resources:Link,forum.Href %>" Text="<%$ Resources:Link,forum.Text%>"></asp:HyperLink></li>
    </ul>
    </div>

    </fieldset>
    <div style=" vertical-align:middle; text-align:center" > <asp:Localize runat="server" Text="<%$ Resources:Title,co.Text%>"></asp:Localize></div>

    </div>
    
    </form>
    <form id="formCulture" action="Default.aspx" method="get"  target="_self">
        <select name="culture"  onchange="document.getElementById('formCulture').submit();" >
            <option value=""  selected="selected">select a langueage...</option>
            <option value="en-us">English </option>
            <option value="zh-cn">中文</option>
            <option value="ko">한국어</option>
        </select>
    </form>
    
</body>
</html>
