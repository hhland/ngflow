using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BP.Sys;
using BP.En;
using BP.Web;
using BP.Web.UC;
namespace CCFlow.WF.MapDef
{

    public partial class Comm_MapDef_SFTable : BP.Web.WebPage
    {
        public new string DoType
        {
            get
            {
                return this.Request.QueryString["DoType"];
            }
        }
        public string IDX
        {
            get
            {
                return this.Request.QueryString["IDX"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SFTable main = new SFTable();
            if (this.RefNo != null)
            {
                main.No = this.RefNo;
                main.Retrieve();
            }
            this.BindSFTable(main);
        }
        public void BindSFTable(SFTable en)
        {

            string star = "<font color=red><b>(*)</b></font>";
            this.Ucsys1.AddTable();
            if (this.RefNo == null)
                this.Ucsys1.AddCaption("<a href='Do.aspx?DoType=AddF&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "'> Add a new field guide </a> - <a href='Do.aspx?DoType=AddSFTable&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "'> Foreign key </a> -  New Table ");
            else
                this.Ucsys1.AddCaption("<a href='Do.aspx?DoType=AddF&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "'> Add a new field guide </a> - <a href='Do.aspx?DoType=AddSFTable&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "'> Foreign key </a> -  Edit Table ");

            if (this.RefNo == null)
                this.Title = " New Table ";
            else
                this.Title = " Edit Table ";

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTDTitle(" Project ");
            this.Ucsys1.AddTDTitle(" Collection ");
            this.Ucsys1.AddTDTitle(" Remark ");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(  " Table English name " + star);
            BP.Web.Controls.TB tb = new BP.Web.Controls.TB();
            tb.ID = "TB_No";
            tb.Text = en.No;
            if (this.RefNo == null)
                tb.Enabled = true;
            else
                tb.Enabled = false;

            if (tb.Text == "")
                tb.Text = "SF_";

            this.Ucsys1.AddTD(tb);
            this.Ucsys1.AddTD( " Enter : The new table name or table name already exists ");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD(" Table Chinese name " + star);
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_Name";
            tb.Text = en.Name;
            this.Ucsys1.AddTD(tb);
            this.Ucsys1.AddTD();
            this.Ucsys1.AddTREnd();


            this.Ucsys1.AddTR();
            this.Ucsys1.AddTD( " Description " + star);
            tb = new BP.Web.Controls.TB();
            tb.ID = "TB_TableDesc";
            tb.Text = en.TableDesc;
            this.Ucsys1.AddTD(tb);
            this.Ucsys1.AddTD("");
            this.Ucsys1.AddTREnd();

            this.Ucsys1.AddTR();
            this.Ucsys1.Add("<TD colspan=3 align=center>");
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.CssClass = "Btn";
            if (this.RefNo == null)
                btn.Text =   " Create ";
            else
                btn.Text =  " Save ";

            btn.Click += new EventHandler(btn_Save_Click);
            this.Ucsys1.Add(btn);


            btn = new Button();
            btn.ID = "Btn_Edit";
            btn.CssClass = "Btn";
            btn.Text =  " Edit Data "; // " Edit Data "
            if (this.RefNo == null)
                btn.Enabled = false;
            if (en.IsClass)
                btn.Attributes["onclick"] = "WinOpen('../Search.aspx?EnsName=" + en.No + "','dg' ); return false;";
            else
                btn.Attributes["onclick"] = "WinOpen('SFTableEditData.aspx?RefNo=" + this.RefNo + "','dg' ); return false;";
            this.Ucsys1.Add(btn);


            btn = new Button();
            btn.ID = "Btn_Add";
            btn.CssClass = "Btn";

            btn.Text =  " Added to the form "; ; // " Added to the form ";
            btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
            btn.Click += new EventHandler(btn_Add_Click);
            if (this.RefNo == null)
                btn.Enabled = false;

            this.Ucsys1.Add(btn);
            btn = new Button();
            btn.ID = "Btn_Del";
            btn.CssClass = "Btn";

            btn.Text =   " Delete ";
            btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
            if (this.RefNo == null)
                btn.Enabled = false;

            btn.Click += new EventHandler(btn_Del_Click);
            this.Ucsys1.Add(btn);
            this.Ucsys1.Add("</TD>");
            this.Ucsys1.AddTREnd();
            this.Ucsys1.AddTableEnd();
        }
        void btn_Add_Click(object sender, EventArgs e)
        {
            SFTable table = new SFTable(this.RefNo);
            if (table.HisEns.Count == 0)
            {
                this.Alert(" The table [" + this.RefNo + "] No data , You need to maintain data ");
                return;
            }

            this.Response.Redirect("Do.aspx?DoType=AddSFTableAttr&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "&RefNo=" + this.RefNo, true);
            this.WinClose();
            return;
        }
        void btn_EditData_Click(object sender, EventArgs e)
        {
            //this.Response.Redirect("SFTable.aspx?DoType=Edit&MyPK=" + this.MyPK + "&IDX=" + this.IDX + "&RefNo=" + this.RefNo, true);
            //this.WinClose();
            return;
        }
        void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SFTable main = new SFTable();
                main = (SFTable)this.Ucsys1.Copy(main);

                if (main.No.Length == 0 || main.Name.Length == 0)
                    throw new Exception(" Number and name can not be empty ");

                try
                {
                    main.HisEns.GetNewEntity.CheckPhysicsTable();
                }
                catch
                {
                }


                if (this.RefNo == null)
                {
                    main.No = this.Ucsys1.GetTBByID("TB_No").Text;

                    if (main.IsExits)
                    {
                        string sql = "select No,Name from " + main.No + " WHERE 1=2";
                        try
                        {
                            BP.DA.DBAccess.RunSQLReturnTable(sql);
                        }
                        catch
                        {
                            this.Alert(  " Error : Table or view does not exist No,Name Column does not conform to the agreed rules   Key=" + main.No);
                            return;
                        }
                    }
                }
                else
                {
                    main.No = this.RefNo;
                    main.Retrieve();
                    main = (SFTable)this.Ucsys1.Copy(main);
                    if (main.No.Length == 0 || main.Name.Length == 0)
                        throw new Exception(" Number and name can not be empty ");
                }

                if (main.Name.Length == 0)
                    throw new Exception(" Number and name can not be empty ");

                if (main.TableDesc.Length == 0)
                    throw new Exception(" Description can not be empty ");

                if (this.RefNo == null)
                {
                    //if (main.No.Contains("SF_") == false)
                    //    throw new Exception(" Physical table does not conform to the naming rules , Must  SF_  Beginning .");
                    //  main.FK_Val = main.No.Replace("SF_", "FK_");
                    main.FK_Val = main.No; //.Replace("SF_", "FK_");
                }

                //string cfgVal = "";
                //int idx = -1;
                //while (idx < 19)
                //{
                //    idx++;
                //    string t = this.Ucsys1.GetTBByID("TB_" + idx).Text.Trim();
                //    if (t.Length == 0)
                //        continue;
                //    cfgVal += "@" + idx + "=" + t;
                //}
                //main.CfgVal = cfgVal;
                //if (main.CfgVal == "")
                //    throw new Exception(" Error , You must enter the table , Please refer to Help .");
                // main.IsDel = true;
                main.Save();


                // Regenerate 
                this.Response.Redirect("SFTable.aspx?RefNo=" + main.No + "&MyPK=" + this.MyPK + "&IDX=" + this.IDX, true);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                //  This type is used to check whether ?
                MapAttrs attrs = new MapAttrs();
                QueryObject qo = new QueryObject(attrs);
                qo.AddWhere(MapAttrAttr.MyDataType, (int)FieldTypeS.FK);
                qo.addAnd();
                qo.AddWhere(MapAttrAttr.KeyOfEn, this.RefNo);
                int i = qo.DoQuery();
                if (i == 0)
                {
                    BP.Sys.SFTable m = new SFTable();
                    m.No = this.RefNo;
                    m.Delete();
                    this.ToWFMsgPage(" Foreign keys deleted successfully ");
                    return;
                }

                string msg = " Error : The following data has been quoted a foreign key, you can not delete it .";
                foreach (MapAttr attr in attrs)
                    msg += "\t\n" + attr.Field + "" + attr.Name + " 表" + attr.FK_MapData;

                throw new Exception(msg);
            }
            catch (Exception ex)
            {
                this.ToErrorPage(ex.Message);
            }

        }
    }

}