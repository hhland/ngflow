
/* Table  Style Effects  */
function TROver(ctrl) {
    ctrl.style.backgroundColor = 'LightSteelBlue';
}

function RefMethod1(path, index, warning, target, ensName, keys) {
    if (warning == null || warning == '') {
    }
    else {
        if (confirm(warning) == false)
            return false;
    }

    var url = "../Comm/RefMethod.aspx?Index=" + index + "&EnsName=" + ensName + keys;
    if (target == null || target == '')
        var b = WinOpen(url, 'remmed');
    else
        var a = WinOpen(url, target);
    return true;
}
function TROut(ctrl) {
    ctrl.style.backgroundColor = 'white';
    ctrl.style.borderWidth = '1px';
    ctrl.style.borderstyle = 'none none dotted none';
}

var s = null;
function TBOnfocus(ctrl) {
    //  background-color: #f4f4f4;
    s = ctrl.className;
    ctrl.style.borderColor = 'LightSteelBlue';
}

function TBOnblur(ctrl) {
    // ctrl.style.borderColor = 'white';
    ctrl.className = s;
}

/*  Default plant problems  */
function OpenHelperTBNo(appPath, EnsName, ctl) {
    var url = appPath + '/Comm/DataHelp.htm?' + appPath + '/Comm/HelperOfTBNo.aspx?EnsName=' + EnsName;
    var str = window.showModalDialog(url, '', 'dialogHeight: 550px; dialogWidth:950px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;
    if (str == null)
        return;
    ctl.value = str;
    return;
}
function To(url) {
    window.location.href = url;
}

window.onerror = function () {
    return true;
}

function OpenItme3(webAppPath, className, url) {
    var url = webAppPath + "/Comm/" + 'Item3.aspx?EnName=' + className + url;
    var newWindow = window.open(url, 'card', 'width=700,top=50,left=50,height=500,scrollbars=yes,resizable=yes,toolbar=false,location=false');
    newWindow.focus();
    return;
}

function WinShowModalDialog(url, winName) {
    var v = window.showModalDialog(url, winName, 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    return;
}


function WinShowModalDialog(url, winName, w, h) {
    var explorer = window.navigator.userAgent;
    if (explorer.indexOf("Chrome") >= 0) {// Google 
        window.open(url, "sd", "scrollbars=yes,resizable=yes,center=yes,minimize=yes,maximize=yes,height= 600px,width= 550px, top=50px, left= 650px");
    }
    else {//IE, Firefox 
        var v = window.showModalDialog(url, '', 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 550px; dialogTop: 50px; dialogLeft: 650px;');
    }
    return;
}
function WinShowModalDialog_Accepter(url) {//14.12.11   秦  Selector   Note the parameters set 
    var explorer = window.navigator.userAgent;
    if (explorer.indexOf("Chrome") >= 0) {// Google 
        window.open(url, "sd", "scrollbars=yes,resizable=yes,center=yes,minimize=yes,resizable=no,maximize=yes,height= 600px,width= 550px, top=50px, left= 650px");
    }
    else {//IE, Firefox 
        var v = window.showModalDialog(url, '', 'scrollbars=yes;resizable=yes;center=yes;resizable:no;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 550px; dialogTop: 50px; dialogLeft: 650px;');
    }
    return;
}
function ReturnVal(ctrl, url, winName) {
    //update by dgq 2013-4-12  There is no judgment ?
    if (ctrl && ctrl.value != "") {
        if (url.indexOf('?') > 0)
            url = url + '&CtrlVal=' + ctrl.value;
        else
            url = url + '?CtrlVal=' + ctrl.value;
    }
    // Control without saving changes to the title 
    if (typeof self.parent.TabFormExists != 'undefined') {
        var bExists = self.parent.TabFormExists();
        if (bExists) {
            self.parent.ChangTabFormTitleRemove();
        }
    }
    //    if (window.ActiveXObject) {
    //        //    var v = window.showModalDialog(url, winName, 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    //        var v = window.showModalDialog(url, winName, 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    //        if (v == null || v == '' || v == 'NaN') {
    //            return;
    //        }
    //        ctrl.value = v;
    //    }
    //    else {
    //        window.showModalDialog(url + '?temp=' + Math.random(), winName, 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    //    }

    //OpenJbox();
    if (window.ActiveXObject) {// In the case of IE Browser , Perform the following method 
        var v = window.showModalDialog(url, winName, 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
        if (v == null || v == '' || v == 'NaN') {
            return;
        }
        ctrl.value = v;
    }
    else {// In the case of chrome, Perform the following method a
        try {
            //OpenJbox();
            $.jBox("iframe:" + url, {
                title: ' Title ',
                width: 800,
                height: 350,
                buttons: { 'Sure': 'ok' },
                submit: function (v, h, f) {
                    var row = h[0].firstChild.contentWindow.getSelected();
                    ctrl.value = row.Name;
                }
            });
        } catch (e) {
            alert(e);
        }
    }
    // Amend title , Save loses focus 
    if (typeof self.parent.TabFormExists != 'undefined') {
        var bExists = self.parent.TabFormExists();
        if (bExists) {
            self.parent.ChangTabFormTitle();
        }
    }
    //    if (v == null || v == '' || v == 'NaN') {
    //        return;
    //    }
    //    ctrl.value = v;
    return;
}

function WinOpen(url) {
    var newWindow = window.open(url, 'z', 'scroll:1;status:1;help:1;resizable:1;dialogWidth:680px;dialogHeight:420px');
    newWindow.focus();
    return;
}

function WinOpen(url, winName) {
    if (EUIWinOpen(url, winName)) return;
    //如果没有easyui，才使用原来的方式
    var newWindow = window.open(url, winName, 'width=700,height=400,top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
    newWindow.focus();
    return;
}

//使用easyui打开窗口
function EUIWinOpen(url, winName) {
    try {
        if (!$.fn.window)return false;
    } catch (ex) {
        return false;
    }
    var winid = "win_" + parseInt(100 * Math.random());
    var $div = $("<div id='"+winid+"'></div>");
    url += "&flag=frame&winid="+winid;
    var w = 700, h=400;
    var fw = w - 30, fh = h - 40;
    var content = "<iframe src='" + url + "' width='100%' height='100%' style='border-width:0px' ><" + "/iframe>";

    winName = winName || "";
    var title = winName == "" ? winid : winName;

    $div.window({
        title: title
        , width:w
        , height: h
        ,modal:true
        //,href:url
        , content: content
        ,closable:true
    });
    return $div;
}

function openAcc(url) {
    location.href = url;
}
/* ESC Key Down  */
function Esc() {
    if (event.keyCode == 27)
        window.close();
    return true;
}

/************************************************  Check class  top *********************************************************/
/*  Used to verify the   Input is not a number ． onkeypress */
function VirtyInt(ctrl) {

}
 

function VirtyNum(ctrl) {

    if (event.keyCode == 190) {
        if (ctrl.value.indexOf('.') == -1) {
            return true;
        }
        else {
            return false;
        }
    }

    if (ctrl.value.indexOf('.') >= 0 && event.keyCode == 46)
        return false;

    if (ctrl.value.indexOf('.') >= 0 && event.keyCode == 45)
        return false;
    if (ctrl.value.indexOf('-') >= 0 && event.keyCode == 46)
        return false;
    // alert(event.keyCode);
    if (event.keyCode >= 37 && event.keyCode <= 40)
        return true;

    if (event.keyCode >= 96 && event.keyCode <= 105)
        return false;
    if (event.keyCode == 8)
        return true;

    //   alert(event.keyCode);

    var txtval = ctrl.value;
    var key = event.keyCode;
    if ((key < 48 || key > 57) && key != 46) {
        event.keyCode = 0;
    }
    else {
        if (key == 46) {
            if (txtval.indexOf(".") != -1 || txtval.length == 0)
                event.keyCode = 0;
        }
    }

    if ((key < 48 || key > 57) && key != 46) {
        event.keyCode = 0;
    }

    if (event.keyCode >= 48 && event.keyCode <= 57)
        return true;

    if (event.keyCode == 229)
        return true;

    if (event.keyCode == 8 || event.keyCode == 190)
        return true;

    if (event.keyCode == 13)
        return true;

    if (event.keyCode == 46)
        return true;

    if (event.keyCode == 45)
        return true;
    return false;
}

function VirtyNum(ctrl, type) {



    if (event.keyCode == 190) {
        if (type == 'int')
            return false;
        else {
            if (ctrl.value.indexOf('.') == -1) {
                return true;
            }
            else {
                return false;
            }
        }
    }
    if (type == 'int') {
        if (event.keyCode == 45 && ctrl.value.indexOf('-') >= 0)
            return false;
        else
            return true;
        if (event.keyCode == 46)
            return false;
    }

 
    if (type == 'float') {

        if (ctrl.value.indexOf('.') > 0 && event.keyCode == 46)
            return false;
        if (ctrl.value.indexOf('-') == 0 && event.keyCode == 45)
            return false;
    }
    // alert(event.keyCode);
    if (event.keyCode >= 37 && event.keyCode <= 40)
        return true;

    if (event.keyCode >= 96 && event.keyCode <= 105)
        return false;
    if (event.keyCode == 8)
        return true;

    //   alert(event.keyCode);

    var txtval = ctrl.value;
    var key = event.keyCode;
    if ((key < 48 || key > 57) && key != 46) {
        event.keyCode = 0;
    }
    else {
        if (key == 46) {
            if (txtval.indexOf(".") != -1 || txtval.length == 0)
                event.keyCode = 0;
        }
    }

    if ((key < 48 || key > 57) && key != 46) {
        event.keyCode = 0;
    }

    if (event.keyCode >= 48 && event.keyCode <= 57)
        return true;

    if (event.keyCode == 229)
        return true;

    if (event.keyCode == 8 || event.keyCode == 190)
        return true;

    if (event.keyCode == 13)
        return true;

    if (event.keyCode == 46)
        return true;

    if (event.keyCode == 45)
        return true;
    return false;
}

function VirtyMoney(number) {

    number = number.replace(/\,/g, "");
    if (number == "")
        return "0.00";

    if (number < 0)
        return '-' + outputDollars(Math.floor(Math.abs(number) - 0) + '') + outputCents(Math.abs(number) - 0);
    else
        return outputDollars(Math.floor(number - 0) + '') + outputCents(number - 0);
}

function outputDollars(number) {
    if (number.length <= 3)
        return (number == '' ? '0' : number);
    else {
        var mod = number.length % 3;
        var output = (mod == 0 ? '' : (number.substring(0, mod)));
        for (i = 0; i < Math.floor(number.length / 3); i++) {
            if ((mod == 0) && (i == 0))
                output += number.substring(mod + 3 * i, mod + 3 * i + 3);
            else
                output += ',' + number.substring(mod + 3 * i, mod + 3 * i + 3);
        }
        return (output);
    }
}
function outputCents(amount) {
    amount = Math.round(((amount) - Math.floor(amount)) * 100);
    return (amount < 10 ? '.0' + amount : '.' + amount);
}
/************************************************  Check class End *********************************************************/

/*  Display Date  */
function ShowDateTime(appPath, ctrl) {
    url = appPath + '/Comm/Pub/CalendarHelp.htm';
    val = window.showModalDialog(url, '', 'dialogHeight: 335px; dialogWidth: 340px; center: yes; help: no');
    if (val == undefined)
        return;
    ctrl.value = val;
}

/*  Default plant problems  */
function DefaultVal1(appPath, ctrl, className, attrKey, empId) {
    if (event.button != 2)
        return;
    url = appPath + '/Comm/DataHelp.htm?' + appPath + '/Comm/HelperOfTB.aspx?EnsName=' + className + '&AttrKey=' + attrKey + '&empId=' + empId;
    str = ctrl.value;
    str = window.showModalDialog(url + '&Key=' + str, '', 'dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;
    ctrl.value = str;
}

/*  Default plant problems 　 */
function RefEns(appPath, ctrl, className, attrKey) {
    if (event.button != 2)
        return;
    url = appPath + '/Comm/DataHelp.htm?' + appPath + '/Comm/HelperOfTB.aspx?EnsName=' + className + '&AttrKey=' + attrKey + '&empId=' + empId;
    str = ctrl.value;
    str = window.showModalDialog(url + '&Key=' + str, '', 'dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;
    ctrl.value = str;
}

/* about cookice */
function GetCookieVal(offset) {
    var endstr = document.cookie.indexOf(";", offset);
    if (endstr == -1)
        endstr = document.cookie.length;
    return unescape(document.cookie.substring(offset, endstr));
}
//  Get cooke . In the case of Null ,  Returns val
function GetCookie(name, isNullReVal) {
    var arg = name + "=";
    var alen = arg.length;
    var clen = document.cookie.length;
    var I = 0;
    while (I < clen) {
        var j = I + alen;
        if (document.cookie.substring(I, j) == arg)
            return GetCookieVal(j);
        I = document.cookie.indexOf(" ", I) + 1;
        if (I == 0)
            break;
    }
    return isNullReVal;
}

//  Set up cook
function SetCookie(name, value) {
    var argv = SetCookie.arguments;
    var argc = SetCookie.arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    var path = (argc > 3) ? argv[3] : null;
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;

    document.cookie = name + "=" + escape(value) + ((expires == null) ? "" : ("; expires=" + expires.toGMTString())) +
    ((path == null) ? "" : ("; path=" + path)) + ((domain == null) ? "" : ("; domain=" + domain)) + ((secure == true) ? "; secure" : "");
}

function HalperOfDDL(appPath, EnsName, refkeyvalue, reftext, ddlID) {
    var url = ''; // appPath+"/Comm/DataHelp.htm?HelperOfDDL.aspx?EnsName="+EnsName+"&RefKey="+refkeyvalue+"&RefText="+reftext;
    // modified by ZhouYue 2013-05-20  changed "/DataHelp.htm..." to "../DataHelp.htm"
    if (appPath == '/')
        url = "../DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext;
    else
        url = appPath + "/Comm/DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext;
    var str = window.showModalDialog(url, '', 'dialogHeight: 500px; dialogWidth:800px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    SetDDLVal(ddlID, str);
}

function SetDDLVal(ddlID, val) {
    if (val == undefined)
        return;
    var ddl = document.getElementById(ddlID);
    var mylen = ddl.options.length - 1;
    while (mylen >= 0) {
        if (ddl.options[mylen].value == val) {
            ddl.options[mylen].selected = true;
        }
        mylen--;
    }
}

function onDDLSelectedMore(ddlID, MainEns, EnsName, refkeyvalue, reftext) {
    var url = '';
    url = "../Comm/DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext + "&MainEns=" + MainEns + "&DDLID=" + ddlID;
    var str = window.showModalDialog(url, '', 'dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    ddlID = ddlID.replace('DDL_', '');
    if (str != null) {
        var hrf = window.location.href;
        hrf = hrf.replace(ddlID, 's');
        hrf = hrf + '&' + ddlID + '=' + str;
        window.location.href = hrf;
    }
}

function onkeydown() {
    if (window.event.srcElement.tagName = "TEXTAREA")
        return false;
    if (event.keyCode == 13)
        event.keyCode = 9;
}

function RSize() {

    if (document.body.scrollWidth > (window.screen.availWidth - 100)) {
        window.dialogWidth = (window.screen.availWidth - 100).toString() + "px"
    } else {
        window.dialogWidth = (document.body.scrollWidth + 50).toString() + "px"
    }

    if (document.body.scrollHeight > (window.screen.availHeight - 70)) {
        window.dialogHeight = (window.screen.availHeight - 50).toString() + "px"
    } else {
        window.dialogHeight = (document.body.scrollHeight + 115).toString() + "px"
    }
    window.dialogLeft = ((window.screen.availWidth - document.body.clientWidth) / 2).toString() + "px"
    window.dialogTop = ((window.screen.availHeight - document.body.clientHeight) / 2).toString() + "px"
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
/*  Set box  cb1.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')"; */
function SetSelected(cb, ids) {
    //alert(ids);
    var arrmp = ids.split(',');
    var arrObj = document.all;
    var isCheck = false;
    if (cb.checked)
        isCheck = true;
    else
        isCheck = false;
    for (var i = 0; i < arrObj.length; i++) {
        if (typeof arrObj[i].type != "undefined" && arrObj[i].type == 'checkbox') {
            for (var idx = 0; idx <= arrmp.length; idx++) {
                if (arrmp[idx] == '')
                    continue;
                var cid = arrObj[i].name + ',';
                var ctmp = arrmp[idx] + ',';
                if (cid.indexOf(ctmp) > 1) {
                    arrObj[i].checked = isCheck;
                    //                    alert(arrObj[i].name + ' is checked ');
                    //                    alert(cid + ctmp);
                }
            }
        }
    }
}


/*  Whether the input is a field type  */
function IsDigit(s) {
    if (s.value == '' || s.value == ' ')
        return true;
    var patrn = new RegExp("^[a-zA-Z][a-zA-Z0-9_]*$");
    if (!patrn.exec(s.value)) {
        alert(" Please enter the letters or numbers , The first character must be a letter !")
        s.value = "";
        return false;
    }
    return true;
}

function parseVal2Float(ctrl) {
    var tb_Ctrl = document.getElementById(ctrl);
    if (tb_Ctrl) {
        if (tb_Ctrl.value == "") {
            tb_Ctrl.value = 0;
        }
        return parseFloat(tb_Ctrl.value.replace(',', ''))
    }
    return 0;
}

function TBHelp(ctrl, appPath, enName, attrKey) {
    var url = GetLocalWFPreHref();
    url = url + "/WF/Comm/HelperOfTBEUI.aspx?EnsName=" + enName + "&AttrKey=" + attrKey;
    //    url = url + "/WF/Comm/HelperOfTBEUI.aspx?EnsName=" + enName + "&AttrKey=" + attrKey;
    //    var str = window.showModalDialog(url, 'sd', 'dialogHeight: 500px; dialogWidth:400px; dialogTop: 120px; dialogLeft: 200px; center: no; help: no');
    //    if (str == undefined)
    //        return;
    //    $("*[id$=" + ctrl + "]").val(str);
    //document.getElementById(ctrl).value = str;
    if (window.ActiveXObject) {

        var str = window.showModalDialog(url, 'sd', 'dialogHeight: 500px; dialogWidth:400px; dialogTop: 120px; dialogLeft: 200px; center: no; help: no');
        if (str == undefined)
            return;
        $("*[id$=" + ctrl + "]").val(str);
    }
    else {
        alert(url);
        $.jBox("iframe:" + url, {
            title: "4567",
            width: 800,
            height: 350,
            buttons: { 'OK': 'ok' },
            submit: function (v, h, f) {
                var row = h[0].firstChild.contentWindow.getSelected();

            }
        });
    }
}
function WorkCheckTBHelp(ctrl, op) {
    var url = "Comm/HelperOfTBEUI.aspx";
    var str = window.showModalDialog(url, 'sd', 'dialogHeight: 500px; dialogWidth:800px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;

    document.getElementsByTagName("iframe")[0].contentDocument.body.children[0][5].innerHTML = str;
}
// Digital Signatures 
function SigantureAct(ele, UserNo, FK_MapData, KeyOfEn, WorkID) {
    if (ele) {
        var imgSrc = ele.src;
        // Modify data 
        var json_data = {
            "method": "sigantureact",
            "imgSrc": imgSrc,
            "UserNo": UserNo,
            "FK_MapData": FK_MapData,
            "KeyOfEn": KeyOfEn,
            "WorkID": WorkID
        };
        if (imgSrc.indexOf(UserNo) > 0 || imgSrc.indexOf("UnName") > 0) {

        }
        else {

        }
        // Modify data 
        $.ajax({
            type: "get",
            url: "../Comm/HelperOfSiganture.aspx",
            data: json_data,
            beforeSend: function (XMLHttpRequest) {
                //ShowLoading();
            },
            success: function (data, textStatus) {
                //ele.src = "/DataUser/Siganture/siganture.JPG";
                ele.src = "../../DataUser/Siganture/" + data + ".JPG";
            },
            complete: function (XMLHttpRequest, textStatus) {
                //HideLoading();
            },
            error: function () {
                // Error processing request 
                alert(" Digital Signature Error , Please contact the administrator to check the form property settings .");
            }
        });
    }
}

// Get WF Before the path 
function GetLocalWFPreHref() {
    var url = window.location.href;
    if (url.indexOf('/WF/') >= 0) {
        var index = url.indexOf('/WF/');
        url = url.substring(0, index);
    }
    return url;
}
//function closeWins() {if (window.dialogArguments && window.dialogArguments.window) window.dialogArguments.window.location = window.dialogArguments.window.location;}
//window.onunload = closeWins;

function endWith(str, s) {
    if (s == null || s == "" || str.length == 0 || s.length > str.length)
        return false;
    if (str.substring(str.length - s.length) == s)
        return true;
    else
        return false;
    return true;
}

function startWith(str, s) {
    if (s == null || s == "" || str.length == 0 || s.length > str.length)
        return false;
    if (str.substr(0, s.length) == s)
        return true;
    else
        return false;
    return true;

}

function replaceAll(s1, s2, s3) {
    return s1.replace(new RegExp(s2, 'gm'), s3);
}


function importScript(src) {
    document.writeln("<script type='text/javascript' src=\""+src+"\"><"+"/script>");
}
