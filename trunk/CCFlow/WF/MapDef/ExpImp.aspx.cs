using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.Web;
using BP.DA;
using BP.En;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_ExpImp : BP.Web.WebPage
    {
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
        public string RefNo
        {
            get
            {
                return this.Request.QueryString["RefNo"];
            }
        }
        public string FromMap
        {
            get
            {
                return this.Request.QueryString["FromMap"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "ccfrom: Import and Export ";
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = this.RefNo;
            md.RetrieveFromDBSources();
            switch (this.DoType)
            {
                case "Exp":
                    DataSet ds = md.GenerHisDataSet();
                    string file = this.Request.PhysicalApplicationPath + "\\DataUser\\Temp\\" + this.RefNo + ".xml";
                    ds.WriteXml(file);
                    BP.Sys.PubClass.DownloadFile(file, md.Name + ".xml");
                    this.WinClose();
                    break;
                case "Imp":
                    MapData mdForm = new MapData(this.FromMap);
                    MapData.ImpMapData(this.RefNo, mdForm.GenerHisDataSet(), true);
                    this.WinClose();
                    return;
                case "Share":
                    this.Share();
                    break;
                default:
                    this.BindHome();
                    break;
            }
        }
        public void Share()
        {
            this.Pub1.AddTable();
            this.Pub1.AddCaptionLeftTX(" In the construction ..");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Collection ");
            this.Pub1.AddTDTitle(" Explanation ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Forms category ");
            MapData md = new MapData(this.RefNo);
            TextBox tb = new TextBox();
            tb.ID = "TB_Sort";
            if (string.IsNullOrEmpty(md.FK_FrmSort.Trim()))
            {
                /* No category , Consider a node form */
            }
            else
            {
                SysFormTree fs = new SysFormTree();
                fs.No = md.No;
                fs.RetrieveFromDBSources();
                tb.Text = md.No;
            }
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Form Name ");
            tb = new TextBox();
            tb.ID = "TB_Name";
            tb.Text = md.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Designers ");
            //tb = new TextBox();
            //tb.ID = "TB_Designer";
            //tb.Text = md.Designer;
            //this.Pub1.AddTD(tb);

            //this.Pub1.AddTD();
            //this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Design units ");
            //tb = new TextBox();
            //tb.ID = "TB_DesignerContact";
            //tb.Text = md.DesignerContact;
            //this.Pub1.AddTD(tb);
            //this.Pub1.AddTD();
            //this.Pub1.AddTREnd();

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Contact ");
            //tb = new TextBox();
            //tb.ID = "TB_DesignerContact";
            //tb.Text = md.DesignerContact;
            //tb.Columns = 50;
            //this.Pub1.AddTD(tb);
            //this.Pub1.AddTD();
            //this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD();
            Button btn = new Button();
            btn.CssClass = "Btn";
            btn.ID = "Btn_Save";
            btn.Text = "Share It";
            btn.Click += new EventHandler(btn_ShareIt_Click);
            this.Pub1.AddTD(btn);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

            this.Pub1.AddFieldSet(" Shared Description ");
            this.Pub1.Add(" If you want to share the bulk of the form , Go to Process Designer - Function execution - Export process and form templates , Send e-mail to template@ccflow.org.");
            this.Pub1.AddFieldSetEnd();
        }
        void btn_ShareIt_Click(object sender, EventArgs e)
        {
        }
        public void BindHome()
        {
            this.Pub1.AddFieldSet(" Export Form Template ");
            this.Pub1.AddUL();
            this.Pub1.AddLi("<a href='ExpImp.aspx?DoType=Exp&RefNo=" + this.RefNo + "' target=_blank > Export form templates and download </a>");
            this.Pub1.AddLi("<a href='../Comm/Method.aspx?M=BP.WF.GenerTemplate' > Export all process templates and form template to the server .</a>");
            // this.Pub1.AddLi("<a href='ExpImp.aspx?DoType=Share&RefNo=" + this.RefNo + "'> Share this form to other Internet friends .</a>");
            this.Pub1.AddLi("<a href=\"javascript:alert(' This feature is in construction , Stay tuned .\t\n After this you can export the template file is sent to the :template@ccflow.org.');\" > Share this form to other Internet friends .</a>");
            this.Pub1.AddULEnd();
            this.Pub1.AddFieldSetEnd();

            this.Pub1.AddFieldSet(" Importing from the Internet ");
            this.Pub1.Add("ccflow Process template form template exchange address .<a href=\"javascript:alert(' This feature is in construction , Stay tuned .');\" >http://template.ccflow.org/</a>.");

            TextBox tb = new TextBox();
            tb.Text = "";
            tb.ID = "TB_Net";
            tb.Columns = 50;
            this.Pub1.AddBR(tb);

            Button btn = new Button();
            btn.Text = " Importing ";
            btn.ID = "Btn_Net";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_Imp_Click);
            this.Pub1.Add(btn);
            this.Pub1.AddFieldSetEnd();

            this.Pub1.AddFieldSet(" Importing from this unit ");
            this.Pub1.Add(" Special Note : The import system will clear the current form of information . Form Template (*.xml)");
            HtmlInputFile fu = new HtmlInputFile();
            fu.ID = "F";
            this.Pub1.Add(fu);
            btn = new Button();
            btn.Text = " Importing ";
            btn.ID = "Btn_Local";
            btn.CssClass = "Btn";
            btn.Click += new EventHandler(btn_Imp_Click);
            this.Pub1.Add(btn);
            this.Pub1.AddFieldSetEnd();

            if (string.IsNullOrEmpty(this.FK_Flow) == false)
            {
                this.Pub1.AddFieldSet(" Importing from this process node ");
                DataTable dt = DBAccess.RunSQLReturnTable("SELECT NodeID,Step,Name FROM WF_Node WHERE FK_Flow='" + this.FK_Flow + "'");
                this.Pub1.AddUL();
                foreach (DataRow dr in dt.Rows)
                {
                    this.Pub1.AddLi("ExpImp.aspx?DoType=Imp&FK_Flow=" + this.FK_Flow + "&RefNo=" + this.RefNo + "&FromMap=ND" + dr["NodeID"], " Node ID:" + dr["NodeID"] + ", Step :" + dr["Step"] + "," + dr["Name"].ToString());
                    //  window.location.href = 'ExpImp.aspx?DoType=Imp&FK_Flow=" + fk_flow + "&RefNo=" +refno + "&FromMap=' + fk_Frm;
                    //     this.Pub1.AddLi("<a href=\"javascript:LoadFrm('" + this.FK_Flow + "','" + this.RefNo + "','ND" + dr["NodeID"] + "');\" >" + dr["Name"].ToString() + "</a>");
                    //  this.Pub1.AddLi("<a href=\"javascript:LoadFrm('" + this.FK_Flow + "','" + this.RefNo + "','ND" + dr["NodeID"] + "');\" >" + dr["Name"].ToString() + "</a>");
                }
                this.Pub1.AddULEnd();
                this.Pub1.AddFieldSetEnd();
            }

            //  Check for the process ID .
            //this.Pub1.AddFieldSet(" Importing from this process node form ");
            //this.Pub1.Add(" Special Note : The import system will clear the current form of information .");
            //this.Pub1.AddBR(" Form Template (*.xml)");
            //HtmlInputFile fu = new HtmlInputFile();
            //fu.ID = "F";
            //this.Pub1.Add(fu);
            //Button btn = new Button();
            //btn.Text = " The import ";
            //btn.Click += new EventHandler(btn_Click);
            //this.Pub1.Add(btn);
            //this.Pub1.AddFieldSetEnd();
        }
        void btn_Imp_Click(object sender, EventArgs e)
        {
            try
            {
                string path = BP.Sys.SystemConfig.PathOfTemp;  
                if (System.IO.Directory.Exists(path) == false)
                    System.IO.Directory.CreateDirectory(path);

                string file =  path+ this.RefNo + ".xml";

                Button btn = sender as Button;
                if (btn.ID == "Btn_Local")
                {
                    HtmlInputFile myfu = this.Pub1.FindControl("F") as HtmlInputFile;
                    myfu.PostedFile.SaveAs(file);
                }

                if (btn.ID == "Btn_Net")
                {
                    string url = this.Pub1.GetTextBoxByID("TB_Net").Text;
                    if (string.IsNullOrEmpty(url))
                    {
                        this.Alert(" Please enter url.");
                        return;
                    }
                    string context = BP.DA.DataType.ReadURLContext(url, 9999, System.Text.Encoding.UTF32);
                    if (context.Contains("Sys_MapAttr") == false)
                        throw new Exception(" Error reading file may be illegal url.\t\n" + url);
                    BP.DA.DataType.SaveAsFile(file, context);
                }

                try
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(file);
                    BP.Sys.MapData.ImpMapData(this.RefNo, ds, true);
                    this.WinClose();
                }
                catch (Exception ex)
                {
                    throw new Exception("@ Import Error :" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}