function isPostalCode(s) {
    var patrn = /^[a-zA-Z0-9 ]{3,12}$/;
    if (!patrn.exec(s.value)) 
    {
       alert(' Illegal ZIP code format .');
       return false;
    }
    return true
} 
