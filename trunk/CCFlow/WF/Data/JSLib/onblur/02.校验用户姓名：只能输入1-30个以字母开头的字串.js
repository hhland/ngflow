function isTrueName(s) {
    var patrn = /^[a-zA-Z]{1,30}$/;
    if (!patrn.exec(s.value))
    {
       alert(' Illegal user name format .');
       return false;
    }
    return true
} 