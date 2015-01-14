using System;
using System.Collections.Generic;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using BP.Web;
using System.Windows.Forms;
using BP.WF;
using BP.Web;

namespace CCFlowWord2007
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                WebUser.HisRib.SetState();
            }
            catch (Exception ex)
            {
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            if (Profile.IsExitProfile == false)
                return;

            var dr = MessageBox.Show(" You want to quit it safe ?", " Prompt ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
                WebUser.SignOut();
        }

        #region Methods
        /// <summary>
        ///  The save 
        /// </summary>
        public void DoSave()
        {
            if (WebUser.FK_Flow == null)
            {
                MessageBox.Show(" You do not have the selection process, you can not save .");
                return;
            }


            if (WebUser.WorkID == 0)
                WebUser.WorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(BP.Web.WebUser.FK_Flow);

            #region  The file on the server 
            FtpSupport.FtpConnection conn = Glo.HisFtpConn;
            try
            {
                conn.SetCurrentDirectory("/DocFlow/" + WebUser.FK_Flow + "/" + WebUser.WorkID + "/");
            }
            catch
            {
                if (conn.DirectoryExist("/DocFlow/") == false)
                    conn.CreateDirectory("/DocFlow/");

                if (conn.DirectoryExist("/DocFlow/" + WebUser.FK_Flow + "/") == false)
                    conn.CreateDirectory("/DocFlow/" + WebUser.FK_Flow + "/");

                if (conn.DirectoryExist("/DocFlow/" + WebUser.FK_Flow + "/" + WebUser.WorkID + "/") == false)
                    conn.CreateDirectory("/DocFlow/" + WebUser.FK_Flow + "/" + WebUser.WorkID);

                conn.SetCurrentDirectory("/DocFlow/" + WebUser.FK_Flow + "/" + WebUser.WorkID + "/");
            }
            string file = Glo.PathOfTInstall + DateTime.Now.ToString("MMddhhmmss") + ".doc";
            ThisAddIn.SaveAs(file);

            System.IO.File.Copy(file, "c:\\Tmp.doc", true);

            conn.PutFile("c:\\Tmp.doc", WebUser.FK_Node + "@" + WebUser.No + ".doc"); // Current file officers .
            conn.PutFile("c:\\Tmp.doc", WebUser.WorkID + ".doc"); // The latest file .
            conn.Close();
            #endregion  The file on the server 

            // Delete Temporary Files .
            System.IO.File.Delete("c:\\Tmp.doc");
            //  MessageBox.Show(" Your file has been saved to the server ", " Saved successfully ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void SaveAs(string file)
        {
            object FileName = file;
            object FileFormat = Word.WdSaveFormat.wdFormatDocument;

            Globals.ThisAddIn.Application.ActiveWindow.Document.Save();
            Globals.ThisAddIn.Application.ActiveWindow.Document.SaveAs(ref FileName, ref FileFormat);
        }

        #endregion

        #region VSTO generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        #endregion
    }
}
