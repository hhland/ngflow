using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Web.Controls;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.Web.Comm.UC;

namespace BP.Web.Comm
{
    public enum FJShowType
    {
        List,
        UploadM,
        UplaodAttrFile
    }
	/// <summary>
	/// FileManager 的摘要说明。
	/// </summary>
    public partial class FileManager : WebPage
    {
        public FJShowType FJShowType
        {
            get
            {
                switch (this.Request.QueryString["ShowType"])
                {
                    case "1":
                        return FJShowType.UploadM;
                    case "2":
                        return FJShowType.UplaodAttrFile;
                        break;
                    case "0":
                    default:
                        return FJShowType.List;
                        break;
                }
            }
        }
        public new string RefPK
        {
            get
            {
                string s = this.Request.QueryString["MyPK"];
                if (s == null || s == string.Empty)
                    s = this.Request.QueryString["PK"];

                if (s == null || s == string.Empty)
                    s = this.Request.QueryString["No"];
                if (s == null || s == string.Empty)
                    s = this.Request.QueryString["OID"];



                if (s == null || s == string.Empty)
                    s = this.Request.QueryString["RefOID"];

                return s;
            }
        }
        public string Caption = null;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Entity en1 = ClassFactory.GetEn(this.EnName);

            string caption = "";
            caption += "<a href='UIEn.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "' >返回</a>";
            caption += "&nbsp;&nbsp;<a href='FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "' >列表</a>";

            Attr attr = null;
            try
            {
                attr = en1.EnMap.GetAttrByKey("MyFileNum");
            }
            catch
            {
            }

            if (attr != null)
                caption += "&nbsp;&nbsp;<a href='FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&ShowType=1' >批量上传:" + attr.Desc + "</a>";

            if (en1.EnMap.HisAttrFiles.Count > 1)
                caption += "&nbsp;&nbsp;<a href='FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&ShowType=2' >指定上传</a>";

            this.Caption = caption;
            //this.Title.Add(this.GenerLabelStr(caption));
            if (this.Request.QueryString["DelPK"] != null)
            {
                SysFileManager SysFileManager = new SysFileManager();
                SysFileManager.OID = int.Parse(this.Request.QueryString["DelPK"]);
                SysFileManager.DirectDelete();
                en1.PKVal = this.RefPK;
                en1.Retrieve();
                try
                {
                    en1.Update("MyFileNum", en1.GetValIntByKey("MyFileNum") - 1);
                }
                catch
                {
                }
                this.Response.Redirect("FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&ShowType=" + this.Request.QueryString["ShowType"], true);
            }

            switch (this.FJShowType)
            {
                case FJShowType.UploadM:
                    BindUploadM();
                    break;
                case FJShowType.UplaodAttrFile:
                    UplaodAttrFile();
                    break;
                default:
                    this.SetDGData();
                    break;
            }
        }
        public void BindUploadM()
        {
            this.UCSys1.AddTable();
            this.UCSys1.AddCaptionLeft(Caption);
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitle("IDX");
            this.UCSys1.AddTDTitle("描述");
            this.UCSys1.AddTDTitle("文件");
            this.UCSys1.AddTREnd();

            for (int i = 1; i < 10; i++)
            {
                this.UCSys1.AddTR();
                this.UCSys1.AddTDIdx(i);
                BP.Web.Controls.TB tb = new TB();
                tb.ID = "TB_" + i;
                this.UCSys1.AddTD(tb);
                HtmlInputFile file = new HtmlInputFile();
                file.ID = "F" + i;
                this.UCSys1.AddTD(file);
                this.UCSys1.AddTREnd();
            }
            this.UCSys1.AddTRSum();
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.Text = " 上 传 ";
            btn.Click += new EventHandler(btn_Click);
            this.UCSys1.AddTD("colspan=3", btn);
            this.UCSys1.AddTREnd();

            this.UCSys1.AddTableEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 10; i++)
            {
                HtmlInputFile file = (HtmlInputFile)this.UCSys1.FindControl("F" + i);
                if (file.Value.Contains(".") == false)
                    continue;

                this.FileSave(this.UCSys1.GetTBByID("TB_" + i).Text, file);
            }

            SysFileManagers sfs = new SysFileManagers(this.EnName, this.RefPK);
            Entity en1 = ClassFactory.GetEn(this.EnName);
            en1.PKVal = this.RefPK;
            en1.Retrieve();
            en1.Update("MyFileNum", sfs.Count);

            this.Response.Redirect("FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&FJShowType=1", true);
        }
        public void UplaodAttrFile()
        {
            Entity en1 = ClassFactory.GetEn(this.EnName);
            AttrFiles fils = en1.EnMap.HisAttrFiles;
            EnCfg cfg = new EnCfg(this.EnName);

            SysFileManagers sfs = new SysFileManagers(this.EnName, this.RefPK);
            this.UCSys1.AddTable();
            this.UCSys1.AddCaptionLeft(Caption);
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitle();
            this.UCSys1.AddTDTitle("描述");
            this.UCSys1.AddTDTitle("文件");
            this.UCSys1.AddTDTitle("打开");
            this.UCSys1.AddTDTitle("");

            this.UCSys1.AddTREnd();
            int i = 1;
            foreach (AttrFile fl in fils)
            {

                this.UCSys1.AddTR();
                this.UCSys1.AddTDIdx(i++);
                this.UCSys1.AddTD(fl.FileName);

                HtmlInputFile file = new HtmlInputFile();
                file.ID = fl.FileNo;

                this.UCSys1.AddTD(file);

                SysFileManager en = sfs.GetEntityByKey(SysFileManagerAttr.AttrFileNo, fl.FileNo) as SysFileManager;
                if (en == null)
                {
                    this.UCSys1.AddTD("");
                    this.UCSys1.AddTD("");
                }
                else
                {
                    this.UCSys1.AddTD("<a href='" + cfg.FJWebPath + "/" + en.OID + "." + en.MyFileExt + "' target=_blank><img src='./../Images/FileType/" + en.MyFileExt + ".gif' border=0/>" + en.MyFileName + "</a>");
                    this.UCSys1.AddTD("<a href='FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&DelPK=" + en.OID + "'><img border=0  src=../Images/Btn/Delete.gif border=0 /></a>");
                }
                this.UCSys1.AddTREnd();
            }

            this.UCSys1.AddTRSum();
            Button btn = new Button();
            btn.Text = " 上 传 ";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SaveAttrFile_Click);
            this.UCSys1.AddTD("colspan=5", btn);
            this.UCSys1.AddTREnd();
            this.UCSys1.AddTableEnd();
        }
        void btn_SaveAttrFile_Click(object sender, EventArgs e)
        {
            Entity en1 = ClassFactory.GetEn(this.EnName);
            AttrFiles fils = en1.EnMap.HisAttrFiles;
            SysFileManagers sfs = new SysFileManagers(this.EnName, this.RefPK);
            foreach (AttrFile fl in fils)
            {
                HtmlInputFile file = (HtmlInputFile)this.UCSys1.FindControl(fl.FileNo);
                if (file.Value.Contains(".") == false)
                    continue;

                SysFileManager en = sfs.GetEntityByKey(SysFileManagerAttr.AttrFileNo, fl.FileNo) as SysFileManager;
                SysFileManager enN = null;
                if (en == null)
                {
                    enN = this.FileSave(null, file);
                }
                else
                {
                    enN = this.FileSave(null, file);
                    en.DirectDelete();
                }

                enN.AttrFileNo = fl.FileNo;
                enN.AttrFileName = fl.FileName;
                enN.Update();
            }
            this.Response.Redirect("FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&ShowType=2", true);
        }
        #region 处理
        private void SetDGData()
        {
            SysFileManagers ens = new SysFileManagers();
            ens.Retrieve(SysFileManagerAttr.EnName, this.EnName, SysFileManagerAttr.RefVal, this.RefPK);
            this.UCSys1.Clear();
            this.UCSys1.AddTable();
            this.UCSys1.AddCaptionLeft(this.Caption);
            this.UCSys1.AddTR();
            this.UCSys1.AddTDTitle("类型");
            this.UCSys1.AddTDTitle("文件名称");
            this.UCSys1.AddTDTitle("大小MB");
            this.UCSys1.AddTDTitle("日期");
            this.UCSys1.AddTDTitle("高度px");
            this.UCSys1.AddTDTitle("宽度px");
            this.UCSys1.AddTDTitle("标记");
            this.UCSys1.AddTDTitle("操作");
            this.UCSys1.AddTREnd();
            EnCfg cfg = new EnCfg(this.EnName);
            foreach (SysFileManager en in ens)
            {
                this.UCSys1.AddTR();
                this.UCSys1.AddTD("<img src='./../Images/FileType/" + en.MyFileExt + ".gif' />");
                this.UCSys1.AddTD("<a href='" + cfg.FJWebPath + "/" + en.OID + "." + en.MyFileExt + "' target=_blank>" + en.MyFileName + "</a>");
                this.UCSys1.AddTD(en.MyFileSize);
                this.UCSys1.AddTD(en.RDT);
                this.UCSys1.AddTD(en.MyFileH);
                this.UCSys1.AddTD(en.MyFileW);
                this.UCSys1.AddTD(en.AttrFileName);
                this.UCSys1.AddTD("<a href='FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&DelPK=" + en.OID + "'><img src=../Images/Btn/Delete.gif border=0 />删除</a>");
                // this.UCSys1.AddTD("<a href=\"javascript:DoAction('FileManager.aspx?EnName=" + this.EnName + "&PK=" + this.RefPK + "&DelPK=" + en.OID + "','删除文件')\">删除</a></a>");
                this.UCSys1.AddTREnd();
            }
            this.UCSys1.AddTableEnd();
        }
        private SysFileManager FileSave(string fileNameDesc, HtmlInputFile File1)
        {
            SysFileManager en = new SysFileManager();

            en.EnName = this.EnName;
            // en.FileID = this.RefPK + "_" + count.ToString();
            EnCfg cfg = new EnCfg(this.EnName);

            string filePath = cfg.FJSavePath; // BP.Sys.SystemConfig.PathOfFDB + "\\" + this.EnName + "\\";
            if (System.IO.Directory.Exists(filePath) == false)
                System.IO.Directory.CreateDirectory(filePath);

            string ext = System.IO.Path.GetExtension(File1.PostedFile.FileName);
            ext = ext.Replace(".", "");
            en.MyFileExt = ext;
            if (fileNameDesc == "" || fileNameDesc == null)
                en.MyFileName = System.IO.Path.GetFileNameWithoutExtension(File1.PostedFile.FileName);
            else
                en.MyFileName = fileNameDesc;
            en.RDT = DataType.CurrentData;
            en.RefVal = this.RefPK;
            en.MyFilePath = filePath;
            en.Insert();


            string fileName = filePath + en.OID + "." + en.MyFileExt;
            File1.PostedFile.SaveAs(fileName);

            File1.PostedFile.InputStream.Close();
            File1.PostedFile.InputStream.Dispose();
            File1.Dispose();

            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            en.MyFileSize = DataType.PraseToMB(fi.Length);

            if (DataType.IsImgExt(en.MyFileExt))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);
                en.MyFileH = img.Height;
                en.MyFileW = img.Width;
                img.Dispose();
            }
            en.WebPath = cfg.FJWebPath + en.OID + "." + en.MyFileExt;
            en.Update();
            return en;
        }
        #endregion

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方  法 - 不要使用代码编辑器修改
        /// 此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

    }
}
