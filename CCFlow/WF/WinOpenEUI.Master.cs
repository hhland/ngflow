using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.WF;
using BP.WF.Template;

namespace CCFlow.WF
{
    public partial class WinOpenEUI : System.Web.UI.MasterPage
    {
        #region  Property 
        /// <summary>
        ///  The current process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(s))
                    throw new Exception("@ Process ID parameter error ...");

                return BP.WF.Dev2Interface.TurnFlowMarkToFlowNo(s);
            }
        }
        public string FromNode
        {
            get
            {
                return this.Request.QueryString["FromNode"];
            }
        }
        public string DoFunc
        {
            get
            {
                return this.Request.QueryString["DoFunc"];
            }
        }
        public string CFlowNo
        {
            get
            {
                return this.Request.QueryString["CFlowNo"];
            }
        }
        public string WorkIDs
        {
            get
            {
                return this.Request.QueryString["WorkIDs"];
            }
        }
        public string Nos
        {
            get
            {
                return this.Request.QueryString["Nos"];
            }
        }

        public bool IsCC
        {
            get
            {

                if (string.IsNullOrEmpty(this.Request.QueryString["Paras"]) == false)
                {
                    string myps = this.Request.QueryString["Paras"];
                    if (myps.Contains("IsCC") == true)
                        return true;
                }
                return false;
            }
        }

        private Int64 _workid = 0;
        /// <summary>
        ///  Current work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {

                if (this.Request.QueryString["WorkID"] == null)
                    return _workid;
                else
                    return Int64.Parse(this.Request.QueryString["WorkID"]);

            }
            set
            {
                _workid = value;
            }
        }
        public Int64 CWorkID
        {
            get
            {
                if (ViewState["CWorkID"] == null)
                {
                    if (this.Request.QueryString["CWorkID"] == null)
                        return 0;
                    else
                        return Int64.Parse(this.Request.QueryString["CWorkID"]);
                }
                else
                    return Int64.Parse(ViewState["CWorkID"].ToString());
            }
            set
            {
                ViewState["CWorkID"] = value;
            }
        }
        private int _FK_Node = 0;
        /// <summary>
        ///  Current  NodeID , At the beginning of time ,nodeID, Is to a , Start node processes ID.
        /// </summary>
        public int FK_Node
        {
            get
            {
                string fk_nodeReq = this.Request.QueryString["FK_Node"];
                if (string.IsNullOrEmpty(fk_nodeReq))
                    fk_nodeReq = this.Request.QueryString["NodeID"];

                if (string.IsNullOrEmpty(fk_nodeReq) == false)
                    return int.Parse(fk_nodeReq);

                if (_FK_Node == 0)
                {
                    if (this.Request.QueryString["WorkID"] != null)
                    {
                        string sql = "SELECT FK_Node from  WF_GenerWorkFlow where WorkID=" + this.WorkID;
                        _FK_Node = DBAccess.RunSQLReturnValInt(sql);
                    }
                    else
                    {
                        _FK_Node = int.Parse(this.FK_Flow + "01");
                    }
                }
                return _FK_Node;
            }
        }
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
        public int PWorkID
        {
            get
            {
                try
                {
                    string s = this.Request.QueryString["PWorkID"];
                    if (string.IsNullOrEmpty(s) == true)
                        s = this.Request.QueryString["PWorkID"];
                    if (string.IsNullOrEmpty(s) == true)
                        s = "0";
                    return int.Parse(s);
                }
                catch
                {
                    return 0;
                }
            }
        }

        private bool isTab = false;

        public bool IsTab
        {
            get { return isTab; }
            set { isTab = value; }
        }

        private string officeTabName = " Text ";

        public string OfficeTabName
        {
            get { return officeTabName; }
            set { officeTabName = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
        //    this.Page.RegisterClientScriptBlock("s",
        //"<link href='" + BP.WF.Glo.CCFlowAppPath + "WF/Comm/Style/Table" + BP.Web.WebUser.Style + ".css' rel='stylesheet' type='text/css' />");
            try
            {
                BtnLab btnLab = new BtnLab(FK_Node);

                if (btnLab.WebOfficeEnable == 2)
                {
                    IsTab = true;
                    OfficeTabName = btnLab.WebOfficeLab;
                    if (WorkID == 0)
                    {
                        Flow currFlow = new Flow(this.FK_Flow);
                        WorkID = currFlow.NewWork().OID;
                    }
                }
            }
            catch 
            { }
        }
    }
}