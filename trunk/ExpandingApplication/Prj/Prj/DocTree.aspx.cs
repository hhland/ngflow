using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Port;
using BP.Web;
using BP.DA;
using BP.En;
using BP.WF;
using BP.PRJ;

public partial class ExpandingApplication_PRJ_DocTree : WebPage
{
    public string FK_Prj
    {
        get
        {
            string s= this.Request.QueryString["FK_Prj"];
            if (s == null)
                s = "0001";
            return s;
        }
    }
    public string FK_Node
    {
        get
        {
            string s = this.Request.QueryString["FK_Node"];
            if (s == null)
                s = "401";
            return s;
        }
    }
    public int IDX
    {
        get
        {
            return int.Parse(this.Request.QueryString["IDX"]);
        }
    }
    /// <summary>
    ///  Upload 
    /// </summary>
    public void BindUpload()
    {
        this.Title = " File upload ";
        this.Pub1.AddFieldSet(" File ");
        this.Pub1.AddB(" Select the native file :<br>");
        FileUpload fu = new FileUpload();
        fu.ID = "file";
        this.Pub1.Add(fu);

        this.Pub1.AddBR();
        Button btn = new Button();
        btn.ID = "Btn";
        btn.Text = " Upload ";
        btn.Click += new EventHandler(btn_Upload_Click);
        this.Pub1.Add(btn);
        this.Pub1.AddFieldSetEnd();
    }
    public void DoDelete()
    {
        Prj prj = new Prj(this.FK_Prj);
        AtPara ap = new AtPara(prj.Files);
        string file = ap.GetValStrByKey(this.IDX.ToString());
        File.Delete(file);
        this.WinClose();
    }
    public void DoDown()
    {
        Prj prj = new Prj(this.FK_Prj);
        AtPara ap = new AtPara(prj.Files);
        string file = ap.GetValStrByKey(this.IDX.ToString());
        FileInfo fi = new FileInfo(file);
        BP.PubClass.DownloadFile(fi.FullName, fi.Name);
        this.WinClose();
    }
    void btn_Upload_Click(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.FileUpload fu = this.Pub1.FindControl("file") as System.Web.UI.WebControls.FileUpload;
        if (fu.HasFile == false || fu.FileName.Length <= 2)
        {
            this.Alert(" Please select a file to upload .");
            return;
        }

        Prj prj = new Prj(this.FK_Prj);
        AtPara ap = new AtPara(prj.Files);
        string file = ap.GetValStrByKey(this.IDX.ToString());
        try
        {
            fu.SaveAs(file);
        }
        catch
        {
            string root = BP.SystemConfig.PathOfDataUser + "\\PrjData\\Templete\\" + this.FK_Prj;
            string rootData = BP.SystemConfig.PathOfDataUser + "\\PrjData\\Data\\" + this.FK_Prj;
            if (Directory.Exists(rootData) == false)
                Directory.CreateDirectory(rootData);
            string[] strs = System.IO.Directory.GetDirectories(root);
            foreach (string str in strs)
            {
                DirectoryInfo info = new DirectoryInfo(str);
                if (Directory.Exists(rootData + "\\" + info.Name) == false)
                    Directory.CreateDirectory(rootData + "\\" + info.Name);
            }
            fu.SaveAs(file);
        }
        this.WinClose();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.DoType)
        {
            case "Upload":
                this.BindUpload();
                return;
            case "DoDown":
                this.DoDown();
                return;
            case "DoDelete":
                this.DoDelete();
                return;
            default:
                break;
        }

        NodeAccesss nrs = new NodeAccesss();
        nrs.Retrieve(NodeAccessAttr.FK_Node, this.FK_Node, NodeAccessAttr.FK_Prj, this.FK_Prj);

        string root = BP.SystemConfig.PathOfDataUser + "\\PrjData\\Templete\\" + this.FK_Prj;
        string rootData = BP.SystemConfig.PathOfDataUser + "\\PrjData\\Data\\" + this.FK_Prj;
        Prj prj = new Prj(this.FK_Prj);
        this.Pub1.AddTable();
        this.Pub1.AddCaptionLeft(" Project directory tree :"+prj.Name);
        this.Pub1.AddTR();
        this.Pub1.AddTDTitle("IDX");
        this.Pub1.AddTDTitle(" Table of Contents ");
        this.Pub1.AddTDTitle(" File ");
        this.Pub1.AddTDTitle(" Operating ");
        this.Pub1.AddTREnd();

        string MyFiles = "";
        int idx=1;
        string[] strs = System.IO.Directory.GetDirectories(root);
        foreach (string str in strs)
        {
            DirectoryInfo dir = new DirectoryInfo(str);
            FileInfo[] fils = dir.GetFiles();
            this.Pub1.AddTR();
            this.Pub1.AddTDIdx(idx++);
            this.Pub1.AddTD("<img src='./../Images/Btn/open.gif'>" + dir.Name);
            this.Pub1.AddTD(" File :" + fils.Length);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();
            foreach (FileInfo info in fils)
            {
                NodeAccess nr = nrs.GetEntityByKey(NodeAccessAttr.FileFullName, info.FullName) as NodeAccess;
                if (nr == null)
                    nr = new NodeAccess();

                if (nr.IsView == false)
                {
                    idx++;
                    continue;
                }


                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx++);
                this.Pub1.AddTD();
                this.Pub1.AddTDB("<img src='./../Images/FileType/" + info.Extension.Replace(".", "") + ".gif'>" + info.Name);

                string relFile = rootData + "\\" + dir.Name + "\\" + info.Name;
                MyFiles += "@" + idx + "=" + relFile;

                if (File.Exists(relFile) == true)
                {
                    string msg = "";
                    if (nr.IsDown)
                        msg += "[<a href=\"javascript:DoDown('" + this.FK_Prj + "','" + idx + "');\" ><img src='../Images/Btn/Save.gif' border=0/> Download </a>]";

                    if (nr.IsDelete)
                        msg += "[<a href=\"javascript:DoDelete('" + this.FK_Prj + "','" + idx + "');\" ><img src='../Images/Btn/Delete.gif' border=0/> Delete </a>]";

                    if (nr.IsUpload)
                        msg += "[<a href=\"javascript:Upload('" + this.FK_Prj + "','" + idx + "');\" ><img src='../Images/Btn/ApplyTask.gif' border=0/> Re-upload </a>]";
                    this.Pub1.AddTD(msg);
                }
                else
                {
                    if (nr.IsUpload)
                        this.Pub1.AddTD("<a href=\"javascript:Upload('" + this.FK_Prj + "','" + idx + "')\"><img src='../Images/Btn/ApplyTask.gif' border=0/> Upload </a>");
                    else
                        this.Pub1.AddTD();
                }
                this.Pub1.AddTREnd();
            }
        }
        this.Pub1.AddTableEnd();

        prj.Files = MyFiles;
        prj.Update();
    }
}