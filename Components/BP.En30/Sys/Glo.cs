using System;
using System.Collections.Generic;
using System.Text;
using BP.Sys;
using BP.DA;

namespace BP.Sys
{
    /// <summary>
    ///  Public static method .
    /// </summary>
    public class Glo
    {
        #region  Written to the system log ( Written document :\DataUser\Log\*.*)
        /// <summary>
        ///  Write a message 
        /// </summary>
        /// <param name="msg"> News </param>
        public static void WriteLineInfo(string msg)
        {
            BP.DA.Log.DefaultLogWriteLineInfo(msg);
        }
        /// <summary>
        ///  Write a warning 
        /// </summary>
        /// <param name="msg"> News </param>
        public static void WriteLineWarning(string msg)
        {
            BP.DA.Log.DefaultLogWriteLineWarning(msg);
        }
        /// <summary>
        ///  Writes an error 
        /// </summary>
        /// <param name="msg"> News </param>
        public static void WriteLineError(string msg)
        {
            BP.DA.Log.DefaultLogWriteLineError(msg);
        }
        #endregion  Written to the system log  

        #region  Write to user log ( Written in the user table :Sys_UserLog).
        /// <summary>
        ///  Write to user log 
        /// </summary>
        /// <param name="logType"> Type </param>
        /// <param name="empNo"> Operator number </param>
        /// <param name="msg"> Information </param>
        /// <param name="ip">IP</param>
        public static void WriteUserLog(string logType, string empNo, string msg, string ip)
        {
            UserLog ul = new UserLog();
            ul.MyPK = DBAccess.GenerGUID();
            ul.FK_Emp = empNo;
            ul.LogFlag = logType;
            ul.Docs = msg;
            ul.IP = ip;
            ul.RDT = DataType.CurrentDataTime;
            ul.Insert();
        }
        /// <summary>
        ///  Write to user log 
        /// </summary>
        /// <param name="logType"> Log Type </param>
        /// <param name="empNo"> Operator number </param>
        /// <param name="msg"> News </param>
        public static void WriteUserLog(string logType, string empNo, string msg)
        {
            UserLog ul = new UserLog();
            ul.MyPK = DBAccess.GenerGUID();
            ul.FK_Emp = empNo;
            ul.LogFlag = logType;
            ul.Docs = msg;
            ul.RDT = DataType.CurrentDataTime;
            try
            {
                if (BP.Sys.SystemConfig.IsBSsystem)
                    ul.IP = BP.Sys.Glo.Request.UserHostAddress;
            }
            catch
            {
            }
            ul.Insert();
        }
        #endregion  Write to user log .

        /// <summary>
        ///  Get the object .
        /// </summary>
        public static System.Web.HttpRequest Request
        {
            get
            {
                return System.Web.HttpContext.Current.Request; 
            }
        }

    }
}
