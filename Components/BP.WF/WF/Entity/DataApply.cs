using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port; 
using BP.Port; 
using BP.En;

namespace BP.WF
{
	/// <summary>
	///  Additional time to apply 
	/// </summary>
	public class DataApplyAttr 
	{
		#region  Basic properties 
        /// <summary>
        /// MyPK
        /// </summary>
        public const string MyPK = "MyPK";
		/// <summary>
		///  The work ID
		/// </summary>
		public const  string WorkID="WorkID";
		/// <summary>
		///  Node 
		/// </summary>
		public const  string NodeId="NodeId";

        /// <summary>
        ///  Applicant 
        /// </summary>
        public const string Applyer = "Applyer";
        public const string ApplyData = "ApplyData";
        public const string ApplyDays = "ApplyDays";
        public const string ApplyNote1 = "ApplyNote1";
        public const string ApplyNote2 = "ApplyNote2";
		/// <summary>
		///  Reviewer 
		/// </summary>
        public const string Checker = "Checker";
        public const string CheckerData = "CheckerData";

        public const string CheckerDays = "CheckerDays";
        public const string CheckerNote1 = "CheckerNote1";
        public const string CheckerNote2 = "CheckerNote2";


        public const string RunState = "RunState";

		#endregion
	}
	/// <summary>
	///  Additional time to apply 
	/// </summary>
	public class DataApply : Entity
	{		
		#region  Basic properties 
        /// <summary>
        /// MyPK
        /// </summary>
        public string MyPK
        {
            get
            {
                return this.GetValStrByKey(DataApplyAttr.MyPK);
            }
            set
            {
                this.SetValByKey(DataApplyAttr.MyPK, value);
            }
        }
		/// <summary>
		///  The work ID
		/// </summary>
        public Int64 WorkID
		{
			get
			{
				return this.GetValInt64ByKey(DataApplyAttr.WorkID);
			}
			set
			{
				SetValByKey(DataApplyAttr.WorkID,value);
			}
		}
		/// <summary>
		/// NodeId
		/// </summary>
		public int  NodeId
		{
			get
			{
				return this.GetValIntByKey(DataApplyAttr.NodeId);
			}
			set
			{
				SetValByKey(DataApplyAttr.NodeId,value);
			}
		}
        public int ApplyDays
        {
            get
            {
                return this.GetValIntByKey(DataApplyAttr.ApplyDays);
            }
            set
            {
                SetValByKey(DataApplyAttr.ApplyDays, value);
            }
        }
        public int CheckerDays
        {
            get
            {
                return this.GetValIntByKey(DataApplyAttr.CheckerDays);
            }
            set
            {
                SetValByKey(DataApplyAttr.CheckerDays, value);
            }
        }
        public string Applyer
		{
			get
			{
				return this.GetValStringByKey(DataApplyAttr.Applyer);
			}
			set
			{
                SetValByKey(DataApplyAttr.Applyer, value);
			}
		}
        public string ApplyNote1
        {
            get
            {
                return this.GetValStringByKey(DataApplyAttr.ApplyNote1);
            }
            set
            {
                SetValByKey(DataApplyAttr.ApplyNote1, value);
            }
        }
        public string ApplyNote2
        {
            get
            {
                return this.GetValStringByKey(DataApplyAttr.ApplyNote2);
            }
            set
            {
                SetValByKey(DataApplyAttr.ApplyNote2, value);
            }
        }
        public string ApplyData
        {
            get
            {
                return this.GetValStringByKey(DataApplyAttr.ApplyData);
            }
            set
            {
                SetValByKey(DataApplyAttr.ApplyData, value);
            }
        }
        public string Checker
        {
            get
            {
                return this.GetValStringByKey(DataApplyAttr.Checker);
            }
            set
            {
                SetValByKey(DataApplyAttr.Checker, value);
            }
        }
        public int RunState
        {
            get
            {
                return this.GetValIntByKey(DataApplyAttr.RunState);
            }
            set
            {
                SetValByKey(DataApplyAttr.RunState, value);
            }
        }
		#endregion 

		#region  Constructor 
		/// <summary>
		///  Additional time to apply 
		/// </summary>
		public DataApply(){}

        public DataApply(int workid, int nodeid)
        {
            this.WorkID = workid;
            this.NodeId = nodeid;
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

                Map map = new Map("WF_DataApply");
                map.EnDesc = " Additional time to apply ";
                map.EnType = EnType.App;

                map.AddMyPK();

                map.AddTBInt(DataApplyAttr.WorkID, 0, " The work ID", true, true);
                map.AddTBInt(DataApplyAttr.NodeId, 0, "NodeId", true, true);
                map.AddTBInt(DataApplyAttr.RunState, 0, " Operating status 0, Not submitted ,1, Submit an application for approval of the Executive ,2, Audit completed .", true, true);


                map.AddTBInt(DataApplyAttr.ApplyDays, 0, " Apply for a few days ", true, true);
                map.AddTBDate(DataApplyAttr.ApplyData, " Date of Application ", true, true);
                map.AddDDLEntities(DataApplyAttr.Applyer, null, " Applicant ", new Emps(), false);
                map.AddTBStringDoc(DataApplyAttr.ApplyNote1, "", " Reasons for applying ", true, true);
                map.AddTBStringDoc(DataApplyAttr.ApplyNote2, "", " Request Note ", true, true);



                map.AddDDLEntities(DataApplyAttr.Checker, null, " Approver ", new Emps(), false);
                map.AddTBDate(DataApplyAttr.CheckerData, " Approval Date ", true, true);
                map.AddTBInt(DataApplyAttr.CheckerDays, 0, " Approval of the number of days ", true, true);
                map.AddTBStringDoc(DataApplyAttr.CheckerNote1, "", " Approval comments ", true, true);
                map.AddTBStringDoc(DataApplyAttr.CheckerNote2, "", " Approval Remarks ", true, true);


                RefMethod rm = new RefMethod();
                rm.Title = " Propose additional time to apply ";
                rm.Warning = " Are you sure you want to raise your leadership additional time to apply it ?";
                rm.ClassMethodName = this.ToString() + ".DoApply";
               // rm.ClassMethodName =   "BP.WF.DataApply.DoApply";


                Attrs attrs = new Attrs();
                attrs.AddTBInt(DataApplyAttr.ApplyDays, 0, " Additional days ", true, false);
                attrs.AddDDLEntities(DataApplyAttr.Checker, null, " Approver ", new Emps(), false);
                attrs.AddTBStringDoc(DataApplyAttr.ApplyNote1, "", " Reasons for applying ", true, false);
                attrs.AddTBStringDoc(DataApplyAttr.ApplyNote2, "", " Request Note ", true, false);
                map.AddRefMethod(rm);


                //rm = new RefMethod();
                //rm.Title = " Approval time ";

                //Attrs attrs = new Attrs();
                //attrs.AddTBInt(DataApplyAttr.CheckerDays, 0, " Approval of the number of days ", true, false);
                //map.AddTBDate(DataApplyAttr.CheckerData, " Approval Date ", true, true);

                //attrs.AddDDLEntities(DataApplyAttr.Checker, null, " Approver ", new Emps(), false);
                //attrs.AddTBStringDoc(DataApplyAttr.ApplyNote1, "", " Reasons for applying ", true, false);
                //attrs.AddTBStringDoc(DataApplyAttr.ApplyNote2, "", " Request Note ", true, false);
                //map.AddRefMethod(rm);



                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion	 

        #region  Method 
        public string DoApply(int days, string checker, string note1, string note2)
        {
            this.ApplyDays = days;

            this.Checker = checker;
            this.ApplyNote1 = note1;
            this.ApplyNote2 = note2;
            this.ApplyData = DataType.CurrentData;


            this.RunState = 1; /* Into the state submitted for review */
            this.Update();

            return " Successful implementation , Has been handed over to " + checker+" Check .";
             
        }
        public string DoCheck(int days, string checker, string note1, string note2)
        {
            this.ApplyDays = days;

            this.Checker = checker;
            this.ApplyNote1 = note1;
            this.ApplyNote2 = note2;

            //  Adjust the current early warning and 
            GenerWorkerLists wls = new GenerWorkerLists(this.WorkID, this.NodeId);
            wls.Retrieve(GenerWorkerListAttr.WorkID, this.WorkID, GenerWorkerListAttr.FK_Node, this.NodeId);
            foreach (GenerWorkerList wl in wls)
            {
               // wl.DTOfWarning = DataType.AddDays(wl.DTOfWarning, days);
               // wl.SDT = DataType.AddDays(wl.SDT, days);
                wl.DirectUpdate();
            }

            this.RunState = 2; /* Into the state submitted for review */
            this.Update();

            return " Successful implementation , Has been handed over to " + checker + " Check .";
        }
        #endregion
    }
	/// <summary>
	///  Additional time to apply s 
	/// </summary>
	public class DataApplys : Entities
	{	 
		#region  Structure 
		/// <summary>
		///  Additional time to apply s
		/// </summary>
		public DataApplys()
		{

		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new DataApply();
			}
		}
		#endregion
	}
	
}
