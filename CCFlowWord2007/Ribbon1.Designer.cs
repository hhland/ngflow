using System;
using System.IO;
using BP.Web;
using System.Windows.Forms;
using BP.WF;
using BP.Comm;
using Microsoft.Office.Tools.Ribbon;

namespace CCFlowWord2007
{
    partial class Ribbon1 : RibbonBase
    {
        /// <summary>
        ///  Required designer variable .
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Methods

        /// <summary>
        ///  Load XML, Creating navigation 
        /// </summary>
        public void LoadXml()
        {
            BP.WF.Tabs tabs = new BP.WF.Tabs();
            tabs.RetrieveAll();

            BP.WF.GroupFuncs gs = new BP.WF.GroupFuncs();
            gs.RetrieveAll();

            BP.WF.Funcs fs = new BP.WF.Funcs();
            fs.RetrieveAll();

            this.SuspendLayout();
            this.Tabs.Clear();
            int i = 1;
            foreach (BP.WF.Tab tb in tabs)
            {
                i++;
                RibbonTab mytab = Factory.CreateRibbonTab();
                mytab.Label = tb.Name;
                mytab.Name = "t" + tb.No + i;
                if (i == 2)
                    mytab.ControlId.ControlIdType = RibbonControlIdType.Custom;
                else
                    mytab.ControlId.ControlIdType = RibbonControlIdType.Office;

                mytab.SuspendLayout();

                foreach (BP.WF.GroupFunc g in gs)
                {
                    if (g.FK_Tab != tb.No)
                        continue;

                    RibbonGroup group = Factory.CreateRibbonGroup();
                    group.Name = "s" + g.No;
                    group.Label = g.Name;
                    group.DialogLauncherClick += new RibbonControlEventHandler(Btn_Click);
                    group.SuspendLayout();

                    foreach (BP.WF.Func f in fs)
                    {
                        if (f.FK_Group != g.No)
                            continue;

                        switch (f.CtlType)
                        {
                            case "Btn":
                                RibbonButton btn = Factory.CreateRibbonButton();
                                btn.Name = "Btn_" + f.No;
                                btn.Label = f.Name;
                                btn.Tag = f;
                                try
                                {
                                    if (f.IsIcon)
                                    {
                                        btn.Image = System.Drawing.Image.FromFile(BP.WF.Glo.PathOfTInstall + "\\Img\\" + f.No + ".gif");
                                        btn.ShowImage = true;
                                    }
                                }
                                catch
                                {
                                }
                                btn.Click += new RibbonControlEventHandler(Btn_Click);
                                group.Items.Add(btn);
                                break;
                            default:
                                RibbonLabel lab = Factory.CreateRibbonLabel();
                                lab.Name = "Lab_" + f.No;
                                lab.Label = f.Name;
                                lab.Tag = f;
                                group.Items.Add(lab);
                                break;
                        }
                    }
                    group.ResumeLayout(false);
                    group.PerformLayout();
                    mytab.Groups.Add(group);
                } // End add to Group.

                mytab.ResumeLayout(false);
                mytab.PerformLayout();
                this.Tabs.Add(mytab);
            } // End add to Tab.

            this.ResumeLayout(false);
            this.RibbonType = "Microsoft.Word.Document";
            //  this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Load += new RibbonUIEventHandler(Ribbon1_Load);
        }
        /// <summary>
        ///  Carried out btn Local events 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="btn"></param>
        public void Do(BP.WF.Func func, RibbonButton btn)
        {
            switch (func.No)
            {
                case "LogOut":
                    if (MessageBox.Show(" Are you sure you want to log out ?", " Perform validation ", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        return;
                    WebUser.SignOut();
                    this.SetState();
                    break;
                case "Login":
                    FrmLogin fl = new FrmLogin();
                    fl.ShowDialog();
                    this.SetState();
                    return;
                case "ChUser":
                    FrmLogin fm = new FrmLogin();
                    fm.ShowDialog();
                    break;
                case "WKInfo":
                    string msg = "\t\n No=" + WebUser.No;
                    msg += "\t\n FK_Flow=" + WebUser.FK_Flow;
                    msg += "\t\n FK_Node=" + WebUser.FK_Node;
                    msg += "\t\n WorkID=" + WebUser.WorkID;
                    MessageBox.Show(msg);
                    break;
                case "About":
                    AboutBox ab = new AboutBox();
                    ab.ShowDialog();
                    break;
                case "Save":
                    this.DoSave();
                    break;
                case "SaveTo":
                default:
                    MessageBox.Show(" Function not implemented :" + func.No + " " + func.Name);
                    break;
            }
        }

        /// <summary>
        ///  Obtain the specified name RibbonBtn
        /// </summary>
        /// <param name="name"> Name </param>
        /// <returns></returns>
        public RibbonButton GetBtn(string name)
        {
            foreach (RibbonTab tab in this.Tabs)
            {
                foreach (RibbonGroup g in tab.Groups)
                {
                    RibbonButton btn;
                    for (int i = 0; i <= g.Items.Count; i++)
                    {
                        try
                        {
                            btn = g.Items[i] as RibbonButton;
                            if (btn == null)
                                continue;

                            if (btn.Name == name)
                                return btn;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            MessageBox.Show("@ Not found Name=" + name + "  Buttons ");
            return null;
        }
        /// <summary>
        ///  The save documents 
        /// </summary>
        public void DoSave()
        {
            Globals.ThisAddIn.DoSave();
        }
        /// <summary>
        ///  Performing transmission 
        /// </summary>
        public void DoSend()
        {
            if (WebUser.FK_Flow == null)
            {
                MessageBox.Show(" You do not have to perform the process of the draft document can not be issued .");
                return;
            }

            // Creating work ID.
            if (WebUser.WorkID == 0)
                WebUser.WorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(WebUser.FK_Flow);

            // Save the file to the server .
            this.DoSave();

            FrmIE ie = new FrmIE();
            ie.Width = 900;
            ie.Height = 600;

            string doWhat = "DealWorkInSmall";
            if (BP.Web.WebUser.FK_Node.ToString().LastIndexOf("01") == 2)
                doWhat = "StartSmall";

            string tag = "@Serv/WF/Port.aspx?DoWhat=" + doWhat;
            tag = tag.Replace("@Serv", BP.WF.Glo.WFServ);
            ie.ShowUrl(tag + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID + "&FK_Flow=" + BP.Web.WebUser.FK_Flow + "&FK_Node=" + WebUser.FK_Node + "&WorkID=" + WebUser.WorkID + "&FID=" + WebUser.FID);
            ie.Text = " Hello :" + WebUser.No + "," + WebUser.Name;
            ie.ShowInTaskbar = false;
            ie.HisRibbon1 = this;
            var v = ie.ShowDialog();

            string dowhat = ie.Tag as string;
            if (v == DialogResult.OK && dowhat == "SendInfo")
            {
                // Send successful ,  Settings button state .
                this.Btn_Save.Enabled = false;
                this.Btn_Send.Enabled = false;
                this.Btn_Return.Enabled = false;
                this.Btn_UnSend.Enabled = true;
                this.Btn_FW.Enabled = false;
                MessageBox.Show(" Work has been successfully sent .");
            }
            else
            {
                MessageBox.Show(" Failure has been successfully sent ...");
            }
        }
        /// <summary>
        ///  Performing handover 
        /// </summary>
        public void DoShift()
        {
            ShiftFrm frm = new ShiftFrm();
            frm.ShowDialog();
            if (frm.DialogResult != DialogResult.OK)
                return;

            // Send successful ,  Settings button state .
            this.Btn_Save.Enabled = false;
            this.Btn_Send.Enabled = false;
            this.Btn_Return.Enabled = false;
            this.Btn_UnSend.Enabled = false;
            this.Btn_FW.Enabled = false;
            this.Btn_Del.Enabled = false;
        }
        /// <summary>
        ///  Implementation of return 
        /// </summary>
        public void DoReturn()
        {
            ReturnFrm frm = new ReturnFrm();
            frm.ShowDialog();
            if (frm.DialogResult != DialogResult.OK)
                return;

            // Send successful ,  Settings button state .
            this.Btn_Save.Enabled = false;
            this.Btn_Send.Enabled = false;
            this.Btn_Return.Enabled = false;
            this.Btn_UnSend.Enabled = false;
            this.Btn_FW.Enabled = false;
            this.Btn_Del.Enabled = false;
        }
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public void DoUnSend()
        {
            if (MessageBox.Show(" Are you sure you want to send it to undo ?", " Prompt ",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                BP.WF.Dev2Interface.GetDesignerServiceInstance().Flow_DoUnSend(BP.Web.WebUser.FK_Flow,
                    BP.Web.WebUser.WorkID, BP.Web.WebUser.No);

                // Send successful ,  Settings button state .
                this.Btn_Save.Enabled = false;
                this.Btn_Send.Enabled = false;
                this.Btn_Return.Enabled = false;
                this.Btn_UnSend.Enabled = false;
                this.Btn_FW.Enabled = false;


                if (BP.Web.WebUser.IsStartNode)
                {
                    this.Btn_Save.Enabled = true;
                    this.Btn_Send.Enabled = true;
                    this.Btn_Return.Enabled = false;
                    this.Btn_UnSend.Enabled = false;
                    this.Btn_FW.Enabled = false;
                }
                else
                {
                    this.Btn_Save.Enabled = true;
                    this.Btn_Send.Enabled = true;
                    this.Btn_Return.Enabled = true;
                    this.Btn_UnSend.Enabled = false;
                    this.Btn_FW.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Revocation failure ");
            }
        }
        public void DoOpenEmpWork(string tag)
        {
            // Get 4 Large parameter .
            string[] strs = tag.ToString().Split('@');
            string fk_flow = strs[1];
            int fk_node = int.Parse(strs[2]);
            Int64 workid = Int64.Parse(strs[3]);
            Int64 fid = Int64.Parse(strs[4]);
            if (WebUser.WorkID == workid && WebUser.FK_Node == fk_node)
                return;

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
            // Open the document .
            Glo.OpenDoc(file, false);

            // Settings button state .
            this.Btn_Start.Enabled = true;
            this.Btn_Send.Enabled = true;
            this.Btn_Save.Enabled = true;
            this.Btn_Return.Enabled = true;
            this.Btn_Del.Enabled = true; // Delete 
            this.Btn_FW.Enabled = true; //  Transfer .
            this.Btn_UnSend.Enabled = false;
        }
        #endregion

        #region btn Events
        void Btn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                RibbonButton btn = (RibbonButton)sender;
                BP.WF.Func func = (BP.WF.Func)btn.Tag;
                switch (func.DoType)
                {
                    case "Shift": //  Transfer .
                        this.DoShift();
                        return;
                    case "Return":
                        this.DoReturn();
                        return;
                    case "Start": // Initiate the process execution .
                        FrmStartFlow start = new FrmStartFlow();
                        var v=  start.ShowDialog();
                        if (v == DialogResult.OK)
                        {
                            // Columns button control set false.
                            this.Btn_Save.Enabled = false;
                            this.Btn_Return.Enabled = false;
                            this.Btn_Send.Enabled = false;
                            this.Btn_FW.Enabled = false;

                            /* After setting state sponsored .*/
                            this.Btn_Save.Enabled = true;
                            this.Btn_Send.Enabled = true;
                        }
                        else
                        {
                            /* No choice to initiate the process , Button state remains unchanged .*/
                        }
                        return;
                    case "Save": // The save .
                        this.DoSave();
                        MessageBox.Show(" Successfully saved to the network ", " Saving tips ", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    case "RunExe":
                        System.Diagnostics.Process.Start(func.Tag);
                        return;
                    case "EmpWorks":
                        EmpWorks ew = new EmpWorks();
                        ew.ShowDialog();
                        if (ew != null)
                        {
                            if (ew.DialogResult == DialogResult.OK)
                                this.DoOpenEmpWork(ew.Tag.ToString());
                        }
                        return;
                    case "RunIE":
                        switch (func.No)
                        {
                            case "Send": // To perform issuance .
                                this.DoSend(); // Performing transmission .
                                return;
                           
                            case "UnSend": // Perform revocation .
                                this.DoUnSend();
                                return;
                            case "Del":
                                if (MessageBox.Show(" Are you sure you want to delete it ?", " Prompt ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    return;
                                break;
                            default:
                                break;
                        }

                        FrmIE ie = new FrmIE();
                        ie.Width = func.Width;
                        ie.Height = func.Height;

                        string tag = func.Tag;
                        tag = tag.Replace("@Serv", BP.WF.Glo.WFServ);
                        ie.ShowUrl(tag + "&UserNo=" + WebUser.No + "&FK_Flow=" + BP.Web.WebUser.FK_Flow + "&FK_Node=" + WebUser.FK_Node + "&WorkID=" + WebUser.WorkID+"&FID="+BP.Web.WebUser.FID);
                        ie.Text = " Hello :" + WebUser.No + "," + WebUser.Name + "  -  " + func.Name;
                        ie.ShowInTaskbar = false;
                        ie.HisRibbon1 = this;
                        ie.ShowDialog();
                        return;
                    default:
                        try
                        {
                            this.Do(func, btn);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(" Carried out " + func.Name + " Error ." + ex.Message);
                        }
                        break;
                }

                this.SetState();
            }
            catch (Exception ex)
            {
                SetState();
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region  Set up  btn  Status 

        /// <summary>
        ///  Settings button state 
        /// </summary>
        public void SetState()
        {
            // Function Buttons 
            this.Btn_Start.Enabled = false;
            this.Btn_Send.Enabled = false;
            this.Btn_Return.Enabled = false;
            this.Btn_Del.Enabled = false;
            this.Btn_FW.Enabled = false;
            this.Btn_UnSend.Enabled = false;    //Node That has not been 
            this.Btn_Rpt.Enabled = false;
            this.Btn_Save.Enabled = false;
          //  this.Btn_Attachment.Enabled = false;

            // Process button 
            this.Btn_EmpWorks.Enabled = false;
            this.Btn_Runing.Enabled = false;
            this.Btn_View.Enabled = false;

            this.Btn_Login.Label = " Log in ";

            if (string.IsNullOrEmpty(WebUser.No)==false)
            {
                this.Btn_Start.Enabled = true;
                this.Btn_EmpWorks.Enabled = true;
                this.Btn_Runing.Enabled = true;
                this.Btn_View.Enabled = true;

                this.Btn_Login.Label = " Replacing user ";

                if (WebUser.CurrentNode != null)
                {
                    this.Btn_Del.Enabled = WebUser.CurrentNode.DelEnable.HasValue && WebUser.CurrentNode.DelEnable.Value;
                    this.Btn_Send.Enabled = WebUser.CurrentNode.SendEnable.HasValue &&
                                            WebUser.CurrentNode.SendEnable.Value;
                    this.Btn_Return.Enabled = WebUser.CurrentNode.ReturnRole != WorkFlow.ReturnRoleKind.UnEnable;
                    this.Btn_FW.Enabled = WebUser.CurrentNode.ShiftEnable.HasValue &&
                                          WebUser.CurrentNode.ShiftEnable.Value;
                    this.Btn_Rpt.Enabled = WebUser.CurrentNode.TrackEnable.HasValue &&
                                           WebUser.CurrentNode.TrackEnable.Value;
                    this.Btn_Save.Enabled = WebUser.CurrentNode.SaveEnable.HasValue &&
                                            WebUser.CurrentNode.SaveEnable.Value;
                   // this.Btn_Attachment.Enabled = WebUser.CurrentNode.FJOpen != WorkFlow.AttachmentRoleKind.Close;
                    this.Btn_UnSend.Enabled = WebUser.HisWork != null && WebUser.HisWork.HisWFState == WFState.Complete;
                }
            }
        }
        #endregion

        #region RibbonBtn
        /// <summary>
        ///  Process track 
        /// </summary>
        public RibbonButton Btn_Rpt
        {
            get
            {
                return this.GetBtn("Btn_Truck");
            }
        }

        /// <summary>
        ///  Undo Send 
        /// </summary>
        public RibbonButton Btn_UnSend
        {
            get
            {
                return this.GetBtn("Btn_UnSend");
            }
        }

        /// <summary>
        ///  Delete Process 
        /// </summary>
        public RibbonButton Btn_Del
        {
            get
            {
                return this.GetBtn("Btn_Del");
            }
        }

        /// <summary>
        ///  Be office paper 
        /// </summary>
        public RibbonButton Btn_EmpWorks
        {
            get
            {
                return this.GetBtn("Btn_EmpWorks");
            }
        }

        /// <summary>
        ///  In-transit documents 
        /// </summary>
        public RibbonButton Btn_Runing
        {
            get
            {
                return this.GetBtn("Btn_Runing");
            }
        }

        /// <summary>
        ///  Official inquiry 
        /// </summary>
        public RibbonButton Btn_View
        {
            get
            {
                return this.GetBtn("Btn_View");
            }
        }

        /// <summary>
        ///  Accessory 
        /// </summary>
        public RibbonButton Btn_Attachment_del
        {
            get
            {
                return this.GetBtn("Btn_Ath");
            }
        }

        /// <summary>
        ///  Log in / Replacing user 
        /// </summary>
        public RibbonButton Btn_Login
        {
            get
            {
                return this.GetBtn("Btn_Login");
            }
        }

        /// <summary>
        ///  Write off 
        /// </summary>
        public RibbonButton Btn_LogOut
        {
            get
            {
                return this.GetBtn("Btn_LogOut");
            }
        }

        /// <summary>
        ///  Return 
        /// </summary>
        public RibbonButton Btn_Return
        {
            get
            {
                return this.GetBtn("Btn_Return");
            }
        }

        /// <summary>
        ///  Transfer 
        /// </summary>
        public RibbonButton Btn_FW
        {
            get
            {
                return this.GetBtn("Btn_FW");
            }
        }

        /// <summary>
        ///  Send 
        /// </summary>
        public RibbonButton Btn_Send
        {
            get
            {
                return this.GetBtn("Btn_Send");
            }
        }

        /// <summary>
        ///  Save to Network 
        /// </summary>
        public RibbonButton Btn_Save
        {
            get
            {
                return this.GetBtn("Btn_Save");
            }
        }

        /// <summary>
        ///  Send e-mail 
        /// </summary>
        public RibbonButton Btn_SendToMail
        {
            get
            {
                return this.GetBtn("Btn_SendToMail");
            }
        }

        /// <summary>
        ///  Save as 
        /// </summary>
        public RibbonButton Btn_SaveAs
        {
            get
            {
                return this.GetBtn("Btn_SaveAs");
            }
        }

        /// <summary>
        ///  Save as PDF
        /// </summary>
        public RibbonButton Btn_SaveAsPDF
        {
            get
            {
                return this.GetBtn("Btn_SaveAsPDF");
            }
        }

        /// <summary>
        ///  Prepare documents 
        /// </summary>
        public RibbonButton Btn_Start
        {
            get
            {
                return this.GetBtn("Btn_Start");
            }
        }

        /// <summary>
        ///  Send to U盘
        /// </summary>
        public RibbonButton Btn_SaveToU
        {
            get
            {
                return this.GetBtn("Btn_SaveToU");
            }
        }

        #endregion


        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        ///  Clean up all the resources being used .
        /// </summary>
        /// <param name="disposing"> If managed resources should be released ,为 true; Otherwise,  false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region  Component Designer generated code 

        /// <summary>
        ///  Required method for Designer support  -  Do not 
        ///  Using the code editor to modify the contents of this method .
        /// </summary>
        private void InitializeComponent()
        {
            //if (BP.Web.WebUser.LoadProfile() == false)
            //{
            //    FrmLogin lg = new FrmLogin();
            //    DialogResult dl = lg.ShowDialog();
            //    if (dl != DialogResult.OK)
            //        return;
            //}

            try
            {
                this.LoadXml();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            BP.Web.WebUser.HisRib = this;
            //  CCFlowWord2007.Globals.ThisAddIn.HisRibbon1 = this;
            return;
        }

        #endregion

        //internal RibbonTab tab1;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
