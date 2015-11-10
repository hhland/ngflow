using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.App.F001
{
    public partial class S101 : System.Web.UI.Page
    {
        #region  Public Methods 
        public void ToMsg(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect(
                "/SDKFlowDemo/SDK/Info.aspx?FK_Flow=2&FK_Type=2&FK_Node=2&WorkID=22" + DateTime.Now.ToString(), false);
        }
        public void ToErrorPage(string msg)
        {
            System.Web.HttpContext.Current.Session["info"] = msg;
            System.Web.HttpContext.Current.Application["info" + BP.Web.WebUser.No] = msg;
            BP.WF.Glo.SessionMsg = msg;
            System.Web.HttpContext.Current.Response.Redirect("/SDKFlowDemo/SDK/ErrorPage.aspx", false);
        }
        #endregion  Public Methods s

        #region  Accept 4 Large parameter .
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
        #endregion 4 Large parameter .

        protected void Page_Load(object sender, EventArgs e)
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            this.Page.Title = " Hello :" + BP.Web.WebUser.No + " - " + BP.Web.WebUser.Name + " .  The current node :" + nd.Name;
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

                if (this.FK_Node != 11001)
                {
                    /* If this is not the start node , Allowed to read only .*/
                    this.TB_No.ReadOnly = true; //
                    this.TB_Name.ReadOnly = true; //  Name of leave .
                    this.TB_DeptNo.ReadOnly = true; // Department number .
                    this.TB_DeptName.ReadOnly = true; // Department name 
                    
                    this.TB_QingJiaTianShu.ReadOnly = true;
                    this.TB_QingJiaYuanYin.ReadOnly = true;
                }
            }
        }
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                Btn_Save_Click(null, null); // The save .
                BP.WF.SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID);//  Performing transmission .
                this.ToMsg(objs.ToMsgOfHtml()); // Output information .
            }
            catch (Exception ex)
            {
                this.ToMsg(ex.Message);
            }
        }
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            #region  Business data retention ,  According to workid,  Use it as the primary key , Data is stored in tables ( This section and ccflow Unrelated ).
            if (this.FK_Node == 11001)
            {
                /* If this is the start node , Before the implementation of preservation .*/
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
            }
            #endregion  Business data retention ,  According to workid,  Use it as the primary key , Data is stored in tables .
        }

    }
}