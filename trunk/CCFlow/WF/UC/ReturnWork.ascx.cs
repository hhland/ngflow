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
using BP.Sys;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class UCReturnWork : BP.Web.UC.UCBase3
    {
        #region  Property 
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
        public string TBClientID
        {
            get
            {
                try
                {
                    return TB1.ClientID;
                }
                catch
                {
                    return "ss";
                }
            }
        }
        public string Info
        {
            get
            {
                   string str=this.Request.QueryString["Info"];
                   if (string.IsNullOrEmpty(str))
                       return null;
                   else
                       return str;
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            #region  You can find the node returned .
            DataTable dt = null;
            if (this.IsPostBack == false)
            {
                dt = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(this.FK_Node, this.WorkID, this.FID);
                if (dt.Rows.Count == 0)
                {
                    this.Pub1.Clear();
                    this.Pub1.AddFieldSet(" Return error ", " The system can not find the return nodes .");
                    return;
                }
            }
            #endregion  You can find the node returned .

            this.Page.Title = " Return to work ";
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            this.ToolBar1.Add("<b> Return to :</b>");
            this.ToolBar1.AddDDL("DDL1");
            this.DDL1.Attributes["onchange"] = "OnChange(this);";
            this.ToolBar1.AddBtn("Btn_OK", " Determine ");
            this.ToolBar1.GetBtnByID("Btn_OK").Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
            this.ToolBar1.GetBtnByID("Btn_OK").Click += new EventHandler(ReturnWork_Click);

           // if (this.Request.QueryString["IsEUI"] == null)
            //{
                this.ToolBar1.AddBtn("Btn_Cancel", " Cancel ");
                this.ToolBar1.GetBtnByID("Btn_Cancel").Click += new EventHandler(ReturnWork_Click);
            //}

            string appPath = this.Request.ApplicationPath;
            if (nd.IsBackTracking)
            {
                /* If you allow the same route back */
                CheckBox cb = new CheckBox();
                cb.ID = "CB_IsBackTracking";
                cb.Text = " Do you want to backtrack after return ?";
                this.ToolBar1.Add(cb);
            }


            TextBox tb = new TextBox();
            tb.TextMode = TextBoxMode.MultiLine;
            tb.ID = "TB_Doc";
            tb.Rows = 15;
            tb.Columns = 50;

            // Added 
            Label lb = new Label();
            lb.Text = "<div style='float:left;display:block;width:100%'><a href=javascript:TBHelp('TB_Doc')><img src='" + BP.WF.Glo.CCFlowAppPath + "WF/Img/Emps.gif' align='middle' border=0 /> Choose vocabulary </a>&nbsp;&nbsp;</div>";
            this.Pub1.Add(lb);

            this.Pub1.Add(tb);

            if (this.IsPostBack == false)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    this.DDL1.Items.Add(new ListItem(dr["RecName"] + "=>" + dr["Name"].ToString(), dr["No"].ToString() + "@" + dr["Rec"].ToString()));
                }

                WorkNode wn = new WorkNode(this.WorkID, this.FK_Node);
                WorkNode pwn = wn.GetPreviousWorkNode();

                this.DDL1.SetSelectItem(pwn.HisNode.NodeID);
                this.DDL1.Enabled = true;
                Work wk = pwn.HisWork;
                if (this.Info != null)
                {
                    this.TB1.Text = this.Info;
                    // Can not save the audit information into the form in , Because the next time he would not review this information the . 
                    //string sql = "UPDATE "+wn.HisWork.EnMap.PhysicsTable+" SET "+;
                }
                else
                {
                    /* Check whether the component is enabled auditing , Fill in the information in the audit to see what ?*/
                    BP.Sys.FrmWorkCheck fwc = new FrmWorkCheck(this.FK_Node);
                    if (fwc.HisFrmWorkCheckSta == FrmWorkCheckSta.Enable)
                    {
                        this.TB1.Text = BP.WF.Dev2Interface.GetCheckInfo(this.FK_Flow, this.WorkID, this.FK_Node);
                        if (tb.Text == " Agree ")
                            tb.Text = "";

                    }
                    else
                    {
                        string info = this.DDL1.SelectedItem.Text;
                        string recName = info.Substring(0, info.IndexOf('='));
                        string nodeName = info.Substring(info.IndexOf('>') + 1);
                        //this.TB1.Text = string.Format("{0} Comrade : \n   You are dealing with [{1}] Working with errors , You need to re-apply ．\n\n Yours sincerely !!!   \n\n  {2}", recName,
                          //  nodeName, wk.CDT, pwn.HisNode.Name, WebUser.Name + "\n  " + BP.DA.DataType.CurrentDataTime);
                        this.TB1.Text = string.Format(GetGlobalResourceMsg("WorkReturnMsgContent.Pattern"), recName,
                           nodeName, wk.CDT, pwn.HisNode.Name, WebUser.Name + "\n  " + BP.DA.DataType.CurrentDataTime);
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
                    BP.WF.Node curNd = new BP.WF.Node(this.FK_Node);
                    if (curNd.FormType == NodeFormType.SheetTree)
                    {
                        this.WinClose();
                    }
                    else
                    {
                        this.Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID, true);
                    }
                    return;
                default:
                    break;
            }

            try
            {
                string returnInfo = this.TB1.Text;
                string reNodeEmp = this.DDL1.SelectedItemStringVal;
                bool IsBackTracking = false;
                try
                {
                    IsBackTracking = this.ToolBar1.GetCBByID("CB_IsBackTracking").Checked;
                }
                catch
                {
                }

                string[] strs = reNodeEmp.Split('@');
                // Implementation of return api.
                string rInfo = BP.WF.Dev2Interface.Node_ReturnWork(this.FK_Flow, this.WorkID, this.FID,
                    this.FK_Node, int.Parse(strs[0]), strs[1], returnInfo, IsBackTracking
                    , GetGlobalResourceMsg("WorkReturnMsgTitle.Pattern")
                    );
                this.ToMsg(rInfo, "info");
                return;
            }
            catch (Exception ex)
            {
                this.ToMsg(ex.Message, "info");
            }
        }
        public void ToMsg(string msg, string type)
        {
            string rowUrl = this.Request.RawUrl;
            if (rowUrl.Contains("&IsClient=1"))
            {
                /* Description This is vsto Calls .*/
                return;
            }

            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            this.Response.Redirect(BP.WF.Glo.CCFlowAppPath + "WF/MyFlowInfo.aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
        }
    }

}