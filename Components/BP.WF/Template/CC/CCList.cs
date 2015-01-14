using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.WF.Template
{
	/// <summary>
	///  Cc   Property 
	/// </summary>
    public class CCListAttr : EntityMyPKAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Title 
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        ///  Cc content 
        /// </summary>
        public const string Doc = "Doc";
        /// <summary>
        ///  CC node 
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  From node 
        /// </summary>
        public const string NDFrom = "NDFrom";
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        public const string FlowName = "FlowName";
        public const string NodeName = "NodeName";
        /// <summary>
        ///  Whether read 
        /// </summary>
        public const string Sta = "Sta";
        public const string WorkID = "WorkID";
        public const string FID = "FID";
        /// <summary>
        ///  Copied to 
        /// </summary>
        public const string CCTo = "CCTo";
        /// <summary>
        ///  PERSON 
        /// </summary>
        public const string CCToName = "CCToName";
        /// <summary>
        ///  Audit opinion 
        /// </summary>
        public const string CheckNote = "CheckNote";
        /// <summary>
        ///  Review time 
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string Rec = "Rec";
        /// <summary>
        /// RDT
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Parent process ID
        /// </summary>
        public const string PWorkID = "PWorkID";
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public const string PFlowNo = "PFlowNo";
        /// <summary>
        ///  Priority 
        /// </summary>
        public const string PRI = "PRI";
        #endregion
    }
    public enum CCSta
    {
        /// <summary>
        ///  Unread 
        /// </summary>
        UnRead,
        /// <summary>
        ///  Has been read 
        /// </summary>
        Read,
        /// <summary>
        ///  Had returned 
        /// </summary>
        CheckOver,
        /// <summary>
        ///  Deleted 
        /// </summary>
        Del
    }
	/// <summary>
	///  Cc 
	/// </summary>
    public class CCList : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Status 
        /// </summary>
        public CCSta HisSta
        {
            get
            {
                return (CCSta)this.GetValIntByKey(CCListAttr.Sta);
            }
            set
            {
                if (value == CCSta.Read)
                    this.CDT = DataType.CurrentDataTime;
                this.SetValByKey(CCListAttr.Sta, (int)value);
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
                if (BP.Web.WebUser.No != "admin")
                {
                    uac.IsView = false;
                    return uac;
                }
                uac.IsDelete = false;
                uac.IsInsert = false;
                uac.IsUpdate = true;
                return uac;
            }
        }
        public string CCTo
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.CCTo);
            }
            set
            {
                this.SetValByKey(CCListAttr.CCTo, value);
            }
        }
        /// <summary>
        ///  Copied to Name
        /// </summary>
        public string CCToName
        {
            get
            {
                string s= this.GetValStringByKey(CCListAttr.CCToName);
                if (string.IsNullOrEmpty(s))
                    s=this.CCTo;
                return s;
            }
            set
            {
                this.SetValByKey(CCListAttr.CCToName, value);
            }
        }

        /// <summary>
        ///  Audit opinion 
        /// </summary>
        public string CheckNote
        {
            get
            {
                string s = this.GetValStringByKey(CCListAttr.CheckNote);
                if (string.IsNullOrEmpty(s))
                    return "нч";
                return s;
            }
            set
            {
                this.SetValByKey(CCListAttr.CheckNote, value);
            }
        }
        /// <summary>
        ///  Audit opinion 
        /// </summary>
        public string CheckNoteHtml
        {
            get
            {
                string s = this.GetValStringByKey(CCListAttr.CheckNote);
                if (string.IsNullOrEmpty(s))
                    return "нч";
                return DataType.ParseText2Html(s);
            }
        }
        /// <summary>
        ///  Read Time 
        /// </summary>
        public string CDT
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.CDT);
            }
            set
            {
                this.SetValByKey(CCListAttr.CDT, value);
            }
        }
        /// <summary>
        ///  Cc where the node number 
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(CCListAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(CCListAttr.FK_Node, value);
            }
        }
        public int NDFrom
        {
            get
            {
                return this.GetValIntByKey(CCListAttr.NDFrom);
            }
            set
            {
                this.SetValByKey(CCListAttr.NDFrom, value);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(CCListAttr.WorkID);
            }
            set
            {
                this.SetValByKey(CCListAttr.WorkID, value);
            }
        }
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(CCListAttr.FID);
            }
            set
            {
                this.SetValByKey(CCListAttr.FID, value);
            }
        }
        /// <summary>
        ///  Parent Process Work ID
        /// </summary>
        public Int64 PWorkID
        {
            get
            {
                return this.GetValInt64ByKey(CCListAttr.PWorkID);
            }
            set
            {
                this.SetValByKey(CCListAttr.PWorkID, value);
            }
        }
        /// <summary>
        ///  Parent process ID 
        /// </summary>
        public string PFlowNo
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.PFlowNo);
            }
            set
            {
                this.SetValByKey(CCListAttr.PFlowNo, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_FlowT
        {
            get
            {
                return this.GetValRefTextByKey(CCListAttr.FK_Flow);
            }
        }
        public string FlowName
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.FlowName);
            }
            set
            {
                this.SetValByKey(CCListAttr.FlowName, value);
            }
        }
        public string NodeName
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.NodeName);
            }
            set
            {
                this.SetValByKey(CCListAttr.NodeName, value);
            }
        }
        /// <summary>
        ///  CC title 
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.Title);
            }
            set
            {
                this.SetValByKey(CCListAttr.Title, value);
            }
        }
        /// <summary>
        ///  Cc content 
        /// </summary>
        public string Doc
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.Doc);
            }
            set
            {
                this.SetValByKey(CCListAttr.Doc, value);
            }
        }
        public string DocHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(CCListAttr.Doc);
            }
        }
        /// <summary>
        ///  CC object 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(CCListAttr.FK_Flow, value);
            }
        }
        public string Rec
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.Rec);
            }
            set
            {
                this.SetValByKey(CCListAttr.Rec, value);
            }
        }
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(CCListAttr.RDT);
            }
            set
            {
                this.SetValByKey(CCListAttr.RDT, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// CCList
        /// </summary>
        public CCList()
        {
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
                Map map = new Map("WF_CCList");
                map.EnDesc = " CC list ";
                map.EnType = EnType.Admin;
                map.AddMyPK();
                map.AddTBString(CCListAttr.FK_Flow, null, " Process ID ", true, true, 0, 500, 10, true);
                map.AddTBString(CCListAttr.FlowName, null, " Process Name ", true, true, 0, 500, 10, true);
                map.AddTBInt(CCListAttr.NDFrom, 0, " From node ", true, true);

                map.AddTBInt(CCListAttr.FK_Node, 0, " Node ", true, true);
                map.AddTBString(CCListAttr.NodeName, null, " Node Name ", true, true, 0, 500, 10, true);
                map.AddTBInt(CCListAttr.WorkID, 0, " The work ID", true, true);
                map.AddTBInt(CCListAttr.FID, 0, "FID", true, true);

                map.AddTBString(CCListAttr.Title, null, " Title ", true, true, 0, 500, 10, true);
                map.AddTBStringDoc();

                map.AddTBString(CCListAttr.Rec, null, " Record people ", true, true, 0, 200, 10, true);
                map.AddTBString(CCListAttr.RDT, null, " Record Date ", true, true, 0, 500, 10, true);
                map.AddTBInt(CCListAttr.Sta, 0, " Status ", true, true);

                map.AddTBString(CCListAttr.CCTo, null, " Copied to ", true, false, 0, 200, 10, true);
                map.AddTBString(CCListAttr.CCToName, null, " Copied to ( PERSON )", true, false, 0, 200, 10, true);
                map.AddTBString(CCListAttr.CheckNote, null, " Audit opinion ", true, false, 0, 600, 10, true);
                map.AddTBString(CCListAttr.CDT, null, " Open time ", true, true, 0, 500, 10, true);


                map.AddTBString(CCListAttr.PFlowNo, null, " Parent process ID ", true, true, 0, 100, 10, true);
                map.AddTBInt(CCListAttr.PWorkID, 0, " Parent process WorkID", true, true);
                 
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
	/// <summary>
	///  Cc 
	/// </summary>
	public class CCLists: EntitiesMyPK
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new CCList();
			}
		}
		/// <summary>
        ///  Cc 
		/// </summary>
		public CCLists(){}


        /// <summary>
        ///  Check out all the CC information 
        /// </summary>
        /// <param name="flowNo"></param>
        /// <param name="workid"></param>
        /// <param name="fid"></param>
        public CCLists(string flowNo, Int64 workid, Int64 fid)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(CCListAttr.FK_Flow, flowNo);
            qo.addAnd();
            if (fid != 0)
                qo.AddWhereIn(CCListAttr.WorkID, "(" + workid + "," + fid + ")");
            else
                qo.AddWhere(CCListAttr.WorkID, workid);
            qo.DoQuery();
        } 		 
		#endregion
	}
}
