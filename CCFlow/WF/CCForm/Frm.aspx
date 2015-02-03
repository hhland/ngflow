<%@ Page Title="" Language="C#" MasterPageFile="WinOpen.master" AutoEventWireup="true"
    Inherits="CCFlow.WF.CCForm.WF_Frm" CodeBehind="Frm.aspx.cs" %>

<%@ Register Src="../UC/UCEn.ascx" TagName="UCEn" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="JavaScript" src="../Comm/JScript.js" type="text/javascript"></script>
    <script language="JavaScript" src="../Comm/JS/Calendar/WdatePicker.js" defer="defer"
        type="text/javascript"></script>
    <script language="JavaScript" src="MapExt.js" type="text/javascript"></script>
    <script language='JavaScript' src='../Scripts/jquery-1.4.1.min.js' type="text/javascript"></script>
    <link href="../Style/FormThemes/CCFormFrm.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">

        $(function () {
            //SetHeight();
        });

        function SetHeight() {
            var screenHeight = document.documentElement.clientHeight;

            var frmHeight = "<%=Height %>";
            if (screenHeight > parseFloat(frmHeight)) {
                $("#divCCForm").height(screenHeight);

            }
        }
        $(window).resize(function () {
            //SetHeight();
        });

        

        //  Get DDL值
        function ReqDDL(ddlID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_DDL_' + ddlID).value;
            if (v == null) {
                alert(' Not found ID=' + ddlID + ' The drop-down box control .');
            }
            return v;
        }

        //  Get TB值
        function ReqTB(tbID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_TB_' + tbID).value;
            if (v == null) {
                alert(' Not found ID=' + tbID + ' The text box control .');
            }
            return v;
        }

        //  Get CheckBox值
        function ReqCB(cbID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_CB_' + cbID).value;
            if (v == null) {
                alert(' Not found ID=' + cbID + ' The text box control .');
            }
            return v;
        }

        ///  Get DDL Obj
        function ReqDDLObj(ddlID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_DDL_' + ddlID);
            if (v == null) {
                alert(' Not found ID=' + ddlID + ' The drop-down box control .');
            }
            return v;
        }
        //  Get TB Obj
        function ReqTBObj(tbID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_TB_' + tbID);
            if (v == null) {
                alert(' Not found ID=' + tbID + ' The text box control .');
            }
            return v;
        }
        //  Get CheckBox Obj值
        function ReqCBObj(cbID) {
            var v = document.getElementById('ContentPlaceHolder1_UCEn1_CB_' + cbID);
            if (v == null) {
                alert(' Not found ID=' + cbID + ' Radio controls .');
            }
            return v;
        }

        function ReqTBIsexist(tbIDs) {
            var url = "/WF/CCForm/Frm.ashx";
            var param = {FK_MapData:"<%=Request.Params["FK_MapData"] %>",
            FID:"<%=Request.Params["FID"] %>"
            ,action:"isexist"
            };
            for (var i = 0; i < tbIDs.length; i++) {
                var tbID = tbIDs[i];
                var val=ReqTB(tbID);
                param["field_" + tbID] = val;
            }
            var isexist = false;
            $.ajax({
                type: "post",
                url: url,
                cache: false,
                async: false,
                data: param,
                //dataType: ($.browser.msie) ? "text" : "xml",
                success: function (str) {
                   var json=eval("("+str+")");
                    isexist = json["isexist"];
                }
            });
            return isexist;
        }

        //  Setting .
        function SetCtrlVal(ctrlID, val) {
            document.getElementById('ContentPlaceHolder1_UCEn1_TB_' + ctrlID).value = val;
            document.getElementById('ContentPlaceHolder1_UCEn1_DDL_' + ctrlID).value = val;
            document.getElementById('ContentPlaceHolder1_UCEn1_CB_' + ctrlID).value = val;
        }

        var isFrmChange = false;
        var isChange = false;
        function SaveDtlData() {
            // Plus regular verification 
            if (SysCheckFrm() == false)
                return false;

            if (isChange == false)
                return;

            try {
                if (document.all.DWebSignSeal) {
                    var v = document.all.DWebSignSeal.GetStoreData();
                    if (v.length == "") {
                        isChange = true;
                        alert(" Must be sealed before they can submit ");
                        return false;
                    }
                    var btn = document.getElementById("<%=Btn_Save.ClientID %>");
                    var sealData = document.getElementById("<%=TB_SealData.ClientID %>");
                    sealData.value = v;
                }
            } catch (e) {
            }

            var btn = document.getElementById("<%=Btn_Save.ClientID %>");
            if (btn) {
                btn.click();
                isChange = false;
            }
        }

        function Change(id) {
            isChange = true;
            var tagElement = window.parent.document.getElementById("HL" + id);
            if (tagElement) {
                var tabText = tagElement.innerText;
                var lastChar = tabText.substring(tabText.length - 1, tabText.length);
                if (lastChar != "*") {
                    tagElement.innerHTML = tagElement.innerText + '*';
                }
            }

            if (typeof self.parent.TabFormExists != 'undefined') {
                var bExists = self.parent.TabFormExists();
                if (bExists) {
                    self.parent.ChangTabFormTitle();
                }
            }
        }

        function TROver(ctrl) {
            ctrl.style.backgroundColor = 'LightSteelBlue';
        }

        function TROut(ctrl) {
            ctrl.style.backgroundColor = 'white';
        }
        function Del(id, ens, refPk, pageIdx) {
            if (window.confirm(' Are you sure you want to delete it ?') == false)
                return;

            var url = 'Do.aspx?DoType=DelDtl&OID=' + id + '&EnsName=' + ens;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = 'Dtl.aspx?EnsName=' + ens + '&RefPKVal=' + refPk + '&PageIdx=' + pageIdx;
        }
        function DtlOpt(workId, fk_mapdtl) {
            var url = 'DtlOpt.aspx?WorkID=' + workId + '&FK_MapDtl=' + fk_mapdtl;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = 'Dtl.aspx?EnsName=' + fk_mapdtl + '&RefPKVal=' + workId;
        }
        function OnKeyPress() {
        }
        function OpenOfiice(fk_ath, pkVal, delPKVal, FK_MapData, NoOfObj, FK_Node) {
            var date = new Date();
            var t = date.getFullYear() + "" + date.getMonth() + "" + date.getDay() + "" + date.getHours() + "" + date.getMinutes() + "" + date.getSeconds();

            var url = '../WebOffice/AttachOffice.aspx?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal + "&FK_MapData=" + FK_MapData + "&NoOfObj=" + NoOfObj + "&FK_Node=" + FK_Node + "&T=" + t;
            //var url = 'WebOffice.aspx?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal;
            // var str = window.showModalDialog(url, '', 'dialogHeight: 1250px; dialogWidth:900px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
            //var str = window.open(url, '', 'dialogHeight: 1200px; dialogWidth:1110px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
            window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');

        }
        function ReinitIframe(frmID, tdID) {
            try {

                var iframe = document.getElementById(frmID);
                var tdF = document.getElementById(tdID);

                iframe.height = iframe.contentWindow.document.body.scrollHeight;
                iframe.width = iframe.contentWindow.document.body.scrollWidth;

                if (tdF.width < iframe.width) {
                    //alert(tdF.width +'  ' + iframe.width);
                    tdF.width = iframe.width;
                } else {
                    iframe.width = tdF.width;
                }

                tdF.height = iframe.height;
                return;

            } catch (ex) {

                return;
            }
            return;
        }
        function OnLoadCheckFrmSlnIsNull() {
            $('.HBtn').hide();
            if (typeof CheckFrmSlnIsNull != 'undefined' && CheckFrmSlnIsNull instanceof Function) {
                CheckFrmSlnIsNull(); //if (CheckFrmSlnIsNull() == false) return false;
            }




            try {
                if (document.all.DWebSignSeal) {
                    var sealData = document.getElementById("<%=TB_SealData.ClientID %>");
                    document.all.DWebSignSeal.SetStoreData(sealData.value);
                    document.all.DWebSignSeal.ShowWebSeals();

                }

            } catch (e) {

            }

        }
        window.onload = OnLoadCheckFrmSlnIsNull;

    </script>
    <%--  <style type="text/css">
        .HBtn
        {
            /* display:none; */
            visibility: visible;
            background-color: White;
            border: 0;
        }
        #divCCForm
        {
            position: relative !important;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div style="float: left">
            <asp:Button ID="Btn_Save" runat="server" Text="" Width="0" Height="0" CssClass="HBtn"
                Visible="false" OnClick="Btn_Save_Click" />
        </div>
        <asp:Button ID="Btn_Print" runat="server" Text=" Print " CssClass="Btn" Visible="true" />
        <uc1:UCEn ID="UCEn1" runat="server" class="uecn" />
        <asp:TextBox runat="server" ID="TB_SealData" Style="display: none"></asp:TextBox>
    </div>
</asp:Content>
