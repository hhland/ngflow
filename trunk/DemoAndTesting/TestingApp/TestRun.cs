using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BP.WF;
using BP.WF.Data;
using BP.En;
using BP.Port;

namespace TestingApp
{
    public partial class TestRun : Form
    {
        public TestRun()
        {
            InitializeComponent();
        }

        private void TestRun_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///  Number of flow tests .
        /// </summary>
        public int TestNum = 2000;
        private void button1_Click(object sender, EventArgs e)
        {
            Console.Beep();
            MessageBox.Show(" Begin execution system will be the case of suspended animation , Please do not control , Wait for execution to complete .");

            // First, remove all the processes .
            Flow fl = new Flow("024");
            fl.DoDelData();

            //让zhanghaicheng  Log in .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            #region  Test creation WorkID.
            // Create the first one workid.  Let him start up .
            Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork("024", null, null, BP.Web.WebUser.No, "");

            BP.Sys.Glo.WriteLineInfo("== Began testing 024 Three nodes efficiency of the process ....");

            DateTime dtStart = DateTime.Now;
            //  Started running .
            for (int i = 0; i < this.TestNum; i++)
            {
                // Create workid.
                workid = BP.WF.Dev2Interface.Node_CreateBlankWork("024", null, null, BP.Web.WebUser.No, "");

                // Send Down .
                BP.WF.Dev2Interface.Node_SendWork("024", workid);
            }

            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;

            decimal d = 1000 / ts.Seconds;
            decimal d2 = ts.Seconds / this.TestNum;
            d2 = Math.Round(d2, 5);

            string msg = " Initiate the process " + this.TestNum + "次, Usage time :" + ts.Seconds + "  Creating an average per " + d.ToString() + "条.";
            BP.Sys.Glo.WriteLineInfo(msg);

            this.label1.Text = msg;
            #endregion  Test creation WorkID.

            this.button2_Click(null, null);
            this.button3_Click(null, null);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sender!=null)
            MessageBox.Show(" Send intermediate points begin , Begin execution system will be the case of suspended animation , Please do not control , Wait for execution to complete .");

            #region  Execution start sending node .
            Console.Beep();

            // Let Zhou Peng Login .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable("zhoupeng", "024");
            //if (dt.Rows.Count!=1000)
            //{
            //    MessageBox.Show(" You need to first perform the first node , Then in the implementation of the first 2 Nodes , Finally, the implementation of the first 3 Nodes ."+dt.Rows.Count);
            //    return;
            //}
            DateTime dtStart = DateTime.Now;
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                BP.WF.Dev2Interface.Node_SendWork("024", workid);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;

            decimal d = 1000 / ts.Seconds;
            decimal d2 = ts.Seconds / this.TestNum;

            string msg = " Initiate intermediate point " + this.TestNum + "次, Usage time :" + ts.Seconds + "  The average execution per second " + d.ToString("0.000") + "条.";
            BP.Sys.Glo.WriteLineInfo(msg);

            BP.Sys.Glo.WriteLineInfo("****************** over *************************");

            this.label2.Text = msg;
            #endregion  Execution start sending node .
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sender != null)
                MessageBox.Show(" Send intermediate points begin , Begin execution system will be the case of suspended animation , Please do not control , Wait for execution to complete .");

            #region  Execution start sending node .
            Console.Beep();

            // Let Zhou Peng Login .
            BP.WF.Dev2Interface.Port_Login("zhoupeng");
            DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable("zhoupeng", "024");
            DateTime dtStart = DateTime.Now;
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                BP.WF.Dev2Interface.Node_SendWork("024", workid);
            }
            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtStart;

            decimal d = 1000 / ts.Seconds;
            decimal d2 = ts.Seconds / this.TestNum;

            string msg = " Last node " + this.TestNum + "次, Usage time :" + ts.Seconds + "  The average execution per second " + d.ToString("0.000") + "条.";
            BP.Sys.Glo.WriteLineInfo(msg);

            BP.Sys.Glo.WriteLineInfo("****************** over *************************");

            this.label3.Text = msg;
            #endregion  Execution start sending node .
        }
    }
}
