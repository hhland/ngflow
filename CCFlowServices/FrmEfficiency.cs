using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using BP.WF;
using BP.Port;
using BP.En;
using BP.Sys;
using BP.DA;
using BP;
using BP.Web;

namespace CCFlowServices
{
    public partial class FrmEfficiency : Form
    {
        delegate void SetTextCallback(string text);
        public FrmEfficiency()
        {
            InitializeComponent();
        }
        private void SetText(string text)
        {
            if (this.TB_Text.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                try
                {
                    this.Invoke(d, new object[] { text });
                }
                catch
                {
                }
            }
            else
            {
                this.TB_Text.Text += "\r\n" + text;
                this.TB_Text.SelectionStart = this.TB_Text.TextLength;
                this.TB_Text.ScrollToCaret();
            }
        }
        public void AddLog(string msg)
        {
            Log.DefaultLogWriteLineInfo(msg);
            this.SetText(msg);
        }
        private void FrmEfficiency_Load(object sender, EventArgs e)
        {
        }
        private void Btn_RunIt_Click(object sender, EventArgs e)
        {
            RunFlow("024", "zhanghaicheng", int.Parse(this.TB_RunTimes.Text));
        }
        public void RunFlow(string fk_flow, string userNo, int runTimes)
        {
            // Log in .
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            // Deleting Data .
            Flow fl = new Flow(fk_flow);
            fl.DoDelData();
            this.AddLog("*********************  For the process :" + fl.Name + ",  Get on " + runTimes + " Single user perform a test execution efficiency .");

            // Perform a warm-up .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_StartWork(fk_flow, null, null, 0, null, 0, null);

            // Initiate execution .
            DateTime dtStart = DateTime.Now;
            this.AddLog("===  Started it " + runTimes + " Process .");
            int num = 0;
            for (int i = 0; i < runTimes; i++)
            {
                num++;
                if (num == 100)
                {
                    this.SetText(" Started :" + i + " A process ");
                    num = 0;
                }

                BP.WF.Dev2Interface.Node_StartWork(fk_flow, null, null, 0, null, 0, null);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;
            this.AddLog(" Initiated the implementation process ends , Total execution seconds :" + ts.TotalSeconds);


            // Intermediate execution .
            Log.DefaultLogWriteLineInfo("===  Intermediate point execution ");
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, fk_flow);
            num = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                num++;
                if (num == 100)
                {
                    this.SetText(" Intermediate point execution :" + i + " Data ");
                    num = 0;
                }
                Int64 workid = Int64.Parse(dt.Rows[i]["WorkID"].ToString());
                BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            }
            DateTime dtEnd2 = DateTime.Now;
            ts = dtEnd2 - dtEnd;

            this.AddLog(" Mid-point execution is completed ,  Implementation of the intermediate point : Total seconds :" + ts.TotalSeconds);

            // Implementation of end point .
            this.AddLog("===  End point execution ");
            dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable(WebUser.No, WFState.Runing, fk_flow);
            num = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                num++;
                if (num == 100)
                {
                    this.SetText(" End point execution :" + i + " Data ");
                    num = 0;
                }
                Int64 workid = Int64.Parse(dt.Rows[i]["WorkID"].ToString());
                BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid);
            }
            DateTime dtEnd3 = DateTime.Now;
            ts = dtEnd3 - dtEnd2;
            this.AddLog(" End point started to perform complete end point : Total seconds :" + ts.TotalSeconds);
        }
    }
}
