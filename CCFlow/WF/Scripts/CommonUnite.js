Application = {}

DataFactory = function () {
    this.common = new commonUnite;
}

jQuery(function ($) {
    Application = new DataFactory();
});

// Public Methods 
commonUnite = function () {
    //sArgName Which represents the value of the parameter to get 
    this.getArgsFromHref = function (sArgName) {
        var sHref = window.location.href;
        var args = sHref.split("?");
        var retval = "";
        if (args[0] == sHref) /* Parameter is empty */
        {
            return retval; /* Do not need any treatment */
        }
        var str = args[1];
        args = str.split("&");
        for (var i = 0; i < args.length; i++) {
            str = args[i];
            var arg = str.split("=");
            if (arg.length <= 1) continue;
            if (arg[0] == sArgName) retval = arg[1];
        }
        return retval;
    }
}