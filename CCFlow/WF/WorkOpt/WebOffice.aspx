<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebOffice.aspx.cs" Inherits="CCFlow.WF.WorkOpt.WebOffice"
    Async="true" %>

<%@ Register Src="../Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Text documents </title>
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
        var isInfo = true;
        var marksType = "doc,docx";

        $(function () {
            InitOffice();
        });


        // Initialization document 
        function InitOffice() {
            webOffice = document.all.WebOffice1;

            if ($('#fileName').val() != "") {

                if ('<%=IsLoadTempLate %>' == 'True')
                    OpenWeb("0");
                else
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
            LoadTemplate('word', ' Document Templates ', "File", OpenWeb);

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

            try {
                var type = $("#fileType").val();
                var fileName = $('#fileName').val();
                var url = location.href + "&action=LoadFile&LoadType=" + loadtype + "&fileName=" + fileName;
                OpenDoc(url, type);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }


        }
        // Open the file 
        function OpenDoc(url, type) {
            var openType = webOffice.LoadOriginalFile(url, type);
            if (openType > 0) {

                var type = webOffice.IsOpened();
                // If you open the word
                if (type == 11) {

                    if ("<%=IsMarks %>" == "True")
                        SetTrack(1);
                    else
                        SetTrack(0);

                    if ("<%=IsTrueTH %>" == "True" && "<%=IsTrueTHTemplate %>" != "") {
                        TaoHong();
                    }
                }


                isOpen = true;


            } else {
                $.jBox.alert(' Failed to open document ', ' Abnormal ');
            }
        }
        // Load Template 
        function LoadTemplate(type, title, loadType, callback) {
            try {
                $.jBox("iframe:/WF/WebOffice/TempLate.aspx?LoadType=" + type + "&Type=" + loadType, {
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

                            if (type == "word") {
                                $("#fileName").val(row.Name);
                                $("#fileType").val(row.Type);
                            } else {
                                $("#sealName").val(row.Name);
                                $("#sealType").val(row.Type);
                            }
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

                LoadTemplate('over', ' Tao Hong template ', "File", TaoHong);
            } else {
                $.jBox.alert(' Open document !', ' Prompt ');
            }
            return false;
        }
        // Sets of red tape 
        function TaoHong() {
            try {

                var mark = AddBooks();
                var name = $('#sealName').val();
                var type = $('#sealType').val();
                var url = window.location.protocol + "//" + window.location.host + "/DataUser/OfficeOverTemplate/" + name;

                if (type == "png" || type == "jpg" || type == "bmp") {
                    webOffice.SetFieldValue(mark, url, "::JPG::");
                } else {
                    webOffice.SetFieldValue(mark, url, "::FILE::");
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
                    // If you open the word
                    if (type == 11) {
                        if ("<%=IsCheckInfo %>" == "True") {
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
                LoadTemplate('seal', ' Official stamp ', "File", seal);
            } else {
                $.jBox.alert(' Open document !', ' Prompt ');
            }
            return false;
        }
        // Stamp 
        function seal() {
            try {
                var name = $('#sealName').val();
                var type = $('#sealType').val();
                var url = window.location.protocol + "//" + window.location.host + "/DataUser/OfficeSeal/" + name;

                //                var pRange = webOffice.GetDocumentObject().Application.ActiveCell;
                //                webOffice.GetDocumentObject().Application.ActiveSheet.Shapes.AddPicture(url, 1, 1, pRange.Left, pRange.Top, -1, -1);
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

        function InsertFlow() {
            if (readOnly()) {
                return false;
            }
            try {
                if (isOpen) {
                    LoadTemplate('flow', ' Flow chart ', "Dic", FlowInsert);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');

                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
            return false;
        }
        function FlowInsert() {
            var name = $('#sealName').val();
            var type = $('#sealType').val();
            var url = window.location.protocol + "//" + window.location.host + "/DataUser/FlowDesc/" + name + "/" + name.replace(".", "_") + ".png";
            var mark = AddBooks();
            //webOffice.InSertFile(url, 8);
            AddPicture(mark, url, 5);
        }


        function closeDoc() {
            webOffice.SetCurrUserName("");
            webOffice.closeDoc(0);
        }

        /// Set up word Margins font 
        function SettingWordFont() {
            try {
                //                var obj = new Object(webOffice.GetDocumentObject());
                //                var wdapp = new ActiveXObject("Word.Application")
                var obj = webOffice.GetDocumentObject().Application;


                obj.ActiveDocument.PageSetup.TopMargin = obj.CentimetersToPoints(parseFloat("3.7")); // On the Margins 
                obj.ActiveDocument.PageSetup.BottomMargin = obj.CentimetersToPoints(parseFloat("3.5")); // Under Margins 
                obj.ActiveDocument.PageSetup.LeftMargin = obj.CentimetersToPoints(parseFloat("2.8")); // Left Margins 
                obj.ActiveDocument.PageSetup.RightMargin = obj.CentimetersToPoints(parseFloat("2.6")); // Right margin 

                obj.Selection.Font.NameFarEast = " Italics _GB2312";
                //                obj.Selection.Font.NameAscii = "Times New Roman";
                //                obj.Selection.Font.NameOther = "Times New Roman";

                obj.Selection.Font.Name = " Italics _GB2312";
                obj.Selection.Font.Size = parseFloat("16");
                obj.Selection.Font.Bold = 0;
                obj.Selection.Font.Italic = 0;
                //                obj.Selection.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
                //                obj.Selection.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                //                obj.Selection.Font.StrikeThrough = 0; // Strikethrough 
                //                obj.Selection.Font.DoubleStrikeThrough = 0; // Double strikethrough 
                //                obj.Selection.Font.Outline = 0; // Hollow 
                //                obj.Selection.Font.Emboss = 0; // Yang Wen 
                //                obj.Selection.Font.Shadow = 0; // Shadow 
                //                obj.Selection.Font.Hidden = 0; // Hidden Text 
                //                obj.Selection.Font.SmallCaps = 0; // Small Caps 
                //                obj.Selection.Font.AllCaps = 0; // All Caps 
                //                obj.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                //                obj.Selection.Font.Engrave = 0; // Engrave 
                //                obj.Selection.Font.Superscript = 0; // Superscript 
                //                obj.Selection.Font.Subscript = 0; // Subscript 
                //                obj.Selection.Font.Spacing = float.Parse("0"); // Character spacing 
                //                obj.Selection.Font.Scaling = 100; // Character Zoom 
                //                obj.Selection.Font.Position = 0; // Location 
                //                obj.Selection.Font.Kerning = float.Parse("1"); // Font spacing adjustment 
                //                obj.Selection.Font.Animation = Microsoft.Office.Interop.Word.WdAnimation.wdAnimationNone; // Text Effects 
                //                obj.Selection.Font.DisableCharacterSpaceGrid = false;
                //                obj.Selection.Font.EmphasisMark = Microsoft.Office.Interop.Word.WdEmphasisMark.wdEmphasisMarkNone;
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nError:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }

    </script>
</head>
<body style="padding: 0px; margin: 0px;" onunload="closeDoc()">
    <form id="form1" runat="server">
    <div id="divMenu" runat="server" style="margin: 0px; padding: 0px; width: 100%">
    </div>
    <div style="display: none;">
        <asp:TextBox ID="fileName" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="fileType" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="sealName" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="sealType" runat="server" Text=""></asp:TextBox>
    </div>
    <div style="margin: 0px; padding: 0px; height: 600px">
        <%--    <object id="WebOffice1" height="100%" width='100%' style='left: 0px; top: 0px;' classid='clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5'
            codebase='/WF/Activex/WebOffice.cab#V7.0.0.8'>
            <param name='_ExtentX' value='6350'>
            <param name='_ExtentY' value='6350'>
        </object>--%>
        <script src="../Scripts/LoadWebOffice.js" type="text/javascript"></script>
        <%--<script type="text/javascript">
            var s = "";
            if (navigator.userAgent.indexOf("MSIE") > 0) {
                s = "<OBJECT id='WebOffice1' align='middle' style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT:100%'  codebase='/WF/Activex/WebOffice.cab#V7.0.0.8'"
		+ "classid=clsid:E77E049B-23FC-4DB8-B756-60529A35FAD5>"
        + "<param name ='_ExtentX' value='6350'>"
        + "<param name ='_ExtentY' value='6350'>"
		+ "</OBJECT>";
            }

            if (navigator.userAgent.indexOf("Chrome") > 0) {
                s = "<object id='WebOffice1' type='application/x-itst-activex' align='baseline' border='0'  codebase='/WF/Activex/WebOffice.cab#V7.0.0.8'"
		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'"
		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
        + "<param name ='_ExtentX' value='6350'>"
        + "<param name ='_ExtentY' value='6350'>"
		+ "</object>";
            }

            if (navigator.userAgent.indexOf("Firefox") > 0) {
                s = "<object id='WebOffice1' type='application/x-itst-activex' align='baseline' border='0'  codebase='/WF/Activex/WebOffice.cab#V7.0.0.8'"
		+ "style='LEFT: 0px; WIDTH: 100%; TOP: 0px; HEIGHT: 100%'"
		+ "clsid='{E77E049B-23FC-4DB8-B756-60529A35FAD5}'"
        + "<param name ='_ExtentX' value='6350'>"
        + "<param name ='_ExtentY' value='6350'>"
		+ "</object>";
            }
            document.write(s);

        </script>--%>
    </div>
    <div style="position: relative;">
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
