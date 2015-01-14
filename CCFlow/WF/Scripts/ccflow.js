
/*  Built-in Pop Automatic return value .
    Use : 
1,  If you want to use this function , Needs  \datauser\Xml\popval.xml  Inside the configuration in accordance with the requirements of the format examples .
2,  The introduction of this file to your page .
*/
function PopVal(ctrlID, ctrlID1, popNameInXML) {


    var ctrl = document.getElementById(ctrlID);
    if (ctrl == null)
        ctrl = document.getElementsByName(ctrlID);

    if (ctrl == null) {
        ctrl = document.getElementById(ctrlID1);
    }
    if (ctrl == null) {
        ctrl = document.getElementsByName(ctrlID1);
    }

    if (ctrl == null) {
        alert('ERR:' + ctrlID + ' Not found ');
        return;
    }

    var url = '/WF/CCForm/FrmPopVal.aspx?FK_MapExt=' + popNameInXML + '&CtrlVal=' + ctrl.value;
    var v = window.showModalDialog(url, 'opp', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
    if (v == null || v == '' || v == 'NaN') {
        return;
    }
    ctrl.value = v;
    return;
}

function WorkReturn(fk_flow, fk_node, workid, fid) {
    var url = "/WF/ReturnWorkSmall.aspx?FK_Node=" + fk_node + "&FID=" + fid + "&WorkID=" + workid + "&FK_Flow=" + fk_flow + "&s=2233";
    var v = window.showModalDialog(url, 'opp', 'dialogHeight: 550px; dialogWidth: 650px; dialogTop: 100px; dialogLeft: 150px; center: yes; help: no');
 

    return v;
}



