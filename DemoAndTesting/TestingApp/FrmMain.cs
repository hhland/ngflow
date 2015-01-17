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
using System.Collections;

namespace SMSServices
{
    public enum ScanSta
    {
        Working,
        Pause,
        Stop
    }
    public partial class FrmMain : Form
    {
        delegate void SetTextCallback(string text);
        public FrmMain()
        {
            InitializeComponent();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
            this.notifyIcon1.Visible = true;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = " Process Engine unit testing ";
            this.textBox1.Text = "";

            // let admin login.
            BP.WF.Dev2Interface.Port_Login("admin");

            #region  Execution supplement sql,  Let the foreign key field length are set to 100.
            //// Load department data .
            //string sqlscript = "";
            //sqlscript = SystemConfig.PathOfData + "\\Install\\SQLScript\\Port_Inc_CH.sql";
            //BP.DA.DBAccess.RunSQLScript(sqlscript);

            BP.WF.Dev2Interface.Port_Login("admin");
            #endregion  Execution supplement sql,  Let the foreign key field length are set to 100.
             
            //BP.WF.GenerWorkerList gwl = new GenerWorkerList();
            //gwl.CheckPhysicsTable();

            //BP.WF.GenerWorkFlow wf = new GenerWorkFlow();
            //wf.CheckPhysicsTable();
        }
        public ScanSta HisScanSta = ScanSta.Pause;
        /// <summary>
        ///  Execution threads .
        /// </summary>
        public void RunIt(BP.CT.EditState sta)
        {
            this.toolStripStatusLabel1.Text = " Being ready to perform （" + sta.ToString() + "） Test ..";
            string errorInfo = "";
            // Begin execution information .
            ArrayList al = ClassFactory.GetObjects("BP.CT.TestBase");

            int i = 0;
            int numOk = 0;
            int numErr = 0;
            string errCls = "";
            foreach (BP.CT.TestBase en in al)
            {
                if (sta != en.EditState)
                    continue;
                i++;
                this.SetText("=== No:" + i.ToString().PadLeft(3, '0'));
                this.SetText(" Started :" + en.Title);
                this.SetText("类:" + en.ToString());
                this.SetText(" Test content :" + en.DescIt);

                if (sta == BP.CT.EditState.Editing)
                {
                    en.Do();
                    this.SetText(" Successfully . \t\n");
                    numOk++;
                    this.toolStripStatusLabel1.Text = " Carried out :" + i + "个, Success :" + numOk + "个, Failure :" + numErr + "个";
                    continue;
                }

                try
                {
                    en.Do();
                    this.SetText(" Successfully . \t\n");
                    numOk++;
                    this.toolStripStatusLabel1.Text = " Carried out :" + i + "个, Success :" + numOk + "个, Failure :" + numErr + "个";
                }
                catch (Exception ex)
                {
                    this.SetText("Error:" + ex.Message);
                    numErr++;
                    errorInfo += "=== No:" + i.ToString().PadLeft(3, '0') + " unpass.";
                    errorInfo += "\t\nEnName:" + en.ToString();
                    errorInfo += "\t\nTitle:" + en.Title;
                    errorInfo += "\t\nError:" + ex.Message;

                    errCls += "\t\n Entity:" + en.ToString() + "  Error Messages " + ex.Message;
                    this.toolStripStatusLabel1.Text = " Carried out :" + i + "个, Success :" + numOk + "个, Failure :" + numErr + "个";
                }

            } // End loop .

            this.toolStripStatusLabel1.Text = " Carried out :" + i + "个, Success :" + numOk + "个, Failure :" + numErr + "个";

            this.SetText("**** OVER ***");
            this.SetText(" The results :" + this.toolStripStatusLabel1.Text);

            if (string.IsNullOrEmpty(errCls) == false)
                this.SetText(" The results :" + this.toolStripStatusLabel1.Text + "  Did not pass the information :" + errCls);
            else
                this.SetText(" The results :" + this.toolStripStatusLabel1.Text);

            // Write information .
            if (string.IsNullOrEmpty(errorInfo))
            {
                BP.DA.DataType.WriteFile("C:\\CCFlowCellTestLog.txt", errorInfo);
            }
        }
        /// <summary>
        ///  Set content 
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            // Write information .
            BP.DA.Log.DefaultLogWriteLineInfo(text);
            if (this.textBox1.InvokeRequired)
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
                this.textBox1.Text += "\r\n" + text;
                this.textBox1.SelectionStart = this.textBox1.TextLength;
                this.textBox1.ScrollToCaret();
            }
        }
        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_Editing_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            this.textBox1.Text = " Are preparing for the implementation of the unit test ......";
            this.toolStripStatusLabel1.Text = " Are preparing for the implementation of the unit test ......";
            btn.Enabled = false;
            this.RunIt(BP.CT.EditState.Editing);
            btn.Enabled = true;
        }

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            this.textBox1.Text = " Are preparing for the implementation of the unit test ......";
            this.toolStripStatusLabel1.Text = " Are preparing for the implementation of the unit test ......";
            btn.Enabled = false;
            this.RunIt(BP.CT.EditState.Passed);
            btn.Enabled = true;
        }

        private void Btn_All_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            this.textBox1.Text = " Are preparing for the implementation of the unit test ......";
            this.toolStripStatusLabel1.Text = " Are preparing for the implementation of the unit test ......";
            btn.Enabled = false;
            this.RunIt(BP.CT.EditState.UnOK );
            btn.Enabled = true;
        }

        private void Btn_RunOne_Click(object sender, EventArgs e)
        {
            TestingApp.FrmRunOne frm = new TestingApp.FrmRunOne();
            frm.Show();
        }

        private void Btn_XiaoLV_Click(object sender, EventArgs e)
        {
            TestingApp.TestRun tr = new TestingApp.TestRun();
            tr.ShowDialog();
        }
    
    }
}
