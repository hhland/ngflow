//1, Variable  gr_InstallPath  After the equals sign is the plug-in installation parameter file where web directory , General purpose from the root site 
//    Record start address , Plug-in installation file must exist in the specified directory .
//2,gr_Version  Argument after the equals sign is the version number of plug-in installation package , If a new version of the plug-in installation package , Should upload the new version 
//    The plug-in installation files to the corresponding directory website , And update the version number here .
//3, For more detailed information, please refer to Help [ Reports plugin (WEB Report form )-> Plug-in installation package in the server deployment ] Section 
var gr_InstallPath = "WF/Activex"; // The actual project should be written from addressing the root directory ,如gr_InstallPath="/myapp/report/grinstall"; 
var gr_Version = "5,8,14,110";

// The following registration number for the machine to develop test registration number , Report Access address localhost Logo can be removed when the trial 
// Registration after purchase , Please replace the values of the following variables with your registered user name and registration number 
var gr_UserName = ' Sharp waves statements plug the machine development and testing Register ';
var gr_SerialNo = '4DFB949E066NYS7W11L8KAT53SA177391Q9LZQ094WUT9C9J3813SX8PTQC4ALPB9UAQN6TMA55Q3BN8E5726Z5A839QAD9P6E76TKNK5';

// Reports can only plug in 32 Bit browser in use 
var _gr_platform = window.navigator.platform;
if (_gr_platform.indexOf("64") > 0)
    alert(" Sharp waves Grid++Report Reports can not run plug-in 64 Bit browser , Related reports and print new features will not work properly , Please use 32 Bit browser !");

// Distinction browser (IE or not)
var _gr_agent = navigator.userAgent.toLowerCase();
var _gr_isIE = (_gr_agent.indexOf("msie")>0)? true : false;

var gr_CodeBase;
if( _gr_isIE )
    gr_CodeBase = 'codebase="' + gr_InstallPath + '/grbsctl5.cab#Version=' + gr_Version + '"';
else
    gr_CodeBase = '';

// Create a report object , Report objects are invisible objects , For details, please see the Help  IGridppReport
//Name -  Specified plugin object ID, You can use js Code  document.getElementById("%Name%")  Get the report object 
//EventParams -  Specified event report objects need to respond to the ,如:"<param name='OnInitialize' value=OnInitialize> <param name='OnProcessBegin' value=OnProcessBegin>" Form , You can specify multiple events 
function CreateReport(PluginID, EventParams)
{
    var typeid;
    if( _gr_isIE )
        typeid = 'classid="clsid:25240C9A-6AA5-416c-8CDA-801BBAF03928" ';
    else
        typeid = 'type="application/x-grplugin-report" ';
    typeid += gr_CodeBase;
	document.write('<object id="' + PluginID + '" ' + typeid);
	document.write(' width="0" height="0" VIEWASTEXT>');
	if (EventParams != undefined)
	    document.write(EventParams);
	document.write('</object>');
	
	document.write('<script type="text/javascript">');
	    document.write(PluginID + '.Register("' + gr_UserName + '", "' + gr_SerialNo + '");');
	document.write('</script>');
}

// Create reports with more parameters print displays plug , For details, please see the Help  IGRPrintViewer
//PluginID -  Plug-in ID, By  var ReportViewer = document.getElementById("%PluginID%");  Such a way to get the plug-reference variables 
//Width -  Display width plug ,"100%" For the entire display area width ,"500" Representation 500 Screen pixels 
//Height -  Display height plugin ,"100%" For the entire display area height ,"500" Representation 500 Screen pixels 
//ReportURL -  Get the report template URL
//DataURL -  Get Report Data URL
//AutoRun -  Specifies whether the plug-in automatically generates and presents a report after it is created , Value false或true
//ExParams -  Specify more elaborate plug-in properties , Shaped like : "<param name="%ParamName%" value="%Value%">" Such a parameter string 
function CreatePrintViewerEx2(PluginID, Width, Height, ReportURL, DataURL, AutoRun, ExParams)
{
    var typeid;
    if( _gr_isIE )
        typeid = 'classid="clsid:B7EF88E6-A0AD-4235-B418-6F07D8533A9F" ' + gr_CodeBase;
    else
        typeid = 'type="application/x-grplugin-printviewer"';
	document.write('<object id="' + PluginID + '" ' + typeid);
	document.write(' width="' + Width + '" height="' + Height + '">');
	document.write('<param name="ReportURL" value="' + ReportURL + '">');
	document.write('<param name="DataURL" value="' + DataURL + '">');
	document.write('<param name="AutoRun" value=' + AutoRun + '>');
	document.write('<param name="SerialNo" value="' + gr_SerialNo + '">');
	document.write('<param name="UserName" value="' + gr_UserName + '">');
	document.write(ExParams);
	document.write('</object>');
}

// Create reports with more parameters print displays plug , For details, please see the Help  IGRDisplayViewer
//PluginID -  Plug-in ID, By  var ReportViewer = document.getElementById("%PluginID%");  Such a way to get the plug-reference variables 
//Width -  Display width plug ,"100%" For the entire display area width ,"500" Representation 500 Screen pixels 
//Height -  Display height plugin ,"100%" For the entire display area height ,"500" Representation 500 Screen pixels 
//ReportURL -  Get the report template URL
//DataURL -  Get Report Data URL
//AutoRun -  Specifies whether the plug-in automatically generates and presents a report after it is created , Value false或true
//ExParams -  Specify more elaborate plug-in properties , Shaped like : "<param name="%ParamName%" value="%Value%">" Such a parameter string 
function CreateDisplayViewerEx2(PluginID, Width, Height, ReportURL, DataURL, AutoRun, ExParams)
{
    var typeid;
    if( _gr_isIE )
        typeid = 'classid="clsid:CB45DFE5-6C35-4687-B790-FEC65D512859" ' + gr_CodeBase;
    else
        typeid = 'type="application/x-grplugin-displayviewer"';
	document.write('<object id="' + PluginID + '" ' + typeid);
	document.write(' width="' + Width + '" height="' + Height + '">');
	document.write('<param name="ReportURL" value="' + ReportURL + '">');
	document.write('<param name="DataURL" value="' + DataURL + '">');
	document.write('<param name="AutoRun" value=' + AutoRun + '>');
	document.write('<param name="SerialNo" value="' + gr_SerialNo + '">');
	document.write('<param name="UserName" value="' + gr_UserName + '">');
	document.write(ExParams);
	document.write('</object>');
}

//以 ReportDesigner 为 ID  Creating a Report Designer plugin (Designer), For details, please see the Help  IGRDesigner
//Width -  Display width plug ,"100%" For the entire display area width ,"500" Representation 500 Screen pixels 
//Height -  Display height plugin ,"100%" For the entire display area height ,"500" Representation 500 Screen pixels 
//LoadReportURL -  Read the report template URL, Since the runtime URL Reads the data and load it into a report template designer plugin 
//SaveReportURL -  Save the report template URL, The results after the design data stored , Thus URL The service WEB The server will report templates persist 
//DataURL -  When you run a report to obtain data URL, When entering the print view and query views in the designer since URL Get Report Data 
//ExParams -  Specify more elaborate plug-in properties , Shaped like : "<param name="%ParamName%" value="%Value%">" Such a parameter string 
function CreateDesignerEx(Width, Height, LoadReportURL, SaveReportURL, DataURL, ExParams)
{
    var typeid;
    if( _gr_isIE )
        typeid = 'classid="clsid:3C19F439-B64D-4dfb-A96A-661FE70EA04D" ' + gr_CodeBase;
    else
        typeid = 'type="application/x-grplugin-designer"';
	document.write('<object id="ReportDesigner" ' + typeid);
	document.write(' width="' + Width + '" height="' + Height + '">');
	document.write('<param name="LoadReportURL" value="' + LoadReportURL + '">');
	document.write('<param name="SaveReportURL" value="' + SaveReportURL + '">');
	document.write('<param name="DataURL" value="' + DataURL + '">');
	document.write('<param name="SerialNo" value="' + gr_SerialNo + '">');
	document.write('<param name="UserName" value="' + gr_UserName + '">');
	document.write(ExParams);
	document.write('</object>');
}

//以 ReportViewer 为 ID  Creating a Report Print Monitor plug (PrintViewer), Parameter Description Reference  CreatePrintViewerEx2
function CreatePrintViewerEx(Width, Height, ReportURL, DataURL, AutoRun, ExParams)
{
    CreatePrintViewerEx2("ReportViewer", Width, Height, ReportURL, DataURL, AutoRun, ExParams)
}

//以 ReportViewer 为 ID  Creating a Report Query Monitor plug-in (DisplayViewer), Parameter Description Reference  CreateDisplayViewerEx2
function CreateDisplayViewerEx(Width, Height, ReportURL, DataURL, AutoRun, ExParams)
{
    CreateDisplayViewerEx2("ReportViewer", Width, Height, ReportURL, DataURL, AutoRun, ExParams)
}

//以 ReportViewer 为 ID  Creating a Report Print Monitor plug (PrintViewer), Plug-size 100% Full location area , Will automatically run after the plug-in creates , Parameter Description Reference  CreatePrintViewerEx2
function CreatePrintViewer(ReportURL, DataURL)
{
    CreatePrintViewerEx("100%", "100%", ReportURL, DataURL, true, "");
}

//以 ReportViewer 为 ID  Creating a Report Query Monitor plug-in (DisplayViewer), Plug-size 100% Full location area , Will automatically run after the plug-in creates , Parameter Description Reference  CreateDisplayViewerEx2
function CreateDisplayViewer(ReportURL, DataURL)
{
    CreateDisplayViewerEx("100%", "100%", ReportURL, DataURL, true, "");
}

//以 ReportDesigner 为 ID  Creating a Report Designer plugin (Designer), Plug-size 100% Full location area , Parameter Description Reference  CreateDesignerEx
function CreateDesigner(LoadReportURL, SaveReportURL, DataURL)
{
    CreateDesignerEx("100%", "100%", LoadReportURL, SaveReportURL, DataURL, "");
}