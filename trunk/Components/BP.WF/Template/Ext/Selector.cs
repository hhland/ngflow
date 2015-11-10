using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.WF.Port;

namespace BP.WF.Template
{
    /// <summary>
    ///  Conditions direction control rules 
    /// </summary>
    public enum CondModel
    {
        /// <summary>
        ///  Calculated in accordance with the conditions set by the user direction 
        /// </summary>
        ByLineCond,
        /// <summary>
        ///  Calculated in accordance with the user selects 
        /// </summary>
        ByUserSelected
    }
    /// <summary>
    ///  Relationship Type 
    /// </summary>
    public enum CondOrAnd
    {
        /// <summary>
        ///  All conditions have been established set of relationships inside .
        /// </summary>
        ByAnd,
        /// <summary>
        ///  Relationship where there is only one set of conditions are met .
        /// </summary>
        ByOr
    }
    /// <summary>
    ///  Upcoming work overtime handling 
    /// </summary>
    public enum OutTimeDeal
    {
        /// <summary>
        ///  Does not deal with 
        /// </summary>
        None,
        /// <summary>
        ///  Automatic steering to the next step 
        /// </summary>
        AutoTurntoNextStep,
        /// <summary>
        ///  Automatically jump to a specific point 
        /// </summary>
        AutoJumpToSpecNode,
        /// <summary>
        ///  Automatically transferred to the designated person 
        /// </summary>
        AutoShiftToSpecUser,
        /// <summary>
        ///  Send a message to the designated officer 
        /// </summary>
        SendMsgToSpecUser,
        /// <summary>
        ///  Delete Process 
        /// </summary>
        DeleteFlow,
        /// <summary>
        ///  Carried out SQL
        /// </summary>
        RunSQL
    }
    /// <summary>
    ///  Display mode 
    /// </summary>
    public enum SelectorDBShowWay
    {
        /// <summary>
        ///  Form 
        /// </summary>
        Table,
        /// <summary>
        /// 树
        /// </summary>
        Tree
    }
    public enum SelectorModel
    {
        /// <summary>
        ///  Form 
        /// </summary>
        Station,
        /// <summary>
        /// 树
        /// </summary>
        Dept,
        /// <summary>
        ///  The operator 
        /// </summary>
        Emp,
        /// <summary>
        /// SQL
        /// </summary>
        SQL,
        /// <summary>
        ///  Custom Link 
        /// </summary>
        Url
    }
    /// <summary>
    /// Selector Property 
    /// </summary>
    public class SelectorAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Process 
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        ///  Accepted model 
        /// </summary>
        public const string SelectorModel = "SelectorModel";

        public const string SelectorP1 = "SelectorP1";
        public const string SelectorP2 = "SelectorP2";
        /// <summary>
        ///  Data Display ( Tables and tree )
        /// </summary>
        public const string SelectorDBShowWay = "SelectorDBShowWay";
    }
    /// <summary>
    ///  Selector 
    /// </summary>
    public class Selector : Entity
    {
        #region  Basic properties 
        public override string PK
        {
            get
            {
                return "NodeID";
            }
        }
        /// <summary>
        ///  Display mode 
        /// </summary>
        public SelectorDBShowWay SelectorDBShowWay
        {
            get
            {
                return (SelectorDBShowWay)this.GetValIntByKey(SelectorAttr.SelectorDBShowWay);
            }
            set
            {
                this.SetValByKey(SelectorAttr.SelectorDBShowWay, (int)value);
            }
        }
        /// <summary>
        ///  Select mode 
        /// </summary>
        public SelectorModel SelectorModel
        {
            get
            {
                return (SelectorModel)this.GetValIntByKey(SelectorAttr.SelectorModel);
            }
            set
            {
                this.SetValByKey(SelectorAttr.SelectorModel, (int)value);
            }
        }

        public string SelectorP1
        {
            get
            {
                string s= this.GetValStringByKey(SelectorAttr.SelectorP1);
                s = s.Replace("~", "'");
                return s;
            }
            set
            {
                this.SetValByKey(SelectorAttr.SelectorP1, value);
            }
        }
        public string SelectorP2
        {
            get
            {
                string s = this.GetValStringByKey(SelectorAttr.SelectorP2);
                s = s.Replace("~", "'");
                return s;
                //return this.GetValStringByKey(SelectorAttr.SelectorP2);
            }
            set
            {
                this.SetValByKey(SelectorAttr.SelectorP2, value);
            }
        }
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(SelectorAttr.NodeID);
            }
            set
            {
                this.SetValByKey(SelectorAttr.NodeID, value);
            }
        }
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsView = false;
                if (BP.Web.WebUser.No == "admin")
                {
                    uac.IsUpdate = true;
                    uac.IsView = true;
                }
                return uac;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// Accpter
        /// </summary>
        public Selector() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        public Selector(int nodeid)
        {
            this.NodeID = nodeid;
            this.Retrieve();
        }

        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_Node");
                map.EnDesc = " Selector ";

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(SelectorAttr.NodeID, 0, "NodeID", true, true);
                map.AddTBString(SelectorAttr.Name, null, " Node Name ", true, true, 0,100,100);

                map.AddDDLSysEnum(SelectorAttr.SelectorDBShowWay, 0, " Data Display ", true, true,
                SelectorAttr.SelectorDBShowWay, "@0= The table shows @1= Tree display ");
                map.AddDDLSysEnum(SelectorAttr.SelectorModel, 0, " Window Mode ", true, true, SelectorAttr.SelectorModel,
                    "@0= By post @1= By sector @2= By staff @3=By SQL@4= Custom Url");


                //map.AddTBString(SelectorAttr.SelectorP1, null, " Parameters 1", true, false, 0, 500, 10, true);
                //map.AddTBString(SelectorAttr.SelectorP2, null, " Parameters 2", true, false, 0, 500, 10, true);

                map.AddTBStringDoc(SelectorAttr.SelectorP1, null, " Parameters 1", true, false, true);
                map.AddTBStringDoc(SelectorAttr.SelectorP2, null, " Parameters 2", true, false, true);

            

                //  Related functions .
                map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeStations(), new BP.WF.Port.Stations(),
                    NodeStationAttr.FK_Node, NodeStationAttr.FK_Station,
                    DeptAttr.Name, DeptAttr.No, " Node positions ");

                map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeDepts(), new BP.WF.Port.Depts(), NodeDeptAttr.FK_Node, NodeDeptAttr.FK_Dept, DeptAttr.Name,
                DeptAttr.No, " Node department ");

                map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeEmps(), new BP.WF.Port.Emps(), NodeEmpAttr.FK_Node, EmpDeptAttr.FK_Emp, DeptAttr.Name,
                    DeptAttr.No, " Acceptance of staff ");


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    /// Accpter
    /// </summary>
    public class Selectors : Entities
    {
        /// <summary>
        /// Accpter
        /// </summary>
        public Selectors()
        {
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Selector();
            }
        }
    }
}
