<%@ Page Title="" Language="C#" MasterPageFile="~/WF/CCForm/WinOpen.master" AutoEventWireup="true"
    CodeBehind="WordFrm.aspx.cs" Inherits="CCFlow.WF.CCForm.OfficeFrm.WordFrm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jBox/jquery.jBox-2.3.min.js" type="text/javascript"></script>
    <link href="../../Scripts/jBox/Skins/Blue/jbox.css" rel="stylesheet" type="text/css" />
    <script src="../../Comm/JScript.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var isShowAll = false;
        var webOffice = null;
        var strTimeKey;
        var isOpen = false;
        var isInfo = false;
        var marksType = "doc,docx";
        var doc = null;
        var win = null;
        var app = null;
        var fields = $.parseJSON('<%=ReplaceFields %>');
        var dtlNos = $.parseJSON('<%=ReplaceDtlNos %>');

        $(function () {
            InitOffice();

            $('body').attr('onunload', 'closeDoc()');
        });

        function InitOffice() {
            /// <summary>
            ///  Initialization Office
            /// </summary>
            webOffice = document.all.WebOffice1;

            if ($('#<%=fileName.ClientID %>').val() != "") {
                OpenWeb("1");
            }

            EnableMenu();
        }

        function EnableMenu() {
            /// <summary>
            ///  Settings button 
            /// </summary>
            webOffice.HideMenuItem(0x01 + 0x02 + 0x04 + 0x10 + 0x20);
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
                $.jBox.alert(" Abnormal \r\nOpenFile Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            loaddingOut(' Open to complete ');
            return false;
        }

        function OpenWeb(loadtype) {
            pageLoadding(' Opening Template ...');

            try {
                var type = $("#<%=fileType.ClientID %>").val();
                var fileName = $('#<%=fileName.ClientID %>').val();
                var url = location.href + "&action=LoadFile&LoadType=" + loadtype + "&fileName=" + fileName;
                OpenDoc(url, type);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nOpenWeb Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            loaddingOut(' Open to complete .');
        }

        // Open the file 
        function OpenDoc(url, type) {
            var openType = webOffice.LoadOriginalFile(url, type);
            
            app = webOffice.GetDocumentObject().Application;
            doc = app.ActiveDocument;
            win = app.ActiveWindow;

            if (openType > 0) {
                if ("<%=IsMarks %>" == "True")
                    SetTrack(1);
                else
                    SetTrack(0);

                isOpen = true;

                if ("<%=IsFirst %>" == "True") {
                    alert('ok');
                    replaceParams();
                }
                else {
                    loadInfos();
                }
            } else {
                $.jBox.alert('OpenDoc  Failed to open document ', ' Abnormal ');
            }
        }

        function SetTrack(track) {
            /// <summary>
            ///  Setting traces , Show all traces of user , Whether read-only document 
            /// </summary>
            if ("<%=ReadOnly %>" == "True") {
                webOffice.ProtectDoc(1, 2, "");
            }
            else {
                webOffice.ProtectDoc(0, 1, "");
            }

            webOffice.SetTrackRevisions(track);

            if (track == 1) {
                webOffice.ShowRevisions(0); // Hide Amendment 
            }

            SetUsers();

            var types = $('#<%=fileType.ClientID %>').val();
            if (marksType.indexOf(types) >= 0) {
                ShowUserName();
            }
        }

        // Set the current operation of the user 
        function SetUsers() {
            try {
                webOffice.SetCurrUserName("<%=UserName %>");

                InitShowName();

            } catch (e) {
                $.jBox.alert(" Abnormal \r\nSetUsers Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
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
                    webOffice.ShowRevisions(1);
                    if (isShowAll) {
                        win.ToggleShowAllReviewers();
                        isShowAll = false;
                    } else {
                        win.ToggleShowAllReviewers();
                    }

                } else {
                    //                    if (!isShowAll) {
                    //                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    //                        webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                    //                        isShowAll = true;
                    //                    }
                    isShowAll = true;
                    webOffice.ShowRevisions(1); // Displays the revision 
                    win.View.Reviewers(user).Visible = true;
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nShowUserName Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
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
                $.jBox.alert(" Abnormal \r\InitShowName Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }

        function closeDoc() {
            webOffice.SetCurrUserName("");
            webOffice.closeDoc(0);
        }

        function OpenTempLate() {
            if (readOnly()) {
                return false;
            }

            LoadTemplate('word', ' Document Templates ', OpenWeb);
            return false;
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
                            $("#<%=fileName.ClientID %>").val(row.Name);
                            $("#<%=fileType.ClientID %>").val(row.Type);
                            callback();
                            loaddingOut(' Open to complete ...');

                            return true;
                        }
                    }
                });
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nLoadTemplate Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }

        // Saved to the server 
        function saveOffice() {
            //            alert();
            //            return;

            if (readOnly()) {
                return false;
            }

            pageLoadding(' Saving ...');

            try {
                if (isOpen) {
                    if ("<%=IsCheck %>" == "True") {
                        if (isInfo) {
                            isInfo = false;
                            doc.Content.InsertAfter("\n<%=NodeInfo %>");
                        }
                    }

                    var fieldJson = getPostJsonString();
                    var dtlsJson = getDtlsPostJsonString();

                    // Here submission form fields in order to solve the default maximum 1146[ Testing out , May be non-exact value ] Problem characters , Will be greater than 1000 Send segmented characters 
                    var len = dtlsJson.length;
                    var arrs = new Array();
                    var spanLen = 1000;
                    var startIdx = 0;

                    webOffice.HttpInit();
                    webOffice.HttpAddPostCurrFile("File", "");
                    // List 
                    for (var i = 1; ; i++) {
                        startIdx = (i - 1) * spanLen;

                        if (startIdx > len) break;

                        arrs.push(dtlsJson.substr(startIdx, Math.min(len - startIdx, spanLen)));
                    }

                    for (var i = 0; i < arrs.length; i++) {
                        webOffice.HttpAddPostString("dtls" + i, arrs[i]);
                    }

                    len = fieldJson.length;
                    arrs = new Array();
                    // Main table 
                    for (var i = 1; ; i++) {
                        startIdx = (i - 1) * spanLen;

                        if (startIdx > len) break;

                        arrs.push(fieldJson.substr(startIdx, Math.min(len - startIdx, spanLen)));
                    }

                    for (var i = 0; i < arrs.length; i++) {
                        webOffice.HttpAddPostString("field" + i, arrs[i]);
                    }

                    var src = location.href + "&action=SaveFile&filename=" + $('#<%=fileName.ClientID %>').val();
                    webOffice.HttpPost(src);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nsaveOffice1 Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            loaddingOut(' Save to finish ...');
            var types = $('#<%=fileType.ClientID %>').val();

            try {
                if (marksType.indexOf(types) >= 0) {
                    InitShowName();

                    if (isShowAll) {
                        win.ToggleShowAllReviewers();
                        //win.ToggleShowAllReviewers();
                        isShowAll = false;
                    } else {
                        win.ToggleShowAllReviewers();
                    }

                    ShowUserName();
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nsaveOffice2 Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            return false;
        }

        // Sets of red tape 
        function TaoHong() {
            try {
                var mark = AddBooks();
                var type = $('#<%=fileType.ClientID %>').val();
                var name = $('#<%=fileName.ClientID %>').val();

                if (type == "png" || type == "jpg" || type == "bmp") {
                    webOffice.SetFieldValue(mark, window.location.protocol + "//" + window.location.host + "/DataUser/OfficeOverTemplate/" + name, "::JPG::");
                } else {
                    webOffice.SetFieldValue(mark, window.location.protocol + "//" + window.location.host + "/DataUser/OfficeOverTemplate/" + name, "::FILE::");
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nTaoHong Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }

        // Accept Change 
        function acceptOffice() {
            try {
                if (readOnly()) {
                    return false;
                }

                webOffice.SetTrackRevisions(4);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nacceptOffice Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
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
                $.jBox.alert(" Abnormal \r\nrefuseOffice Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            return false;
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

        // Printing documents 
        function printOffice() {
            try {
                if (isOpen) {
                    webOffice.PrintDoc(1);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nprintOffice Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }

            return false;
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
                var name = $('#<%=fileName.ClientID %>').val();
                var type = $('#<%=fileType.ClientID %>').val();
                var url = window.location.protocol + "//" + window.location.host + "/DataUser/OfficeSeal/" + name;

                var mark = AddBooks();
                //webOffice.InSertFile(url, 8);
                AddPicture(mark, url, 5);
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nseal Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
            }
        }

        // Add a bookmark 
        function AddBooks() {
            var d = new Date();
            var date = d.getFullYear() + "" + d.getMonth() + "" + d.getDay() + "" + d.getHours() + "" + d.getMinutes() + "" + d.getSeconds();
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

        function DownLoad() {
            try {
                if (isOpen) {
                    webOffice.ShowDialog(84);
                } else {
                    $.jBox.alert(' Open document !', ' Prompt ');
                }
            } catch (e) {
                $.jBox.alert(" Abnormal \r\nDownLoad Error:" + e + "\r\nError Code:" + e.number + "\r\nError Des:" + e.description, ' Abnormal ');
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

        /* The following is a helper object for program design and definition */
        function JWDtl(sDtlNo, oWebTable) {
            /// <summary>JS端Word Form </summary>
            /// <param name="sDtlNo" Type="String"> Serial number , Number corresponding server list </param>
            /// <param name="oWebTable" Type="Object"> Storing data acquired from the server Table Object </param>
            this.dtlNo = sDtlNo;
            this.webTable = oWebTable;
            this.wordTable = null;
            this.columns = new Array();  //JWColumn An array of objects 

            if (typeof JWDtl._initialized == "undefined") {

                JWDtl.prototype.getFieldValue = function (rowIdx, colIdx) {
                    /// <summary> Gets the value of the specified row cell </summary>
                    /// <param name="rowIdx" Type="Int"> Line number ,从1 Begin </param>
                    /// <param name="colIdx" Type="Int"> Column No. ,从1 Begin </param>

                    if (this.wordTable == null) return null;

                    var text = this.wordTable.Cell(rowIdx, colIdx).Range.Text;
                    return text.substr(0, text.length - 2);
                };

                JWDtl.prototype.getValuesJsonString = function () {

                    if (this.wordTable == null) return null;

                    var rows = this.wordTable.Rows.Count;
                    var json = '{"dtlno":"' + this.dtlNo + '","dtl":[';
                    var rowstr = '';
                    var isLastRow = true;
                    var cellValue = '';

                    for (var i = 2; i <= rows; i++) {
                        json += '{"rowid":"' + (i - 1) + '","cells":[';

                        rowstr = '';
                        isLastRow = true;
                        cellValue = '';

                        for (var ci = 0; ci < this.columns.length; ci++) {
                            if (this.columns[ci].field == 'rowid') continue;

                            cellValue = this.getFieldValue(i, this.columns[ci].columnIdx);

                            if (isLastRow && cellValue.length > 0) {
                                isLastRow = false;
                            }

                            rowstr += '{"key":"' + this.columns[ci].field + '","value":"' + cellValue + '"},';
                        }

                        if (!isLastRow) {
                            json += rowstr;
                        }
                        else {
                            json = removeLastComma(json) + ']},';
                            break;
                        }

                        json = removeLastComma(json) + ']},';
                    }

                    json = removeLastComma(json) + ']}';
                    return json;
                }

                JWDtl._initialized = true;
            }
        }

        function JWColumn(sField, sBookmarkName, iColumnIdx) {
            /// <summary>Word The list column field information </summary>
            /// <param name="sField" Type="String"> Field Name </param>
            /// <param name="sBookmarkName" Type="String"> Bookmark name identifies the field </param>
            /// <param name="iColumnIdx" Type="Int"> Column number where the table fields </param>
            this.field = sField;
            this.bookmarkName = sBookmarkName;
            this.columnIdx = iColumnIdx;
        }

        function JWField(sField, sBookmarkName, sValue) {
            /// <summary>Word The main table field information </summary>
            /// <param name="sField" Type="String"> Field Name </param>
            /// <param name="sBookmarkName" Type="String"> Bookmark name identifies the field </param>
            /// <param name="sValue" Type="String"> Field values </param>
            this.field = sField;
            this.bookmarkName = sBookmarkName;
            this.value = sValue;
        }

        function getPostJsonString() {
            // Get the main table padding field and generate new value JSON String 
            var mainJson = '[';
            var fieldValue = '';

            $.each(jwMains, function () {
                mainJson += '{"key":"' + this.field + '","value":"';
                fieldValue = doc.Bookmarks(this.bookmarkName).Range.Text;

                if (fieldValue != null) {
                    if (fieldValue.length > 1 && fieldValue.charAt(0) == ' ') {
                        fieldValue = fieldValue.substr(1);
                    }

                    if (fieldValue.length > 0 && fieldValue.charAt(fieldValue.length - 1) == ' ') {
                        fieldValue = fieldValue.substr(0, fieldValue.length - 1);
                    }

                    fieldValue = fieldValue.replace('"', '\"').replace('\\', '\\\\');
                }

                mainJson += fieldValue + '"},';
            });

            mainJson = mainJson.substr(0, mainJson.length - 1) + ']';
            return mainJson;
        }

        function getDtlsPostJsonString() {
            var dtlsJson = '[';

            $.each(jwDtls, function () {
                dtlsJson += this.getValuesJsonString() + ',';
            });

            dtlsJson = removeLastComma(dtlsJson) + ']';
            return dtlsJson;
        }

        function removeLastComma(str) {
            /// <summary> Removal of the specified string last comma </summary>
            /// <param name="str" Type="String"> String </param>
            if (str.charAt(str.length - 1) == ',') {
                return str.substr(0, str.length - 1);
            }

            return str;
        }

        function loadInfos() {

            var ccflow_main_bm_prefix = 'ccflow_bm_main_';
            var ccflow_dtl_bm_prefix = 'ccflow_bm_dtl_';            
            var ccflow_dtl_bm_prefix_len = ccflow_dtl_bm_prefix.length;
            var jwfield;
            var jwdtl;
            var bmName;
            var dtlBmNamePrefix;
            var dtlBmNamePrefix_len;
            var dtlWordTable;
            var firstCell;
            var colField;
            var isExist;

            // Get the main table filled with fields set 
            for (var i = 0; i < fields.length; i++) {
                if (doc.Bookmarks.Exists(ccflow_main_bm_prefix + fields[i])) {
                    jwfield = new JWField(fields[i], ccflow_main_bm_prefix + fields[i], '');
                    jwMains.push(jwfield);
                }
            }

            for (var i = 1; i <= doc.Bookmarks.Count; i++) {
                bmName = doc.Bookmarks(i).Name;

                if (bmName.length <= ccflow_dtl_bm_prefix_len || bmName.substr(0, ccflow_dtl_bm_prefix_len) != ccflow_dtl_bm_prefix) {
                    continue;
                }

                for (var d = 0; d < dtlNos.length;d++) {
                    // To determine whether this has already been saved dtl在jwDtls中
                    isExist = false;

                    for (var jw = 0; jw < jwDtls.length; jw++) {
                        if (jwDtls[jw].dtlNo == dtlNos[d]) {
                            isExist = true;
                            break;
                        }
                    }

                    if (isExist) {
                        continue;
                    }

                    dtlBmNamePrefix = ccflow_dtl_bm_prefix + dtlNos[d] + '_';
                    dtlBmNamePrefix_len = dtlBmNamePrefix.length;

                    if (bmName.length <= dtlBmNamePrefix_len || bmName.substr(0, dtlBmNamePrefix_len) != dtlBmNamePrefix) {
                        continue;
                    }

                    dtlWordTable = getTable(dtlBmNamePrefix);

                    if (dtlWordTable == null) {
                        continue;
                    }

                    jwdtl = new JWDtl(dtlNos[d], $.parseJSON('[]'));
                    jwdtl.wordTable = dtlWordTable;
                    jwDtls.push(jwdtl);

                    for (var j = 1; j <= dtlWordTable.Columns.Count; j++) {
                        firstCell = dtlWordTable.Rows(1).Cells(j);

                        if (firstCell.Range.Bookmarks.Count == 0) continue;

                        for (var b = 1; b <= firstCell.Range.Bookmarks.Count; b++) {
                            // Here firstCell.Range.Bookmarks Get the current table of all Bookmark, Get current and envisaged only in the cells Bookmark Inconsistent , Therefore, the range of cells you want to compare with the bookmark range exclude non bookmark within the current cell 
                            if ((firstCell.Range.Bookmarks(b).Range.Start >= firstCell.Range.Start && firstCell.Range.Bookmarks(b).Range.End < firstCell.Range.End) == false) {
                                continue;
                            }

                            if (firstCell.Range.Bookmarks(b).Name.length > dtlBmNamePrefix_len && firstCell.Range.Bookmarks(b).Name.substr(0, dtlBmNamePrefix_len) == dtlBmNamePrefix) {
                                colField = firstCell.Range.Bookmarks(b).Name.substr(dtlBmNamePrefix_len);

                                // Record fill column information 
                                jwdtl.columns.push(new JWColumn(colField, firstCell.Range.Bookmarks(b).Name, j));
                            }
                        }
                    }
                }
            }
        }

        var jwMains = new Array();
        var jwDtls = new Array();

        function replaceParams() {
            /// <summary> Replace all properties </summary>
            var params = $.parseJSON('<%=ReplaceParams %>');
            var dtls = $.parseJSON('<%=ReplaceDtls %>');

            // Replace the main table data 
            $.each(params, function () {
                replace(this.key, this.value.replace("\\\\", "\\").replace("\'", "'"));
            });

            // Replace schedule data 
            $.each(dtls, function () {
                var jwdtl = new JWDtl(this.dtlno, this.dtl);
                jwDtls.push(jwdtl);
                replaceDtl(this.dtlno, this.dtl, jwdtl);
            });

            // Accept Change 
            doc.AcceptAllRevisions();
        }

        function replace(field, text) {
            /// <summary> Replace Text </summary>
            /// <param name="oldStr" type="String"> Text to be replaced </param>
            /// <param name="newStr" type="String"> Was asked to replace text </param>
            //app.Selection.Find.Execute(oldStr, true, true, false, false, false, true, 1, false, newStr, 2);
            var ccflow_bm_name = 'ccflow_bm_main_' + field;
            if (doc.Bookmarks.Exists(ccflow_bm_name)) {
                var bm = doc.Bookmarks(ccflow_bm_name);
                var bmRange = bm.Range;
                var bmRangeStart = bmRange.Start;
                var jwfield = new JWField(field, ccflow_bm_name, text);

                jwMains.push(jwfield);
                bm.Select();

                if (text == null || text == '') {
                    text = '     '; // If it is empty , The default is 5 Spaces , You can not avoid when filling in bookmarks 
                }

                bmRange.Text = text;
                app.Selection.SetRange(app.Selection.Start, app.Selection.Start + text.length);
                app.Selection.Bookmarks.Add(ccflow_bm_name);
            }
        }

        function replaceDtl(dtlno, rows, jwdtl) {
            /// <summary> Filling schedules data </summary>
            /// <param name="dtlno" type="String"> List No</param>
            /// <param name="rows" type="Array"> Rows of the collection schedule </param>
            /// <param name="jwdtl" type="JWDtl"> Rows of the collection schedule </param>
            var ccflow_table_bm_prefix = 'ccflow_bm_dtl_' + dtlno + '_';
            var ccflow_formula_bm_prefix = 'ccflow_bm_dtl_formula_';
            var ccflow_table = getTable(ccflow_table_bm_prefix);

            if (ccflow_table != null) {
                // Stored in memory , Useful in saving time 
                jwdtl.wordTable = ccflow_table;

                // Completion of rows 
                if (ccflow_table.Rows.Count < rows.length + 1) {
                    ccflow_table.Rows(ccflow_table.Rows.Count).Select();
                    app.Selection.InsertRowsBelow(rows.length + 1 - ccflow_table.Rows.Count);
                }

                var cellIDPrefixLen = ccflow_table_bm_prefix.length;
                var formulaPrefixLen = ccflow_formula_bm_prefix.length;

                // Fill data 
                for (var j = 1; j <= ccflow_table.Columns.Count; j++) {
                    var firstCell = ccflow_table.Rows(1).Cells(j);

                    if (firstCell.Range.Bookmarks.Count == 0) continue;
                    var validBookmark;
                    var colField;
                    var formula;

                    for (var b = 1; b <= firstCell.Range.Bookmarks.Count; b++) {
                        // Here firstCell.Range.Bookmarks Get the current table of all Bookmark, Get current and envisaged only in the cells Bookmark Inconsistent , Therefore, the range of cells you want to compare with the bookmark range exclude non bookmark within the current cell 
                        if ((firstCell.Range.Bookmarks(b).Range.Start >= firstCell.Range.Start && firstCell.Range.Bookmarks(b).Range.End < firstCell.Range.End) == false) {
                            continue;
                        }

                        // Determine whether the column summary 
                        if (firstCell.Range.Bookmarks(b).Name.length > formulaPrefixLen && firstCell.Range.Bookmarks(b).Name.substr(0, formulaPrefixLen) == ccflow_formula_bm_prefix) {
                            formula = firstCell.Range.Bookmarks(b).Name.substr(formulaPrefixLen).split('_')[0];
                        }
                        else {
                            formula = '';
                        }

                        if (firstCell.Range.Bookmarks(b).Name.length > cellIDPrefixLen && firstCell.Range.Bookmarks(b).Name.substr(0, cellIDPrefixLen) == ccflow_table_bm_prefix) {
                            colField = firstCell.Range.Bookmarks(b).Name.substr(cellIDPrefixLen);

                            // Record fill column information 
                            jwdtl.columns.push(new JWColumn(colField, firstCell.Range.Bookmarks(b).Name, j));

                            for (var i = 0; i < rows.length; i++) {
                                ccflow_table.Rows(i + 2).Cells(j).Range.Text = getCellValue(rows[i], colField);
                            }
                        }

                        // Additions Summary domain function 
                        if (formula.length > 0) {
                            var colAlpha = String.fromCharCode(64 + j);
                            var formulaString = '=' + formula + '(' + colAlpha + '2:' + colAlpha + (rows.length + 1) + ')';
                            var formulaFormat = '';
                            var formulaLower = formula.toLocaleLowerCase();

                            if (formulaLower == 'count') {
                                formulaFormat = ' Total :0';
                            }
                            else if (formulaLower == 'average') {
                                formulaFormat = ' Average :0.00';
                            }

                            // Determine whether the summary line has increased 
                            if (ccflow_table.Rows.Count < rows.length + 2) {
                                ccflow_table.Rows(ccflow_table.Rows.Count).Select();
                                app.Selection.InsertRowsBelow(1);
                            }

                            // As used herein, Cell.Formula Methods will have problems , For unknown reasons 
                            ccflow_table.Cell(ccflow_table.Rows.Count, j).Select();
                            app.Selection.MoveLeft(1, 1);
                            app.Selection.InsertFormula(formulaString, formulaFormat);
                        }
                    }
                }
            }
        }

        function getCellValue(row, colName) {
            if (colName == 'rowid') {
                return row.rowid;
            }

            for (var i = 0; i < row.cells.length; i++) {
                if (row.cells[i].key == colName) {
                    return row.cells[i].value;
                }
            }

            return '';
        }

        function getTable(ccflow_table_bm_prefix) {
            var bms = doc.Bookmarks;
            var intable_bm;

            for (var i = 1; i <= bms.Count; i++) {
                if (bms(i).Name.length <= ccflow_table_bm_prefix.length)
                    continue;

                if (bms(i).Name.substr(0, ccflow_table_bm_prefix.length) == ccflow_table_bm_prefix) {
                    intable_bm = bms(i);
                    break;
                }
            }

            if (intable_bm == null) return null;

            intable_bm.Select();
            // Bookmark not the case here in the form of not considered , No logic in this case 
            return app.Selection.Tables(1);
        }

        function pageLoadding(msg) {
            $.jBox.tip(msg, 'loading');
        }

        function loaddingOut(msg) {
            $.jBox.tip(msg, 'success');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="easyui-layout" data-options="fit:true">
        <div data-options="region:'north',border:false,noheader:true" style="background-color: #E0ECFF;
            line-height: 30px; height: auto; padding: 2px">
            <div id="divMenu" runat="server">
            </div>
        </div>
        <div style="display: none">
            <asp:TextBox ID="fileName" runat="server" Text=""></asp:TextBox>
            <asp:TextBox ID="fileType" runat="server" Text=""></asp:TextBox>
        </div>
        <div data-options="region:'center',border:false,noheader:true">
            <script src="../../Scripts/LoadWebOffice.js" type="text/javascript"></script>
        </div>
    </div>
</asp:Content>
