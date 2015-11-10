using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BP.Web;
using BP.DA;
using BP.WF;
using Office = Microsoft.Office.Core;
using CCFlowWord2007;

namespace BP.Comm
{
    public partial class FrmIE : Form
    {
        public FrmIE()
        {
            InitializeComponent();
        }

        public Ribbon1 HisRibbon1;

        #region Load

        private void FrmIE_Load(object sender, EventArgs e)
        {
             
        }
        string MyURL = "";
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (this.MyURL.Contains("SendInfo") == true)
                this.Tag = "SendInfo";

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            base.OnFormClosed(e);
        }

        #endregion

        #region Control Events

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.AbsoluteUri;
            MyURL = url;
            this.statusStrip1.Text = url;
            string pageID = url.Substring(url.LastIndexOf('/') + 1);

            this.toolStripStatusLabel1.Text = url;

            if (pageID.IndexOf(".aspx") != -1)
            {
                pageID = pageID.Substring(0, pageID.IndexOf(".aspx"));
                url = url.Substring(url.IndexOf(".aspx"));
                url = url.Replace("?", "@");
                url = url.Replace("&", "@");
                url = url.Replace(".aspx", "");

                var para = new AtPara(url);
                switch (pageID)
                {
                    case "DoClient":
                        try
                        {
                            switch (para.DoType)
                            {
                                case DoTypeConst.DoStartFlow: // Initiate the process 
                                    this.DoStartFlow(para);
                                    break;
                                case DoTypeConst.DoStartFlowByTemple: // Start the process 
                                    this.DoStartFlowByTemple(para);
                                    break;
                                case DoTypeConst.OpenFlow: // Open Process 
                                    this.DoOpenFlow(para);
                                    break;
                                case DoTypeConst.OpenDoc:
                                    this.DoOpenDoc(para);
                                    break;
                                case DoTypeConst.DelFlow: // Delete process .
                                    WebUser.FK_Flow = null;
                                    WebUser.FK_Node = 0;
                                    WebUser.WorkID = 0;
                                    this.Close();
                                    break;
                                default:
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("@ Error :" + ex.Message + " PageID=" + pageID + "  DoType=" + para.DoType, " Error ",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Show url
        /// </summary>
        /// <param name="url"></param>
        public void ShowUrl(string url)
        {
            this.webBrowser1.Url = new Uri(url);
        }
        public void OpenDoc(string file, object _isReadonly)
        {
        }
        #endregion

        #region Flow Methods
        /// <summary>
        ///  Performing transmission 
        /// </summary>
        /// <param name="para"></param>
        public void DoSend(AtPara para)
        {
            Globals.ThisAddIn.DoSave();
            object obj = Type.Missing;
            Globals.ThisAddIn.Application.ActiveDocument.Close(ref obj, ref obj, ref obj);
        }
        public void DoOpenDoc(AtPara para)
        {
            string fk_flow = para.GetValStrByKey("FK_Flow");
            int workid = para.GetValIntByKey("WorkID");
            int fk_node = para.GetValIntByKey("FK_Node");

            string file = Glo.PathOfTInstall + workid + "@" + WebUser.No + ".doc";
            if (File.Exists(file) == false)
            {
                try
                {
                    FtpSupport.FtpConnection conn = Glo.HisFtpConn;
                    if (conn.DirectoryExist("/DocFlow/" + fk_flow + "/" + workid))
                    {
                        conn.SetCurrentDirectory("/DocFlow/" + fk_flow + "/" + workid);
                        conn.GetFile(workid + ".doc", file, true, FileAttributes.Archive);
                        conn.Close();
                    }
                    else
                    {
                        throw new Exception("@ File not found , Process error .");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("@ Open the document error @ Technical Information :" + ex.Message + "@ Process ID :" + WebUser.FK_Flow);
                    return;
                }
            }

            WebUser.FK_Flow = fk_flow;
            WebUser.FK_Node = fk_node;
            WebUser.WorkID = workid;

            WebUser.RetrieveWFNode(WebUser.FK_Node);

            /* If this file exists , To activate it .*/
            WebUser.WriterIt(StartFlag.DoOpenDoc, fk_flow, fk_node, workid);

            this.OpenDoc(file, false);
            this.HisRibbon1.SetState();
            this.Close();
        }

        /// <summary>
        ///  Open Process 
        /// </summary>
        /// <param name="para"></param>
        public void DoOpenFlow(AtPara para)
        {
            string fk_flow = para.GetValStrByKey("FK_Flow");
            int workid = para.GetValIntByKey("WorkID");
            int fk_node = para.GetValIntByKey("FK_Node");

            if (WebUser.WorkID == workid && WebUser.FK_Node == fk_node)
            {
                if (MessageBox.Show(" The current process has been opened , You want to reload it ?",
                    " Prompt ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    this.webBrowser1.GoBack();
                    return;
                }
            }

            string file = Glo.PathOfTInstall + workid + "@" + WebUser.No + ".doc";
            if (File.Exists(file) == false)
            {
                try
                {
                    FtpSupport.FtpConnection conn = Glo.HisFtpConn;
                    if (conn.DirectoryExist("/DocFlow/" + fk_flow + "/" + workid))
                    {
                        conn.SetCurrentDirectory("/DocFlow/" + fk_flow + "/" + workid);
                        conn.GetFile(workid + ".doc", file, true, FileAttributes.Archive);
                        conn.Close();
                    }
                    else
                    {
                        throw new Exception("@ File not found , Process file missing error .");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("@ Open the document error @ Technical Information :" + ex.Message + "@ Process ID :" + WebUser.FK_Flow);
                    return;
                }
            }

            WebUser.FK_Flow = fk_flow;
            WebUser.FK_Node = fk_node;
            WebUser.WorkID = workid;

            WebUser.RetrieveWFNode(WebUser.FK_Node);

            /* If this file exists , To activate it .*/
            WebUser.WriterIt(StartFlag.DoOpenFlow, fk_flow, fk_node, workid);
            this.OpenDoc(file, false);

            
            this.Close();
        }

        /// <summary>
        ///  Initiate the process 
        /// </summary>
        /// <param name="para"></param>
        public void DoStartFlow(AtPara para)
        {
            string fk_flow = para.GetValStrByKey("FK_Flow");
            string workid = para.GetValStrByKey("WorkID");
            string file = Glo.PathOfTInstall + workid + "@" + WebUser.No + ".doc";

            if (File.Exists(file) == false)
            {
                try
                {
                    FtpSupport.FtpConnection conn = Glo.HisFtpConn;
                    if (conn.DirectoryExist("/DocFlow/" + fk_flow + "/" + workid))
                    {
                        conn.SetCurrentDirectory("/DocFlow/" + fk_flow + "/" + workid);
                        if (conn.FileExist(WebUser.FK_Node + "@" + WebUser.No + ".doc"))
                            conn.GetFile(WebUser.FK_Node + "@" + WebUser.No + ".doc", file, true, FileAttributes.Archive);
                        else
                            file = null;
                    }
                    else
                    {
                        file = null;
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("@ Process design errors , No maintenance documents for the process template .@ Technical Information :" + ex.Message + "@ Process ID :" + WebUser.FK_Flow);
                    return;
                }
            }

            WebUser.FK_Flow = fk_flow;
            WebUser.WorkID = int.Parse(workid);

            /* If this file exists , To activate it .*/
            WebUser.WriterIt(StartFlag.DoNewFlow, fk_flow, int.Parse(fk_flow + "01"), int.Parse(workid));
            this.OpenDoc(file, false);
            this.Close();
        }
        /// <summary>
        ///  Generate documents in accordance with the template 
        /// </summary>
        /// <param name="para"></param>
        public void DoStartFlowByTemple(AtPara para)
        {
            string fk_flow = para.GetValStrByKey("FK_Flow");
            //  Download Process Template  
            FtpSupport.FtpConnection conn = Glo.HisFtpConn;
            string file = Glo.PathOfTInstall + fk_flow + "@" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".doc";
            try
            {
                conn.SetCurrentDirectory("/DocFlowTemplete/");
                if (conn.FileExist(fk_flow + ".doc") == false)
                    throw new Exception("@ No startup settings templates for documents .");

                conn.GetFile(fk_flow + ".doc", file, true, FileAttributes.Archive);
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                file = null;
                MessageBox.Show("@ Exception Information :" + ex.Message + "\t\n@ Process ID :" + WebUser.FK_Flow + "\t\n@ Possible reasons are as follows :1, Designers do not have the correct settings ftp Server . \t\n2, Without this process of document templates .");
            }

            WebUser.WorkID = 0;
            WebUser.FK_Flow = fk_flow;
            WebUser.FK_Node = int.Parse(fk_flow + "01");
            WebUser.RetrieveWFNode(WebUser.FK_Node);
            this.HisRibbon1.SetState();
            WebUser.WriterIt(StartFlag.DoNewFlow, fk_flow, int.Parse(fk_flow + "01"), 0);
            this.OpenDoc(file, false);
            this.Close();
        }
        #endregion
    }
}
