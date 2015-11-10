using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.QingJia
{
    public partial class S3_ZongJingLiShenpi : System.Web.UI.Page
    {

        #region  Accept 4 Large parameter ( These four parameters are ccflow Passing on this page ).
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
        ///  The current node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        #endregion  Accept 4 Large parameter ( These four parameters are ccflow Passing on this page ).

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == false)
            {

                //  Check out .
                BP.Demo.QingJia en = new BP.Demo.QingJia();
                en.OID = (int)this.WorkID;
                en.Retrieve();

                //  Basic information leave people assignment .
                this.TB_No.Text = en.QingJiaRenNo; //
                this.TB_Name.Text = en.QingJiaRenName; //  Name of leave .
                this.TB_DeptNo.Text = en.QingJiaRenDeptNo; // Department number .
                this.TB_DeptName.Text = en.QingJiaRenDeptName; // Department name 
                this.TB_QingJiaYuanYin.Text = en.QingJiaYuanYin; // The reason for leave 
                this.TB_QingJiaTianShu.Text = en.QingJiaTianShu.ToString(); // Leave a few days .

                // Field assignment to the department manager for approval .
                this.TB_BMNote.Text = en.NoteBM;
            }
        }

        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            //  The save .
            Btn_Save_Click(null, null);

            #region 第2步:  Performing transmission .

            // Call to send api,  Back Send object .
            BP.WF.SendReturnObjs objs = null;


            //  Check out .
            BP.Demo.QingJia en = new BP.Demo.QingJia();
            en.OID = (int)this.WorkID;
            en.Retrieve();

            //  Send Down .
            objs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID, 0, null);


            /*
              Note here : 
             * 1, Send api There are multiple ,  According to different scenarios using different api  But on that common 1个, You can see the production parameters for use .
             * BP.WF.Node_SendWork(string fk_flow, Int64 workID, int toNodeID, string toEmps) 
             *
             * 2, Send objects back inside the system variables , These system variables include sending to whom , Send to where the .
             *  Developers can according to system variables , Implementation of the relevant business logic operations .
             */
            #endregion 第2步:  Performing transmission .


            #region 第3步:  Send out the message alert .
            string info = objs.ToMsgOfText();
            info = info.Replace("\t\n", "<br>@");
            info = info.Replace("@", "<br>@");
            this.Response.Write("<font color=blue>" + info + "</font>");
            #endregion 第3步:  Send out the message alert .

            // Set interface buttons can not be used .
            this.Btn_Save.Enabled = false;
            this.Btn_Send.Enabled = false;
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            BP.Demo.QingJia en = new BP.Demo.QingJia();
            en.OID = (int)this.WorkID;
            en.Retrieve();

            en.NoteBM = this.TB_NoteRL.Text;
            en.Update(); //  Write to the department manager comments .
        }

        protected void Btn_Track_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_OneWork(this.FK_Flow, this.WorkID, this.FID);

        }

        protected void Btn_Return_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_Return(this.FK_Flow, this.FK_Node, this.WorkID, this.FID);
        }
    }
}