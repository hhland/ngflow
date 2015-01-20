using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices; 

namespace BP
{
    public enum GSMSta
    {
        /// <summary>
        ///  Stagnate 
        /// </summary>
        Stop,
        /// <summary>
        ///  Run 
        /// </summary>
        Runing
    }
    public class Modem
    {


        #region API
        /// <summary>
        ///  Main entry point for the application .
        /// </summary>
        [STAThread]
        [DllImport("sms.dll", EntryPoint = "Sms_Connection")]
        public static extern uint Sms_Connection(string CopyRight, uint Com_Port, uint Com_BaudRate, out string Mobile_Type, out string CopyRightToCOM);

        [DllImport("sms.dll", EntryPoint = "Sms_Disconnection")]
        public static extern uint Sms_Disconnection();

        [DllImport("sms.dll", EntryPoint = "Sms_Send")]
        public static extern uint Sms_Send(string Sms_TelNum, string Sms_Text);

        [DllImport("sms.dll", EntryPoint = "Sms_Receive")]
        public static extern uint Sms_Receive(string Sms_Type, out string Sms_Text);

        [DllImport("sms.dll", EntryPoint = "Sms_Delete")]
        public static extern uint Sms_Delete(string Sms_Index);

        [DllImport("sms.dll", EntryPoint = "Sms_AutoFlag")]
        public static extern uint Sms_AutoFlag();

        [DllImport("sms.dll", EntryPoint = "Sms_NewFlag")]
        public static extern uint Sms_NewFlag(); 
        #endregion
        /// <summary>
        ///  Status 
        /// </summary>
        public static GSMSta GSMState = GSMSta.Stop;
        /// <summary>
        ///  Connected to the device 
        /// </summary>
        /// <returns></returns>
        public static bool Conn()
        {
            try
            {
                String TypeStr = "";
                String CopyRightToCOM = "";
                String CopyRightStr = "// Shanghai Information Technology Co., fast race , Site www.xunsai.com//";
                for (int i = 0; i < 10; i++)
                {
                    if (Sms_Connection(CopyRightStr, uint.Parse(i.ToString()), 9600, out TypeStr, out CopyRightToCOM) == 1)
                    {
                        ///5 For the serial number ,0 Infrared interface ,1,2,3,... For the serial 
                        GSMState = GSMSta.Runing;
                        Console.Beep();
                        return true;
                    }
                    // Stop 5秒.
                    System.Threading.Thread.Sleep(5000);
                }
                GSMState = GSMSta.Stop;
                return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(" An error occurred when connecting devices , Please check plugged .\t\n Technical Information :"+ex.Message, 
                    " Prompt ", MessageBoxButtons.OK);
                return false;
            }
        }
        /// <summary>
        ///  Broken Links 
        /// </summary>
        /// <returns></returns>
        public static void Close()
        {
            Sms_Disconnection();
        }
        /// <summary>
        ///  Performing transmission 
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Send(string tel, string msg)
        {
            if (Sms_Send(tel, msg) == 1)
                return true;
            else
                return false;
        }
        public Modem()
        {
        }
    }
}
