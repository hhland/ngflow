function isMobil(s) {
    var patrn = /^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
    if (!patrn.exec(s.value)) 
    {
       alert(' Illegal phone number .');
       return false;
    }
    return true
}