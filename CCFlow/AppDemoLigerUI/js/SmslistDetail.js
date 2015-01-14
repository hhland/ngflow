var myPK = "";

// Obtain from a url Passed to the parameter list 
function QueryString() {
    var name, value, i;

    var str = location.href;
    var num = str.indexOf("?")

    str = str.substr(num + 1);
    var arrtmp = str.split("&");

    for (i = 0; i < arrtmp.length; i++) {
        num = arrtmp[i].indexOf("=");
        if (num > 0) {
            name = arrtmp[i].substring(0, num);
            value = arrtmp[i].substr(num + 1);
            this[name] = value;
        }
    }
}

$(function () {

    var Request = new QueryString();
    myPK = Request["MyPK"];

    LoadGrid();
}
)

// Callback 
function callBack(jsonData, scope) {
    if (jsonData) {
        if (jsonData.length > 17) {

            var pushData = eval('(' + jsonData + ')');
            var doc = pushData.Rows[0].Doc.replace(/~/g, "'");

            document.getElementById("tdTitle").innerHTML = pushData.Rows[0].Title;
            document.getElementById("lbSender").innerHTML = pushData.Rows[0].Sender;
            document.getElementById("lbRDT").innerHTML = pushData.Rows[0].RDT.toString();
            document.getElementById("lbSendTo").innerHTML = pushData.Rows[0].SendTo;
            document.getElementById("divDoc").innerHTML = doc;
        }
    }
  
    else {
        $.ligerDialog.warn(' Error loading data , Please retry close !');
    }
    // Modify Read    Status 
    Application.data.upMsgSta(myPK, upMsgSta, this);
}
// Load   Details 
function LoadGrid() {
    Application.data.getDetailSms(myPK, callBack, this);
}
// Modify the data state   2013.05.23 H
function upMsgSta(my_PK, jsonData, scope) {
    if (jsonData) { }
}