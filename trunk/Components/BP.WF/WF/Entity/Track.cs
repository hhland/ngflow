using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.WF.Template;

namespace BP.WF
{
    /// <summary>
    ///   Property 
    /// </summary>
    public class TrackAttr : EntityMyPKAttr
    {
        /// <summary>
        ///  Record Date 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  Completion Date 
        /// </summary>
        public const string CDT = "CDT";
        /// <summary>
        /// FID
        /// </summary>
        public const string FID = "FID";
        /// <summary>
        /// WorkID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        /// CWorkID
        /// </summary>
        public const string CWorkID = "CWorkID";
        /// <summary>
        ///  Activity Type 
        /// </summary>
        public const string ActionType = "ActionType";
        /// <summary>
        ///  Activity Type Name 
        /// </summary>
        public const string ActionTypeText = "ActionTypeText";

        /// <summary>
        ///  Time span 
        /// </summary>
        public const string WorkTimeSpan = "WorkTimeSpan";

        /// <summary>
        ///  Node data 
        /// </summary>
        public const string NodeData = "NodeData";

        /// <summary>
        ///  Track field 
        /// </summary>
        public const string TrackFields = "TrackFields";

        /// <summary>
        ///  Remark 
        /// </summary>
        public const string Note = "Note";

        /// <summary>
        ///  From node 
        /// </summary>
        public const string NDFrom = "NDFrom";

        /// <summary>
        ///  To node 
        /// </summary>
        public const string NDTo = "NDTo";

        /// <summary>
        ///  From staff 
        /// </summary>
        public const string EmpFrom = "EmpFrom";

        /// <summary>
        ///  To staff 
        /// </summary>
        public const string EmpTo = "EmpTo";

        /// <summary>
        ///  Check 
        /// </summary>
        public const string Msg = "Msg";

        /// <summary>
        /// EmpFromT
        /// </summary>
        public const string EmpFromT = "EmpFromT";

        /// <summary>
        /// NDFromT
        /// </summary>
        public const string NDFromT = "NDFromT";

        /// <summary>
        /// NDToT
        /// </summary>
        public const string NDToT = "NDToT";
        /// <summary>
        /// EmpToT
        /// </summary>
        public const string EmpToT = "EmpToT";
        /// <summary>
        ///  The actual implementation of staff 
        /// </summary>
        public const string Exer = "Exer";
        /// <summary>
        ///  Parameter information 
        /// </summary>
        public const string Tag = "Tag";
        /// <summary>
        ///  Internal key 
        /// </summary>
        public const string InnerKey_del = "InnerKey";
    }

    /// <summary>
    ///  Locus 
    /// </summary>
    public class Track : BP.En.Entity
    {
        public override string PK
        {
            get
            {
                return "MyPK";
            }
        }

        public override string PKField
        {
            get
            {
                return "MyPK";
            }
        }

        #region attrs

        /// <summary>
        ///  Node from 
        /// </summary>
        public int NDFrom
        {
            get
            {
                return this.GetValIntByKey(TrackAttr.NDFrom);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDFrom, value);
            }
        }

        /// <summary>
        ///  Node 
        /// </summary>
        public int NDTo
        {
            get
            {
                return this.GetValIntByKey(TrackAttr.NDTo);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDTo, value);
            }
        }
        /// <summary>
        ///  From staff 
        /// </summary>
        public string EmpFrom
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpFrom);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpFrom, value);
            }
        }
        ///// <summary>
        /////  Internal PK.
        ///// </summary>
        //public string InnerKey_del
        //{
        //    get
        //    {
        //        return this.GetValStringByKey(TrackAttr.InnerKey);
        //    }
        //    set
        //    {
        //        this.SetValByKey(TrackAttr.InnerKey, value);
        //    }
        //}
        /// <summary>
        ///  To staff 
        /// </summary>
        public string EmpTo
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpTo);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpTo, value);
            }
        }
        public string Tag
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.Tag);
            }
            set
            {
                this.SetValByKey(TrackAttr.Tag, value);
            }
        }
        /// <summary>
        ///  Record Date 
        /// </summary>
        public string RDT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.RDT);
            }
            set
            {
                this.SetValByKey(TrackAttr.RDT, value);
            }
        }

        /// <summary>
        /// fid
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64ByKey(TrackAttr.FID);
            }
            set
            {
                this.SetValByKey(TrackAttr.FID, value);
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(TrackAttr.WorkID);
            }
            set
            {
                this.SetValByKey(TrackAttr.WorkID, value);
            }
        }
        /// <summary>
        /// CWorkID
        /// </summary>
        public Int64 CWorkID
        {
            get
            {
                return this.GetValInt64ByKey(TrackAttr.CWorkID);
            }
            set
            {
                this.SetValByKey(TrackAttr.CWorkID, value);
            }
        }
        /// <summary>
        ///  Activity Type 
        /// </summary>
        public ActionType HisActionType
        {
            get
            {
                return (ActionType)this.GetValIntByKey(TrackAttr.ActionType);
            }
            set
            {
                this.SetValByKey(TrackAttr.ActionType, (int)value);
            }
        }

        /// <summary>
        ///  Get action text 
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public static string GetActionTypeT(ActionType at)
        {
            switch (at)
            {
                case ActionType.Forward:
                    return " Go ahead ";
                case ActionType.Return:
                    return " Return ";
                case ActionType.Shift:
                    return " Transfer ";
                case ActionType.UnShift:
                    return " Undo transfer ";
                case ActionType.Start:
                    return " Launch ";
                case ActionType.UnSend:
                    return " Undo launched ";
                case ActionType.ForwardFL:
                    return " - Go ahead ( Split point )";
                case ActionType.ForwardHL:
                    return " - Send to confluence ";
                case ActionType.FlowOver:
                    return " Process ends ";
                case ActionType.CallChildenFlow:
                    return " Subprocess call ";
                case ActionType.StartChildenFlow:
                    return " Sub-processes initiated ";
                case ActionType.SubFlowForward:
                    return " Forward thread ";
                case ActionType.RebackOverFlow:
                    return " Recovery process has been completed ";
                case ActionType.FlowOverByCoercion:
                    return " Forced End Process ";
                case ActionType.HungUp:
                    return " Pending ";
                case ActionType.UnHungUp:
                    return " Unsuspend ";
                case ActionType.Press:
                    return " Reminders ";
                case ActionType.CC:
                    return " Cc ";
                case ActionType.WorkCheck:
                    return " Check ";
                case ActionType.ForwardAskfor:
                    return " Send endorsement ";
                case ActionType.AskforHelp:
                    return " Plus sign ";
                case ActionType.Skip:
                    return " Jump ";
                case ActionType.Info:
                    return " Information ";
                case ActionType.DeleteFlowByFlag:
                    return " Tombstone ";
                case ActionType.Order:
                    return " Send Queue ";
                default:
                    return " Unknown ";
            }
        }

        public string ActionTypeText
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.ActionTypeText);
            }
            set
            {
                this.SetValByKey(TrackAttr.ActionTypeText, value);
            }
        }

        /// <summary>
        ///  Node data 
        /// </summary>
        public string NodeData
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NodeData);
            }
            set
            {
                this.SetValByKey(TrackAttr.NodeData, value);
            }
        }

        public string Exer
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.Exer);
            }
            set
            {
                this.SetValByKey(TrackAttr.Exer, value);
            }
        }

        /// <summary>
        ///  Audit opinion 
        /// </summary>
        public string Msg
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.Msg);
            }
            set
            {
                this.SetValByKey(TrackAttr.Msg, value);
            }
        }

        public string MsgHtml
        {
            get
            {
                return this.GetValHtmlStringByKey(TrackAttr.Msg);
            }
        }

        public string EmpToT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpToT);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpToT, value);
            }
        }

        public string EmpFromT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.EmpFromT);
            }
            set
            {
                this.SetValByKey(TrackAttr.EmpFromT, value);
            }
        }

        public string NDFromT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NDFromT);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDFromT, value);
            }
        }

        public string NDToT
        {
            get
            {
                return this.GetValStringByKey(TrackAttr.NDToT);
            }
            set
            {
                this.SetValByKey(TrackAttr.NDToT, value);
            }
        }

        #endregion attrs

        #region  Property 

        public string RptName = null;

        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();

                #region  Basic properties 

                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); // To connect to a data source £¨ Indicate that the system you want to connect to the database £©.
                map.PhysicsTable = "WF_Track"; //  To physical table .
                map.EnDesc = " Track list ";
                map.EnType = EnType.App;

                #endregion  Basic properties 

                #region  Field 

                // An automatic increase in the growth column .
                map.AddTBIntPK(TrackAttr.MyPK, 0, "MyPK", true, false);

                map.AddTBInt(TrackAttr.ActionType, 0, " Type ", true, false);
                map.AddTBString(TrackAttr.ActionTypeText, null, " Type ( Name )", true, false, 0, 30, 100);
                map.AddTBInt(TrackAttr.FID, 0, " Process ID", true, false);
                map.AddTBInt(TrackAttr.WorkID, 0, " The work ID", true, false);
              //  map.AddTBInt(TrackAttr.CWorkID, 0, "CWorkID", true, false);

                map.AddTBInt(TrackAttr.NDFrom, 0, " From node ", true, false);
                map.AddTBString(TrackAttr.NDFromT, null, " From node ( Name )", true, false, 0, 300, 100);

                map.AddTBInt(TrackAttr.NDTo, 0, " To node ", true, false);
                map.AddTBString(TrackAttr.NDToT, null, " To node ( Name )", true, false, 0, 999, 900);

                map.AddTBString(TrackAttr.EmpFrom, null, " From staff ", true, false, 0, 20, 100);
                map.AddTBString(TrackAttr.EmpFromT, null, " From staff ( Name )", true, false, 0, 30, 100);

                map.AddTBString(TrackAttr.EmpTo, null, " To staff ", true, false, 0, 2000, 100);
                map.AddTBString(TrackAttr.EmpToT, null, " To staff ( Name )", true, false, 0, 2000, 100);

                map.AddTBString(TrackAttr.RDT, null, " Date ", true, false, 0, 20, 100);
                map.AddTBFloat(TrackAttr.WorkTimeSpan, 0, " Time span (days)", true, false);
                map.AddTBStringDoc(TrackAttr.Msg, null, " News ", true, false);
                map.AddTBStringDoc(TrackAttr.NodeData, null, " Node data ( Log Information )", true, false);
                map.AddTBString(TrackAttr.Tag, null, " Parameters ", true, false, 0, 300, 3000);
                map.AddTBString(TrackAttr.Exer, null, " Executor ", true, false, 0, 200, 100);
             //   map.AddTBString(TrackAttr.InnerKey, null, " Internal Key, To prevent insertion of duplicate ", true, false, 0, 200, 100);
                #endregion  Field 

                this._enMap = map;
                return this._enMap;
            }
        }

        public string FK_Flow = null;

        /// <summary>
        ///  Locus 
        /// </summary>
        public Track()
        {
        }

        /// <summary>
        ///  Locus 
        /// </summary>
        /// <param name="flowNo"> Process ID </param>
        /// <param name="mypk"> Primary key </param>
        public Track(string flowNo, string mypk)
        {
            string sql = "SELECT * FROM ND" + int.Parse(flowNo) + "Track WHERE MyPK='" + mypk + "'";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
                throw new Exception("@ Log Data Loss .." + sql);
            this.Row.LoadDataTable(dt, dt.Rows[0]);
        }

        /// <summary>
        ///  Create track.
        /// </summary>
        /// <param name="fk_flow"> Process ID </param>
        public static void CreateOrRepairTrackTable(string fk_flow)
        {
            string ptable = "ND" + int.Parse(fk_flow) + "Track";

            #region  Check for this table .
            if (BP.DA.DBAccess.IsExitsObject(ptable))
            {
                try
                {
                    // Add special column .
                    DBAccess.RunSQL("ALTER TABLE  " + ptable + " ADD Tag NVARCHAR(4000) DEFAULT '' NULL");
                }
                catch
                {
                }
                return;
            }

            #endregion  Check for this table .

            try
            {
                /* If the specified table does not exist , Create it .*/
                BP.DA.DBAccess.RunSQL("DROP TABLE WF_Track");
            }
            catch
            {
            }

            Track tk = new Track();
            tk.CheckPhysicsTable();

            string sqlRename = "";
            switch (SystemConfig.AppCenterDBType)
            {
                case DBType.MSSQL:
                    sqlRename = "EXEC SP_RENAME WF_Track, " + ptable;
                    break;

                case DBType.Informix:
                    sqlRename = "RENAME TABLE WF_Track TO " + ptable;
                    break;

                case DBType.Oracle:
                    sqlRename = "ALTER TABLE WF_Track rename to " + ptable;
                    break;

                case DBType.MySQL:
                    sqlRename = "ALTER TABLE WF_Track rename to " + ptable;
                    break;
                default:
                    throw new Exception("@ Not involve this type .");
            }
            DBAccess.RunSQL(sqlRename);

        }
        /// <summary>
        ///  Insert 
        /// </summary>
        /// <param name="mypk"></param>
        public void DoInsert(Int64 mypk)
        {
            string ptable = "ND" + int.Parse(this.FK_Flow) + "Track";
            string dbstr = SystemConfig.AppCenterDBVarStr;
            string sql = "INSERT INTO " + ptable;
            sql += "(";
            sql += "" + TrackAttr.MyPK + ",";
            sql += "" + TrackAttr.ActionType + ",";
            sql += "" + TrackAttr.ActionTypeText + ",";
            sql += "" + TrackAttr.FID + ",";
            sql += "" + TrackAttr.WorkID + ",";
            sql += "" + TrackAttr.NDFrom + ",";
            sql += "" + TrackAttr.NDFromT + ",";
            sql += "" + TrackAttr.NDTo + ",";
            sql += "" + TrackAttr.NDToT + ",";
            sql += "" + TrackAttr.EmpFrom + ",";
            sql += "" + TrackAttr.EmpFromT + ",";
            sql += "" + TrackAttr.EmpTo + ",";
            sql += "" + TrackAttr.EmpToT + ",";
            sql += "" + TrackAttr.RDT + ",";
            sql += "" + TrackAttr.WorkTimeSpan + ",";
            sql += "" + TrackAttr.Msg + ",";
            sql += "" + TrackAttr.NodeData + ",";
            sql += "" + TrackAttr.Tag + ",";

            sql += "" + TrackAttr.Exer + "";
            sql += ") VALUES (";
            sql += dbstr + TrackAttr.MyPK + ",";
            sql += dbstr + TrackAttr.ActionType + ",";
            sql += dbstr + TrackAttr.ActionTypeText + ",";
            sql += dbstr + TrackAttr.FID + ",";
            sql += dbstr + TrackAttr.WorkID + ",";
            sql += dbstr + TrackAttr.NDFrom + ",";
            sql += dbstr + TrackAttr.NDFromT + ",";
            sql += dbstr + TrackAttr.NDTo + ",";
            sql += dbstr + TrackAttr.NDToT + ",";
            sql += dbstr + TrackAttr.EmpFrom + ",";
            sql += dbstr + TrackAttr.EmpFromT + ",";
            sql += dbstr + TrackAttr.EmpTo + ",";
            sql += dbstr + TrackAttr.EmpToT + ",";
            sql += dbstr + TrackAttr.RDT + ",";
            sql += dbstr + TrackAttr.WorkTimeSpan + ",";
            sql += dbstr + TrackAttr.Msg + ",";
            sql += dbstr + TrackAttr.NodeData + ",";
            sql += dbstr + TrackAttr.Tag + ",";
            sql += dbstr + TrackAttr.Exer + "";
            sql += ")";

            // If there is empty , It is considered , Removed from the system which .
            if (string.IsNullOrEmpty(this.ActionTypeText))
                this.ActionTypeText = Track.GetActionTypeT(this.HisActionType);

            if (mypk == 0)
            {
                this.SetValByKey(TrackAttr.MyPK, DBAccess.GenerOIDByGUID());
                //this.SetValByKey(TrackAttr.MyPK, DBAccess.GenerGUID());

            }
            else
            {
                DBAccess.RunSQL("DELETE  FROM " + ptable + " WHERE MyPK=" + mypk);
                this.SetValByKey(TrackAttr.MyPK, mypk);
            }

            this.RDT = DataType.CurrentDataTimess;

            #region  The save 
            try
            {
                Paras ps = SqlBuilder.GenerParas(this, null);
                ps.SQL = sql;

                switch (SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                        this.RunSQL(ps);
                        break;
                    case DBType.Access:
                        this.RunSQL(ps);
                        break;
                    case DBType.MySQL:
                    case DBType.Informix:
                    default:
                        ps.SQL = ps.SQL.Replace("[", "").Replace("]", "");
                        this.RunSQL(ps); //  Run sql.
                        //  this.RunSQL(sql.Replace("[", "").Replace("]", ""), SqlBuilder.GenerParas(this, null));
                        break;
                }
            }
            catch (Exception ex)
            {
                //  Written to the log .
                Log.DefaultLogWriteLineError(ex.Message);

                // Create track.
                Track.CreateOrRepairTrackTable(this.FK_Flow);
                throw ex;
            }

            #endregion  The save 
        }

        /// <summary>
        ///  Increase the authorized person 
        /// </summary>
        /// <returns></returns>
        protected override bool beforeInsert()
        {
            if (BP.Web.WebUser.No == "Guest")
            {
                this.Exer = BP.Web.GuestUser.Name;
            }
            else
            {
                if (BP.Web.WebUser.IsAuthorize)
                    this.Exer = BP.WF.Glo.DealUserInfoShowModel(BP.Web.WebUser.AuthorizerEmpID, BP.Web.WebUser.Auth);
                else
                    this.Exer = BP.WF.Glo.DealUserInfoShowModel(BP.Web.WebUser.No, BP.Web.WebUser.Name);
            }

            this.RDT = BP.DA.DataType.CurrentDataTimess;

            this.DoInsert(0);
            return false;
        }
        #endregion  Property 
    }

    /// <summary>
    ///  Track collection 
    /// </summary>
    public class Tracks : BP.En.Entities
    {
        /// <summary>
        ///  Track collection 
        /// </summary>
        public Tracks()
        {
        }

        public override Entity GetNewEntity
        {
            get
            {
                return new Track();
            }
        }
    }
}