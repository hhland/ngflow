using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.En;
using BP.Web.Controls;
using BP.DA;
using BP.Web;
using BP;
using BP.Sys;

namespace CCFlow.WF.MapDef
{
    public partial class SlnDo : WebPage
    {
        #region  Property .
        public string FK_MapData
        {
            get
            {
                return this.Request.QueryString["FK_MapData"];
            }
        }
        public string FK_Node
        {
            get
            {
                return this.Request.QueryString["FK_Node"];
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public string KeyOfEn
        {
            get
            {
                return this.Request.QueryString["KeyOfEn"];
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "DelSln": // Delete sln.
                    FrmField sln = new FrmField();
                    sln.Delete(FrmFieldAttr.FK_MapData, this.FK_MapData,
                        FrmFieldAttr.KeyOfEn, this.KeyOfEn,
                        FrmFieldAttr.FK_Flow, this.FK_Flow,
                        FrmFieldAttr.FK_Node, this.FK_Node);
                    this.WinClose();
                    return;
                case "EditSln": // Editor sln.
                    this.EditSln();
                    return;
                case "Copy": // Editor sln.
                    this.Copy();
                    return;
                case "CopyIt": // Editor sln.
                    FrmFields fss = new FrmFields();
                    fss.Delete(FrmFieldAttr.FK_MapData, this.FK_MapData,
                        FrmFieldAttr.FK_Flow, this.FK_Flow,
                        FrmFieldAttr.FK_Node, this.FK_Node);

                    fss = new FrmFields(this.FK_MapData,int.Parse(this.Request.QueryString["FromSln"]));
                    //fss.Retrieve(FrmFieldAttr.FK_MapData, this.FK_MapData,
                    //    FrmFieldAttr.FK_Node, this.Request.QueryString["FromSln"]);

                    foreach (FrmField sl in fss)
                    {
                        sl.FK_Node = int.Parse(this.FK_Node);
                        sl.FK_Flow = this.FK_Flow;
                        sl.MyPK = this.FK_MapData + "_" +this.FK_Flow+"_"+ this.FK_Node + "_" + sl.KeyOfEn;
                        sl.Insert();
                    }
                    this.WinClose();
                    return;
                default:
                    break;
            }
        }
        /// <summary>
        ///  Replication .
        /// </summary>
        public void Copy()
        {
            string sql = "SELECT NodeID, Name, Step FROM WF_Node WHERE NodeID IN (SELECT FK_Node FROM Sys_FrmSln WHERE FK_MapData='" + this.FK_MapData + "' )";
            DataTable dtNodes = BP.DA.DBAccess.RunSQLReturnTable(sql);

            this.Pub1.AddFieldSet(" Please select copy Node .");

            this.Pub1.AddUL();
            foreach (DataRow dr in dtNodes.Rows)
            {
                string name = " Step :" + dr[2] + ", Node ID:" + dr[0] + ":" + dr[1].ToString();
                string no = dr[0].ToString();

                if (this.FK_Node == no)
                    continue;
                else
                    this.Pub1.AddLi("<a href='SlnDo.aspx?FK_MapData=" + this.FK_MapData + "&FromSln=" + no +"&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&DoType=CopyIt' >" + name + "</a>");
            }
            this.Pub1.AddULEnd();
             
            this.Pub1.AddFieldSetEnd();

        }
        public void EditSln()
        {
            BP.Sys.FrmField sln = new BP.Sys.FrmField();
            int num = sln.Retrieve(FrmFieldAttr.FK_MapData, this.FK_MapData,
                         FrmFieldAttr.KeyOfEn, this.KeyOfEn,
                         FrmFieldAttr.FK_Node, this.FK_Node);

            BP.Sys.MapAttr attr = new MapAttr();
            attr.Retrieve(MapAttrAttr.FK_MapData, this.FK_MapData,
                      MapAttrAttr.KeyOfEn, this.KeyOfEn);

            if (num == 0)
            {
                sln.UIIsEnable = attr.UIIsEnable;
                sln.UIVisible = attr.UIVisible;
                sln.IsSigan =attr.IsSigan;
                sln.DefVal =attr.DefValReal;
            }

            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Project ");
            this.Pub1.AddTDTitle(" Information ");
            this.Pub1.AddTDTitle(" Remark ");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Field ");
            this.Pub1.AddTD(attr.KeyOfEn);
            this.Pub1.AddTD("");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Chinese name ");
            this.Pub1.AddTD(attr.Name);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD();
            CheckBox cb = new CheckBox();
            cb.ID = "CB_Visable";
            cb.Text = " Is visible ?";
            cb.Checked = sln.UIVisible;

            this.Pub1.AddTD(cb);
            this.Pub1.AddTD(" Is visible in the program ?");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD();
            cb = new CheckBox();
            cb.ID = "CB_Readonly";
            cb.Text = " Is read-only ?";
            cb.Checked = sln.UIIsEnable;
            this.Pub1.AddTD(cb);
            this.Pub1.AddTD(" In this scenario, if the read-only ?");
            this.Pub1.AddTREnd();

            if ( attr.MyDataType== DataType.AppString)
            {
                /* Read-only , And is String. */
                this.Pub1.AddTR();
                this.Pub1.AddTD();
                cb = new CheckBox();
                cb.ID = "CB_IsSigan";
                cb.Text = " Whether it is a digital signature ?";
                cb.Checked = sln.IsSigan;
                this.Pub1.AddTD(cb);
                this.Pub1.AddTD(" In the case of , And the need to show the signature of the current staff in the current program :<br> Please enter a default value in @WebUser.No");
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTR();
            this.Pub1.AddTD(" Defaults ");
            TextBox tb=new TextBox();
            tb.ID = "TB_DefVal";
            tb.Text = sln.DefVal;
            this.Pub1.AddTD(tb);
            this.Pub1.AddTD(" Stand by ccflow Global variables .");
            this.Pub1.AddTREnd();
            this.Pub1.AddTableEnd();

            Button btn = new Button();
            btn.ID = "Btn_Save";
            btn.Text = "Save";
            btn.Click += new EventHandler(btn_Click);
            this.Pub1.Add(btn);

            this.Pub1.AddFieldSet(" Process the form of a digital signature setting method ");
            this.Pub1.AddBR(" Application Overview :");
            this.Pub1.AddBR("1,  There will be more of a process to form a digital signature . ");
            this.Pub1.AddBR("2,  These digital signatures are sometimes read before signing , Sometimes current digital signature .");
            this.Pub1.AddBR("3,  If the current program needs to read the previous digital signature , Then clear the default value information , Otherwise it is set @WebUser.No  Get current digital signature operator .");
            this.Pub1.AddFieldSetEnd();
        }

        void btn_Click(object sender, EventArgs e)
        {
            BP.Sys.FrmField sln = new BP.Sys.FrmField();
            sln.Retrieve(BP.Sys.FrmFieldAttr.FK_MapData, this.FK_MapData,
                           BP.Sys.FrmFieldAttr.KeyOfEn, this.KeyOfEn,
                           BP.Sys.FrmFieldAttr.FK_Node, this.FK_Node);

            sln.UIIsEnable = this.Pub1.GetCBByID("CB_Readonly").Checked;
            sln.UIVisible = this.Pub1.GetCBByID("CB_Visable").Checked;

            if (this.Pub1.IsExit("CB_IsSigan"))
                sln.IsSigan = this.Pub1.GetCBByID("CB_IsSigan").Checked;

            sln.DefVal = this.Pub1.GetTextBoxByID("TB_DefVal").Text;

            sln.FK_MapData = this.FK_MapData;
            sln.KeyOfEn = this.KeyOfEn;
            sln.FK_Node = int.Parse(this.FK_Node);
            sln.FK_Flow = this.FK_Node;

            sln.MyPK = this.FK_MapData +"_"+this.FK_Flow+ "_" + this.FK_Node + "_" + this.KeyOfEn;
            sln.CheckPhysicsTable();
            sln.Save();
            this.WinClose();
        }
    }
}