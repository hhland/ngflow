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
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class SMS : BP.Web.UC.UCBase3
    {
        public Int32 WorkID
        {
            get
            {
                try
                {
                    return Int32.Parse(this.Request.QueryString["WorkID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int NodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return 0;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Title = "SMS";

            this.AddMsgOfInfo(" Error ", " SMS equipment is not installed ");
            return;


            string sql = "SELECT No,Name,Tel FROM WF_Emp WHERE NO IN (select FK_Emp from WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " AND FK_Node=" + this.NodeID + ")";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);

            if (dt.Rows.Count == 0)
            {
                this.WinCloseWithMsg(" I am sorry , Staff is not set to accept SMS Alert .");
                return;
            }

            this.AddFieldSet(" Send SMS reminders ");
            this.AddTable();
            this.AddTR();
            this.Add("<TD class=BigDoc>");
            bool isHave = false;
            foreach (DataRow dr in dt.Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + dr["No"].ToString();
                BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(dr["No"].ToString());
                if (emp.Tel.Length > 10)
                {
                    cb.Checked = true;
                    cb.Text = emp.No + " " + emp.Name + " ( " + emp.Tel + ")";
                    isHave = true;
                }
                else
                {
                    cb.Text = emp.No + " " + emp.Name + " ( The phone number is not set )";
                    cb.Checked = false;
                    cb.Enabled = false;
                    this.Add(cb);
                }
            }
            this.AddTDEnd();
            this.AddTREnd();

            this.AddTR();
            this.Add("<TD class=BigDoc>");
            TextBox tb = new TextBox();
            tb.Attributes["width"] = "100%";
            tb.TextMode = TextBoxMode.MultiLine;
            BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            tb.Text = " Hello :\t\n You have work to do " + nd.Name + " ; \t\n" + WebUser.Name;
            tb.Columns = 50;
            tb.Rows = 7;
            this.Add(tb);
            this.AddTDEnd();
            this.AddTREnd();

            this.AddTR();
            this.Add("<TD>");
            Btn btn = new Btn();
            btn.Text = " Send phone messages ";
            btn.Click += new EventHandler(btn_Click);
            btn.Enabled = isHave;

            this.Add(btn);
            this.AddTDEnd();
            this.AddTREnd();
            this.AddTableEnd();

            this.AddFieldSetEnd();

        }

        void btn_Click(object sender, EventArgs e)
        {
            this.WinCloseWithMsg(" Sent successfully ");
        }
    }

}