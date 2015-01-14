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
    public partial class Leader : BP.Web.WebPage
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
               string val= this.Request.QueryString["FK_Flow"];
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
                string val= this.Request.QueryString["S1"];
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
                    this.Response.Redirect("Leader.aspx?S1=" + en.SortVal1 + "&RefOID=" + this.RefOID + "&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node, true);
                    return;
                }
            }

            #region 1级
            this.UCS1.AddFieldSet(" Personnel range of other parameters ");
            BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_S1"; //  The first latitude .
            ddl.AutoPostBack = true;
            ddl.Items.Add(new ListItem(" Current author ", "0"));
            ddl.Items.Add(new ListItem(" The author of the specified node ", "1"));
            ddl.Items.Add(new ListItem(" Form fields specified by the author ", "2"));
            if (this.S1 != null)
                ddl.SetSelectItem(this.S1);
            else
                ddl.SetSelectItem(en.SortVal1);

            ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);
            this.UCS1.Add(" Select staff ");
            this.UCS1.Add(ddl);
            #endregion 2级

            #region 2级
            if (this.S1 == "1" )
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

            #region  Binding the second half of .
            this.UCS3.AddFieldSet(" Personnel range of other parameters ");
            //  Other configuration information .
            BP.Web.Controls.RadioBtn rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_0";
            rb.Text = " Direct supervisor ";
            if (en.SortVal2 == "0")
                rb.Checked = true;

            this.UCS3.Add(rb);
            this.UCS3.AddHR();

            // Specific job level executives 
            rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_1";
            rb.Text = " Specific job level executives ";
            if (en.SortVal2 == "1")
                rb.Checked = true;

            this.UCS3.Add(rb);
            ddl = new BP.Web.Controls.DDL();
            ddl.ID = "DDL_DutyLevel";
            ddl.BindSQL("SELECT distinct DutyLevel AS No, DutyLevel as Name FROM Port_DeptEmp WHERE DutyLevel IS NOT NULL",
                "No", "Name", "20");
            this.UCS3.Add(ddl);
            ddl.SetSelectItem(en.TagVal2); 
            this.UCS3.AddHR();

            //  Specific leadership positions 
            rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_2";
            rb.Text = " Specific leadership positions ";

            if (en.SortVal2 == "2")
                rb.Checked = true;

            this.UCS3.Add(rb);
            ddl = new BP.Web.Controls.DDL();
            ddl.BindSQL("SELECT No, Name FROM Port_Duty ", "No", "Name", "20");
            ddl.SetSelectItem(en.TagVal2);
            ddl.ID = "DDL_Duty";

            this.UCS3.Add(ddl);
            this.UCS3.AddHR();

            //  Specific job 
            rb = new BP.Web.Controls.RadioBtn();
            rb.GroupName = "s";
            rb.ID = "RB_3";
            rb.Text = " Specific job ";
            this.UCS3.Add(rb);
            if (en.SortVal2 == "3")
                rb.Checked = true;
            ddl = new BP.Web.Controls.DDL();
            ddl.BindSQL("SELECT No, Name FROM Port_Station ", "No", "Name", "20");
            ddl.SetSelectItem(en.TagVal2);
            ddl.ID = "DDL_Station";
            this.UCS3.Add(ddl);
            this.UCS3.AddFieldSetEnd();
            #endregion  Binding the second half of .

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = " Save ";
            btn.Click += new EventHandler(btn_Click);
            this.UCS3.Add(btn);
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
            fwr.SortText0 = " Superiors ";

            //  Two .
            fwr.SortVal1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            fwr.SortText1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItem.Text;

            //  Three .
            if (this.UCS3.GetRadioBtnByID("RB_0").Checked)
            {
                fwr.SortVal2 = "0";
                fwr.SortText2 = this.UCS3.GetRadioBtnByID("RB_0").Text;
            }

            if (this.UCS3.GetRadioBtnByID("RB_1").Checked)
            {
                fwr.SortVal2 = "1";
                fwr.SortText2 = this.UCS3.GetRadioBtnByID("RB_1").Text;
            }

            if (this.UCS3.GetRadioBtnByID("RB_2").Checked)
            {
                fwr.SortVal2 = "2";
                fwr.SortText2 = this.UCS3.GetRadioBtnByID("RB_2").Text;
            }

            if (this.UCS3.GetRadioBtnByID("RB_3").Checked)
            {
                fwr.SortVal2 = "3";
                fwr.SortText2 = this.UCS3.GetRadioBtnByID("RB_3").Text;
            }

            if (fwr.SortVal2 == "")
            {
                fwr.SortVal2 = "0";
                fwr.SortText2 = this.UCS3.GetRadioBtnByID("RB_0").Text;
            }
            #endregion  Processing Description .

            #region  Processing data 
            try
            {
                //  Get 1 Latitude parameters .
                fwr.TagVal1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;
                fwr.TagText1 = this.UCS2.GetDDLByID("DDL_V1").SelectedItem.Text;
            }
            catch
            {
                fwr.TagVal1 = ""; // this.UCS2.GetDDLByID("DDL_V1").SelectedItemStringVal;
                fwr.TagText1 = ""; // this.UCS2.GetDDLByID("DDL_V1").SelectedItem.Text;
            }

            //  Treatment 2 Latitude parameters .
            switch (fwr.HisFindLeaderModel)
            {
                case FindLeaderModel.SpecDutyLevelLeader:
                    fwr.TagVal2 = this.UCS3.GetDDLByID("DDL_DutyLevel").SelectedItemStringVal;
                    fwr.TagText2 = this.UCS3.GetDDLByID("DDL_DutyLevel").SelectedItem.Text;
                    break;
                case FindLeaderModel.DutyLeader:
                    fwr.TagVal2 = this.UCS3.GetDDLByID("DDL_Duty").SelectedItemStringVal;
                    fwr.TagText2 = this.UCS3.GetDDLByID("DDL_Duty").SelectedItem.Text;
                    break;
                case FindLeaderModel.SpecStation:
                    fwr.TagVal2 = this.UCS3.GetDDLByID("DDL_Station").SelectedItemStringVal;
                    fwr.TagText2 = this.UCS3.GetDDLByID("DDL_Station").SelectedItem.Text;
                    break;
                default:
                    fwr.TagVal2 = "";
                    fwr.TagText2 = "";
                    break;
            }
            #endregion  Processing data 

            fwr.FK_Node = int.Parse(this.FK_Node);
            fwr.Save();

            // Set bpm Status .
            Node nd = new Node(this.FK_Node);
            nd.HisDeliveryWay = DeliveryWay.ByCCFlowBPM;
            nd.DirectUpdate();

            this.Response.Redirect("List.aspx?"+this.FK_Flow+"&FK_Node="+this.FK_Node,true);
            //this.WinCloseWithMsg(" Saved successfully ...");
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s1 = this.UCS1.GetDDLByID("DDL_S1").SelectedItemStringVal;
            this.Response.Redirect("Leader.aspx?S1=" + s1 + "&RefOID=" + this.RefOID+"&FK_Flow="+this.FK_Flow+"&FK_Node="+this.FK_Node, true);
        }
    }
}