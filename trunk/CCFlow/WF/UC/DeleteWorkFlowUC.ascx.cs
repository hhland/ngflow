using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.UC
{
    public partial class DeleteWorkFlowUC : BP.Web.UC.UCBase3
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public int FID
        {
            get
            {

                return int.Parse(this.Request.QueryString["FID"]);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public DDL DDL1
        {
            get
            {
                return this.ToolBar1.GetDDLByID("DDL1");
            }
        }
        public TextBox TB1
        {
            get
            {
                return this.Pub1.GetTextBoxByID("TB_Doc");
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Job Remove ";
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            this.ToolBar1.Add("<b> Delete way :</b>");
            this.ToolBar1.AddDDL("DDL1");
            this.DDL1.Attributes["onchange"] = "OnChange(this);";
            this.ToolBar1.AddBtn("Btn_OK", " Determine ");
            this.ToolBar1.GetBtnByID("Btn_OK").Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
            this.ToolBar1.GetBtnByID("Btn_OK").Click += new EventHandler(ReturnWork_Click);
            this.ToolBar1.AddBtn("Btn_Cancel", " Cancel ");
            this.ToolBar1.GetBtnByID("Btn_Cancel").Click += new EventHandler(ReturnWork_Click);
            string appPath = this.Request.ApplicationPath;
            TextBox tb = new TextBox();
            tb.TextMode = TextBoxMode.MultiLine;
            tb.ID = "TB_Doc";
            tb.Rows = 15;
            tb.Columns = 50;
            this.Pub1.Add(tb);
            if (this.IsPostBack == false)
            {
                if (nd.HisDelWorkFlowRole == DelWorkFlowRole.DeleteAndWriteToLog)
                {
                    /* Remove and logging  */
                    SysEnum se = new SysEnum(BtnAttr.DelEnable, (int)DelWorkFlowRole.DeleteAndWriteToLog);
                    this.DDL1.Items.Add(new ListItem(se.Lab, se.IntKey.ToString()));
                }

                if (nd.HisDelWorkFlowRole == DelWorkFlowRole.DeleteByFlag)
                {
                    /* Tombstone  */
                    SysEnum se = new SysEnum(BtnAttr.DelEnable, (int)DelWorkFlowRole.DeleteByFlag);
                    this.DDL1.Items.Add(new ListItem(se.Lab, se.IntKey.ToString()));
                }

                if (nd.HisDelWorkFlowRole == DelWorkFlowRole.ByUser)
                {
                    /* Allows the user to decide .*/
                    SysEnums ses = new SysEnums(BtnAttr.DelEnable);
                    foreach (SysEnum se in ses)
                    {
                        DelWorkFlowRole role = (DelWorkFlowRole)se.IntKey;
                        if (role == DelWorkFlowRole.None)
                            continue;
                        if (role == DelWorkFlowRole.ByUser)
                            continue;
                        this.DDL1.Items.Add(new ListItem(se.Lab, se.IntKey.ToString()));
                    }
                }
            }
        }
        void ReturnWork_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (btn.ID)
            {
                case "Btn_Cancel":
                    this.Response.Redirect("MyFlow" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                    return;
                default:
                    break;
            }

            try
            {
                string info = this.TB1.Text;
                BP.WF.DelWorkFlowRole role = (BP.WF.DelWorkFlowRole)this.DDL1.SelectedItemIntVal;
                string rInfo = "";
                switch (role)
                {
                    case DelWorkFlowRole.DeleteAndWriteToLog:
                        rInfo=BP.WF.Dev2Interface.Flow_DoDeleteFlowByWriteLog(this.FK_Flow, this.WorkID, info, true);
                        break;
                    case DelWorkFlowRole.DeleteByFlag:
                        rInfo = BP.WF.Dev2Interface.Flow_DoDeleteFlowByFlag(this.FK_Flow, this.WorkID, info, true);
                        break;
                    case DelWorkFlowRole.DeleteReal:
                        rInfo = BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(this.FK_Flow, this.WorkID, true);
                        break;
                    default:
                        throw new Exception("@ Not related to the deletion .");
                }
                this.ToMsg(rInfo, "info");
            }
            catch (Exception ex)
            {
                this.ToMsg(ex.Message, "info");
            }
        }
        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Response.Redirect("MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }
    }
}