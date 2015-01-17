
//  Whether dealing with some controls available .
function WhenClickCheckBoxClick(cb) {

    // Use ccform The built-in function to obtain a control based on the field name , Then assign attributes to it .
    ReqTBObj('JiaTingZhuZhi').disabled = !cb.checked;
    ReqTBObj('JiaTingDianHua').disabled = !cb.checked;
    ReqDDLObj('FK_SF').disabled = !cb.checked;
    ReqDDLObj('XingBie').disabled = !cb.checked;
    ReqCBObj('HunFou').disabled = !cb.checked;

    // Let the control change background color .
    var color = 'InfoBackground';
    if (cb.checked) {
        color = 'white';
    }
    ReqTBObj('JiaTingZhuZhi').style.backgroundColor = color;
    ReqTBObj('JiaTingDianHua').style.backgroundColor = color;
    ReqDDLObj('FK_SF').style.backgroundColor = color;
    ReqDDLObj('XingBie').style.backgroundColor = color;
    ReqCBObj('HunFou').style.backgroundColor = color;
    return;
}
