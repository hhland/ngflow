using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.Admin
{
    public partial class SimulationRun : System.Web.UI.Page
    {
        #region  Property .
        /// <summary>
        ///  Staff ID
        /// </summary>
        public string IDs
        {
            get
            {
                return this.Request.QueryString["IDs"];
            }
        }
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
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] emps = this.IDs.Split(',');
            this.Pub1.AddTable();
            this.Pub1.AddCaption(" Process simulation run automatically ");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("IDX");
            this.Pub1.AddTDTitle(" Analog Executives ");
            this.Pub1.AddTDTitle(" Parameters （ Can be empty )");
            this.Pub1.AddTREnd();

            int idx = 0;
            foreach (string empStr in emps)
            {
                if (string.IsNullOrEmpty(empStr))
                    continue;

                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);

                BP.Port.Emp emp = new BP.Port.Emp(empStr);
                CheckBox cb = new CheckBox();
                cb.Text = emp.No + "-" + emp.Name;
                cb.Checked = true;
                cb.ID = "CB_" + emp.No;
                this.Pub1.AddTD(cb);

                TextBox tb = new TextBox();
                tb.ID = "TB_" + emp.No;
                tb.Width = 300;
                this.Pub1.AddTD(tb);
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();

            this.Pub1.Add(" Format : @Para1=Value1@Para2=Value2） Such as :@QingJiaTianShu=20");

            Button btn = new Button();
            btn.ID = "ss";
            btn.Text = " Carried out ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            string html = "";
            foreach (Control ctl in this.Pub1.Controls)
            {
                TextBox tb = ctl as TextBox;
                if (tb == null)
                    continue;

                string empid = ctl.ID.Replace("TB_", "");
                string paras = tb.Text;

                html += "<fieldset>";
                html += "<legend>" + empid + "</legend>";
                html += BP.WF.Glo.Simulation_RunOne(this.FK_Flow, empid, paras);
                html += "</fieldset>";
            }

            // Output execution results .
            this.Response.Write(html);

            // No .
            BP.WF.Dev2Interface.Port_Login("admin");
        }
    }
}