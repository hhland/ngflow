using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Web.Controls;
using BP.Sys;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.Web.Comm.UI
{
	/// <summary>
	/// HelperOfTB  The summary .
	/// </summary>
    public partial class HelperOfTB :BP.Web.PageBase
    {
        protected System.Web.UI.WebControls.Label Label1;
        /// <summary>
        /// TB_Key
        /// </summary>
        public BP.Web.Controls.ToolbarTB TB_Key
        {
            get
            {
                return (ToolbarTB)this.BPToolBar2.GetTBByID("TB_Key");
            }
        }
        public BP.Web.Controls.ToolbarBtn Btn_Delete
        {
            get
            {
                return this.BPToolBar1.GetBtnByKey(NamesOfBtn.Delete);
            }
        }
        /// <summary>
        ///  The class name 
        /// </summary>
        public string EnsName
        {
            get
            {
                return this.Request.QueryString["EnsName"];

                 
            }
        }
        /// <summary>
        ///  Property  Key
        /// </summary>
        public string AttrKey
        {
            get
            {
                return this.Request.QueryString["AttrKey"];
            }
        }
        /// <summary>
        ///  Property Key
        /// </summary>
        public string AttrKeyValue
        {
            get
            {
                return this.BPToolBar2.GetTBByID("TB_Key").Text;
            }
        }
        /// <summary>
        /// ToolbarCheckBtn
        /// </summary>
        public ToolbarCheckBtn CurrentSelectedCheckButton
        {
            get
            {
                if (this.ToolbarCheckBtnGroup1.SelectedCheckButton == null)
                    this.ToolbarCheckBtnGroup1.Items[1].Selected = true;
                return (ToolbarCheckBtn)this.ToolbarCheckBtnGroup1.SelectedCheckButton;
            }
        }
        public ToolbarCheckBtnGroup ToolbarCheckBtnGroup1
        {
            get
            {
                return (ToolbarCheckBtnGroup)this.BPToolBar1.GetToolbarCheckBtnGroupByKey("ToolbarCheckBtnGroup1");
            }
        }
        public BP.En.Entity GetEn
        {
            get
            {
                return ClassFactory.GetEn(this.EnsName);
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.BPToolBar1.ButtonClick += new EventHandler(BPToolBar1_ButtonClick);
            this.BPToolBar2.ButtonClick += new EventHandler(BPToolBar2_ButtonClick);

            if (this.IsPostBack == false)
            {
                BP.En.Entity en = ClassFactory.GetEn(this.EnsName);

                this.BPToolBar1.AddLab("Lab_desc", " Gets or sets the default value ");
                this.BPToolBar1.AddSpt("spt14");
                this.BPToolBar1.AddBtn(NamesOfBtn.Delete," Delete ");
                this.BPToolBar1.AddSpt("spt1");

                this.BPToolBar1.AddToolbarCheckBtnGroup("ToolbarCheckBtnGroup1");
                this.ToolbarCheckBtnGroup1.Add("Btn_My", " My Settings ");
                if (WebUser.No == "admin")
                    this.ToolbarCheckBtnGroup1.Add("Btn_Global", " Global Settings ");

                this.ToolbarCheckBtnGroup1.Add("Btn_Top40", " Global Settings ");
                //this.ToolbarCheckBtnGroup1.Add("Btn_Top40", " History ");

                this.BPToolBar1.AddSpt("spt2");
                this.BPToolBar1.AddBtn("Btn_Help", " Help ");
                this.ToolbarCheckBtnGroup1.Items[1].Selected = true;

                this.BPToolBar1.AddSpt("spt2");

                this.BPToolBar2.AddLab("Lab2", "ֵ");
                this.BPToolBar2.AddTB("TB_Key");
                this.TB_Key.Width = new System.Web.UI.WebControls.Unit(350);
                if (WebUser.No == "admin")
                {
                    this.BPToolBar2.AddBtn("Btn_SaveToMyDefaultValues", " Save my settings ");
                    this.BPToolBar2.AddBtn("Btn_SaveToAppDefaultValues", " Save as Global Settings ");
                }
                else
                {
                    this.BPToolBar2.AddBtn("Btn_SaveToMyDefaultValues",   " Save ");
                }

                this.BPToolBar2.AddSpt("spt1");
                this.BPToolBar2.AddBtn(NamesOfBtn.Cancel);
                this.BPToolBar2.AddBtn(NamesOfBtn.Confirm);
                this.BPToolBar2.AddSpt("spt2");
                this.SetDGData();
            }

            if (this.CurrentSelectedCheckButton.Index == 2)
                this.BPToolBar1.GetBtnByKey(NamesOfBtn.Delete).Enabled = false;
            else
                this.BPToolBar1.GetBtnByKey(NamesOfBtn.Delete).Enabled = true;
        }
        /// <summary>
        ///  Set up 
        /// </summary>
        public void SetDGData()
        {
            string sql = "";
            if (this.CurrentSelectedCheckButton.Index == 0)
            {
                sql = "SELECT Val FROM Sys_DefVal WHERE FK_Emp='" + this.FK_Emp + "' AND EnsName='" + this.EnsName + "' AND AttrKey='" + this.AttrKey + "'";
            }
            else if (this.CurrentSelectedCheckButton.Index == 1)
            {
                if (this.FK_Emp != "admin")
                    this.Btn_Delete.Enabled = false;
                else
                    this.Btn_Delete.Enabled = true;

                sql = "SELECT Val FROM Sys_DefVal WHERE EnsName='" + this.EnsName + "' AND AttrKey='" + this.AttrKey + "' AND FK_Emp='0'";
            }
            else if (this.CurrentSelectedCheckButton.Index == 2)
            {
                BP.En.Entity en = ClassFactory.GetEn(this.EnsName);
                string field = en.EnMap.GetFieldByKey(this.AttrKey);


                switch (DBAccess.AppCenterDBType)
                {
                    case DBType.Oracle:
                        sql = "SELECT DISTINCT  " + field + "  FROM  " + en.EnMap.PhysicsTable + " WHERE  length(   trim(" + field + " )  ) > 0 AND rownum <=40  ";
                        break;
                    default:
                        sql = "SELECT DISTINCT TOP 40  " + field + "  FROM  " + en.EnMap.PhysicsTable + " WHERE  len(rtrim(ltrim(" + field + " )) ) > 0 ";
                        break;
                }
            }

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            this.CBL1.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                this.CBL1.Items.Add(dr[0].ToString());
            }
        }

        #region Web  Form Designer generated code 
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN:  This call is  ASP.NET Web  Form Designer required .
            //

            //this.BPToolBar1.CheckChange+=new EventHandler(BPToolBar1_CheckChange);
            InitializeComponent();
            base.OnInit(e);
        }
        /// <summary>
        ///  Required method for Designer support  -  Do not use the code editor to modify .
        ///  Contents of this method .
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                string id = "";
                try
                {
                    ToolbarBtn btn = (ToolbarBtn)sender;
                    id = btn.ID;
                }
                catch
                {
                    if (this.CurrentSelectedCheckButton.Index == 2)
                        this.BPToolBar1.GetBtnByKey(NamesOfBtn.Delete).Enabled = false;
                    else
                        this.BPToolBar1.GetBtnByKey(NamesOfBtn.Delete).Enabled = true;
                    this.SetDGData();
                    return;
                }
                switch (id)
                {
                    case NamesOfBtn.Update:
                        if (AttrKeyValue == null || AttrKeyValue == "")
                            throw new Exception("@ The default value can not be null .");
                        //DBAccess.RunSQL("update  Sys_UIDefaultValue set DefaultVal='"+this.AttrKeyValue+"' WHERE EnsName='"+this.EnsName+"' AND AttrKey='"+this.AttrKey+"' AND DefaultVal='"+this.DG1.CurrendSelectedNo+"'" );
                        this.ResponseWriteBlueMsg_UpdataOK();
                        this.SetDGData();
                        break;
                    case NamesOfBtn.Confirm:
                        this.Confirm();
                        break;
                    case NamesOfBtn.Close:
                        this.WinClose();
                        break;
                    case NamesOfBtn.Delete:
                        if (this.CurrentSelectedCheckButton.Index == 0)
                        {
                            foreach (ListItem li in this.CBL1.Items)
                            {
                                if (li.Selected == false)
                                    continue;

                                BP.Sys.DefVal dv = new DefVal();
                                dv.Delete(DefValAttr.Val, li.Text,
                                    DefValAttr.EnsName, this.EnsName,
                                    DefValAttr.FK_Emp, this.FK_Emp);
                            }
                        }
                        else
                        {
                            foreach (ListItem li in this.CBL1.Items)
                            {
                                if (li.Selected == false)
                                    continue;

                                BP.Sys.DefVal dv = new DefVal();
                                dv.Delete(DefValAttr.Val, li.Text,
                                    DefValAttr.EnsName, this.EnsName,
                                    DefValAttr.FK_Emp, "0");
                                // DBAccess.RunSQL("DELETE FROM Sys_UIDefaultValue WHERE DefaultVal='" + li.Text + "' AND ENSCLASSNAME='" + this.EnsName + "' AND AttrKey='" + this.AttrKey + "' AND No='0'");
                            }
                        }
                        this.ResponseWriteBlueMsg_DeleteOK();
                        this.SetDGData();
                        break;
                    case NamesOfBtn.New:
                        //this.DG1.SelectedIndex =-1;
                        this.TB_Key.Text = "";
                        break;
                    case NamesOfBtn.Help:
                        this.Helper();
                        break;
                    default:
                        this.SetDGData();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);
                //this.ResponseWriteRedMsg(ex);
            }
        }
        private void Confirm()
        {
            try
            {
                int i = 0;
                string str = "";
                foreach (ListItem li in this.CBL1.Items)
                {
                    if (li.Selected)
                    {
                        i++;
                        str += i + "," + li.Text + "\\n";
                    }
                }
                if (i == 0)
                {
                    this.WinClose();
                    return;
                }
                if (i == 1)
                    str = str.Replace("1,", "");
                str = str.Replace(":", "\\:");
                str = str.Replace(" ", "");
                str = str.Replace("\r\n", "");

                string clientscript = "<script language='javascript'> window.returnValue = '" + str + "'; window.close(); </script>";
                this.Page.Response.Write(clientscript);
            }
            catch (System.Exception ex)
            {
                this.Alert(ex);
            }
        }
        /// <summary>
        /// UserID
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return WebUser.No;
            }
        }
        private void BPToolBar2_ButtonClick(object sender, System.EventArgs e)
        {
            try
            {
                ToolbarBtn btn = (ToolbarBtn)sender;
                DefVal en = new DefVal();
                QueryObject qo = new QueryObject(en);

                // BP.En.Entity enDA = BP.En.ClassFactory.GetEn(this.EnsName);
                switch (btn.ID)
                {
                    case NamesOfBtn.Cancel:
                        this.WinClose();
                        break;
                    case "Btn_SaveToMyDefaultValues":
                        if (AttrKeyValue.Trim().Length == 0)
                            throw new Exception("@ The default value can not be null .");

                        en.FK_Emp = WebUser.No;
                        en.EnsName = this.EnsName;
                        en.AttrKey = this.AttrKey;
                        en.Val = this.AttrKeyValue;

                        qo.AddWhere(DefValAttr.FK_Emp, en.FK_Emp);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.AttrKey, en.AttrKey);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.EnsName, en.EnsName);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.Val, en.Val);
                        if (qo.DoQuery() == 0)
                            en.Insert();

                        this.SetDGData();
                        this.ResponseWriteBlueMsg_SaveOK();
                        break;
                    case "Btn_SaveToAppDefaultValues":
                        if (AttrKeyValue.Trim().Length == 0)
                            throw new Exception("@ The default value can not be null .");

                        en.FK_Emp = "0";
                        en.EnsName = this.EnsName;
                        en.AttrKey = this.AttrKey;
                        en.Val = this.AttrKeyValue;

                        qo.AddWhere(DefValAttr.FK_Emp, en.FK_Emp);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.AttrKey, en.AttrKey);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.EnsName, en.EnsName);
                        qo.addAnd();
                        qo.AddWhere(DefValAttr.Val, en.Val);
                        if (qo.DoQuery() == 0)
                            en.Insert();

                        this.SetDGData();
                        this.ResponseWriteBlueMsg_SaveOK();
                        break;
                    case NamesOfBtn.Confirm:
                        this.Confirm();
                        break;
                    default:
                        throw new Exception(" Untreated controls " + btn.ID);
                }
            }
            catch (Exception ex)
            {
                this.ResponseWriteRedMsg(ex);
            }
        }
    }
}
