
//  Go to the message screen prompts , This window prompts longer .
function ToMsg(msg) {
    alert(msg);
    window.location.href = '/WF/MyFlowInfo.aspx?Msg=' + data;
}

var paras = "";
// Generate url Para.
function GetParas() {
    paras = "";
    // Get other parameters 
    var sHref = window.location.href;
    var args = sHref.split("?");
    var retval = "";
    if (args[0] != sHref) /* Parameter is not empty */
    {
        var str = args[1];
        args = str.split("&");
        for (var i = 0; i < args.length; i++) {
            str = args[i];
            var arg = str.split("=");
            if (arg.length <= 1)
                continue;
            // It does not contain added 
            if (paras.indexOf(arg[0]) == -1) {
                paras += "&" + arg[0] + "=" + arg[1];
            }
        }
    }
}