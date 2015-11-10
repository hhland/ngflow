using System.Text;
using System.Security.Cryptography;
using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Windows.Forms;
using BP.WF;
using BP.Sys;
using BP;
using BP.En;
using System.Data.Sql;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace SMSServices
{
    public class Glo
    {
        #region 向CCIM Send a message 
        /// <summary>
        ///  Wansai  im  Interface .
        /// </summary>
        /// <param name="sms"></param>
        public static void SendMessage_WinSoft(SMS sms)
        {

        }
        /// <summary>
        ///  Generated news ,userid Write messages to ensure uniqueness ,receiveid The real recipient 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="now"></param>
        /// <param name="msg"></param>
        /// <param name="receiveid"></param>
        public static void SendMessage_CCIM(string userid, string now, string msg, string receiveid)
        {
            // Save the system notification message 
            StringBuilder strHql1 = new StringBuilder();
            // Encryption process 
            msg = CCFlowServices.SecurityDES.Encrypt(msg);
            strHql1.Append("Insert into " + BP.WF.Glo.CCIMDBName + ".RecordMsg ([sendID],[msgDateTime],[msgContent],[ImageInfo],[fontName],[fontSize],[fontBold],");
            strHql1.Append("[fontColor],[InfoClass],[GroupID],[SendUserID]) values(");
            strHql1.Append("'SYSTEM',");
            strHql1.Append("'").Append(now).Append("',");
            strHql1.Append("'").Append(msg).Append("',");
            strHql1.Append("'',");
            strHql1.Append("' Times New Roman ',");
            strHql1.Append("10,");
            strHql1.Append("0,");
            strHql1.Append("-16777216,");
            strHql1.Append("15,");
            strHql1.Append("-1,");
            strHql1.Append("'").Append(userid).Append("')");

            BP.DA.DBAccess.RunSQL(strHql1.ToString());

            // Remove just saved msgID
            string msgID;
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable("SELECT MsgID FROM " + BP.WF.Glo.CCIMDBName + ".RecordMsg WHERE sendID='SYSTEM' AND msgDateTime='" + now + "' AND SendUserID='" + userid + "'");

            foreach (DataRow dr in dt.Rows)
            {
                msgID = dr["MsgID"].ToString();

                // Save messaging object 
                StringBuilder strHql2 = new StringBuilder();
                strHql2.Append("Insert into " + BP.WF.Glo.CCIMDBName + ".RecordMsgUser ([MsgId],[ReceiveID]) values(");

                strHql2.Append(msgID).Append(",");
                strHql2.Append("'").Append(receiveid).Append("')");

                BP.DA.DBAccess.RunSQL(strHql2.ToString());
            }
        }
     
        #endregion

        public static bool IsExitProcess(string name)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pro in processes)
            {
                if (pro.ProcessName + ".exe" == name)
                    return true;
            }
            return false;
        }
        public static bool KillProcess(string name)
        {
            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process pro in processes)
            {
                if (pro.ProcessName == name)
                {
                    pro.Kill();
                    return true;
                }
            }
            return false;
        }
        public static string PathOfVisualFlow
        {
            get
            {
                //return @"D:\ccflow\VisualFlow";
                string path= Application.StartupPath + @"\.\..\..\..\CCFlow\";
                if (System.IO.Directory.Exists(path) == false)
                    throw new Exception("@ Not found web Applications folder , This program needs to read web.config File to run .");
                return path;
            }
        }
        public static void LoadConfigByFile()
        {
            //BP.WF.Glo.IntallPath = PathOfVisualFlow;
            BP.Sys.SystemConfig.IsBSsystem_Test = false;
            BP.Sys.SystemConfig.IsBSsystem = false;
            SystemConfig.IsBSsystem = false;

            string path = PathOfVisualFlow + "\\web.config"; // If this file is loaded it .
            if (System.IO.File.Exists(path) == false)
            {
                MessageBox.Show(" Configuration file is not found :" + path);
                return;
                //throw new Exception(" Configuration file is not found :" + path);
            }

            ClassFactory.LoadConfig(path);
            try
            {
                try
                {
                    BP.Port.Emp em = new BP.Port.Emp("admin");
                }
                catch
                {
                    BP.Port.Emp em = new BP.Port.Emp("admin");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Abnormal connection to the database :" + ex.Message);
                return;
            }

            SystemConfig.IsBSsystem_Test = false;
            SystemConfig.IsBSsystem = false;
            SystemConfig.IsBSsystem = false;
            //   BP.Win.WF.Global.FlowImagePath = BP.WF.Global.PathOfVisualFlow + "\\Data\\FlowDesc\\";
            BP.Web.WebUser.SysLang = "CH";

            BP.Sys.SystemConfig.IsBSsystem_Test = false;
            BP.Sys.SystemConfig.IsBSsystem = false;
            SystemConfig.IsBSsystem = false;
        }
    }
}
