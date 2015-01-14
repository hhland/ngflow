using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Sys;
using BP.Web.Controls;
using BP.Web;
using BP.WF;
using BP.WF.Data;

namespace CCFlow.WF.WebOffice
{
    public partial class AttachOffice : System.Web.UI.Page
    {
        #region  Property 
        /// <summary>
        /// OID Serial number 
        /// </summary>
        public string PKVal
        {
            get
            {
                return this.Request.QueryString["PKVal"];
            }
        }
        /// <summary>
        ///  Annex data number primary key 
        /// </summary>
        public string DelPKVal
        {
            get
            {
                return this.Request.QueryString["DelPKVal"];
            }
        }
        /// <summary>
        ///  Design forms the primary key 
        /// </summary>
        public string FK_FrmAttachment
        {
            get
            {
                return this.Request.QueryString["FK_FrmAttachment"];
            }
        }

        public string DoType
        {
            get { return this.Request.QueryString["DoType"]; }
        }
        /// <summary>
        ///  Node number 
        /// </summary>
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        /// <summary>
        ///  Attachment Name 
        /// </summary>
        public string NoOfObj
        {
            get
            {
                return this.Request.QueryString["NoOfObj"];
            }
        }
        /// <summary>
        ///  Form Number 
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        /// <summary>
        /// Username  
        /// </summary>
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

        private bool isReadOnly = false;
        public bool ReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = true; }
        }

        private string fileSavePath;

        public string FileSavePath
        {
            get { return fileSavePath; }
            set { fileSavePath = value; }
        }

        private string _realFileName;

        public string RealFileName
        {
            get { return _realFileName; }
            set { _realFileName = value; }
        }

        private string _fileFullName;

        public string FileFullName
        {
            get { return _fileFullName; }
            set { _fileFullName = value; }
        }

      
        public string NodeInfo
        {
            get
            {
                BP.WF.Node nodeInfo = new BP.WF.Node(FK_Node);

                return nodeInfo.Name + ": " + WebUser.Name + "     Time :" + DateTime.Now.ToString("YYYY-MM-dd HH:mm:ss");

            }
        }

        private bool _isCheck = false;
        public bool IsCheck
        {
            get { return _isCheck; }
            set { _isCheck = value; }
        }

        private bool _isSavePDF = false;

        public bool IsSavePDF
        {
            get { return _isSavePDF; }
            set { _isSavePDF = value; }
        }

        private bool _isMarks = false;

        public bool IsMarks
        {
            get { return _isMarks; }
            set { _isMarks = value; }
        }

 
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FK_MapData) || string.IsNullOrEmpty(FK_FrmAttachment))
            {
                divMenu.InnerHtml = "<h1 style='color:red'> Incoming parameter error !<h1>";
                return;
            }

            if (!IsPostBack)
            {
                string type = Request["action"];
                if (string.IsNullOrEmpty(type))
                {
                    InitOffice(true);
                }
                else
                {
                    InitOffice(false);
                    if (type.Equals("LoadFile"))
                        LoadFile();
                    else if (type.Equals("SaveFile"))
                        SaveFile();
                    else
                        throw new Exception(" Incoming parameter is incorrect !");
                }
            }
        }

        private void InitOffice(bool isMenu)
        {
            bool isCompleate = false;
            BP.WF.Node node = new BP.WF.Node(FK_Node);
            try
            {
            

                WorkFlow workFlow = new WorkFlow(node.FK_Flow, Int64.Parse(PKVal));
                isCompleate = workFlow.IsComplete;
            }
            catch (Exception)
            {
                try
                {
                    Flow fl = new Flow(node.FK_Flow);
                    GERpt rpt = fl.HisGERpt;
                    rpt.OID = Int64.Parse(PKVal);
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
                    isCompleate = !BP.WF.Dev2Interface.Flow_IsCanDoCurrentWork(node.FK_Flow, node.NodeID, Int64.Parse(PKVal), WebUser.No);
                    //WorkFlow workFlow = new WorkFlow(node.FK_Flow, Int64.Parse(PKVal));
                    //isCompleate = !workFlow.IsCanDoCurrentWork(WebUser.No);
                }
                catch (Exception ex)
                {
                    
                }
           
            }

            FrmAttachment attachment = new FrmAttachment();
            int result = 0;
            // Form number and node is not empty 
            if (this.FK_MapData != null && this.FK_Node != null)
            {
                BP.En.QueryObject objInfo = new BP.En.QueryObject(attachment);
                objInfo.AddWhere(FrmAttachmentAttr.FK_MapData, this.FK_MapData);
                objInfo.addAnd();
                objInfo.AddWhere(FrmAttachmentAttr.FK_Node, this.FK_Node);
                objInfo.addAnd();
                objInfo.AddWhere(FrmAttachmentAttr.NoOfObj, this.NoOfObj);
                result = objInfo.DoQuery();
                //result = attachment.Retrieve(FrmAttachmentAttr.FK_MapData, this.FK_MapData,
                //                            FrmAttachmentAttr.FK_Node, this.FK_Node, FrmAttachmentAttr.NoOfObj, this.DelPKVal);
            }
            if (result == 0) /* If not defined , To get the default .*/
            {
                attachment.MyPK = this.FK_FrmAttachment;
                attachment.Retrieve();
            }
            if (!isCompleate)
            {
                if (attachment.IsWoEnableReadonly)
                {
                    ReadOnly = true;
                }
                if (attachment.IsWoEnableCheck)
                {
                    IsCheck = true;
                }
                IsMarks = attachment.IsWoEnableMarks;
 

            }
            else
            {
                ReadOnly = true;
            }
            if (isMenu && !isCompleate)
            {
                #region  Initialization button 
                if (attachment.IsWoEnableViewKeepMark)
                {
                    divMenu.InnerHtml = "<select id='marks' onchange='ShowUserName()'  style='width: 100px'><option value='1'> Whole </option><select>";
                }

                if (attachment.IsWoEnableTemplete)
                {
                    AddBtn("openTempLate", " Open the template ", "OpenTempLate");
                }
                if (attachment.IsWoEnableSave)
                {
                    AddBtn("saveFile", " Save ", "saveOffice");
                }
                if (attachment.IsWoEnableRevise)
                {
                    AddBtn("accept", " Accept Change ", "acceptOffice");
                    AddBtn("refuse", " Reject Changes ", "refuseOffice");
                }

                if (attachment.IsWoEnableOver)
                {
                    AddBtn("over", " Tao Hong file ", "overOffice");
                }

                if (attachment.IsWoEnablePrint)
                {

                    AddBtn("print", " Print ", "printOffice");

                }
                if (attachment.IsWoEnableSeal)
                {
                    AddBtn("seal", " Signature ", "sealOffice");
                }
                if (attachment.IsWoEnableInsertFlow)
                {
                    AddBtn("flow", " Insert flowchart ", "InsertFlow");
                }
                if (attachment.IsWoEnableInsertFlow)
                {
                    AddBtn("fegnxian", " Risk insertion point ", "InsertFengXian");
                }
                if (attachment.IsWoEnableDown)
                {
                    AddBtn("download", " Download ", "DownLoad");
                }
                #endregion
            }
            #region    Initialization file 

            FrmAttachmentDB downDB = new FrmAttachmentDB();

            downDB.MyPK = this.DelPKVal;
            downDB.Retrieve();
            fileName.Text = downDB.FileName;
            fileType.Text = downDB.FileExts;
            RealFileName = downDB.FileName;
            FileFullName = downDB.FileFullName;
            FileSavePath = attachment.SaveTo;

            #endregion
        }

        private void LoadFile()
        {
            try
            {
                string loadType = Request["LoadType"];
                string type = fileType.Text;
                string name = Request["fileName"];
                string path = null;
                if (loadType.Equals("1"))
                {
                    try
                    {
                        path = Server.MapPath("~/" + FileFullName);

                    }
                    catch (Exception ex)
                    {
                        path = FileFullName;
                    }
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

        private void SaveFile()
        {
            try
            {

                HttpFileCollection files = HttpContext.Current.Request.Files;


                //string file = Request["Path"];
                //file = HttpUtility.UrlDecode(file, Encoding.UTF8);

                if (files.Count > 0)
                {
                    ///' Check the file name extension 
                    HttpPostedFile postedFile = files[0];
                    string fileName, fileExtension;
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);


                    string path = "";
                    try
                    {
                        path = Server.MapPath("~/" + FileFullName);
                    }
                    catch (Exception ex)
                    {
                        path = FileFullName;

                    }
                    

                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        postedFile.SaveAs(path);
                    }
                }
            }
            catch (Exception)
            {
                throw;
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





    }
}