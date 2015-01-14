var isShowAll = false;
var webOffice = null;


var strTimeKey;

function InitOffice() {
    webOffice = document.all.WebOffice1;

    EnableMenu();
    OpenWeb();
    strTimeKey = "";
    var date = new Date();
    strTimeKey += date.getFullYear(); //年
    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
    strTimeKey += date.getDate(); //日
    strTimeKey += date.getHours(); //HH
    strTimeKey += date.getMinutes(); //MM
    strTimeKey += date.getSeconds(); //SS

}

function SetTrack(track) {
    /// <summary>
    ///  Setting traces mode 
    /// </summary>
    /// <param name="track">1- Traces mode  0- Non traces mode  x</param>
    webOffice.SetTrackRevisions(track);

}

function SetUser() {
    /// <summary>
    ///  Set the current user 
    /// </summary>
    var user = document.getElementById('TB_User').value;

    webOffice.SetCurrUserName(user);
}


function OpenWeb() {
    /// <summary>
    ///  Open the server files 
    /// </summary>

    try {
        var type = document.getElementById('TB_FileType').value;
        var url = location.href + "&action=LoadFile";
        webOffice.LoadOriginalFile(url, type);

        SetUser();


        var isTrack = document.getElementById('TB_Track').value;
        if (isTrack == 1) {
            SetTrack(1);
        } else {
            SetTrack(0);
        }

        var isRead = document.getElementById('TB_IsReadOnly').value;

        if (isRead == 1) {
            ProtectDoc();
        } else {
            UnPortectDoc();
        }

        InitShowName();
    }
    catch (e) {
        alert(e.Message);
    }


}

function EnableMenu() {
    /// <summary>
    ///  Settings button 
    /// </summary>
    var isPrint = document.getElementById('TB_IsPrint').value;

    if (isPrint == 1) {
        webOffice.HideMenuItem(0x01 + 0x02 + 0x04);
    } else {
        webOffice.HideMenuItem(0x01 + 0x02 + 0x04 + 0x10 + 0x20);
    }


}

function ShowTrack(track) {
    /// <summary>
    ///  Show traces 
    /// </summary>
    /// <param name="track">0- Hide  1- Show </param>

    webOffice.ShowRevisions(track);
}

function SaveTrack() {
    /// <summary>
    ///  Save Amendment 
    /// </summary>
    // webOffice.AcceptAllRevisions();
    webOffice.SetTrackRevisions(4);
}
function ReturnTrack() {
    /// <summary>
    ///  Reject all the amendments 
    /// </summary>
    var vCount = webOffice.GetRevCount();
    var strUserName;
    for (var i = 1; i <= vCount; i++) {
        strUserName = webOffice.GetRevInfo(i, 0);
        webOffice.AcceptRevision(strUserName, 1);
    }
}

function InitShowName() {
    var count = webOffice.GetRevCount();

    var showName = $("#sShowName");
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
}

function SaveService() {
    /// <summary>
    ///  Server saves 
    /// </summary>
    try {

        var path = document.getElementById("TB_FilePath").value;

        webOffice.HttpInit();
        webOffice.HttpAddPostCurrFile("File", "");
        var src = location.href + "&action=SaveFile";

        webOffice.HttpPost(src);
        alert(' Saved successfully ');
    } catch (e) {
        alert(e.message);
    }
}

function ShowUserName() {
    /// <summary>
    ///  Significant traces of the current user 
    /// </summary>

    try {

        var user = $("#sShowName option:selected").val();
        if (user == " Whole ") {
            if (isShowAll) {
                webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                isShowAll = false;
            }
        } else {
            if (!isShowAll) {
                webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                isShowAll = true;
            } else {
                webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
                webOffice.GetDocumentObject().Application.ActiveWindow.ToggleShowAllReviewers();
            }
            webOffice.GetDocumentObject().Application.ActiveWindow.View.Reviewers(user).Visible = true;
        }
    } catch (e) {
        alert(e.message);
    }
}

function CloseDoc() {
    webOffice.CloseDoc(0);
}


function ProtectDoc() {
    webOffice.ProtectDoc(1, 2, "");
}

function UnPortectDoc() {
    webOffice.ProtectDoc(0, 1, "");
}

function InsertFile() {
    webOffice.InSertFile(" I was good to say ", 0);
}


//----- Action : Dynamically add WORD Super connection to download attachments -----------------------------------//
function WebWordDownFile() {
    var no = $('#CB_Flow').combobox('getValue');
    var text = $('#CB_Flow').combobox('getText');
    try {
        if (text != null && text != '' && no != null && no != '') {
            var myRange = webOffice.GetDocumentObject().Application.Selection.Range;  // Custom cursor position 

            var myHyperLink = "http://" + location.host + "/WF/Chart.aspx?FK_Flow=" + no + "&DoType=Chart&T=" + strTimeKey;
            //var myHyperLink = "http://www.goldgrid.cn/iSignature/MakeSignGif.rar";
            // Definition Download , May have other URLs , This content can get through the background 
            var myTextToDisplay = text;                           // Tips definition index information 
            var myHyperLinkName = text;          // Defines the name of the displayed text 
            var Hyperlinks = webOffice.GetDocumentObject().Application.ActiveDocument.Hyperlinks;
            Hyperlinks.Add(myRange, myHyperLink, "", myTextToDisplay, myHyperLinkName, "4");
            // The last parameter IE6:4;IE5:3
        }

    } catch (e) {
        alert(" Insert Hyperlink abnormal ..." + e.message);
    }
}

$(function () {
    LoadFLow();
});

function LoadFLow() {

    //    $.post(location.href, { 'action': 'LoadFlow' }, function(js) {

    //        if (js) {
    //            var pusData = eval('(' + js + ')');
    //            $('#CB_Flow').combobox({
    //                data: pusData,
    //                valueField: 'No',
    //                textField: 'Name'
    //            });
    //        }
    //    });


    $('#CB_Flow').combobox({
        url: location.href + "&action=LoadFlow",
        valueField: 'No',
        textField: 'Name'
    });
}


function InputFiles() {
    var file = document.getElementById('TB_Image').value;
    if (file != '' && file != null) {
        webOffice.InSertFile(file, 8);
    }
}

// Electronic Signature 
function Signature(name) {


    var url = window.location.protocol + "//" + window.location.host + "/DataUser/Seal/" + name + ".png";
    //    document.all.WebOffice1.SetFieldValue("mark_1", " Beijing ", "::ADDMARK::");
    //    webOffice.SetFieldValue("mark_1", url, "::JPG::");
    webOffice.InSertFile(url, 8);

    //AddPicture("Signature", url, 5);
}

function AddPicture(strMarkName, strBmpPath, vType) {
    // Definition of an object , Used to store ActiveDocument Object 
    var obj = new Object(webOffice.GetDocumentObject());
    if (obj != null) {
        var pBookMarks;
        // VAB Get bookmark collection interface 
        pBookMarks = obj.Bookmarks;

        var date = new Date().getFullYear() + "" + new Date().getMonth() + "" + new Date().getDay() + "" + new Date().getHours() + "" + new Date().getMinutes() + "" + new Date().getSeconds();


        webOffice.SetFieldValue("Signature" + date, "", "::ADDMARK::");

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
        pRangeInline = pRangeInlines.AddPicture(strBmpPath);
        // Picture Style ,5 Floating in the text above 
        pRangeInline.ConvertToShape().WrapFormat.TYPE = vType;
        delete obj;
    }
}


 