using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.DA;

namespace CCFlow.WF.Admin.FindWorker
{
    public partial class SpecEmps : BP.Web.WebPage
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
                    this.Response.Redirect("SpecEmps.aspx?S1=" + en.SortVal1 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
                    return;
                }
            }

            #region 1级
            this.UCS1.AddFieldSet(" Personnel range of other parameters ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_S1"; //  The first latitude .
            ddl.AutoPostBack = true;
            ddl.Items.Add(new ListItem(" Current treatment of people ", "0"));
            ddl.Items.Add(new ListItem(" Processors specified node ", "1"));
            ddl.Items.Add(new ListItem(" Designated by the form field handling people ", "2"));
            if (this.S1 != null)
                ddl.SetSelectItem(this.S1);
            else
                ddl.SetSelectItem(en.SortVal1);

            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.UCS1.Add(" Select staff ");
            this.UCS1.Add(ddl);
            #endregion 2级

            #region 2级
            if (this.S1 == "1")
            {
                this.UCS2.AddFieldSet(" You need to specify a node .");
                this.UCS2.Add(" Select the node ");
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_V1"; //  The first argument of latitude .
                ddl.BindSQL("SELECT NodeID AS No, Name FROM WF_Node WHERE FK_Flow='" + this.FK_Flow + "' ORDER BY NODEID ",
                    "No", "Name", "20");
                this.UCS2.Add(ddl);
                ddl.SetSelectItem(en.TagVal1); //  The first argument of latitude .
                this.UCS2.AddFieldSetEnd();
            }

            if (this.S1 == "2")
            {
                this.UCS2.AddFieldSet(" Select the form field ");
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_V1";  // The first argument of latitude 
                ddl.BindSQL("SELECT KeyOfEn as No, KeyOfEn+' - '+Name as Name FROM Sys_MapAttr WHERE FK_MapData='ND" + int.Parse(this.FK_Flow) + "Rpt' AND MyDataType=1 ",
                    "No", "Name", "20");

                this.UCS2.Add(" Select a field ");
                this.UCS2.Add(ddl);
                ddl.SetSelectItem(en.TagVal1); //  The first argument of latitude .
                this.UCS2.AddFieldSetEnd();
            }
            #endregion 2级


            #region  Sector-wide 
            this.UCS3.AddFieldSet(" Sectoral nature of the range ");
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_DutyType";  //第3 Latitude parameters 
            ddl.BindSQL("SELECT No,Name FROM Port_DeptType",
                "No", "Name", "20");

            this.UCS3.Add(" Please select ");
            this.UCS3.Add(ddl);
            ddl.SetSelectItem(en.TagVal2); // 第3 Latitude parameters .
            this.UCS3.AddFieldSetEnd();
            #endregion  Sector-wide 


            #region  Binding the second half of .

            if (string.IsNullOrEmpty(en.SortVal2))
            {
                en.SortVal3 = "0";
            }

            this.UCS4.AddFieldSet(" Personnel range of other parameters ");
            //  Other configuration information .
            BP.Web.Controls.RadioBtn rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_0";
            rb.Text = " All members of the ";
            if (en.SortVal3 == "0")
                rb.Checked = true;

            this.UCS4.Add(rb);
            this.UCS4.AddHR();

            // Specific job level executives 
            rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_1";
            rb.Text = " Specific jobs ";
            if (en.SortVal3 == "1")
                rb.Checked = true;

            this.UCS4.Add(rb);
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_Duty";
            ddl.BindSQL("SELECT No,Name Name FROM Port_Duty ",
                "No", "Name", "20");
            this.UCS4.Add(ddl);
            ddl.SetSelectItem(en.TagVal3);
            this.UCS4.AddHR();

            //  Specific leadership positions 
            rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_2";
            rb.Text = " Specific job ";
            if (en.SortVal2 == "2")
                rb.Checked = true;

            this.UCS4.Add(rb);
            ddl = new BP.Web.Controls.DDL();
            ddl.BindSQL("SELECT No, Name FROM Port_Station ", "No", "Name", "20");
            ddl.SetSelectItem(en.TagVal3);
            ddl.ID = "DDL_Station";
            this.UCS4.Add(ddl);
            #endregion  Binding the second half of .

            this.UCS4.AddHR();
            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.UCS4.Add(btn);
        }


        void btn_Click(object sender, EventArgs e)
        {
            FindWorkerRole fwr = new FindWorkerRole();
            fwr.CheckPhysicsTable();
            fwr.OID = this.RefOID;
            if (fwr.OID != 0)
                fwr.Retrieve();

            #region  Processing Description .
            //  One 
            fwr.SortVal0 = this.PageID;
            fwr.SortText0 = " Find colleagues ";

            //  Two ,  The way :  Form within , Specified node .
            fwr.SortVal1 =  this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            fwr.SortText1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItem.Text;

            // 3级, Sectoral nature .
            fwr.TagVal2 = this.UCS3.GetDDLByID("DDL_DutyType").SelectedItemStringVal;
            fwr.TagText3 = this.UCS3.GetDDLByID("DDL_DutyType").SelectedItem.Text;

            fwr.SortVal2 = " Sector-wide ";
            fwr.SortText2 = " Sector-wide ";


            // 4级.
            if (this.UCS4.GetRadioBtnByID("RB_0").Checked)
            {
                fwr.SortVal3 = "0";
                fwr.SortText3 = this.UCS4.GetRadioBtnByID("RB_0").Text;
            }

            if (this.UCS4.GetRadioBtnByID("RB_1").Checked)
            {
                fwr.SortVal3 = "1";
                fwr.SortText3 = this.UCS4.GetRadioBtnByID("RB_1").Text;

                fwr.TagVal3 = this.UCS4.GetDDLByID("DDL_Duty").SelectedItemStringVal;
                fwr.TagText3 = this.UCS4.GetDDLByID("DDL_Duty").SelectedItem.Text;
            }

            if (this.UCS4.GetRadioBtnByID("RB_2").Checked)
            {
                fwr.SortVal3 = "2";
                fwr.SortText3 = this.UCS4.GetRadioBtnByID("RB_2").Text;

                fwr.TagVal3 = this.UCS4.GetDDLByID("DDL_Station").SelectedItemStringVal;
                fwr.TagText3 = this.UCS4.GetDDLByID("DDL_Station").SelectedItem.Text;
            }
            #endregion  Processing Description .

            #region  Processing data 

            try
            {
                //  Get 1 Latitude parameters . // Specific node , Specific fields .
                fwr.TagVal1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;
                fwr.TagText1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItem.Text;
            }
            catch
            {
                fwr.TagVal1 = ""; // this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;
                fwr.TagText1 = ""; // this.UCS2.GetDDLByID("DDL_V1").SelectedItem.Text;
            }

            //  Get 2 Latitude parameters . Sectoral nature of the range .
            fwr.TagVal2 = this.UCS3.GetDDLByID("DDL_DutyType").SelectedItemStringVal;
            fwr.TagText2 = this.UCS3.GetDDLByID("DDL_DutyType").SelectedItem.Text;
            #endregion  Processing data 

            fwr.FK_Node = int.Parse(this.FK_Node);
            fwr.Save();

            // Set bpm Status .
            Node nd = new Node(this.FK_Node);
            nd.HisDeliveryWay = DeliveryWay.ByCCFlowBPM;
            nd.DirectUpdate();

            this.Response.Redirect("List.aspx?" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
            //this.WinCloseWithMsg(" Saved successfully ...");
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            this.Response.Redirect("SpecEmps.aspx?S1=" + s1 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
        }
    }
}