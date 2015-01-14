var DefuaultUrl = "Base/DataService.aspx";

ccflow = {}
Application = {}

DataFactory = function () {
    this.data = new ccflow.Data(DefuaultUrl);
    this.common = new ccflow.common();
}

jQuery(function ($) {
    Application = new DataFactory();
});
// Public Methods 
ccflow.common = function () {
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
ccflow.Data = function (url) {
    this.url = url;
    // Get all system 
    this.getApps = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getapps" };
        queryData(tUrl, params, callback, scope);
    }
    // Get the left menu 
    this.getLeftMenu = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getleftmenu" };
        queryData(tUrl, params, callback, scope);
    }
    // No. acquisition system based on the system menu 
    this.getAppMenus = function (appName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getsystemmenu",
            appName: appName
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get all menus 
    this.getMenus = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenus"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Press Menu to assign permissions 
    this.getMenusOfMenuForEmp = function (parentNo, isLoadChild, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenusofmenuforemp",
            parentNo: parentNo,
            isLoadChild: isLoadChild
        };
        queryData(tUrl, params, callback, scope);
    }
    // Operation node 
    this.nodeManage = function (nodeNo, dowhat, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "menunodemanage",
            nodeNo: nodeNo,
            dowhat: dowhat
        };
        queryData(tUrl, params, callback, scope);
    }
    // Number Gets the menu according to the user 
    this.getMenuByEmpNo = function (empNo, appName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenubyempno",
            fk_emp: empNo,
            fk_app: appName
        };
        queryData(tUrl, params, callback, scope);
    }
    // Gets the menu according to numbers 
    this.getMenusById = function (id, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenusbyid",
            Id: id
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get all personnel information 
    this.getEmps = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getemps"
        };
        queryData(tUrl, params, callback, scope);
    }
    // The user name or user ID fuzzy search 
    this.getEmpsByNoOrName = function (objSearch, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempsbynoorname",
            objSearch: objSearch
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get all rights group 
    this.getEmpGroups = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempgroups"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Rights Groups fuzzy search 
    this.getEmpGroupsByName = function (objSearch, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempgroupsbyname",
            objSearch: objSearch
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get information department staff 
    this.getDeptEmpTree = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getdeptemptree"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get sub-department staff 
    this.getDeptEmpChildNodes = function (nodeNo, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getdeptempchildnodes",
            nodeNo: nodeNo
        };
        queryData(tUrl, params, callback, scope);
    }
    // Save user with a menu relations 
    this.saveUserOfMenus = function (empNo, menuIds, menuIdsUn, menuIdsUnExt, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "saveuserofmenus",
            empNo: empNo,
            menuIds: menuIds,
            menuIdsUn: menuIdsUn,
            menuIdsUnExt: menuIdsUnExt
        };
        queryPostData(tUrl, params, callback, scope);
    }
    // User menu Permissions 
    this.getEmpOfMenusByEmpNo = function (empNo, parentNo, isLoadChild, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempofmenusbyempno",
            empNo: empNo,
            parentNo: parentNo,
            isLoadChild: isLoadChild
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get permission group menu 
    this.getEmpGroupOfMenusByNo = function (groupNo, parentNo, isLoadChild, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempgroupofmenusbyno",
            groupNo: groupNo,
            parentNo: parentNo,
            isLoadChild: isLoadChild
        };
        queryData(tUrl, params, callback, scope);
    }
    // Save Permissions Group Menu 
    this.saveUserGroupOfMenus = function (groupNo, menuIds, menuIdsUn, menuIdsUnExt, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "saveusergroupofmenus",
            groupNo: groupNo,
            menuIds: menuIds,
            menuIdsUn: menuIdsUn,
            menuIdsUnExt: menuIdsUnExt
        };
        queryPostData(tUrl, params, callback, scope);
    }
    // Empty replication user rights 
    this.clearOfCopyUserPower = function (copyUser, pastUsers, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopyuserpower",
            copyUser: copyUser,
            pastUsers: pastUsers
        };
        queryData(tUrl, params, callback, scope);
    }
    // Overlay copy user permissions 
    this.coverOfCopyUserPower = function (copyUser, pastUsers, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopyuserpower",
            copyUser: copyUser,
            pastUsers: pastUsers
        };
        queryData(tUrl, params, callback, scope);
    }
    // Empty permission to set permissions 
    this.clearOfCopyUserGroupPower = function (copyGroupNo, pastGroupNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopyusergrouppower",
            copyGroupNo: copyGroupNo,
            pastGroupNos: pastGroupNos
        };
        queryData(tUrl, params, callback, scope);
    }
    // Covering Permissions Group Permissions 
    this.coverOfCopyUserGroupPower = function (copyGroupNo, pastGroupNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopyusergrouppower",
            copyGroupNo: copyGroupNo,
            pastGroupNos: pastGroupNos
        };
        queryData(tUrl, params, callback, scope);
    }
    // Open a submenu 
    this.getAppChildMenus = function (appname, no, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getAppChildMenus",
            appname: appname,
            no: no
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get all departments  2013-09-24
    this.getAllDept = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "GetAllDept" };
        queryData(tUrl, params, callback, scope);
    }
    // Press Menu to assign permissions , Get the template data 
    this.getTemplateData = function (menuNo, model, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "gettemplatedata",
            menuNo: menuNo,
            model: model
        };
        queryData(tUrl, params, callback, scope);
    }
    // Press Menu to assign permissions to save 
    this.saveMenuForEmp = function (menuNo, ckNos, model, saveChildNode, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "savemenuforemp",
            menuNo: menuNo,
            ckNos: ckNos,
            model: model,
            saveChildNode: saveChildNode
        };
        queryPostData(tUrl, params, callback, scope);
    }
    // Get System Log Log 
    this.getSystemLoginLogs = function (callback, scope, startdate, enddate) {
        var tUrl = this.url;
        var params = { method: "getsystemloginlogs", startdate: startdate, enddate: enddate };
        queryData(tUrl, params, callback, scope);
    }
    // Get all posts   2013-12-30 Ｈ
    this.getStations = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getstations"
        };
        queryData(tUrl, params, callback, scope);
    }
    // Save jobs menu  2013-12-30 Ｈ
    this.saveStationOfMenus = function (stationNo, menuIds, menuIdsUn, menuIdsUnExt, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "savestationofmenus",
            stationNo: stationNo,
            menuIds: menuIds,
            menuIdsUn: menuIdsUn,
            menuIdsUnExt: menuIdsUnExt
        };
        queryPostData(tUrl, params, callback, scope);
    }
    // Get jobs   Menu  2013-12-30 Ｈ
    this.getStationOfMenusByNo = function (stationNo, parentNo, isLoadChild, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getstationofmenusbyno",
            stationNo: stationNo,
            parentNo: parentNo,
            isLoadChild: isLoadChild
        };
        queryData(tUrl, params, callback, scope);
    }
    // Empty style   Copy jobs   Competence  2013-12-31 Ｈ
    this.clearOfCopyStation = function (copyStationNo, pastStationNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopystation",
            copyStationNo: copyStationNo,
            pastStationNos: pastStationNos
        };
        queryData(tUrl, params, callback, scope);
    }
    // Overlay   Copy jobs   Competence   2013-12-31 Ｈ
    this.coverOfCopyStation = function (copyStationNo, pastStationNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopystation",
            copyStationNo: copyStationNo,
            pastStationNos: pastStationNos
        };
        queryData(tUrl, params, callback, scope);
    }
    // Post   Fuzzy Lookup  2013-12-31 Ｈ
    this.getStationByName = function (stationName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getstationbyname",
            stationName: stationName
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get process categories and processes 
    this.getFlowTree = function (parentNode, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getflowtree",
            nodeNo: parentNode
        };
        queryData(tUrl, params, callback, scope);
    }
    // Get a form library 
    this.getFormTree = function (parentNode, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getformtree",
            nodeNo: parentNode
        };
        queryData(tUrl, params, callback, scope);
    }
    // Save Process / Forms menu 
    this.saveFlowFormMenu = function (model, curMenuNo, pastSortNos, pastNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "saveflowformmenu",
            model: model,
            curMenuNo: curMenuNo,
            pastSortNos: pastSortNos,
            pastNos: pastNos
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
            complete: function () { }, //AJAX When the request is complete Hide loading Prompt 
            error: function (XMLHttpRequest, errorThrown) {
                callback(XMLHttpRequest);
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