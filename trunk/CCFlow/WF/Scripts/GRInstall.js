// Prior to reference this file in a web page , Should be cited CreateControl.js
//document.write("<script type='text/javascript' src='CreateControl.js'></script>");

// Insert a report object , Used to determine whether the plug has been installed , Or if you need to install an updated version 
// This function should be in the pages of <head> Calling , See specific examples  ReportHome.htm  The use of 
function Install_InsertReport()
{
    var typeid;
    if( _gr_isIE )
        typeid = 'classid="clsid:25240C9A-6AA5-416c-8CDA-801BBAF03928" ';
    else
        typeid = 'type="application/x-grplugin-report" ';
    typeid += gr_CodeBase;
	document.write('<object id="_ReportOK" ' + typeid);
	document.write(' width="0" height="0" VIEWASTEXT>');
	document.write('</object>');
}

// Used to determine whether the plug has been installed , Or if you need to install an updated version . If you need to install , Web page insert in the text related to the installation 
// If the plug is installed and do not update , Returns  true, Otherwise it is  false.
// This function should be in the pages of <body> Start position calls , See specific examples  ReportHome.htm  The use of 
function Install_Detect()
{
    var _ReportOK = document.getElementById("_ReportOK");
    if (_ReportOK.Register == undefined) //if ((_ReportOK == null) || (_ReportOK.Register == undefined))
    {
        document.write('<div style="width: 100%; background-color: #fff8dc; text-align: center; vertical-align: middle; line-height: 20pt; padding-bottom: 12px; padding-top: 12px;">');
            document.write('<strong>  This site needs to be installed   Reports plugin sharp waves   In order to ensure their normal operation <br /></strong>');
            
        if( _gr_isIE )
            document.write('<strong><span style="color: #ff0000">  If prompted bar at the top or bottom of the browser , Left-click and run the add tooltip , Click here to install the easiest way </span><br /></strong>');
            
            document.write('<a href="' + gr_InstallPath + '/grbsctl5.exe"><span style="color: #ff0000"><strong> Click here to download the plug-in installation package reports sharp waves <br /></strong></span></a>');
            document.write(' After installing the plug-sharp wave reports ,<a href="#" onclick="javascript:document.location.reload();"> Click here </a>  Reload this site ');
        document.write('</div>');
        return false;
    }
    else if ((_ReportOK.Utility.ShouldUpdatePlugin == undefined) || _ReportOK.Utility.ShouldUpdatePlugin(gr_Version) == true)  // Check whether you should download a new version of the program 
    {
        document.write('<div style="width: 100%; background-color: #fff8dc; text-align: center; vertical-align: middle; line-height: 20pt; padding-bottom: 12px; padding-top: 12px;">');
            document.write('<strong>  This site requires an upgrade installation   Reports plugin sharp waves   In order to ensure their normal operation <br /></strong>');
            document.write('<a href="' + gr_InstallPath + '/grbsctl5.exe"><span style="color: #ff0000"><strong> Click here to download the plug-in installation package reports sharp waves <br /></strong></span></a>');
            document.write(' Must close the page window plug-in installation , Click on the window close button on this page to shut down , After the installation is complete reopen this page <br />');
            document.write(' Such as the installation appears [ Can not open file for writing ...] Prompt , Please close the page window , Then click [ Retry ] Button to continue with the installation ');
        document.write('</div>');
        return false;
    }
    
    return true;
}
