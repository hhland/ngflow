var DefuaultUrl = "/WF/SDKComponents/Base/SDKBase.aspx";

ccflowSDK = {}
Application = {}

DataFactory = function () {
    this.data = new ccflowSDK.Data(DefuaultUrl);
    this.common = new ccflowSDK.common();
}

jQuery(function ($) {
    Application = new DataFactory();
});

// Public Methods 
ccflowSDK.common = function () {
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
// Data Access 
ccflowSDK.Data = function (url) {
    this.url = url;
    // Get Toolbar 
    this.getAppToolBar = function (nodeId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getapptoolbar",
            nodeId: nodeId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get the left tree form 
    this.getFlowFormTree = function (flowId, nodeId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getflowformtree",
            flowId: flowId,
            nodeId: nodeId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Recipient selection 
    this.checkAccepter = function (FK_Node, WorkID, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "checkaccepter",
            FK_Node: FK_Node,
            WorkID: WorkID
        };
        queryData(tUrl, params, callback, scope);
    }
    // Set CC have read 
    this.ReadCC = function (FK_Node, WorkID, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "readCC",
            FK_Node: FK_Node,
            WorkID: WorkID
        };
        queryData(tUrl, params, callback, scope);
    }
    // Performing transmission 
    this.sendCase = function (FK_Flow, FK_Node, WorkID, DoFunc, CFlowNo, WorkIDs, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "sendcase",
            FK_Flow: FK_Flow,
            FK_Node: FK_Node,
            WorkID: WorkID,
            DoFunc: DoFunc,
            CFlowNo: CFlowNo,
            WorkIDs: WorkIDs
        };
        queryData(tUrl, params, callback, scope);
    }
    // Transmission is performed to the specified node 
    this.sendCaseToNode = function (FK_Flow, FK_Node, WorkID, DoFunc, CFlowNo, WorkIDs, ToNode, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "sendcasetonode",
            FK_Flow: FK_Flow,
            FK_Node: FK_Node,
            WorkID: WorkID,
            DoFunc: DoFunc,
            CFlowNo: CFlowNo,
            WorkIDs: WorkIDs,
            ToNode: ToNode
        };
        queryData(tUrl, params, callback, scope);
    }
    // Send revocation 
    this.unSendCase = function (flowId, workId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "unsendcase",
            FK_Flow: flowId,
            WorkID: workId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Delete Process 
    this.delcase = function (flowId, nodeId, workId, fId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "delcase",
            flowId: flowId,
            nodeId: nodeId,
            workId: workId,
            fId: fId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Signature Process 
    this.signcase = function (flowId, nodeId, workId, fId, yj, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "signcase",
            flowId: flowId,
            nodeId: nodeId,
            workId: workId,
            fId: fId,
            yj: yj
        };
        queryData(tUrl, params, callback, scope);
    }
    // End Process 
    this.endCase = function (flowId, workId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "endcase",
            flowId: flowId,
            workId: workId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Public Methods 
    function queryData(url, param, callback, scope, method, showErrMsg) {
        if (!method) method = 'GET';
        $.ajax({
            type: method, // Use GET或POST Method of accessing the background 
            dataType: "text", // Return json Data format 
            contentType: "application/json; charset=utf-8",
            url: url, // Backstage address to be accessed 
            data: param, // Data to be transmitted 
            async: false,
            cache: false,
            complete: function () { }, //AJAX When the request is complete Hide loading Prompt 
            error: function (XMLHttpRequest, errorThrown) {
                callback(XMLHttpRequest);
                alert("error");
            },
            success: function (msg) {//msg For the returned data , Here to do data binding 
                var data = msg;
                callback(data, scope);
            }
        });
    }

    // Public Methods 
    function queryPostData(url, param, callback, scope) {
        $.post(url, param, callback);
    }
}