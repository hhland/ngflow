function isPasswd(s) {
    var patrn = /^(\w){6,20}$/;
    if (!patrn.exec(s.value)) 
    {
       alert(' Illegal password format .');
       return false;
    }
    return true
}