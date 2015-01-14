// Integer value from the three primary colors synthesis 
function ColorFromRGB(red, green, blue)
{
    return red + green*256 + blue*256*256;
}

// Get the red color value , Its argument is an integer representation RGB值
function ColorGetR(intColor)
{
    return intColor & 255;
}

// Get the color of green value , Its argument is an integer representation RGB值
function ColorGetG(intColor)
{
    //return intColor & 255*256;
    return (intColor>>8) & 255;
}

// Get the value of the blue color , Its argument is an integer representation RGB值
function ColorGetB(intColor)
{
    //return intColor & 255*256*256;
    return (intColor>>16) & 255;
}

// Create  XMLHttpRequest  Object 
function CreateXMLHttpRequest() 
{
    var xmlhttp;
    if (window.XMLHttpRequest)
        xmlhttp = new XMLHttpRequest(); // code for IE7+, Firefox, Chrome, Opera, Safari
    else
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); // code for IE6, IE5
    return xmlhttp;
}

// Report data asynchronously request , Data will be loaded into the report in response to events , Then perform subsequent task function 
function AjaxReportRun(Report, DataUrl, RunFun) 
{
    var xmlhttp = CreateXMLHttpRequest();
    xmlhttp.onreadystatechange=function()
    {
        if (xmlhttp.readyState==4 && xmlhttp.status==200)
        {
            Report.LoadDataFromAjaxRequest(xmlhttp.responseText, xmlhttp.getAllResponseHeaders()); // Load report data 
            RunFun(); // After the data is loaded tasks to be performed 
        }
    }
    xmlhttp.open("POST", DataUrl, true);
    xmlhttp.send();
}

// Report data asynchronously request , Data will be loaded into the report in response to events , Report Viewer and start running 
function AjaxReportViewerStart(ReportViewer, DataUrl) 
{
    ReportViewer.Stop(); // First stop running the report 

    var xmlhttp = CreateXMLHttpRequest();
    xmlhttp.onreadystatechange=function()
    {
        if (xmlhttp.readyState==4 && xmlhttp.status==200)
        {
            ReportViewer.Report.LoadDataFromAjaxRequest(xmlhttp.responseText, xmlhttp.getAllResponseHeaders()); // Load report data 
            ReportViewer.Start(); // Start report runs 
        }
    }
    xmlhttp.open("POST", DataUrl, true);
    xmlhttp.send();
}

// By synchronous request report data , Immediately after the method invocation request method call report data loading data 
//用 Ajax  Sub-report data must be loaded  HTTP  Synchronous Data Request , Which uses this function 
function AjaxSyncLoadReportData(Report, DataUrl) 
{
    var xmlhttp = CreateXMLHttpRequest();
    xmlhttp.open("POST", DataUrl, false);
    xmlhttp.send();
    Report.LoadDataFromAjaxRequest(xmlhttp.responseText, xmlhttp.getAllResponseHeaders()); // Load report data 
}

