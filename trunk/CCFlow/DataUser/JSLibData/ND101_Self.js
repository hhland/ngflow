
//  Perform a security check before sending data .
function CheckBlank() {
    alert('sss');
    return true;


    var msg = "";
    if (ReqAthFileName('GaoJian') == null) {
        msg += ' You do not have to upload file attachments  \t\n';
    }
    if (ReqTB('BianXiaoRen') == "") {
        msg += ' Redaction people : Can not be empty  \t\n';
    }

    if (ReqTB('BianXiaoRenDianHua') == "") {
        msg += ' Redaction Phone : Can not be empty  \t\n';
    }

    if (ReqTB('QianFaRen') == "") {
        msg += ' Issuer : Can not be empty  \t\n';
    }

    if (ReqTB('QianFaRenDianHua') == "") {
        msg += ' Issuer Phone : Can not be empty  \t\n';
    }

    if (ReqTB('WenZhangBiaoTi') == "") {
        msg += ' Article Title : Can not be empty  \t\n';
    }
    if (msg == "")
        return true; /* You can submit .*/
    alert(msg);
    return false; /* Can not be submitted .*/
}
