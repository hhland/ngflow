using System;
using System.IO;
using BP.Pub;
using BP.Sys;

namespace BP.DA
{
	#region enum LogType
	/// <summary>
	///  Information Type 
	/// </summary>
    public enum LogType
    {
        /// <summary>
        ///  Prompt 
        /// </summary>
        Info = 1,
        /// <summary>
        ///  Caveat 
        /// </summary>
        Warning = 2,
        /// <summary>
        ///  Error 
        /// </summary>
        Error = 3
    }
	#endregion

	#region class Log	
	/// <summary>
	///  Journal 
	/// </summary>
	public class Log
	{
		#region  Under test 
		public static void DebugWriteInfo(string msg)
		{
			if (SystemConfig.IsDebug)
				DefaultLogWriteLine(LogType.Info,msg);
		}
		public static void DebugWriteWarning(string msg)
		{
			if (SystemConfig.IsDebug)
				DefaultLogWriteLine(LogType.Warning,msg);
		}
        public static void DebugWriteError(string msg)
        {
            if (SystemConfig.IsDebug)
                DefaultLogWriteLine(LogType.Error, msg);
        }
		#endregion


		public static void DefaultLogWriteLineError(string msg)
		{
			DefaultLogWriteLine(LogType.Error,msg);
		}

        public static void DefaultLogWriteLineError(string msg, bool isOutDos)
        {
            DefaultLogWriteLine(LogType.Error, msg);
            if (isOutDos)
                System.Console.WriteLine(msg);
        }

		public static void DefaultLogWriteLineInfo(string msg)
		{
			DefaultLogWriteLine(LogType.Info,msg);
		}

        public static void DefaultLogWriteLineInfo(string msg, bool isoutDos)
        {
            DefaultLogWriteLine(LogType.Info, msg);
            if (isoutDos)
                System.Console.WriteLine(msg);
        }

		public static void DefaultLogWriteLineWarning(string msg)
		{		
			DefaultLogWriteLine(LogType.Warning,msg);
		}
        public static void DefaultLogWriteLineWarning(string msg, bool isOutdoc)
        {
            DefaultLogWriteLine(LogType.Warning, msg);
            if (isOutdoc)
                System.Console.WriteLine(msg);
        }

		#region  Static methods are often used 
//		private static Log _enlog = new Log(Log.GetLogFileName("EnLog"));
//		public static void EnLogWriteLine(LogType type, string info)
//		{
//			_log.WriteLine(type, info);
//		}

		private static Log _log = new Log( Log.GetLogFileName() );
		public static void DefaultLogWriteLine(LogType type, string info)
		{
			_log.WriteLine(type, info);
		}
        public static void DefaultLogWriteLineWithOutUseInfo(string info)
        {
            _log.openFile();
            _log.writelog(info);
            _log.closeFile();
        }
		#endregion

		private bool isReady = false;
		private StreamWriter swLog;
		private string strLogFile;
		private string userName="System";
		/// <summary>
		///  Constructor 
		/// </summary>
		/// <param name="LogFileName"></param>
        public Log(string LogFileName)
        {
            this.strLogFile = LogFileName;
            try
            {
                openFile();
                writelog("-----------------------------------------------------------------------------------------------------");
            }
            finally
            {
                closeFile();
            }
        }
		/// <summary>
		///  User 
		/// </summary>
		public string UserName
		{
			set{this.userName = value;}
		}
		private void writelog(string msg) 
		{
			if(isReady) 
			{
				swLog.WriteLine(msg);
			} 
			else 
			{
				Console.WriteLine("Error Cannot write to log file.");
			}
		}
        public static void ClearLog()
        {
            try
            {
                File.Delete(BP.Sys.SystemConfig.PathOfLog + DateTime.Now.ToString("\\yyyy_MM_dd.log"));
            }
            catch
            {
            }
        }
        private void openFile()
        {
            try
            {
                swLog = File.AppendText(strLogFile);
                isReady = true;
            }
            catch
            {
                isReady = false;
            }
        }
		private void closeFile() 
		{			
			if(isReady)
			{
				try 
				{
					swLog.Flush();
					swLog.Close();
				} 
				catch 
				{
				}
			}
		}
		/// <summary>
		///  Obtain Log The file path and file name 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetLogFileName()
		{
			string filepath=SystemConfig.PathOfLog ;
			
			// If the directory is not established , Is established .
			if (Directory.Exists(filepath) == false) 
				Directory.CreateDirectory(filepath);

			return filepath + "\\"+ DateTime.Now.ToString("yyyy_MM_dd") + ".log";

			//return filepath + "\\"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".log";
		}
		/// <summary>
		///  Write a line of log 
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			WriteLine(LogType.Info,message);
		}
		/// <summary>
		///  Write a line of log 
		/// </summary>
		/// <param name="logtype"></param>
		/// <param name="message"></param>
		public void WriteLine(LogType logtype, string message) 
		{
//			string stub = DateTime.Now.ToString("@HH:MM:ss") ;
            string stub = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
			switch(logtype) 
			{
				case LogType.Info:
					stub += "Info:";
					break;
				case LogType.Warning:
					stub += "Warning:";
					break;
				case LogType.Error:
					stub += "Fatal:";
					break;
			}
			stub = stub + userName + "');\"" + message;
			openFile();
			writelog(stub);
			closeFile();
			//Console.WriteLine(stub);
		}
        /// <summary>
        ///  Open the log directory 
        /// </summary>
        public static void OpenLogDir()
        {
            string file = BP.Sys.SystemConfig.PathOfLog;
            try
            {
                System.Diagnostics.Process.Start(file);
            }
            catch (Exception ex)
            {
                throw new Exception("@ Open the log directory error ." + ex.Message);
            }
        }
        /// <summary>
        ///  Open today log 
        /// </summary>
        public static void OpeLogToday()
        {
            string file = BP.Sys.SystemConfig.PathOfLog + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
            try
            {
                System.Diagnostics.Process.Start(file);
            }
            catch(Exception ex)
            {
                throw new Exception("@ Open the log file errors ."+ex.Message );
            }
        }

	}
	#endregion
}
