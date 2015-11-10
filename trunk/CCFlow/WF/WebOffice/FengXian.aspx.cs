using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.Web;
using BP.Web.Controls;
using BP.WF;
using BP.WF.Template;
using BP.WF.Data;
using Silverlight.Controls;
using Microsoft.Office.Interop.Word;

namespace CCFlow.WF.WebOffice
{
    public partial class FengXian : System.Web.UI.Page
    {
        #region  Property 

        public int FK_Node
        {
            get
            {
                try
                {
                    return int.Parse(Request["FK_Node"]);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public int FID
        {
            get
            {
                try
                {
                    return int.Parse(Request["FID"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public Int64 WorkID
        {
            get
            {
                try
                {
                    return int.Parse(Request["WorkID"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public string FK_Flow
        {
            get { return Request["FK_Flow"]; }
        }

        public string UserName
        {
            get
            {
                if (BP.Web.WebUser.No == "Guest")
                {
                    return BP.Web.GuestUser.Name;
                }
                else
                {
                    return BP.Web.WebUser.Name;
                }
            }
        }

        private string heBing = "";

        public string HeBing
        {
            get { return heBing; }
            set { heBing = value; }
        }
        private bool isReadOnly = false;
        public bool ReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = true; }
        }

        private bool isCheckInfo = false;

        public bool IsCheckInfo
        {
            get { return isCheckInfo; }
            set { isCheckInfo = value; }
        }

        public string NodeInfo
        {
            get
            {
                BP.WF.Node node = new BP.WF.Node(this.FK_Node);
                return node.Name + ":" + WebUser.Name + "   Date :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private bool isSavePDF = false;

        public bool IsSavePDF
        {
            get { return isSavePDF; }
            set { isSavePDF = value; }
        }
        private bool _isMarks = false;

        public bool IsMarks
        {
            get { return _isMarks; }
            set { _isMarks = value; }
        }

        private string _officeTemplate = null;

        public string OfficeTemplate
        {
            get { return _officeTemplate; }
            set { _officeTemplate = value; }
        }

        private bool _isLoadTempLate = false;

        public bool IsLoadTempLate
        {
            get { return _isLoadTempLate; }
            set { _isLoadTempLate = value; }
        }

        public string CCFlowAppPath = BP.WF.Glo.CCFlowAppPath;

        #endregion

        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FK_Node.ToString()) || string.IsNullOrEmpty(WorkID.ToString()))
            {
                divMenu.InnerHtml = "<h1 style='color:red'> Incoming parameter error !<h1>";
                return;
            }

            if (!IsPostBack)
            {
                string type = Request["action"];
                if (string.IsNullOrEmpty(type))
                {
                    LoadMenu(true);
                    ReadFile();
                }
                else
                {
                    LoadMenu(false);
                    if (type.Equals("LoadFile"))
                        LoadFile();
                    else if (type.Equals("SaveFile"))
                        SaveFile();
                    else
                        throw new Exception(" Incoming parameter is incorrect !");
                }
            }
        }

        private void ReadFile()
        {
            string path = Server.MapPath("~/DataUser/FengXianFile/" + FK_Flow + "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string[] files = Directory.GetFiles(path);
            bool isHave = false;
            BP.WF.Node node = new BP.WF.Node(FK_Node);




            string fileStart = WorkID.ToString();
            if (node.HisNodeWorkType == NodeWorkType.SubThreadWork)
            {
                fileStart = FID.ToString();
            }

            try
            {
                WorkFlow workflow = new WorkFlow(this.FK_Flow, this.WorkID);

                if (workflow.HisGenerWorkFlow.PWorkID != 0)
                {
                    BtnLab btnLab = new BtnLab(this.FK_Node);
                    if (btnLab.OfficeIsParent)
                        fileStart = workflow.HisGenerWorkFlow.PWorkID.ToString();
                }


            }
            catch (Exception)
            {


            }

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Name.StartsWith(fileStart))
                {
                    fileName.Text = fileInfo.Name;
                    fileType.Text = fileInfo.Extension.TrimStart('.');
                    isHave = true;
                    break;
                }
            }
            if (!isHave)
            {
                if (node.IsStartNode)
                {
                    if (!string.IsNullOrEmpty(OfficeTemplate))
                    {
                        fileName.Text = "/" + OfficeTemplate;
                        fileType.Text = OfficeTemplate.Split('.')[1];
                        IsLoadTempLate = true;
                    }
                }

                //if (node.HisNodeWorkType == NodeWorkType.SubThreadWork)
                //{
                //    File.Exists(path+)
                //    foreach (string file in files)
                //    {
                //        FileInfo fileInfo = new FileInfo(file);
                //        if (fileInfo.Name.StartsWith(this.FID.ToString()))
                //        {
                //            fileInfo.CopyTo(path + "\\" + this.WorkID + fileInfo.Extension);
                //            fileName.Text = this.WorkID + fileInfo.Extension;
                //            fileType.Text = fileInfo.Extension.TrimStart('.');
                //            break;
                //        }
                //    }
                //}
            }
            else
            {

                //    if (node.HisNodeWorkType == NodeWorkType.WorkHL || node.HisNodeWorkType == NodeWorkType.WorkFHL)
                //    {

                //        GenerWorkFlows generWorksFlows = new GenerWorkFlows();
                //        generWorksFlows.RetrieveByAttr(GenerWorkFlowAttr.FID, this.WorkID);
                //        string tempH = "";
                //        foreach (GenerWorkFlow generWork in generWorksFlows)
                //        {
                //            tempH += generWork.WorkID + ",";
                //        }
                //        HeBing = tempH.TrimEnd(',');
                //    }
            }
        }

        private void LoadFile()
        {
            try
            {
                string loadType = Request["LoadType"];
                string type = fileType.Text;
                string name = getUTF8ToString("fileName");
                string path = null;
                if (loadType.Equals("1"))
                {
                    path = Server.MapPath("~/DataUser/FengXianFile/" + FK_Flow + "/" + name);
                }
                else
                {
                    path = Server.MapPath("~/DataUser/OfficeTemplate/" + name);
                }

                var result = File.ReadAllBytes(path);

                Response.Clear();

                Response.BinaryWrite(result);
                Response.End();
            }
            catch (Exception)
            {

                throw;
            }

        }
 

        private void LoadMenu(bool isMenu)
        {
            BtnLab btnLab = new BtnLab(this.FK_Node);
            bool isCompleate = false;
            BP.WF.Node node = new BP.WF.Node(FK_Node);
            try
            {
                WorkFlow workFlow = new WorkFlow(node.FK_Flow, WorkID);
                isCompleate = workFlow.IsComplete;

            }
            catch (Exception)
            {
                try
                {
                    Flow fl = new Flow(node.FK_Flow);
                    GERpt rpt = fl.HisGERpt;
                    rpt.OID = WorkID;
                    rpt.Retrieve();

                    if (rpt != null)
                    {
                        if (rpt.WFState == WFState.Complete)
                            isCompleate = true;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            if (!isCompleate)
            {
                try
                {
                    isCompleate = !BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(node.FK_Flow, this.FK_Node, this.WorkID, WebUser.No);
                    //WorkFlow workFlow = new WorkFlow(node.FK_Flow, WorkID);
                    //isCompleate = !workFlow.IsCanDoCurrentWork(WebUser.No);
                }
                catch (Exception ex)
                {

                }

            }

            if (isMenu && !isCompleate)
            {
               
                if (btnLab.OfficeOpenEnable)
                {

                    AddBtn("openFile", btnLab.OfficeOpenLab, "OpenFile");

                }
                if (btnLab.OfficeOpenTemplateEnable)
                {
                    AddBtn("openTempLate", btnLab.OfficeOpenTemplateLab, "OpenTempLate");

                }
                if (btnLab.OfficeSaveEnable)
                {

                    AddBtn("saveFile", btnLab.OfficeSaveLab, "saveOffice");

                }


                if (btnLab.OfficePrintEnable)
                {

                    AddBtn("print", btnLab.OfficePrintLab, "printOffice");

                }

            
                if (btnLab.OfficeIsDown)
                {
                    AddBtn("download", btnLab.OfficeDownLab, "DownLoad");

                }
            }
            OfficeTemplate = btnLab.OfficeFengXianTemplate;

            if (!isCompleate)
            {
                if (btnLab.OfficeReadOnly)
                {
                    ReadOnly = true;
                }

              
                if (btnLab.OfficeReSavePDF)
                {
                    IsSavePDF = true;
                }

                IsMarks = btnLab.OfficeIsMarks;
            }
            else
            {
                ReadOnly = true;
            }
        }

        private void AddBtn(string id, string label, string clickEvent)
        {
            Btn btn = new Btn();
            btn.ID = id;
            btn.Text = label;
            btn.Attributes["onclick"] = "return " + clickEvent + "()";
            btn.Attributes["class"] = "btn";
            divMenu.Controls.Add(btn);
        }


        private void SaveFile()
        {
            try
            {
                HttpFileCollection files = HttpContext.Current.Request.Files;

                BP.WF.Node node = new BP.WF.Node(FK_Node);

                string fileStart = WorkID.ToString();
                if (node.HisNodeWorkType == NodeWorkType.SubThreadWork)
                {
                    fileStart = FID.ToString();
                }

                //string file = Request["Path"];
                //file = HttpUtility.UrlDecode(file, Encoding.UTF8);

                if (files.Count > 0)
                {
                    ///' Check the file name extension 
                    HttpPostedFile postedFile = files[0];
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    string path = Server.MapPath("~/DataUser/FengXianFile/" + FK_Flow + "/");

                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);

                        postedFile.SaveAs(path + "\\" + fileStart + fileExtension);
                    }
                    //try
                    //{


                    //    Microsoft.Office.Interop.Word.Application appClass = new Microsoft.Office.Interop.Word.Application();
                    //    appClass.Visible = false;

                    //    Object missing = System.Reflection.Missing.Value;

                    //    object fileNameR = path + "\\" + fileStart + fileExtension;
                    //    var wordDoc = appClass.Documents.Open(ref fileNameR, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);


                    //    object format = WdSaveFormat.wdFormatPDF;
                    //    object savePath = path + "\\" + fileStart + ".pdf";
                    //    wordDoc.SaveAs(ref savePath, ref format, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
                    //    wordDoc.Close();

                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}