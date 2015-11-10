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
//公共方法
ccflow.common = function () {
    //sArgName表示要获取哪个参数的值
    this.getArgsFromHref = function (sArgName) {
        var sHref = window.location.href;
        var args = sHref.split("?");
        var retval = "";
        if (args[0] == sHref) /*参数为空*/
        {
            return retval; /*无需做任何处理*/
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
//数据访问
ccflow.Data = function (url) {
    this.url = url;
    //获取所有系统
    this.getApps = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getapps" };
        queryData(tUrl, params, callback, scope);
    }
    //获取左侧菜单
    this.getLeftMenu = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "getleftmenu" };
        queryData(tUrl, params, callback, scope);
    }
    //根据系统编号获取系统菜单
    this.getAppMenus = function (appName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getsystemmenu",
            appName: appName
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取所有菜单
    this.getMenus = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenus"
        };
        queryData(tUrl, params, callback, scope);
    }
    //按菜单分配权限
    this.getMenusOfMenuForEmp = function (parentNo, isLoadChild, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenusofmenuforemp",
            parentNo: parentNo,
            isLoadChild: isLoadChild
        };
        queryData(tUrl, params, callback, scope);
    }
    //操作节点
    this.nodeManage = function (nodeNo, dowhat, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "menunodemanage",
            nodeNo: nodeNo,
            dowhat: dowhat
        };
        queryData(tUrl, params, callback, scope);
    }
    //根据用户编号获取菜单
    this.getMenuByEmpNo = function (empNo, appName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenubyempno",
            fk_emp: empNo,
            fk_app: appName
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取菜单根据编号
    this.getMenusById = function (id, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getmenusbyid",
            Id: id
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取所有人员信息
    this.getEmps = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getemps"
        };
        queryData(tUrl, params, callback, scope);
    }
    //根据用户名或编号模糊查找用户
    this.getEmpsByNoOrName = function (objSearch, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempsbynoorname",
            objSearch: objSearch
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取所有权限组
    this.getEmpGroups = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempgroups"
        };
        queryData(tUrl, params, callback, scope);
    }
    //权限组模糊查找
    this.getEmpGroupsByName = function (objSearch, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getempgroupsbyname",
            objSearch: objSearch
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取部门人员信息
    this.getDeptEmpTree = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getdeptemptree"
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取子部门人员
    this.getDeptEmpChildNodes = function (nodeNo, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getdeptempchildnodes",
            nodeNo: nodeNo
        };
        queryData(tUrl, params, callback, scope);
    }
    //保存用户与菜单关系
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
    //用户菜单权限
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
    //获取权限组菜单
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
    //保存权限组菜单
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
    //清空式复制用户权限
    this.clearOfCopyUserPower = function (copyUser, pastUsers, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopyuserpower",
            copyUser: copyUser,
            pastUsers: pastUsers
        };
        queryData(tUrl, params, callback, scope);
    }
    //覆盖式复制用户权限
    this.coverOfCopyUserPower = function (copyUser, pastUsers, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopyuserpower",
            copyUser: copyUser,
            pastUsers: pastUsers
        };
        queryData(tUrl, params, callback, scope);
    }
    //清空式权限组权限
    this.clearOfCopyUserGroupPower = function (copyGroupNo, pastGroupNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopyusergrouppower",
            copyGroupNo: copyGroupNo,
            pastGroupNos: pastGroupNos
        };
        queryData(tUrl, params, callback, scope);
    }
    //覆盖权限组权限
    this.coverOfCopyUserGroupPower = function (copyGroupNo, pastGroupNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopyusergrouppower",
            copyGroupNo: copyGroupNo,
            pastGroupNos: pastGroupNos
        };
        queryData(tUrl, params, callback, scope);
    }
    //打开子菜单
    this.getAppChildMenus = function (appname, no, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getAppChildMenus",
            appname: appname,
            no: no
        };
        queryData(tUrl, params, callback, scope);
    }
    //获取所有部门 2013-09-24
    this.getAllDept = function (callback, scope) {
        var tUrl = this.url;
        var params = { method: "GetAllDept" };
        queryData(tUrl, params, callback, scope);
    }
    //按菜单分配权限，获取模版数据
    this.getTemplateData = function (menuNo, model, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "gettemplatedata",
            menuNo: menuNo,
            model: model
        };
        queryData(tUrl, params, callback, scope);
    }
    //保存按菜单分配权限
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
    //获取系统登录日志
    this.getSystemLoginLogs = function (callback, scope, startdate, enddate) {
        var tUrl = this.url;
        var params = { method: "getsystemloginlogs", startdate: startdate, enddate: enddate };
        queryData(tUrl, params, callback, scope);
    }
    //获取所有岗位  2013-12-30 Ｈ
    this.getStations = function (callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getstations"
        };
        queryData(tUrl, params, callback, scope);
    }
    //保存岗位菜单 2013-12-30 Ｈ
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
    //获取岗位 菜单 2013-12-30 Ｈ
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
    //清空式 复制岗位 权限 2013-12-31 Ｈ
    this.clearOfCopyStation = function (copyStationNo, pastStationNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "clearofcopystation",
            copyStationNo: copyStationNo,
            pastStationNos: pastStationNos
        };
        queryData(tUrl, params, callback, scope);
    }
    //覆盖式 复制岗位 权限  2013-12-31 Ｈ
    this.coverOfCopyStation = function (copyStationNo, pastStationNos, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "coverofcopystation",
            copyStationNo: copyStationNo,
            pastStationNos: pastStationNos
        };
        queryData(tUrl, params, callback, scope);
    }
    //岗位 模糊查找 2013-12-31 Ｈ
    this.getStationByName = function (stationName, callback, scope) {
        var tUrl = this.url;
        var params = {
            method: "getstationbyname",
            stationName: stationName
        };
        queryData(tUrl, params, callback, scope);
    }
    //公共方法
    function queryData(url, param, callback, scope, method, showErrMsg) {
        if (!method) method = 'GET';
        $.ajax({
            type: method, //使用GET或POST方法访问后台
            dataType: "text", //返回json格式的数据
            contentType: "application/json; charset=utf-8",
            url: url, //要访问的后台地址
            data: param, //要发送的数据
            async: true,
            cache: false,
            complete: function () { }, //AJAX请求完成时隐藏loading提示
            error: function (XMLHttpRequest, errorThrown) {
                callback(XMLHttpRequest);
            },
            success: function (msg) {//msg为返回的数据，在这里做数据绑定
                var data = msg;
                callback(data, scope);
            }
        });
    }

    //公共方法
    function queryPostData(url, param, callback, scope) {
        $.post(url, param, callback);
    }
}