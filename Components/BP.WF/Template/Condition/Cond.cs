using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.WF.Data;

namespace BP.WF.Template
{
    /// <summary>
    ///  Condition data source 
    /// </summary>
    public enum ConnDataFrom
    {
        /// <summary>
        ///  Form data 
        /// </summary>
        Form,
        /// <summary>
        ///  Jobs data 
        /// </summary>
        Stas,
        /// <summary>
        /// Depts
        /// </summary>
        Depts,
        /// <summary>
        /// 按sql Calculate .
        /// </summary>
        SQL,
        /// <summary>
        ///  By parameters 
        /// </summary>
        Paras,
        /// <summary>
        /// 按Url.
        /// </summary>
        Url
    }
    /// <summary>
    ///  Condition attributes 
    /// </summary>
    public class CondAttr
    {
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public const string DataFrom = "DataFrom";
        /// <summary>
        ///  Property Key
        /// </summary>
        public const string NodeID = "NodeID";
        /// <summary>
        ///  Property Key
        /// </summary>
        public const string AttrKey = "AttrKey";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string AttrName = "AttrName";
        /// <summary>
        ///  Property 
        /// </summary>
        public const string FK_Attr = "FK_Attr";
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
        public const string CondType = "CondType";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Conditions for effective direction 
        /// </summary>
        public const string ToNodeID = "ToNodeID";
        /// <summary>
        ///  Way to judge 
        /// </summary>
        public const string ConnJudgeWay = "ConnJudgeWay";
        /// <summary>
        /// MyPOID
        /// </summary>
        public const string MyPOID = "MyPOID";
        /// <summary>
        /// PRI
        /// </summary>
        public const string PRI = "PRI";
        /// <summary>
        ///  Condition Type .
        /// </summary>
        public const string CondOrAnd = "CondOrAnd";
    }
    /// <summary>
    ///  Condition Type 
    /// </summary>
    public enum CondType
    {
        /// <summary>
        ///  Node closing conditions 
        /// </summary>
        Node,
        /// <summary>
        ///  Process conditions 
        /// </summary>
        Flow,
        /// <summary>
        ///  Direction of the condition 
        /// </summary>
        Dir
    }
    /// <summary>
    ///  Condition 
    /// </summary>
    public class Cond : EntityMyPK
    {
        public GERpt en = null;
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public ConnDataFrom HisDataFrom
        {
            get
            {
                return (ConnDataFrom)this.GetValIntByKey(CondAttr.DataFrom);
            }
            set
            {
                this.SetValByKey(CondAttr.DataFrom, (int)value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(CondAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Condition Type ( Form Conditions , Job conditions , Sector Conditions , Developers parameters )
        /// </summary>
        public CondType HisCondType
        {
            get
            {
                return (CondType)this.GetValIntByKey(CondAttr.CondType);
            }
            set
            {
                this.SetValByKey(CondAttr.CondType, (int)value);
            }
        }
        /// <summary>
        ///  Node to be operated 
        /// </summary>
        public Node HisNode
        {
            get
            {
                return new Node(this.NodeID);
            }
        }
        /// <summary>
        ///  Priority 
        /// </summary>
        public int PRI
        {
            get
            {
                return this.GetValIntByKey(CondAttr.PRI);
            }
            set
            {
                this.SetValByKey(CondAttr.PRI, value);
            }
        }
        /// <summary>
        /// MyPOID
        /// </summary>
        public int MyPOID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.MyPOID);
            }
            set
            {
                this.SetValByKey(CondAttr.MyPOID, value);
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.NodeID);
            }
            set
            {
                this.SetValByKey(CondAttr.NodeID, value);
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                int i = this.GetValIntByKey(CondAttr.FK_Node);
                if (i == 0)
                    return this.NodeID;
                return i;
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Node, value);
            }
        }
        /// <summary>
        ///  Node Name 
        /// </summary>
        public string FK_NodeT
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                return nd.Name;
            }
        }
        /// <summary>
        ///  Conditions for effective direction 
        /// </summary>
        public int ToNodeID
        {
            get
            {
                return this.GetValIntByKey(CondAttr.ToNodeID);
            }
            set
            {
                this.SetValByKey(CondAttr.ToNodeID, value);
            }
        }
        /// <summary>
        ///  Relationship Type 
        /// </summary>
        public CondOrAnd CondOrAnd
        {
            get
            {
                return (CondOrAnd)this.GetValIntByKey(CondAttr.CondOrAnd);
            }
            set
            {
                this.SetValByKey(CondAttr.CondOrAnd, (int)value);
            }
        }
        /// <summary>
        ///  Before the update and insert operations should be done .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdateInsertAction()
        {
            this.RunSQL("UPDATE WF_Node SET IsCCFlow=0");
           // this.RunSQL("UPDATE WF_Node SET IsCCNode=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond WHERE CondType=" + (int)CondType.Node + ")");
            this.RunSQL("UPDATE WF_Node SET IsCCFlow=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond WHERE CondType=" + (int)CondType.Flow + ")");

            this.MyPOID = BP.DA.DBAccess.GenerOID();
            return base.beforeUpdateInsertAction();
        }

        #region  Achieve the basic square method 
        /// <summary>
        ///  Property 
        /// </summary>
        public string FK_Attr
        {
            get
            {
                return this.GetValStringByKey(CondAttr.FK_Attr);
            }
            set
            {
                if (value == null)
                    throw new Exception("FK_Attr Can not be set null");

                value = value.Trim();

                this.SetValByKey(CondAttr.FK_Attr, value);

                BP.Sys.MapAttr attr = new BP.Sys.MapAttr(value);
                this.SetValByKey(CondAttr.AttrKey, attr.KeyOfEn);
                this.SetValByKey(CondAttr.AttrName, attr.Name);
            }
        }
        /// <summary>
        ///  Entity attributes to be operated 
        /// </summary>
        public string AttrKey
        {
            get
            {
                return this.GetValStringByKey(CondAttr.AttrKey);
            }
        }
        /// <summary>
        ///  Property name 
        /// </summary>
        public string AttrName
        {
            get
            {
                return this.GetValStringByKey(CondAttr.AttrName);
            }
        }
        /// <summary>
        ///  The value of the operation 
        /// </summary>
        public string OperatorValueT
        {
            get
            {
                return this.GetValStringByKey(CondAttr.OperatorValueT);
            }
            set
            {
                this.SetValByKey(CondAttr.OperatorValueT, value);
            }
        }
        /// <summary>
        ///  Operators 
        /// </summary>
        public string FK_Operator
        {
            get
            {
                string s = this.GetValStringByKey(CondAttr.FK_Operator);
                if (s == null || s == "")
                    return "=";
                return s;
            }
            set
            {
                this.SetValByKey(CondAttr.FK_Operator, value);
            }
        }
        /// <summary>
        ///  Computed values 
        /// </summary>
        public object OperatorValue
        {
            get
            {
                string s= this.GetValStringByKey(CondAttr.OperatorValue);
                s = s.Replace("~", "'");
                return s;
            }
            set
            {                
                this.SetValByKey(CondAttr.OperatorValue, value as string);
            }
        }
        /// <summary>
        ///  Operating values Str
        /// </summary>
        public string OperatorValueStr
        {
            get
            {
                string sql= this.GetValStringByKey(CondAttr.OperatorValue);
                sql = sql.Replace("~", "'");
                return sql;
            }
        }
        /// <summary>
        ///  Operating values int
        /// </summary>
        public int OperatorValueInt
        {
            get
            {
                return this.GetValIntByKey(CondAttr.OperatorValue);
            }
        }
        /// <summary>
        ///  Operating values boolen
        /// </summary>
        public bool OperatorValueBool
        {
            get
            {
                return this.GetValBooleanByKey(CondAttr.OperatorValue);
            }
        }
        /// <summary>
        /// workid
        /// </summary>
        private Int64 _WorkID = 0;
        public Int64 WorkID
        {
            get
            {
                return _WorkID;
            }
            set
            {
                _WorkID = value;
            }
        }
        /// <summary>
        ///  Conditions news 
        /// </summary>
        public string MsgOfCond = "";
        /// <summary>
        ///  Move 
        /// </summary>
        /// <param name="fk_node"> Node ID</param>
        public void DoUp(int fk_node)
        {
            int condtypeInt = (int)this.HisCondType;
            this.DoOrderUp(CondAttr.FK_Node, fk_node.ToString(),CondAttr.CondType, condtypeInt.ToString(), CondAttr.PRI);
        }
        /// <summary>
        ///  Down 
        /// </summary>
        /// <param name="fk_node"> Node ID</param>
        public void DoDown(int fk_node)
        {
            int condtypeInt = (int)this.HisCondType;
            this.DoOrderDown(CondAttr.FK_Node, fk_node.ToString(), CondAttr.CondType, condtypeInt.ToString(), CondAttr.PRI);
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Condition 
        /// </summary>
        public Cond() { }
        /// <summary>
        ///  Condition 
        /// </summary>
        /// <param name="mypk"></param>
        public Cond(string mypk)
        {
            this.MyPK = mypk;
            this.Retrieve();
        }
        #endregion

        #region  Public Methods 
        /// <summary>
        ///  This condition can not pass 
        /// </summary>
        public virtual bool IsPassed
        {
            get
            {
                Node nd = new Node(this.FK_Node);
                if (this.en == null)
                {
                    GERpt en = nd.HisFlow.HisGERpt; 
                    try
                    {
                        en.SetValByKey("OID", this.WorkID);
                        en.Retrieve();
                        en.ResetDefaultVal();
                        this.en = en;
                    }
                    catch (Exception ex)
                    {
                        //this.Delete();
                        return false;
                        //throw new Exception("@ Determine the conditions obtaining entity [" + nd.EnDesc + "],  Error :" + ex.Message + "@ Error reason is the definition of the process to determine the error condition , May be your choice to determine the conditions of work working class is the next step in the cause of the current node , To get any instance of this entity .");
                    }
                }

                if (this.HisDataFrom == ConnDataFrom.Stas)
                {
                    string strs = this.OperatorValue.ToString();
                    BP.Port.EmpStations sts = new BP.Port.EmpStations();
                    sts.Retrieve("FK_Emp",BP.Web.WebUser.No);

                    string strs1 = "";
                    foreach (BP.Port.EmpStation st in sts)
                    {
                        if (strs.Contains("@" + st.FK_Station+"@"))
                        {
                            this.MsgOfCond = "@ In order to determine the direction of the post , Conditions true: Post collection " + strs + ", The operator (" + BP.Web.WebUser.No + ") Post :" + st.FK_Station + st.FK_StationT;
                            return true;
                        }
                        strs1 += st.FK_Station + "-" + st.FK_StationT;
                    }

                    this.MsgOfCond = "@ In order to determine the direction of the post , Conditions false: Post collection " + strs + ", The operator (" + BP.Web.WebUser.No + ") Post :" + strs1;
                    return false;
                }

                if (this.HisDataFrom == ConnDataFrom.Depts)
                {
                    string strs = this.OperatorValue.ToString();
                    BP.Port.EmpDepts sts = new BP.Port.EmpDepts();
                    sts.Retrieve("FK_Emp", BP.Web.WebUser.No);
                    string strs1 = "";
                    foreach (BP.Port.EmpDept st in sts)
                    {
                        if (strs.Contains("@" + st.FK_Dept + "@"))
                        {
                            this.MsgOfCond = "@ In order to determine the direction of the post , Conditions true: Collection department " + strs + ", The operator (" + BP.Web.WebUser.No + ") Department :" + st.FK_Dept + st.FK_DeptT;
                            return true;
                        }
                        strs1 += st.FK_Dept + "-" + st.FK_DeptT;
                    }

                    this.MsgOfCond = "@ In order to determine the direction of department , Conditions false: Collection department " + strs + ", The operator (" + BP.Web.WebUser.No + ") Department :" + strs1;
                    return false;
                }

                if (this.HisDataFrom == ConnDataFrom.SQL)
                {
                    //this.MsgOfCond = "@ Direction to form value judgments ,值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ")  Operator :(" + this.FK_Operator + ")  Judgment value :(" + this.OperatorValue.ToString() + ")";
                    string sql = this.OperatorValueStr;
                    sql = sql.Replace("~", "'");
                    sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                    sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                    if (sql.Contains("@") == true)
                    {
                        /*  If you include  @ */
                        foreach (Attr attr in this.en.EnMap.Attrs)
                        {
                            sql = sql.Replace("@" + attr.Key, en.GetValStrByKey(attr.Key));
                        }
                    }

                    int result = DBAccess.RunSQLReturnValInt(sql, -1);
                    if (result == 0)
                        return false;

                    if (result >= 1)
                        return true;

                    throw new Exception("@ You set sql The return value , Does not comply ccflow Requirements , Must be 0 Or greater than or equal 1.");
                }

                if (this.HisDataFrom == ConnDataFrom.Url)
                {
                    string url = this.OperatorValueStr;
                    if (url.Contains("?") == false)
                        url = url + "?1=2";

                    url = Glo.DealExp(url, this.en, "");

                    #region  Add the necessary parameters .
                    if (url.Contains("FK_Flow") == false)
                        url += "&FK_Flow=" + this.FK_Flow;
                    if (url.Contains("FK_Node") == false)
                        url += "&FK_Node=" + this.FK_Node;

                    if (url.Contains("WorkID") == false)
                        url += "&WorkID=" + this.WorkID;
                    if (url.Contains("FID") == false)
                        url += "&FID=" + this.en.FID;

                    if (url.Contains("SID") == false)
                        url += "&SID=" + BP.Web.WebUser.SID;
                    if (url.Contains("UserNo") == false)
                        url += "&UserNo=" + BP.Web.WebUser.No;
                    #endregion  Add the necessary parameters .

                    #region 对url Processing .
                    if (SystemConfig.IsBSsystem)
                    {
                        /*是bs System , And is url Parameters execution type .*/
                        string myurl = BP.Sys.Glo.Request.RawUrl;
                        if (myurl.IndexOf('?') != -1)
                            myurl = myurl.Substring(url.IndexOf('?'));
                        string[] paras = myurl.Split('&');
                        foreach (string s in paras)
                        {
                            if (url.Contains(s))
                                continue;
                            url += "&" + s;
                        }
                        url = url.Replace("&?", "&");
                    }

                    // Replace special variable .
                    url = url.Replace("&?", "&");

                    if (SystemConfig.IsBSsystem == false)
                    {
                        /*非bs Call mode , For example, in cs Call it mode , It takes less than a parameter . */
                    }

                    if (url.Contains("http") == false)
                    {
                        /* If there is no absolute path  */
                        if (SystemConfig.IsBSsystem)
                        {
                            /*在cs Automatic acquisition mode */
                            string host = BP.Sys.Glo.Request.Url.Host;
                            if (url.Contains("@AppPath"))
                                url = url.Replace("@AppPath", "http://" + host + BP.Sys.Glo.Request.ApplicationPath);
                            else
                                url = "http://" + BP.Sys.Glo.Request.Url.Authority + url;
                        }

                        if (SystemConfig.IsBSsystem == false)
                        {
                            /*在cs Its mode baseurl 从web.config Get .*/
                            string cfgBaseUrl = SystemConfig.AppSettings["BaseUrl"];
                            if (string.IsNullOrEmpty(cfgBaseUrl))
                            {
                                string err = " Calling url Failure : Not web.config Configure BaseUrl, Resulting in url Event can not be executed .";
                                Log.DefaultLogWriteLineError(err);
                                throw new Exception(err);
                            }
                            url = cfgBaseUrl + url;
                        }
                    }
                    #endregion 对url Processing .

                    #region 求url Value 
                    try
                    {
                        url = url.Replace("'", "");
                        url = url.Replace("//", "/");
                        url = url.Replace("//", "/");

                        System.Text.Encoding encode = System.Text.Encoding.GetEncoding("gb2312");
                        string text = DataType.ReadURLContext(url, 8000, encode);
                        if (text == null)
                            throw new Exception("@ Orientation process design error conditions , Execution URL Error :" + url + ",  Returns null,  Check the settings are correct .");

                        if (string.IsNullOrEmpty(text) ==true)
                            throw new Exception("@ Error , Does not receive the return value .");

                        if (DataType.IsNumStr(text)==false)
                            throw new Exception("@ Error , Does not comply with the agreed format , Must be a numeric type .");

                        float f = float.Parse(text);
                        if (f > 0)
                            return true;
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Judge url Direction of the error :" + ex.Message+", Carried out url Error :"+url);
                    }
                    #endregion 对url Processing .
                }

                if (this.HisDataFrom == ConnDataFrom.Paras)
                {
                    //this.MsgOfCond = "@ Direction to form value judgments ,值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ")  Operator :(" + this.FK_Operator + ")  Judgment value :(" + this.OperatorValue.ToString() + ")";
                    string exp = this.OperatorValueStr;
                    string[] strs = exp.Trim().Split(' ');

                    string key = strs[0].Trim();
                    string oper = strs[1].Trim();
                    string val = strs[2].Trim();
                    val = val.Replace("'", "");
                    val = val.Replace("%", "");
                    val = val.Replace("~", "");

                    BP.En.Row row = this.en.Row;

                    string valPara = null;
                    if ( row.ContainsKey(key) == false)
                    {
                        try
                        {
                            /* If the key is not included in the specified key,  Went to look for common variable . */
                            if (Glo.SendHTOfTemp.ContainsKey(key) == false)
                                throw new Exception("@ When the judgment condition error , Please confirm whether the parameter misspellings , No corresponding expression found :" + exp + " Key=(" + key + ") oper=(" + oper + ")Val=(" + val + ")");
                            valPara = Glo.SendHTOfTemp[key].ToString().Trim();
                        }
                        catch
                        {
                            // There may be a constant . 
                            valPara = key;
                        }
                    }
                    else
                    {
                        valPara = row[key].ToString().Trim();
                    }

                    #region  Begin to judge .
                    if (oper == "=")
                    {
                        if (valPara == val)
                            return true;
                        else
                            return false;
                    }

                    if (oper.ToUpper() == "LIKE")
                    {
                        if (valPara.Contains(val))
                            return true;
                        else
                            return false;
                    }

                    if (oper == ">")
                    {
                        if (float.Parse(valPara) > float.Parse(val))
                            return true;
                        else
                            return false;
                    }
                    if (oper == ">=")
                    {
                        if (float.Parse(valPara) >= float.Parse(val))
                            return true;
                        else
                            return false;
                    }
                    if (oper == "<")
                    {
                        if (float.Parse(valPara) < float.Parse(val))
                            return true;
                        else
                            return false;
                    }
                    if (oper == "<=")
                    {
                        if (float.Parse(valPara) <= float.Parse(val))
                            return true;
                        else
                            return false;
                    }

                    if (oper == "!=")
                    {
                        if (float.Parse(valPara) != float.Parse(val))
                            return true;
                        else
                            return false;
                    }
                    throw new Exception("@ Parameter format error :" + exp + " Key=" + key + " oper=" + oper + " Val=" + val);
                    #endregion  Begin to judge .

                    // throw new Exception("@ When the judgment condition error , No corresponding expression found :" + exp + " Key=(" + key + ") oper=(" + oper + ")Val=(" + val+")");
                }

                try
                {
                    if (en.EnMap.Attrs.Contains(this.AttrKey) == false)
                        throw new Exception(" Determine the direction of the error condition : Entity :" + nd.EnDesc + "  Property " + this.AttrKey + " Does not exist .");

                    this.MsgOfCond = "@ Direction to form value judgments ,值 " + en.EnDesc + "." + this.AttrKey + " (" + en.GetValStringByKey(this.AttrKey) + ")  Operator :(" + this.FK_Operator + ")  Judgment value :(" + this.OperatorValue.ToString() + ")";

                    switch (this.FK_Operator.Trim().ToLower())
                    {
                        case "<>":
                            if (en.GetValStringByKey(this.AttrKey) != this.OperatorValue.ToString())
                                return true;
                            else
                                return false;
                        case "=":  //  In the case of  = 
                            if (en.GetValStringByKey(this.AttrKey) == this.OperatorValue.ToString())
                                return true;
                            else
                                return false;
                        case ">":
                            if (en.GetValDoubleByKey(this.AttrKey) > Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case ">=":
                            if (en.GetValDoubleByKey(this.AttrKey) >= Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "<":
                            if (en.GetValDoubleByKey(this.AttrKey) < Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "<=":
                            if (en.GetValDoubleByKey(this.AttrKey) <= Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "!=":
                            if (en.GetValDoubleByKey(this.AttrKey) != Double.Parse(this.OperatorValue.ToString()))
                                return true;
                            else
                                return false;
                        case "like":
                            if (en.GetValStringByKey(this.AttrKey).IndexOf(this.OperatorValue.ToString()) == -1)
                                return false;
                            else
                                return true;
                        default:
                            throw new Exception("@ Operation Symbol not found (" + this.FK_Operator.Trim().ToLower() + ").");
                    }
                }
                catch (Exception ex)
                {
                    Node nd23 = new Node(this.NodeID);
                    throw new Exception("@ Determine the conditions :Node=[" + this.NodeID + "," + nd23.EnDesc + "],  Error .@" + ex.Message + ". There may be conditions you set up an illegal way to judge .");
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
                Map map = new Map("WF_Cond");
                map.EnDesc = " Process conditions ";

                map.AddMyPK();
                map.AddTBInt(CondAttr.CondType, 0, " Condition Type ", true, true);
                //map.AddDDLSysEnum(CondAttr.CondType, 0, " Condition Type ", true, false, CondAttr.CondType,"@0= Node closing conditions @1= Process closing conditions @2= Direction of the condition ");

                map.AddTBInt(CondAttr.DataFrom, 0, " Conditions Data Sources 0 Form ,1 Post ( Conditions for effective direction )", true, true);
                map.AddTBString(CondAttr.FK_Flow, null, " Process ", true, true, 0, 60, 20);
                map.AddTBInt(CondAttr.NodeID, 0, " Events MainNode", true, true);
                map.AddTBInt(CondAttr.FK_Node, 0, " Node ID", true, true);
                map.AddTBString(CondAttr.FK_Attr, null, " Property ", true, true, 0, 80, 20);
                map.AddTBString(CondAttr.AttrKey, null, " Attribute key ", true, true, 0, 60, 20);
                map.AddTBString(CondAttr.AttrName, null, " Chinese Name ", true, true, 0, 500, 20);
                map.AddTBString(CondAttr.FK_Operator, "=", " Operators ", true, true, 0, 60, 20);
                map.AddTBString(CondAttr.OperatorValue, "", " Value to operations ", true, true, 0, 4000, 20);
                map.AddTBString(CondAttr.OperatorValueT, "", " Value to operations T", true, true, 0, 4000, 20);
                map.AddTBInt(CondAttr.ToNodeID, 0, "ToNodeID（ Conditions for effective direction ）", true, true);
                map.AddDDLSysEnum(CondAttr.ConnJudgeWay, 0, " Conditions relationship ", true, false,
                    CondAttr.ConnJudgeWay, "@0=or@1=and");

                map.AddTBInt(CondAttr.MyPOID, 0, "MyPOID", true, true);
                map.AddTBInt(CondAttr.PRI, 0, " Calculation priority ", true, true);

                map.AddTBInt(CondAttr.CondOrAnd, 0, " Direction of the condition type ", true, true);


          //      map.AddDDLSysEnum(NodeAttr.CondOrAnd, 0, " Direction of the condition type ",
          //true, true, NodeAttr.CondOrAnd, "@0=And( Set of conditions have been established in all )@1=Or( Only one set of conditions established )");


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Condition s
    /// </summary>
    public class Conds : Entities
    {
        #region  Property 
        /// <summary>
        ///  Get Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get { return new Cond(); }
        }
        /// <summary>
        ///  All the conditions in which there is not in line with .
        /// </summary>
        public bool IsAllPassed
        {
            get
            {
                if (this.Count == 0)
                    throw new Exception("@ Do not want to judge the set .");

                foreach (Cond en in this)
                {
                    if (en.IsPassed == false)
                        return false;
                }
                return true;
            }
        }
        public CondOrAnd CondOrAnd
        {
            get
            {
                foreach (Cond item in this)
                    return item.CondOrAnd;

                return CondOrAnd.ByAnd;
            }
        }
        /// <summary>
        ///  Whether through 
        /// </summary>
        public bool IsPass
        {
            get
            {
                if (this.CondOrAnd == CondOrAnd.ByAnd)
                    return this.IsPassAnd;
                else
                    return this.IsPassOr;
            }
        }
        /// <summary>
        ///  Whether through   
        /// </summary>
        public bool IsPassAnd
        {
            get
            {
                //  Judge   and.  Relationship .
                foreach (Cond en in this)
                {
                    if (en.IsPassed == false)
                        return false;
                }
                return true;
            }
        }
        public bool IsPassOr
        {
            get
            {
                //  Judge   and.  Relationship .
                foreach (Cond en in this)
                {
                    if (en.IsPassed == true)
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        ///  Description 
        /// </summary>
        public string MsgOfDesc
        {
            get
            {
                string msg = "";
                foreach (Cond c in this)
                {
                    msg += "@" + c.MsgOfCond;
                }
                return msg;
            }
        }
        /// <summary>
        ///  Is not one of passed. 
        /// </summary>
        public bool IsOneOfCondPassed
        {
            get
            {
                foreach (Cond en in this)
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
        public Cond GetOneOfCondPassed
        {
            get
            {
                foreach (Cond en in this)
                {
                    if (en.IsPassed == true)
                        return en;
                }
                throw new Exception("@ No closing conditions .");
            }
        }
        public int NodeID = 0;
        #endregion

        #region  Structure 
        /// <summary>
        ///  Condition 
        /// </summary>
        public Conds()
        {
        }
        /// <summary>
        ///  Condition 
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        public Conds(string fk_flow)
        {
            this.Retrieve(CondAttr.FK_Flow, fk_flow);
        }
        /// <summary>
        ///  Condition 
        /// </summary>
        /// <param name="ct"> Type </param>
        /// <param name="nodeID"> Node </param>
        public Conds(CondType ct, int nodeID, Int64 workid,GERpt enData)
        {
            this.NodeID = nodeID;
            this.Retrieve(CondAttr.NodeID, nodeID, CondAttr.CondType, (int)ct, CondAttr.PRI);
            foreach (Cond en in this)
            {
                en.WorkID = workid;
                en.en = enData;
            }
        }

        public string ConditionDesc
        {
            get
            {
                return "";
            }
        }
        #endregion
    }
}
