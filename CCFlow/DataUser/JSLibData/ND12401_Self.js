function CheckBlank() {
    var msg = "";

    //Js script check the if T3XT is empty?
    msg += $.trim(ReqTB("T3XT"))==""?"T3XT can't be empty .\n":"";

    msg += ReqDDL("CanidateList")!="1"?"CanidateList must be accepted .\n":"";

    if (msg != "") {
        alert(msg);
        return false;
    }
    return true;
}


