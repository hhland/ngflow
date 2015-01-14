using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Port;
using BP.En;
using BP.WF;
using BP.Sys;

namespace BP.WF.Rpt
{
    /// <summary>
    ///  Report Viewer control permissions 
    /// </summary>
    public enum RightViewWay
    {
        /// <summary>
        ///  Anyone can view 
        /// </summary>
        AnyOne,
        /// <summary>
        ///  According to the structure of the organization permission 
        /// </summary>
        ByPort,
        /// <summary>
        ///  According to SQL Control 
        /// </summary>
        BySQL
    }
    /// <summary>
    ///  Department data access control mode 
    /// </summary>
    public enum RightDeptWay
    {
        /// <summary>
        ///  Data from all departments .
        /// </summary>
        All,
        /// <summary>
        ///  The data in this sector .
        /// </summary>
        SelfDept,
        /// <summary>
        ///  The data in this sector and sub-sector .
        /// </summary>
        SelfDeptAndSubDepts,
        /// <summary>
        ///  Data specified department .
        /// </summary>
        SpecDepts
    }

    /// <summary>
    ///  Report Design 
    /// </summary>
    public class MapRptAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Physical table query 
        /// </summary>
        public const string PTable = "PTable";
        /// <summary>
        ///  Remark 
        /// </summary>
        public const string Note = "Note";
        /// <summary>
        ///  Report inheritance FK_MapData
        /// </summary>
        public const string ParentMapData = "ParentMapData";
        /// <summary>
        ///  Process ID 
        /// </summary>
        public const string FK_Flow = "FK_Flow";

        #region  Access control  2014-12-18
        /// <summary>
        ///  Report Viewer control permissions 
        /// </summary>
        public const string RightViewWay = "RightViewWay";
        /// <summary>
        ///  Data Storage 
        /// </summary>
        public const string RightViewTag = "RightViewTag";

        /// <summary>
        ///  Department data access control mode 
        /// </summary>
        public const string RightDeptWay = "RightDeptWay";
        /// <summary>
        ///  Data Storage 
        /// </summary>
        public const string RightDeptTag = "RightDeptTag";
        #endregion  Access control 
    }
    /// <summary>
    ///  Report Design 
    /// </summary>
    public class MapRpt : EntityNoName
    {
        #region  Reports access control mode 
        /// <summary>
        ///  Report Viewer Access Control .
        /// </summary>
        public RightViewWay RightViewWay
        {
            get
            {
                return (RightViewWay)this.GetValIntByKey(MapRptAttr.RightViewWay);
            }
            set
            {
                this.SetValByKey(MapRptAttr.RightViewWay, (int)value);
            }
        }
        /// <summary>
        ///  Report Viewer Access Control - Data 
        /// </summary>
        public string RightViewTag
        {
            get
            {
                return this.GetValStringByKey(MapRptAttr.RightViewTag);
            }
            set
            {
                this.SetValByKey(MapRptAttr.RightViewTag, value);
            }
        }
        /// <summary>
        ///  Reports department access control .
        /// </summary>
        public RightDeptWay RightDeptWay
        {
            get
            {
                return (RightDeptWay)this.GetValIntByKey(MapRptAttr.RightDeptWay);
            }
            set
            {
                this.SetValByKey(MapRptAttr.RightDeptWay, (int)value);
            }
        }
        /// <summary>
        ///  Reports department access control - Data 
        /// </summary>
        public string RightDeptTag
        {
            get
            {
                return this.GetValStringByKey(MapRptAttr.RightDeptTag);
            }
            set
            {
                this.SetValByKey(MapRptAttr.RightDeptTag, value);
            }
        }
        #endregion  Reports access control mode 

        #region  Foreign key attribute 
        /// <summary>
        ///  Frame 
        /// </summary>
        public MapFrames MapFrames
        {
            get
            {
                MapFrames obj = this.GetRefObject("MapFrames") as MapFrames;
                if (obj == null)
                {
                    obj = new MapFrames(this.No);
                    this.SetRefObject("MapFrames", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Group field 
        /// </summary>
        public GroupFields GroupFields
        {
            get
            {
                GroupFields obj = this.GetRefObject("GroupFields") as GroupFields;
                if (obj == null)
                {
                    obj = new GroupFields(this.No);
                    this.SetRefObject("GroupFields", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Logical extension 
        /// </summary>
        public MapExts MapExts
        {
            get
            {
                MapExts obj = this.GetRefObject("MapExts") as MapExts;
                if (obj == null)
                {
                    obj = new MapExts(this.No);
                    this.SetRefObject("MapExts", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Event 
        /// </summary>
        public FrmEvents FrmEvents
        {
            get
            {
                FrmEvents obj = this.GetRefObject("FrmEvents") as FrmEvents;
                if (obj == null)
                {
                    obj = new FrmEvents(this.No);
                    this.SetRefObject("FrmEvents", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Many 
        /// </summary>
        public MapM2Ms MapM2Ms
        {
            get
            {
                MapM2Ms obj = this.GetRefObject("MapM2Ms") as MapM2Ms;
                if (obj == null)
                {
                    obj = new MapM2Ms(this.No);
                    this.SetRefObject("MapM2Ms", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  From Table 
        /// </summary>
        public MapDtls MapDtls
        {
            get
            {
                MapDtls obj = this.GetRefObject("MapDtls") as MapDtls;
                if (obj == null)
                {
                    obj = new MapDtls(this.No);
                    this.SetRefObject("MapDtls", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Hyperlinks 
        /// </summary>
        public FrmLinks FrmLinks
        {
            get
            {
                FrmLinks obj = this.GetRefObject("FrmLinks") as FrmLinks;
                if (obj == null)
                {
                    obj = new FrmLinks(this.No);
                    this.SetRefObject("FrmLinks", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Push button 
        /// </summary>
        public FrmBtns FrmBtns
        {
            get
            {
                FrmBtns obj = this.GetRefObject("FrmLinks") as FrmBtns;
                if (obj == null)
                {
                    obj = new FrmBtns(this.No);
                    this.SetRefObject("FrmBtns", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Element 
        /// </summary>
        public FrmEles FrmEles
        {
            get
            {
                FrmEles obj = this.GetRefObject("FrmEles") as FrmEles;
                if (obj == null)
                {
                    obj = new FrmEles(this.No);
                    this.SetRefObject("FrmEles", obj);
                }
                return obj;
            }
        }
        /// <summary>
        /// Ïß
        /// </summary>
        public FrmLines FrmLines
        {
            get
            {
                FrmLines obj = this.GetRefObject("FrmLines") as FrmLines;
                if (obj == null)
                {
                    obj = new FrmLines(this.No);
                    this.SetRefObject("FrmLines", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Label 
        /// </summary>
        public FrmLabs FrmLabs
        {
            get
            {
                FrmLabs obj = this.GetRefObject("FrmLabs") as FrmLabs;
                if (obj == null)
                {
                    obj = new FrmLabs(this.No);
                    this.SetRefObject("FrmLabs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Picture 
        /// </summary>
        public FrmImgs FrmImgs
        {
            get
            {
                FrmImgs obj = this.GetRefObject("FrmLabs") as FrmImgs;
                if (obj == null)
                {
                    obj = new FrmImgs(this.No);
                    this.SetRefObject("FrmLabs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Accessory 
        /// </summary>
        public FrmAttachments FrmAttachments
        {
            get
            {
                FrmAttachments obj = this.GetRefObject("FrmAttachments") as FrmAttachments;
                if (obj == null)
                {
                    obj = new FrmAttachments(this.No);
                    this.SetRefObject("FrmAttachments", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Image Attachment 
        /// </summary>
        public FrmImgAths FrmImgAths
        {
            get
            {
                FrmImgAths obj = this.GetRefObject("FrmImgAths") as FrmImgAths;
                if (obj == null)
                {
                    obj = new FrmImgAths(this.No);
                    this.SetRefObject("FrmImgAths", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Radio buttons 
        /// </summary>
        public FrmRBs FrmRBs
        {
            get
            {
                FrmRBs obj = this.GetRefObject("FrmRBs") as FrmRBs;
                if (obj == null)
                {
                    obj = new FrmRBs(this.No);
                    this.SetRefObject("FrmRBs", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  Property 
        /// </summary>
        public MapAttrs MapAttrs
        {
            get
            {
                MapAttrs obj = this.GetRefObject("MapAttrs") as MapAttrs;
                if (obj == null)
                {
                    obj = new MapAttrs(this.No);
                    this.SetRefObject("MapAttrs", obj);
                }
                return obj;
            }
        }
        #endregion

        #region  Property 
        public string FK_Flow
        {
            get
            {
                string s = this.GetValStrByKey(MapRptAttr.FK_Flow);
                if (s == "" || s == null)
                {
                    s = this.ParentMapData;
                    if (string.IsNullOrEmpty(s))
                        throw new Exception("@ Error Reporting " + this.No + "," + this.Name + " ,  Field ParentMapData:" + this.ParentMapData + "  Should not empty .");
                    s = s.Replace("ND", "");
                    s = s.Replace("Rpt", "");
                    s = s.PadLeft(3, '0');
                    return s;
                }
                return s;
            }
            set
            {
                this.SetValByKey(MapRptAttr.FK_Flow, value);
            }
        }
        public string PTable
        {
            get
            {
                string s = this.GetValStrByKey(MapRptAttr.PTable);
                if (s == "" || s == null)
                    return this.No;
                return s;
            }
            set
            {
                this.SetValByKey(MapRptAttr.PTable, value);
            }
        }
        /// <summary>
        ///  Remark 
        /// </summary>
        public string Note
        {
            get
            {
                return this.GetValStrByKey(MapRptAttr.Note);
            }
            set
            {
                this.SetValByKey(MapRptAttr.Note, value);
            }
        }
        public string ParentMapData
        {
            get
            {
                return this.GetValStrByKey(MapRptAttr.ParentMapData);
            }
            set
            {
                this.SetValByKey(MapRptAttr.ParentMapData, value);
            }
        }
        
       
        public Entities _HisEns = null;
        public new Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                {
                    _HisEns = BP.En.ClassFactory.GetEns(this.No);
                }
                return _HisEns;
            }
        }
        public Entity HisEn
        {
            get
            {
                return this.HisEns.GetNewEntity;
            }
        }
        #endregion

        #region  Constructor 
        private GEEntity _HisEn = null;
        public GEEntity HisGEEn
        {
            get
            {
                if (this._HisEn == null)
                    _HisEn = new GEEntity(this.No);
                return _HisEn;
            }
        }
        /// <summary>
        ///  Entity 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public GEEntity GenerGEEntityByDataSet(DataSet ds)
        {
            // New  Its instances .
            GEEntity en = this.HisGEEn;

            //  Its table.
            DataTable dt = ds.Tables[this.No];

            // Loading data .
            en.Row.LoadDataTable(dt, dt.Rows[0]);

            // dtls.
            MapDtls dtls = this.MapDtls;
            foreach (MapDtl item in dtls)
            {
                DataTable dtDtls = ds.Tables[item.No];
                GEDtls dtlsEn = new GEDtls(item.No);
                foreach (DataRow dr in dtDtls.Rows)
                {
                    //  Produced it Entity data.
                    GEDtl dtl = (GEDtl)dtlsEn.GetNewEntity;
                    dtl.Row.LoadDataTable(dtDtls, dr);

                    // Join this collection .
                    dtlsEn.AddEntity(dtl);
                }

                // Added to his collection in .
                en.Dtls.Add(dtDtls);
            }
            return en;
        }
        /// <summary>
        ///  Report Design 
        /// </summary>
        public MapRpt()
        {
        }
        /// <summary>
        ///  Report Design 
        /// </summary>
        /// <param name="no"> Number Mapping </param>
        public MapRpt(string no, string flowNo)
        {
            this.No = no;
            this.Retrieve();

            //if (this.RetrieveFromDBSources() == 0)
            //{
            //    string fk_Mapdata = "ND" + int.Parse(flowNo) + "Rpt";
            //    MapData mapData = new MapData(fk_Mapdata);
            //    mapData.No = "ND" + int.Parse(flowNo) + "MyRpt";
            //    mapData.Name = " My Process ";
            //    mapData.Note = " The system automatically generates .";
            //    mapData.Save();

            //    // Reset .
            //    this.No = mapData.No;
            //    this.ParentMapData = fk_Mapdata;
            //    this.ResetIt();
            //    this.Update();
            //}
        }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("Sys_MapData");
                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Report Design ";
                map.EnType = EnType.Sys;
                map.CodeStruct = "4";

                map.AddTBStringPK(MapRptAttr.No, null, " Serial number ", true, false, 1, 20, 20);
                map.AddTBString(MapRptAttr.Name, null, " Description ", true, false, 0, 500, 20);
           //     map.AddTBString(MapRptAttr.SearchKeys, null, " Query key ", true, false, 0, 500, 20);
                map.AddTBString(MapRptAttr.PTable, null, " Physical table ", true, false, 0, 500, 20);
                map.AddTBString(MapRptAttr.FK_Flow, null, " Process ID ", true, false, 0, 3, 3);

                //Tag
             //   map.AddTBString(MapRptAttr.Tag, null, "Tag", true, false, 0, 500, 20);

                // Time Query : For the report query .
              //  map.AddTBInt(MapRptAttr.IsSearchKey, 0, " The need for keyword query ", true, false);
             //   map.AddTBInt(MapRptAttr.DTSearchWay, 0, " Time query ", true, false);
             //   map.AddTBString(MapRptAttr.DTSearchKey, null, " Time query field ", true, false, 0, 200, 20);
                map.AddTBString(MapRptAttr.Note, null, " Remark ", true, false, 0, 500, 20);
            
                map.AddTBString(MapRptAttr.ParentMapData, null, "ParentMapData", true, false, 0, 128, 20);


                #region  Access control . 2014-12-18
                map.AddTBInt(MapRptAttr.RightViewWay, 0, " Report Viewer control permissions ", true, false);
                map.AddTBString(MapRptAttr.RightViewTag, null, " Report Viewer Access Control Tag", true, false, 0, 4000, 20);

                map.AddTBInt(MapRptAttr.RightDeptWay, 0, " View control department ", true, false);
                map.AddTBString(MapRptAttr.RightDeptTag, null, " View control department Tag", true, false, 0, 4000, 20);

                map.AttrsOfOneVSM.Add(new RptStations(), new Stations(), RptStationAttr.FK_Rpt, RptStationAttr.FK_Station,
                    DeptAttr.Name, DeptAttr.No, " Permissions post ");
                map.AttrsOfOneVSM.Add(new RptDepts(), new Depts(), RptDeptAttr.FK_Rpt, RptDeptAttr.FK_Dept,
                    DeptAttr.Name, DeptAttr.No, " Department Permissions ");
                map.AttrsOfOneVSM.Add(new RptEmps(), new Emps(), RptEmpAttr.FK_Rpt, RptEmpAttr.FK_Emp,
                 DeptAttr.Name, DeptAttr.No, " Staff privileges ");
                #endregion  Access control .


                this._enMap = map;
                return this._enMap;
            }
        }

         
        #endregion

        public MapAttrs HisShowColsAttrs
        {
            get
            {
                MapAttrs mattrs = new MapAttrs(this.No);
                return mattrs;
            }
        }

        /// <summary>
        ///  Whether the person has permission to query the current landing 
        /// </summary>
        public bool IsCanSearchRpt
        {
            get
            {
                string FK_Emp = BP.Web.WebUser.No;
                if (!string.IsNullOrEmpty(FK_Emp))
                {
                    // Judge staff 
                    RptEmps emps = new RptEmps();
                    emps.RetrieveByAttr(RptEmpAttr.FK_Emp, FK_Emp);
                    if (emps != null && emps.Count > 0)
                        return true;

                    // Judgment department 

                    // Judge posts 
                    
                    return true;
                }
                return false;
            }
        }

        protected override bool beforeInsert()
        {
            this.ResetIt();
            return base.beforeInsert();
        }

        public void ResetIt()
        {
            MapData md = new MapData(this.No);
            md.RptIsSearchKey = true;
            md.RptDTSearchWay = DTSearchWay.None;
            md.RptDTSearchKey = "";
            md.RptSearchKeys = "*FK_Dept*WFSta*FK_NY*";

            MapData pmd = new MapData(this.ParentMapData);
            this.PTable = pmd.PTable;
            this.Update();

            string keys = "'OID','FK_Dept','FlowStarter','WFState','Title','FlowStartRDT','FlowEmps','FlowDaySpan','FlowEnder','FlowEnderRDT','FK_NY','FlowEndNode'";
            MapAttrs attrs = new MapAttrs(this.ParentMapData);
            attrs.Delete(MapAttrAttr.FK_MapData, this.No); //  Some fields have been deleted .
            foreach (MapAttr attr in attrs)
            {
                if (keys.Contains("'" + attr.KeyOfEn + "'") == false)
                    continue;
                attr.FK_MapData = this.No;
                attr.Insert();
            }
        }
        
        protected override bool beforeDelete()
        {
            MapAttrs attrs = new MapAttrs();
            attrs.Delete(MapAttrAttr.FK_MapData, this.No);
            return base.beforeDelete();
        }
    }
    /// <summary>
    ///  Report Design s
    /// </summary>
    public class MapRpts : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Report Design s
        /// </summary>
        public MapRpts()
        {
        }
        /// <summary>
        ///  Report Design s
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        public MapRpts(string fk_flow)
        {
            string fk_Mapdata = "ND" + int.Parse(fk_flow) + "Rpt";
            int i = this.Retrieve(MapRptAttr.ParentMapData, fk_Mapdata);
            if (i == 0)
            {
                MapData mapData = new MapData(fk_Mapdata);
                mapData.No = "ND" + int.Parse(fk_flow) + "MyRpt";
                mapData.Name = " My Process ";
                mapData.Note = " The system automatically generates .";
                mapData.Insert();

                MapRpt rpt = new MapRpt(mapData.No, fk_flow);
                rpt.ParentMapData = fk_Mapdata;
                rpt.ResetIt();


                rpt.Update();
            }
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapRpt();
            }
        }
        #endregion
    }
}
