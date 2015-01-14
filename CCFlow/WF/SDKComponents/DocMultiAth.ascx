<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocMultiAth.ascx.cs" Inherits="CCFlow.WF.App.Comm.DocMultiAth" %>
<link href="CSSTab.css" rel="stylesheet" type="text/css" />
<%
    string enName = "ND" + this.Request.QueryString["FK_Node"];
    BP.Sys.FrmWorkCheck fwc = new BP.Sys.FrmWorkCheck();
    string src = "/WF/CCForm/AttachmentUpload.aspx?FID=" + this.Request.QueryString["FID"];
    src += "&WorkID=" + this.Request.QueryString["WorkID"];
    src += "&FK_Node=" + this.Request.QueryString["FK_Node"];
    src += "&FK_Flow=" + this.Request.QueryString["FK_Flow"];
    src += "&FK_FrmAttachment=ND" + this.Request.QueryString["FK_Node"] + "_DocMultiAth";
    src += "&RefPKVal=" + this.Request.QueryString["WorkID"];
    src += "&PKVal=" + this.Request.QueryString["WorkID"];
    src += "&Ath=DocMultiAth" ;
    src += "&FK_MapData=" + enName;
    src += "&Paras=" + this.Request.QueryString["Paras"];
%>
<script type="text/javascript">

   
    function HideDivFuJian() {
        var oTb = document.getElementById("fujian");
        if (oTb.rows[1].style.display == "none") {
            for (var i = 1; i < oTb.rows.length; i++) {
                oTb.rows[i].style.display = "";
            }
            $('#fujianTitle').html("<b class='bt'> Upload attachment &nbsp;（ Hide ）</b>");

        } else {
            for (var i = 1; i < oTb.rows.length; i++) {
                oTb.rows[i].style.display = "none";
            }
            $('#fujianTitle').html("<b class='bt'> Upload attachment &nbsp;（ Show ）</b>");
        }
    }
</script>
<div class="mainTab">
<table width="100%" border="0"cellspacing="4"  cellpadding="0" class="Tab" id="fujian" >
 <%--   <tr>
        <td class="tit" id="fujianTitle" onclick="HideDivFuJian()">
            <b> Upload attachment &nbsp;（ Hide ）</b>
        </td>
    </tr>--%>
    <tr>
        <td>
            <iframe id='F332222' src='<%=src%>' frameborder="0" style='padding: 0px; border: 0px;'
                    leftmargin='0' topmargin='0' width='100%' style="align: center" height="150px"
                    scrolling="auto"></iframe>
        </td>
    </tr>
</table>
</div>
