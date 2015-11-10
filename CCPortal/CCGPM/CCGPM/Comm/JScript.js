
/* Table 特效风格 */
function TROver(ctrl) {
    ctrl.style.backgroundColor = 'LightSteelBlue';
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
    ctrl.className=s;
}

/* 默认植问题 */
function OpenHelperTBNo(appPath, EnsName, ctl) {
    var url = appPath + '/Comm/RefFunc/DataHelp.htm?' + appPath + '/Comm/HelperOfTBNo.aspx?EnsName=' + EnsName;
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
    var v = window.showModalDialog(url, winName, 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
    return;
}

function WinShowModalDialog(url, winName, w, h) {
    var v = window.showModalDialog(url, winName, 'dialogHeight: ' + h + 'px; dialogWidth: ' + w + 'px; center: yes; help: no');
    return;
}

function ReturnVal(ctrl, url, winName) {
    url = url + '&CtrlVal=' + ctrl.value;
    var v = window.showModalDialog(url, winName, 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
    if (v == null || v == '' || v == 'NaN') {
        return;
    }
    ctrl.value = v;
    return;
}

function WinOpen(url) {
    var newWindow = window.open(url, 'z', 'scroll:1;status:1;help:1;resizable:1;dialogWidth:680px;dialogHeight:420px');
    newWindow.focus();
    return;
}

function WinOpen(url, winName) {
    var newWindow = window.open(url, winName, 'width=700,height=400,top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
    newWindow.focus();
    return;
}
 
/* ESC Key Down  */
function Esc() {
    if (event.keyCode == 27)
        window.close();
    return true;
}
/************************************************ 校验类 top *********************************************************/
/* 用来验证 输入的是不是一个数字． onkeypress */
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

    // alert(event.keyCode);
    if (event.keyCode >= 37 && event.keyCode <= 40)
        return true;

    if (event.keyCode >= 96 && event.keyCode <= 105)
        return true;
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

//    if (event.keyCode <= 105 && event.keyCode >= 96)
//        return true;

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
/************************************************ 校验类End *********************************************************/

/* 显示日期 */
function ShowDateTime(appPath, ctrl) {
    url = appPath + '/Comm/Pub/CalendarHelp.htm';
    val = window.showModalDialog(url, '', 'dialogHeight: 335px; dialogWidth: 340px; center: yes; help: no');
    if (val == undefined)
        return;
    ctrl.value = val;
}

/* 默认植问题 */
function DefaultVal1(appPath, ctrl, className, attrKey, empId) {
    if (event.button != 2)
        return;
    url = appPath + '/Comm/RefFunc/DataHelp.htm?' + appPath + '/Comm/HelperOfTB.aspx?EnsName=' + className + '&AttrKey=' + attrKey + '&empId=' + empId;
    str = ctrl.value;
    str = window.showModalDialog(url + '&Key=' + str, '', 'dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;
    ctrl.value = str;
}

/* 默认植问题　 */
function RefEns(appPath, ctrl, className, attrKey) {
    if (event.button != 2)
        return;
    url = appPath + '/Comm/RefFunc/DataHelp.htm?' + appPath + '/Comm/HelperOfTB.aspx?EnsName=' + className + '&AttrKey=' + attrKey + '&empId=' + empId;
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
// 得到cooke .如果是Null , 返回的val
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
 
// 设置cook
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
    if (appPath == '/')
        url = "DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext;
    else
        url = "/Comm/RefFunc/DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext;

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
    url = "/ccflow/Comm/RefFunc/DataHelp.htm?HelperOfDDL.aspx?EnsName=" + EnsName + "&RefKey=" + refkeyvalue + "&RefText=" + reftext + "&MainEns=" + MainEns + "&DDLID=" + ddlID;
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
/* 设置选框 cb1.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')"; */
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


/* 输入的是否是字段类型 */
function IsDigit(s) {
    if (s.value == '' || s.value==' ' )
        return true;
    var patrn = new RegExp("^[a-zA-Z][a-zA-Z0-9_]*$");
    if (!patrn.exec(s.value)) {
        alert("请输入字母或数字，第一个字符必须是字母！")
        s.value = "";
        return false;
    }
    return true;
}

function TBHelp(ctrl, appPath, enName, attrKey) {
    var url = appPath + "/Comm/RefFunc/DataHelp.htm?" + appPath + "/Comm/HelperOfTB.aspx?EnsName=" + enName + "&AttrKey=" + attrKey;
    var str = window.showModalDialog(url, 'sd', 'dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no');
    if (str == undefined)
        return;
    document.getElementById(ctrl).value = str;
}