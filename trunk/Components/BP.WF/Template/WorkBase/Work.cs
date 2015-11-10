using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.Port;
using System.Security.Cryptography;
using System.Text;
using BP.WF.XML;

namespace BP.WF
{
    /// <summary>
    ///  System agreed list of fields 
    /// </summary>
    public class WorkSysFieldAttr
    {
        /// <summary>
        ///  Send personnel field 
        ///  Used to determine when the node sending the person receiving the next node ,  Similar to send a message to select recipients .
        ///  And the next node attributes   Select Access Rules ¡¾ By Form SysSendEmps Field calculations ¡¿ Effective .
        /// </summary>
        public const string SysSendEmps = "SysSendEmps";
        /// <summary>
        ///  Cc field staff 
        ///  When the current job requires Cc ,  Need to form the current node , Increasing this field .
        ///  Rules node and select Properties in the Cc ¡¾ By Form SysCCEmps Field calculations ¡¿ Effective .
        ///  If there are multiple operators , Accept the value of a field separated by commas . Such as : zhangsan,lisi,wangwu
        /// </summary>
        public const string SysCCEmps = "SysCCEmps";
        /// <summary>
        ///  The process should be completed by the date 
        ///  Explanation : Increasing this field at the beginning of the nodes form , This process should be used to mark the date of completion .
        ///  Will send the user after this value is recorded in WF_GenerWorkFlow µÄ SDTOfFlow ÖÐ.
        ///  This field is displayed in the to-do , Launch , In transit , Delete , Pending list .
        /// </summary>
        public const string SysSDTOfFlow = "SysSDTOfFlow";
        /// <summary>
        ///  Node should finish time 
        ///  Explanation : Increasing this field at the beginning of the nodes form , Mark next to this node should be completed date .
        /// </summary>
        public const string SysSDTOfNode = "SysSDTOfNode";
        /// <summary>
        /// PWorkID  Calling 
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        /// FromNode
        /// </summary>
        public const string FromNode = "FromNode";
        /// <summary>
        ///  The need for a read receipt 
        /// </summary>
        public const string SysIsReadReceipts = "SysIsReadReceipts";

        #region  Quality assessment and related fields 
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string EvalEmpNo = "EvalEmpNo";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string EvalEmpName = "EvalEmpName";
        /// <summary>
        ///  Scores 
        /// </summary>
        public const string EvalCent = "EvalCent";
        /// <summary>
        ///  Content 
        /// </summary>
        public const string EvalNote = "EvalNote";
        #endregion  Quality assessment and related fields 
    }
    /// <summary>
    ///  Working Properties 
    /// </summary>
    public class WorkAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string OID = "OID";
        /// <summary>
        ///  Time to complete the task 
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        ///  Record Time 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string Rec = "Rec";
        /// <summary>
        ///  Record people Text
        /// </summary>
        public const string RecText = "RecText";
        /// <summary>
        /// Emps
        /// </summary>
        public const string Emps = "Emps";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// MyNum
        /// </summary>
        public const string MyNum = "MyNum";
        /// <summary>
        /// MD5
        /// </summary>
        public const string MD5 = "MD5";
        #endregion
    }
    /// <summary>
    /// WorkBase  The summary .
    ///  The work 
    /// </summary>
    abstract public class Work : Entity
    {
        /// <summary>
        ///  Inspection MD5 Value is through 
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsPassCheckMD5()
        {
            string md51 = this.GetValStringByKey(WorkAttr.MD5);
            string md52 = Glo.GenerMD5(this);
            if (md51 != md52)
                return false;
            return true;
        }

        #region  Basic properties ( Required attributes )
        public override string PK
        {
            get
            {
                return "OID";
            }
        }
        /// <summary>
        /// classID
        /// </summary>
        public override string ClassID
        {
            get
            {
                return "ND"+this.HisNode.NodeID;
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public virtual Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(WorkAttr.FID);
            }
            set
            {
                this.SetValByKey(WorkAttr.FID, value);
            }
        }
        /// <summary>
        /// workid, If it is empty on return  0 . 
        /// </summary>
        public virtual Int64 OID
        {
            get
            {
                return this.GetValInt64ByKey(WorkAttr.OID);
            }
            set
            {
                this.SetValByKey(WorkAttr.OID, value);
            }
        }
        /// <summary>
        ///  Completion Time 
        /// </summary>
        public string CDT
        {
            get
            {
                string str = this.GetValStringByKey(WorkAttr.CDT);
                if (str.Length < 5)
                    this.SetValByKey(WorkAttr.CDT, DataType.CurrentDataTime);

                return this.GetValStringByKey(WorkAttr.CDT);
            }
        }
        public string Emps
        {
            get
            {
                return this.GetValStringByKey(WorkAttr.Emps);
            }
            set
            {
                this.SetValByKey(WorkAttr.Emps, value);
            }
        }
        public override int RetrieveFromDBSources()
        {
            try
            {
                return base.RetrieveFromDBSources();
            }
            catch (Exception ex)
            {
                this.CheckPhysicsTable();
                throw ex;
            }
        }
        public int RetrieveFID()
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhereIn(WorkAttr.OID, "(" + this.FID + "," + this.OID + ")");
            int i = qo.DoQuery();
            if (i == 0)
            {
                if (SystemConfig.IsDebug == false)
                {
                    this.CheckPhysicsTable();
                    throw new Exception("@ Node [" + this.EnDesc + "] Data Loss :WorkID=" + this.OID + " FID=" + this.FID + " sql=" + qo.SQL);
                }
            }
            return i;
        }
        public override int Retrieve()
        {
            try
            {
                return base.Retrieve();
            }
            catch (Exception ex)
            {
                this.CheckPhysicsTable();
                throw ex;
            }
        }
        /// <summary>
        ///  Record Time 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(WorkAttr.RDT);
            }
        }
        public string RDT_Date
        {
            get
            {
                try
                {
                    return DataType.ParseSysDate2DateTime(this.RDT).ToString(DataType.SysDataFormat);
                }
                catch
                {
                    return DataType.CurrentData;
                }
            }
        }
        public DateTime RDT_DateTime
        {
            get
            {
                try
                {
                    return DataType.ParseSysDate2DateTime(this.RDT_Date);
                }
                catch
                {
                    return DateTime.Now;
                }
            }
        }
        public string Record_FK_NY
        {
            get
            {
                return this.RDT.Substring(0, 7);
            }
        }
        /// <summary>
        ///  Record people 
        /// </summary>
        public string Rec
        {
            get
            {
                string str = this.GetValStringByKey(WorkAttr.Rec);
                if (str == "")
                    this.SetValByKey(WorkAttr.Rec, BP.Web.WebUser.No);

                return this.GetValStringByKey(WorkAttr.Rec);
            }
            set
            {
                this.SetValByKey(WorkAttr.Rec, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public Emp RecOfEmp
        {
            get
            {
                return new Emp(this.Rec);
            }
        }
        /// <summary>
        ///  Name of recording 
        /// </summary>
        public string RecText
        {
            get
            {
                try
                {
                    return this.HisRec.Name;
                }
                catch
                {
                    return this.Rec;
                }
            }
            set
            {
                this.SetValByKey("RecText", value);
            }
        }
        /// <summary>
        ///  Delay of several days   
        /// </summary>
        public float DelayDays
        {
            get
            {
                if (this.SpanDays == 0)
                    return 0;
                float days = this.SpanDays - this.HisNode.DeductDays;
                if (days < 0)
                    return 0;
                return days;
            }
        }
        private Node _HisNode = null;
        /// <summary>
        ///  Node work .
        /// </summary>
        public Node HisNode
        {
            get
            {
                if (this._HisNode == null)
                {
                    this._HisNode = new Node(this.NodeID);
                }
                return _HisNode;
            }
            set
            {
                _HisNode = value;
            }
        }
        /// <summary>
        ///  From Table .
        /// </summary>
        public MapDtls HisMapDtls
        {
            get
            {
                return this.HisNode.MapData.MapDtls;
            }
        }
        /// <summary>
        ///  From Table .
        /// </summary>
        public FrmAttachments HisFrmAttachments
        {
            get
            {
                return this.HisNode.MapData.FrmAttachments;
            }
        }
        #endregion

        #region  Extended Attributes 
        /// <summary>
        ///  The span of a few days 
        /// </summary>
        public int SpanDays
        {
            get
            {
                if (this.CDT == this.RDT)
                    return 0;
                return DataType.SpanDays(this.RDT, this.CDT);
            }
        }
        /// <summary>
        ///  Obtained from the work completed to the present date 
        /// </summary>
        /// <returns></returns>
        public int GetCDTSpanDays(string todata)
        {
            return DataType.SpanDays(this.CDT, todata);
        }
        /// <summary>
        ///  His record people 
        /// </summary>
        public Emp HisRec
        {
            get
            {
              //  return new Emp(this.Rec);
                Emp emp = this.GetValByKey("HisRec"+this.Rec) as Emp;
                if (emp == null)
                {
                    emp = new Emp(this.Rec);
                    this.SetValByKey("HisRec" + this.Rec, emp);
                }
                return emp;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  The work 
        /// </summary>
        protected Work()
        {
           
        }
        /// <summary>
        ///  The work 
        /// </summary>
        /// <param name="oid">WFOID</param>		 
        protected Work(Int64 oid)
        {
            this.SetValByKey(EntityOIDAttr.OID, oid);
            this.Retrieve();
        }
        #endregion

        #region Node.xml  To configure the .
        /// <summary>
        ///  Produce this work all foreign key parameters 
        ///  Additional necessary attributes .
        /// </summary>
        /// <returns></returns>
        private string GenerParas_del()
        {
            string paras = "*WorkID" + this.OID + "*UserNo=" + this.Rec ;
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.Normal)
                    continue;

                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.MyFieldType == FieldType.NormalVirtual)
                    continue;

                if (attr.Key == WorkAttr.Rec
                    || attr.Key == "OID"
                    )
                    continue;

                paras += "*" + attr.Key + "=" + this.GetValStringByKey(attr.Key);
            }
            return paras;
        }
        public virtual string WorkEndInfo
        {
            get
            {
                string tp = "";
                //FAppSets sets = new FAppSets(this.NodeID);
                //foreach (FAppSet set in sets)
                //{
                //    if (set.DoWhat.Contains("?"))
                //        tp += "[<a href=\"javascript:WinOpen('" + set.DoWhat + "&WorkID=" + this.OID + "' ,'sd');\" ><img src='/WF/Img/Btn/Do.gif' border=0/>" + set.Name + "</a>]";
                //    else
                //        tp += "[<a href=\"javascript:WinOpen('" + set.DoWhat + "?WorkID=" + this.OID + "' ,'sd');\" ><img src='/WF/Img/Btn/Do.gif' border=0/>" + set.Name + "</a>]";
                //}
                if (this.HisNode.IsHaveSubFlow)
                {
                    NodeFlows flows = new NodeFlows(this.HisNode.NodeID);
                    foreach (NodeFlow fl in flows)
                    {
                        tp += "[<a href='CallSubFlow.aspx?FID=" + this.OID + "&FK_Flow=" + fl.FK_Flow + "&FK_FlowFrom=" + this.HisNode.FK_Flow + "' ><img src='/WF/Img/Btn/Do.gif' border=0/>" + fl.FK_FlowT + "</a>]";
                    }
                }
                if (tp.Length > 0)
                    return "<div align=left>" + tp + "</div>";
                return tp;
            }
        }
        /// <summary>
        ///  Produce to be executed url.
        /// </summary>
        public string GenerNextUrl()
        {
            string appName = BP.Sys.Glo.Request.ApplicationPath;
            string ip = SystemConfig.AppSettings["CIP"];
            if (ip == null || ip == "")
                throw new Exception("@ You do not set CIP");
            return "http://" + ip + "/" + appName + "/WF/Port.aspx?UserNo=" + BP.Web.WebUser.No + "&DoWhat=DoNode&WorkID=" + this.OID + "&FK_Node=" + this.HisNode.NodeID + "&Key=MyKey";
        }
        #endregion

        #region  Need to write a subclass method 
        public void DoAutoFull(Attr attr)
        {
            if (this.OID == 0)
                return;

            if (attr.AutoFullDoc == null || attr.AutoFullDoc.Length == 0)
                return;

            string objval = null;

            //  This code needs to purify the base class .
            switch (attr.AutoFullWay)
            {
                case BP.En.AutoFullWay.Way0:
                    return;
                case BP.En.AutoFullWay.Way1_JS:
                    break;
                case BP.En.AutoFullWay.Way2_SQL:
                    string sql = attr.AutoFullDoc;
                    Attrs attrs1 = this.EnMap.Attrs;
                    foreach (Attr a1 in attrs1)
                    {
                        if (a1.IsNum)
                            sql = sql.Replace("@" + a1.Key, this.GetValStringByKey(a1.Key));
                        else
                            sql = sql.Replace("@" + a1.Key, "'" + this.GetValStringByKey(a1.Key) + "'");
                    }

                    objval = DBAccess.RunSQLReturnString(sql);
                    break;
                case BP.En.AutoFullWay.Way3_FK:
                    try
                    {
                        string sqlfk = "SELECT @Field FROM @Table WHERE No=@AttrKey";
                        string[] strsFK = attr.AutoFullDoc.Split('@');
                        foreach (string str in strsFK)
                        {
                            if (str == null || str.Length == 0)
                                continue;

                            string[] ss = str.Split('=');
                            if (ss[0] == "AttrKey")
                                sqlfk = sqlfk.Replace('@' + ss[0], "'" + this.GetValStringByKey(ss[1]) + "'");
                            else
                                sqlfk = sqlfk.Replace('@' + ss[0], ss[1]);
                        }
                        sqlfk = sqlfk.Replace("''", "'");

                        objval = DBAccess.RunSQLReturnString(sqlfk);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("@ Processing AutoComplete : Foreign key [" + attr.Key + ";" + attr.Desc + "], When an error occurs . Exception Information :" + ex.Message);
                    }
                    break;
                case BP.En.AutoFullWay.Way4_Dtl:
                    string mysql = "SELECT @Way(@Field) FROM @Table WHERE RefPK='"+ this.OID+"'";
                    string[] strs = attr.AutoFullDoc.Split('@');
                    foreach (string str in strs)
                    {
                        if (str == null || str.Length == 0)
                            continue;

                        string[] ss = str.Split('=');
                        mysql = mysql.Replace('@' + ss[0], ss[1]);
                    }
                    objval = DBAccess.RunSQLReturnString(mysql);
                    break;
                default:
                    throw new Exception(" Type is not involved .");
            }
            if (objval == null)
                return;

            if (attr.IsNum)
            {
                try
                {
                    decimal d = decimal.Parse(objval);
                    this.SetValByKey(attr.Key, objval);
                }
                catch
                {
                }
            }
            else
            {
                this.SetValByKey(attr.Key, objval);
            }
            return;
        }
      
        #endregion

        #region   Override the base class methods .
        /// <summary>
        ///  As specified OID Insert.
        /// </summary>
        public void InsertAsOID(Int64 oid)
        {
            this.SetValByKey("OID", oid);
            this.RunSQL(SqlBuilder.Insert(this));
        }
        /// <summary>
        ///  As specified OID  Save 
        /// </summary>
        /// <param name="oid"></param>
        public void SaveAsOID(Int64 oid)
        {
            this.SetValByKey("OID", oid);
            if (this.RetrieveNotSetValues().Rows.Count == 0)
                this.InsertAsOID(oid);
            this.Update();
        }
        /// <summary>
        ///  Save entity information 
        /// </summary>
        public new int Save()
        {
            if (this.OID <= 10)
                throw new Exception("@ Did not give WorkID Assignment , You can not save .");
            if (this.Update() == 0)
            {
                this.InsertAsOID(this.OID);
                return 0;
            }
            return 1;
        }
        public override void Copy(DataRow dr)  {
            foreach (Attr attr in this.EnMap.Attrs)
            {
                if (attr.Key == WorkAttr.CDT
                   || attr.Key == WorkAttr.RDT
                   || attr.Key == WorkAttr.Rec
                   || attr.Key == WorkAttr.FID
                   || attr.Key == WorkAttr.OID
                   || attr.Key == "No"
                   || attr.Key == "Name")
                    continue;

                try
                {
                    this.SetValByKey(attr.Key, dr[attr.Key]);
                }
                catch
                {
                }
            }
        }
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
                    || attr.Key == "No"
                    || attr.Key == "Name")
                    continue;
                this.SetValByKey(attr.Key, fromEn.GetValByKey(attr.Key));
            }
        }
        /// <summary>
        ///  Delete the main table data should delete its detail 
        /// </summary>
        protected override void afterDelete()
        {
            #warning  Delete the details , May cause other effects .
            //MapDtls dtls = this.HisNode.MapData.MapDtls;
            //foreach (MapDtl dtl in dtls).
            //    DBAccess.RunSQL("DELETE FROM  " + dtl.PTable + " WHERE RefPK=" + this.OID);

            base.afterDelete();
        }
        #endregion

        #region   Public Methods 
        /// <summary>
        ///  Before the update 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeUpdate()
        {
            #region  Special treatment 
            try
            {
                if (this.GetValStrByKey("WFState") == "Runing")
                {
                    this.SetValByKey("WFState", (int)WFState.Runing);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            return base.beforeUpdate();
        }
        /// <summary>
        ///  Work to be done directly before saving 
        /// </summary>
        public virtual void BeforeSave()
        {
            // Automatic calculation .
            this.AutoFull();
            //  Save before execution event .

            this.HisNode.HisFlow.DoFlowEventEntity(EventListOfNode.SaveBefore, this.HisNode, this.HisNode.HisWork, null);
        }
        /// <summary>
        ///  Direct save 
        /// </summary>
        public new void DirectSave()
        {
            this.beforeUpdateInsertAction();
            if (this.DirectUpdate() == 0)
            {
                this.SetValByKey(WorkAttr.RDT, DateTime.Now.ToString("yyyy-MM-dd"));
                this.DirectInsert();
            }
        }
        public string NodeFrmID = "";
        protected int _nodeID = 0;
        public int NodeID
        {
            get
            {
                if (_nodeID == 0)
                    throw new Exception(" You do not have to _Node To value .");
                return this._nodeID;
            }
            set
            {
                if (this._nodeID != value)
                {
                    this._nodeID = value;
                    this._enMap = null;
                }
                this._nodeID = value;
            }
        }
        #endregion
    }
    /// <summary>
    ///  The work   Set 
    /// </summary>
    abstract public class Works : EntitiesOID
    {
        #region  Constructor 
        /// <summary>
        ///  Information collection base class 
        /// </summary>
        public Works()
        {
        }
        #endregion

        #region  Query methods 
        /// <summary>
        ///  Inquiries ( Unsuitable Audit node query )
        /// </summary>
        /// <param name="empId"> Staff </param>
        /// <param name="nodeStat"> Node status </param>
        /// <param name="fromdate"> Date from </param>
        /// <param name="todate"> Record Date to </param>
        /// <returns></returns>
        public int Retrieve(string key, string empId, string fromdate, string todate)
        {
            QueryObject qo = new QueryObject(this);
                qo.AddWhere(WorkAttr.Rec, empId);

            qo.addAnd();
            qo.AddWhere(WorkAttr.RDT, ">=", fromdate);
            qo.addAnd();
            qo.AddWhere(WorkAttr.RDT, "<=", todate);

            if (key.Trim().Length == 0)
                return qo.DoQuery();
            else
            {
                if (key.IndexOf("%") == -1)
                    key = "%" + key + "%";
                Entity en = this.GetNewEntity;
                qo.addAnd();
                qo.addLeftBracket();
                qo.AddWhere(en.PK, " LIKE ", key);
                foreach (Attr attr in en.EnMap.Attrs)
                {
                    if (attr.MyFieldType == FieldType.RefText)
                        continue;
                    if (attr.UIContralType == UIContralType.DDL || attr.UIContralType == UIContralType.CheckBok)
                        continue;
                    qo.addOr();
                    qo.AddWhere(attr.Key, " LIKE ", key);
                }
                qo.addRightBracket();
                return qo.DoQuery();
            }
        }
        public int Retrieve(string fromDataTime, string toDataTime)
        {
            QueryObject qo = new QueryObject(this);
            qo.Top = 90000;
            qo.AddWhere(WorkAttr.RDT, " >=", fromDataTime);
            qo.addAnd();
            qo.AddWhere(WorkAttr.RDT, " <= ", toDataTime);
            return qo.DoQuery();
        }
        #endregion
    }
}
