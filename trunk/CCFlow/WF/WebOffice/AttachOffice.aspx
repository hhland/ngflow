﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AttachOffice.aspx.cs" Inherits="CCFlow.WF.WebOffice.AttachOffice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Accessories editor </title>
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <script src="../Comm/JScript.js" type="text/javascript"></script>
    <style type="text/css">
        .btn
        {
            border: 0;
            background: #4D77A7;
            color: #FFF;
            font-size: 12px;
            padding: 6px 10px;
            margin: 5px;
        }
    </style>
    <script type="text/javascript">
        var isShowAll = false;
        var webOffice = null;
        var strTimeKey;
        var isOpen = false;
        var isInfo = false;
        var marksType = "doc,docx";

        $(function () {
            InitOffice();
        });
        // Initialization document 
        function InitOffice() {
            webOffice = document.all.WebOffice1;

            if ($('#fileName').val() != "") {
                OpenWeb("1");
            }
            EnableMenu();
        }
        // Set the current operation of the user 
        function SetUsers() {
            try {
                webOffice.SetCurrUserName("<%=UserName %>");

                InitShowName();

            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }
        // Show traces of the specified user 
        function ShowUserName() {
            /// <summary>
            ///  Significant traces of the current user 
            /// </summary>


            try {

                var user = $("#marks option:selected").val();

                if (user == " Whole " || user == undefined) {
                    if (isShowAll) {
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                        isShowAll = false;
                    }

                } else {
                    //                    if (!isShowAll) {
                    //                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    //                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    //                        isShowAll = true;
                    //                    }
                    if (!isShowAll) {
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                        isShowAll = true
                    }
                    else {
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    }


                    webOffice.GetDocumentObject().Application.ActiveWindow.View.Reviewers(user).Visible = true;
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }
        // Loading the traces of user 
        function InitShowName() {
            try {
                var count = webOffice.GetRevCount();

                var showName = $("#marks");
                showName.empty();

                var list = " Whole ,";

                //GetRevInfo(i,int) int=1  Acquisition time   int=3  Get Content   int=0  Get the name 
                for (var i = 1; i <= count; i++) {
                    var strOpt = webOffice.GetRevInfo(i, 0);

                    if (list.indexOf(strOpt) < 0) {
                        list += strOpt + ",";
                    }
                }
                var data = list.split(',');
                for (var i = 0; i < data.length; i++) {

                    if (data[i] != null && data[i] != "") {
                        var option = $("<option>").text(data[i]).val(data[i]);
                        showName.append(option);
                    }
                }

            } catch (e) {

            }
        }

        // Hide   Document button 
        function EnableMenu() {
            /// <summary>
            ///  Settings button 
            /// </summary>
            webOffice.HideMenuItem(0x01 + 0x02 + 0x04 + 0x10 + 0x20);
        }
        // Setting traces , Show all traces of user , Whether read-only document 
        function SetTrack(track) {
            if ("<%=ReadOnly %>" == "True") {
                webOffice.ProtectDoc(1, 2, "");
            }
            else {
                webOffice.ProtectDoc(0, 1, "");
            }
            webOffice.SetTrackRevisions(track);

            SetUsers();
            var types = $('#fileType').val();
            var type = webOffice.IsOpened();
            // If you open the word
            if (type == 11) {
                ShowUserName();
            }
        }
        // Open local files 
        function OpenFile() {
            pageLoadding(' Opening ...');
            try {
                if (readOnly()) {
                    return false;
                }

                OpenDoc("open", "doc");
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            loaddingOut(' Open to complete ');
            return false;
        }
        function OpenTempLate() {
            if (readOnly()) {
                return false;
            }
            LoadTemplate('word', ' Document Templates ', OpenWeb);

            return false;
        }
        // Printing documents 
        function printOffice() {
            try {
                if (isOpen) {
                    webOffice.PrintDoc(1);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            return false;
        }

        function pageLoadding(msg) {
            $.jBox.tip(msg, 'loading');
        }

        function loaddingOut(msg) {
            $.jBox.tip(msg, 'success');

        }
        function DownLoad() {
            try {
                if (isOpen) {
                    webOffice.ProtectDoc(0, 1, "");
                    webOffice.ShowDialog(84);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            webOffice.ProtectDoc(1, 2, "");
            return false;
        }
        // Open the server files 
        function OpenWeb(loadtype) {
            pageLoadding(' Opening Template ...');
            try {
                var type = $("#fileType").val();
                var fileName = $('#fileName').val();
                var url = location.href + "&action=LoadFile&LoadType=" + loadtype + "&fileName=" + fileName;
                OpenDoc(url, type);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            loaddingOut(' Open to complete .');

        }
        // Open the file 
        function OpenDoc(url, type) {
            var openType = webOffice.LoadOriginalFile(url, type);
            if (openType > 0) {

                if ("<%=IsMarks %>" == "True")
                    SetTrack(1);
                else
                    SetTrack(0);

                isOpen = true;
            } else {
                $.jBox.alert(' Failed to open document ', ' Abnormal ');
            }
        }
        // Load Template 
        function LoadTemplate(type, title, callback) {
            try {
                $.jBox("iframe:/WF/WebOffice/TempLate.aspx?LoadType=" + type, {
                    title: title,
                    width: 800,
                    height: 350,
                    buttons: { ' Determine ': 'ok' },
                    submit: function (v, h, f) {
                        var row = h[0].firstChild.contentWindow.getSelected();
                        if (row == null) {
                            $.jBox.info(' Please select a template ');
                            return false;
                        } else {
                            pageLoadding(' Open the ...');
                            $("#fileName").val(row.Name);
                            $("#fileType").val(row.Type);
                            callback();
                            loaddingOut(' Open to complete ...');

                            return true;
                        }
                    }
                });
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }
        // Load Template red tape 
        function overOffice() {
            if (readOnly()) {
                return false;
            }
            if (isOpen) {

                LoadTemplate('over', ' Tao Hong template ', TaoHong);
            } else {
                $.jBox.alert(' Open document !', ' Prompt ');
            }
            return false;
        }
        // Sets of red tape 
        function TaoHong() {
            try {

                var mark = AddBooks();
                var type = $('#fileType').val();
                var name = $('#fileName').val();

                if (type == "png" || type == "jpg" || type == "bmp") {
                    webOffice.SetFieldValue(mark, window.location.protocol + "//" + window.location.host + "/DataUser/OfficeOverTemplate/" + name, "::JPG::");
                } else {
                    webOffice.SetFieldValue(mark, window.location.protocol + "//" + window.location.host + "/DataUser/OfficeOverTemplate/" + name, "::FILE::");
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }
        // Saved to the server 
        function saveOffice() {
            if (readOnly()) {
                return false;
            }
            var type = webOffice.IsOpened();

            pageLoadding(' Saving ...');
            try {
                if (isOpen) {
                    if (type == 11) {
                        if ("<%=IsCheck %>" == "True") {
                            if (isInfo) {
                                isInfo = false;
                                webOffice.GetDocumentObject().Application.ActiveDocument.Content.InsertAfter("\n<%=NodeInfo %>");
                            }

                        }
                    }
                    webOffice.HttpInit();
                    webOffice.HttpAddPostCurrFile("File", "");
                    var src = location.href + "&action=SaveFile";

                    webOffice.HttpPost(src);


                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');

                }
            } catch (e) {

                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            loaddingOut(' Save to finish ...');
            var types = $('#fileType').val();
            try {
                // If you open the word
                if (type == 11) {
                    InitShowName();
                    if (isShowAll) {
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                        isShowAll = false;
                    } else {
                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    }
                    ShowUserName();
                }
            } catch (e) {

            }
            return false;
        }
        // Reject Changes 
        function refuseOffice() {
            try {
                if (readOnly()) {
                    return false;
                }
                var vCount = webOffice.GetRevCount();
                var strUserName;
                for (var i = 1; i <= vCount; i++) {
                    strUserName = webOffice.GetRevInfo(i, 0);
                    webOffice.AcceptRevision(strUserName, 1);
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');

            }
            return false;

        }
        // Accept Change 
        function acceptOffice() {
            try {
                if (readOnly()) {
                    return false;
                }
                webOffice.SetTrackRevisions(4);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');

            }
            return false;
        }
        // Document read-only tips 
        function readOnly() {
            if ("<%=ReadOnly %>" == "True") {
                $.jBox.alert(' This document can not be read-only !', ' Prompt ');
                return true;
            }
        }
        // Seal all documents loaded 
        function sealOffice() {

            if (readOnly()) {
                return false;
            }
            if (isOpen) {
                LoadTemplate('seal', ' Official stamp ', seal);
            } else {
                $.jBox.alert(' Open document !', ' Prompt ');
            }
            return false;
        }
        // Stamp 
        function seal() {
            try {
                var name = $('#fileName').val();
                var type = $('#fileType').val();
                var url = window.location.protocol + "//" + window.location.host + "/DataUser/OfficeSeal/" + name;


                var mark = AddBooks();
                //webOffice.InSertFile(url, 8);
                AddPicture(mark, url, 5);

            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');

            }
        }
        // Add a bookmark 
        function AddBooks() {

            var date = new Date().getFullYear() + "" + new Date().getMonth() + "" + new Date().getDay() + "" + new Date().getHours() + "" + new Date().getMinutes() + "" + new Date().getSeconds();
            var strMarkName = "mark_" + date;
            var obj = new Object(webOffice.GetDocumentObject());
            if (obj != null) {
                var pBookM;
                var pBookMarks;
                // VAB Get bookmark collection interface 
                pBookMarks = obj.Bookmarks;
                try {
                    pBookM = pBookMarks(strMarkName);
                    return pBookM.Name;
                } catch (e) {
                    webOffice.SetFieldValue(strMarkName, "", "::ADDMARK::");
                }
            }
            return strMarkName;
        }
        // By VBA  To insert a picture 
        function AddPicture(strMarkName, strBmpPath, vType) {
            // Definition of an object , Used to store ActiveDocument Object 
            var obj = new Object(webOffice.GetDocumentObject());
            if (obj != null) {
                var pBookMarks;
                // VAB Get bookmark collection interface 
                pBookMarks = obj.Bookmarks;
                var pBookM;
                // VAB Interface to obtain bookmark strMarkName
                pBookM = pBookMarks(strMarkName);
                var pRange;
                // VAB Interface to obtain bookmark strMarkName的Range Object 
                pRange = pBookM.Range;
                var pRangeInlines;
                // VAB Interface to obtain bookmark strMarkName的Range Object InlineShapes Object 
                pRangeInlines = pRange.InlineShapes;
                var pRangeInline;
                // VAB Interfaces InlineShapes Insert pictures into a document object 
                pRangeInline = pRangeInlines.AddPicture(strBmpPath, 128);
                // Picture Style ,5 Floating in the text above 
                pRangeInline.ConvertToShape().WrapFormat.TYPE = vType;
                delete obj;
            }
        }
        /// Insert File Test 
        function InsertFileWeb() {
            var url = window.location.protocol + "//" + window.location.host + "/DataUser/OfficeFile/099/112.docx";
            webOffice.LoadOriginalFile(url, "docx");
        }

        function closeDoc() {
            webOffice.SetCurrUserName("");
            webOffice.closeDoc(0);
        }
    </script>
</head>
<body onunload="closeDoc()" style="padding: 0px; margin: 0px;">
    <form id="form1" runat="server">
    <div id="divMenu" runat="server" style="padding: 0px; margin: 0px;">
    </div>
    <div style="display: none">
        <asp:TextBox ID="fileName" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="fileType" runat="server" Text=""></asp:TextBox>
    </div>
    <div style="padding: 0px; margin: 0px; position: fixed; height: 600px; width: 100%">
        <%--  <object id="WebOffice1" height="100%" width='100%' style='left: 0px; top: 0px; position: fixed;'
            classid='clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5' codebase='/WF/Activex/WebOffice.cab#V7.0.0.8'>
            <param name='_ExtentX' value='6350'>
            <param name='_ExtentY' value='6350'>
        </object>--%>
        <script src="../Scripts/LoadWebOffice.js" type="text/javascript"></script>
    </div>
    </form>
</body>
</html>
