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
using BP.WF;
using BP.Web;

public partial class WF_MapDef_WFRptDtl : BP.Web.WebPage
{
    public string FK_Flow
    {
        get
        {
            string flowNo = this.MyPK.Replace("ND", "");

            flowNo = flowNo.Replace("Rpt", "");

            flowNo = flowNo.Replace("Dtl1", "");
            flowNo = flowNo.Replace("Dtl2", "");
            flowNo = flowNo.Replace("Dtl3", "");
            flowNo = flowNo.Replace("Dtl4", "");
            flowNo = flowNo.Replace("Dtl", "");

            flowNo = flowNo.PadLeft(3, '0');
            return flowNo;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        switch (this.DoType)
        {
            case "DeDtl":
                BindDtlList();
                break;
            default:
                InsertDtl();
                break;
        }
    }
    public void BindDtlList()
    {
       string sql = "SELECT No,Name FROM Sys_MapData WHERE No LIKE '" + this.MyPK + "Dtl%'";
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

        this.Pub1.AddUL();
        foreach (DataRow dr in dt.Rows)
        {
            this.Pub1.AddLi("WFRpt.aspx?MyPK=" + dr[0].ToString(), dr[1].ToString());
        }
        this.Pub1.AddULEnd();
    }
    public void InsertDtl()
    {
        string sql = "SELECT * FROM Sys_MapDtl WHERE FK_MapData LIKE '" + this.MyPK.Replace("Rpt", "") + "%'";
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        if (dt.Rows.Count == 0)
        {
            this.WinCloseWithMsg(" This process is not so you can not insert from the table .");
            return;
        }

        Nodes nds = new Nodes(BP.WF.Glo.GenerFlowNo(this.MyPK));
        this.Pub1.AddTable();
        this.Pub1.AddCaptionLeft(" Please select from the table to insert ");
        foreach (BP.WF.Node nd in nds)
        {
            if (nd.IsEndNode == false)
                continue;

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle(" Node Step :" + nd.Step + "  Node Name :" + nd.Name);
            this.Pub1.AddTREnd();

            MapDtls dtls = new MapDtls("ND" + nd.NodeID);

            this.Pub1.AddTR();
            this.Pub1.AddTDBegin();

            foreach (MapDtl dtl in dtls)
            {
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + dtl.No;
                cb.Text = dtl.Name;
                this.Pub1.Add(cb);

                #region  Field Grouping 
                DDL ddlGroup = new DDL();
                ddlGroup.ID = "DDL_" + dtl.No;
                GroupFields gfs = new GroupFields(dtl.FK_MapData);
                ddlGroup.Bind(gfs, GroupFieldAttr.OID, GroupFieldAttr.Lab);
                this.Pub1.Add(" Insert position ");
                this.Pub1.Add(ddlGroup);

                //this.Pub1.Add(" Query conditions :");
                //MapAttrs attrs = new MapAttrs(dtl.No);
                //foreach (MapAttr attr in attrs)
                //{
                //    if (attr.UIContralType != UIContralType.DDL)
                //        continue;
                //    cb = new CheckBox();
                //    cb.ID = "CB_" + dtl.No + "_" + attr.KeyOfEn;
                //    cb.Text = attr.Name;
                //    this.Pub1.Add(cb);
                //}
                #endregion  Field Grouping 
            }

            this.Pub1.AddTDEnd();
            this.Pub1.AddTREnd();
        }

        this.Pub1.AddTRSum();
        Button btn = new Button();
        btn.ID = "Btn_Save";
        btn.Text = " Inserted into the report ";
        btn.CssClass = "Btn";
        btn.Click += new EventHandler(btn_Click);
        this.Pub1.AddTD(btn);
        this.Pub1.AddTREnd();
        this.Pub1.AddTableEnd();
    }

    void btn_Click(object sender, EventArgs e)
    {
        MapDtls dtl2s = new MapDtls();
        dtl2s.Delete(MapDtlAttr.FK_MapData, this.MyPK);
        Nodes nds = new Nodes(BP.WF.Glo.GenerFlowNo(this.MyPK));
        foreach (BP.WF.Node nd in nds)
        {
            if (nd.IsEndNode == false)
                continue;

            MapDtls dtls = new MapDtls("ND" + nd.NodeID);
            int i = 0;

            foreach (MapDtl dtl in dtls)
            {
                if (this.Pub1.GetCBByID("CB_" + dtl.No).Checked == false)
                    continue;

                i++;
                //  Generated from the table so that it could show them in a single data .
                MapDtl dtlNew = new MapDtl();
                dtlNew.Copy(dtl);
                dtlNew.No = this.MyPK + i;
                dtlNew.FK_MapData = this.MyPK;
                dtlNew.GroupID = this.Pub1.GetDDLByID("DDL_" + dtl.No).SelectedItemIntVal;
                dtlNew.Insert();

                //  Delete the original data .
                MapAttrs attrsDtl = new MapAttrs();
                attrsDtl.Delete(MapAttrAttr.FK_MapData, dtlNew.No);

                //  Copied to the new data table .
                MapAttrs attrs = new MapAttrs(dtl.No);
                foreach (MapAttr attr in attrs)
                {
                    MapAttr attrN = new MapAttr();
                    attrN.Copy(attr);
                    attrN.FK_MapData = dtlNew.No;
                    attrN.Insert();
                }
                Cash.Map_Cash.Remove(dtlNew.No);


                #region   Copied into the   Main table . So that it could check .
                //  Processing the main table .
                MapData md = new MapData();
                md.Copy(dtlNew);
                md.No = "ND" + int.Parse(this.FK_Flow) + "RptDtl" + i.ToString();
                md.Save();

                //  Delete the original property .
                attrs.Delete(MapAttrAttr.FK_MapData, md.No);

                //  Delete grouping .
                GroupField gfBase = new GroupField();
                gfBase.Delete(GroupFieldAttr.EnName, md.No);

                //  Increase the basic information packet .
                gfBase.EnName = md.No;
                gfBase.Lab = md.Name;
                gfBase.Idx = 99;
                gfBase.Insert();


                // Basic information generated property .
                foreach (MapAttr attr in attrs)
                {
                    MapAttr attrN = new MapAttr();
                    attrN.Copy(attr);
                    attrN.FK_MapData = md.No;
                    attrN.GroupID = gfBase.OID;
                    attrN.Insert();
                }

                MapAttrs attrNs = new MapAttrs(md.No);

                //  Processing of individual fields .
                foreach (MapAttr attr in attrNs)
                {
                    switch (attr.KeyOfEn)
                    {
                        case StartWorkAttr.FK_Dept:
                            continue;
                            //if (attr.UIContralType != UIContralType.DDL)
                            //{
                            //attr.UIBindKey = "BP.Port.Depts";
                            //attr.UIContralType = UIContralType.DDL;
                            //attr.LGType = FieldTypeS.FK;
                            //attr.UIVisible = true;
                            //// if (gfs.Contains(attr.GroupID) == false)
                            //attr.GroupID = gfBase.OID;// gfs[0].GetValIntByKey("OID");
                            //attr.Update();
                            //// }
                            break;
                        case "FK_NY":
                            //attr.Delete();
                            ////if (attr.UIContralType != UIContralType.DDL)
                            ////{
                            //attr.UIBindKey = "BP.Pub.NYs";
                            //attr.UIContralType = UIContralType.DDL;
                            //attr.LGType = FieldTypeS.FK;
                            //attr.UIVisible = true;
                            ////   if (gfs.Contains(attr.GroupID) == false)
                            //attr.GroupID = gfBase.OID; // gfs[0].GetValIntByKey("OID");
                            //attr.Update();
                            break;
                        case "Rec":
                            attr.UIBindKey = "BP.Port.Emps";
                            attr.UIContralType = UIContralType.DDL;
                            attr.LGType = FieldTypeS.FK;
                            attr.UIVisible = true;
                            attr.Name = " The final disposition of people ";
                            attr.GroupID = gfBase.OID;
                            attr.Update();
                            break;
                        default:
                            break;
                    }
                }

                //  Basic information generation process attributes .
                GroupField gfFlow = new GroupField();
                gfFlow.EnName = md.No;
                gfFlow.Idx = 0;
                gfFlow.Lab = " Process Information ";
                gfFlow.Insert();


                MapAttr attrFlow = new BP.Sys.MapAttr();

                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "Title";
                attrFlow.Name = " Title "; 
                attrFlow.MyDataType = BP.DA.DataType.AppString;
                attrFlow.UIContralType = UIContralType.TB;
                attrFlow.LGType = FieldTypeS.Normal;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = true;
                attrFlow.UIIsLine = true;
                attrFlow.MinLen = 0;
                attrFlow.MaxLen = 1000;
                attrFlow.IDX = -100;
                attrFlow.GroupID = gfFlow.OID;
                attrFlow.Insert();


                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "FlowStarter";
                attrFlow.Name = " Sponsor "; //" Sponsor ";
                attrFlow.MyDataType = BP.DA.DataType.AppString;
                attrFlow.UIContralType = UIContralType.DDL;
                attrFlow.UIBindKey = "BP.Port.Emps";
                attrFlow.LGType = FieldTypeS.FK;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = false;
                attrFlow.UIIsLine = false;
                attrFlow.MaxLen = 20;
                attrFlow.MinLen = 0;
                attrFlow.Insert();

                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "FlowStarterDept";
                attrFlow.Name = " Sponsor department ";
                attrFlow.MyDataType = BP.DA.DataType.AppString;
                attrFlow.UIContralType = UIContralType.DDL;
                attrFlow.UIBindKey = "BP.Port.Depts";
                attrFlow.LGType = FieldTypeS.FK;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = false;
                attrFlow.MaxLen = 20;
                attrFlow.MinLen = 0;
                attrFlow.Insert();

               


                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "FlowEmps";
                attrFlow.Name = " Participants "; //  
                attrFlow.MyDataType = BP.DA.DataType.AppString;
                attrFlow.UIContralType = UIContralType.TB;
                attrFlow.LGType = FieldTypeS.Normal;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = true;
                attrFlow.UIIsLine = false;
                attrFlow.MinLen = 0;
                attrFlow.MaxLen = 1000;
                attrFlow.IDX = -100;
                attrFlow.GroupID = gfFlow.OID;
                attrFlow.Insert();


                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "FlowStartRDT";
                attrFlow.Name = " Start Time "; //  
                attrFlow.MyDataType = BP.DA.DataType.AppDateTime;
                attrFlow.UIContralType = UIContralType.TB;
                attrFlow.LGType = FieldTypeS.Normal;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = true;
                attrFlow.UIIsLine = false;
                attrFlow.MinLen = 0;
                attrFlow.MaxLen = 1000;
                attrFlow.IDX = -100;
                attrFlow.GroupID = gfFlow.OID;
                attrFlow.Insert();

                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "FlowNY";
                attrFlow.Name = " Years of membership "; 
                attrFlow.MyDataType = BP.DA.DataType.AppString;
                attrFlow.UIContralType = UIContralType.DDL;
                attrFlow.UIBindKey = "BP.Pub.NYs";
                attrFlow.LGType = FieldTypeS.FK;
                attrFlow.UIVisible = true;
                attrFlow.UIIsEnable = false;
                attrFlow.MaxLen = 20;
                attrFlow.MinLen = 0;
                attrFlow.Insert();


                attrFlow = new BP.Sys.MapAttr();
                attrFlow.FK_MapData = md.No;
                attrFlow.HisEditType = EditType.UnDel;
                attrFlow.KeyOfEn = "MyNum";
                attrFlow.Name = " records"; //  
                attrFlow.MyDataType = BP.DA.DataType.AppInt;
                attrFlow.DefVal = "1";
                attrFlow.UIContralType = UIContralType.TB;
                attrFlow.LGType = FieldTypeS.Normal;
                attrFlow.UIVisible = false;
                attrFlow.UIIsEnable = false;
                attrFlow.UIIsLine = false;
                attrFlow.IDX = -101;
                attrFlow.GroupID = gfFlow.OID;
                if (attrFlow.IsExits == false)
                    attrFlow.Insert();

                //  Clear Cache map.
                Cash.Map_Cash.Remove(md.No);
                // Check the correctness of the main table .
                GEEntity ge = new GEEntity(md.No);
                ge.CheckPhysicsTable();
                #endregion   Copied into the   Main table . So that it could check .
            }
        }
        this.WinClose();
    }
}
