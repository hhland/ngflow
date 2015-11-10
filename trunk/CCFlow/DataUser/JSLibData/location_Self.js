function CheckBlank() {
    
    if (ReqTBIsexist(["L_NAME"])) {
        alert("Location ID " + ReqTB("L_NAME") + " is exist, please check it and create it again");
        return false;
    }
    return true;
}