
/**
*  Check the minimum length of the maximum length .
*  Enter :str   String 
*   Return :true 或 flase; true Represents the correct format 
*/
function checkLength(tb, minLen, maxLen) {
    if (tb.value.length < minLen || tb.value.length > maxLen) {
        alert(' Error : Length of the input must be ' + minLen + ' 到 ' + maxLen + ' Between .');
    }
}

/*  Is mailbox  */
function isEmail(tb) {
    if (tb.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) != -1)
        return true;
    alert(' Error message format .');
}

/**
*  Check the phone number entered in the correct format 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
*/
function checkMobilePhone(str) {
    if (str.match(/^(?:13\d|15[89])-?\d{5}(\d{3}|\*{3})$/) == null) {
        return false;
    }
    else {
        return true;
    }
}

/**
*  Fixed telephone number to check whether the correct input 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
*/
function checkTelephone(str) {
    if (str.match(/^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?$/) == null) {
        return false;
    }
    else {
        return true;
    }
}


/**
*  Check the input of the ID number is correct 
*  Enter :str   String 
*   Return :true 或 flase; true Represents the correct format 
*/
function checkCard(str) {
    //15 Digit ID regex 
    var arg1 = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/;
    //18 Digit ID regex 
    var arg2 = /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/;
    if (str.match(arg1) == null && str.match(arg2) == null) {
        return false;
    }
    else {
        return true;
    }
}




/****************************  Not finishing  **************************************/

/**
* 2009-10-01
* 贺  臣
* 情  缘
* js Various forms of data validation 
*/
/**************************************************************************************/
/************************************* Digital Authentication *****************************************/
/**************************************************************************************/
/**
*  Check whether the input string of characters all digital 
*  Enter :str   String 
*  Return :true 或 flase; true Expressed as a number 
*/
function checkNum(str) {
    return str.match(/\D/) == null;
}


/**
*  Check the input string character is a decimal 
*  Enter :str   String 
*  Return :true 或 flase; true Expressed as a decimal 
*/
function checkDecimal(str) {
    if (str.match(/^-?\d+(\.\d+)?$/g) == null) {
        return false;
    }
    else {
        return true;
    }
}

/**************************************************************************************/
/************************************* Verification characters *****************************************/
/**************************************************************************************/


/**
*  Check whether the input string of characters is a character 
*  Enter :str   String 
*  Return :true 或 flase; true Expressed as all character   Does not include Chinese characters 
*/
function checkStr(str) {
    if (/[^\x00-\xff]/g.test(str)) {
        return false;
    }
    else {
        return true;
    }
}


/**
*  Check the input of a string of characters contains Kanji 
*  Enter :str   String 
*  Return :true 或 flase; true Representation contains Kanji 
*/
function checkChinese(str) {
    if (escape(str).indexOf("%u") != -1) {
        return true;
    }
    else {
        return false;
    }
}


/**
*  Check the mailbox entered in the correct format 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
*/
function checkEmail(str) {
    if (str.match(/[A-Za-z0-9_-]+[@](\S*)(net|com|cn|org|cc|tv|[0-9]{1,3})(\S*)/g) == null) {
        return false;
    }
    else {
        return true;
    }
}





/**
*  Inspection QQ The correct format 
*  Enter :str   String 
*   Return :true 或 flase; true Represents the correct format 
*/
function checkQQ(str) {
    if (str.match(/^\d{5,10}$/) == null) {
        return false;
    }
    else {
        return true;
    }
}


/**
*  Check the input IP Address is correct 
*  Enter :str   String 
*   Return :true 或 flase; true Represents the correct format 
*/
function checkIP(str) {
    var arg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
    if (str.match(arg) == null) {
        return false;
    }
    else {
        return true;
    }
}

/**
*  Check the input URL Address is correct 
*  Enter :str   String 
*   Return :true 或 flase; true Represents the correct format 
*/
function checkURL(str) {
    if (str.match(/(http[s]?|ftp):\/\/[^\/\.]+?\..+\w$/i) == null) {
        return false
    }
    else {
        return true;
    }
}


/**************************************************************************************/
/************************************* Verification time *****************************************/
/**************************************************************************************/

/**
*  Check the date format is correct 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
*  Watch out : Chinese date formats can not be verified here 
*  Verify that the short date （2007-06-05）
*/
function checkDate(str) {
    //var value=str.match(/((^((1[8-9]\d{2})|([2-9]\d{3}))(-)(10|12|0?[13578])(-)(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(11|0?[469])(-)(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(0?2)(-)(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)(-)(0?2)(-)(29)$)|(^([3579][26]00)(-)(0?2)(-)(29)$)|(^([1][89][0][48])(-)(0?2)(-)(29)$)|(^([2-9][0-9][0][48])(-)(0?2)(-)(29)$)|(^([1][89][2468][048])(-)(0?2)(-)(29)$)|(^([2-9][0-9][2468][048])(-)(0?2)(-)(29)$)|(^([1][89][13579][26])(-)(0?2)(-)(29)$)|(^([2-9][0-9][13579][26])(-)(0?2)(-)(29)$))/);
    var value = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (value == null) {
        return false;
    }
    else {
        var date = new Date(value[1], value[3] - 1, value[4]);
        return (date.getFullYear() == value[1] && (date.getMonth() + 1) == value[3] && date.getDate() == value[4]);
    }
}

/**
*  Check whether the correct time format 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
*  Verification Time (10:57:10)
*/
function checkTime(str) {
    var value = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/)
    if (value == null) {
        return false;
    }
    else {
        if (value[1] > 24 || value[3] > 60 || value[4] > 60) {
            return false
        }
        else {
            return true;
        }
    }
}

/**
*  Check the full date and time format is correct 
*  Enter :str   String 
*  Return :true 或 flase; true Represents the correct format 
* (2007-06-05 10:57:10)
*/
function checkFullTime(str) {
    //var value = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/);
    var value = str.match(/^(?:19|20)[0-9][0-9]-(?:(?:0[1-9])|(?:1[0-2]))-(?:(?:[0-2][1-9])|(?:[1-3][0-1])) (?:(?:[0-2][0-3])|(?:[0-1][0-9])):[0-5][0-9]:[0-5][0-9]$/);
    if (value == null) {
        return false;
    }
    else {
        //var date = new Date(checkFullTime[1], checkFullTime[3] - 1, checkFullTime[4], checkFullTime[5], checkFullTime[6], checkFullTime[7]);
        //return (date.getFullYear() == value[1] && (date.getMonth() + 1) == value[3] && date.getDate() == value[4] && date.getHours() == value[5] && date.getMinutes() == value[6] && date.getSeconds() == value[7]);
        return true;
    }

}

/**************************************************************************************/
/************************************ Verify identity card numbers *************************************/
/**************************************************************************************/

/**  
*  ID card 15 Bit Encoding Rules :dddddd yymmdd xx p
* dddddd: Area code 
* yymmdd:  Date of Birth 
* xx:  Class coding sequence , Can not be determined 
* p:  Sex , Odd for men , Even number for women 
* <p />
*  ID card 18 Bit Encoding Rules :dddddd yyyymmdd xxx y
* dddddd: Area code 
* yyyymmdd:  Date of Birth 
* xxx: Class coding sequence , Can not be determined , Odd for men , Even number for women 
* y:  Checksum , Before this bit value through 17 Bit calculated 
* <p />
* 18 Digit number weighting factor for ( Right to left ) Wi = [ 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2,1 ]
*  Validation bit  Y = [ 1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2 ]
*  The parity bit is calculated :Y_P = mod( ∑(Ai×Wi),11 )
* i ID number for the number from right to left  2...18 位; Y_P To feet checksum checksum array locations where 
*
*/
var Wi = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1]; //  Weighting factor    
var ValideCode = [1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2]; //  Identity verification bit values .10 On behalf of X   
function IdCardValidate(idCard) {
    idCard = trim(idCard.replace(/ /g, ""));
    if (idCard.length == 15) {
        return isValidityBrithBy15IdCard(idCard);
    }
    else
        if (idCard.length == 18) {
            var a_idCard = idCard.split(""); //  Get ID array    
            if (isValidityBrithBy18IdCard(idCard) && isTrueValidateCodeBy18IdCard(a_idCard)) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
}

/**  
*  To determine the identity card number 18 When the last bit of validation bit is correct 
* @param a_idCard  ID number array 
* @return
*/
function isTrueValidateCodeBy18IdCard(a_idCard) {
    var sum = 0; //  Declare variables weighted sum    
    if (a_idCard[17].toLowerCase() == 'x') {
        a_idCard[17] = 10; //  The last bit is x Replace the verification code 10 Facilitate subsequent operations    
    }
    for (var i = 0; i < 17; i++) {
        sum += Wi[i] * a_idCard[i]; //  Weighted sum    
    }
    valCodePosition = sum % 11; //  Get the location code    
    if (a_idCard[17] == ValideCode[valCodePosition]) {
        return true;
    }
    else {
        return false;
    }
}

/**  
*  By ID judge is male or female 
* @param idCard 15/18 Digit ID number 
* @return 'female'-女,'male'-男
*/
function maleOrFemalByIdCard(idCard) {
    idCard = trim(idCard.replace(/ /g, "")); //  We handle on the identity card number . Including spaces between characters .   
    if (idCard.length == 15) {
        if (idCard.substring(14, 15) % 2 == 0) {
            return 'female';
        }
        else {
            return 'male';
        }
    }
    else
        if (idCard.length == 18) {
            if (idCard.substring(14, 17) % 2 == 0) {
                return 'female';
            }
            else {
                return 'male';
            }
        }
        else {
            return null;
        }
}

/**  
*  Verification 18 Digit ID numbers are valid birthday birthday 
* @param idCard 18 Bit string ID book 
* @return
*/
function isValidityBrithBy18IdCard(idCard18) {
    var year = idCard18.substring(6, 10);
    var month = idCard18.substring(10, 12);
    var day = idCard18.substring(12, 14);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    //  Here with getFullYear() Get Year , Avoid the millennium bug    
    if (temp_date.getFullYear() != parseFloat(year) ||
    temp_date.getMonth() != parseFloat(month) - 1 ||
    temp_date.getDate() != parseFloat(day)) {
        return false;
    }
    else {
        return true;
    }
}

/**  
*  Verification 15 Digit ID numbers are valid birthday birthday 
* @param idCard15 15 Bit string ID book 
* @return
*/
function isValidityBrithBy15IdCard(idCard15) {
    var year = idCard15.substring(6, 8);
    var month = idCard15.substring(8, 10);
    var day = idCard15.substring(10, 12);
    var temp_date = new Date(year, parseFloat(month) - 1, parseFloat(day));
    //  For the old identity of your age you do not need to consider the use of the millennium bug problem getYear() Method    
    if (temp_date.getYear() != parseFloat(year) ||
    temp_date.getMonth() != parseFloat(month) - 1 ||
    temp_date.getDate() != parseFloat(day)) {
        return false;
    }
    else {
        return true;
    }
}

// Remove string trailing spaces    
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}
