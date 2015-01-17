function getLodop(oOBJECT, oEMBED) {
    /**************************
     This function uses which object type is determined based on the browser as a control instance :
    IE Series ,IE The browser uses the kernel series oOBJECT,
     Other browsers (Firefox Series ,Chrome Series ,Opera Series ,Safari Series ) Adoption oEMBED,
     For 64 Bit browser to 64 Bit installer install_lodop64.exe.
    **************************/
    var strHtmInstall = "<br><font color='#FF00FF'> Print control is not installed ! Click here <a href='/DataUser/PrintTools/install_lodop32.exe' target='_self'> Installation </a>, Please refresh the page after the installation or re-enter .</font>";
    var strHtmUpdate = "<br><font color='#FF00FF'> Print controls need to be upgraded ! Click here <a href='/DataUser/PrintTools/install_lodop32.exe' target='_self'> Perform the upgrade </a>, Please re-enter after upgrade .</font>";
    var strHtm64_Install = "<br><font color='#FF00FF'> Print control is not installed ! Click here <a href='/DataUser/PrintTools/install_lodop64.exe' target='_self'> Installation </a>, Please refresh the page after the installation or re-enter .</font>";
    var strHtm64_Update = "<br><font color='#FF00FF'> Print controls need to be upgraded ! Click here <a href='/DataUser/PrintTools/install_lodop64.exe' target='_self'> Perform the upgrade </a>, Please re-enter after upgrade .</font>";
    var strHtmFireFox = "<br><br><font color='#FF00FF'> Watch out :<br>1: As has been installed Lodop Older Accessories npActiveXPLugin, Please 【 Tool 】->【 Add-ons 】->【 Expand 】 Zhongxianxieta .</font>";
    var LODOP = oEMBED;
    try {
        if (navigator.appVersion.indexOf("MSIE") >= 0) LODOP = oOBJECT;
        if ((LODOP == null) || (typeof (LODOP.VERSION) == "undefined")) {
            if (navigator.userAgent.indexOf('Firefox') >= 0)
                document.documentElement.innerHTML = strHtmFireFox + document.documentElement.innerHTML;
            if (navigator.userAgent.indexOf('Win64') >= 0) {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtm64_Install); else
                    document.documentElement.innerHTML = strHtm64_Install + document.documentElement.innerHTML;
            } else {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtmInstall); else
                    document.documentElement.innerHTML = strHtmInstall + document.documentElement.innerHTML;
            }
            return LODOP;
        } else if (LODOP.VERSION < "6.1.4.5") {
            if (navigator.userAgent.indexOf('Win64') >= 0) {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtm64_Update); else
                    document.documentElement.innerHTML = strHtm64_Update + document.documentElement.innerHTML;
            } else {
                if (navigator.appVersion.indexOf("MSIE") >= 0) document.write(strHtmUpdate); else
                    document.documentElement.innerHTML = strHtmUpdate + document.documentElement.innerHTML;
            }
            return LODOP;
        }
        //===== Calls for unity as empty space function :=====	     


        //=======================================
        return LODOP;
    } catch (err) {
        if (navigator.userAgent.indexOf('Win64') >= 0)
            document.documentElement.innerHTML = "Error:" + strHtm64_Install + document.documentElement.innerHTML; else
            document.documentElement.innerHTML = "Error:" + strHtmInstall + document.documentElement.innerHTML;
        return LODOP;
    }
}
