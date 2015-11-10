using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.WorkOpt
{
    public partial class CCCheckNote : BP.Web.WebPage
    {
        #region  Parameters 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                string note = BP.WF.Dev2Interface.GetCheckInfo(this.FK_Flow, this.WorkID, this.FK_Node);
                if (string.IsNullOrEmpty(note))
                {
                    BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.FK_Node, this.WorkID, this.FID," Have read ", " Access to knowledge ");
                    note = " Have read ";
                }
                this.TextBox1.Text = note;
            }
        }

        protected void Btn_OK_Click(object sender, EventArgs e)
        {

            BP.Sys.FrmWorkCheck fwc = new BP.Sys.FrmWorkCheck(this.FK_Node);

            BP.WF.Dev2Interface.WriteTrackWorkCheck(this.FK_Flow, this.FK_Node, this.WorkID,
                this.FID, this.TextBox1.Text, fwc.FWCOpLabel);

            // Setting audit completed .
            BP.WF.Dev2Interface.Node_CC_SetSta(this.FK_Node, this.WorkID, BP.Web.WebUser.No, BP.WF.Template.CCSta.CheckOver);

            this.WinCloseWithMsg(" Audit Success ");
        }
    }
}