using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class AllotTask : BP.Web.UC.UCBase3
    {
        #region  Property .
        /// <summary>
        /// WorkID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        /// <summary>
        /// FID
        /// </summary>
        public int FID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// IsFHL
        /// </summary>
        public bool IsFHL
        {
            get
            {
                if (this.WorkID == this.FID)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// NodeID
        /// </summary>
        public int NodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["NodeID"]);
                }
                catch
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Assignments ";
            if (this.IsPostBack == false)
            {
                string fk_emp = this.Request.QueryString["FK_Emp"];
                string sid = this.Request.QueryString["SID"];

                if (fk_emp != null )
                {
                    if (BP.Web.WebUser.CheckSID(fk_emp, sid) == false)
                        return;

                    Emp emp = new Emp(fk_emp);
                    BP.Web.WebUser.SignInOfGenerLang(emp, null);
                }
            }

            //  Staff privileges of the current use of .
            this.Clear();

            GenerWorkerLists wls = new GenerWorkerLists(this.WorkID, this.NodeID, true);

            if (WebUser.IsWap)
                this.AddFieldSet("<a href='./WAP/Home.aspx' ><img src='/WF/Img/Home.gif' border=0/> Home </a> -  Assignments ");
            else
                this.AddFieldSet(" Assignments ");

            string ids = "";
            this.AddUL();
            foreach (GenerWorkerList wl in wls)
            {
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + wl.FK_Emp;
                ids += "," + cb.ID;

                cb.Text = BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);
                cb.Checked = wl.IsEnable;
                this.Add("<li>");
                this.Add(cb);
                this.Add("</li>");
            }
            this.AddULEnd();

            this.AddHR();
            Btn btn = new Btn();
            btn.ID = "Btn_Do";
            btn.Text = "   Determine   ";
            btn.Click += new EventHandler(BPToolBar1_ButtonClick);
            this.Add(btn);

            CheckBox cbx = new CheckBox();
            cbx.ID = "seleall";
            cbx.Text = " Select all ";
            cbx.Checked = true;
            cbx.Attributes["onclick"] = "SetSelected(this,'" + ids + "')";
            this.Add(cbx);
            //this.Add("<input type=button value=' Cancel ' onclick='window.close();'  />");
            this.Add("<br><br> Help : The system will remember this work is specified , The next time you work it will automatically delivered to you at this time to specify the person sending time .");
            this.AddFieldSetEnd();

        }
        private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            this.Confirm();
            return;
        }
        /// <summary>
        ///  OK window 
        /// </summary>
        public void Confirm()
        {
            GenerWorkerLists wls = new GenerWorkerLists(this.WorkID, this.NodeID, true);
            try
            {
                #region  Check whether the person selected for  0 .
                bool isHave0 = true;
                foreach (GenerWorkerList wl in wls)
                {
                    CheckBox cb = this.GetCBByID("CB_" + wl.FK_Emp);
                    if (cb.Checked)
                    {
                        isHave0 = false;
                        break;
                    }
                }

                if (isHave0 == true)
                {
                    this.Alert(" Your current job is not assigned to anyone , This work will not be performed by others !");
                    return;
                }
                #endregion  Check whether the person selected for  0 .

                #region  Perform assigned work  -  Select the status update .
                foreach (GenerWorkerList wl in wls)
                {
                    CheckBox cb = this.GetCBByID("CB_" + wl.FK_Emp);
                    if (wl.IsEnable != cb.Checked)
                    {
                        wl.IsEnable = cb.Checked;
                        wl.Update();
                    }
                }
                #endregion  Perform assigned work  -  Select the status update .

                //  Save Memory , So that you can remember next time to send him to live .
                RememberMe rm = new RememberMe();
                rm.FK_Emp = BP.Web.WebUser.No;
                rm.FK_Node = NodeID;
                rm.Objs = "@";
                rm.ObjsExt = "";

                foreach (GenerWorkerList wl in wls)
                {
                    if (wl.IsEnable == false)
                        continue;

                    rm.Objs += wl.FK_Emp + "@";
                    rm.ObjsExt += wl.FK_EmpText + "&nbsp;&nbsp;";
                }

                rm.Emps = "@";
                rm.EmpsExt = "";

                foreach (GenerWorkerList wl in wls)
                {
                    rm.Emps += wl.FK_Emp + "@";

                    string empInfo = BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText);
                    if (rm.Objs.IndexOf(wl.FK_Emp) != -1)
                        rm.EmpsExt += "<font color=green>" + BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText) + "</font>&nbsp;&nbsp;";
                    else
                        rm.EmpsExt += "<strike><font color=red>(" + BP.WF.Glo.DealUserInfoShowModel(wl.FK_Emp, wl.FK_EmpText) + "</font></strike>&nbsp;&nbsp;";
                }
                rm.Save();

                if (WebUser.IsWap)
                {
                    this.Clear();
                    this.AddFieldSet(" Message ");
                    this.Add("<br>&nbsp;&nbsp; Assignment Success , Special Note : When the flow of the transmission system will follow the path you have set for intelligent delivery next time .");
                    this.AddUL();
                    this.AddLi("<a href='./WAP/Home.aspx' ><img src='/WF/Img/Home.gif' border=0/> Home </a>");
                    this.AddLi("<a href='./WAP/Start.aspx' ><img src='/WF/Img/Start.gif' border=0/> Launch </a>");
                    this.AddLi("<a href='./WAP/Runing.aspx' ><img src='/WF/Img/Runing.gif' border=0/> Upcoming </a>");
                    this.AddULEnd();
                    this.AddFieldSetEnd();
                }
                else
                {
                    this.ToMsg(" Assignment Success !!!", "Info");
                    //this.WinCloseWithMsg(" Assignment Success .");
                }
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);
                Log.DebugWriteWarning(ex.Message);
                this.Alert(" Task allocation error :" + ex.Message);
            }
        }
        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            this.Response.Redirect("../MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.NodeID + "&WorkID=" + this.WorkID, false);

        }

        public void DealWithFHLFlow(ArrayList al, GenerWorkerLists wlSeles)
        {
            GenerWorkerLists wls = new GenerWorkerLists();
            wls.Retrieve(GenerWorkerListAttr.FID, this.FID);

            DBAccess.RunSQL("UPDATE  WF_GenerWorkerlist SET IsEnable=0  WHERE FID=" + this.FID);

            string emps = "";
            string myemp = "";
            foreach (Object obj in al)
            {
                emps += obj.ToString() + ",";
                myemp = obj.ToString();
                DBAccess.RunSQL("UPDATE  WF_GenerWorkerlist SET IsEnable=1  WHERE FID=" + this.FID + " AND FK_Emp='" + obj + "'");
            }

            //BP.WF.Node nd = new BP.WF.Node(NodeID);
            //Work wk = nd.HisWork;
            //wk.OID = this.WorkID;
            //wk.Retrieve();
            //wk.Emps = emps;
            //wk.Update();
        }

        public void DealWithPanelFlow(ArrayList al, GenerWorkerLists wlSeles)
        {
            //  Delete work with the current non- .
            //  Has a non-coordinating or automatically assigned tasks .
            GenerWorkFlow gwf = new GenerWorkFlow(this.WorkID);
            int NodeID = gwf.FK_Node;
            Int64 workId = this.WorkID;
            //GenerWorkerLists wls = new GenerWorkerLists(this.WorkID,NodeID);
            DBAccess.RunSQL("UPDATE  WF_GenerWorkerlist SET IsEnable=0  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + NodeID);
            //  string vals = "";
            string emps = "";
            string myemp = "";
            foreach (Object obj in al)
            {
                emps += obj.ToString() + ",";
                myemp = obj.ToString();
                DBAccess.RunSQL("UPDATE  WF_GenerWorkerlist SET IsEnable=1  WHERE WorkID=" + this.WorkID + " AND FK_Node=" + NodeID + " AND fk_emp='" + obj + "'");
            }

            BP.WF.Node nd = new BP.WF.Node(NodeID);
            Work wk = nd.HisWork;

            wk.OID = this.WorkID;
            wk.Retrieve();

            wk.Emps = emps;
            wk.Update();
        }
    }
}