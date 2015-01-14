using System;
using System.Collections;
using System.Web.Hosting;
using BP.Web;

namespace BP.WF
{
	/// <summary>
	/// MsgsManager
	/// </summary>
	public class MsgsManager
	{
		/// <summary> 
		///  Delete work by The work ID
		/// </summary>
		/// <param name="workId"></param>
        public static void DeleteByWorkID(Int64 workId)
        {
            System.Web.HttpContext.Current.Application.Lock();
            Msgs msgs = (Msgs)System.Web.HttpContext.Current.Application["WFMsgs"];
            if (msgs == null)
            {
                msgs = new Msgs();
                System.Web.HttpContext.Current.Application["WFMsgs"] = msgs;
            }
            //  Clear all work ID=workid  News .
            msgs.ClearByWorkID(workId);
            System.Web.HttpContext.Current.Application.UnLock();
        }
		/// <summary>
		///  Adding information 
		/// </summary>
		/// <param name="wls"> Workers collection </param>
		/// <param name="flowName"> Process Name </param>
		/// <param name="nodeName"> Node Name </param>
		/// <param name="title"> Title </param>
        public static void AddMsgs(GenerWorkerLists wls, string flowName, string nodeName, string title)
		{
			return;

			System.Web.HttpContext.Current.Application.Lock();
			Msgs msgs= (Msgs)System.Web.HttpContext.Current.Application["WFMsgs"];
			if (msgs==null)
			{
				msgs= new Msgs();
				System.Web.HttpContext.Current.Application["WFMsgs"]=msgs;
			}
			//  Clear all work ID=workid  News .
			msgs.ClearByWorkID(wls[0].GetValIntByKey("WorkID"));
            foreach (GenerWorkerList wl in wls)
			{
				if (wl.FK_Emp==WebUser.No)
					continue;
				//msgs.AddMsg(wl.WorkID,wl.FK_Node,wl.FK_Emp," From the process ["+flowName+"] Node ["+nodeName+"] Work node title ["+title+"] News .");
			}
			System.Web.HttpContext.Current.Application.UnLock();
		}
		/// <summary>
		/// sss
		/// </summary>
		/// <param name="empId"></param>
		/// <returns></returns>
		public static Msgs GetMsgsByEmpID_del(int empId)
		{
			Msgs msgs= (Msgs)System.Web.HttpContext.Current.Application["WFMsgs"];
			if (msgs==null)
			{
				msgs= new Msgs();
				System.Web.HttpContext.Current.Application["WFMsgs"]=msgs;
			}
			return msgs.GetMsgsByEmpID_del(empId); 
		}
		/// <summary>
		///  Remove the number of messages 
		/// </summary>
		/// <param name="empId"></param>
		/// <returns></returns>
		public static int GetMsgsCountByEmpID(int empId)
		{
			string sql="select COUNT(*) from v_wf_msg WHERE FK_Emp='"+WebUser.No+"'";
			return DA.DBAccess.RunSQLReturnValInt(sql);
		}
		/// <summary>
		///  Clear message 
		/// </summary>
		/// <param name="empId"></param>
		public static void ClearMsgsByEmpID_(int empId)
		{
			System.Web.HttpContext.Current.Application.Lock();
			Msgs msgs= (Msgs)System.Web.HttpContext.Current.Application["WFMsgs"];
			msgs.ClearByEmpId_del(empId); 
			System.Web.HttpContext.Current.Application.UnLock();
		}
		/// <summary>
		///  Initializes all news .
		/// </summary>
		public static void InitMsgs()
		{
		}	 
	}
	/// <summary>
	/// Msg  The summary .
	/// </summary>
	public class Msg
	{
		#region  Property 
		/// <summary>
		///  Sound files 
		/// </summary>
		private string  _SoundUrl="/WF/Sound/ring.wav";
		/// <summary>
		///  Sound files 
		/// </summary>
		public string SoundUrl
		{
			get
			{
				return  _SoundUrl;
			}
			set
			{
				_SoundUrl=value;
			}
		}
		/// <summary>
		/// _IsOpenSound
		/// </summary>
		private bool  _IsOpenSound=true;
		/// <summary>
		/// IsOpenSound
		/// </summary>
		public bool IsOpenSound
		{
			get
			{
				if (this._IsOpenSound==false)
				{
					return false;
				}
				else
				{
					this._IsOpenSound=false;
					return true;
				}
			}
		}
		/// <summary>
		/// _WorkID
		/// </summary>
		private int _WorkID=0;
		/// <summary>
		/// _NodeId
		/// </summary>
		private int _NodeId=0;
		/// <summary>
		/// _Info
		/// </summary>
		private string  _Info="";
		/// <summary>
		/// _ToEmpId
		/// </summary>
		private int _ToEmpId=0;
		/// <summary>
		///  Information 
		/// </summary>
		public string Info
		{
			get
			{
				return this._Info;
			}
			set
			{
				_Info=value;
			}
		}
		/// <summary>
		///  The work ID
		/// </summary>
		public int WorkID
		{
			get
			{
				return _WorkID;
			}
			set
			{
				_WorkID=value;
			}
		}
		/// <summary>
		/// NodeID
		/// </summary>
		public int NodeId
		{
			get
			{
				return _NodeId;
			}
			set
			{
				_NodeId=value;
			}
		}
		/// <summary>
		/// ToEmpId
		/// </summary>
		public int ToEmpId
		{
			get
			{
				return _ToEmpId;
			}
			set
			{
				_ToEmpId=value;
			}
		}		
		#endregion

		/// <summary>
		///  Information 
		/// </summary>
		public Msg(){}

     

		/// <summary>
		///  Information 
		/// </summary>
		/// <param name="workId"></param>
		/// <param name="nodeId"></param>
		/// <param name="toEmpId"></param>
		/// <param name="info"></param>
		public Msg(int workId , int nodeId, int toEmpId,string info)
		{
			this.WorkID=workId;
			this.NodeId=nodeId;
			this.ToEmpId=toEmpId;
			this.Info=info;
		}
	}
	/// <summary>
	///  Message set 
	/// </summary>
	public class Msgs:ArrayList
	{

		#region  Increase news 
		/// <summary>
		///  Increase news 
		/// </summary>
		/// <param name="workId"></param>
		/// <param name="nodeId"></param>
		/// <param name="toEmpId"></param>
		/// <param name="info"></param>
		public void AddMsg(int workId , int nodeId, int toEmpId,string info)
		{
			return ;
			Msg msg = new Msg();
			msg.WorkID=workId;
			msg.NodeId=nodeId;
			msg.ToEmpId=toEmpId;
			msg.Info=info;
			this.Add(msg);
		}
		/// <summary>
		///  Increase news 
		/// </summary>
		/// <param name="msg"> News </param>
		public void AddMsg(Msg msg)
		{
			return ;
			this.Add(msg);
		}
		#endregion 

		#region  Operating on news collection 
		/// <summary>
		///  Ann Work ID  Clear Message .
		/// </summary>
		/// <param name="workId"></param>
        public void ClearByWorkID(Int64 workId)
		{
			return ;
			Msgs ens = this.GetMsgsByWorkID(workId);
			foreach(Msg msg in ens)
			{			 
				this.Remove(msg);
			} 
		}
		/// <summary>
		///  Clear Staff Information 
		/// </summary>
		/// <param name="empId"></param>
		public void ClearByEmpId_del(int empId)
		{
			return ;
			Msgs ens = this.GetMsgsByEmpID_del(empId);
			foreach(Msg msg in ens)
			{
				this.Remove(msg);
			}
		}
		/// <summary>
		///  Clear Staff Information 
		/// </summary>
		/// <param name="workId"></param>
		/// <returns></returns>
        public Msgs GetMsgsByWorkID(Int64 workId)
		{
			return null ;
			Msgs ens = new Msgs();
			foreach(Msg msg in this)
			{
				if (msg.WorkID==workId)
					ens.AddMsg(msg);
			}
			return ens;
		}
		/// <summary>
		/// sss
		/// </summary>
		/// <param name="empId"></param>
		/// <returns></returns>
		public Msgs GetMsgsByEmpID_del(int empId)
		{
			//return ;
			Msgs ens = new Msgs();
			foreach(Msg msg in this)
			{
				if (msg.ToEmpId==empId)
					ens.AddMsg(msg);
			}
			return ens;
		}
		/// <summary>
		///  Obtain information on the number of .
		/// </summary>
		/// <param name="empId"> Staff </param>
		/// <returns> Information Number </returns>
		public int GetMsgsCountByEmpID(int empId)
		{
			return 0;
			int i = 0 ;
			//bool isHaveNew=false;
			int newMsgNum=0; 
			foreach(Msg msg in this)
			{
				if (msg.ToEmpId==empId)
				{
					if (msg.IsOpenSound)
						newMsgNum++;
					i++;
				}
			}
			if (newMsgNum>0)
			{
                //if (WebUser.IsSoundAlert)				 
                //    System.Web.HttpContext.Current.Response.Write("<bgsound src='"+BP.Sys.Glo.Request.ApplicationPath+Web.WebUser.NoticeSound+"' loop=1 >"  );
                //if (WebUser.IsTextAlert)
                //    BP.Sys.PubClass.ResponseWriteBlueMsg(" You have ["+newMsgNum+"] New jobs ." );
				//System.Web.HttpContext.Current.Response.Write("<bgsound src='"+BP.Sys.Glo.Request.ApplicationPath+Web.WebUser.NoticeSound+"' loop=1 >"  );

			}
			return i;
		}
		#endregion

		/// <summary>
		///  News s
		/// </summary>
		public Msgs()
		{
		}
        
		/// <summary>
		///  Access to data based on location 
		/// </summary>
		public new Msg this[int index]
		{
			get 
			{
				return (Msg)this[index];
			}
		}	 

	}
	/// <summary>
	///  Users news 
	/// </summary>
	public class UserMsgs
	{
		#region  Property 
		/// <summary>
		/// _IsOpenSound
		/// </summary>
		private bool  _IsOpenSound=false;
		/// <summary>
		/// _IsOpenSound
		/// </summary>
		public bool IsOpenSound
		{
			get
			{
				if (this._IsOpenSound==false)
				{
					return false;
				}
				else
				{
					this._IsOpenSound=false;
					return true;
				}
			}
		}
		#endregion

		#region  Structure 
		/// <summary>
		///  Users news 
		/// </summary>
		public UserMsgs()
		{
		}
		/// <summary>
		///  Users news 
		/// </summary>
		/// <param name="empId"></param>
		public UserMsgs(int empId)
		{
		}
		#endregion
	}
}
