using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
namespace CCFlow.WF.UC
{
    public partial class ToolWap : BP.Web.UC.UCBase3
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = " Set up ";
            if (WebUser.IsWap == true && this.RefNo == null)
            {
                this.BindTools();
                return;
            }
            switch (this.RefNo)
            {
                case "AthFlows":
                    this.AthFlows();
                    break;
                case "Skin":
                    this.Skin();
                    break;
                case "MyWorks":
                    this.MyWorks();
                    break;
                case "Siganture":
                    this.Siganture();
                    break;
                case "AdminSet":
                    AdminSet();
                    break;
                case "AutoLog":
                    BindAutoLog();
                    break;
                case "Pass":
                    BindPass();
                    break;
                case "Profile":
                    BindProfile();
                    break;
                case "Auto":
                    BindAuto();
                    break;
                case "AutoDtl":
                    BindAutoDtl();
                    break;
                case "Times": //  Aging analysis 
                    BindTimes();
                    break;
                case "FtpSet": //  Aging analysis 
                    BindFtpSet();
                    break;
                case "PerPng":
                    BingPerPng();
                    break;
                case "BitmapCutter":
                    this.BitmapCutter();
                    break;
                case "Per":
                default:
                    BindPer();
                    break;
            }
        }
        public void AthFlows()
        {
            FlowSorts sorts = new FlowSorts();
            sorts.RetrieveAll();
            Flows fls = new Flows();
            fls.RetrieveAll();

            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);

            this.AddTable();
            this.AddCaptionLeft(" The scope of the authorization process ");
            this.AddTR();
            this.AddTDTitle("IDX");
            this.AddTDTitle(" Category ");
            this.AddTDTitle(" Process ");
            this.AddTREnd();
            int i = 0;
            foreach (FlowSort sort in sorts)
            {
                i++;
                this.AddTRSum();
                this.AddTDIdx(i);
                this.AddTDB(sort.Name);
                CheckBox cbAll = new CheckBox();
                cbAll.Text = " Select category All ";
                cbAll.ID = "CB_d" + sort.No;
                this.AddTD(cbAll);
                this.AddTREnd();

                string ctlIDs = "";
                foreach (Flow fl in fls)
                {
                    if (fl.FK_FlowSort != sort.No)
                        continue;

                    i++;
                    this.AddTR();
                    this.AddTDIdx(i);
                    this.AddTD("");
                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + fl.No;
                    cb.Text = fl.Name;
                    if (emp.AuthorFlows.Contains(fl.No))
                        cb.Checked = true;
                    ctlIDs += cb.ID + ",";
                    this.AddTD(cb);
                    this.AddTREnd();
                }
                cbAll.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')";
            }

            this.AddTR();
            this.AddTDTitle("");
            Button btnSaveAthFlows = new Button();
            btnSaveAthFlows.CssClass = "Btn";
            btnSaveAthFlows.ID = "Btn_Save";
            btnSaveAthFlows.Text = "Save";
            btnSaveAthFlows.Click += new EventHandler(btnSaveAthFlows_Click);
            this.Add(btnSaveAthFlows);
            this.AddTD("colspan=2", btnSaveAthFlows);
            this.AddTREnd();
            this.AddTableEnd();
        }
        void btnSaveAthFlows_Click(object sender, EventArgs e)
        {
            Flows fls = new Flows();
            fls.RetrieveAll();
            string strs = "";
            foreach (Flow fl in fls)
            {
                CheckBox check = this.GetCBByID("CB_" + fl.No);

                if (check == null)
                    continue;

                if (check.Checked == false)
                    continue;
                strs += "," + fl.No;
            }

            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
            emp.AuthorFlows = strs;
            emp.Update();

            BP.Sys.Glo.WriteUserLog("Auth", WebUser.No, " Authorize :" + strs);
            this.WinCloseWithMsg(" Saved successfully .");
        }
        public void MyWorks()
        {
            Flows fls = new Flows();
            fls.RetrieveAll();

            Nodes nds = new Nodes();
            nds.RetrieveAll();

            this.AddTable();
            this.AddTR();
            this.AddTDTitle("IDX");
            this.AddTDTitle(" Process ");
            this.AddTDTitle(" Node ");
            this.AddTDTitle(" Inquiry ");
            this.AddTREnd();

            int idx = 0;
            foreach (Flow fl in fls)
            {
                bool isHave = false;
                Nodes mynds = new Nodes();
                foreach (BP.WF.Node nd in nds)
                {
                    if (nd.FK_Flow != fl.No)
                        continue;

                    //if (nd.NodeStations.Contains(WebUser.HisStations))
                    //{
                    //    mynds.AddEntity(nd);
                    //}
                    if (nd.NodeEmps.Contains(WebUser.No))
                        mynds.AddEntity(nd);
                }

                if (mynds.Count == 0)
                    continue;


                bool isFirst = true;
                foreach (BP.WF.Node mynd in mynds)
                {
                    if (isFirst)
                        this.AddTRSum();
                    else
                        this.AddTR();

                    this.AddTDIdx(idx);
                    if (isFirst)
                        this.AddTD(mynd.FlowName);
                    else
                        this.AddTD();

                    this.AddTD(mynd.Name);

                    this.AddTD("<a href=\"javascript:WinOpen('FlowSearchSmallSingle.aspx?FK_Node=" + mynd.NodeID + "');\"> Job inquiries </a>");
                    this.AddTREnd();



                    idx++;
                    isFirst = false;
                }
            }
            this.AddTableEnd();
        }
        public void BindTools()
        {
            BP.WF.XML.Tools tools = new BP.WF.XML.Tools();
            tools.RetrieveAll();

            this.AddFieldSet("<a href='Home.aspx'><img src='../Img/Home.gif' border=0/>Home</a>");
            this.AddUL();
            foreach (BP.WF.XML.Tool tool in tools)
            {
                this.AddLi("" + this.PageID + ".aspx?RefNo=" + tool.No, tool.Name, "_self");
            }
            this.AddULEnd();
            this.AddFieldSetEnd();
        }
        public void Skin()
        {
            string pageID = this.PageID;
            string setNo = this.Request.QueryString["SetNo"];
            if (setNo != null)
            {
                BP.WF.Port.WFEmp em = new BP.WF.Port.WFEmp(BP.Web.WebUser.No);
                em.Style = setNo;
                em.Update();
                WebUser.Style = setNo;
                this.Response.Redirect(pageID + ".aspx?RefNo=Skin", true);
                return;
            }

            this.AddFieldSet(" Style settings ");

            BP.WF.XML.Skins sks = new BP.WF.XML.Skins();
            sks.RetrieveAll();

            this.AddUL();
            foreach (BP.WF.XML.Skin item in sks)
            {

                // modified by ZhouYue 2013-05-20 ignore 'if (WebUser.Style == item.No)' case.
                //if (WebUser.Style == item.No)
                //    this.AddLi(item.Name + "&nbsp;&nbsp;<span style='background:" + item.CSS + "' ><i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i></span>");
                //else
                this.AddLi(pageID + ".aspx?RefNo=Skin&SetNo=" + item.No, item.Name + "&nbsp;&nbsp;<span style='background:" + item.CSS + "' ><i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</i></span>");

                //System.Web.UI.WebControls.RadioButton rb = new RadioButton();
                //rb.ID = "RB_" + item.No;
                //rb.Text = item.Name;
                //rb.GroupName = "s";
                //if (WebUser.Style == item.No)
                //    rb.Checked=true;

                //this.Add(rb);
                //this.AddBR();
            }
            this.AddULEnd();

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            btn.Text = "Save";
            btn.Click += new EventHandler(btn_SaveSkin_Click);
            this.AddFieldSetEnd(); // (" Style settings ");
        }

        void btn_SaveSkin_Click(object sender, EventArgs e)
        {
            BP.WF.XML.Skins sks = new BP.WF.XML.Skins();
            sks.RetrieveAll();
            foreach (BP.WF.XML.Skin item in sks)
            {
                if (this.GetRadioButtonByID("RB_" + item.No).Checked)
                {
                    WebUser.Style = item.No;
                    BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
                    emp.Style = item.No;
                    emp.Update();
                    this.Response.Redirect(this.Request.RawUrl, true);
                    return;
                }
            }
        }

        public void BindFtpSet()
        {
            this.AddFieldSet("ftp setting");

            this.AddTable();
            this.AddTR();
            this.AddTDTitle();
            this.AddTDTitle();
            this.AddTDTitle();
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Username ");
            TextBox tb = new TextBox();

            tb.ID = "TB_UserNo";
            this.AddTD(tb);
            this.AddTD();
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Password ");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.Password;
            tb.ID = "TB_Pass1";
            this.AddTD(tb);
            this.AddTD();
            this.AddTREnd();

            //this.AddTR();
            //this.AddTD(" Reenter new password ");
            //tb = new TextBox();
            //tb.TextMode = TextBoxMode.Password;
            //tb.ID = "TB_Pass3";
            //this.AddTD(tb);
            //this.AddTD();
            //this.AddTREnd();


            this.AddTR();
            this.AddTD("");

            Btn btn = new Btn();
            btn.Text = " Determine ";
            btn.Click += new EventHandler(btn_Click);
            this.AddTD(btn);
            this.AddTD();
            this.AddTREnd();
            this.AddTableEnd();
            this.AddFieldSetEnd();
        }
        public void Siganture()
        {
            string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\T.JPG";
            if (this.DoType != null || System.IO.File.Exists(path) == false)
            {
                string pathMe = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + WebUser.No + ".JPG";
                File.Copy(BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\Templete.JPG",
                    path, true);

                string fontName = " Times New Roman ";
                switch (this.DoType)
                {
                    case "ST":
                        fontName = " Times New Roman ";
                        break;
                    case "LS":
                        fontName = " Clerical ";
                        break;
                    default:
                        break;
                }

                System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                Font font = new Font(fontName, 15);
                Graphics g = Graphics.FromImage(img);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);// Text 
                g.DrawString(WebUser.Name, font, drawBrush, 3, 3);
                try
                {
                    File.Delete(pathMe);
                }
                catch
                {
                }
                img.Save(pathMe);
                img.Dispose();
                g.Dispose();

                File.Copy(pathMe, BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + WebUser.Name + ".JPG", true);
            }

            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='../Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>- Electronic signature settings " + WebUser.Auth);
            else
                this.AddFieldSet(" Electronic signature settings " + WebUser.Auth);

            // this.AddFieldSet(" Electronic signature settings ");

            this.Add("<p align=center><img src='../DataUser/Siganture/" + WebUser.No + ".jpg' border=1 onerror=\"this.src='../DataUser/Siganture/UnName.jpg'\"/> </p>");

            this.Add(" Upload ");

            System.Web.UI.WebControls.FileUpload fu = new System.Web.UI.WebControls.FileUpload();
            fu.ID = "F";
            this.Add(fu);

            Btn btn = new Btn();
            btn.Text = " Determine ";
            btn.Click += new EventHandler(btn_Siganture_Click);
            this.Add(btn);

            this.AddHR();



            this.AddB(" Using a scanner setup steps :");
            this.AddUL();
            this.AddLi(" Write your signature on white paper ");
            this.AddLi(" Fed scanner , And get jpg File .");
            this.AddLi(" The use of image processing tools to deal with them down to  90*30 Pixel size .");
            this.AddULEnd();

            this.AddB(" Handwriting settings :");
            this.AddUL();
            this.AddLi(" Start sketchpad program , Write your signature .");
            this.AddLi(" Save as .jpg File , Set file 90*30 Pixel size .");
            this.AddULEnd();

            this.AddB(" Let the system automatically created for you （ Please select the font ）:");
            this.AddUL();
            this.AddLi("<a href='" + this.PageID + ".aspx?RefNo=Siganture&DoType=ST'> Times New Roman </a>");
            this.AddLi("<a href='" + this.PageID + ".aspx?RefNo=Siganture&DoType=LS'> Clerical </a>");
            this.AddULEnd();

            this.AddFieldSetEnd();
        }

        void btn_Siganture_Click(object sender, EventArgs e)
        {
            FileUpload f = (FileUpload)this.FindControl("F");

            if (f.HasFile == false)
                return;

            try
            {
                System.IO.File.Delete(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Siganture/T.jpg");

                f.SaveAs(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Siganture/T.jpg");
                System.Drawing.Image img = System.Drawing.Image.FromFile(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Siganture/T.jpg");
                if (img.Width != 90 || img.Height != 30)
                {
                    img.Dispose();
                    throw new Exception(" Picture you upload does not meet the requirements of highly =30px  Width =90px  Requirements .");
                }

                img.Dispose();
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
                return;
            }

            f.SaveAs(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Siganture/" + WebUser.No + ".jpg");
            f.SaveAs(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Siganture/" + WebUser.Name + ".jpg");

            f.PostedFile.InputStream.Close();
            f.PostedFile.InputStream.Dispose();
            f.Dispose();


            this.Response.Redirect(this.Request.RawUrl, true);
            //this.Alert(" Saved successfully .");
        }
        public void AdminSet()
        {
            this.AddFieldSet(" System Settings ");

            this.AddTable();
            this.AddTR();
            this.AddTDTitle(" Project ");
            this.AddTDTitle(" Project value ");
            this.AddTDTitle(" Description ");
            this.AddTREnd();


            this.AddTR();
            this.AddTD(" Title Picture ");
            FileUpload fu = new FileUpload();
            fu.ID = "F";
            this.AddTD(fu);
            this.AddTD(" System at the top of the title picture ");
            //   this.AddTDBigDoc(" Please adjust your own image size , Then upload it up . In the system settings you can control whether to display the title picture .");
            this.AddTREnd();

            this.AddTR();
            this.AddTD("ftp URL");
            TextBox tb = new TextBox();
            tb.Width = 200;
            tb.ID = "TB_FtpUrl";
            this.AddTD(tb);
            this.AddTD();
            this.AddTREnd();

            this.AddTR();
            this.AddTD("");

            Btn btn = new Btn();
            btn.Text = " OK ";
            btn.Click += new EventHandler(btn_AdminSet_Click);
            this.AddTD(btn);
            this.AddTD();
            this.AddTREnd();

            this.AddTR();
            this.AddTD();
            this.AddTD("<a href=\"javascript:WinOpen('../Comm/Sys/EditWebConfig.aspx')\" >System Setting</a>-<a href=\"javascript:WinOpen('../OA/FtpSet.aspx')\" >FTP Services</a>-<a href=\"javascript:WinOpen('/WF/Comm/Ens.aspx?EnsName=BP.OA.Links')\" >Link</a>");
            this.AddTD("");
            //  this.AddTD("<a href=\"javascript:WinOpen('./../WF/ClearDatabase.aspx')\" >" + this.ToE("ClearDB", " Clear process data ") + "</a>");

            this.AddTD();
            this.AddTREnd();

            this.AddTableEnd();

            this.AddFieldSetEnd();
        }
        void btn_AdminSet_Click(object sender, EventArgs e)
        {
            FileUpload f = (FileUpload)this.FindControl("F");
            if (f.HasFile == false)
                return;

            f.SaveAs(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/Title.gif");
            this.Response.Redirect(this.Request.RawUrl, true);
            //this.Alert(" Saved successfully .");
        }
        public void BindPass()
        {

            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>- Password change ");
            else
                this.AddFieldSet(" Password change ");

            this.AddBR();

            this.Add("<table border=0 width=80% align=center > ");
            this.AddTR();
            this.AddTDTitle();
            this.AddTDTitle();
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Old Password :");
            TextBox tb = new TextBox();
            tb.TextMode = TextBoxMode.Password;
            tb.ID = "TB_Pass1";
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" New Password :");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.Password;
            tb.ID = "TB_Pass2";
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Reenter new password :");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.Password;
            tb.ID = "TB_Pass3";
            this.AddTD(tb);
            this.AddTREnd();


            this.AddTR();
            this.AddTD("");





            Btn btn = new Btn();
            btn.Text = " Determine ";
            btn.Click += new EventHandler(btn_Click);
            this.AddTD(btn);
            this.AddTREnd();
            this.AddTableEnd();

            this.AddBR();
            this.AddFieldSetEnd();

        }
        public void BindProfile()
        {
            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>-" + " Basic Information " + WebUser.Auth);
            else
                this.AddFieldSet(" Basic Information " + WebUser.Auth);

            this.Add("<br><table border=0 width='80%' align=center >");
            this.AddTR();
            this.AddTD(" Cell phone ");
            TextBox tb = new TextBox();
            tb.TextMode = TextBoxMode.SingleLine;
            tb.ID = "TB_Tel";
            tb.Text = emp.Tel;
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD("Email");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.SingleLine;
            tb.ID = "TB_Email";
            tb.Text = emp.Email;
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD("QQ/RTX/MSN");
            tb = new TextBox();
            tb.TextMode = TextBoxMode.SingleLine;
            tb.ID = "TB_TM";
            tb.Text = emp.Email;
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Receive mode ");
            DDL ddl = new DDL();
            ddl.ID = "DDL_Way";
            ddl.BindSysEnum("AlertWay");
            //ddl.Items.Add(new ListItem(" Do not receive ", "0"));
            //ddl.Items.Add(new ListItem(" SMS ", "1"));
            //ddl.Items.Add(new ListItem(" Mail ", "2"));
            //ddl.Items.Add(new ListItem(" SMS + Mail ", "3"));
            ddl.SetSelectItem((int)emp.HisAlertWay);
            this.AddTD(ddl);
            this.AddTREnd();

            this.AddTR();
            Btn btn = new Btn();
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Profile_Click);
            this.AddTD("colspan=2 align=center", btn);
            this.AddTREnd();
            this.AddTableEnd();
            this.AddBR();
            this.AddFieldSetEnd();
        }
        void btn_Profile_Click(object sender, EventArgs e)
        {
            string tel = this.GetTextBoxByID("TB_Tel").Text;
            string mail = this.GetTextBoxByID("TB_Email").Text;
            int way = this.GetDDLByID("DDL_Way").SelectedItemIntVal;
 

            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
            emp.Tel = tel;
            emp.Email = mail;
            emp.HisAlertWay = (BP.WF.Port.AlertWay)way;
  

            try
            {
                emp.Update();
                this.Alert(" Set to take effect , Thank you for using .");
            }
            catch (Exception ex)
            {
                emp.CheckPhysicsTable();
                this.Alert(" Setting error :" + ex.Message);
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            string p1 = this.GetTextBoxByID("TB_Pass1").Text;
            string p2 = this.GetTextBoxByID("TB_Pass2").Text;
            string p3 = this.GetTextBoxByID("TB_Pass3").Text;




            if (p2.Length == 0 || p1.Length == 0)
            {
                this.Alert(" Passwords can not be empty ");
                return;
            }

            if (p2 != p3)
            {
                this.Alert(" The two passwords do not match .");
                return;
            }


            Emp emp = new Emp(WebUser.No);
            if (emp.Pass == p1)
            {
                emp.Pass = p2;
                emp.Update();
                this.Alert(" Password changed successfully , Keep in mind that your new password .");
            }
            else
            {
                this.Alert(" Old password error , You are not allowed to modify it .");
            }
        }
        /// <summary>
        ///  Aging analysis 
        /// </summary>
        public void BindTimes()
        {
            if (this.Request.QueryString["FK_Node"] != null)
            {
                this.BindTimesND();
                return;
            }
            if (this.Request.QueryString["FK_Flow"] != null)
            {
                this.BindTimesFlow();
                return;
            }

            FlowSorts sorts = new FlowSorts();
            sorts.RetrieveAll();

            Flows fls = new Flows();
            fls.RetrieveAll();

            Nodes nds = new Nodes();
            nds.RetrieveAll();

            this.AddTable();

            //  this.AddCaptionLeft("第1步: Select the process to be analyzed ");
            //  this.AddTR();
            ////  this.AddTDTitle("ID");
            //  this.AddTDTitle(" Process Category ");
            //  this.AddTDTitle(" Process / Process ");
            // // this.AddTDTitle(" Operating ");
            //  this.AddTDTitle(" Hours of Work ");
            //  this.AddTDTitle(" The average in days ");
            //  this.AddTREnd();

            foreach (FlowSort sort in sorts)
            {
                this.AddTRSum();
                this.AddTDB(sort.Name);
                this.AddTD("");
                this.AddTD();
                this.AddTD();
                this.AddTD();
                this.AddTD();

                this.AddTREnd();

                foreach (Flow fl in fls)
                {
                    if (sort.No != fl.FK_FlowSort)
                        continue;

                    this.AddTRSum();
                    this.AddTD();
                    this.AddTDB(fl.Name);
                    //  this.AddTD("<a href='"+this.PageID+".aspx?DoType=Times&FK_Flow=" + fl.No + "'> Analysis </a>");
                    this.AddTD(" Hours of Work ");
                    this.AddTD(" Average days " + fl.AvgDay.ToString("0.00"));

                    this.AddTD(" I count the work participation ");
                    this.AddTD(" The total number of working ");

                    this.AddTREnd();

                    decimal avgDay = 0;
                    foreach (BP.WF.Node nd in nds)
                    {
                        if (nd.FK_Flow != fl.No)
                            continue;

                        this.AddTR();
                        this.AddTD();
                        this.AddTD(nd.Name);
                        //  this.AddTD("<a href='"+this.PageID+".aspx?DoType=Times&FK_Node=" + nd.NodeID + "'> Analysis </a>");
                        string sql = "";

                        sql = "SELECT  COUNT(*) FROM ND" + nd.NodeID;

                        try
                        {
                            int num = DBAccess.RunSQLReturnValInt(sql);
                            this.AddTD(num);
                        }
                        catch
                        {
                            nd.CheckPhysicsTable();
                            this.AddTD(" Invalid ");
                        }

                        sql = "SELECT AVG( DateDiff(d, cast(RDT as datetime),  cast(CDT as datetime) ) ) FROM ND" + nd.NodeID;
                        try
                        {
                            decimal day = DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                            avgDay += day;
                            this.AddTD(day.ToString("0.00"));
                        }
                        catch
                        {
                            nd.CheckPhysicsTable();
                            this.AddTD(" Invalid ");
                        }

                        // day = DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                        //this.AddTD(DBAccess.RunSQLReturnValInt(""));
                        this.AddTD(" Invalid ");

                        this.AddTREnd();
                    }

                    if (avgDay != fl.AvgDay)
                    {
                        fl.AvgDay = avgDay;
                        fl.Update();
                    }
                }
            }
            this.AddTableEnd();
        }
        public void BindTimesFlow()
        {

        }
        public void BindTimesND()
        {
            int nodeid = int.Parse(this.Request.QueryString["FK_Node"]);
            BP.WF.Node nd = new BP.WF.Node(nodeid);
            this.AddTable();
            this.AddCaptionLeft("<a href='" + this.PageID + ".aspx?DoType=Times&FK_Flow=" + nd.FK_Flow + "'>" + nd.FlowName + "</a> => " + nd.Name);
            this.AddTR();
            this.AddTDTitle("IDX");
            this.AddTDTitle(" Staff ");
            this.AddTDTitle("Average time");
            this.AddTDTitle("Participation times");
            this.AddTREnd();
            this.AddTableEnd();
        }
        public void BindAutoLog()
        {
            string sql = "";

            switch (BP.Sys.SystemConfig.AppCenterDBType)
            {
                case DBType.Oracle:
                    sql = "SELECT a.No || a.Name as Empstr,AuthorDate, a.No,AuthorToDate FROM WF_Emp a WHERE Author='" + WebUser.No + "' AND AuthorWay >= 1";
                    break;
                default:
                    sql = "SELECT a.No + a.Name as Empstr,AuthorDate, a.No ,AuthorToDate FROM WF_Emp a WHERE Author='" + WebUser.No + "' AND AuthorWay >= 1";
                    break;
            }

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                if (WebUser.IsWap)
                {
                    this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>- Password change ");
                    this.AddBR();
                    this.AddMsgGreen(" Prompt ", " No colleagues are licensed to you , You can not use the License landing .");
                    this.AddFieldSetEnd();
                }
                else
                {
                    this.AddMsgGreen(" Prompt ", " No colleagues are licensed to you , You can not use the License landing .");
                }
                return;
            }

            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>- The following colleagues are licensed to you ");
            else
                this.AddFieldSet(" The following colleagues are licensed to you ");


            this.Add("<ul>");
            foreach (DataRow dr in dt.Rows)
            {
                this.AddLi("<a href=\"javascript:LogAs('" + dr[2] + "')\">" + " Authorized person " + ":" + dr["Empstr"] + "</a> -  Authorization Date :" + dr["AuthorDate"] + ", Effective Date :" + dr["AuthorToDate"]);
            }
            this.Add("</ul>");
            this.AddFieldSetEnd();
        }
        public void BindAuto()
        {
            string sql = "SELECT a.No,a.Name,b.Name as DeptName FROM Port_Emp a, Port_Dept b WHERE a.FK_Dept=b.No AND a.FK_Dept LIKE '" + WebUser.FK_Dept + "%' ORDER  BY a.FK_Dept ";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 >Home</a>-<a href='" + this.PageID + ".aspx'> Set up </a>- Please select authorized personnel ");
            else
                this.AddFieldSet(" Please select authorized personnel ");

            string deptName = null;
            this.AddBR();
            this.Add(" <table width='80%' align=center border=1 > ");
            this.AddTR();
            this.AddTDTitle(" Serial number ");
            this.AddTDTitle(" Department ");
            this.AddTDTitle(" Authorized personnel should perform ");
            this.AddTREnd();

            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string fk_emp = dr["No"].ToString();
                if (fk_emp == "admin" || fk_emp == WebUser.No)
                    continue;
                idx++;
                if (dr["DeptName"].ToString() != deptName)
                {
                    deptName = dr["DeptName"].ToString();
                    this.AddTRSum();
                    this.AddTDIdx(idx);
                    this.AddTD(deptName);
                }
                else
                {
                    this.AddTR();
                    this.AddTDIdx(idx);
                    this.AddTD();
                }

                string str = BP.WF.Glo.DealUserInfoShowModel(fk_emp, dr["Name"].ToString());
                //this.AddTD("<a href=\"" + this.PageID + ".aspx?RefNo=AutoDtl&FK_Emp=" + fk_emp + "\" >" + str + "</a>");
                this.AddTD("<a href=\"" + this.PageID + ".aspx?RefNo=AutoDtl&FK_Emp=" + fk_emp + "\" >" + str + "</a>");
                this.AddTREnd();
            }
            this.AddTableEnd();
            this.AddBR();
            this.AddFieldSetEnd();
        }
        /// <summary>
        ///  Authorization details 
        /// </summary>
        public void BindAutoDtl()
        {
            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='/WF/Img/Home.gif' border=0 >Home</a>-<a href='" + this.PageID + ".aspx'> Set up </a>- License Details ");
            else
                this.AddFieldSet(" License Details ");

            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
            BP.WF.Port.WFEmp empAu = new BP.WF.Port.WFEmp(this.Request["FK_Emp"]);

            this.AddBR();
            this.AddTable();
            this.AddTR();
            this.AddTDTitle(" Project ");
            this.AddTDTitle(" Content ");
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Licensed to :");
            this.AddTD(empAu.No + "    " + empAu.Name);
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Withdraw authorization date :");
            TB tb = new TB();
            tb.ID = "TB_DT";
            System.DateTime dtNow = System.DateTime.Now;
            dtNow = dtNow.AddDays(14);
            tb.Text = dtNow.ToString(DataType.SysDataTimeFormat);
            tb.ShowType = TBType.DateTime;
            tb.Attributes["onfocus"] = "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'});";
            this.AddTD(tb);
            this.AddTREnd();

            this.AddTR();
            this.AddTD(" Authorization :");
            DDL ddl = new DDL();
            ddl.ID = "DDL_AuthorWay";
            ddl.BindSysEnum(BP.WF.Port.WFEmpAttr.AuthorWay);
            ddl.SetSelectItem(emp.AuthorWay);
            this.AddTD(ddl);
            this.AddTREnd();

            Button btnSaveIt = new Button();
            btnSaveIt.ID = "Btn_Save";
            btnSaveIt.CssClass = "Btn";
            btnSaveIt.Text = " Save ";
            btnSaveIt.Click += new EventHandler(btnSaveIt_Click);
            this.AddTR();
            this.AddTD("colspan=1", "<b><a href=\"javascript:WinShowModalDialog('ToolsSet.aspx?RefNo=AthFlows&d=" + DateTime.Now.ToString() + "')\" > Sets the authorization process scope </a></b>");
            this.AddTD("colspan=1", btnSaveIt);
            this.AddTREnd();

            this.AddTR();
            this.AddTDBigDoc("colspan=2", " Explanation : After you determine the recovery after the date of authorization , Authorized person can no longer log in to your identity ,<br> If not yet reached the specified date, you can retrieve the authorization .");
            this.AddTREnd();
            this.AddTableEndWithBR();
            this.AddFieldSetEnd();
        }
        void btnSaveIt_Click(object sender, EventArgs e)
        {
            BP.WF.Port.WFEmp emp = new BP.WF.Port.WFEmp(WebUser.No);
            emp.AuthorDate = BP.DA.DataType.CurrentData;
            emp.Author = this.Request["FK_Emp"];
            emp.AuthorToDate = this.GetTBByID("TB_DT").Text;
            emp.AuthorWay = this.GetDDLByID("DDL_AuthorWay").SelectedItemIntVal;
            if (emp.AuthorWay == 2 && emp.AuthorFlows.Length < 3)
            {
                this.Alert(" You specify the License is authorized by the specified flow range , But you do not specify the scope of authorization processes .");
                return;
            }
            emp.Update();
            //BP.Sys.UserLog.AddLog("Auth", WebUser.No, " All authorized ");
            BP.Sys.Glo.WriteUserLog("Auth", WebUser.No, " All authorized ");
            this.Response.Redirect(this.PageID + ".aspx", true);
        }
        public void BindPer()
        {
            if (WebUser.Auth != null)
            {
                this.AddFieldSet(" Prompt ");
                this.AddBR();
                this.Add(" Your login is authorized mode , You can not view personal information .");
                this.AddUL();
                this.AddLi("<a href=\"javascript:ExitAuth('" + WebUser.Auth + "')\"> Exit licensing model </a>");
                this.AddLi("<a href=" + this.PageID + ".aspx > Set up </a>");
                if (WebUser.IsWap)
                    this.AddLi("<a href='Home.aspx'> Back to Home </a>");

                this.AddULEnd();
                this.AddFieldSetEnd();
                return;
            }


            BP.WF.Port.WFEmp au = new BP.WF.Port.WFEmp(WebUser.No);

            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='../Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>-" + " Basic Information " + WebUser.Auth);
            else
                this.AddFieldSet(" Basic Information " + WebUser.Auth);

            this.Add("<p class=BigDoc >");

            this.Add(" User Account :&nbsp;&nbsp;<font color=green>" + WebUser.No + "</font>&nbsp;&nbsp;");
            this.Add("<br> Username :&nbsp;&nbsp;<font color=green>" + WebUser.Name + "</font>&nbsp;&nbsp;");
            this.AddHR();

            this.AddB(" Electronic Signatures :<img src='../DataUser/Siganture/" + WebUser.No + ".jpg' border=1 onerror=\"this.src='../../DataUser/Siganture/UnName.jpg'\"/><a href='" + this.PageID + ".aspx?RefNo=Siganture' > Set up / Modification </a>.");

            this.AddBR();
            this.AddHR();

            this.Add(" Primary sector  : <font color=green>" + WebUser.FK_DeptName + "</font>");

            this.AddBR();
            this.AddBR();


            // this.Add(au.AuthorIsOK.ToString());
            if (au.AuthorIsOK == false)
                this.Add(" Authorization : Unauthorized  - <a href='" + this.PageID + ".aspx?RefNo=Auto' > Perform authorization </a>.");
            else
            {
                string way = "";
                if (au.AuthorWay == 1)
                    way = " All authorized ";
                else
                    way = " Specifies the authorization process scope ";
                this.Add(" Authorization : Licensed to :<font color=green>" + au.Author + "</font>, Authorization Date : <font color=green>" + au.AuthorDate + "</font>, Withdraw authorization date :<font color=green>" + au.AuthorToDate + "</font>.<br> I want :<a href=\"javascript:TakeBack('" + au.Author + "')\" > Deauthorize </a>; Authorization :<font color=green>" + way + "</font>,<a href=\"" + this.PageID + ".aspx?RefNo=AutoDtl&FK_Emp=" + au.Author + "\"> I want to modify the authorization information </a>.");
            }

            this.Add("&nbsp; I want :<a href='" + this.PageID + ".aspx?RefNo=Pass'> Change Password </a>");

            this.AddBR("<hr><b> Information Tips :</b><a href='" + this.PageID + ".aspx?RefNo=Profile'> Set up / Modification </a>");
            this.Add("<br><br> SMS Alert accept phone number  : <font color=green>" + au.TelHtml + "</font>");
            this.Add("<br><br> Accept E-mail Remind  : <font color=green>" + au.EmailHtml + "</font>");
           
            this.AddHR();
            Stations sts = WebUser.HisStations;
            this.AddB(" Post / Department - Competence ");
            this.AddBR();
            this.AddBR(" Permissions post ");
            foreach (Station st in sts)
            {
                this.Add(" - <font color=green>" + st.Name + "</font>");
            }

            Depts depts = WebUser.HisDepts;
            this.AddBR();
            this.AddBR();
            this.Add(" Department Permissions ");
            foreach (Dept st in depts)
                this.Add(" - <font color=green>" + st.Name + "</font>");

            this.Add("</p>");
            this.AddFieldSetEnd();
        }
        public void BingPerPng()
        {
            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='../Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>-" + " Icon Information ");
            else
                this.AddFieldSet(" Icon Information ");

            string picfile = BP.Sys.SystemConfig.PathOfWebApp + "DataUser\\UserIcon";

            this.AddHR();
            this.AddBR();

            this.AddB("Icon Set up :<a href='" + this.PageID + ".aspx?RefNo=BitmapCutter' > Set up / Icon Set </a>");
            this.AddHR();
            this.AddBR();
            bool isNo = false;

            System.Drawing.Image myImage = null;
            int phWidth = 0;
            int phHeight = 0;
            DirectoryInfo di = new DirectoryInfo(picfile);
            if (di.Exists)
            {
                string[] subpic = Directory.GetFiles(picfile);
                if (subpic.Length > 0)
                {
                    foreach (string PicPath in subpic)
                    {
                        string[] tempFilePath = PicPath.Split('\\');

                        if (tempFilePath[tempFilePath.Length - 1].StartsWith(WebUser.No + "BigerCon"))
                        {
                            myImage = System.Drawing.Image.FromFile(picfile + "/" + WebUser.No + "BigerCon.png");
                            phWidth = myImage.Width;
                            phHeight = myImage.Height;

                            if (phWidth > 510)
                            {
                                isNo = false;
                            }
                            else
                            {
                                isNo = true;
                            }
                            myImage.Dispose();
                            break;
                        }
                    }
                }
            }
            if (isNo)
            {
                this.Add("<div id='Container'>");
                this.Add("<div id='Content'>");
                this.Add("<div id='Content-Left'><img src='../DataUser/UserIcon/" + WebUser.No + "BigerCon.png'/></div>");
                this.Add("<div id='Content-Main2'><img src='../DataUser/UserIcon/" + WebUser.No + "Smaller.png' width='40px' height='40px'/></div>");
                this.Add("<div id='Content-Main3'>32*32</div>");
                this.Add("<div id='Content-Main'><img src='../DataUser/UserIcon/" + WebUser.No + ".png' width='60px' height='60px'/></div>");
                this.Add("<div id='Content-Main3'>60*60</div>");
                this.Add("<div id='Content-Main1'><img src='../DataUser/UserIcon/" + WebUser.No + "Biger.png' width='100px' height='100px'/></div>");
                this.Add("<div id='Content-Main3'>100*100</div>");
                this.Add("</div>");
                this.Add("</div>");
            }
            else
            {
                this.Add("<div id='Container'>");
                this.Add("<div id='Content'>");
                this.Add("<div id='Content-Left' style='border: solid 1px #7d9edb;padding: 1px;'></div>");
                this.Add("<div id='Content-Main2'><img src='../DataUser/UserIcon/Default.png' border=1 width='32px' height='32px'/></div>");
                this.Add("<div id='Content-Main3'>32*32</div>");
                this.Add("<div id='Content-Main'><img src='../DataUser/UserIcon/Default.png' border=1 width='60px' height='60px'/></div>");
                this.Add("<div id='Content-Main3'>60*60</div>");
                this.Add("<div id='Content-Main1'><img src='../DataUser/UserIcon/Default.png' border=1 width='100px' height='100px'/></div>");
                this.Add("<div id='Content-Main3'>100*100</div>");
                this.Add("</div>");
                this.Add("</div>");
            }
            this.AddTableEnd();
            this.AddBR();
            this.AddFieldSetEnd();
        }

        public void BitmapCutter()
        {
            if (WebUser.IsWap)
                this.AddFieldSet("<a href=Home.aspx ><img src='../Img/Home.gif' border=0 > Home </a>-<a href='" + this.PageID + ".aspx'> Set up </a>- Settings icon ");
            else
                this.AddFieldSet(" Settings icon ");

            string picfile = BP.Sys.SystemConfig.PathOfWebApp + "DataUser\\UserIcon";
            bool isNo = false;

            System.Drawing.Image myImage = null;
            int phWidth = 0;
            int phHeight = 0;
            DirectoryInfo di = new DirectoryInfo(picfile);
            if (di.Exists)
            {
                string[] subpic = Directory.GetFiles(picfile);
                if (subpic.Length > 0)
                {
                    foreach (string PicPath in subpic)
                    {
                        string[] tempFilePath = PicPath.Split('\\');

                        if (tempFilePath[tempFilePath.Length - 1].StartsWith(WebUser.No + "BigerCon"))
                        {
                            myImage = System.Drawing.Image.FromFile(picfile + "/" + WebUser.No + "BigerCon.png");
                            phWidth = myImage.Width;
                            phHeight = myImage.Height;
                            if (phWidth > 510)
                            {
                                isNo = false;
                            }
                            else
                            {
                                isNo = true;
                            }
                            myImage.Dispose();
                            break;
                        }
                    }
                }
            }
            this.Add("<div id='Player'>");
            TextBox tb = new TextBox();
            if (Request["Image"] == null)
            {
                if (isNo)
                {
                    tb.Text = WebUser.No + "BigerCon.png";
                }
                else
                {
                    tb.Text = "Default.png";
                }
            }
            else
            {
                tb.Text = Request["Image"];
            }
            tb.ID = "ImageName";
            tb.Attributes["style"] = "display:none";
            this.Add(tb);

            tb = new TextBox();
            if (Request["Width"] == null)
            {
                if (isNo)
                {

                    if (phWidth > 510)
                    {
                        tb.Text = "500";
                    }
                    else
                    {
                        tb.Text = phWidth.ToString();
                    }
                }
                else
                {
                    tb.Text = "500";
                }
            }
            else
            {
                tb.Text = Request["Width"];
            }
            tb.ID = "WSize";
            tb.Attributes["style"] = "display:none";
            this.Add(tb);

            tb = new TextBox();
            if (Request["Height"] == null)
            {
                if (isNo)
                {

                    if (phHeight > 800)
                    {
                        tb.Text = "400";
                    }
                    else
                    {
                        tb.Text = phHeight.ToString();
                    }
                }
                else
                {
                    tb.Text = "400";
                }
            }
            else
            {
                tb.Text = Request["Height"];
            }
            tb.ID = "HSize";
            tb.Attributes["style"] = "display:none";
            this.Add(tb);

            tb = new TextBox();
            if (Request["cHg"] == null)
            {
                if (isNo)
                {
                    tb.Text = (phWidth / 2).ToString();
                }
                else
                {
                    tb.Text = "200";
                }
            }
            else
            {
                tb.Text = Request["cHg"];
            }
            tb.ID = "Chg";
            tb.Attributes["style"] = "display:none";
            this.Add(tb);

            System.Web.UI.WebControls.FileUpload fu = new System.Web.UI.WebControls.FileUpload();
            fu.ID = "F";
            this.Add(fu);

            Btn btn = new Btn();
            btn.Text = " Determine ";
            btn.Click += new EventHandler(btn_Sure_Click);
            this.Add(btn);

            this.Add("</div>");
            this.Add("<div id='Container'>");
            this.Add("</div>");
            this.AddBR();
            this.AddFieldSetEnd();
        }

        void btn_Sure_Click(object sender, EventArgs e)
        {
            string guidName = Guid.NewGuid().ToString();
            FileUpload f = (FileUpload)this.FindControl("F");
            string imName = f.FileName;
            if (f.HasFile == false)
                return;
            string imgPath = Server.MapPath("~/DataUser/UserIcon/Model/" + WebUser.No + "Model.png");
            f.SaveAs(imgPath);
            string fileName = "Model.png";

            System.Drawing.Image myImage = System.Drawing.Image.FromStream(f.PostedFile.InputStream);
            int phWidth = myImage.Width;
            int phHeight = myImage.Height;
            int widths = 0;
            int heights = 0;
            int chg = 0;
            if (phWidth > 510)
            {

                if ((phWidth * 510) > (phHeight * 500))
                {
                    widths = 500;
                    heights = (510 * phHeight) / phWidth;
                }
                else
                {
                    heights = 500;
                }

                GetPicThumbnail(imgPath, Server.MapPath("~/DataUser/UserIcon/" + WebUser.No + "Model.png"), heights, 510);
                widths = 510;
            }
            else
            {
                if (phWidth < 450)
                {
                    if ((phWidth * 510) > (phHeight * 500))
                    {
                        widths = 510;
                        heights = (510 * phHeight) / phWidth;
                    }
                    else
                    {
                        heights = 500;
                    }

                    GetPicThumbnail(imgPath, Server.MapPath("~/DataUser/UserIcon/" + WebUser.No + "Model.png"), heights, 510);
                    widths = 510;
                }
                else
                {
                    widths = phWidth;
                    heights = phHeight;
                    GetPicThumbnail(imgPath, Server.MapPath("~/DataUser/UserIcon/" + WebUser.No + "Model.png"), heights, widths);
                    chg = Convert.ToInt32(widths * 0.5);
                    if (chg > 250)
                    {
                        chg = 250;
                    }
                    myImage.Dispose();
                }
            }
            if (File.Exists(imgPath))
            {
                File.SetAttributes(imgPath, FileAttributes.Normal);
                File.Delete(imgPath);
            }
            //System.Drawing.Image.GetThumbnailImageAbort callb = null;
            //System.Drawing.Image newImage = myImage.GetThumbnailImage(widths, heights, callb, new IntPtr());
            //newImage.Save(BP.Sys.SystemConfig.PathOfWebApp + "/DataUser/UserIcon/" + WebUser.No + "Model.png");
            //newImage.Dispose();
            f.PostedFile.InputStream.Close();
            f.PostedFile.InputStream.Dispose();
            f.Dispose();

            string s = this.Request.QueryString["RefNo"];
            this.Response.Redirect("/WF/Tools.aspx?RefNo=" + s + "&Image=" + WebUser.No + fileName + "&Width=" + widths + "&Height=" + heights + "&cHg=" + chg, true);

        }
        /// <summary>
        ///  Redefine a picture 
        /// </summary>
        /// <param name="fromImg">Image Path type of picture </param>
        /// <param name="width"> Redefine the image width </param>
        /// <param name="height"> Redefine the image height </param>
        /// <returns></returns>
        public static Bitmap MakeThumbnail(System.Drawing.Image fromImg, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            int ow = fromImg.Width;
            int oh = fromImg.Height;

            // Create a sketchpad 
            Graphics g = Graphics.FromImage(bmp);

            // Set high quality interpolation method 
            g.InterpolationMode = InterpolationMode.High;
            // Set high , Low-speed rendering smoothness 
            g.SmoothingMode = SmoothingMode.HighQuality;
            // Empty canvas and filled with a transparent background color 
            g.Clear(Color.Transparent);

            g.DrawImage(fromImg, new Rectangle(0, 0, width, height),
                new Rectangle(0, 0, ow, oh),
                GraphicsUnit.Pixel);

            return bmp;

        }
        /// <summary>
        ///  Lossless compression Picture 
        /// </summary>
        /// <param name="sFile"> Pictures of the original path </param>
        /// <param name="dFile"> Save path scaled </param>
        /// <param name="dHeight"> Scaled picture height </param>
        /// <param name="dWidth"> Scaled picture broadband </param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);// Created from the specified file Image
            ImageFormat tFormat = iSource.RawFormat;// Specify the file format and gain 
            int sW = 0, sH = 0;// Recording width and height 
            Size tem_size = new Size(iSource.Width, iSource.Height);// Instantiation size. To know the height and width of the rectangle 
            if (tem_size.Height > dHeight || tem_size.Width > dWidth)// Determine the size of the original image size is larger than the specified 
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else// If the original size is smaller than the specified size 
            {
                sW = dWidth;// Original width equal to the specified width 
                sH = dHeight;// Original height equal to the specified height 
            }
            // Get redefined Pictures 
            Bitmap thumBnail = MakeThumbnail(iSource, sH, 510);

            Bitmap oB = new Bitmap(dWidth, dHeight);// Instantiation 
            Graphics g = Graphics.FromImage(oB);// From the specified Image Created Graphics
            Rectangle destRect = new Rectangle(new Point(0, 0), new Size(510, sH));// Target location 
            Rectangle origRect = new Rectangle(new Point(0, 0), new Size(sH, 510));// Original Location （ The default picture taken from the original image size is equal to the target size of the picture ）
            Graphics G = Graphics.FromImage(oB);

            G.Clear(Color.White);
            //  Specifies the bi-cubic interpolation quality . Performing pre-screened to ensure high-quality shrink . This mode can be converted to produce the highest quality image . 
            G.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //  Specify high-quality , Low-speed rendering . 
            G.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(thumBnail, destRect, origRect, GraphicsUnit.Pixel);
            //G.DrawString("Xuanye", f, b, 0, 0);
            G.Dispose();
            // When you save the picture , Set the compression quality 
            EncoderParameters ep = new EncoderParameters();// Used to pass values to the image encoder 
            long[] qy = new long[1];
            qy[0] = 100;
            EncoderParameter eParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParm;
            try
            {
                oB.Save(dFile, tFormat);//  Save the specified format to the specified file 
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();// Release resources 
                oB.Dispose();
            }
        }
    }
}