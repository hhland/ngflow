using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.Admin.UC
{
    public partial class WF_Admin_UC_CondByUrl : BP.Web.UC.UCBase3
    {
        #region  Property 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public new string MyPK
        {
            get
            {
                return this.Request.QueryString["MyPK"];
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
        /// <summary>
        ///  Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return this.FK_MainNode;
                }
            }
        }
        public int FK_MainNode
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_MainNode"]);
            }
        }
        public int ToNodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["ToNodeID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  Execution Type 
        /// </summary>
        public CondType HisCondType
        {
            get
            {
                return (CondType)int.Parse(this.Request.QueryString["CondType"]);
            }
        }
        public string GetOperVal
        {
            get
            {
                if (this.IsExit("TB_Val"))
                    return this.GetTBByID("TB_Val").Text;
                return this.GetDDLByID("DDL_Val").SelectedItemStringVal;
            }
        }
        public string GetOperValText
        {
            get
            {
                if (this.IsExit("TB_Val"))
                    return this.GetTBByID("TB_Val").Text;
                return this.GetDDLByID("DDL_Val").SelectedItem.Text;
            }
        }
        public string GenerMyPK
        {
            get
            {
                return this.FK_MainNode + "_" + this.ToNodeID + "_" + this.HisCondType.ToString() + "_" + ConnDataFrom.Url.ToString();
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            Cond cond = new Cond();
            cond.MyPK = this.GenerMyPK;
            cond.RetrieveFromDBSources();

            this.AddTable("class='Table' cellSpacing='1' cellPadding='1'  border='1' style='width:100%'");
            this.AddTR();
            this.AddTD("class='GroupTitle'", " Setting a URL,this Url Returns a value , Used to determine the direction and whether through ");
            this.AddTREnd();

            AddTR();

            TextBox tb = new TextBox();
            tb.ID = "TB_Para";
            tb.Rows = 1;
            tb.Columns = 80;
            tb.Style.Add("width", "99%");
            tb.Text = cond.OperatorValueStr;
            AddTD("", tb);

            AddTREnd();
            AddTableEnd();

            AddBR();
            AddSpace(1);

            var btn = new LinkBtn(false, NamesOfBtn.Save, " Save ");
            btn.Click += new EventHandler(btn_Click);
            this.Add(btn);
            AddSpace(1);

            btn = new LinkBtn(false, NamesOfBtn.Delete, " Delete ");
            btn.Attributes["onclick"] = "return confirm(' Are you sure you want to delete it ?');";
            btn.Click += new EventHandler(btn_Click);
            this.Add(btn);
            AddBR();
            AddBR();



            AddEasyUiPanelInfo(" Help ", " Format : http://xxxx/xxx.xxx?s=sss , 此url Returns less than or equal 0 Is not passed , Greater than or equal 1 Through . System calls this url When it will be passed to 4 Large parameter :FK_Flow,FK_Node,WorkID,FID You through these four parameters , You can obtain the current process data .");
            //  Also WebUser.No,SID Two calibration parameters ,
        }

        void btn_Click(object sender, EventArgs e)
        {
            string exp = this.GetTextBoxByID("TB_Para").Text;

            if (string.IsNullOrEmpty(exp))
            {
                this.Alert(" Please fill in the format expression .");
                return;
            }

            exp = exp.Trim();

            Cond cond = new Cond();
            cond.Delete(CondAttr.NodeID, this.FK_MainNode,
                  CondAttr.ToNodeID, this.ToNodeID,
                  CondAttr.CondType, (int)this.HisCondType);
             
            var btn = sender as LinkBtn;

            if (btn.ID == NamesOfBtn.Delete)
            {
                this.Response.Redirect(this.Request.RawUrl, true);
                return;
            }

            cond.MyPK = this.GenerMyPK;
            cond.HisDataFrom = ConnDataFrom.Url;
            cond.NodeID = this.FK_MainNode;
            cond.FK_Node = this.FK_MainNode;
            cond.FK_Flow = this.FK_Flow;
            cond.ToNodeID = this.ToNodeID;
            cond.OperatorValue = exp;
            cond.FK_Flow = this.FK_Flow;
            cond.HisCondType = this.HisCondType;
            cond.FK_Node = this.FK_Node;
            cond.Insert();

            EasyUiHelper.AddEasyUiMessager(this, " Saved successfully !");
        }
    }
}