function CheckBlank() {
    var msg = "";

    //Js script check the if T3XT is empty?
    msg += ReqTB("Mt3_1") != "Admin" ? "you are not admin .\n" : "";
    //11 Beijing , 50 Chongqing
    var FK_SF_val = ReqDDL("FK_SF");

    msg += (FK_SF_val != 11 && FK_SF_val!=50) ? "Province or City is not correct .\n" : "";

    if (msg != "") {
        alert(msg);
        return false;
    }
    return true;
}