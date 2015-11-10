using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Port;
using BP.Sys;
using BP.DA;
using BP.WF;
using BP.WF.Template.Ext;
using BP.WF.Rpt;

namespace CCFlow.WF.Admin.XAP
{
    public partial class Designer : System.Web.UI.Page
    {
        #region  Property .
        /// <summary>
        ///  Check for updates .
        /// </summary>
        public bool IsCheckUpdate
        {
            get
            {
                if (this.Request.QueryString["IsCheckUpdate"] != null)
                    return true;

                 if (this.Request.QueryString["IsUpdate"] != null)
                    return true;

                return false;
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            #region  Check whether it is installed ccflow If you do not let it install .
            try
            {
                // If you do not Port_Dept  Table may not be installed on ccflow.
                DBAccess.RunSQL("SELECT * FROM Port_Dept WHERE 1=2");
            }
            catch
            {
                /* Database link barrier or abnormal , Description is not installed .*/
                this.Response.Redirect("../DBInstall.aspx", true);
                return;
            }
            #endregion  Check whether it is installed ccflow If you do not let it install .

            #region  Carried out admin Landed .
                Emp emp = new Emp();
                emp.No = "admin";
                if (emp.RetrieveFromDBSources() == 1)
                {
                    BP.Web.WebUser.SignInOfGener(emp, true);
                }
                else
                {
                    emp.No = "admin";
                    emp.Name = "admin";
                    emp.FK_Dept = "01";
                    emp.Pass = "pub";
                    emp.Insert();
                    BP.Web.WebUser.SignInOfGener(emp, true);
                    //throw new Exception("admin  Users lose , Please note case .");
                }
            #endregion  Carried out admin Landed .

            //  Perform the upgrade ,  Now move to upgrade the code  Glo  Inside .
            string str = BP.WF.Glo.UpdataCCFlowVer(); // Perform the upgrade .
            if (str != null)
                BP.Sys.PubClass.Alert(" System successfully upgrade to :"+str);

        }
    }
}