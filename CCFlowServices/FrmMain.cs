using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Security.Cryptography;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.Port;
using BP.En;
using BP.Sys;
using BP.DA;
using BP;
using BP.Web;

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

        public void Test_Insert_Model1()
        {
            DBAccess.RunSQL("DELETE CN_Area");
            int i = 0;
            DateTime dtNow = DateTime.Now;
            while (i != 100000)
            {
                i++;
                string sql = " INSERT CN_Area (No,Name) VALUES ('" + i + "' , '" + i + "')";
                DBAccess.RunSQL(sql);
            }
            DateTime dtEnd = DateTime.Now;

            TimeSpan ts = dtEnd - dtNow;
            MessageBox.Show(ts.TotalSeconds.ToString() + " - " + ts.TotalMilliseconds.ToString());
        }

        public void Test_Insert_Model2()
        {
            DBAccess.RunSQL("DELETE CN_Area");
            SqlConnection conn = new SqlConnection(SystemConfig.AppCenterDSN);
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.ConnectionString = SystemConfig.AppCenterDSN;
                conn.Open();
            }
            try
            {

                int i = 0;
                DateTime dtNow = DateTime.Now;
                while (i != 100000)
                {
                    i++;
                    string sql = " INSERT CN_Area (No,Name) VALUES ('" + i + "' , '" + i + "')";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                }
                DateTime dtEnd = DateTime.Now;

                TimeSpan ts = dtEnd - dtNow;
                MessageBox.Show(ts.TotalSeconds.ToString() + " - " + ts.TotalMilliseconds.ToString());
                conn.Close();
            }
            catch 
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }
        }

        public void Test_T_1()
        {
            DBAccess.RunSQL("DELETE CN_Area");
            int i = 0;
            DateTime dtNow = DateTime.Now;

            DBAccess.DoTransactionBegin();
            while (i != 10)
            {
                i++;
                string sql = " INSERT CN_Area (No,Name) VALUES ('" + i + "' , '" + i + "')";
                DBAccess.RunSQL(sql);
            }
            DBAccess.DoTransactionCommit();

            DateTime dtEnd = DateTime.Now;
            TimeSpan ts = dtEnd - dtNow;
            MessageBox.Show(ts.TotalSeconds.ToString() + " - " + ts.TotalMilliseconds.ToString());

        }

        public void Test_T_2()
        {
            DBAccess.RunSQL("DELETE CN_Area");
            SqlConnection conn = new SqlConnection(SystemConfig.AppCenterDSN);
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.ConnectionString = SystemConfig.AppCenterDSN;
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("BEGIN TRANSACTION", conn);
            try
            {
                cmd.ExecuteNonQuery();

                int i = 0;
                DateTime dtNow = DateTime.Now;
                while (i != 10)
                {
                    i++;
                    string sql = " INSERT CN_Area (No,Name) VALUES ('" + i + "' , '" + i + "')";

                    cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                DateTime dtEnd = DateTime.Now;
                TimeSpan ts = dtEnd - dtNow;

                cmd.CommandText = "commit transaction";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                cmd.CommandText = "rollback transaction";
                cmd.ExecuteNonQuery();
                //DBAccess.DoTransactionRollback();
                conn.Close();
            }
        }
        public FrmMain()
        {
            //Test_T_1();
            //return;
            //Test_T_2();
            //return;
            ///*  A difference in  20%  About .*/
            //this.Test_Insert_Model1();
            //this.Test_Insert_Model2();
            //return;

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
            // this.toolStripStatusLabel1.Text = " Service stopped state ...";
            this.toolStripStatusLabel1.Text = " Service suspended ";
            this.textBox1.Text = " Stop Service ...";
            this.Btn_StartStop.Text = " Start up ";
        }

        Thread thread = null;
        private void Btn_StartStop_Click(object sender, EventArgs e)
        {

            #region  Upgrade scripts .
            try
            {
                BP.DA.DBAccess.RunSQL("alter table GPM.dbo.RecordMsg alter column  SendUserID nvarchar(900)");
            }
            catch
            {
            }
            #endregion

            if (this.Btn_StartStop.Text == " Start up ")
            {
                if (this.thread == null)
                {
                    ThreadStart ts = new ThreadStart(RunIt);
                    thread = new Thread(ts);
                    thread.Start();
                    this.Btn_StartStop.Text = " Time out ";
                }
                this.HisScanSta = ScanSta.Working;
                this.SetText(" Service starts ***********" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                this.Btn_StartStop.Text = " Time out ";
                this.toolStripStatusLabel1.Text = " Service starts ";
            }
            else
            {
                this.SetText(" Service suspended ***********" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                this.HisScanSta = ScanSta.Pause;
                this.Btn_StartStop.Text = " Start up ";
                this.toolStripStatusLabel1.Text = " Service suspended ";
            }
        }
        public ScanSta HisScanSta = ScanSta.Pause;
        /// <summary>
        ///  Perform automatic startup process tasks  WF_Task 
        /// </summary>
        public void DoTask()
        {
            string sql = "SELECT * FROM WF_Task WHERE TaskSta=0 ORDER BY Starter";
            DataTable dt = null;
            try
            {
                dt = DBAccess.RunSQLReturnTable(sql);
            }
            catch
            {
                Task ta = new Task();
                ta.CheckPhysicsTable();
                dt = DBAccess.RunSQLReturnTable(sql);
            }

            if (dt.Rows.Count == 0)
                return;

            #region  Automatic start-up process 
            foreach (DataRow dr in dt.Rows)
            {
                string mypk = dr["MyPK"].ToString();
                string taskSta = dr["TaskSta"].ToString();
                string paras = dr["Paras"].ToString();
                string starter = dr["Starter"].ToString();
                string fk_flow = dr["FK_Flow"].ToString();

                string startDT = dr[TaskAttr.StartDT].ToString();
                if (string.IsNullOrEmpty(startDT) == false)
                {
                    /* If you set the launch time , To check the current time matches the time now .*/
                    if (DateTime.Now.ToString("yyyy-MM-dd HH:mm").Contains(startDT) == false)
                        continue;
                }

                Flow fl = new Flow(fk_flow);
                this.SetText(" Started (" + starter + ") Launch (" + fl.Name + ") Process .");
                try
                {
                    string fTable = "ND" + int.Parse(fl.No + "01").ToString();
                    MapData md = new MapData(fTable);
                    sql = "";
                    //   sql = "SELECT * FROM " + md.PTable + " WHERE MainPK='" + mypk + "' AND WFState=1";
                    try
                    {
                        if (DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                            continue;
                    }
                    catch
                    {
                        this.SetText(" Form table starting node :" + fTable + " Default field is not set MainPK. " + sql);
                        continue;
                    }

                    if (BP.Web.WebUser.No != starter)
                    {
                        BP.Web.WebUser.Exit();
                        BP.Port.Emp empadmin = new BP.Port.Emp(starter);
                        BP.Web.WebUser.SignInOfGener(empadmin);
                    }

                    Work wk = fl.NewWork();
                    string[] strs = paras.Split('@');
                    foreach (string str in strs)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        if (str.Contains("=") == false)
                            continue;

                        string[] kv = str.Split('=');
                        wk.SetValByKey(kv[0], kv[1]);
                    }

                    wk.SetValByKey("MainPK", mypk);
                    wk.Update();

                    WorkNode wn = new WorkNode(wk, fl.HisStartNode);
                    string msg = wn.NodeSend().ToMsgOfText();
                    msg = msg.Replace("'", "~");
                    DBAccess.RunSQL("UPDATE WF_Task SET TaskSta=1,Msg='" + msg + "' WHERE MyPK='" + mypk + "'");
                }
                catch (Exception ex)
                {
                    // If you send an error .
                    this.SetText(ex.Message);
                    string msg = ex.Message;
                    try
                    {
                        DBAccess.RunSQL("UPDATE WF_Task SET TaskSta=2,Msg='" + msg + "' WHERE MyPK='" + mypk + "'");
                    }
                    catch
                    {
                        Task TK = new Task();
                        TK.CheckPhysicsTable();
                    }
                }
            }
            #endregion  Automatic start-up process 
        }
        /// <summary>
        ///  Execution threads .
        /// </summary>
        public void RunIt()
        {
            BP.WF.Flows fls = new BP.WF.Flows();
            fls.RetrieveAll();

            HisScanSta = ScanSta.Working;
            while (true)
            {
                System.Threading.Thread.Sleep(20000);
                while (this.HisScanSta == ScanSta.Pause)
                {
                    System.Threading.Thread.Sleep(3000);
                    if (this.checkBox1.Checked)
                        Console.Beep();
                }

                this.SetText("********************************");

                this.SetText(" Scanning triggered automatically initiates the process table ......");
                this.DoTask();

                this.SetText(" Scan timing process initiated ....");
                this.DoAutuFlows(fls);

                this.SetText(" Scanning message table , Wanted to send a message outside ....");
                this.DoSendMsg();

                this.SetText(" Scanning overdue data .");
                this.DoOverDueFlow();
                //this.SetText("向CCIM In sending a message ...");
                //this.DoSendMsgOfCCIM();

                if (DateTime.Now.Hour < 18 && DateTime.Now.Hour > 8)
                {
                    /*  Can perform this scheduling work time . */
                    string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    if (now.Contains(":13") || now.Contains(":33") || now.Contains(":53"))
                    {
                        this.SetText(" Automatic node retrieval task ....");
                        this.DoAutoNode();
                    }
                }

                System.Threading.Thread.Sleep(1000);
                switch (this.toolStripStatusLabel1.Text)
                {
                    case " Service starts ":
                        this.toolStripStatusLabel1.Text = " Service starts ..";
                        break;
                    case " Service starts ..":
                        this.toolStripStatusLabel1.Text = " Service starts ........";
                        break;
                    case " Service starts ....":
                        this.toolStripStatusLabel1.Text = " Service starts .............";
                        break;
                    default:
                        this.toolStripStatusLabel1.Text = " Service starts ";
                        break;
                }
            }
        }
        /// <summary>
        ///  Automatic execution node 
        /// </summary>
        private void DoAutoNode()
        {
            string sql = "SELECT * FROM WF_GenerWorkerList WHERE FK_Node IN (SELECT NODEID FROM WF_Node WHERE (WhoExeIt=1 OR  WhoExeIt=2) AND IsPass=0 AND IsEnable=1) ORDER BY FK_Emp";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                Int64 workid = Int64.Parse(dr["WorkID"].ToString());
                int fk_node = int.Parse(dr["FK_Node"].ToString());
                string fk_emp = dr["FK_Emp"].ToString();
                string fk_flow = dr["FK_Flow"].ToString();

                try
                {
                    if (WebUser.No != fk_emp)
                    {
                        WebUser.Exit();
                        Emp emp = new Emp(fk_emp);
                        WebUser.SignInOfGener(emp);
                    }
                    string msg = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workid).ToMsgOfText();
                    this.SetText("@ Deal with :" + WebUser.No + ",WorkID=" + workid + ", Correctly handle :" + msg);
                }
                catch (Exception ex)
                {
                    this.SetText("@ Deal with :" + WebUser.No + ",WorkID=" + workid + ", Job Information :" + ex.Message);
                }
            }
        }
        /// <summary>
        ///  Send a message 
        /// </summary>
        private void DoSendMsg()
        {
            int idx = 0;
            #region  Send a message 
            SMSs sms = new SMSs();
            BP.En.QueryObject qo = new BP.En.QueryObject(sms);
            sms.Retrieve(SMSAttr.EmaiSta, (int)MsgSta.UnRun);
            foreach (SMS sm in sms)
            {
                if (this.HisScanSta == ScanSta.Stop)
                    return;

                while (this.HisScanSta == ScanSta.Pause)
                {
                    if (this.HisScanSta == ScanSta.Stop)
                        return;

                    System.Threading.Thread.Sleep(3000);

                    if (this.checkBox1.Checked)
                        Console.Beep();
                }

                if (sm.Email.Length == 0)
                {
                    sm.HisEmaiSta = MsgSta.RunOK;
                    sm.Update();
                    continue;
                }
                try
                {
                    this.SetText("@ Carried out :send email: " + sm.Email);
                    this.SendMail(sm);

                    idx++;
                    this.SetText(" Completed  , 第:" + idx + " 个.");
                    this.SetText("--------------------------------");

                    if (this.checkBox1.Checked)
                        Console.Beep();
                }
                catch (Exception ex)
                {
                    this.SetText("@ Error :" + ex.Message);
                }
            }
            #endregion  Send a message 
        }
        /// <summary>
        ///  Overdue Process 
        /// </summary>
        private void DoOverDueFlow()
        {
            BP.DA.Log.DefaultLogWriteLine(LogType.Info, " Late start scanning process data .");
            DataTable generTab = null;
            try
            {
                string sql =
                    string.Format(
                        "select FK_Flow,WorkID,Title,FK_Node,SDTOfNode,Starter from WF_GenerWorkFlow where SDTOfNode<='{0}' and WFState in({1})",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (int)WFState.Runing);
                generTab = DBAccess.RunSQLReturnTable(sql);
            }
            catch (Exception ex)
            {
                this.SetText(" Scan data anomalies overdue :" + ex.Message);
                BP.DA.Log.DefaultLogWriteLine(LogType.Error, " Scan data anomalies overdue :" + ex.Message);
            }

            string msg = "";
            foreach (DataRow row in generTab.Rows)
            {
                string fk_flow = row["FK_Flow"] + "";
                string fk_node = row["FK_Node"] + "";
                long workid = long.Parse(row["WorkID"] + "");
                string title = row["Title"] + "";
                string compleateTime = row["SDTOfNode"] + "";
                string starter = row["Starter"] + "";
                try
                {
                    Node node = new Node(int.Parse(fk_node));
                    if (node.IsStartNode)
                        continue;

                    Flow flow = new Flow(fk_flow);

                    string doOutTime = node.GetValByKey("DoOutTime") + "";
                    switch (node.HisOutTimeDeal)
                    {
                        case OutTimeDeal.None:
                            msg = " Process  '"+flow.Name+"': Title  '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                  "' Overtime rules for ' Does not deal with '";
                            SetText(msg);
                            BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);
                            break;
                        case OutTimeDeal.AutoJumpToSpecNode:
                            if (string.IsNullOrEmpty(doOutTime))
                            {
                                msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                      "' Overtime rules for ' Automatically jump to the corresponding node ', No configuration processing content .";
                                SetText(msg);
                                BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                            }
                            else
                            {
                                try
                                {
                                    string[] jumps = doOutTime.Split(',');
                                    if (jumps.Count() > 2)
                                    {
                                        msg = " Process  '"+flow.Name+"', Title :'" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                              "' Overtime rules for ' Automatically jump to the corresponding node ', Incorrect configuration of content , Format should be :'Node,EmpNo'.";
                                        SetText(msg);
                                        BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);
                                    }
                                    else
                                    {
                                        string jumpNode = jumps[0];
                                        string jumpEmp = jumps[1];

                                        Emp emp = new Emp(jumpEmp);

                                        if (string.IsNullOrEmpty(emp.No))
                                        {
                                            msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                                  "' Overtime rules for ' Automatically jump ', The system does not numbered '" + jumpEmp + "' Staff .";
                                            SetText(msg);
                                            BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);
                                            return;
                                        }

                                        Node jumpToNode = new Node(jumpNode);
                                        if (string.IsNullOrEmpty(jumpToNode.Name))
                                        {
                                            msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                                  "' Overtime rules for ' Automatically jump ', The system does not numbered '" + jumpNode + "' Staff .";
                                            SetText(msg);
                                            BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                                            return;
                                        }
                                        // Performing transmission .
                                        //string info =  BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID, null, null, nd.NodeID, emp.No).ToMsgOfText();
                                        string jumpInfo = BP.WF.Dev2Interface.Flow_Schedule(workid, jumpToNode.NodeID,
                                            emp.No);
                                        msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                              "' Overtime rules for ' Automatically jump '," + jumpInfo;
                                        SetText(msg);
                                        BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                          "' Overtime rules for ' Automatically jump ', Jump abnormal :" + ex.Message;
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);

                                }
                            }

                            break;
                        case OutTimeDeal.AutoShiftToSpecUser:
                            if (string.IsNullOrEmpty(doOutTime))
                            {
                                SetText(" Process  '"+flow.Name+"', Title :  '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                        "' Overtime rules for ' Handed over to the designated person ', No configuration processing content .");
                            }
                            else
                            {
                                Emp emp = new Emp(doOutTime);
                                if (string.IsNullOrEmpty(emp.No))
                                {
                                    msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                          "' Overtime rules for ' Handed over to the designated person ', The system does not numbered '" + doOutTime + "' Staff .";
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                                }

                                else
                                {
                                    try
                                    {
                                        BP.WF.Dev2Interface.Node_Shift(fk_flow,int.Parse( fk_node), workid,0, emp.No,
                                            " Process node overdue , The system automatically transfer ");

                                        msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                              "' Overtime rules for ' Handed over to the designated person ', Have been automatically transferred to '" + emp.Name + ".";
                                        SetText(msg);
                                        BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                                    }
                                    catch (Exception ex)
                                    {
                                        msg = " Process  '"+flow.Name+"' , Title :'" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                              "' Overtime rules for ' Handed over to the designated person ', Abnormal transfer :" + ex.Message;
                                        SetText(msg);
                                        BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);
                                    }
                                }
                            }
                            break;
                        case OutTimeDeal.AutoTurntoNextStep:
                            try
                            {
                               GenerWorkerList workerList = new GenerWorkerList();
                                workerList.RetrieveByAttrAnd(GenerWorkerListAttr.WorkID, workid,
                                    GenerWorkFlowAttr.FK_Node, fk_node);

                                WebUser.SignInOfGener(workerList.HisEmp);

                                WorkNode firstwn = new WorkNode(workid, int.Parse(fk_node));
                                string sendIfo = firstwn.NodeSend().ToMsgOfText();
                                msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                      "' Overtime rules for ' Automatically sent to the next node ', Send a message to :" + sendIfo;
                                SetText(msg);
                                BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                            }
                            catch (Exception ex)
                            {
                                msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                      "' Overtime rules for ' Automatically sent to the next node ', Send abnormal :" + ex.Message;
                                SetText(msg);
                                BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);

                            }
                            break;
                        case OutTimeDeal.DeleteFlow:
                            string info = BP.WF.Dev2Interface.Flow_DoDeleteDraft(fk_flow, workid, true);
                            msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                  "' Overtime rules for ' Delete Process '," + info;
                            SetText(msg);
                            BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                            break;
                        case OutTimeDeal.RunSQL:
                            if (string.IsNullOrEmpty(doOutTime))
                            {
                                SetText(" Process    '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                        "' Overtime rules for ' Carried out SQL', No configuration processing content .");
                            }
                            else
                            {

                                try
                                {
                                    // Replacement string 
                                    doOutTime.Replace("@OID", workid + "");
                                    doOutTime.Replace("@FK_Flow", fk_flow);
                                    doOutTime.Replace("@FK_Node", fk_node);
                                    doOutTime.Replace("@Starter", starter);
                                    if (doOutTime.Contains("@"))
                                    {
                                        msg = " Process  '"+flow.Name+"', Title :  '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                              "' Overtime rules for ' Carried out SQL'. There are not replaced SQL Variable .";
                                        SetText(msg);
                                        BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);

                                        return;
                                    }
                                    DBAccess.RunSQLReturnCOUNT(doOutTime);
                                }
                                catch (Exception ex)
                                {
                                    msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                          "' Overtime rules for ' Carried out SQL'. Run SQL Abnormal :" + ex.Message;
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);

                                }
                            }
                            break;
                        case OutTimeDeal.SendMsgToSpecUser:
                            try
                            {
                                if (string.IsNullOrEmpty(doOutTime))
                                {
                                    msg = " Process   '" + flow.Name + "', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                     "' Overtime rules for ' Designated person to send a message '. Not specified sender " ;
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);
                                    return;
                                }
                                Emp emp = new Emp(doOutTime);
                                if (string.IsNullOrEmpty(emp.No))
                                {
                                    msg = " Process   '" + flow.Name + "', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                    "' Overtime rules for ' Designated person to send a message '. Did not find the number of '"+doOutTime+"' Staff .";
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);
                                    return;
                                }

                                bool boo = BP.WF.Dev2Interface.WriteToSMS(emp.No,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), " The system sends a message overdue ", " Your processes :'"+title+"' Completion time should be '"+compleateTime+"', Process overdue , Please timely processing !"," System Messages ");
                                if (boo)
                                    msg = "'"+title+"' Overdue message has been sent to the :'"+emp.Name+"'";
                                else
                                    msg = "'"+title+"' Overdue messages sent unsuccessfully , Send artificial :'"+emp.Name+"'";

                                SetText(msg);
                                BP.DA.Log.DefaultLogWriteLine(LogType.Info, msg);
                            }
                            catch (Exception ex)
                            {
                                    msg = " Process   '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                          "' Overtime rules for ' Carried out SQL'. Run SQL Abnormal :" + ex.Message;
                                    SetText(msg);
                                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);
                            }
                            break;
                        default:
                            msg = " Process  '"+flow.Name+"', Title : '" + title + "' Should be completed by the time of '" + compleateTime + "', The current node '" + node.Name +
                                  "' Supermarkets do not find the appropriate processing rules .";
                            SetText(msg);
                            BP.DA.Log.DefaultLogWriteLine(LogType.Error, msg);

                            break;
                    }
                }
                catch (Exception ex)
                {
                    SetText(" Abnormal flow overdue :" + ex.Message);
                    BP.DA.Log.DefaultLogWriteLine(LogType.Error, ex.ToString());

                }
            }
            BP.DA.Log.DefaultLogWriteLine(LogType.Info, " End scanning overdue process data .");
        }
        /// <summary>
        ///  Regular tasks 
        /// </summary>
        /// <param name="fls"></param>
        private void DoAutuFlows(BP.WF.Flows fls)
        {
            #region  Automatic start-up process 
            foreach (BP.WF.Flow fl in fls)
            {
                if (  fl.HisFlowRunWay == BP.WF.FlowRunWay.HandWork)
                    continue;

                if (DateTime.Now.ToString("HH:mm") == fl.Tag)
                    continue;

                if (fl.RunObj == null || fl.RunObj == "")
                {
                    string msg = " You set the auto-run process error , There is no set process content , Process ID :" + fl.No + ", Process Name :" + fl.Name;
                    this.SetText(msg);
                    continue;
                }

                #region  Determining whether the current time can run it .
                string nowStr = DateTime.Now.ToString("yyyy-MM-dd,HH:mm");
                string[] strs = fl.RunObj.Split('@'); // Time to break open string .
                bool IsCanRun = false;
                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str))
                        continue;
                    if (nowStr.Contains(str))
                        IsCanRun = true;
                }

                if (IsCanRun == false)
                    continue;

                //  Set time .
                fl.Tag = DateTime.Now.ToString("HH:mm");
                #endregion  Determining whether the current time can run it .

                //  Users enter this .
                switch (fl.HisFlowRunWay)
                {
                    case BP.WF.FlowRunWay.SpecEmp: // Designated personnel to run on time .
                        string RunObj = fl.RunObj;
                        string fk_emp = RunObj.Substring(0, RunObj.IndexOf('@'));

                        BP.Port.Emp emp = new BP.Port.Emp();
                        emp.No = fk_emp;
                        if (emp.RetrieveFromDBSources() == 0)
                        {
                            this.SetText(" Automatically start the process error : Sponsor (" + fk_emp + ") Does not exist .");
                            continue;
                        }

                        try
                        {
                            //让 userNo  Log in .
                            BP.WF.Dev2Interface.Port_Login(emp.No);

                            // Create a blank ,  Initiating the start node .
                            Int64 workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null,
                                 WebUser.No, null, 0, null, 0, null);

                            // Performing transmission .
                            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);
                            //string info_send= BP.WF.Dev2Interface.Node_StartWork(fl.No,);
                            this.SetText(" Process :" + fl.No + fl.Name + " The timing of the task \t\n -------------- \t\n" + objs.ToMsgOfText());
                        }
                        catch (Exception ex)
                        {
                            this.SetText(" Process :" + fl.No + fl.Name + " Automatically initiates error :\t\n -------------- \t\n" + ex.Message);
                        }
                        continue;
                    case BP.WF.FlowRunWay.DataModel: // Driven by the data collection mode execution .
                        this.SetText("@ Performs a data-driven process scheduling :" + fl.Name);
                        this.DTS_Flow(fl);
                        continue;
                    default:
                        break;
                }
            }
            if (BP.Web.WebUser.No != "admin")
            {
                BP.Port.Emp empadmin = new BP.Port.Emp("admin");
                BP.Web.WebUser.SignInOfGener(empadmin);
            }
            #endregion  Send a message 
        }
        public void DTS_Flow(BP.WF.Flow fl)
        {
            #region  Read data .
            BP.Sys.MapExt me = new MapExt();
            me.MyPK = "ND" + int.Parse(fl.No) + "01" + "_" + MapExtXmlList.StartFlow;
            int i = me.RetrieveFromDBSources();
            if (i == 0)
            {
                BP.DA.Log.DefaultLogWriteLineError(" No for the process (" + fl.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .");
                return;
            }
            if (string.IsNullOrEmpty(me.Tag))
            {
                BP.DA.Log.DefaultLogWriteLineError(" No for the process (" + fl.Name + ") The start node is set to initiate data , Please refer to the instructions to solve .");
                return;
            }

            //  Data obtained from the table .
            DataSet ds = new DataSet();
            string[] dtlSQLs = me.Tag1.Split('*');
            foreach (string sql in dtlSQLs)
            {
                if (string.IsNullOrEmpty(sql))
                    continue;

                string[] tempStrs = sql.Split('=');
                string dtlName = tempStrs[0];
                DataTable dtlTable = BP.DA.DBAccess.RunSQLReturnTable(sql.Replace(dtlName + "=", ""));
                dtlTable.TableName = dtlName;
                ds.Tables.Add(dtlTable);
            }
            #endregion  Read data .

            #region  Check the data source is correct .
            string errMsg = "";
            //  Get the main table data .
            DataTable dtMain = BP.DA.DBAccess.RunSQLReturnTable(me.Tag);
            if (dtMain.Rows.Count == 0)
            {
                BP.DA.Log.DefaultLogWriteLineError(" Process (" + fl.Name + ") At this time no task .");
                this.SetText(" Process (" + fl.Name + ") At this time no task .");
                return;
            }

            this.SetText("@ Queried (" + dtMain.Rows.Count + ") Article task .");

            if (dtMain.Columns.Contains("Starter") == false)
                errMsg += "@ The main table with a value of no Starter列.";

            if (dtMain.Columns.Contains("MainPK") == false)
                errMsg += "@ The main table with a value of no MainPK列.";

            if (errMsg.Length > 2)
            {
                this.SetText(errMsg);
                BP.DA.Log.DefaultLogWriteLineError(" Process (" + fl.Name + ") The start node is set to initiate data , Incomplete ." + errMsg);
                return;
            }
            #endregion  Check the data source is correct .

            #region  Processing launched .
            string nodeTable = "ND" + int.Parse(fl.No) + "01";
            int idx = 0;
            foreach (DataRow dr in dtMain.Rows)
            {
                idx++;

                string mainPK = dr["MainPK"].ToString();
                string sql = "SELECT OID FROM " + nodeTable + " WHERE MainPK='" + mainPK + "'";
                if (DBAccess.RunSQLReturnTable(sql).Rows.Count != 0)
                {
                    this.SetText("@" + fl.Name + ",第" + idx + "条, This task has been completed before .");
                    continue; /* Description been scheduled over */
                }

                string starter = dr["Starter"].ToString();
                if (WebUser.No != starter)
                {
                    BP.Web.WebUser.Exit();
                    BP.Port.Emp emp = new BP.Port.Emp();
                    emp.No = starter;
                    if (emp.RetrieveFromDBSources() == 0)
                    {
                        this.SetText("@" + fl.Name + ",第" + idx + "条, Sponsored personnel set :" + emp.No + " Does not exist .");
                        BP.DA.Log.DefaultLogWriteLineInfo("@ Data-driven approach to initiate the process (" + fl.Name + ") Sponsored personnel set :" + emp.No + " Does not exist .");
                        continue;
                    }
                    WebUser.SignInOfGener(emp);
                }

                #region   To value .
                //System.Collections.Hashtable ht = new Hashtable();

                Work wk = fl.NewWork();

                string err = "";
                #region  Check the spelling of the user sql Correct ?
                foreach (DataColumn dc in dtMain.Columns)
                {
                    string f = dc.ColumnName.ToLower();
                    switch (f)
                    {
                        case "starter":
                        case "mainpk":
                        case "refmainpk":
                        case "tonode":
                            break;
                        default:
                            bool isHave = false;
                            foreach (Attr attr in wk.EnMap.Attrs)
                            {
                                if (attr.Key.ToLower() == f)
                                {
                                    isHave = true;
                                    break;
                                }
                            }
                            if (isHave == false)
                            {
                                err += " " + f + " ";
                            }
                            break;
                    }
                }
                if (string.IsNullOrEmpty(err) == false)
                    throw new Exception(" Field you set :" + err + " There is no start node form , Set sql:" + me.Tag);

                #endregion  Check the spelling of the user sql Correct ?

                foreach (DataColumn dc in dtMain.Columns)
                    wk.SetValByKey(dc.ColumnName, dr[dc.ColumnName].ToString());

                if (ds.Tables.Count != 0)
                {
                    // MapData md = new MapData(nodeTable);
                    MapDtls dtls = new MapDtls(nodeTable);
                    foreach (MapDtl dtl in dtls)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (dt.TableName != dtl.No)
                                continue;

                            // Delete the original data .
                            GEDtl dtlEn = dtl.HisGEDtl;
                            dtlEn.Delete(GEDtlAttr.RefPK, wk.OID.ToString());

                            //  Executing data insertion .
                            foreach (DataRow drDtl in dt.Rows)
                            {
                                if (drDtl["RefMainPK"].ToString() != mainPK)
                                    continue;

                                dtlEn = dtl.HisGEDtl;
                                foreach (DataColumn dc in dt.Columns)
                                    dtlEn.SetValByKey(dc.ColumnName, drDtl[dc.ColumnName].ToString());

                                dtlEn.RefPK = wk.OID.ToString();
                                dtlEn.OID = 0;
                                dtlEn.Insert();
                            }
                        }
                    }
                }
                #endregion   To value .


                int toNodeID = 0;
                try
                {
                    toNodeID = int.Parse(dr["ToNode"].ToString());
                }
                catch
                {
                    /* Possible 4.5 There are no previous versions tonode This convention .*/
                }

                //  Send information processing .
                //  Node nd =new Node();
                string msg = "";
                try
                {
                    if (toNodeID == 0)
                    {
                        WorkNode wn = new WorkNode(wk, fl.HisStartNode);
                        msg = wn.NodeSend().ToMsgOfText();
                    }

                    if (toNodeID == fl.StartNodeID)
                    {
                        /*  Let it stay on the start node after launch , Is to create a to-do for a start node .*/
                        Int64 workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null);
                        if (workID != wk.OID)
                            throw new Exception("@ Exception Information : Should not be inconsistent workid.");
                        else
                            wk.Update();
                        msg = " Already (" + WebUser.No + ")  Created to work node . ";
                    }

                    BP.DA.Log.DefaultLogWriteLineInfo(msg);
                    this.SetText("@" + fl.Name + ",第" + idx + "条, Sponsored staff :" + WebUser.No + "-" + WebUser.Name + " Completed .\r\n" + msg);
                }
                catch (Exception ex)
                {
                    this.SetText("@" + fl.Name + ",第" + idx + "条, Sponsored staff :" + WebUser.No + "-" + WebUser.Name + " An error occurred while initiating .\r\n" + ex.Message);
                    BP.DA.Log.DefaultLogWriteLineWarning(ex.Message);
                }
            }
            #endregion  Processing launched .
        }
        /// <summary>
        ///  Send e-mail .
        /// </summary>
        /// <param name="sms"></param>
        public void SendMail(SMS sms)
        {
            #region 向ccim Write information .
            // If the  ccim  Write messages .
            if (this.CB_IsWriteToCCIM.Checked == true)
            {
                /* If you are selected , Is to show ccim Inside information is written . */
                try
                {
                    Glo.SendMessage_WinSoft(sms);

                    // Glo.SendMessage_CCIM(sms.MyPK, DateTime.Now.ToString(), sms.Title + "\t\n" + sms.DocOfEmail, sms.Email);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, " Error ");
                    return;
                }
            }
            #endregion 向ccim Write information .

            #region  Send e-mail .
            if (string.IsNullOrEmpty(sms.Email))
            {
                BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(sms.SendTo);
                sms.Email = emp.Email;
            }

            System.Net.Mail.MailMessage myEmail = new System.Net.Mail.MailMessage();
            myEmail.From = new MailAddress("ccflow.cn@gmail.com", "ccflow", System.Text.Encoding.UTF8);

            myEmail.To.Add(sms.Email);
            myEmail.Subject = sms.Title;
            myEmail.SubjectEncoding = System.Text.Encoding.UTF8;// Mail header encoding 

            myEmail.Body = sms.DocOfEmail;
            myEmail.BodyEncoding = System.Text.Encoding.UTF8;// Mail content encoding 
            myEmail.IsBodyHtml = true;// Is HTML Mail 

            myEmail.Priority = MailPriority.High;// Priority Mail 

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(SystemConfig.GetValByKey("SendEmailAddress", "ccflow.cn@gmail.com"),
                SystemConfig.GetValByKey("SendEmailPass", "ccflow123"));
            // Said write your email and password 
            client.Port = SystemConfig.GetValByKeyInt("SendEmailPort", 587); // Ports Used 
            client.Host = SystemConfig.GetValByKey("SendEmailHost", "smtp.gmail.com");
            client.EnableSsl = SystemConfig.GetValByKeyBoolen("SendEmailEnableSsl", true);

            object userState = myEmail;
            try
            {
                client.SendAsync(myEmail, userState);
                sms.HisEmaiSta = MsgSta.RunOK;
                sms.Update();
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw ex;
            }
            #endregion  Send e-mail .

        }
        private void SetText(string text)
        {
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
            if (thread != null)
            {
                this.HisScanSta = ScanSta.Stop;
                thread.Abort();
            }
            this.Close();
        }

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            this.Hide();
            this.notifyIcon1.Visible = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BP.WF.Flow fl = new BP.WF.Flow("040");
            BP.WF.Dev2Interface.DTS_AutoStarterFlow(fl);
        }

        private void Btn_ToolBox_Click(object sender, EventArgs e)
        {
            CCFlowServices.ToolBox tb = new CCFlowServices.ToolBox();
            tb.Show();
        }

        private void CB_IsWriteToCCIM_CheckedChanged(object sender, EventArgs e)
        {

        }


    }
}
