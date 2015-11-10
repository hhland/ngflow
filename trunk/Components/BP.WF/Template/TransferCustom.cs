using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.WF
{
	/// <summary>
	///  Custom runpath   Property 
	/// </summary>
    public class TransferCustomAttr : EntityMyPKAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  The work ID
        /// </summary>
        public const string WorkID = "WorkID";
        /// <summary>
        ///  Node ID
        /// </summary>
        public const string FK_Node = "FK_Node";
        /// <summary>
        ///  Processors 
        /// </summary>
        public const string Worker = "Worker";
        /// <summary>
        ///  Order 
        /// </summary>
        public const string Idx = "Idx";
        /// <summary>
        ///  Start Time 
        /// </summary>
        public const string StartDT = "StartDT";
        /// <summary>
        ///  Insert Date 
        /// </summary>
        public const string RDT = "RDT";
        /// <summary>
        ///  To enable a number of sub-processes 
        /// </summary>
        public const string SubFlowNo = "SubFlowNo";
        #endregion
    }
	/// <summary>
	///  Custom runpath 
	/// </summary>
    public class TransferCustom : EntityMyPK
    {
        #region  Property 
        /// <summary>
        ///  Node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return this.GetValIntByKey(TransferCustomAttr.FK_Node);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.FK_Node, value);
            }
        }
        public Int64 WorkID
        {
            get
            {
                return this.GetValInt64ByKey(TransferCustomAttr.WorkID);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.WorkID, value);
            }
        }
        /// <summary>
        ///  Processors 
        /// </summary>
        public string Worker
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.Worker);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.Worker, value);
            }
        }
        /// <summary>
        ///  To enable a number of sub-processes 
        /// </summary>
        public string SubFlowNo
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.SubFlowNo);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.SubFlowNo, value);
            }
        }
        /// <summary>
        ///  Order 
        /// </summary>
        public int Idx
        {
            get
            {
                return this.GetValIntByKey(TransferCustomAttr.Idx);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.Idx, value);
            }
        }
        /// <summary>
        ///  Start Time £¨ Can be empty £©
        /// </summary>
        public string StartDT
        {
            get
            {
                return this.GetValStringByKey(TransferCustomAttr.StartDT);
            }
            set
            {
                this.SetValByKey(TransferCustomAttr.StartDT, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        /// TransferCustom
        /// </summary>
        public TransferCustom()
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
                Map map = new Map("WF_TransferCustom");
                map.EnDesc = " Custom runpath "; // Person in charge liuxianchen.
                map.EnType = EnType.Admin;

                map.AddMyPK(); // Unique primary key .

                // Primary key .
                map.AddTBInt(TransferCustomAttr.WorkID, 0, "WorkID", true, false);
                map.AddTBInt(TransferCustomAttr.FK_Node, 0, " The work ID", true, false);
                map.AddTBString(TransferCustomAttr.Worker, null, " Processors ", true, false, 0, 200, 10);
                map.AddTBString(TransferCustomAttr.SubFlowNo, null, " To go through a number of sub-processes ", true, false, 0, 3, 10);
                map.AddTBDateTime(TransferCustomAttr.RDT, null, " Date Time ", true, false);
                map.AddTBInt(TransferCustomAttr.Idx, 0, " Sequence number ", true, false);
              
                //map.AddTBString(TransferCustomAttr.StartDT, null, " Start Time ", true, false, 0, 20, 10);
                
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        /// <summary>
        ///  Get to define a path to reach the .
        ///  To analyze the following situations :
        /// 1,  The current node does not exist inside the queue , The first one returns .
        /// 2,  If the current queue is empty , Considers the need to end off ,  Return null.
        /// 3,  If the current node is the last , Returns null, To indicate the end of the process .
        /// </summary>
        /// <param name="workid"> Current work ID</param>
        /// <param name="currNodeID"> The current node ID</param>
        /// <returns> Get to define a path to reach the , If you do not return empty .</returns>
        public static TransferCustom GetNextTransferCustom(Int64 workid, int currNodeID)
        {
            TransferCustoms ens = new TransferCustoms();
            ens.Retrieve(TransferCustomAttr.WorkID, workid, TransferCustomAttr.Idx);
            if (ens.Count == 0)
                return null;

                /* Gets the last */
                TransferCustom tEnd = ens[ens.Count-1] as TransferCustom;
                if (tEnd.FK_Node == currNodeID)
                    return null; // Pledged to end , Because this is the last link .

            //  Start looking for ,  To find the next current node .
            bool isRec = false;
            foreach (TransferCustom en in ens)
            {
                if (en.FK_Node == currNodeID)
                {
                    isRec = true;
                    continue;
                }
                if (isRec)
                    return en;
            }

            // If you do not find , It returns the last .
            return (TransferCustom)ens[0];
        }
    }
	/// <summary>
	///  Custom runpath 
	/// </summary>
	public class TransferCustoms: EntitiesMyPK
	{
		#region  Method 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new TransferCustom();
			}
		}
		/// <summary>
        ///  Custom runpath 
		/// </summary>
		public TransferCustoms(){} 		 
		#endregion
	}
}
