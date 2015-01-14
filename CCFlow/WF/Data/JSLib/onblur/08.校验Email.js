function checkMail(c) {
    var val = c.value;
    if (val == '')
        return;

    // Presence @ Symbol 
    var a = val.indexOf("@");
    // The existence of points 
    var point = val.indexOf(".");
    // Presence @,点, And   Point @ After that , Not adjacent 
    if (a == -1 || point == -1 || point - a <= 1) {
        alert("Email Incorrect format . Right example abc@ccflow.org");
        c.select();
        c.focus();
        return false;
    }
    //@ Can not be the first character , Point can not be the last character 
    if (a == 0 || point == val.length - 1) {
        alert("Email Incorrect format . Right example abc@ccflow.org");
        c.select();
        c.focus();
        return false;
    }
    return true;
}