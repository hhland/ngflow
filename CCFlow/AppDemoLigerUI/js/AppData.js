var DefuaultUrl = "Base/DataService.aspx";

ccflow = {}
Application = {}

DataFactory = function () {
    this.data = new ccflow.Data(DefuaultUrl);
}
// Profiles 
ccflow.config = {
    IsWinOpenStartWork: 1, // Whether to start work opens a new window  0= In this window open ,1= Open in new window , 2= Open a window procedure 
    IsWinOpenEmpWorks: "TRUE"// Whether to open the work to be done to open a new window 
};

jQuery(function ($) {
    Application = new DataFactory();

    // Initial 
    Application.data.init(function (json) {
        if (json != "") {
            var configData = eval('(' + json + ')');
            ccflow.config.IsWinOpenStartWork = configData.config[0].IsWinOpenStartWork;
            ccflow.config.IsWinOpenEmpWorks = configData.config[0].IsWinOpenEmpWorks;
        }
    }, this);
});

ccflow.Data = function (url) {
    this.url = url;
    // Get Process List 
    this.getStartFlow = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "startflow" };
        queryData(tUrl, params, callback, scope);
    }

    // Get Process List 
    this.getStartFlowTree = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "startflowTree" };
        queryData(tUrl, params, callback, scope);
    }
    // Get To Do List 
    this.getEmpWorks = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getempworks" };
        queryData(tUrl, params, callback, scope);
    }
    // Get CC list 
    this.getCCFlowList = function (ccSta, callback, scope) {
        var tUrl = this.url;
        var params = { method: "getccflowlist", ccSta: ccSta };
        queryData(tUrl, params, callback, scope);
    }
    // Get pending process 
    this.getHungUpList = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "gethunguplist" };
        queryData(tUrl, params, callback, scope);
    }
    // Get the list of passers 
    this.getRunning = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "Running" };
        queryData(tUrl, params, callback, scope);
    }
    // Send revocation 
    this.unSend = function (fkFlow, workId, callback, scope) {
        var tUrl = this.url;
        var params = { method: "unsend", FK_Flow: fkFlow, WorkID: workId };
        queryData(tUrl, params, callback, scope);
    }
    // Workflow inquiry 
    this.flowSearch = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "flowsearch" };
        queryData(tUrl, params, callback, scope);
    }
    // Get directory information 
    this.getEmps = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getemps" };
        queryData(tUrl, params, callback, scope);
    }
    // Retrieve approval 
    this.getTask = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "gettask" };
        queryData(tUrl, params, callback, scope);
    }
    // Keyword Search 
    this.keySearch = function (checkBox, content, queryType, callback, scope) {
        var tUrl = this.url;
        var params = { method: "keySearch", checkBox: checkBox, content: encodeURI(content), queryType: queryType };
        queryData(tUrl, params, callback, scope);
    }
    // Get the configuration parameters 
    this.init = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getconfigparm" };
        queryData(tUrl, params, callback, scope);
    }
    // Get Upcoming , Cc , Suspend number 
    this.getEmpWorkCounts = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getempworkcounts" };
        queryData(tUrl, params, callback, scope);
    }
    // Get Historical launched 
    this.getHistoryStartFlow = function (flowNo, callback, scope) {
        var tUrl = this.url;
        var params = { method: "historystartflow", FK_FLOW: flowNo };
        queryData(tUrl, params, callback, scope);
    }
    // Pop up   System Messages   Window   2013.05.23 H
    this.popAlert = function (type, callback, scope) {
        var tUrl = this.url;
        var params = { method: "popAlert", type: type };
        queryData(tUrl, params, callback, scope);
    }

    // Modification   Data   Status    2013.05.23 H
    this.upMsgSta = function (my_PK, callback, scope) {
        var tUrl = this.url;
        var params = { method: "upMsgSta", myPK: my_PK };
        queryData(tUrl, params, callback, scope);
    }
    // Get   Detailed data    2013.05.23 H
    this.getDetailSms = function (my_PK, callback, scope) {
        var tUrl = this.url;
        var params = { method: "getDetailSms", myPK: my_PK };
        queryData(tUrl, params, callback, scope);
    }
    // Load Menu  2013.07.23 H 
    this.getMenu = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getmenu" };
        queryData(tUrl, params, callback, scope);
    }
    // Get all the historical processes 
    this.getStoryHistory = function (fk_flow, callback, scope) {
        var tUrl = this.url;
        var params = { method: "getstoryHistory", FK_Flow: fk_flow };
        queryData(tUrl, params, callback, scope);
    }
    // New monthly plan 
    this.createMonthPlan = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "createmonthplan"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Month plan summary 
    this.monthPlanCollect = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "monthplancollect"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Business Process Operation 
    this.workFlowManage = function (doWhat, flowIdAndWorkId, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "workflowmanage",
            doWhat: doWhat,
            flowIdAndWorkId: flowIdAndWorkId
        };
        queryData(tUrl, params, callback, scope);
    }
    // Loading process tree  H 
    this.treedata = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "treeData"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Create an empty Process 
    this.createEmptyCase = function (flowId, title, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "createemptycase",
            flowId: flowId,
            title: encodeURI(title)
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
            async: true,
            cache: false,
            complete: function () { $("#load").hide(); }, //AJAX When the request is complete Hide loading Prompt 
            error: function (XMLHttpRequest, errorThrown) {
                callback(XMLHttpRequest);
            },
            success: function (msg) {//msg For the returned data , Here to do data binding 
                var data = msg;
                callback(data, scope);
            }
        });
    }
}