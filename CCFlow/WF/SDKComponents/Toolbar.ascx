<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="CCFlow.WF.App.Comm.Toolbar" %>
<%@ Import Namespace="BP.En" %>
<%@ Import Namespace="BP.Web" %>
<%@ Import Namespace="BP.WF" %>
<%@ Import Namespace="BP.WF.Template" %>
<%@ Import Namespace="BP.WF.Data" %>


<%@ Register Src="../UC/Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<%@ Import Namespace="BP.WF" %>
<%
    int fk_node = int.Parse(this.Request.QueryString["FK_Node"]);
    Int64 workID = Int64.Parse(this.Request.QueryString["WorkID"]);
    Int64 fid = Int64.Parse(this.Request.QueryString["FID"]);
    string fk_flow = this.Request.QueryString["FK_Flow"];
    this.Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "SDKData", "" + Glo.CCFlowAppPath + "SDKComponents/Base/SDKData.js");
    string isToolbar = Request.QueryString["IsToobar"];
    bool toolbar = true;
    if (!string.IsNullOrEmpty(isToolbar) && isToolbar == "0")
        toolbar = false;
         
    string paras = this.Request.QueryString["AtPara"];

    bool isCC = false;
    if (paras != null && paras.Contains("IsCC=1"))
        isCC = true;

    paras = this.Request.QueryString["Paras"];
    if (paras != null && paras.Contains("IsCC=1"))
        isCC = true;

    BP.WF.Template.BtnLab btn = new BP.WF.Template.BtnLab(fk_node);
    BP.WF.Node node = new Node(fk_node);

    WFState workState = WFState.Runing;
    string appPath = Glo.CCFlowAppPath; //this.Request.ApplicationPath;
    string msg = null;
    bool isInfo = false;
    try
    {
        WorkFlow workFlow = new WorkFlow(node.FK_Flow, workID);
        workState = workFlow.HisGenerWorkFlow.WFState;
        if (workState != WFState.Complete)
        {
            switch (workFlow.HisGenerWorkFlow.WFState)
            {
                case WFState.AskForReplay: //  Returns information plus sign .
                    string mysql = "SELECT * FROM ND" + int.Parse(node.FK_Flow) + "Track WHERE WorkID=" + workID + " AND " + TrackAttr.ActionType + "=" + (int)ActionType.ForwardAskfor;
                    System.Data.DataTable mydt = BP.DA.DBAccess.RunSQLReturnTable(mysql);
                    foreach (System.Data.DataRow dr in mydt.Rows)
                    {
                        string msgAskFor = dr[TrackAttr.Msg].ToString();
                        string worker = dr[TrackAttr.EmpFrom].ToString();
                        string workerName = dr[TrackAttr.EmpFromT].ToString();
                        string rdt = dr[TrackAttr.RDT].ToString();

                        // Message .
                        this.FlowMsg.AlertMsg_Info(worker + "," + workerName + " Reply message :",
                            BP.DA.DataType.ParseText2Html(msgAskFor) + "<br>" + rdt);
                        isInfo = true;
                    }
                    break;
                case WFState.Askfor: // Plus sign .
                    string sql = "SELECT * FROM ND" + int.Parse(node.FK_Flow) + "Track WHERE WorkID=" + workID + " AND " + TrackAttr.ActionType + "=" + (int)ActionType.AskforHelp;
                    System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        string msgAskFor = dr[TrackAttr.Msg].ToString();
                        string worker = dr[TrackAttr.EmpFrom].ToString();
                        string workerName = dr[TrackAttr.EmpFromT].ToString();
                        string rdt = dr[TrackAttr.RDT].ToString();

                        // Message .
                        this.FlowMsg.AlertMsg_Info(worker + "," + workerName + " Request for endorsement :",
                             BP.DA.DataType.ParseText2Html(msgAskFor) + "<br>" + rdt + " --<a href='./WorkOpt/AskForRe.aspx?FK_Flow=" + fk_flow + "&FK_Node=" + fk_node + "&WorkID=" + workID + "&FID=" + fid + "' > Reply endorsement views </a> --");
                        isInfo = true;

                    }

                    break;
                case WFState.ReturnSta:
                    /*  If the worker node to return the */
                    ReturnWorks rws = new ReturnWorks();
                    rws.Retrieve(ReturnWorkAttr.ReturnToNode, fk_node,
                        ReturnWorkAttr.WorkID, workID,
                        ReturnWorkAttr.RDT);
                    if (rws.Count != 0)
                    {
                        string msgInfo = "";
                        foreach (BP.WF.ReturnWork rw in rws)
                        {
                            msgInfo += "<fieldset width='100%' ><legend>&nbsp;  From node :" + rw.ReturnNodeName + "  Return man :" + rw.ReturnerName + "  " + rw.RDT + "&nbsp;<a href='" + appPath + "DataUser/ReturnLog/" + fk_flow + "/" + rw.MyPK + ".htm' target=_blank> Work Log </a></legend>";
                            msgInfo += rw.NoteHtml;
                            msgInfo += "</fieldset>";
                        }
                        this.FlowMsg.AlertMsg_Info(" Prompt refund process ", msgInfo);
                        isInfo = true;

                        //gwf.WFState = WFState.Runing;ruhe huoqu yige div
                        //gwf.DirectUpdate();
                    }
                    break;
                case WFState.Shift:
                    /*  Judge handed over . */
                    ShiftWorks fws = new ShiftWorks();
                    BP.En.QueryObject qo = new QueryObject(fws);
                    qo.AddWhere(ShiftWorkAttr.WorkID, workID);
                    qo.addAnd();
                    qo.AddWhere(ShiftWorkAttr.FK_Node, fk_node);
                    qo.addOrderBy(ShiftWorkAttr.RDT);
                    qo.DoQuery();
                    if (fws.Count >= 1)
                    {
                        this.FlowMsg.AddFieldSet(" Historical information transfer ");
                        foreach (ShiftWork fw in fws)
                        {
                            msg = "@ Handed people [" + fw.FK_Emp + "," + fw.FK_EmpName + "].@ Recipient :" + fw.ToEmp + "," + fw.ToEmpName + ".<br> Transfer reason @" + fw.NoteHtml;
                            if (fw.FK_Emp == WebUser.No)
                                msg = "<b>" + msg + "</b>";

                            msg = msg.Replace("@", "<br>@");
                            this.FlowMsg.Add(msg + "<hr>");
                        }
                        this.FlowMsg.AddFieldSetEnd();
                    }
                    workFlow.HisGenerWorkFlow.WFState = WFState.Runing;
                    workFlow.HisGenerWorkFlow.DirectUpdate();
                    isInfo = true;

                    break;
                default:
                    break;
            }

            bool isCanDo = workFlow.IsCanDoCurrentWork(BP.Web.WebUser.No);
            if (!isCanDo)
                workState = WFState.Complete;
        }
    }
    catch (Exception)
    {
        try
        {
            Flow fl = new Flow(fk_flow);
            GERpt rpt = fl.HisGERpt;
            rpt.OID = workID;
            rpt.Retrieve();

            if (rpt != null)
            {
                workState = rpt.WFState;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
%>
<script type="text/javascript" language="javascript" src="/DataUser/PrintTools/LodopFuncs.js"></script>
<script type="text/javascript">
    function ShowUrl(obj) {

            var strTimeKey = "";
                    var date = new Date();
                    strTimeKey += date.getFullYear(); //年
                    strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
                    strTimeKey += date.getDate(); //日
                    strTimeKey += date.getHours(); //HH
                    strTimeKey += date.getMinutes(); //MM
                    strTimeKey += date.getSeconds(); //SS
        
        var btnID = obj.id;

         if (btnID == 'Btn_Return') {
             //OpenUrl('ReturnMsg', ' Return ', '/WF/WorkOpt/ReturnWork.aspx?1=2', 500, 350);
             OpenUrlLocation('/WF/WorkOpt/ReturnWork.aspx?1=2');
             return;
         }

         if (btnID == 'Btn_Track') {
             OpenUrl('TrackMsg', ' Process Steps ', '/WF/WorkOpt/OneWork/Track.aspx?1=2', 800, 500);
             return;
         }

         if (btnID == 'Btn_SelectAccepter') {
             OpenUrl('SelectAccepterMsg', ' Recipient ', '/WF/WorkOpt/Accepter.aspx?1=2', 600, 450);
             
             return;
         }

         if (btnID == 'Btn_Askfor') {
            // OpenUrl('AskforMsg', ' Plus sign ', '/WF/WorkOpt/AskFor.aspx?1=2', 600, 400);
             OpenUrlLocation( '/WF/WorkOpt/AskFor.aspx?1=2');
             return;
         }

         if (btnID == 'Btn_Shift') {
             //OpenUrl('ShiftMsg', ' Transfer ', '/WF/WorkOpt/Forward.aspx?1=2', 600, 450);
             OpenUrlLocation('/WF/WorkOpt/Forward.aspx?1=2');
             return;
         }

         if (btnID == 'Btn_CC') {
             OpenUrl('CCMsg', ' Cc ', '/WF/WorkOpt/CC.aspx?1=2', 700, 450);
             return;
         }

         if (btnID == 'Btn_Delete') {
         if (confirm(" Really need to be removed ?"))
              OpenUrlLocation('/WF/DeleteWorkFlow.aspx?1=2');
             return;
         }

     if (btnID == 'Btn_CheckNote') {
            // OpenUrl('CCMsg', ' Check ', '/WF/WorkOpt/CCCheckNote.aspx?1=2', 700, 450);
         OpenUrlLocation('/WF/WorkOpt/CCCheckNote.aspx?1=2');
             return;
         }

         if (btnID == 'Btn_Office') {
             WinOpen( '/WF/WorkOpt/WebOffice.aspx?1=2', ' Text documents ', 700, 450);
             return;
         }

         if (btnID == 'Btn_Read') {
            Application.data.ReadCC("<%=node.NodeID %>", "<%=workID %>", ReadCCResult, this);
         }

         if (btnID == 'Btn_Print') {
             printFrom();
             return;
         }

//        if (btnID == 'Btn_Delete') {
//             if (confirm(" Really need to be removed ?"))
//                        DelCase();
//            return;
//        }
    }
    function DeleteResult(json) {
        
    }
    function ReadCCResult(json) {
        if (json != "true") {
            alert(' Have read failure !');
        } 
    }

    var LODOP;
     function printFrom() {
        LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        LODOP.PRINT_INIT(" Print Form ");
     //    LODOP.ADD_PRINT_URL(30, 20, 746, "100%", location.href);
        LODOP.ADD_PRINT_HTM(0, 0, "100%", "100%", document.getElementById("divCCForm").innerHTML);
        // LODOP.ADD_PRINT_URL(0, 0, "100%", "100%", url);
        LODOP.SET_PRINT_STYLEA(0, "HOrient", 3);
        LODOP.SET_PRINT_STYLEA(0, "VOrient", 3);
        //		LODOP.SET_SHOW_MODE("MESSAGE_GETING_URL",""); // The statement hide the progress bar or modify the message 
        //		LODOP.SET_SHOW_MODE("MESSAGE_PARSING_URL","");// The statement hide the progress bar or modify the message 
        //  LODOP.PREVIEW();

        LODOP.PREVIEW();


    }

    function ShowFlowMessage() {
          $('#flowMessage').window(
        {
            closeable: false,
            title: " Information log ",
            modal: true,
            width: 800,
            height: 400,
            buttons: [{ text: ' Shut down ', handler: function () {
                $('#flowMessage').dialog("close");
            }
            }]
        }
        );
    }
      function WinOpen(url, winName,width,height) {

       //  Generation parameters .
        _GetParas();

        // 把IsEUI Deal with it , Let the other side of the functional interface to receive this parameter personalize .
        url = url + _paras + '&IsEUI=1';
        var newWindow = window.open(url, winName, 'width='+width+',height='+height+',top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
        newWindow.focus();
        return;
    }

      function DelCase() {
            var strTimeKey = "";
            var date = new Date();
            strTimeKey += date.getFullYear(); //年
            strTimeKey += date.getMonth() + 1; //月  May be less than the actual month 1
            strTimeKey += date.getDate(); //日
            strTimeKey += date.getHours(); //HH
            strTimeKey += date.getMinutes(); //MM
            strTimeKey += date.getSeconds(); //SS

            Application.data.delcase(<%=fk_flow %>, <%=fk_node %>, <%=workID %>, "", function (js) {
                if (js) {
                    var str = js;
                    if (str == " Deleted successfully ") {
                        $.messager.alert(' Prompt ', ' Deleted successfully !');
                    }
                    else {
                        $.messager.alert(' Prompt ', str);
                    }
                }
            }, this);
        }
        function OpenUrlLocation(url) {
              _GetParas();
              url = url + _paras + '&IsEUI=1';
                window.name = "dialogPage";
            window.open(url, "dialogPage");
//            window.location.href = url;
        }
    function OpenUrl(divID, title, url, w, h) {

    //  Generation parameters .
        _GetParas();
              url = url + _paras + '&IsEUI=1';
        <% if (BP.Sys.SystemConfig.AppSettings["SDKWinOpenType"] == "1")
           { %>
        // 把IsEUI Deal with it , Let the other side of the functional interface to receive this parameter personalize .
        try {
         window.parent.OpenJboxIfream(title,url,w,h);
        } catch (e) {
            OpenWindow(url,title,w,h);
        }
       
<% }
           else
           { %>
        OpenWindow(url,title,w,h);
<% } %>
    }

    $(function() {
        var html = $('#flowMessage').text();
        if (html != "" &&  html != null && html.length>6) {
            ShowFlowMessage();
        }
    });

    function OpenWindow(url,title,w,h) {
        $('#windowIfrem').window(
        {
              content: ' <iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>',
            closeable: false,
            title: title,
            modal: true,
            width: w,
            height: h,
            buttons: [{ text: ' Shut down ', handler: function () {
                $('#windowIfrem').dialog("close");
            }
            }]
        }
        );
    }

    var _paras = "";
    function _GetParas() {
        _paras = "";
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
                if (_paras.indexOf(arg[0]) == -1) {
                    _paras += "&" + arg[0] + "=" + arg[1];
                }
            }
        }
    }

    function closeWin() {
        if (window.dialogArguments && window.dialogArguments.window) {
            window.dialogArguments.window.location = window.dialogArguments.window.location;
        }
        if (window.opener) {
            if (window.opener.name && window.opener.name == "main") {
                window.opener.location.href = window.opener.location.href;
                window.opener.top.leftFrame.location.href = window.opener.top.leftFrame.location.href;
            }
        }
        window.close();
    }
</script>
<div id="ReturnMsg">
</div>
<div id="SelectAccepterMsg">
</div>
<div id="AskforMsg">
</div>
<div id="ShiftMsg">
</div>
<div id="CCMsg">
</div>
<div id="TrackMsg">
</div>
<div id="msgPanel">
</div>
<div id="windowIfrem">
</div>
<div id="flowMessage">
    <uc1:Pub ID="FlowMsg" runat="server" />
</div>
<% if (toolbar)
   { %>
<% if (isCC == false && workState != WFState.Complete)
   { %>
<% if (workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<!-- Send -->
<input type="button" onclick="Send()" id="Btn_Send" value='<%= btn.SendLab %>' />
<% } %>
<!--  Save -->
<% if (btn.SaveEnable == true && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="Save()" id="Btn_Save" value='<%= btn.SaveLab %>' />
<% } %>
<!--  Return -->
<% if (btn.ReturnEnable == true && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Return" name="Btn_Return" value='<%= btn.ReturnLab %>' />
<% } %>
<!--  Recipient -->
<% if (btn.SelectAccepterEnable != 0 && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_SelectAccepter" value='<%= btn.SelectAccepterLab %>' />
<% } %>
<!--  Transfer -->
<% if (btn.ShiftEnable == true && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Shift" value='<%= btn.ShiftLab %>' />
<% } %>
<!--  Delete -->
<% if (btn.DeleteEnable != 0 && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Delete" value='<%= btn.DeleteLab %>' />
<% } %>
<!--  Plus sign -->
<% if (btn.AskforEnable == true && workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Askfor" value='<%= btn.AskforLab %>' />
<% } %>
<% }
   else
   {
       /*  If CC . */ %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Read" value=" Have read " />
<%
    BP.Sys.FrmWorkCheck fwc = new BP.Sys.FrmWorkCheck(fk_node);

    if (fwc.HisFrmWorkCheckSta != BP.Sys.FrmWorkCheckSta.Enable)
    {
        /* If you do not enable , You can review the window on the show . */
        string url = "";
%>
<input type="button" value=' Fill audit opinion ' id="Btn_CheckNote" onclick="ShowUrl(this)" />
<%
    }
   } %>
<!--  Cc -->
<% if (btn.CCRole == BP.WF.CCRole.HandAndAuto || btn.CCRole == BP.WF.CCRole.HandCC)
   { %>
<% if (workState != WFState.Complete && workState != WFState.Fix && workState != WFState.HungUp)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_CC" value='<%= btn.CCLab %>' />
<% } %>
<% } %>
<!--  Locus -->
<% if (btn.TrackEnable)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Track" value='<%= btn.TrackLab %>' />
<% } %>
<% } %>
<!--  Print -->
<% if (btn.PrintDocEnable)
   { %>
<% if (node.HisPrintDocEnable == PrintDocEnable.PrintHtml)
   { %>
<object id="LODOP_OB" classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0"
    height="0">
    <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0" pluginspage="/DataUser/PrintTools/install_lodop32.exe"></embed>
</object>
<input type="button" onclick="ShowUrl(this)" id="Btn_Print" value='<%= btn.PrintDocLab %>' />
<% } %>
<% } %>
<!--  Document -->
<% if (btn.WebOfficeEnable == 1)
   { %>
<input type="button" onclick="ShowUrl(this)" id="Btn_Office" value='<%= btn.WebOfficeLab %>' />
<% } %>
<% if (isInfo)
   { %>
<input type="button" onclick="ShowFlowMessage()" id="Btn_flowInfo" value=' Information log ' />
<% } %>
