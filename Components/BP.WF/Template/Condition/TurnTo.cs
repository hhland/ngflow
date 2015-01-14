using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.WF.Template
{
    /// <summary>
    ///  Condition attributes 
    /// </summary>
    public class TurnToAttr
    {
        /// <summary>
        ///  Property Key
        /// </summary>
        public const string FK_Attr = "FK_Attr";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string AttrT = "AttrT";
        /// <summary>
        ///  Operators 
        /// </summary>
        public const string FK_Operator = "FK_Operator";
        /// <summary>
        ///  Value calculation 
        /// </summary>
        public const string OperatorValue = "OperatorValue";
        /// <summary>
        ///  Operating values 
        /// </summary>
        public const string OperatorValueT = "OperatorValueT";
        /// <summary>
        /// Node
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Condition Type 
        /// </summary>
        public const string TurnToType = "TurnToType";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        /// TurnToURL
        /// </summary>
        public const string TurnToURL = "TurnToURL";
        /// <summary>
        /// AttrKey
        /// </summary>
        public const string AttrKey = "AttrKey";
    }
    /// <summary>
    ///  Condition Type 
    /// </summary>
    public enum TurnToType
    {
        /// <summary>
        ///  Node 
        /// </summary>
        Node,
        /// <summary>
        ///  Process 
        /// </summary>
        Flow
    }
    /// <summary>
    ///  Condition 
    /// </summary>
    public class TurnTo : EntityMyPK
    {
        /// <summary>
        ///  Process 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(TurnToAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Node 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(TurnToAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(TurnToAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Condition Type 
        /// </summary>
        public TurnToType HisTurnToType
        {
            get
            {
                return (TurnToType)this.GetValIntByKey(TurnToAttr.TurnToType);
            }
            set
            {
                this.SetValByKey(TurnToAttr.TurnToType, (int)value);
            }
        }
        /// <summary>
        ///  Steering URL
        /// </summary>
        public string TurnToURL
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.TurnToURL);
            }
            set
            {
                this.SetValByKey(TurnToAttr.TurnToURL, value);
            }
        }

        #region  Achieve the basic square method 
        /// <summary>
        ///  Property 
        /// </summary>
        public string FK_Attr
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.FK_Attr);
            }
            set
            {
                if (value == null)
                    throw new Exception("FK_Attr Can not be set null");

                value = value.Trim();
                this.SetValByKey(TurnToAttr.FK_Attr, value);
                BP.Sys.MapAttr attr = new BP.Sys.MapAttr(value);
                this.SetValByKey(TurnToAttr.AttrKey, attr.KeyOfEn);
                this.SetValByKey(TurnToAttr.AttrT, attr.Name);
            }
        }
        /// <summary>
        ///  Property Key
        /// </summary>
        public string AttrKey
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.AttrKey);
            }
        }
        /// <summary>
        ///  Property Text
        /// </summary>
        public string AttrT
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.AttrT);
            }
        }
        /// <summary>
        ///  The value of the operation 
        /// </summary>
        public string OperatorValueT
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.OperatorValueT);
            }
            set
            {
                this.SetValByKey(TurnToAttr.OperatorValueT, value);
            }
        }
        /// <summary>
        ///  Operators 
        /// </summary>
        public string FK_Operator
        {
            get
            {
                string s = this.GetValStringByKey(TurnToAttr.FK_Operator);
                if (s == null || s == "")
                    return "=";
                return s;
            }
            set
            {
                this.SetValByKey(TurnToAttr.FK_Operator, value);
            }
        }
        /// <summary>
        ///  Operator Description 
        /// </summary>
        public string FK_OperatorExt
        {
            get
            {
                string s = this.FK_Operator.ToLower();
                switch (s)
                {
                    case "=":
                        return "dengyu";
                    case ">":
                        return "dayu";
                    case ">=":
                        return "dayudengyu";
                    case "<":
                        return "xiaoyu";
                    case "<=":
                        return "xiaoyudengyu";
                    case "!=":
                        return "budengyu";
                    case "like":
                        return "like";
                    default:
                        return s;
                }
            }
        }
        /// <summary>
        ///  Computed values 
        /// </summary>
        public object OperatorValue
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.OperatorValue);
            }
            set
            {
                this.SetValByKey(TurnToAttr.OperatorValue, value as string);
            }
        }
        /// <summary>
        ///  Operating values str
        /// </summary>
        public string OperatorValueStr
        {
            get
            {
                return this.GetValStringByKey(TurnToAttr.OperatorValue);
            }
        }
        /// <summary>
        ///  Operating values int
        /// </summary>
        public int OperatorValueInt
        {
            get
            {
                return this.GetValIntByKey(TurnToAttr.OperatorValue);
            }
        }
        /// <summary>
        ///  Operating values boolen
        /// </summary>
        public bool OperatorValueBool
        {
            get
            {
                return this.GetValBooleanByKey(TurnToAttr.OperatorValue);
            }
        }
        /// <summary>
        /// workid
        /// </summary>
        public Int64 WorkID = 0;
        /// <summary>
        ///  Steering news 
        /// </summary>
        public string MsgOfTurnTo = "";
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Condition 
        /// </summary>
        public TurnTo() { }
        /// <summary>
        ///  Condition 
        /// </summary>
        /// <param name="mypk">PK</param>
        public TurnTo(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        #endregion

        #region  Public Methods 
        /// <summary>
        ///  It worked 
        /// </summary>
        public Work HisWork = null;
        /// <summary>
        ///  This condition can not pass 
        /// </summary>
        public virtual bool IsPassed
        {
            get
            {

                BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
                attr.MyPK = this.FK_Attr;
                attr.RetrieveFromDBSources();

                if (this.HisWork.EnMap.Attrs.Contains(attr.KeyOfEn) == false)
                    throw new Exception(" Determine the direction of the error condition : Entity :" + this.HisWork.EnDesc + "  Property " + this.FK_Attr + " Does not exist .");

                this.MsgOfTurnTo = "@ Direction to form value judgments ,ֵ " + this.HisWork.EnDesc + "." + this.FK_Attr + " (" + this.HisWork.GetValStringByKey(attr.KeyOfEn) + ")  Operator :(" + this.FK_Operator + ")  Judgment value :(" + this.OperatorValue.ToString() + ")";

                switch (this.FK_Operator.Trim().ToLower())
                {
                    case "=":  //  In the case of  = 
                        if (this.HisWork.GetValStringByKey(attr.KeyOfEn) == this.OperatorValue.ToString())
                            return true;
                        else
                            return false;

                    case ">":
                        if (this.HisWork.GetValDoubleByKey(attr.KeyOfEn) > Double.Parse(this.OperatorValue.ToString()))
                            return true;
                        else
                            return false;

                    case ">=":
                        if (this.HisWork.GetValDoubleByKey(attr.KeyOfEn) >= Double.Parse(this.OperatorValue.ToString()))
                            return true;
                        else
                            return false;

                    case "<":
                        if (this.HisWork.GetValDoubleByKey(attr.KeyOfEn) < Double.Parse(this.OperatorValue.ToString()))
                            return true;
                        else
                            return false;

                    case "<=":
                        if (this.HisWork.GetValDoubleByKey(attr.KeyOfEn) <= Double.Parse(this.OperatorValue.ToString()))
                            return true;
                        else
                            return false;
                    case "!=":
                        if (this.HisWork.GetValDoubleByKey(attr.KeyOfEn) != Double.Parse(this.OperatorValue.ToString()))
                            return true;
                        else
                            return false;
                    case "like":
                        if (this.HisWork.GetValStringByKey(attr.KeyOfEn).IndexOf(this.OperatorValue.ToString()) == -1)
                            return false;
                        else
                            return true;
                    default:
                        throw new Exception("@ Operation Symbol not found ..");
                }
            }
        }
        /// <summary>
        ///  Property 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_TurnTo");
                map.EnDesc = " Steering condition ";

                map.AddMyPK();
                map.AddTBInt(TurnToAttr.TurnToType, 0, " Condition Type ", true, true);
                map.AddTBString(TurnToAttr.FK_Flow, null, " Process ", true, true, 0, 60, 20);
                map.AddTBInt(TurnToAttr.FK_Node, 0, " Node ID", true, true);

                map.AddTBString(TurnToAttr.FK_Attr, null, " Foreign key attribute Sys_MapAttr", true, true, 0, 80, 20);
                map.AddTBString(TurnToAttr.AttrKey, null, " Key ", true, true, 0, 80, 20);
                map.AddTBString(TurnToAttr.AttrT, null, " Property name ", true, true, 0, 80, 20);

                map.AddTBString(TurnToAttr.FK_Operator, "=", " Operators ", true, true, 0, 60, 20);

                map.AddTBString(TurnToAttr.OperatorValue, "", " Value to operations ", true, true, 0, 60, 20);
                map.AddTBString(TurnToAttr.OperatorValueT, "", " Value to operations T", true, true, 0, 60, 20);

                map.AddTBString(TurnToAttr.TurnToURL, null, " To turn URL", true, true, 0, 700, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Condition s
    /// </summary>
    public class TurnTos : Entities
    {
        #region  Property 
        /// <summary>
        ///  Condition 
        /// </summary>
        public override Entity GetNewEntity
        {
            get { return new TurnTo(); }
        }
        /// <summary>
        ///  Condition .
        /// </summary>
        public bool IsAllPassed
        {
            get
            {
                if (this.Count == 0)
                    throw new Exception("@ Do not want to judge the set .");

                foreach (TurnTo en in this)
                {
                    if (en.IsPassed == false)
                        return false;
                }
                return true;
            }
        }
        /// <summary>
        ///  Whether through 
        /// </summary>
        public bool IsPass
        {
            get
            {
                if (this.Count == 1)
                    if (this.IsOneOfTurnToPassed)
                        return true;
                    else
                        return false;
                return false;
            }
        }
        public string MsgOfDesc
        {
            get
            {
                string msg = "";
                foreach (TurnTo c in this)
                {
                    msg += "@" + c.MsgOfTurnTo;
                }
                return msg;
            }
        }
        /// <summary>
        ///  Is not one of passed. 
        /// </summary>
        public bool IsOneOfTurnToPassed
        {
            get
            {
                foreach (TurnTo en in this)
                {
                    if (en.IsPassed == true)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        ///  Remove one of the closing conditions .. 
        /// </summary>
        public TurnTo GetOneOfTurnToPassed
        {
            get
            {
                foreach (TurnTo en in this)
                {
                    if (en.IsPassed == true)
                        return en;
                }
                throw new Exception("@ No closing conditions .");
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID = 0;
        #endregion

        #region  Structure 
        /// <summary>
        ///  Condition 
        /// </summary>
        public TurnTos()
        {
        }
        /// <summary>
        ///  Condition 
        /// </summary>
        public TurnTos(string fk_flow)
        {
            this.Retrieve(TurnToAttr.FK_Flow, fk_flow);
        }
        /// <summary>
        ///  Condition 
        /// </summary>
        /// <param name="ct"> Type </param>
        /// <param name="nodeID"> Node </param>
        public TurnTos(TurnToType ct, int nodeID, Int64 workid)
        {
            this.NodeID = nodeID;
            this.Retrieve(TurnToAttr.FK_Node, nodeID, TurnToAttr.TurnToType, (int)ct);

            foreach (TurnTo en in this)
                en.WorkID = workid;
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string TurnToitionDesc
        {
            get
            {
                return "";
            }
        }
        #endregion
    }
}
