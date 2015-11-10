using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.WF;
using BP.WF.Data;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;
namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_BillSet : BP.Web.WebPage
    {
        public int NodeID
        {
            get
            {
                return int.Parse(this.Request.QueryString["NodeID"]);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  Get File Path 
        /// </summary>
        public string FileFullPath(string fileName, BillTemplate bt)
        {
            string fileType = "";
            if (bt.HisBillFileType == BillFileType.RuiLang)
            {
                fileType = ".grf";
            }
            else
            {
                fileType = ".rtf";
            }

            string filePath = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\" + bt.No + fileType;
            BP.WF.Node curNode = new BP.WF.Node(this.NodeID);
            // To modify the form is stored path tree 
            if (curNode.FormType == NodeFormType.SheetTree)
            {
                bt.Url = "FlowFrm\\" + this.FK_Flow + "\\" + this.NodeID + "\\" + fileName.Replace(fileType, "");
                filePath = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\FlowFrm\\" + this.FK_Flow + "\\" + this.NodeID;
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                filePath = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\FlowFrm\\" + this.FK_Flow + "\\" + this.NodeID + "\\" + fileName;
            }
            return filePath;
        }
        public void DoNew(BillTemplate bill)
        {
            this.Ucsys1.Clear();
            BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            this.Ucsys1.AddTable();
            this.Ucsys1.AddCaptionLeft("<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "' >" + " Return " + "</a> - <a href=Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=New ><img  border=0 src='../Img/Btn/New.gif' /> New </a>");
            this.Ucsys1.AddTR();
            this.Ucsys1.AddTDTitle(" Project ");
            this.Ucsys1.AddTDTitle(" Enter ");
            this.Ucsys1.AddTDTitle(" Remark ");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Document Type "); //  Invoice / Documents Name 
            DDL ddl = new DDL();
            ddl.ID = "DDL_BillType";

            BP.WF.Data.BillTypes ens = new BillTypes();
            ens.RetrieveAllFromDBSource();

            if (ens.Count == 0)
            {
                BP.WF.Data.BillType enB = new BillType();
                enB.Name = " New Type " + "1";
                enB.FK_Flow = this.FK_Flow;
                enB.No = "01";
                enB.Insert();
                ens.AddEntity(enB);
            }

            ddl.BindEntities(ens);
            ddl.SetSelectItem(bill.FK_BillType);
            this.Ucsys1.AddTD(ddl);
            this.Ucsys1.AddTD("<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=EditType'><img src='../Img/Btn/Edit.gif' border=0/> Category Maintenance </a>");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Serial number ");
            TB tb = new TB();
            tb.ID = "TB_No";
            tb.Text = bill.No;
            tb.Enabled = false;
            if (tb.Text == "")
                tb.Text = " The system automatically generates ";

            this.Ucsys1.AddTD(tb);
            this.Ucsys1.AddTD("");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Name "); //  Invoice / Documents Name 
            tb = new TB();
            tb.ID = "TB_Name";
            tb.Text = bill.Name;
            tb.Columns = 40;
            this.Ucsys1.AddTD("colspan=2", tb);
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Generated file type "); //  Invoice / Documents Name 
            ddl = new DDL();
            ddl.ID = "DDL_BillFileType";
            ddl.BindSysEnum("BillFileType");
            ddl.SetSelectItem((int)bill.HisBillFileType);
            this.Ucsys1.AddTD(ddl);
            this.Ucsys1.AddTD(" Does not currently support excel,html Format .");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Document templates ");
            HtmlInputFile file = new HtmlInputFile();
            file.ID = "f";
            file.Attributes["width"] = "100%";
            this.Ucsys1.AddTD("colspan=2", file);
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTRSum();
            this.Ucsys1.Add("<TD class=TD colspan=3 align=center>");
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            this.Ucsys1.Add(btn);
            btn.Click += new EventHandler(btn_Click);
            this.Ucsys1.Add(btn);
            if (bill.No.Length > 1)
            {


                btn = new Button();
                btn.ID = "Btn_Del";
                btn.CssClass = "Btn";
                btn.Text = " Delete "; // " Delete documents ";
                this.Ucsys1.Add(btn);
                btn.Attributes["onclick"] += " return confirm(' You acknowledge that you ?');";
                btn.Click += new EventHandler(btn_Del_Click);
            }
            string url = "";
            string fileType = "";
            if (bill.HisBillFileType == BillFileType.RuiLang)
            {
                fileType = "grf";
            }
            else
            {
                fileType = "rtf";
            }

            if (this.RefNo != null)
                url = "<a href='../../DataUser/CyclostyleFile/" + bill.Url + "." + fileType + "'><img src='../Img/Btn/save.gif' border=0/>  Template Download </a>";

            this.Ucsys1.Add(url + "</TD>");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTable();
        }
        void btn_Gener_Click(object sender, EventArgs e)
        {
            string url = "Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=Edit&RefNo=" + this.RefNo;
            this.Response.Redirect(url, true);
        }
        void btn_Click(object sender, EventArgs e)
        {
            HtmlInputFile file = this.Ucsys1.FindControl("f") as HtmlInputFile;
            BillTemplate bt = new BillTemplate();
            bt.NodeID = this.NodeID;
            BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            if (this.RefNo != null)
            {
                bt.No = this.RefNo;
                bt.Retrieve();
                bt = this.Ucsys1.Copy(bt) as BillTemplate;
                bt.NodeID = this.NodeID;
                bt.FK_BillType = this.Ucsys1.GetDDLByID("DDL_BillType").SelectedItemStringVal;
                if (file.Value == null || file.Value.Trim() == "")
                {
                    bt.Update();
                    this.Alert(" Saved successfully ");
                    return;
                }

                if (bt.HisBillFileType == BillFileType.RuiLang)
                {
                    if (file.Value.ToLower().Contains(".grf") == false)
                    {
                        this.Alert("@ Error , Unlawful  grf  Format file .");
                        return;
                    }
                }
                else
                {
                    if (file.Value.ToLower().Contains(".rtf") == false)
                    {
                        this.Alert("@ Error , Unlawful  rtf  Format file .");
                        return;
                    }
                }
                string temp = "";
                string tempName = "";
                if (bt.HisBillFileType == BillFileType.RuiLang)
                {
                    tempName = "Temp.grf";
                    temp = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\Temp.grf";
                    file.PostedFile.SaveAs(temp);
                }
                else
                {
                    tempName = "Temp.rtf";
                    temp = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\Temp.rtf";
                    file.PostedFile.SaveAs(temp);
                }



                // Check the file is correct .
                try
                {
                    string[] paras = BP.DA.Cash.GetBillParas_Gener(tempName, nd.HisFlow.HisGERpt.EnMap.Attrs);
                }
                catch (Exception ex)
                {
                    this.Ucsys2.AddMsgOfWarning(" Error Messages ", ex.Message);
                    return;
                }
                string fullFile = FileFullPath(file.PostedFile.FileName, bt);//BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\" + bt.No + ".rtf";
                System.IO.File.Copy(temp, fullFile, true);
                bt.Update();
                return;
            }
            bt = this.Ucsys1.Copy(bt) as BillTemplate;

            if (file.Value != null)
            {
                if (bt.HisBillFileType == BillFileType.RuiLang)
                {
                    if (file.Value.ToLower().Contains(".grf") == false)
                    {
                        this.Alert("@ Error , Unlawful  grf  Format file .");
                        // this.Alert("@ Error , Unlawful  rtf  Format file .");
                        return;
                    }
                }
                else
                {
                    if (file.Value.ToLower().Contains(".rtf") == false)
                    {
                        this.Alert("@ Error , Unlawful  rtf  Format file .");
                        // this.Alert("@ Error , Unlawful  rtf  Format file .");
                        return;
                    }
                }

            }
            else
            {
                this.Alert(" Please upload a file .");
                // this.Alert("@ Error , Unlawful  rtf  Format file .");
                return;

            }

            /*  If you include these two fields .*/
            string fileName = file.PostedFile.FileName;
            fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            if (bt.Name == "")
            {

                bt.Name = fileName.Replace(".rtf", "");
                bt.Name = fileName.Replace(".grf", "");
            }

            try
            {
                bt.No = BP.Tools.chs2py.convert(bt.Name);
                if (bt.IsExits)
                    bt.No = bt.No + "." + BP.DA.DBAccess.GenerOID().ToString();
            }
            catch
            {
                bt.No = BP.DA.DBAccess.GenerOID().ToString();
            }
            string tmp = "";
            string tmpName = "";
            if (bt.HisBillFileType == BillFileType.RuiLang)
            {
                tmpName = "Temp.grf";
                tmp = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\Temp.grf";
                file.PostedFile.SaveAs(tmp);
            }
            else
            {
                tmpName = "Temp.rtf";
                tmp = BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\Temp.rtf";
                file.PostedFile.SaveAs(tmp);
            }


            // Check the file is correct .
            try
            {
                string[] paras1 = BP.DA.Cash.GetBillParas_Gener(tmpName, nd.HisFlow.HisGERpt.EnMap.Attrs);
            }
            catch (Exception ex)
            {
                this.Ucsys2.AddMsgOfWarning("Error:", ex.Message);
                return;
            }

            string fullFile1 = FileFullPath(fileName, bt);//BP.Sys.SystemConfig.PathOfCyclostyleFile + "\\" + bt.No + ".rtf";
            System.IO.File.Copy(tmp, fullFile1, true);
            // file.PostedFile.SaveAs(fullFile1);
            bt.FK_BillType = this.Ucsys1.GetDDLByID("DDL_BillType").SelectedItemStringVal;
            bt.Insert();

            #region  Node information update .
            string Billids = "";
            BillTemplates tmps = new BillTemplates(nd);
            foreach (BillTemplate Btmp in tmps)
            {
                Billids += "@" + Btmp.No;
            }
            nd.HisBillIDs = Billids;
            nd.Update();
            #endregion  Node information update .

            this.Response.Redirect("Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID, true);
        }
        void btn_Del_Click(object sender, EventArgs e)
        {
            BillTemplate t = new BillTemplate();
            t.No = this.RefNo;
            t.Delete();

            #region  Node information update .
            BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            string Billids = "";
            BillTemplates tmps = new BillTemplates(nd);
            foreach (BillTemplate tmp in tmps)
            {
                Billids += "@" + tmp.No;
            }
            nd.HisBillIDs = Billids;
            nd.Update();
            #endregion  Node information update .
            this.Response.Redirect("Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID, true);
        }
        /// <summary>
        ///  Category modification 
        /// </summary>
        public void EditTypes()
        {
            this.Ucsys1.AddTable();
            this.Ucsys1.AddCaptionLeft("<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "'> Return </a> - Document Category Maintenance ");
            this.Ucsys1.AddTR();
            this.Ucsys1.AddTDTitle(" Category Number ");
            this.Ucsys1.AddTDTitle(" Category Name ");
            this.Ucsys1.AddTREnd();

            BillTypes ens = new BillTypes();
            ens.RetrieveAll();
            for (int i = 1; i < 18; i++)
            {
                this.Ucsys1.AddTR();
                this.Ucsys1.AddTD(i.ToString().PadLeft(2, '0'));
                TextBox tb = new TextBox();
                tb.ID = "TB_" + i;
                tb.Columns = 50;
                try
                {
                    BillType en = ens[i - 1] as BillType;
                    tb.Text = en.Name;
                    this.Ucsys1.AddTD(tb);
                }
                catch
                {
                    this.Ucsys1.AddTD(tb);
                }
                this.Ucsys1.AddTREnd();
            }

            this.Ucsys1.AddTableEndWithHR();
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = "Save";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_SaveTypes_Click);
            this.Ucsys1.Add(btn);
        }
        protected void btn_SaveTypes_Click(object sender, EventArgs e)
        {
            BillTypes ens = new BillTypes();
            ens.RetrieveAll();
            ens.Delete();
            for (int i = 1; i < 18; i++)
            {
                string name = this.Ucsys1.GetTextBoxByID("TB_" + i).Text;
                if (string.IsNullOrEmpty(name))
                    continue;

                BillType en = new BillType();
                en.No = i.ToString().PadLeft(2, '0');
                en.Name = name;
                en.FK_Flow = this.FK_Flow;
                en.Insert();
            }
            this.Alert(" Saved successfully .");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = " Node Design Documents "; //" Node Design Documents ";
            switch (this.DoType)
            {
                case "Edit":
                    BillTemplate bk1 = new BillTemplate(this.RefNo);
                    bk1.NodeID = this.NodeID;
                    this.DoNew(bk1);
                    return;
                case "New":
                    BillTemplate bk = new BillTemplate();
                    bk.NodeID = this.RefOID;
                    this.DoNew(bk);
                    return;
                case "EditType":
                    EditTypes();
                    return;
                default:
                    break;
            }

            BillTemplates Bills = new BillTemplates(this.NodeID);
            if (Bills.Count == 0)
            {
                this.Response.Redirect("Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=New", true);
                return;
            }

            BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            this.Title = nd.Name + " - " + " Document management ";  // Document management 
            this.Ucsys1.AddTable();
            if (this.RefNo == null)
                this.Ucsys1.AddCaptionLeft("<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=New'><img src='../Img/Btn/New.gif' border=0/> New </a> -<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID + "&DoType=EditType'><img src='../Img/Btn/Edit.gif' border=0/> Category Maintenance </a>");
            this.Ucsys1.AddTR();
            this.Ucsys1.AddTDTitle("IDX");
            this.Ucsys1.AddTDTitle(" Serial number ");
            this.Ucsys1.AddTDTitle(" Name ");
            this.Ucsys1.AddTDTitle(" Operating ");
            this.Ucsys1.AddTREnd();
            int i = 0;
            foreach (BillTemplate Bill in Bills)
            {
                i++;
                this.Ucsys1.AddTR();
                this.Ucsys1.AddTDIdx(i);

                this.Ucsys1.AddTD(Bill.No);
                string fileUrl = "";
                //../WorkOpt/GridEdit.aspx?grf=" +Bill.Url + ".grf&t="+DateTime.Now.ToString("yyMMddhh:mm:ss")+" target='_blank'
                if (Bill.HisBillFileType == BillFileType.RuiLang)
                {
                    string name = Bill.Url;

                    name = name.Replace('\\', '-');

                    //if (name.Split('\\').Count() > 2)
                    //{
                    //    string tempName = "";
                    //    foreach (string single in name.Split('\\'))
                    //    {
                    //        tempName += single + "-";
                    //    }
                    //    name = tempName.Substring(0, tempName.Length - 1);
                    //}
                    fileUrl = "<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID +
                              "&DoType=Edit&RefNo=" + Bill.No +
                              "'><img src='../Img/Btn/Edit.gif' border=0/ Editor /a>|<a href='../../../DataUser/CyclostyleFile/" +
                              Bill.Url + ".grf'><img src='../Img/Btn/Save.gif' border=0/>  Template Download </a>|<a href='javascript:openEidt(\"" + name + "\")'  ><img src='../Img/Btn/Edit.gif' /> Edit Template </a>";
                }
                else
                {
                    fileUrl = "<a href='Bill.aspx?FK_Flow=" + this.FK_Flow + "&NodeID=" + this.NodeID +
                            "&DoType=Edit&RefNo=" + Bill.No +
                            "'><img src='../Img/Btn/Edit.gif' border=0/ Editor /a>|<a href='../../DataUser/CyclostyleFile/" +
                            Bill.Url + ".rtf'><img src='../Img/Btn/save.gif' border=0/>  Template Download </a>";
                }
                this.Ucsys1.AddTD("<img src='../Img/Btn/Word.gif' >" + Bill.Name);
                this.Ucsys1.AddTD(fileUrl);
                this.Ucsys1.AddTREnd();
            }
            this.Ucsys1.AddTableEnd();
        }
    }

}