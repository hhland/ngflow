using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.Sys;
using BP;
using BP.DA;
using BP.Web;
using BP.En;
using BP.Web.Controls;

namespace CCFlow.WF.MapDef.Rpt
{
    public partial class SearchCond : BP.Web.PageBase
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];

            }
        }
        public string RptNo
        {
            get
            {
                return this.Request.QueryString["RptNo"];

            }
        }
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];

            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            MapData md = new MapData();
            md.No = this.RptNo;
            md.RetrieveFromDBSources();

            MapAttrs attrs = new MapAttrs(this.RptNo);

            #region  The query definition 

            this.Pub2.Add("<div style='width:100%'>");
            this.Pub2.Add("<div class='easyui-panel' title=' Whether to increase the keyword query ' data-options=\"iconCls:'icon-tip',fit:true\" style='height:auto;padding:10px'>");
            this.Pub2.Add(" Keyword query is to accept user input a keyword , Use the display column in the entire report like Inquiry ( Foreign key , Enumerate , Except for numeric types )");
            this.Pub2.AddBR();

            CheckBox mycb = new CheckBox();
            mycb.ID = "CB_IsSearchKey";
            mycb.Text = " Whether to increase the keyword query ";
            mycb.Checked = md.RptIsSearchKey;
            this.Pub2.Add(mycb);
            this.Pub2.AddDivEnd();
            this.Pub2.AddDivEnd();
            this.Pub2.AddBR();

            this.Pub2.Add("<div style='width:100%'>");
            this.Pub2.Add("<div class='easyui-panel' title=' Foreign keys with enumerations ' data-options=\"iconCls:'icon-tip',fit:true\" style='height:auto;padding:10px'>");
            this.Pub2.Add(" Foreign key , Enumeration types of data can be added to the query condition , Please select search criteria to be added :");
            this.Pub2.AddBR();

            foreach (MapAttr mattr in attrs)
            {
                if (mattr.UIContralType != UIContralType.DDL)
                    continue;

                CheckBox cb = new CheckBox();
                cb.ID = "CB_F_" + mattr.KeyOfEn;
                if (md.RptSearchKeys.Contains("*" + mattr.KeyOfEn))
                    cb.Checked = true;

                cb.Text = mattr.Name + "(" + mattr.KeyOfEn + ")";
                this.Pub2.Add(cb);
                this.Pub2.AddBR();
            }

            this.Pub2.AddDivEnd();
            this.Pub2.AddDivEnd();
            this.Pub2.AddBR();

            bool isHave = false;

            foreach (MapAttr mattr in attrs)
            {
                if (mattr.MyDataType == DataType.AppDate || mattr.MyDataType == DataType.AppDateTime)
                {
                    isHave = true;
                    break;
                }
            }

            if (isHave)
            {
                this.Pub2.Add("<div style='width:100%'>");
                this.Pub2.Add("<div class='easyui-panel' title=' Period ' data-options=\"iconCls:'icon-tip',fit:true\" style='height:auto;padding:10px'>");
                this.Pub2.Add(" Query the data in accordance with the time period , Such as : Process initiated by time , Query in the specified time period .");
                this.Pub2.AddBR();

                this.Pub2.Add(" Select mode :");
                BP.Web.Controls.DDL ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_DTSearchWay";
                ddl.BindSysEnum("DTSearchWay");
                ddl.SetSelectItem((int)md.RptDTSearchWay);
                this.Pub2.Add(ddl);
                this.Pub2.AddSpace(3);

                this.Pub2.Add(" Field :");
                ddl = new BP.Web.Controls.DDL();
                ddl.ID = "DDL_DTSearchKey";

                foreach (MapAttr mattr in attrs)
                {
                    if (mattr.MyDataType == DataType.AppDate || mattr.MyDataType == DataType.AppDateTime)
                    {
                        if (mattr.UIVisible == false)
                            continue;
                        ddl.Items.Add(new ListItem(mattr.KeyOfEn + "  " + mattr.Name, mattr.KeyOfEn));
                    }
                }

                ddl.SetSelectItem(md.RptDTSearchKey);
                this.Pub2.Add(ddl);
                this.Pub2.AddDivEnd();
                this.Pub2.AddDivEnd();
            }
            #endregion
        }

        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            Save();

            this.Response.Redirect("S5_SearchCond.aspx?FK_MapData=" + this.FK_MapData + "&RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow, true);
        }

        protected void Btn_Cancel_Click(object sender, EventArgs e)
        {
            this.WinClose();
        }

        protected void Btn_SaveAndNext1_Click(object sender, EventArgs e)
        {
            Save();

            this.Response.Redirect("S6_Power.aspx?FK_MapData=" + this.FK_MapData + "&RptNo=" + this.RptNo + "&FK_Flow=" + this.FK_Flow, true);
        }

        private void Save()
        {
            MapData md = new MapData();
            md.No = this.RptNo;
            md.RetrieveFromDBSources();

            MapAttrs mattrs = new MapAttrs(this.RptNo);
            string keys = "";
            foreach (MapAttr mattr in mattrs)
            {
                if (mattr.UIContralType != UIContralType.DDL)
                    continue;
                CheckBox cb = this.Pub2.GetCBByID("CB_F_" + mattr.KeyOfEn);
                if (cb.Checked)
                    keys += "*" + mattr.KeyOfEn;
            }

            md.RptSearchKeys = keys + "*";
            md.RptIsSearchKey = this.Pub2.GetCBByID("CB_IsSearchKey").Checked;

            if (this.Pub2.IsExit("DDL_DTSearchWay"))
            {
                BP.Web.Controls.DDL ddl = this.Pub2.GetDDLByID("DDL_DTSearchWay");
                md.RptDTSearchWay = (DTSearchWay)ddl.SelectedItemIntVal;

                ddl = this.Pub2.GetDDLByID("DDL_DTSearchKey");
                md.RptDTSearchKey = ddl.SelectedItemStringVal;
            }
            md.Update();

            Cash.Map_Cash.Remove(this.RptNo);
        }
    }
}