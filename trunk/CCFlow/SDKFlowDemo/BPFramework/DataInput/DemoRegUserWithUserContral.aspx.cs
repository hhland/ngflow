using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Demo;
namespace CCFlow.SDKFlowDemo
{
    public partial class DemoRegUserWithUserContral : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Pub1.AddTable();
            this.Pub1.AddCaption(" Demonstrate the use of BP User control and collect data to present .");

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Account number ");
            BP.Web.Controls.TB tb =new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.No;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Can not be empty , A combination of letters or underscore .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Password ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Pass;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Weight lost password ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_Pass1";
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Password can not be repeated twice .");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Full name ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Can not be empty ");
            this.Pub1.AddTREnd();

            // Enumerated type .
            this.Pub1.AddTR();
            this.Pub1.AddTD(" Sex ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.BindSysEnum("XB"); // 在Sys_Eumm  Already a member of the enumeration values .
            ddl.ID = "TB_" + BPUserAttr.XB;
            this.Pub1.AddTD(ddl);
            this.Pub1.AddTD(" Please select ");
            this.Pub1.AddTREnd();


            //  Numeric Types .
            this.Pub1.AddTR();
            this.Pub1.AddTD(" Age ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Age;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Enter int Types of data .");
            this.Pub1.AddTREnd();


            this.Pub1.AddTR();
            this.Pub1.AddTD(" Address ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Addr;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Phone ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Tel;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Mail ");
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_" + BPUserAttr.Email;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            Button btn = new Button();
            btn.ID = "Btn_Reg";
            btn.Text = " Sign up ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.AddTD("colspan=3",btn);
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

        }

        void btn_Click(object sender, EventArgs e)
        {
            try
            {
                string pass = this.Pub1.GetTBByID("TB_Pass").Text;
                string pass1 = this.Pub1.GetTBByID("TB_Pass1").Text;

                // Do a complete check before submitting .
                if (string.IsNullOrEmpty(pass))
                    throw new Exception(" Passwords can not be empty .");
                if (pass != pass1)
                    throw new Exception(" Enter the password twice inconsistent .");

                // Create a entity.
                BP.Demo.BPUser user = new BP.Demo.BPUser();
                user.CheckPhysicsTable();

                // Carried out copy, 从Pub1中, As for how to achieve please go inside track to the method .
                user = this.Pub1.Copy(user) as BP.Demo.BPUser;
                if (user.IsExits)
                    throw new Exception("@ Username :" + user.No + " Already exists .");

                // Assign the current date .
                user.FK_NY = BP.DA.DataType.CurrentYearMonth;
                // Executing data insertion .
                user.Insert();
                BP.Sys.PubClass.Alert(" User registration is successful ");
                this.Pub1.GetButtonByID("Btn_Reg").Enabled = false;
            }
            catch (Exception ex)
            {
                BP.Sys.PubClass.Alert(ex.Message);
                return;
            }
        }
    }
}