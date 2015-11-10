// Expand easyui Forms Authentication 
$.extend($.fn.validatebox.defaults.rules, {
    // Verification man 
    chinese: {
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/.test(value);
        },
        message: ' You can only enter Chinese characters '
    },
    // Mobile phone number verification 
    mobile: {//value Value is the value in the text box 
        validator: function (value) {
            var reg = /^1[3|4|5|8|9]\d{9}$/;
            return reg.test(value);
        },
        message: ' Enter the phone number format is not accurate .'
    },
    // ID number verification 
    idcard: {
        validator: function (value, param) {
            return idCard(value);
        },
        message:' Please enter the correct ID number '
    },
    // Domestic zip code verification 
    zipcode: {
        validator: function (value) {
            var reg = /^[1-9]\d{5}$/;
            return reg.test(value);
        },
        message: ' Zip Code must be non- 0 Beginning 6 Digit .'
    },
    // Verify digital 
    number: {
        validator: function (value, param) {
            return /^\d+$/.test(value);
        },
        message: ' Please enter a number '
    },
    // User account verification ( Only include  _  Digital   Letter ) 
    account: {//param Value [] Median 
        validator: function (value, param) {
            if (value.length < param[0] || value.length > param[1]) {
                $.fn.validatebox.defaults.rules.account.message = ' Username length must be at ' + param[0] + '至' + param[1] + ' Range ';
                return false;
            } else {
                if (!/^[\w]+$/.test(value)) {
                    $.fn.validatebox.defaults.rules.account.message = ' Username can only figure , Letter , Underscores .';
                    return false;
                } else {
                    return true;
                }
            }
        }, message: ''
    }
})
var idCard = function (value) {
    if (value.length == 18 && 18 != value.length) return false;
    var number = value.toLowerCase();
    var d, sum = 0, v = '10x98765432', w = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], a = '11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91';
    var re = number.match(/^(\d{2})\d{4}(((\d{2})(\d{2})(\d{2})(\d{3}))|((\d{4})(\d{2})(\d{2})(\d{3}[x\d])))$/);
    if (re == null || a.indexOf(re[1]) < 0) return false;
    if (re[2].length == 9) {
        number = number.substr(0, 6) + '19' + number.substr(6);
        d = ['19' + re[4], re[5], re[6]].join('-');
    } else d = [re[9], re[10], re[11]].join('-');
    if (!isDateTime.call(d, 'yyyy-MM-dd')) return false;
    for (var i = 0; i < 17; i++) sum += number.charAt(i) * w[i];
    return (re[2].length == 9 || number.charAt(17) == v.charAt(sum % 11));
}