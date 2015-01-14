using System;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.WF.Template;
using BP.Sys;

namespace BP.WF.Data
{
    /// <summary>
    ///   Property 
    /// </summary>
    public class GERptAttr
    {
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Participants 
        /// </summary>
        public const string FlowEmps = "FlowEmps";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// Workid
        /// </summary>
        public const string OID = "OID";
        /// <summary>
        ///  Launch date 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Sponsor ID
        /// </summary>
        public const string FlowStarter = "FlowStarter";
        /// <summary>
        ///  Launch date 
        /// </summary>
        public const string FlowStartRDT = "FlowStartRDT";
        /// <summary>
        ///  Sponsor department ID
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFState = "WFState";
        /// <summary>
        ///  Process Status 
        /// </summary>
        public const string WFSta = "WFSta";
        /// <summary>
        ///  Quantity 
        /// </summary>
        public const string MyNum = "MyNum";
        /// <summary>
        ///  End people 
        /// </summary>
        public const string FlowEnder = "FlowEnder";
        /// <summary>
        ///  Last Activity Date 
        /// </summary>
        public const string FlowEnderRDT = "FlowEnderRDT";
        /// <summary>
        ///  Span 
        /// </summary>
        public const string FlowDaySpan = "FlowDaySpan";
        /// <summary>
        ///  End node 
        /// </summary>
        public const string FlowEndNode = "FlowEndNode";
        /// <summary>
        ///  Parent process WorkID
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        ///  Parent process node sends 
        /// </summary>
        public const string PNodeID = "PNodeID";
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public const string PFlowNo = "PFlowNo";
        /// <summary>
        ///  Call the staff of the parent process 
        /// </summary>
        public const string PEmp = "PEmp";
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public const string GuestNo = "GuestNo";
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public const string GuestName = "GuestName";
        /// <summary>
        ///  Document Number 
        /// </summary>
        public const string BillNo = "BillNo";
        /// <summary>
        ///  Process Notes 
        /// </summary>
        public const string FlowNote = "FlowNote";
        /// <summary>
        ///  Continuation of the process ID
        /// </summary>
        public const string CWorkID = "CWorkID";
        /// <summary>
        ///  Continuation of the process ID 
        /// </summary>
        public const string CFlowNo = "CFlowNo";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string AtPara = "AtPara";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        /// <summary>
        ///  Item Number 
        /// </summary>
        public const string ProjNo = "ProjNo";
    }
    /// <summary>
    ///  Report form 
    /// </summary>
    public class GERpt : BP.En.EntityOID
    {
        #region attrs
        public new Int64 OID
        {
            get
            {
                return this.GetValInt64ByKey(GERptAttr.OID);
            }
            set
            {
                this.SetValByKey(GERptAttr.OID, value);
            }
        }
        /// <summary>
        ///  Process time span 
        /// </summary>
        public int FlowDaySpan
        {
            get
            {
                return this.GetValIntByKey(GERptAttr.FlowDaySpan);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowDaySpan, value);
            }
        }
        public int MyNum
        {
            get
            {
                return this.GetValIntByKey(GERptAttr.MyNum);
            }
            set
            {
                this.SetValByKey(GERptAttr.MyNum, value);
            }
        }
        /// <summary>
        ///  The main flow ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(GERptAttr.FID);
            }
            set
            {
                this.SetValByKey(GERptAttr.FID, value);
            }
        }
        public string GUID
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.GUID);
            }
            set
            {
                this.SetValByKey(GERptAttr.GUID, value);
            }
        }
        /// <summary>
        ///  Process participants 
        /// </summary>
        public string FlowEmps
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowEmps);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowEmps, value);
            }
        }
        /// <summary>
        ///  Process Notes 
        /// </summary>
        public string FlowNote
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowNote);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowNote, value);
            }
        }
        /// <summary>
        ///  Customer Number 
        /// </summary>
        public string GuestNo
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.GuestNo);
            }
            set
            {
                this.SetValByKey(GERptAttr.GuestNo, value);
            }
        }
        /// <summary>
        ///  Customer Name 
        /// </summary>
        public string GuestName
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.GuestName);
            }
            set
            {
                this.SetValByKey(GERptAttr.GuestName, value);
            }
        }
        public string BillNo
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.BillNo);
            }
            set
            {
                this.SetValByKey(GERptAttr.BillNo, value);
            }
        }
        /// <summary>
        ///  Process sponsor 
        /// </summary>
        public string FlowStarter
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowStarter);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowStarter, value);
            }
        }
        /// <summary>
        ///  Process initiated by date 
        /// </summary>
        public string FlowStartRDT
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowStartRDT);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowStartRDT, value);
            }
        }
        /// <summary>
        ///  End of the process by 
        /// </summary>
        public string FlowEnder
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowEnder);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowEnder, value);
            }
        }
        /// <summary>
        ///  Process End Time 
        /// </summary>
        public string FlowEnderRDT
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FlowEnderRDT);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowEnderRDT, value);
            }
        }
        public string FlowEndNodeText
        {
            get
            {
                Node nd =new Node(this.FlowEndNode);
                return nd.Name;
            }
        }
        public int FlowEndNode
        {
            get
            {
                return this.GetValIntByKey(GERptAttr.FlowEndNode);
            }
            set
            {
                this.SetValByKey(GERptAttr.FlowEndNode, value);
            }
        }
        /// <summary>
        ///  Process title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.Title);
            }
            set
            {
                this.SetValByKey(GERptAttr.Title, value);
            }
        }
        /// <summary>
        ///  Years of membership 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(GERptAttr.FK_NY, value);
            }
        }
        /// <summary>
        ///  Sponsor department 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(GERptAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Process Status 
        /// </summary>
        public WFState WFState
        {
            get
            {
                return (WFState)this.GetValIntByKey(GERptAttr.WFState);
            }
            set
            {
                if (value == WF.WFState.Complete)
                    SetValByKey(GenerWorkFlowAttr.WFSta, (int)WFSta.Complete);
                else if (value == WF.WFState.Delete)
                    SetValByKey(GenerWorkFlowAttr.WFSta, (int)WFSta.Delete);
                else
                    SetValByKey(GenerWorkFlowAttr.WFSta, (int)WFSta.Runing);

                this.SetValByKey(GERptAttr.WFState, (int)value);
            }
        }
        /// <summary>
        ///  Parent process WorkID
        /// </summary>
        public Int64 PWorkID
        {
            get
            {
                return this.GetValInt64ByKey(GERptAttr.PWorkID);
            }
            set
            {
                this.SetValByKey(GERptAttr.PWorkID, value);
            }
        }
        /// <summary>
        ///  Node issued 
        /// </summary>
        public int PNodeID
        {
            get
            {
                return this.GetValIntByKey(GERptAttr.PNodeID);
            }
            set
            {
                this.SetValByKey(GERptAttr.PNodeID, value);
            }
        }
        /// <summary>
        ///  Parent process ID process 
        /// </summary>
        public string PFlowNo
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.PFlowNo);
            }
            set
            {
                this.SetValByKey(GERptAttr.PFlowNo, value);
            }
        }
        public string PEmp
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.PEmp);
            }
            set
            {
                this.SetValByKey(GERptAttr.PEmp, value);
            }
        }
        /// <summary>
        ///  Continuation of the process WorkID
        /// </summary>
        public Int64 CWorkID
        {
            get
            {
                return this.GetValInt64ByKey(GERptAttr.CWorkID);
            }
            set
            {
                this.SetValByKey(GERptAttr.CWorkID, value);
            }
        }
        /// <summary>
        ///  Continuation of the process flow numbers 
        /// </summary>
        public string CFlowNo
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.CFlowNo);
            }
            set
            {
                this.SetValByKey(GERptAttr.CFlowNo, value);
            }
        }
        /// <summary>
        ///  Item Number 
        /// </summary>
        public string ProjNo
        {
            get
            {
                return this.GetValStringByKey(GERptAttr.ProjNo);
            }
            set
            {
                this.SetValByKey(GERptAttr.ProjNo, value);
            }
        }
        #endregion attrs

        #region  Rewrite .
        public override void Copy(Entity fromEn)
        {
            if (fromEn == null)
                return;

            Attrs attrs = fromEn.EnMap.Attrs;
            foreach (Attr attr in attrs)
            {
                if (attr.Key == WorkAttr.CDT
                    || attr.Key == WorkAttr.RDT
                    || attr.Key == WorkAttr.Rec
                    || attr.Key == WorkAttr.FID
                    || attr.Key == WorkAttr.OID
                    || attr.Key == WorkAttr.Emps
                    || attr.Key == GERptAttr.AtPara
                    || attr.Key == GERptAttr.BillNo
                    || attr.Key == GERptAttr.CFlowNo
                    || attr.Key == GERptAttr.CWorkID
                    || attr.Key == GERptAttr.FID
                    || attr.Key == GERptAttr.FK_Dept
                    || attr.Key == GERptAttr.FK_NY
                    || attr.Key == GERptAttr.FlowDaySpan
                    || attr.Key == GERptAttr.FlowEmps
                    || attr.Key == GERptAttr.FlowEnder
                    || attr.Key == GERptAttr.FlowEnderRDT
                    || attr.Key == GERptAttr.FlowEndNode
                    || attr.Key == GERptAttr.FlowNote
                    || attr.Key == GERptAttr.FlowStarter
                    || attr.Key == GERptAttr.GuestName
                    || attr.Key == GERptAttr.GuestNo
                    || attr.Key == GERptAttr.GUID
                    || attr.Key == GERptAttr.PEmp
                    || attr.Key == GERptAttr.PFlowNo
                    || attr.Key == GERptAttr.PNodeID
                    || attr.Key == GERptAttr.PWorkID
                    || attr.Key == GERptAttr.Title
                    || attr.Key == GERptAttr.ProjNo
                    || attr.Key == "No"
                    || attr.Key == "Name")
                    continue;


                if (attr.Key== GERptAttr.MyNum)
                {
                    this.SetValByKey(attr.Key, 1);
                    continue;
                }
                this.SetValByKey(attr.Key, fromEn.GetValByKey(attr.Key));
            }
        }
        #endregion

        #region attrs - attrs
        private string _RptName = null;
        public string RptName
        {
            get
            {
                return _RptName;
            }
            set
            {
                this._RptName = value;

                this._SQLCash = null;
                this._enMap = null;
                this.Row = null;
            }
        }
        public override string ToString()
        {
            return RptName;
        }
        public override string PK
        {
            get
            {
                return "OID";
            }
        }
        public override string PKField
        {
            get
            {
                return "OID";
            }
        }
        /// <summary>
        /// Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this.RptName == null)
                {
                    BP.Port.Emp emp = new BP.Port.Emp();
                    return emp.EnMap;
                }

                if (this._enMap == null)
                    this._enMap = MapData.GenerHisMap(this.RptName);

                return this._enMap;
            }
        }
        /// <summary>
        ///  Report form 
        /// </summary>
        /// <param name="rptName"></param>
        public GERpt(string rptName)
        {
            this.RptName = rptName;
        }
        public GERpt()
        {
        }
        /// <summary>
        ///  Report form 
        /// </summary>
        /// <param name="rptName"></param>
        /// <param name="oid"></param>
        public GERpt(string rptName, Int64 oid)
        {
            this.RptName = rptName;
            this.OID = (int)oid;
            this.Retrieve();
        }
        #endregion attrs
    }
    /// <summary>
    ///  Collection of reports 
    /// </summary>
    public class GERpts : BP.En.EntitiesOID
    {
        /// <summary>
        ///  Collection of reports 
        /// </summary>
        public GERpts()
        {
        }

        public override Entity GetNewEntity
        {
            get
            {
                return new GERpt();
            }
        }
    }
}
