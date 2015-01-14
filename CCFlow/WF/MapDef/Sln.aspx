<%@ Page Title=" Form Permissions " Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true" CodeBehind="Sln.aspx.cs" Inherits="CCFlow.WF.MapDef.Sln" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function DelSln(fk_mapdata,fk_flow, fk_node, keyofen) {
        var url = 'SlnDo.aspx?DoType=DelSln&FK_MapData=' + fk_mapdata + '&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&KeyOfEn=' + keyofen;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function EditSln(fk_mapdata,fk_flow,fk_node, keyofen) {
        var url = 'SlnDo.aspx?DoType=EditSln&FK_MapData=' + fk_mapdata + '&FK_Flow=' + fk_flow + '&FK_Node=' + fk_node + '&KeyOfEn=' + keyofen;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function SetSln(fk_mapdata) {
        var url = 'EditMapData.aspx?DoType=EditSln&FK_MapData=' + fk_mapdata ;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }
    function CopyIt(fk_mapdata,fk_flow, nodeID) {
        var url = 'SlnDo.aspx?DoType=Copy&FK_MapData=' + fk_mapdata + '&FK_Flow=' + fk_flow + '&FK_Node=' + nodeID;
        var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 700px;center: yes; help: no');
        window.location.href = window.location.href;
    }

    var IsNoteNull=false;
    function CheckAll(idstr) {
        var arrObj = document.all;
        IsNoteNull = !IsNoteNull;
        for (var i = 0; i < arrObj.length; i++) {

            if (arrObj[i].type != 'checkbox')
                continue;

            var cid = arrObj[i].name;
            if (cid == null || cid == "" || cid == '')
                continue;

            

            if (cid.indexOf(idstr) == -1)
                continue;

            arrObj[i].checked = IsNoteNull;
            //  !arrObj[i].checked;
        }
    }

    var IsEnable = false;
    function CheckAllIsEnable(idstr) {
        var arrObj = document.all;
        IsEnable = !IsEnable;
        for (var i = 0; i < arrObj.length; i++) {

            if (arrObj[i].type != 'checkbox')
                continue;

            var cid = arrObj[i].name;
            if (cid == null || cid == "" || cid == '')
                continue;

            if (cid.indexOf(idstr) == -1)
                continue;

            arrObj[i].checked = IsEnable;
            //  !arrObj[i].checked;
        }
    }

    // Edit attachments 
    function EditFJ(fk_node,fk_mapdata,ath) {
        WinShowModalDialog('Attachment.aspx?FK_Node=' + fk_node + '&FK_MapData=' + fk_mapdata + '&Ath=' + ath, 'ss');
        window.location.href = window.location.href;
    }
    // Remove attachment .
    function DeleteFJ(fk_flow, fk_node, fk_mapdata, ath) {

        if (confirm(' Are you sure you want to delete it ?') == false)
            return;

        var url = 'Sln.aspx?DoType=DeleteFJ&FK_Flow='+fk_flow+'&FK_Node=' + fk_node + '&FK_MapData=' + fk_mapdata + '&Ath=' + ath;
        alert(url);
        WinShowModalDialog(url, 'ss');
        window.location.href = window.location.href;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Pub ID="Pub2" runat="server" />
</asp:Content>
