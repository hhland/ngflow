using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.Sys;
using BP.En;
using BP.Web.Controls;
using BP.DA;
using BP.Web;
using BP.Tools;

namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_GroupField : BP.Web.WebPage
    {
        public string FromGroupField
        {
            get
            {
                return this.Request.QueryString["FromGroupField"];
            }
        }
        public new string RefNo
        {
            get
            {
                string s = this.Request.QueryString["RefNo"];
                if (s == null)
                    return "t";
                else
                    return s;
            }
        }
        void btn_Check_Click(object sender, EventArgs e)
        {
            string sta = this.Pub1.GetTBByID("TB_Sta").Text.Trim();
            if (sta.Length == 0)
            {
                this.Alert(" Audit jobs can not be empty ");
                return;
            }

            string Prx = this.Pub1.GetTBByID("TB_Prx").Text.Trim();
            if (Prx.Length == 0)
            {
                Prx = chs2py.convert(sta);
            }

            MapAttr attr = new MapAttr();
            int i = attr.Retrieve(MapAttrAttr.FK_MapData, this.RefNo, MapAttrAttr.KeyOfEn, Prx + "_Note");
            i += attr.Retrieve(MapAttrAttr.FK_MapData, this.RefNo, MapAttrAttr.KeyOfEn, Prx + "_Checker");
            i += attr.Retrieve(MapAttrAttr.FK_MapData, this.RefNo, MapAttrAttr.KeyOfEn, Prx + "_RDT");

            if (i > 0)
            {
                this.Alert(" Prefixes have been used :" + Prx + " ,  Please confirm that you add this audit group or , Please replace other prefixes .");
                return;
            }

            GroupField gf = new GroupField();
            gf.Lab = sta;
            gf.EnName = this.RefNo;
            gf.Insert();

            attr = new MapAttr();
            attr.FK_MapData = this.RefNo;
            attr.KeyOfEn = Prx + "_Note";
            attr.Name = " Audit opinion "; // sta;  // this.ToE("CheckNote", " Audit opinion ");
            attr.MyDataType = DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.UIIsEnable = true;
            attr.UIIsLine = true;
            attr.MaxLen = 4000;
            attr.ColSpan = 4;
            attr.GroupID = gf.OID;
            attr.UIHeight = 23 * 3;
            attr.IDX = 1;
            attr.Insert();

            attr = new MapAttr();
            attr.FK_MapData = this.RefNo;
            attr.KeyOfEn = Prx + "_Checker";
            attr.Name = " Reviewer ";// " Reviewer ";
            attr.MyDataType = DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.MaxLen = 50;
            attr.MinLen = 0;
            attr.UIIsEnable = true;
            attr.UIIsLine = false;
            attr.DefVal = "@WebUser.Name";
            attr.UIIsEnable = false;
            attr.GroupID = gf.OID;
            attr.IsSigan = true;
            attr.IDX = 2;
            attr.Insert();

            attr = new MapAttr();
            attr.FK_MapData = this.RefNo;
            attr.KeyOfEn = Prx + "_RDT";
            attr.Name = " Review Date "; // " Review Date ";
            attr.MyDataType = DataType.AppDateTime;
            attr.UIContralType = UIContralType.TB;
            attr.UIIsEnable = true;
            attr.UIIsLine = false;
            attr.DefVal = "@RDT";
            attr.UIIsEnable = false;
            attr.GroupID = gf.OID;
            attr.IDX = 3;
            attr.Insert();
            this.WinCloseWithMsg(" Saved successfully "); // " Increase the success , You can adjust its position and modify the label field ."
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = " Field Grouping ";

            switch (this.DoType)
            {
                case "FunList":
                    this.Pub1.AddFieldSet(" Field Grouping Wizard ");
                    this.Pub1.AddUL();
                    this.Pub1.AddLi("<b><a href='GroupField.aspx?DoType=NewGroup&RefNo=" + this.RefNo + "'> New Blank Field Grouping </a></b><br><font color=green> Blank Field Grouping , After the establishment can put into this grouping related fields in .</font>");
                    this.Pub1.AddLi("<b><a href='GroupField.aspx?DoType=NewCheckGroup&RefNo=" + this.RefNo + "'> New Audit Group </a></b><br><font color=green> The system will let you enter information audit , And create an audit group .</font>");
                    this.Pub1.AddLi("<b><a href='GroupField.aspx?DoType=NewEvalGroup&RefNo=" + this.RefNo+"'> Creating quality assessment field work groups </a></b><br><font color=green> Creating quality assessment : EvalEmpNo,EvalEmpName,EvalCent,EvalNote 4 A necessary fields .</font>");
                    this.Pub1.AddULEnd();
                    this.Pub1.AddFieldSetEnd();
                    return;
                case "NewCheckGroup":
                    this.Pub1.AddFieldSet("<a href=GroupField.aspx?DoType=FunList&RefNo=" + this.RefNo + " > Field Grouping Wizard </a> - " + " Audit Group ");
                    TB tbc = new TB();
                    tbc.ID = "TB_Sta";
                    this.Pub1.Add(" Audit jobs <font color=red>*</font>");
                    this.Pub1.Add(tbc);
                    this.Pub1.AddBR("<font color=green> Such as : Commissaires review , Chief Audit , General Manager of Audit .</font>");
                    this.Pub1.AddBR();

                    tbc = new TB();
                    tbc.ID = "TB_Prx";
                    this.Pub1.Add(" Prefix field :");
                    this.Pub1.Add(tbc);
                    this.Pub1.AddBR("<font color=green> Used to automatically create fields , Please enter the letters or alphanumeric combinations . The system automatically generates the field based on your input .如:XXX_Note, Audit opinion .XXX_RDT Review time .XXX_Checker Reviewer , Automatic Pinyin system is represented as empty .</font>");
                    this.Pub1.AddBR();
                    this.Pub1.AddHR();
                    Btn btnc = new Btn();
                    btnc.Click += new EventHandler(btn_Check_Click);
                    btnc.Text = " Save ";

                    this.Pub1.Add(btnc);
                    this.Pub1.AddFieldSetEnd();
                    return;
                case "NewEvalGroup":

                    GroupField gf = new GroupField();
                    gf.Lab = " Work quality assessment ";
                    gf.EnName = this.RefNo;
                    gf.Insert();

                    MapAttr attr = new MapAttr();
                    attr.FK_MapData = this.RefNo;
                    attr.KeyOfEn = BP.WF.WorkSysFieldAttr.EvalNote; 
                    attr.Name = " Audit opinion "; 
                    attr.MyDataType = DataType.AppString;
                    attr.UIContralType = UIContralType.TB;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = true;
                    attr.MaxLen = 500;
                    attr.GroupID = gf.OID;
                    attr.UIHeight = 23 ;
                    attr.ColSpan = 4;
                    attr.IDX = 1;
                    attr.Insert();

                    attr = new MapAttr();
                    attr.FK_MapData = this.RefNo;
                    attr.KeyOfEn = BP.WF.WorkSysFieldAttr.EvalCent; 
                    attr.Name = " Scores "; 
                    attr.MyDataType = DataType.AppFloat;
                    attr.UIContralType = UIContralType.TB;
                    attr.MaxLen = 50;
                    attr.MinLen = 0;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = false;
                    attr.DefVal = "0";
                    attr.UIIsEnable = false;
                    attr.GroupID = gf.OID;
                    attr.IsSigan = true;
                    attr.IDX = 2;
                    attr.Insert();

                    attr = new MapAttr();
                    attr.FK_MapData = this.RefNo;
                    attr.KeyOfEn = BP.WF.WorkSysFieldAttr.EvalEmpNo; 
                    attr.Name = " Number of people being evaluated "; 
                    attr.MyDataType = DataType.AppString;
                    attr.UIContralType = UIContralType.TB;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = false;
                    attr.DefVal = "";
                    attr.UIIsEnable = false;
                    attr.GroupID = gf.OID;
                    attr.IDX = 3;
                    attr.Insert();

                     attr = new MapAttr();
                    attr.FK_MapData = this.RefNo;
                    attr.KeyOfEn = BP.WF.WorkSysFieldAttr.EvalEmpName; 
                    attr.Name = " People are assessed name ";  
                    attr.MyDataType = DataType.AppString;
                    attr.UIContralType = UIContralType.TB;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = false;
                    attr.DefVal = "";
                    attr.UIIsEnable = false;
                    attr.GroupID = gf.OID;
                    attr.IDX = 4;
                    attr.Insert();
                    this.WinCloseWithMsg(" Saved successfully "); // " Increase the success , You can adjust its position and modify the label field ."
                    return;
                case "NewGroup":
                    GroupFields mygfs = new GroupFields(this.RefNo);
                    GroupField gf1 = new GroupField();
                    gf1.Idx = mygfs.Count;
                    gf1.Lab = " New Field grouping "; // " New Field grouping ";
                    gf1.EnName = this.RefNo;
                    gf1.Insert();
                    this.Response.Redirect("GroupField.aspx?RefNo=" + this.RefNo + "&RefOID=" + gf1.OID, true);
                    return;
                default:
                    break;
            }

            #region edit operation
            GroupField en = new GroupField(this.RefOID);
            this.Pub1.Add("<Table border=0 align=center>");
            this.Pub1.AddCaptionLeft(" Field Grouping ");
            this.Pub1.AddTR();

            this.Pub1.AddTD(" Group Name ");

            TB tb = new TB();
            tb.ID = "TB_Lab_" + en.OID;
            tb.Text = en.Lab;
            tb.Columns = 50;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTREnd();

            this.Pub1.AddTRSum();
            this.Pub1.Add("<TD align=center colspan=2>");
            Btn btn = new Btn();
            btn.Text = " Save ";
            btn.ID = "Btn_Save";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
            btn = new Btn();
            btn.Text = " Save and Close ";
            btn.ID = "Btn_SaveAndClose";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            btn = new Btn();
            btn.Text = " New Field ";
            btn.ID = "Btn_NewField";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            //btn = new Btn();
            //btn.Text = this.ToE("CopyField", " Copy field ");
            //btn.ID = "Btn_CopyField";
            //btn.Click += new EventHandler(btn_Click);
            //this.Pub1.Add(btn);


            btn = new Btn();
            btn.Text = " Delete ";
            btn.ID = "Btn_Delete";
            btn.Click += new EventHandler(btn_del_Click);
            btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";

            this.Pub1.Add(btn);

            this.Pub1.Add("</TD>");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();
            this.Pub1.Add(" The similar field in a large unit Gerry , As used not just to show any significance calculated .");
            #endregion

        }

        void btnC_Click(object sender, EventArgs e)
        {
            BP.WF.Node mynd = new BP.WF.Node(this.RefNo);
            BP.WF.Nodes nds = new BP.WF.Nodes(mynd.FK_Flow);
            foreach (BP.WF.Node nd in nds)
            {
                if ("ND" + nd.NodeID == this.RefNo)
                    continue;

                GroupFields gfs = new GroupFields("ND" + nd.NodeID);
                foreach (GroupField gf in gfs)
                {
                    string id = "CB_" + gf.OID;
                    if (this.Pub1.GetCBByID(id).Checked == false)
                        continue;

                    MapAttrs attrs = new MapAttrs();
                    attrs.Retrieve(MapAttrAttr.GroupID, gf.OID);
                    if (attrs.Count == 0)
                        continue;

                }
            }
        }
        void btn_del_Click(object sender, EventArgs e)
        {
            MapAttrs attrs = new MapAttrs();
            attrs.Retrieve(MapAttrAttr.GroupID, this.RefOID);
            foreach (MapAttr attr in attrs)
            {
                if (attr.HisEditType != EditType.Edit)
                    continue;
                if (attr.KeyOfEn == "FID")
                    continue;

                attr.Delete();
            }

            MapDtls dtls = new MapDtls();
            dtls.Retrieve(MapDtlAttr.GroupID, this.RefOID);
            foreach (MapDtl dtl in dtls)
                dtl.Delete();

            GroupField gf = new GroupField(this.RefOID);
            gf.Delete();
            this.WinClose();// (" Deleted successfully .");
        }

        void btn_Click(object sender, EventArgs e)
        {

            GroupField en = new GroupField(this.RefOID);
            en.Lab = this.Pub1.GetTBByID("TB_Lab_" + en.OID).Text;
            en.Update();

            Btn btn = sender as Btn;
            switch (btn.ID)
            {
                case "Btn_SaveAndClose":
                    this.WinClose();
                    break;
                case "Btn_NewField":
                    this.Session["GroupField"] = this.RefOID;
                    this.Response.Redirect("Do.aspx?DoType=AddF&MyPK=" + this.RefNo + "&GroupField=" + this.RefOID, true);
                    break;
                case "Btn_CopyField":
                    this.Response.Redirect("CopyFieldFromNode.aspx?FK_Node=" + this.RefNo + "&GroupField=" + this.RefOID, true);
                    break;
                default:
                    this.Response.Redirect("GroupField.aspx?RefNo=" + this.RefNo + "&RefOID=" + this.RefOID, true);
                    break;
            }
        }
    }

}