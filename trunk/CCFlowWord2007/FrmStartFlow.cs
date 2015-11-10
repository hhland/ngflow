using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BP.WF;
using BP.Web;

namespace CCFlowWord2007
{
    public partial class FrmStartFlow : Form
    {
        public FrmStartFlow()
        {
            InitializeComponent();
        }

        private void FrmStartFlow_Load(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = BP.WF.Dev2Interface.DataTable_DB_GenerCanStartFlowsOfDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.Message);
                return;
            }

            this.treeView1.Nodes.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                FlowAppType appType = (FlowAppType)int.Parse(dr["FlowAppType"].ToString());
                if (appType != FlowAppType.DocFlow)
                    continue;

                TreeNode tn = new TreeNode();
                tn.Text = dr["Name"].ToString();
                tn.Tag = dr["No"].ToString();
                this.treeView1.Nodes.Add(tn);
            }
        }
        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            //FrmStartFlow_Load(null, null);
            this.Close();
        }
        /// <summary>
        ///  New Process 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            TreeNode tn = this.treeView1.SelectedNode;
            if (tn == null)
            {
                MessageBox.Show(" Please select a process , You can launch it .",
                    " Prompt ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Process ID .
            string fk_flow = tn.Tag.ToString();
            BP.Web.WebUser.FK_Flow = fk_flow;

            // Generate a new workid.
            WebUser.WorkID = BP.WF.Dev2Interface.GetDesignerServiceInstance().Node_CreateBlankWork(WebUser.FK_Flow,
                WebUser.No, null);


            #region  First, there is no judgment on the server that processes the draft document , If it falls out .
            FtpSupport.FtpConnection conn = Glo.HisFtpConn;
            try
            {
                conn.SetCurrentDirectory("/DocFlow/" + WebUser.FK_Flow + "/" + WebUser.WorkID + "/");
                bool isHave = conn.FileExist(WebUser.WorkID + ".doc");
                if (isHave == false)
                {
                    /* If there is no will go down . */
                }
                else
                {
                    if (MessageBox.Show(" We found that you have saved draft , Do you want to open this draft ?", " Prompt ",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {
                            /* Download , And open it .*/
                            string fileTemp = Glo.PathOfTInstall + fk_flow + "@" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".doc";
                            conn.GetFile(WebUser.WorkID + ".doc", fileTemp, true, FileAttributes.Archive);
                            conn.Close();
                            MessageBox.Show(" The draft has been opened ", " Prompt ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Glo.OpenDoc(fileTemp, false);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, " Drafts open failed ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        /* Does not deal with , Open the template .*/
                    }
                }
            }
            catch
            {
                /* Description There are no files , Down on the implementation .*/
            }
            #endregion

            //  Download Process Template  
            conn = Glo.HisFtpConn;
            string file = Glo.PathOfTInstall + fk_flow + "@" + DateTime.Now.ToString("MM月dd日hh时mm分ss秒") + ".doc";
            try
            {
                conn.SetCurrentDirectory("/DocFlowTemplete/");
                if (conn.FileExist(fk_flow + ".doc") == false)
                    throw new Exception("@ No startup settings templates for documents .");

                conn.GetFile(fk_flow + ".doc", file, true, FileAttributes.Archive);
                conn.Close();

                Glo.OpenDoc(file, false);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                file = null;
                MessageBox.Show("@ Exception Information :" + ex.Message + "\t\n@ Process ID :" + WebUser.FK_Flow + "\t\n@ Possible reasons are as follows :1, Designers do not have the correct settings ftp Server . \t\n2, Without this process of document templates .");
            }

            WebUser.FK_Flow = fk_flow;
            WebUser.FK_Node = int.Parse(fk_flow + "01");

            //  WebUser.RetrieveWFNode(WebUser.FK_Node);
            //    WebUser.WriterIt(StartFlag.DoNewFlow, fk_flow, int.Parse(fk_flow + "01"), 0);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
