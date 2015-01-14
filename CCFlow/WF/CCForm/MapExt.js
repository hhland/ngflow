// **********************  Dynamic queries based on keywords . ******************************** //
var oldValue = "";
var oid;
var highlightindex = -1;
function DoAnscToFillDiv(sender, e, tbid, fk_mapExt) {

    openDiv(sender, tbid);
    var myEvent = event || window.event;
    var myKeyCode = myEvent.keyCode;
    //  Get ID为divinfo Inside DIV Object  .  
    var autoNodes = $("#divinfo").children("div");
    if (myKeyCode == 38) {
        if (highlightindex != -1) {
            autoNodes.eq(highlightindex).css("background-color", "Silver");
            autoNodes.eq(highlightindex).css("color", "Black");
            if (highlightindex == 0) {
                highlightindex = autoNodes.length - 1;
            }
            else {
                highlightindex--;
            }
        }
        else {
            highlightindex = autoNodes.length - 1;
        }
        autoNodes.eq(highlightindex).css("background-color", "blue");
        autoNodes.eq(highlightindex).css("color", "white");
    }
    else if (myKeyCode == 40) {
        if (highlightindex != -1) {
            autoNodes.eq(highlightindex).css("background-color", "Silver");
            autoNodes.eq(highlightindex).css("color", "black");
            highlightindex++;
        }
        else {
            highlightindex++;
        }

        if (highlightindex == autoNodes.length) {
            autoNodes.eq(autoNodes.length).css("background-color", "Silver");
            autoNodes.eq(autoNodes.length).css("color", "black");
            highlightindex = 0;
        }
        autoNodes.eq(highlightindex).css("background-color", "blue");
        autoNodes.eq(highlightindex).css("color", "white");
    }
    else if (myKeyCode == 13) {
        if (highlightindex != -1) {

            // Get the value of the selected text 
            var textInputText = autoNodes.eq(highlightindex).text();
            var strs = textInputText.split('|');
            autoNodes.eq(highlightindex).css("background-color", "Silver");
            $("#" + tbid).val(strs[0]);
            $("#divinfo").hide();
            oldValue = strs[0];

            //  Filling .
            FullIt(oldValue, tbid, fk_mapExt);
            highlightindex = -1;
        }
    }
    else {
        if (e != oldValue) {
            $("#divinfo").empty();
            var url = GetLocalWFPreHref();
            var json_data = { "Key": e, "FK_MapExt": fk_mapExt, "KVs": kvs };
            $.ajax({
                type: "get",
                url: url + "/WF/CCForm/HanderMapExt.ashx",
                data: json_data,
                beforeSend: function (XMLHttpRequest, fk_mapExt) {
                    //ShowLoading();
                },
                success: function (data, textStatus) {
                    /*  How as wide as the width of the drop-down box to resolve textbox .*/
                    //alert($("#" + tbid));
                    if (data != "") {
                        highlightindex = -1;
                        dataObj = eval("(" + data + ")"); //  Converted to json Object  
                        $.each(dataObj.Head, function (idx, item) {
                            $("#divinfo").append("<div style='" + itemStyle + "' name='" + idx + "' onmouseover='MyOver(this)' onmouseout='MyOut(this)' onclick=\"ItemClick('" + sender.id + "','" + item.No + "','" + tbid + "','" + fk_mapExt + "');\" value='" + item.No + "'>" + item.No + '|' + item.Name + "</div>");
                        });
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    //    alert('HideLoading');
                    //HideLoading();
                },
                error: function () {
                    alert('error when load data.');
                    // Error processing request 
                }
            });
            oldValue = e;
        }
    }
}

function FullIt(oldValue, tbid, fk_mapExt) {

    if (oid == null)
        oid = GetQueryString('OID');

    if (oid == null)
        oid = GetQueryString('WorkID');

    if (oid == null)
        oid = 0;

    // Execution control fills the main table .
    FullCtrl(oldValue, tbid, fk_mapExt);

    // Personalized populate drop-down box to perform .
    FullCtrlDDL(oldValue, tbid, fk_mapExt);

    // Execution filling from the table .
    FullDtl(oldValue, fk_mapExt);

    // Carried out m2m  Relationship filled .
    FullM2M(oldValue, fk_mapExt);
}
// Turn on div.
function openDiv_bak(e, tbID) {

    //alert(document.getElementById("divinfo").style.display);
    if (document.getElementById("divinfo").style.display == "none") {
        var txtObject = document.getElementById(tbID);
        var orgObject = document.getElementById("divinfo");

        var rect = getoffset(txtObject);
        orgObject.style.top = rect[0] + 22;
        orgObject.style.left = rect[1];

        //        orgObject.style.top =  $("#" + tbID).attr("top") + 22;
        //        orgObject.style.left = $("#" + tbID).attr("left");

        orgObject.style.display = "block";
        txtObject.focus();
    }
}
function openDiv(e, tbID) {

    //alert(document.getElementById("divinfo").style.display);
    if (document.getElementById("divinfo").style.display == "none") {

        var txtObject = document.getElementById(tbID);
        var orgObject = document.getElementById("divinfo");
        var rect = getoffset(txtObject);
        var t = rect[0] + 22;
        var l = rect[1];

        orgObject.style.top = t + 'px';
        orgObject.style.left = l + 'px';
        orgObject.style.display = "block";
        txtObject.focus();
    }
}
function getoffset(e) {
    var t = e.offsetTop;
    var l = e.offsetLeft;
    while (e = e.offsetParent) {
        t += e.offsetTop;
        l += e.offsetLeft;
    }
    var rec = new Array(1);
    rec[0] = t;
    rec[1] = l;
    return rec
}

/*  Built-in Pop Automatic return value . */
function ReturnValCCFormPopVal(ctrl, fk_mapExt, refEnPK) {
    //update by dgq  Modify the path 
    //url = 'CCForm/FrmPopVal.aspx?FK_MapExt=' + fk_mapExt + '&RefPK=' + refEnPK + '&CtrlVal=' + ctrl.value;
    var wfpreHref = GetLocalWFPreHref();
    url = wfpreHref + '/WF/CCForm/FrmPopVal.aspx?FK_MapExt=' + fk_mapExt + '&RefPK=' + refEnPK + '&CtrlVal=' + ctrl.value;
    var v = window.showModalDialog(url, 'opp', 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    if (v == null || v == '' || v == 'NaN') {
        return;
    }
    ctrl.value = v;
    return;
}


/*  ReturnValTBFullCtrl */
function ReturnValTBFullCtrl(ctrl, fk_mapExt) {
    var wfPreHref = GetLocalWFPreHref();
    var url = wfPreHref + '/WF/CCForm/FrmReturnValTBFullCtrl.aspx?CtrlVal=' + ctrl.value + '&FK_MapExt=' + fk_mapExt;
    var v = window.showModalDialog(url, 'wd', 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    if (v == null || v == '' || v == 'NaN') {
        return;
    }
    ctrl.value = v;
    //  Filling .
    FullIt(oldValue, ctrl.id, fk_mapExt);
    return;
}

var kvs = null;
function GenerPageKVs() {
    var ddls = null;
    ddls = parent.document.getElementsByTagName("select");
    kvs = "";
    for (var i = 0; i < ddls.length; i++) {
        var id = ddls[i].name;

        if (id.indexOf('DDL_') == -1) {
            continue;
        }
        var myid = id.substring(id.indexOf('DDL_') + 4);
        kvs += '~' + myid + '=' + ddls[i].value;
    }

    ddls = document.getElementsByTagName("select");
    for (var i = 0; i < ddls.length; i++) {
        var id = ddls[i].name;

        if (id.indexOf('DDL_') == -1) {
            continue;
        }
        var myid = id.substring(id.indexOf('DDL_') + 4);
        kvs += '~' + myid + '=' + ddls[i].value;
    }
    return kvs;
}

//var kvs = null;
//function GenerPageKVs() {
//    var ddls = document.getElementsByTagName("select");
//    kvs = "";
//    for (var i = 0; i < ddls.length; i++) {
//        var id = ddls[i].name;
//        var myid = id.substring(id.indexOf('DDL_') + 4);
//        kvs += '~' + myid + '=' + ddls[i].value;
//        //  if (ddls[i].type == "text" || ddls[i].type == "checkbox") {
//        //}
//    }
//}
/*  Automatic filling  */
function DDLFullCtrl(e, ddlChild, fk_mapExt) {
    GenerPageKVs();
    var url = GetLocalWFPreHref();
    var json_data = { "Key": e, "FK_MapExt": fk_mapExt, "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx?KVs=" + kvs,
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {
            if (data) {
                var dataObj = eval("(" + data + ")"); // Converted to json Object  
                for (var i in dataObj.Head) {
                    if (typeof (i) == "function")
                        continue;
                    FullIt(e, ddlChild, fk_mapExt);
                    return;
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}
/*  Cascading drop-down box */
function DDLAnsc(e, ddlChild, fk_mapExt) {
    GenerPageKVs();
    var url = GetLocalWFPreHref();
    var json_data = { "Key": e, "FK_MapExt": fk_mapExt, "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {

            //  Here we must set about obtaining external data .
            // var seleValOfOld = $("#" + ddlChild).selectedindex;
            // alert(seleValOfOld);
            //  Get originally selected value .

            var oldVal = null;
            var ddl = document.getElementById(ddlChild);
            var mylen = ddl.options.length - 1;
            while (mylen >= 0) {
                if (ddl.options[mylen].selected) {
                    oldVal = ddl.options[mylen].value;
                }
                mylen--;
            }

            //alert(oldVal);

            $("#" + ddlChild).empty();

            if (data == "")
                return;

            var dataObj = eval("(" + data + ")"); // Converted to json Object .

            $.each(dataObj.Head, function (idx, item) {
                $("#" + ddlChild).append("<option value='" + item.No + "'>" + item.Name + "</option");
            });

            var isInIt = false;
            mylen = ddl.options.length - 1;
            while (mylen >= 0) {
                if (ddl.options[mylen].value == oldVal) {
                    ddl.options[mylen].selected = true;
                    isInIt = true;
                    break;
                }
                mylen--;
            }
            if (isInIt == false) {

                $("#" + ddlChild).append("<option value='" + oldVal + "' selected='selected' >* Please select </option");
				$("#" + ddlChild).attr("value",oldVal);
            }
            //  alert(oldVal);
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}


function FullM2M(key, fk_mapExt) {
    //alert(key);
    GenerPageKVs();
    var url = GetLocalWFPreHref();
    var json_data = { "Key": key, "FK_MapExt": fk_mapExt, "DoType": "ReqM2MFullList", "OID": oid, "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {
            if (data == "")
                return;

            var dataObj = eval("(" + data + ")"); // Converted to json Object .
            for (var i in dataObj.Head) {
                if (typeof (i) == "function")
                    continue;

                for (var k in dataObj.Head[i]) {
                    var fullM2M = dataObj.Head[i][k];
                    var frm = document.getElementById('F' + fullM2M);
                    if (frm == null)
                        continue;

                    var src = frm.src;
                    var idx = src.indexOf("&Key");
                    if (idx == -1)
                        src = src + '&Key=' + key + '&FK_MapExt=' + fk_mapExt;
                    else
                        src = src.substring(0, idx) + '&Key=' + key + '&FK_MapExt=' + fk_mapExt;
                    frm.src = src;
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}

// Filling details .
function FullDtl(key, fk_mapExt) {

    GenerPageKVs();
    var url = GetLocalWFPreHref();
    //FullM2M(key, fk_mapExt); // Filling M2M.
    var json_data = { "Key": key, "FK_MapExt": fk_mapExt, "DoType": "ReqDtlFullList", "OID": oid, "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {
            if (data == "")
                return;

            var dataObj = eval("(" + data + ")"); // Converted to json Object .
            for (var i in dataObj.Head) {
                if (typeof (i) == "function")
                    continue;

                for (var k in dataObj.Head[i]) {
                    var fullDtl = dataObj.Head[i][k];
                    //  alert(' Are you sure you want to fill it from the table ?, Inside the data is to be deleted .' + key + ' ID= ' + fullDtl);
                    var frm = document.getElementById('F' + fullDtl);
                    var src = frm.src;
                    var idx = src.indexOf("&Key");
                    if (idx == -1)
                        src = src + '&Key=' + key + '&FK_MapExt=' + fk_mapExt;
                    else
                        src = src.substring(0, idx) + '&Key=' + key + '&FK_MapExt=' + fk_mapExt;
                    frm.src = src;
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}

function FullCtrlDDL(key, ctrlIdBefore, fk_mapExt) {
    GenerPageKVs();
    var url = GetLocalWFPreHref();
    var json_data = { "Key": key, "FK_MapExt": fk_mapExt, "DoType": "ReqDDLFullList", "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {
            if (data == "")
                return;

            var dataObj = eval("(" + data + ")"); // Converted to json Object  
            var beforeID = ctrlIdBefore.substring(0, ctrlIdBefore.indexOf('DDL_'));
            var endId = ctrlIdBefore.substring(ctrlIdBefore.lastIndexOf('_'));

            for (var i in dataObj.Head) {
                if (typeof (i) == "function")
                    continue;

                for (var k in dataObj.Head[i]) {
                    var fullDDLID = dataObj.Head[i][k];

                    //alert(fullDDLID);
                    FullCtrlDDLDB(key, fullDDLID, beforeID, endId, fk_mapExt);
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}
function FullCtrlDDLDB(e, ddlID, ctrlIdBefore, endID, fk_mapExt) {
    GenerPageKVs();
    // alert('FullCtrlDDLDBs:' + ddlID + ' ctrlIdBefore: ' + ctrlIdBefore);
    var url = GetLocalWFPreHref();
    var json_data = { "Key": e, "FK_MapExt": fk_mapExt, "DoType": "ReqDDLFullListDB", "MyDDL": ddlID, "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {

            //     alert(textStatus);

            endID = endID.replace('_', '');
            if (endID != parseInt(endID)) {
                endID = "";
            } else {
                endID = "_" + endID;
            }
            var id = ctrlIdBefore + "DDL_" + ddlID + "" + endID;
            // alert('FullCtrlDDLDB:' + id);

            $("#" + id).empty();
            var dataObj = eval("(" + data + ")"); // Converted to json Object  
            //alert(data);
            //alert( $("#" + id) );
            //            $.each(dataObj.Head, function (idx, item) {
            //                $("#" + ddlChild).append("<option value='" + item.No + "'>" + item.Name + "</option");
            //            });

            $.each(dataObj.Head, function (idx, item) {
                //                alert(idx);
                //                alert(item.No);
                //                alert(item.Name);
                $("#" + id).append("<option value='" + item.No + "'>" + item.Name + "</option");
                //$("#" + id).append("<option value='" + item.No + "'>" + item.Name + "</option");
            });
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
        }
    });
}

function FullCtrl(e, ctrlIdBefore, fk_mapExt) {
    e = escape(e);
    GenerPageKVs();
    var url = GetLocalWFPreHref();
    var json_data = { "Key": e, "FK_MapExt": fk_mapExt, "DoType": "ReqCtrl", "KVs": kvs };
    $.ajax({
        type: "get",
        url: url + "/WF/CCForm/HanderMapExt.ashx",
        data: json_data,
        beforeSend: function (XMLHttpRequest) {
            //ShowLoading();
        },
        success: function (data, textStatus) {
            var dataObj = eval("(" + data + ")"); // Converted to json Object  
            var beforeID = null;
            var endId = null;

            //  According to ddl 与 tb  Different . 
            if (ctrlIdBefore.indexOf('DDL_') > 1) {
                beforeID = ctrlIdBefore.substring(0, ctrlIdBefore.indexOf('DDL_'));
                endId = ctrlIdBefore.substring(ctrlIdBefore.lastIndexOf('_'));
            } else {
                beforeID = ctrlIdBefore.substring(0, ctrlIdBefore.indexOf('TB_'));
                endId = ctrlIdBefore.substring(ctrlIdBefore.lastIndexOf('_'));
            }

            for (var i in dataObj.Head) {
                if (typeof (i) == "function")
                    continue;

                for (var k in dataObj.Head[i]) {
                    if (k == 'No' || k == 'Name')
                        continue;

                    //  alert(k + ' val= ' + dataObj.Head[i][k]);

                    $("#" + beforeID + 'TB_' + k).val(dataObj.Head[i][k]);
                    $("#" + beforeID + 'TB_' + k + endId).val(dataObj.Head[i][k]);

                    $("#" + beforeID + 'DDL_' + k).val(dataObj.Head[i][k]);
                    $("#" + beforeID + 'DDL_' + k + endId).val(dataObj.Head[i][k]);

                    if (dataObj.Head[i][k] == '1') {
                        $("#" + beforeID + 'CB_' + k).attr("checked", true);
                        $("#" + beforeID + 'CB_' + k + endId).attr("checked", true);
                    } else {
                        $("#" + beforeID + 'CB_' + k).attr("checked", false);
                        $("#" + beforeID + 'CB_' + k + endId).attr("checked", false);
                    }
                }
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
            //HideLoading();
        },
        error: function () {
            // Error processing request 
            alert('error where funnCtrl');
        }
    });
}

var itemStyle = 'padding:2px;color: #000000;background-color:Silver;width:100%;border-style: none double double none;border-width: 1px;';
var itemStyleOfS = 'padding:2px;color: #000000;background-color:green;width:100%';
function ItemClick(sender, val, tbid, fk_mapExt) {

    $("#divinfo").empty();
    $("#divinfo").css("display", "none");
    highlightindex = -1;
    oldValue = val;

    $("#" + tbid).val(oldValue);

    //  Filling .
    FullIt(oldValue, tbid, fk_mapExt);
}

function MyOver(sender) {
    if (highlightindex != -1) {
        $("#divinfo").children("div").eq(highlightindex).css("background-color", "Silver");
    }

    highlightindex = $(sender).attr("name");
    $(sender).css("background-color", "blue");
    $(sender).css("color", "white");
}

function MyOut(sender) {
    $(sender).css("background-color", "Silver");
    $(sender).css("color", "black");
}

function hiddiv() {
    $("#divinfo").css("display", "none");
}

//  Get parameters .
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return unescape(r[2]);
    return null;
}
// Through regular expressions to detect 
function CheckInput(oInput, filter) {
    var re = filter;
    if (typeof (filter) == "string") {
        re = new RegExp(filter);
    }
    return re.test(oInput);
}
// Input inspection 
function txtTest_Onkeyup(ele, filter, message) {
    if (ele == null) return;
    var re = filter;
    if (typeof (filter) == "string") {
        re = new RegExp(filter);
    }
    var format = re.test(ele.value);
    if (!format) {
        ele.value = "";
        alert(message);
    }
}
// Input inspection 
function EleInputCheck(ele, filter, message) {
    if (ele == null) return;
    if (CheckInput(ele.value, filter) == true) {
        ele.title = "";
        ele.style.border = "1";
        ele.style.backgroundColor = "White";
        ele.style.borderBottomColor = "Black";
    }
    else {
        ele.title = message;
        ele.style.border = "2";
        ele.style.backgroundColor = "#FFDEAD";
        ele.style.borderBottomColor = "Red";
    }
}
function EleInputCheck2(ele, filter, message) {
    if (ele == null) return;

    var reg = new RegExp(filter);
    var isPass = reg.test(ele.value);
    if (isPass == true) {
        ele.title = "";
        ele.style.border = "1";
        ele.style.backgroundColor = "White";
        ele.style.borderBottomColor = "Black";
    }
    else {
        ele.title = message;
        ele.style.border = "2";
        ele.style.backgroundColor = "#FFDEAD";
        ele.style.borderBottomColor = "Red";
    }
}

function EleSubmitCheck(ele,message) {
        ele.title = message;
        ele.style.border = "2";
        ele.style.backgroundColor = "#FFDEAD";
        ele.style.borderBottomColor = "Red";
}

// Save inspection 
function EleSubmitCheck(ele, filter, message) {
    if (ele == null) return;
    if (CheckInput(ele.value, filter) == true) {
        ele.title = "";
        ele.style.border = "1";
        ele.style.backgroundColor = "White";
        ele.style.borderBottomColor = "Black";
        return true;
    }
    else {
        ele.title = message;
        ele.style.border = "2";
        ele.style.backgroundColor = "#FFDEAD";
        ele.style.borderBottomColor = "Red";
        return false;
    }
}

function TB_ClickNum(ele, defVal) {
    if (ele == null) return;
    if (ele.value == "" && typeof (defVal) != 'undefined') {
        ele.value = defVal;
        return;
    }
    
    // Assignment 
    if (ele.value == "0") ele.value = "";
    if (ele.value == "0.00") ele.value = "";
    // Determine the number of decimal places 
    var pointNum = ele.value.split('.');
    if (pointNum) {
        if (pointNum.length > 2) {
            ele.value = pointNum[0] +"."+ pointNum[1];
        }
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