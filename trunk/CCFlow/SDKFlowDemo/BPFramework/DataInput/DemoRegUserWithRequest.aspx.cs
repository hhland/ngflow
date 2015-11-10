using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.SDKFlowDemo
{
    public partial class DemoRegUserWithRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btn_Reg_Click(object sender, EventArgs e)
        {
            try
            {
                // Do a complete check before submitting .
                if (string.IsNullOrEmpty(this.TB_Pass.Text.Trim()))
                    throw new Exception(" Passwords can not be empty .");

                if (this.TB_Pass.Text.Trim() != this.TB_Pass1.Text.Trim())
                    throw new Exception(" Enter the password twice inconsistent .");

                // Create a entity.
                BP.Demo.BPUser user = new BP.Demo.BPUser();

                // Carried out copy, 从Request中, As for how to achieve please go inside track to the method .
                user = BP.Sys.PubClass.CopyFromRequest(user) as BP.Demo.BPUser;
                if (user.IsExits)
                    throw new Exception("@ Username :" + user.No + " Already exists .");

                // Assign the current date .
                user.FK_NY = BP.DA.DataType.CurrentYearMonth;

                // Executing data insertion .
                user.Insert();
                BP.Sys.PubClass.Alert(" User registration is successful ");
                this.Btn_Reg.Enabled = false;
            }
            catch(Exception ex)
            {
                BP.Sys.PubClass.Alert(ex.Message);
                return;
            }

        }
    }
}