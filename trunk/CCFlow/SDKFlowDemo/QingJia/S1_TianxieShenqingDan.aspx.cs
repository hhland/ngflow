using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo.QingJia
{
    public partial class S1_TianxieShenqingDan : System.Web.UI.Page
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

                // Check out the data to the form assignment .
                BP.Demo.QingJia en = new BP.Demo.QingJia();
                en.OID = (int)this.WorkID;
                if (en.RetrieveFromDBSources() == 1)
                {
                    /*  Data sources have  */
                    this.TB_No.Text = en.QingJiaRenNo; //
                    this.TB_Name.Text = en.QingJiaRenName; //  Name of leave .
                    this.TB_DeptNo.Text = en.QingJiaRenDeptNo; // Department number .
                    this.TB_DeptName.Text = en.QingJiaRenDeptName; // Department name 
                    this.TB_QingJiaYuanYin.Text = en.QingJiaYuanYin; // The reason for leave 
                    this.TB_QingJiaTianShu.Text = en.QingJiaTianShu.ToString(); // Leave a few days 
                }
                else
                {
                    /* The default value for him */
                    this.TB_No.Text = BP.Web.WebUser.No;
                    this.TB_Name.Text = BP.Web.WebUser.Name;
                    this.TB_DeptNo.Text = BP.Web.WebUser.FK_Dept;
                    this.TB_DeptName.Text = BP.Web.WebUser.FK_DeptName;
                }

            }

         

        }
        /// <summary>
        ///  Performing transmission 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            //第1步:  The save .
            this.Btn_Save_Click(null, null);

            //  Check the integrity of the 
            BP.Demo.QingJia en = new BP.Demo.QingJia();
            en.OID = (int)this.WorkID;
            en.Retrieve();
            if (en.QingJiaTianShu <= 0)
            {
                this.Response.Write("<font color=red> Failed to save , Number of leave days can not be less than zero .</font>");
                return;
            }

            #region 第2步:  Performing transmission .
            BP.WF.SendReturnObjs objs = null;
            try
            {
                // Call to send api,  Back Send object .
                objs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID);
            }
            catch(Exception ex)
            {
                this.Response.Write("<font color=red> Abnormalities during transmission :" + ex.Message + "</font>");
            }
            /*
              Note here :
             * 1, Send api There are multiple ,  According to different scenarios using different api  But on that common 1个, You can see the production parameters for use .
             * BP.WF.Node_SendWork(string fk_flow, Int64 workID, int toNodeID, string toEmps) 
             * 2, Send objects back inside the system variables , These system variables include sending to whom , Send to where the .
             *  Developers can according to system variables , Implementation of the relevant business logic operations .
             */
            #endregion 第2步:  Performing transmission .

            #region 第3步:  Send out the message alert .
           
            string info = objs.ToMsgOfText();
            info = info.Replace("\t\n", "<br>@");
            info = info.Replace("@", "<br>@");
            this.Response.Write("<font color=blue>"+info+"</font>");
            #endregion 第3步:  Send out the message alert .

            this.Btn_Save.Enabled = false;
            this.Btn_Send.Enabled = false;
        }
        /// <summary>
        ///  The save .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            #region  Business data retention ,  According to workid,  Use it as the primary key , Data is stored in tables ( This section and ccflow Unrelated ).
            //本demo We use BP Frame made data storage .
            BP.Demo.QingJia en = new BP.Demo.QingJia();
            en.OID = (int)this.WorkID;
            en.QingJiaRenNo = BP.Web.WebUser.No;
            en.QingJiaRenName = BP.Web.WebUser.Name;
            en.QingJiaRenDeptNo = BP.Web.WebUser.FK_Dept;
            en.QingJiaRenDeptName = BP.Web.WebUser.FK_DeptName;
            en.QingJiaYuanYin = this.TB_QingJiaYuanYin.Text;
            en.QingJiaTianShu = float.Parse(this.TB_QingJiaTianShu.Text);

            if (en.IsExits == false)
                en.InsertAsOID(this.WorkID);  /* If not already exist .*/
            else
                en.Update();
            #endregion  Business data retention ,  According to workid,  Use it as the primary key , Data is stored in tables .

        }

        protected void Btn_Track_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_OneWork(this.FK_Flow, this.WorkID, this.FID);
        }
    }
}