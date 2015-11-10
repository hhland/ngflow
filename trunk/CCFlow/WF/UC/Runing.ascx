<%@ Control Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.UC.Runing" Codebehind="Runing.ascx.cs" %>
<%@ Register src="./../Comm/UC/ToolBar.ascx" tagname="ToolBar" tagprefix="uc2" %>
    <%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
    <script   type="text/javascript">
        //  Revocation .
        function UnSend(appPath, pageID, fid, workid, fk_flow) {
            if (window.confirm(' Are you sure you want to cancel this transmission ?') == false)
                return;
            var url = appPath + 'WF/Do.aspx?DoType=UnSend&FID=' + fid + '&WorkID=' + workid + '&FK_Flow=' + fk_flow + '&PageID=' + pageID;
            window.location.href = url;
            return;
        }
        function Press(appPath, fid, workid, fk_flow) {
            var url = appPath+'WF/WorkOpt/Press.aspx?FID=' + fid + '&WorkID=' + workid + '&FK_Flow=' + fk_flow;
            var v = window.showModalDialog(url, 'sd', 'dialogHeight: 200px; dialogWidth: 350px;center: yes; help: no');
        }

        function GroupBarClick(appPath,rowIdx) {
            var alt = document.getElementById('Img' + rowIdx).alert;
            var sta = 'block';
            if (alt == 'Max') {
                sta = 'block';
                alt = 'Min';
            } else {
                sta = 'none';
                alt = 'Max';
            }
            
            document.getElementById('Img' + rowIdx).src = appPath+'WF/Img/' + alt + '.gif';
            document.getElementById('Img' + rowIdx).alert = alt;
            var i = 0;
            for (i = 0; i <= 5000; i++) {
                if (document.getElementById(rowIdx + '_' + i) == null)
                    continue;
                if (sta == 'block') {
                    document.getElementById(rowIdx + '_' + i).style.display = '';
                } else {
                    document.getElementById(rowIdx + '_' + i).style.display = sta;
                }
            
            }
        }
    </script>
     <style>
        .TTD
        {
          word-wrap: break-word; 
      　　word-break: normal; 
        }
        .Icon
{
    width:16px;
    height:16px;
}
</style>
<uc1:Pub ID="Pub1" runat="server" />

