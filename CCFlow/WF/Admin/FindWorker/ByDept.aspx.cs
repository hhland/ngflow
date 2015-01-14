using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.WF.Template;
using BP.DA;

namespace CCFlow.WF.Admin.FindWorker
{
    public partial class ByDept : BP.Web.WebPage
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                string val = this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(val))
                    return "001";
                return val;
            }
        }
        public string FK_Node
        {
            get
            {
                string val = this.Request.QueryString["FK_Node"];
                if (string.IsNullOrEmpty(val))
                    return "101";
                return val;
            }
        }
        public string S1
        {
            get
            {
                string val = this.Request.QueryString["S1"];
                if (string.IsNullOrEmpty(val))
                    return null;
                return val;
            }
        }
        public string S2
        {
            get
            {
                string val = this.Request.QueryString["S2"];
                if (string.IsNullOrEmpty(val))
                    return null;
                return val;
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            FindWorkerRole en = new FindWorkerRole();
            en.OID = this.RefOID;
            if (en.OID != 0)
                en.Retrieve();

            if (this.RefOID != 0 && this.S1 == null)
            {
                if (en.SortVal1 != "0")
                {
                    this.Response.Redirect("ByDept.aspx?S1=" + en.SortVal1 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
                    return;
                }
            }
            #region 1级

            
            this.UCS1.AddFieldSet(" Department set type ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_S1"; //  The first latitude .
            ddl.AutoPostBack = true;
            ddl.Items.Add(new ListItem(" Designated by position ", "0"));
            ddl.Items.Add(new ListItem(" Designated by post ", "1"));
            ddl.Items.Add(new ListItem(" All members of the specified department ", "2"));
            if (this.S1 != null)
                en.SortVal1=this.S1;

            if (string.IsNullOrEmpty(en.SortVal1))
                en.SortVal1 = "0";

                ddl.SetSelectItem(en.SortVal1);

            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.UCS1.Add(" Choose ");
            this.UCS1.Add(ddl);
            #endregion 2级

            #region 2级
            this.UCS2.AddFieldSet(" Select Sector .");
            this.UCS2.Add(" Department ");
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_V1"; //  The first argument of latitude .
            ddl.BindSQL("SELECT No,Name  FROM Port_Dept   ORDER BY No ",
                "No", "Name", BP.Web.WebUser.FK_Dept);
            this.UCS2.Add(ddl);
            ddl.SetSelectItem(en.TagVal1); // 第2 Latitude parameters .
            this.UCS2.AddFieldSetEnd();
            #endregion 2级

            #region 2级
            if (en.SortVal1 == "0")
            {
                this.UCS2.AddFieldSet(" Select duties .");
                this.UCS2.Add(" Position ");
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_V2"; //  The first argument of latitude .
                ddl.BindSQL("SELECT No,Name  FROM Port_Duty   ORDER BY No ",
                    "No", "Name", "01");
                this.UCS2.Add(ddl);
                ddl.SetSelectItem(en.TagVal2); // 第3 Latitude parameters .
                this.UCS2.AddFieldSetEnd();
            }

            if (en.SortVal1 == "1")
            {
                this.UCS2.AddFieldSet(" Select posts .");
                this.UCS2.Add(" Post ");
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_V2"; //  The first argument of latitude .
                ddl.BindSQL("SELECT No,Name  FROM Port_Station   ORDER BY No ",
                    "No", "Name", "01");
                this.UCS2.Add(ddl);
                ddl.SetSelectItem(en.TagVal2); // 第3 Latitude parameters .
                this.UCS2.AddFieldSetEnd();
            }
            #endregion 2级

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.UCS3.Add(btn);
        }

        void btn_Click(object sender, EventArgs e)
        {
            FindWorkerRole fwr = new FindWorkerRole();
            fwr.OID = this.RefOID;
            if (fwr.OID != 0)
                fwr.Retrieve();

            #region  Processing Description .
            //  One 
            fwr.SortVal0 = this.PageID;
            fwr.SortText0 = " By sector ";

            //  Two .
            fwr.SortVal1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            fwr.SortText1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItem.Text;

            //  Two values ( Choose department )
            fwr.TagVal1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;
            fwr.TagText1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItem.Text;

            if (fwr.SortVal1 == "2")
            {
                fwr.TagVal2 = "";
                fwr.TagText2 = "";
            }
            else
            {
                try
                {
                    //  Two values ( Select the job or jobs )
                    fwr.TagVal2 = this.UCS2.GetDDLByID("DDL_V2").SelectedItemStringVal;
                    fwr.TagText2 = this.UCS2.GetDDLByID("DDL_V2").SelectedItem.Text;
                }
                catch
                {
                }
            }
            #endregion  Processing Description .

            fwr.FK_Node = int.Parse(this.FK_Node);
            fwr.Save();

            // Set bpm Status .
            Node nd = new Node(this.FK_Node);
            nd.HisDeliveryWay = DeliveryWay.ByCCFlowBPM;
            nd.DirectUpdate();

            this.Response.Redirect("List.aspx?" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            string s2 = this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;

            this.Response.Redirect("ByDept.aspx?S1=" + s1 + "&S2=" + s2 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
          //  this.Response.Redirect("ByDept.aspx?S1=" + s1 + "&S2=" + s2 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);

        }
    }
}