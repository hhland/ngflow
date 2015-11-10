using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class WF_Admin_UC_CondBySQL : BP.Web.UC.UCBase3
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
                return this.FK_MainNode + "_" + this.ToNodeID + "_" + this.HisCondType.ToString() + "_" + ConnDataFrom.SQL.ToString();
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
            this.AddTD("class='GroupTitle'", " Set up SQL");
            this.AddTREnd();

            AddTR();

            TextBox tb = new TextBox();
            tb.ID = "TB_SQL";
            tb.TextMode = TextBoxMode.MultiLine;
            tb.Rows = 10;
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
            btn.Attributes["onclick"] = " return confirm(' Are you sure you want to delete it ?');";
            btn.Click += new EventHandler(btn_Click);
            this.Add(btn);
            AddBR();
            AddBR();

            string help = "";
            help += "<ul>";
            help += "<li> Set up a query in the text box SQL, It returns a row . Such as : SELECT COUNT(*) AS Num FROM MyTable WHERE NAME='@MyFieldName'. </li>";
            help += "<li> This SQL Expression argument support system , What is ccflow See instructions expression .</li>";
            help += "<li> The system will get the value of the return of put it into decimal Type </li>";
            help += "<li> If the value is greater than zero , The condition is established or not the establishment .</li>";
            help += "</ul>";
            AddEasyUiPanelInfo(" Help ",  help);
        }

        void btn_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkBtn;

            if (btn.ID == NamesOfBtn.Delete)
            {
                #region songhonggang (2014-06-15)  Click Delete to delete the time to modify the conditions 
                Cond deleteCond = new Cond();
                deleteCond.Delete(CondAttr.NodeID, this.FK_MainNode,
                  CondAttr.ToNodeID, this.ToNodeID,
                  CondAttr.CondType, (int)this.HisCondType);
                #endregion
                this.Response.Redirect(this.Request.RawUrl, true);

                return;
            }

            string sql = this.GetTextBoxByID("TB_SQL").Text;

            if (string.IsNullOrEmpty(sql))
            {
                this.Alert(" Please fill in sql Statement .");
                return;
            }

            Cond cond = new Cond();
            cond.Delete(CondAttr.NodeID, this.FK_MainNode,
              CondAttr.ToNodeID, this.ToNodeID,
              CondAttr.CondType, (int)this.HisCondType);

            cond.MyPK = this.GenerMyPK;
            cond.HisDataFrom = ConnDataFrom.SQL;
            cond.NodeID = this.FK_MainNode;
            cond.FK_Node = this.FK_MainNode;
            cond.FK_Flow = this.FK_Flow;
            cond.ToNodeID = this.ToNodeID;
            cond.OperatorValue = sql;
            cond.FK_Flow = this.FK_Flow;
            cond.HisCondType = this.HisCondType;

            cond.Insert();

            EasyUiHelper.AddEasyUiMessager(this, " Saved successfully !");

            //switch (this.HisCondType)
            //{
            //    case CondType.Flow:
            //    case CondType.Node:
            //        cond.Update();
            //        this.Response.Redirect("CondDept.aspx?MyPK=" + cond.MyPK + "&FK_Flow=" + cond.FK_Flow + "&FK_Node=" + cond.FK_Node + "&FK_MainNode=" + cond.NodeID + "&CondType=" + (int)cond.HisCondType + "&FK_Attr=" + cond.FK_Attr, true);
            //        return;
            //    case CondType.Dir:
            //        cond.ToNodeID = this.ToNodeID;
            //        cond.Update();
            //        this.Response.Redirect("CondDept.aspx?MyPK=" + cond.MyPK + "&FK_Flow=" + cond.FK_Flow + "&FK_Node=" + cond.FK_Node + "&FK_MainNode=" + cond.NodeID + "&CondType=" + (int)cond.HisCondType + "&FK_Attr=" + cond.FK_Attr + "&ToNodeID=" + this.Request.QueryString["ToNodeID"], true);
            //        return;
            //    default:
            //        throw new Exception(" Without design .");
            //}
        }
    }
}