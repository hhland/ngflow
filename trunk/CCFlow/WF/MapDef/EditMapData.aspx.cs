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
using BP.DA;
using BP.En;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class WF_MapDef_EditMapData : System.Web.UI.Page
    {
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            MapData md = new MapData(this.FK_MapData);
            this.Pub1.AddTable();
            //   this.Pub1.AddCaptionLeft(" Fool Form Properties ");

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Information ");
            this.Pub1.AddTDTitle(" Remark ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Serial number ");
            this.Pub1.AddTD(this.FK_MapData);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Name ");
            TextBox tb = new TextBox();
            tb.ID = "TB_" + MapDataAttr.Name;
            tb.Text = md.Name;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Form the main table ");
            tb = new TextBox();
            tb.ID = "TB_" + MapDataAttr.PTable;
            tb.Text = md.PTable;

            this.Pub1.AddTD(tb);
            this.Pub1.AddTD();
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Display width ( Unit px)");
            tb = new TextBox();
            tb.ID = "TB_" + MapDataAttr.TableWidth;
            tb.Text = md.GetValStringByKey(MapDataAttr.TableWidth);
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" If set to 0  Is considered to be 100%.");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Table showing the number of columns ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_Col";
            ddl.Items.Add(new ListItem("4", "4"));
            ddl.Items.Add(new ListItem("6", "6"));
            ddl.Items.Add(new ListItem("8", "8"));
            ddl.Items.Add(new ListItem("10", "10"));
            ddl.Items.Add(new ListItem("12", "12"));
            ddl.SetSelectItem(md.TableCol);
            this.Pub1.AddTD(ddl);

            this.Pub1.AddTD(" Used to control the horizontal layout form fields .");
            this.Pub1.AddTREnd();

            if (md.No.Contains("ND") == true)
            {
                this.Pub1.AddTR();
                this.Pub1.AddTD(" Audit Components ");
                FrmWorkCheck fwc = new FrmWorkCheck(md.No);
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_FWC";
                ddl.Items.Add(new ListItem(" Unavailable ", "0"));
                ddl.Items.Add(new ListItem(" Fillable ", "1"));
                ddl.Items.Add(new ListItem(" Read-only ", "2"));
                ddl.SetSelectItem((int)fwc.HisFrmWorkCheckSta);
                this.Pub1.AddTD(ddl);
                this.Pub1.AddTD(" User node audit work .");
                this.Pub1.AddTREnd();
            }

            //this.Pub1.AddTR();
            //this.Pub1.AddTD(" Form access control scheme ");
            //tb = new TextBox();
            //tb.ID = "TB_" + MapDataAttr.Slns;
            //tb.Text = md.Slns;
            //tb.Columns = 60;
            //this.Pub1.AddTD("colspan=2", tb);
            //this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("colspan=3", " Program description : Format :@0= Default @1=Step1 Sets of programs @2=Step2 Sets of programs @3=Step3 Sets of programs ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTableEnd();

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            MapData md = new MapData(this.FK_MapData);
            md = this.Pub1.Copy(md) as MapData;
            md.TableCol = this.Pub1.GetDDLByID("DDL_Col").SelectedItemIntVal;
            md.Update();

            try
            {
                FrmWorkCheck fwc = new FrmWorkCheck(md.No);
                fwc.HisFrmWorkCheckSta = (FrmWorkCheckSta)this.Pub1.GetDDLByID("DDL_FWC").SelectedItemIntVal;
                fwc.Update();
            }
            catch
            {
            }
            
            BP.Sys.PubClass.WinClose();
        }
    }
}
