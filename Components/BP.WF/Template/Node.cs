using System;
using System.Data;
using BP.DA;
using BP.Sys;
using BP.En;
using System.Collections;
using BP.Port;
using BP.WF.Data;
using BP.WF.Template;

namespace BP.WF
{
    /// <summary>
    ///  Here each node to store information .	 
    /// </summary>
    public class Node : Entity
    {
        

        #region  Sons process property 
        /// <summary>
        /// ( The current node is the promoter process node ) Check that all the sub-process after the end of , The node can send down ?
        /// </summary>
        public bool IsCheckSubFlowOver
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCheckSubFlowOver);
            }
        }
        #endregion  Sons process property 

        #region  Execution node event .
        /// <summary>
        ///  Performing motion events 
        /// </summary>
        /// <param name="doType"> Event Type </param>
        /// <param name="en"> Entity Parameters </param>
        //public string DoNodeEventEntity(string doType,Node currNode, Entity en, string atPara)
        //{
        //    if (this.NDEventEntity != null)
        //        return NDEventEntity.DoIt(doType,currNode, en, atPara);

        //    return this.MapData.FrmEvents.DoEventNode(doType, en, atPara);
        //}
        //private BP.WF.NodeEventBase _NDEventEntity = null;
        ///// <summary>
        /////  Entity class node , Did not return empty .
        ///// </summary>
        //private BP.WF.NodeEventBase NDEventEntity11
        //{
        //    get
        //    {
        //        if (_NDEventEntity == null && this.NodeMark!="" && this.NodeEventEntity!="" )
        //            _NDEventEntity = BP.WF.Glo.GetNodeEventEntityByEnName( this.NodeEventEntity);

        //        return _NDEventEntity;
        //    }
        //}
        #endregion  Execution node event .

        #region  Parameter Properties 
        /// <summary>
        ///  Conditions direction control rules 
        /// </summary>
        public CondModel CondModel
        {
            get
            {
                return (CondModel)this.GetValIntByKey(NodeAttr.CondModel);
            }
        }
        /// <summary>
        ///  Timeout handling 
        /// </summary>
        public OutTimeDeal HisOutTimeDeal
        {
            get
            {
                return (OutTimeDeal)this.GetValIntByKey(NodeAttr.OutTimeDeal);
            }
            set
            {
                this.SetValByKey(NodeAttr.OutTimeDeal, (int)value);
            }
        }
        /// <summary>
        ///  Child thread type 
        /// </summary>
        public SubThreadType HisSubThreadType
        {
            get
            {
                return (SubThreadType)this.GetValIntByKey(NodeAttr.SubThreadType);
            }
            set
            {
                this.SetValByKey(NodeAttr.SubThreadType, (int)value);
            }
        }
        #endregion

        #region  Foreign key attribute .
        public CC HisCC
        {
            get
            {
                CC obj = this.GetRefObject("HisCC") as CC;
                if (obj == null)
                {
                    obj = new CC();
                    obj.NodeID = this.NodeID;
                    obj.Retrieve();
                    this.SetRefObject("HisCC", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  He going to turn the direction of the collection 
        ///  If he did to the steering direction , He is the end node .
        ///  No concept of life cycle , All nodes .
        /// </summary>
        public Nodes HisToNodes
        {
            get
            {
                Nodes obj = this.GetRefObject("HisToNodes") as Nodes;
                if (obj == null)
                {
                    obj = new Nodes();
                    obj.AddEntities(this.HisToNDs);
                    this.SetRefObject("HisToNodes", obj);
                }
                return obj;
            }
        }
        /// <summary>
        ///  His work 
        /// </summary>
        public Work HisWork
        {
            get
            {
                Work obj=null;
                if (this.IsStartNode)
                {
                    obj = new BP.WF.GEStartWork(this.NodeID,this.NodeFrmID);
                    obj.HisNode = this;
                    obj.NodeID = this.NodeID;
                    return obj;
                    this.SetRefObject("HisWork", obj);
                }
                else
                {
                    obj = new BP.WF.GEWork( this.NodeID,this.NodeFrmID);
                    obj.HisNode = this;
                    obj.NodeID = this.NodeID;
                    return obj;
                    //this.SetRefObject("HisWork", obj);
                }
               // return obj;

                /*  Into the cache, there is no way to perform data clone. */

               // Work obj = this.GetRefObject("HisWork") as Work;
               // if (obj == null)
               // {
               //     if (this.IsStartNode)
               //     {
               //         obj = new BP.WF.GEStartWork(this.NodeID);
               //         obj.HisNode = this;
               //         obj.NodeID = this.NodeID;
               //         this.SetRefObject("HisWork", obj);
               //     }
               //     else
               //     {
               //         obj = new BP.WF.GEWork(this.NodeID);
               //         obj.HisNode = this;
               //         obj.NodeID = this.NodeID;
               //         this.SetRefObject("HisWork", obj);
               //     }
               // }
               //// obj.GetNewEntities.GetNewEntity;
               //// obj.Row = null;
               // return obj;
            }
        }
        /// <summary>
        ///  His work s
        /// </summary>
        public Works HisWorks
        {
            get
            {
                Works obj = this.HisWork.GetNewEntities as Works;
                return obj;
                ////Works obj = this.GetRefObject("HisWorks") as Works;
                ////if (obj == null)
                ////{
                //    this.SetRefObject("HisWorks",obj);
                //}
                //return obj;
            }
        }

        /// <summary>
        ///  Process 
        /// </summary>
        public Flow HisFlow
        {
            get
            {
                Flow  obj =this.GetRefObject("Flow") as Flow;
                if (obj == null)
                {
                    obj=new Flow(this.FK_Flow);
                    this.SetRefObject("Flow", obj);
                }
                return obj;
            }
        }
        /// <summary>
        /// HisFrms
        /// </summary>
        public Frms HisFrms
        {
            get
            {
                Frms frms = new Frms();
                FrmNodes fns = new FrmNodes(this.FK_Flow,this.NodeID);
                foreach (FrmNode fn in fns)
                    frms.AddEntity(fn.HisFrm);
                return frms;

                //this.SetRefObject("HisFrms", obj);
                //Frms obj = this.GetRefObject("HisFrms") as Frms;
                //if (obj == null)
                //{
                //    obj = new Frms();
                //    FrmNodes fns = new FrmNodes(this.NodeID);
                //    foreach (FrmNode fn in fns)
                //        obj.AddEntity(fn.HisFrm);
                //    this.SetRefObject("HisFrms", obj);
                //}
                //return obj;
            }
        }
        /// <summary>
        ///  He will have to come from the direction of the collection 
        ///  If he did not come in the direction , He is the starting node .
        /// </summary>
        public Nodes FromNodes
        {
            get
            {
                Nodes obj = this.GetRefObject("HisFromNodes") as Nodes;
                if (obj == null)
                {
                    //  Depending on the direction of this node generates arrive .
                    Directions ens = new Directions();
                    if (this.IsStartNode)
                        obj = new Nodes();
                    else
                        obj = ens.GetHisFromNodes(this.NodeID);
                    this.SetRefObject("HisFromNodes", obj);
                }
                return obj;
            }
        }
        public BillTemplates BillTemplates
        {
            get
            {
                BillTemplates obj= this.GetRefObject("BillTemplates") as BillTemplates;
                if (obj == null)
                {
                    obj = new BillTemplates(this.NodeID);
                    this.SetRefObject("BillTemplates", obj);
                }
                return obj;
            }
        }
        public NodeStations NodeStations
        {
            get
            {
                NodeStations obj = this.GetRefObject("NodeStations") as NodeStations;
                if (obj == null)
                {
                    obj = new NodeStations(this.NodeID);
                    this.SetRefObject("NodeStations", obj);
                }
                return obj;
            }
        }
        public NodeDepts NodeDepts
        {
            get
            {
                NodeDepts obj = this.GetRefObject("NodeDepts") as NodeDepts;
                if (obj == null)
                {
                    obj = new NodeDepts(this.NodeID);
                    this.SetRefObject("NodeDepts", obj);
                }
                return obj;
            }
        }
        public NodeEmps NodeEmps
        {
            get
            {
                NodeEmps obj = this.GetRefObject("NodeEmps") as NodeEmps;
                if (obj == null)
                {
                    obj = new NodeEmps(this.NodeID);
                    this.SetRefObject("NodeEmps", obj);
                }
                return obj;
            }
        }
        public FrmNodes FrmNodes
        {
            get
            {
                FrmNodes obj = this.GetRefObject("FrmNodes") as FrmNodes;
                if (obj == null)
                {
                    obj = new FrmNodes(this.FK_Flow, this.NodeID);
                    this.SetRefObject("FrmNodes", obj);
                }
                return obj;
            }
        }
        public MapData MapData
        {
            get
            {
                MapData obj = this.GetRefObject("MapData") as MapData;
                if (obj == null)
                {
                    obj = new MapData("ND"+this.NodeID);
                    this.SetRefObject("MapData", obj);
                }
                return obj;
            }
        }
        #endregion

        #region  Preliminary examination of the global  Node
        public override string PK
        {
            get
            {
                return "NodeID";
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
                if (BP.Web.WebUser.No == "admin")
                    uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  Preliminary examination of the global 
        /// </summary>
        /// <returns></returns>
        public NodePosType GetHisNodePosType()
        {
            string nodeid = this.NodeID.ToString();
            if (nodeid.Substring(nodeid.Length - 2) == "01")
                return NodePosType.Start;

            if (this.FromNodes.Count == 0)
                return NodePosType.Mid;

            if (this.HisToNodes.Count == 0)
                return NodePosType.End;
            return NodePosType.Mid;
        }
        /// <summary>
        ///  Inspection process , Calculated field repair necessary information .
        /// </summary>
        /// <param name="fl"> Process </param>
        /// <returns> Return check information </returns>
        public static string CheckFlow(Flow fl)
        {
            string sqls = "UPDATE WF_Node SET IsCCFlow=0";
            sqls += "@UPDATE WF_Node  SET IsCCFlow=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond a WHERE a.NodeID= NodeID AND CondType=1 )";
            BP.DA.DBAccess.RunSQLs(sqls);
            //  Delete the necessary data .
            DBAccess.RunSQL("DELETE FROM WF_NodeEmp WHERE FK_Emp  not in (select No from Port_Emp)");
            DBAccess.RunSQL("DELETE FROM WF_Emp WHERE NO not in (select No from Port_Emp )");
            DBAccess.RunSQL("UPDATE WF_Emp set Name=(SELECT Name From Port_Emp where Port_Emp.No=WF_Emp.No),FK_Dept=(select FK_Dept from Port_Emp where Port_Emp.No=WF_Emp.No)");

            Nodes nds = new Nodes();
            nds.Retrieve(NodeAttr.FK_Flow, fl.No);

            if (nds.Count == 0)
                return " Process [" + fl.No + fl.Name + "] No node data , You need to register at this process .";

            //  Whether the update is complete condition node .
            DA.DBAccess.RunSQL("UPDATE WF_Node SET IsCCFlow=0  WHERE FK_Flow='" + fl.No + "'");
            DA.DBAccess.RunSQL("DELETE FROM WF_Direction WHERE Node=0 OR ToNode=0");
            DA.DBAccess.RunSQL("DELETE FROM WF_Direction WHERE Node  NOT IN (SELECT NODEID FROM WF_Node )");
            DA.DBAccess.RunSQL("DELETE FROM WF_Direction WHERE ToNode  NOT IN (SELECT NODEID FROM WF_Node) ");

            //  Document information , Post , Node information .
            foreach (Node nd in nds)
            {
                DA.DBAccess.RunSQL("UPDATE WF_Node SET FK_FlowSort='" + fl.FK_FlowSort + "',FK_FlowSortT='" + fl.FK_FlowSortText + "'");

                BP.Sys.MapData md = new BP.Sys.MapData();
                md.No = "ND" + nd.NodeID;
                if (md.IsExits == false)
                {
                    nd.CreateMap();
                }

                //  Jobs .
                NodeStations stas = new NodeStations(nd.NodeID);
                string strs = "";
                foreach (NodeStation sta in stas)
                    strs += "@" + sta.FK_Station;
                nd.HisStas = strs;

                //  Departments .
                NodeDepts ndpts = new NodeDepts(nd.NodeID);
                strs = "";
                foreach (NodeDept ndp in ndpts)
                    strs += "@" + ndp.FK_Dept;

                nd.HisDeptStrs = strs;

                //  Executable staff .
                NodeEmps ndemps = new NodeEmps(nd.NodeID);
                strs = "";
                foreach (NodeEmp ndp in ndemps)
                    strs += "@" + ndp.FK_Emp;
                //nd.HisEmps = strs;

                //  Subprocess .
                NodeFlows ndflows = new NodeFlows(nd.NodeID);
                strs = "";
                foreach (NodeFlow ndp in ndflows)
                    strs += "@" + ndp.FK_Flow;
                nd.HisSubFlows = strs;

                //  Node direction .
                strs = "";
                Directions dirs = new Directions(nd.NodeID, 0);
                foreach (Direction dir in dirs)
                    strs += "@" + dir.ToNode;
                nd.HisToNDs = strs;

                //  Invoice 
                strs = "";
                BillTemplates temps = new BillTemplates(nd);
                foreach (BillTemplate temp in temps)
                    strs += "@" + temp.No;
                nd.HisBillIDs = strs;

                //  Check the location attribute node .
                nd.HisNodePosType = nd.GetHisNodePosType();
                nd.DirectUpdate();
            }

            //  Packet processing jobs .
            string sql = "SELECT HisStas, COUNT(*) as NUM FROM WF_Node WHERE FK_Flow='" + fl.No + "' GROUP BY HisStas";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string stas = dr[0].ToString();
                string nodes = "";
                foreach (Node nd in nds)
                {
                    if (nd.HisStas == stas)
                        nodes += "@" + nd.NodeID;
                }

                foreach (Node nd in nds)
                {
                    if (nodes.Contains("@" + nd.NodeID.ToString()) == false)
                        continue;

                    nd.GroupStaNDs = nodes;
                    nd.DirectUpdate();
                }
            }

            /*  Type judgment process  */
            sql = "SELECT Name FROM WF_Node WHERE (NodeWorkType=" + (int)NodeWorkType.StartWorkFL + " OR NodeWorkType=" + (int)NodeWorkType.WorkFHL + " OR NodeWorkType=" + (int)NodeWorkType.WorkFL + " OR NodeWorkType=" + (int)NodeWorkType.WorkHL + ") AND (FK_Flow='" + fl.No + "')";
            dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            //if (dt.Rows.Count == 0)
            //    fl.HisFlowType = FlowType.Panel;
            //else
            //    fl.HisFlowType = FlowType.FHL;

            fl.DirectUpdate();
            return null;
        }
        protected override bool beforeUpdate()
        {
            if (this.IsStartNode)
            {
                this.SetValByKey(BtnAttr.ReturnRole, (int)ReturnRole.CanNotReturn);
                this.SetValByKey(BtnAttr.ShiftEnable, 0);
                //  this.SetValByKey(BtnAttr.CCRole, 0);
                this.SetValByKey(BtnAttr.EndFlowEnable, 0);
            }

            //给icon Set Default .
            if (this.GetValStrByKey(NodeAttr.ICON)=="")
                this.ICON = "/WF/Data/NodeIcon/ Check .png";


            #region  If the data consolidation mode , Necessary to check whether there is a node in a child thread , If you have a child thread will need a separate table .
            if (this.HisRunModel == RunModel.SubThread)
            {
                MapData md = new MapData("ND" + this.NodeID);
                if (md.PTable != "ND" + this.NodeID)
                {
                    md.PTable = "ND" + this.NodeID;
                    md.Update();
                }
            }
            #endregion  If the data consolidation mode , Necessary to check whether there is a node in a child thread , If you have a child thread will need a separate table .

            // Updated version number .
            Flow.UpdateVer(this.FK_Flow);


            #region  // Get  NEE  Entity .
            //if (string.IsNullOrEmpty(this.NodeMark) == false)
            //{
            //    object obj = Glo.GetNodeEventEntityByNodeMark(fl.FlowMark,this.NodeMark);
            //    if (obj == null)
            //        throw new Exception("@ Node tag error : Did not find the node labeled (" + this.NodeMark + ") Entity nodes event .");
            //    this.NodeEventEntity = obj.ToString();
            //}
            //else
            //{
            //    this.NodeEventEntity = "";
            //}
            #endregion  Synchronization event entity .

            #region  Analyzing mark update process conditions .
            DBAccess.RunSQL("UPDATE WF_Node SET IsCCFlow=0  WHERE FK_Flow='" + this.FK_Flow + "'");
            DBAccess.RunSQL("UPDATE WF_Node SET IsCCFlow=1 WHERE NodeID IN (SELECT NodeID FROM WF_Cond WHERE CondType=1) AND FK_Flow='" + this.FK_Flow + "'");
            #endregion


            Flow fl = new Flow(this.FK_Flow);
            
            Node.CheckFlow(fl);
            this.FlowName = fl.Name;

            DBAccess.RunSQL("UPDATE Sys_MapData SET Name='" + this.Name + "' WHERE No='ND" + this.NodeID + "'");
            switch (this.HisRunModel)
            {
                case RunModel.Ordinary:
                    if (this.IsStartNode)
                        this.HisNodeWorkType = NodeWorkType.StartWork;
                    else
                        this.HisNodeWorkType = NodeWorkType.Work;
                    break;
                case RunModel.FL:
                    if (this.IsStartNode)
                        this.HisNodeWorkType = NodeWorkType.StartWorkFL;
                    else
                        this.HisNodeWorkType = NodeWorkType.WorkFL;
                    break;
                case RunModel.HL:
                    //if (this.IsStartNode)
                    //    throw new Exception("@ You can not set the start node to node confluence .");
                    //else
                    //    this.HisNodeWorkType = NodeWorkType.WorkHL;
                    break;
                case RunModel.FHL:
                    //if (this.IsStartNode)
                    //    throw new Exception("@ You can not set the starting node for the sub-confluent nodes .");
                    //else
                    //    this.HisNodeWorkType = NodeWorkType.WorkFHL;
                    break;
                case RunModel.SubThread:
                    this.HisNodeWorkType = NodeWorkType.SubThreadWork;
                    break;
                default:
                    throw new Exception("eeeee");
                    break;
            }
            return base.beforeUpdate();
        }
        #endregion

        #region  Basic properties 
        /// <summary>
        ///  Internal Number 
        /// </summary>
        public string No
        {
            get
            {
                try
                {
                    return this.NodeID.ToString().Substring(this.NodeID.ToString().Length - 2);
                }
                catch(Exception ex)
                {
                    Log.DefaultLogWriteLineInfo(ex.Message+" - "+this.NodeID);
                    throw new Exception("@ Did not get to it NodeID = "+this.NodeID);
                }
            }
        }
        /// <summary>
        ///  Automatically jump Rules 0- Who is the author treatment 
        /// </summary>
        public bool AutoJumpRole0
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.AutoJumpRole0);
            }
            set
            {
                this.SetValByKey(NodeAttr.AutoJumpRole0, value);
            }
        }
        /// <summary>
        ///  Automatically jump Rules 1- Treatment have occurred 
        /// </summary>
        public bool AutoJumpRole1
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.AutoJumpRole1);
            }
            set
            {
                this.SetValByKey(NodeAttr.AutoJumpRole1, value);
            }
        }
        /// <summary>
        ///  Automatically jump Rules 2- People with the same processing step 
        /// </summary>
        public bool AutoJumpRole2
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.AutoJumpRole2);
            }
            set
            {
                this.SetValByKey(NodeAttr.AutoJumpRole2, value);
            }
        }
        /// <summary>
        ///  Startup Parameters 
        /// </summary>
        public string SubFlowStartParas
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.SubFlowStartParas);
            }
            set
            {
                this.SetValByKey(NodeAttr.SubFlowStartParas, value);
            }
        }
        /// <summary>
        ///  Child thread startup mode 
        /// </summary>
        public SubFlowStartWay SubFlowStartWay
        {
            get
            {
                return (SubFlowStartWay)this.GetValIntByKey(NodeAttr.SubFlowStartWay);
            }
            set
            {
                this.SetValByKey(NodeAttr.SubFlowStartWay, (int)value);
            }
        }

        public NodeFormType HisFormType
        {
            get
            {
                return (NodeFormType)this.GetValIntByKey(NodeAttr.FormType);
            }
            set
            {
                this.SetValByKey(NodeAttr.FormType, (int)value);
            }
        }
        /// <summary>
        /// OID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.NodeID);
            }
            set
            {
                this.SetValByKey(NodeAttr.NodeID, value);
            }
        }
        public bool IsEnableTaskPool
        {
            get
            {
                if (this.TodolistModel == WF.TodolistModel.Sharing)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Node Avatar 
        /// </summary>
        public string ICON
        {
            get
            {
                string s= this.GetValStrByKey(NodeAttr.ICON);
                if (string.IsNullOrEmpty(s))
                    if (this.IsStartNode)
                        return "/WF/Data/NodeIcon/ Check .png";
                    else
                        return "/WF/Data/NodeIcon/ Reception .png";
                return s;
            }
            set
            {
                this.SetValByKey(NodeAttr.ICON, value);
            }
        }
        /// <summary>
        /// FormUrl 
        /// </summary>
        public string FormUrl
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.FormUrl);
            }
            set
            {
                this.SetValByKey(NodeAttr.FormUrl, value);
            }
        }
        public NodeFormType FormType
        {
            get
            {
                return (NodeFormType)this.GetValIntByKey(NodeAttr.FormType);
            }
            set
            {
                this.SetValByKey(NodeAttr.FormType, value);
            }
        }
        
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStrByKey(EntityOIDNameAttr.Name);
            }
            set
            {
                this.SetValByKey(EntityOIDNameAttr.Name, value);
            }
        }
        /// <summary>
        ///  The number of days required （ Deadline ）
        /// </summary>
        public float DeductDays
        {
            get
            {
                float i= this.GetValFloatByKey(NodeAttr.DeductDays);
                if (i == 0)
                    return 1;
                return i;
            }
            set
            {
                this.SetValByKey(NodeAttr.DeductDays, value);
            }
        }
        /// <summary>
        ///  Maximum deduction 
        /// </summary>
        public float MaxDeductCent
        {
            get
            {
                return this.GetValFloatByKey(NodeAttr.MaxDeductCent);
            }
            set
            {
                this.SetValByKey(NodeAttr.MaxDeductCent, value);
            }
        }
        /// <summary>
        ///  Hard highest points 
        /// </summary>
        public float SwinkCent
        {
            get
            {
                return this.GetValFloatByKey(NodeAttr.SwinkCent);
            }
            set
            {
                this.SetValByKey(NodeAttr.SwinkCent, value);
            }
        }
        /// <summary>
        ///  Save mode  @0= Only node table  @1= Node NDxxxRtp表.
        /// </summary>
        public SaveModel SaveModel
        {
            get
            {
                return (SaveModel)this.GetValIntByKey(NodeAttr.SaveModel);
            }
            set
            {
                this.SetValByKey(NodeAttr.SaveModel, (int)value);
            }
        }
        /// <summary>
        ///  Process Steps 
        /// </summary>
        public int Step
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.Step);
            }
            set
            {
                this.SetValByKey(NodeAttr.Step, value);
            }
        }

        /// <summary>
        ///  Deadline (  The number of days required （ Deadline ）+ Warning Days )
        /// </summary>
        public float NeedCompleteDays
        {
            get
            {
                return this.DeductDays;
            }
        }
        /// <summary>
        ///  Deduction rate （分/天）
        /// </summary>
        public float DeductCent
        {
            get
            {
                return this.GetValFloatByKey(NodeAttr.DeductCent);
            }
            set
            {
                this.SetValByKey(NodeAttr.DeductCent, value);
            }
        }
        /// <summary>
        ///  Is the client node is executed ?
        /// </summary>
        public bool IsGuestNode
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsGuestNode);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsGuestNode, value);
            }
        }
        /// <summary>
        ///  Whether it is the start node 
        /// </summary>
        public bool IsStartNode
        {
            get
            {
                if (this.No == "01")
                    return true;
                return false;

                //if (this.HisNodePosType == NodePosType.Start)
                //    return true;
                //else
                //    return false;
            }
        }
        /// <summary>
        /// x
        /// </summary>
        public int X
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.X);
            }
            set
            {
                this.SetValByKey(NodeAttr.X, value);
            }
        }
        public float WarningDays
        {
            get
            {
                if (this.GetValFloatByKey(NodeAttr.WarningDays) == 0)
                    return this.DeductDays;
                else
                    return this.DeductDays - this.GetValFloatByKey(NodeAttr.WarningDays);
            }
        }
        /// <summary>
        /// y
        /// </summary>
        public int Y
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.Y);
            }
            set
            {
                this.SetValByKey(NodeAttr.Y, value);
            }
        }
        /// <summary>
        ///  Water execute it ?
        /// </summary>
        public int WhoExeIt
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.WhoExeIt);
            }
            set
            {
                this.SetValByKey(NodeAttr.WhoExeIt, value);
            }
        }
         
        /// <summary>
        ///  Location 
        /// </summary>
        public NodePosType NodePosType
        {
            get
            {
                return (NodePosType)this.GetValIntByKey(NodeAttr.NodePosType);
            }
            set
            {
                this.SetValByKey(NodeAttr.NodePosType, (int)value);
            }
        }
        /// <summary>
        ///  Run mode 
        /// </summary>
        public RunModel HisRunModel
        {
            get
            {
                return (RunModel)this.GetValIntByKey(NodeAttr.RunModel);
            }
            set
            {
                this.SetValByKey(NodeAttr.RunModel, (int)value);
            }
        }
        /// <summary>
        ///  Focus field 
        /// </summary>
        public string FocusField
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.FocusField);
            }
            set
            {
                this.SetValByKey(NodeAttr.FocusField,value);
            }
        }
        /// <summary>
        ///  Return information field .
        /// </summary>
        public string ReturnField_del
        {
            get
            {
                return this.GetValStrByKey(BtnAttr.ReturnField);
            }
        }
        /// <summary>
        ///  Node of the transaction number 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.FK_Flow);
            }
            set
            {
                SetValByKey(NodeAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Get it on the step of the diversion point 
        /// </summary>
        private Node _GetHisPriFLNode(Nodes nds)
        {
            foreach (Node mynd in nds)
            {
                if (mynd.IsFL)
                    return mynd;
                else
                    return _GetHisPriFLNode(mynd.FromNodes);
            }
            return null;
        }
        /// <summary>
        ///  It a step in the shunt node 
        /// </summary>
        public Node HisPriFLNode
        {
            get
            {
                return _GetHisPriFLNode(this.FromNodes);
            }
        }
        public string TurnToDealDoc
        {
            get
            {
                string s= this.GetValStrByKey(NodeAttr.TurnToDealDoc);
                if (this.HisTurnToDeal == TurnToDeal.SpecUrl)
                {
                    if (s.Contains("?"))
                        s += "&1=1";
                    else
                        s += "?1=1";
                }
                return s;
            }
            set
            {
                SetValByKey(NodeAttr.TurnToDealDoc, value);
            }
        }
        /// <summary>
        ///  Redirect node 
        /// </summary>
        public string JumpToNodes
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.JumpToNodes);
            }
            set
            {
                SetValByKey(NodeAttr.JumpToNodes, value);
            }
        }
        /// <summary>
        ///  Node Form ID
        /// </summary>
        public string NodeFrmID
        {
            get
            {
                string str =this.GetValStrByKey(NodeAttr.NodeFrmID);
                if (string.IsNullOrEmpty(str))
                    return "ND" + this.NodeID;
                return str;
            }
            set
            {
                SetValByKey(NodeAttr.NodeFrmID, value);
            }
        }

        
        public string FlowName
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.FlowName);
            }
            set
            {
                SetValByKey(NodeAttr.FlowName, value);
            }
        }
        /// <summary>
        ///  Printing Methods 
        /// </summary>
        public PrintDocEnable HisPrintDocEnable
        {
            get
            {
                return (PrintDocEnable)this.GetValIntByKey(NodeAttr.PrintDocEnable);
            }
            set
            {
                this.SetValByKey(NodeAttr.PrintDocEnable, (int)value);
            }
        }
        /// <summary>
        ///  Batch Rules 
        /// </summary>
        public BatchRole HisBatchRole
        {
            get
            {
                return (BatchRole)this.GetValIntByKey(NodeAttr.BatchRole);
            }
            set
            {
                this.SetValByKey(NodeAttr.BatchRole, (int)value);
            }
        }
        /// <summary>
        ///  Batch processing rules 
        /// @ Display Fields .
        /// </summary>
        public string BatchParas
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.BatchParas);
            }
            set
            {
                this.SetValByKey(NodeAttr.BatchParas, value);
            }
        }
        public string PTable
        {
            get
            {
                
                return "ND" + this.NodeID;
            }
            set
            {
                SetValByKey(NodeAttr.PTable, value);
            }
        }
        /// <summary>
        ///  To appear in the back of the form 
        /// </summary>
        public string ShowSheets
        {
            get
            {
                string s = this.GetValStrByKey(NodeAttr.ShowSheets);
                if (s == "")
                    return "@";
                return s;
            }
            set
            {
                SetValByKey(NodeAttr.ShowSheets, value);
            }
        }
        /// <summary>
        /// Doc
        /// </summary> 
        public string Doc
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.Doc);
            }
            set
            {
                SetValByKey(NodeAttr.Doc, value);
            }
        }
        public string GroupStaNDs
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.GroupStaNDs);
            }
            set
            {
                this.SetValByKey(NodeAttr.GroupStaNDs, value);
            }
        }
        /// <summary>
        ///  The number of nodes reached .
        /// </summary>
        public int HisToNDNum
        {
            get
            {
                string[] strs = this.HisToNDs.Split('@');
                return strs.Length-1;
            }
        }
        public string HisToNDs
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.HisToNDs);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisToNDs, value);
            }
        }
        public string HisDeptStrs
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.HisDeptStrs);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisDeptStrs, value);
            }
        }
        public string HisStas
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.HisStas);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisStas, value);
            }
        }
        public string HisEmps_del
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.HisEmps);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisEmps, value);
            }
        }
        public string HisBillIDs
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.HisBillIDs);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisBillIDs, value);
            }
        }
        /// <summary>
        ///  Official word on the left 
        /// </summary>
        public string DocLeftWord
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.DocLeftWord);
            }
            set
            {
                this.SetValByKey(NodeAttr.DocLeftWord, value);
            }
        }
        /// <summary>
        ///  Documents on the right words 
        /// </summary>
        public string DocRightWord
        {
            get
            {
                return this.GetValStrByKey(NodeAttr.DocRightWord);
            }
            set
            {
                this.SetValByKey(NodeAttr.DocRightWord, value);
            }
        }
        #endregion

        #region  Extended Attributes 
        /// <summary>
        ///  Is not much post work node .
        /// </summary>
        public bool IsMultiStations
        {
            get
            {
                if (this.NodeStations.Count > 1)
                    return true;
                return false;
            }
        }
        public string HisStationsStr
        {
            get
            {
                string s = "";
                foreach (NodeStation ns in this.NodeStations)
                {
                    s += ns.FK_StationT + ",";
                }
                return s;
            }
        }
        #endregion

        #region  Public Methods 
        /// <summary>
        ///  Get a job data Entity 
        /// </summary>
        /// <param name="workId"> The work ID</param>
        /// <returns> If you do not return null</returns>
        public Work GetWork(Int64 workId)
        {
            Work wk = this.HisWork;
            wk.SetValByKey("OID", workId);
            if (wk.RetrieveFromDBSources() == 0)
                return null;
            else
                return wk;
            return wk;
        }
        #endregion

        #region  Job Type node 
        /// <summary>
        ///  Steering handle 
        /// </summary>
        public TurnToDeal HisTurnToDeal
        {
            get
            {
                return (TurnToDeal)this.GetValIntByKey(NodeAttr.TurnToDeal);
            }
            set
            {
                this.SetValByKey(NodeAttr.TurnToDeal, (int)value);
            }
        }
        /// <summary>
        ///  Access Rules 
        /// </summary>
        public DeliveryWay HisDeliveryWay
        {
            get
            {
                return (DeliveryWay)this.GetValIntByKey(NodeAttr.DeliveryWay);
            }
            set
            {
                this.SetValByKey(NodeAttr.DeliveryWay, (int)value);
            }
        }
        /// <summary>
        ///  CC rules 
        /// </summary>
        public CCRole HisCCRole
        {
            get
            {
                return (CCRole)this.GetValIntByKey(NodeAttr.CCRole);
            }
        }
        /// <summary>
        ///  Delete process rules 
        /// </summary>
        public DelWorkFlowRole HisDelWorkFlowRole
        {
            get
            {
                return (DelWorkFlowRole)this.GetValIntByKey(BtnAttr.DelEnable);
            }
        }
        /// <summary>
        ///  Manner of man is not found 
        /// </summary>
        public WhenNoWorker HisWhenNoWorker
        {
            get
            {
                return (WhenNoWorker)this.GetValIntByKey(NodeAttr.WhenNoWorker);
            }
            set
            {
                this.SetValByKey(NodeAttr.WhenNoWorker, (int)value);
            }
        }
         /// <summary>
        ///  Avoidance rules 
        /// </summary>
        public CancelRole HisCancelRole
        {
            get
            {
                return (CancelRole)this.GetValIntByKey(NodeAttr.CancelRole);
            }
            set
            {
                this.SetValByKey(NodeAttr.CancelRole, (int)value);
            }
        }
        
        /// <summary>
        ///  Data written to the rules 
        /// </summary>
        public CCWriteTo CCWriteTo
        {
            get
            {
                return (CCWriteTo)this.GetValIntByKey(NodeAttr.CCWriteTo);
            }
            set
            {
                this.SetValByKey(NodeAttr.CCWriteTo, (int)value);
            }
        }
       
        /// <summary>
        /// Int type
        /// </summary>
        public NodeWorkType HisNodeWorkType
        {
            get
            {
#warning 2012-01-24 Revise , Not automatically calculated properties .
                switch (this.HisRunModel)
                {
                    case RunModel.Ordinary:
                        if (this.IsStartNode)
                            return NodeWorkType.StartWork;
                        else
                            return NodeWorkType.Work;
                    case RunModel.FL:
                        if (this.IsStartNode)
                            return NodeWorkType.StartWorkFL;
                        else
                            return NodeWorkType.WorkFL;
                    case RunModel.HL:
                            return NodeWorkType.WorkHL;
                    case RunModel.FHL:
                        return NodeWorkType.WorkFHL;
                    case RunModel.SubThread:
                        return NodeWorkType.SubThreadWork;
                    default:
                        throw new Exception("@ Does not determine the type of NodeWorkType.");
                }
            }
            set
            {
                this.SetValByKey(NodeAttr.NodeWorkType, (int)value);
            }
        }
        public string HisNodeWorkTypeT
        {
            get
            {
                return this.HisNodeWorkType.ToString();

                //Sys.SysEnum se = new Sys.SysEnum(NodeAttr.NodeWorkType, this.GetValIntByKey(NodeAttr.NodeWorkType));
                //return se.Lab;
            }
        }
        #endregion

        #region  Projected property  ( For the determination node location )
       
        /// <summary>
        ///  Type 
        /// </summary>
        public NodePosType HisNodePosType
        {
            get
            {
                if (SystemConfig.IsDebug)
                {
                    this.SetValByKey(NodeAttr.NodePosType, (int)this.GetHisNodePosType());
                    return this.GetHisNodePosType();
                }
                return (NodePosType)this.GetValIntByKey(NodeAttr.NodePosType);
            }
            set
            {
                if (value == NodePosType.Start)
                    if (this.No != "01")
                        value = NodePosType.Mid;

                this.SetValByKey(NodeAttr.NodePosType, (int)value);
            }
        }
        /// <summary>
        ///  Is not the end node 
        /// </summary>
        public bool IsEndNode
        {
            get
            {
                if (this.HisNodePosType == NodePosType.End)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Whether to allow the child to accept the staff duplicate threads ( Pair threads valid point )?
        /// </summary>
        public bool IsAllowRepeatEmps
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsAllowRepeatEmps);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsAllowRepeatEmps, value);
            }
        }
        /// <summary>
        ///  Can backtrack after return ?
        /// </summary>
        public bool IsBackTracking
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsBackTracking);
            }
        }
        /// <summary>
        ///  Whether automatic memory function is enabled 
        /// </summary>
        public bool IsRememberMe
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsRM);
            }
        }
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public bool IsCanDelFlow
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCanDelFlow);
            }
        }

        /// <summary>
        ///  Ordinary working node processing mode 
        /// </summary>
        public TodolistModel TodolistModel
        {
            get
            {
                return (TodolistModel)this.GetValIntByKey(NodeAttr.TodolistModel);
            }
        }
        /// <summary>
        ///  Child thread delete rules 
        /// </summary>
        public ThreadKillRole ThreadKillRole
        {
            get
            {
                return (ThreadKillRole)this.GetValIntByKey(NodeAttr.ThreadKillRole);
            }
        }
        /// <summary>
        ///  Whether confidential step 
        /// </summary>
        public bool IsSecret
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsSecret);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsSecret, value);
            }
        }
        public decimal PassRate
        {
            get
            {
                return this.GetValDecimalByKey(NodeAttr.PassRate);
            }
            set
            {
                this.SetValByKey(NodeAttr.PassRate, value);
            }  
        }
        /// <summary>
        ///  Whether to allow the distribution of work 
        /// </summary>
        public bool IsTask
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsTask);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsTask, value);
            }
        }
        public bool IsCanOver
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCanOver);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsCanOver, value);
            }
        }
        public bool IsCanRpt
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCanRpt);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsCanRpt, value);
            }
        }
        /// <summary>
        ///  Can I transfer 
        /// </summary>
        public bool IsHandOver
        {
            get
            {
                if (this.IsStartNode)
                    return false;

                return this.GetValBooleanByKey(NodeAttr.IsHandOver);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsHandOver, value);
            }
        }
        public bool IsCanHidReturn_del
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCanHidReturn);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsCanHidReturn, value);
            }
        }
        public bool IsCanReturn
        {
            get
            {
                if (this.HisReturnRole == ReturnRole.CanNotReturn)
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Read Receipts 
        /// </summary>
        public ReadReceipts ReadReceipts
        {
            get
            {
                return (ReadReceipts)this.GetValIntByKey(NodeAttr.ReadReceipts);
            }
            set
            {
                this.SetValByKey(NodeAttr.ReadReceipts, (int)value);
            }
        }
        /// <summary>
        ///  Return rules 
        /// </summary>
        public ReturnRole HisReturnRole
        {
            get
            {
                return (ReturnRole)this.GetValIntByKey(NodeAttr.ReturnRole);
            }
            set
            {
                this.SetValByKey(NodeAttr.ReturnRole, (int)value);
            }
        }
        /// <summary>
        ///  Is not an intermediate node 
        /// </summary>
        public bool IsMiddleNode
        {
            get
            {
                if (this.HisNodePosType == NodePosType.Mid)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Is the quality assessment point 
        /// </summary>
        public bool IsEval
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsEval);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsEval, value);
            }
        }
        public string HisSubFlows
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.HisSubFlows);
            }
            set
            {
                this.SetValByKey(NodeAttr.HisSubFlows, value);
            }
        }
        public string FrmAttr
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.FrmAttr);
            }
            set
            {
                this.SetValByKey(NodeAttr.FrmAttr, value);
            }
        }
     
        /// <summary>
        ///  Whether there are sub-processes 
        /// </summary>
        public bool IsHaveSubFlow
        {
            get
            {
                if (this.HisSubFlows.Length > 2)
                    return true;
                else
                    return false;
            }
        }
        public bool IsHL
        {
            get
            {
                switch (this.HisNodeWorkType)
                {
                    case NodeWorkType.WorkHL:
                    case NodeWorkType.WorkFHL:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        ///  Is shunt 
        /// </summary>
        public bool IsFL
        {
            get
            {
                switch (this.HisNodeWorkType)
                {
                    case NodeWorkType.WorkFL:
                    case NodeWorkType.WorkFHL:
                    case NodeWorkType.StartWorkFL:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        ///  Whether streaming confluence 
        /// </summary>
        public bool IsFLHL
        {
            get
            {
                switch (this.HisNodeWorkType)
                {
                    case NodeWorkType.WorkHL:
                    case NodeWorkType.WorkFL:
                    case NodeWorkType.WorkFHL:
                    case NodeWorkType.StartWorkFL:
                        return true;
                    default:
                        return false;
                }
            }
        }
        /// <summary>
        ///  Are there conditions to complete the process 
        /// </summary>
        public bool IsCCFlow
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.IsCCFlow);
            }
            set
            {
                this.SetValByKey(NodeAttr.IsCCFlow, value);
            }
        }
        /// <summary>
        ///  Recipient sql
        /// </summary>
        public string DeliveryParas
        {
            get
            {
                string s= this.GetValStringByKey(NodeAttr.DeliveryParas);
                s = s.Replace("~", "'");
                return s;
            }
            set
            {
                this.SetValByKey(NodeAttr.DeliveryParas, value);
            }
        }
        /// <summary>
        ///  Is not it PC Work node 
        /// </summary>
        public bool IsPCNode
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        ///  Nature of the work 
        /// </summary>
        public string NodeWorkTypeText
        {
            get
            {
                return this.HisNodeWorkType.ToString();
            }
        }
        #endregion

        #region  Public Methods  ( After the user to perform actions , Work to be done )
        /// <summary>
        ///  After the user to perform actions , Work to be done 		 
        /// </summary>
        /// <returns> Return message , Run news </returns>
        public string AfterDoTask()
        {
            return "";
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Node 
        /// </summary>
        public Node() { }
        /// <summary>
        ///  Node 
        /// </summary>
        /// <param name="_oid"> Node ID</param>	
        public Node(int _oid)
        {
            this.NodeID = _oid;
            if (SystemConfig.IsDebug)
            {
                if (this.RetrieveFromDBSources() <= 0)
                    throw new Exception("Node Retrieve  No error ID=" + _oid);
            }
            else
            {
                //  Remove cache .
                this.RetrieveFromDBSources();
                //if (this.Retrieve() <= 0)
                //    throw new Exception("Node Retrieve  No error ID=" + _oid);
            }
        }
        public Node(string ndName)
        {
            ndName = ndName.Replace("ND", "");
            this.NodeID = int.Parse(ndName);

            if (SystemConfig.IsDebug)
            {
                if (this.RetrieveFromDBSources() <= 0)
                    throw new Exception("Node Retrieve  No error ID=" + ndName);
            }
            else
            {
                if (this.Retrieve() <= 0)
                    throw new Exception("Node Retrieve  No error ID=" + ndName);
            }
        }
        public string EnName
        {
            get
            {
                return "ND" + this.NodeID;
            }
        }
        public string EnsName
        {
            get
            {
                return "ND" + this.NodeID + "s";
            }
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
                map.EnDesc = " Node "; // " Node ";

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                #region  Basic properties .
                map.AddTBIntPK(NodeAttr.NodeID, 0, " Node ID", true, true);
                map.AddTBString(NodeAttr.Name, null, " Name ", true, false, 0, 100, 10);
                map.AddTBInt(NodeAttr.Step, (int)NodeWorkType.Work,  " Process Steps ", true, false);

                // Head portrait . "/WF/Data/NodeIcon/ Check .png"  "/WF/Data/NodeIcon/ Reception .png"
                map.AddTBString(NodeAttr.ICON, null, " Node ICON Picture Path ", true, false, 0, 200, 10);

                map.AddTBInt(NodeAttr.NodeWorkType, 0, " Node Type ", false, false);
                map.AddTBInt(NodeAttr.SubThreadType, 0, " Child thread ID", false, false);

                map.AddTBString(NodeAttr.FK_Flow, null, "FK_Flow", false, false, 0, 3, 10);
                map.AddTBInt(NodeAttr.IsGuestNode, 0, " Is the client node is executed ", false, false);

                map.AddTBString(NodeAttr.FlowName, null, " Process name ", false, true, 0, 100, 10);
                map.AddTBString(NodeAttr.FK_FlowSort, null, "FK_FlowSort", false, true, 0, 4, 10);
                map.AddTBString(NodeAttr.FK_FlowSortT, null, "FK_FlowSortT", false, true, 0, 100, 10);
                map.AddTBString(NodeAttr.FrmAttr, null, "FrmAttr", false, true, 0, 300, 10);
                #endregion  Basic properties .

                #region  Property assessment .
                map.AddTBFloat(NodeAttr.WarningDays, 0,  " Warning period (0 Without warning )", false, false); // " Warning period (0 Without warning )"
                map.AddTBFloat(NodeAttr.DeductDays, 1, " Deadline (days)", false, false); //" Deadline (天)"
                map.AddTBFloat(NodeAttr.DeductCent, 2, " Deduction ( Each extension 1 Days buckle )", false, false); //" Deduction ( Each extension 1 Days buckle )"
                map.AddTBFloat(NodeAttr.MaxDeductCent, 10, " Maximum deduction ", false, false); //" Maximum deduction "
                map.AddTBFloat(NodeAttr.SwinkCent, float.Parse("0.1"), " Working score ", false, false); //" Working score "
                #endregion  Property assessment .


                map.AddTBString(NodeAttr.Doc, null, " Description ", true, false, 0, 100, 10);
                map.AddBoolean(NodeAttr.IsTask, true, " No work permit allocation ?", true, true);

                map.AddTBInt(NodeAttr.ReturnRole, 2, " Return rules ", true, true);
                map.AddTBInt(NodeAttr.DeliveryWay, 0, " Access Rules ", true, true);
                map.AddTBInt(NodeAttr.CancelRole, 0, " Avoidance rules ", true, true);

                map.AddTBInt(NodeAttr.WhenNoWorker, 0, " When dealing with people not found ", true, true);
                map.AddTBString(NodeAttr.DeliveryParas, null, " Access Rule ", true, false, 0, 500, 10);
                map.AddTBString(NodeAttr.NodeFrmID, null, " Node Form ID", true, false, 0, 200, 10);

                map.AddTBInt(NodeAttr.CCRole, 0, " CC rules ", true, true);
                map.AddTBInt(NodeAttr.CCWriteTo, 0, " CC data is written to the rules ", true, true);

                map.AddTBInt(BtnAttr.DelEnable, 0, " Delete Rule ", true, true);
                map.AddTBInt(NodeAttr.IsEval, 0, " Whether the quality assessment ", true, true);
                map.AddTBInt(NodeAttr.SaveModel, 0, " Save mode ", true, true);


                map.AddTBInt(NodeAttr.IsCanRpt, 1, " Can I view Report ?", true, true);
                map.AddTBInt(NodeAttr.IsCanOver, 0, " Whether the process can be terminated ", true, true);
                map.AddTBInt(NodeAttr.IsSecret, 0, " Is secrecy step ", true, true);
                map.AddTBInt(NodeAttr.IsCanDelFlow, 0, " Can I delete process ", true, true);

                map.AddTBInt(NodeAttr.ThreadKillRole, 0, " Delete the way the child thread ", true, true);
                map.AddTBInt(NodeAttr.TodolistModel, 0, " Is the queue node ", true, true);

                map.AddTBInt(NodeAttr.IsAllowRepeatEmps, 0, " Whether to allow the child to accept the staff duplicate threads ( Pair threads valid point )?", true, true);
                map.AddTBInt(NodeAttr.IsBackTracking, 0, " Can backtrack after return ( Only enable the return function is effective )", true, true);
                map.AddTBInt(NodeAttr.IsRM, 1, " Whether automatic memory function is enabled the delivery path ?", true, true);
                map.AddBoolean(NodeAttr.IsHandOver, false, " Can I transfer ", true, true);
                map.AddTBInt(NodeAttr.PassRate, 100, " By Rate ", true, true);
                map.AddTBInt(NodeAttr.RunModel, 0, " Run mode ( Effective for ordinary nodes )", true, true);
              

                map.AddTBInt(NodeAttr.WhoExeIt, 0, " Who performed it ", true, true);
                map.AddTBInt(NodeAttr.ReadReceipts, 0, " Read Receipts ", true, true);
                map.AddTBInt(NodeAttr.CondModel, 0, " Conditions direction control rules ", true, true);

                //  Automatically jump .
                map.AddTBInt(NodeAttr.AutoJumpRole0, 0, " Who is the author treatment 0", false, false);
                map.AddTBInt(NodeAttr.AutoJumpRole1, 0, " Treatment have occurred 1", false, false);
                map.AddTBInt(NodeAttr.AutoJumpRole2, 0, " People with the same processing step 2", false, false);

                //  Batch .
                map.AddTBInt(NodeAttr.BatchRole, 0, " Batch ", true, true);
                map.AddTBString(NodeAttr.BatchParas, null, " Parameters ", true, false, 0, 100, 10);
                map.AddTBInt(NodeAttr.PrintDocEnable, 0, " Printing Methods ", true, true);
                

                // The default form for freedom .
                map.AddTBInt(NodeAttr.OutTimeDeal, 0, " Timeout handling ", false, false);
                map.AddTBString(NodeAttr.DoOutTime, null, " Timeout processing content ", true, false, 0, 300, 10, true);

                map.AddTBInt(NodeAttr.FormType, 1, " Form type ", false, false);
                map.AddTBString(NodeAttr.FormUrl, "http://", " Form URL", true, false, 0, 200, 10);
                map.AddTBString(NodeAttr.DeliveryParas, null, " Recipient SQL", true, false, 0, 300, 10, true);
                map.AddTBInt(NodeAttr.TurnToDeal, 0, " Steering handle ", false, false);
                map.AddTBString(NodeAttr.TurnToDealDoc, null, " After sending the message ", true, false, 0, 1000, 10, true);
                map.AddTBInt(NodeAttr.NodePosType, 0, " Location ", false, false);
                map.AddTBInt(NodeAttr.IsCCFlow, 0, " Are there conditions to complete the process ", false, false);
                map.AddTBString(NodeAttr.HisStas, null, " Post ", false, false, 0, 1000, 10);
                map.AddTBString(NodeAttr.HisDeptStrs, null, " Department ", false, false, 0, 1000, 10);
                map.AddTBString(NodeAttr.HisToNDs, null, " Go to the node ", false, false, 0, 100, 10);
                map.AddTBString(NodeAttr.HisBillIDs, null, " Invoice IDs", false, false, 0, 200, 10);
              //  map.AddTBString(NodeAttr.HisEmps, null, "HisEmps", false, false, 0, 3000, 10);
                map.AddTBString(NodeAttr.HisSubFlows, null, "HisSubFlows", false, false, 0, 200, 10);
                map.AddTBString(NodeAttr.PTable, null, " Physical table ", false, false, 0, 100, 10);

                map.AddTBString(NodeAttr.ShowSheets, null, " Form display ", false, false, 0, 100, 10);
                map.AddTBString(NodeAttr.GroupStaNDs, null, " Post grouping node ", false, false, 0, 200, 10);
                map.AddTBInt(NodeAttr.X, 0, "X Coordinate ", false, false);
                map.AddTBInt(NodeAttr.Y, 0, "Y Coordinate ", false, false);

                map.AddTBString(NodeAttr.FocusField, null, " Focus field ", false, false, 0, 30, 10);
                map.AddTBString(NodeAttr.JumpToNodes, null, " Redirect node ", true, false, 0, 200, 10, true);

                // Button control section .
               // map.AddTBString(BtnAttr.ReturnField, "", " Return information to fill in the fields ", true, false, 0, 200, 10, true);
                map.AddTBAtParas(500);

                //  Promoter thread parameters  2013-01-04
                map.AddTBInt(NodeAttr.SubFlowStartWay, 0, " Child thread startup mode ", true, false);
                map.AddTBString(NodeAttr.SubFlowStartParas, null, " Startup Parameters ", true, false, 0, 100, 10);
                 
                #region  Sons process property .
                map.AddTBInt(NodeAttr.IsCheckSubFlowOver, 0, "( When the current node of the parent process ) Check whether the child process is complete after the parent process to perform ", true, true);
                #endregion  Sons process property .

                map.AddTBString(NodeAttr.DocLeftWord, null, " Official word on the left ( Multiple use @ Accord separated )", true, false, 0, 200, 10);
                map.AddTBString(NodeAttr.DocRightWord, null, " Documents on the right words ( Multiple use @ Accord separated )", true, false, 0, 200, 10);

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  I can handle it the current node ?
        /// </summary>
        /// <returns></returns>
        public bool CanIdoIt()
        {
            return false;
        }
        #endregion

        /// <summary>
        ///  Delete logic before .
        /// </summary>
        /// <returns></returns>
        protected override bool beforeDelete()
        {
            // Determining whether can be removed . 
             int num = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM WF_GenerWorkerlist WHERE FK_Node="+this.NodeID);
             if (num != 0)
                throw new Exception("@ The node ["+this.NodeID+","+this.Name+"] The existence of pending work to do , You can not delete it .");

            //  Delete its node .
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = "ND" + this.NodeID;
            md.Delete();

            //  Delete grouping .
            BP.Sys.GroupFields gfs = new BP.Sys.GroupFields();
            gfs.Delete(BP.Sys.GroupFieldAttr.EnName, md.No);

            // Remove its details .
            BP.Sys.MapDtls dtls = new BP.Sys.MapDtls(md.No);
            dtls.Delete();

            // Delete framework 
            BP.Sys.MapFrames frams = new BP.Sys.MapFrames(md.No);
            frams.Delete();

            //  Delete multiple choice 
            BP.Sys.MapM2Ms m2ms = new BP.Sys.MapM2Ms(md.No);
            m2ms.Delete();

            //  Remove extensions 
            BP.Sys.MapExts exts = new BP.Sys.MapExts(md.No);
            exts.Delete();

            // Delete node corresponding positions .
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_NodeStation WHERE FK_Node=" + this.NodeID);
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_NodeEmp  WHERE FK_Node=" + this.NodeID);
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_NodeDept WHERE FK_Node=" + this.NodeID);
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_NodeFlow WHERE FK_Node=" + this.NodeID);
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_FrmNode  WHERE FK_Node=" + this.NodeID);
            BP.DA.DBAccess.RunSQL("DELETE FROM WF_CCEmp  WHERE FK_Node=" + this.NodeID);
            return base.beforeDelete();
        }
        /// <summary>
        ///  Instruments Process 
        /// </summary>
        /// <param name="md"></param>
        private void AddDocAttr(BP.Sys.MapData md)
        {
            /* If the documentation process ? */
            BP.Sys.MapAttr attr = new BP.Sys.MapAttr();

            //attr = new BP.Sys.MapAttr();
            //attr.FK_MapData = md.No;
            //attr.HisEditType = BP.En.EditType.UnDel;
            //attr.KeyOfEn = "Title";
            //attr.Name = " Title ";
            //attr.MyDataType = BP.DA.DataType.AppString;
            //attr.UIContralType = UIContralType.TB;
            //attr.LGType = FieldTypeS.Normal;
            //attr.UIVisible = true;
            //attr.UIIsEnable = true;
            //attr.MinLen = 0;
            //attr.MaxLen = 300;
            //attr.IDX = 1;
            //attr.UIIsLine = true;
            //attr.IDX = -100;
            //attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "KeyWord";
            attr.Name = " MeSH ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.UIIsLine = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = -99;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.Insert();


            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "FZ";
            attr.Name = " NOTES ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.UIIsLine = true;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.IDX = -98;
            attr.Insert();


            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = "DW_SW";
            attr.Name = " Received unit ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.UIIsLine = true;
            attr.IDX = 1;
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "DW_FW";
            attr.Name = " Units issued ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = true;
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "DW_BS";
            attr.Name = " Main News Unit ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = true;
            attr.Insert();


            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "DW_CS";
            attr.Name = " Send a copy Unit ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = true;
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "NumPrint";
            attr.Name = " Printed copies ";
            attr.MyDataType = BP.DA.DataType.AppInt;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 10;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = false;
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "JMCD";
            attr.Name = " Confidential degree ";
            attr.MyDataType = BP.DA.DataType.AppInt;
            attr.UIContralType = UIContralType.DDL;
            attr.LGType = FieldTypeS.Enum;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = false;
            attr.UIBindKey = "JMCD";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = "PRI";
            attr.Name = " Urgency ";
            attr.MyDataType = BP.DA.DataType.AppInt;
            attr.UIContralType = UIContralType.DDL;
            attr.LGType = FieldTypeS.Enum;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.UIIsLine = false;
            attr.UIBindKey = "PRI";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "GWWH";
            attr.Name = " Document Symbol ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = true;
            attr.UIIsEnable = true;
            attr.MinLen = 0;
            attr.MaxLen = 300;
            attr.IDX = 1;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.UIIsLine = false;
            attr.Insert();
        }
        /// <summary>
        ///  Repair map
        /// </summary>
        public string RepareMap()
        {
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = "ND" + this.NodeID;
            if (md.RetrieveFromDBSources() == 0)
            {
                this.CreateMap();
                return "";
            }

            BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
            if (attr.IsExit(MapAttrAttr.KeyOfEn, "OID", MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr.FK_MapData = md.No;
                attr.KeyOfEn = "OID";
                attr.Name = "WorkID";
                attr.MyDataType = BP.DA.DataType.AppInt;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "0";
                attr.HisEditType = BP.En.EditType.Readonly;
                attr.Insert();
            }

            if (attr.IsExit(MapAttrAttr.KeyOfEn, "FID", MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.KeyOfEn = "FID";
                attr.Name = "FID";
                attr.MyDataType = BP.DA.DataType.AppInt;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.DefVal = "0";
                attr.Insert();
            }

            if (attr.IsExit(MapAttrAttr.KeyOfEn, WorkAttr.RDT, MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = WorkAttr.RDT;
                attr.Name = " Accept time ";  //" Accept time ";
                attr.MyDataType = BP.DA.DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.Tag = "1";
                attr.Insert();
            }

            if (attr.IsExit(MapAttrAttr.KeyOfEn, WorkAttr.CDT, MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = WorkAttr.CDT;
                if (this.IsStartNode)
                    attr.Name = " Start Time "; //" Start Time ";
                else
                    attr.Name = " Completion Time "; //" Completion Time ";

                attr.MyDataType = BP.DA.DataType.AppDateTime;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.DefVal = "@RDT";
                attr.Tag = "1";
                attr.Insert();
            }


            if (attr.IsExit(MapAttrAttr.KeyOfEn, WorkAttr.Rec, MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = WorkAttr.Rec;
                if (this.IsStartNode == false)
                    attr.Name = " Record people "; // " Record people ";
                else
                    attr.Name = " Sponsor "; //" Sponsor ";

                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MaxLen = 20;
                attr.MinLen = 0;
                attr.DefVal = "@WebUser.No";
                attr.Insert();
            }


            if (attr.IsExit(MapAttrAttr.KeyOfEn, WorkAttr.Emps, MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = WorkAttr.Emps;
                attr.Name = WorkAttr.Emps;
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MaxLen = 400;
                attr.MinLen = 0;
                attr.Insert();
            }

            if (attr.IsExit(MapAttrAttr.KeyOfEn, StartWorkAttr.FK_Dept, MapAttrAttr.FK_MapData, md.No) == false)
            {
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = StartWorkAttr.FK_Dept;
                attr.Name = " Operator department "; //" Operator department ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.DDL;
                attr.LGType = FieldTypeS.FK;
                attr.UIBindKey = "BP.Port.Depts";
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 20;
                attr.Insert();
            }

            Flow fl = new Flow(this.FK_Flow);
            if (fl.IsMD5
                && attr.IsExit(MapAttrAttr.KeyOfEn, WorkAttr.MD5, MapAttrAttr.FK_MapData, md.No) == false)
            {
                /*  In the case of MD5 Encryption process . */
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = StartWorkAttr.MD5;
                attr.UIBindKey = attr.KeyOfEn;
                attr.Name = "MD5";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIIsEnable = false;
                attr.UIIsLine = false;
                attr.UIVisible = false;
                attr.MinLen = 0;
                attr.MaxLen = 40;
                attr.IDX = -100;
                attr.Insert();
            }

            if (this.NodePosType == NodePosType.Start)
            {

                if (Glo.IsEnablePRI && this.IsStartNode
                    && attr.IsExit(MapAttrAttr.KeyOfEn, StartWorkAttr.PRI, MapAttrAttr.FK_MapData, md.No) == false)
                {
                    /*  If there is a priority  */
                    attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = md.No;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = StartWorkAttr.PRI;
                    attr.UIBindKey = attr.KeyOfEn;
                    attr.Name = " Priority ";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.DDL;
                    attr.LGType = FieldTypeS.Enum;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = false;
                    attr.MinLen = 0;
                    attr.MaxLen = 200;
                    attr.IDX = -100;
                    attr.DefVal = "0";
                    attr.X = (float)174.76;
                    attr.Y = (float)56.19;
                    attr.Insert();
                }

                
                if (attr.IsExit(MapAttrAttr.KeyOfEn, StartWorkAttr.Title, MapAttrAttr.FK_MapData, md.No) == false)
                {
                    attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = md.No;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = StartWorkAttr.Title;
                    attr.Name = " Title "; // " Process title ";
                    attr.MyDataType = BP.DA.DataType.AppString;
                    attr.UIContralType = UIContralType.TB;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = true;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = true;
                    attr.UIWidth = 251;

                    attr.MinLen = 0;
                    attr.MaxLen = 200;
                    attr.IDX = -100;
                    attr.X = (float)171.2;
                    attr.Y = (float)68.4;
                    attr.Insert();
                }

                //if (attr.IsExit(MapAttrAttr.KeyOfEn, "faqiren", MapAttrAttr.FK_MapData, md.No) == false)
                //{
                //    attr = new BP.Sys.MapAttr();
                //    attr.FK_MapData = md.No;
                //    attr.HisEditType = BP.En.EditType.Edit;
                //    attr.KeyOfEn = "faqiren";
                //    attr.Name = " Sponsor "; // " Sponsor ";
                //    attr.MyDataType = BP.DA.DataType.AppString;
                //    attr.UIContralType = UIContralType.TB;
                //    attr.LGType = FieldTypeS.Normal;
                //    attr.UIVisible = true;
                //    attr.UIIsEnable = false;
                //    attr.UIIsLine = false;
                //    attr.MinLen = 0;
                //    attr.MaxLen = 200;
                //    attr.IDX = -100;
                //    attr.DefVal = "@WebUser.No";
                //    attr.X = (float)159.2;
                //    attr.Y = (float)102.8;
                //    attr.Insert();
                //}

                //if (attr.IsExit(MapAttrAttr.KeyOfEn, "faqishijian", MapAttrAttr.FK_MapData, md.No) == false)
                //{
                //    attr = new BP.Sys.MapAttr();
                //    attr.FK_MapData = md.No;
                //    attr.HisEditType = BP.En.EditType.Edit;
                //    attr.KeyOfEn = "faqishijian";
                //    attr.Name = " Start Time "; //" Start Time ";
                //    attr.MyDataType = BP.DA.DataType.AppDateTime;
                //    attr.UIContralType = UIContralType.TB;
                //    attr.LGType = FieldTypeS.Normal;
                //    attr.UIVisible = true;
                //    attr.UIIsEnable = false;
                //    attr.DefVal = "@RDT";
                //    attr.Tag = "1";
                //    attr.X = (float)324;
                //    attr.Y = (float)102.8;
                //    attr.Insert();
                //}


                if (attr.IsExit(MapAttrAttr.KeyOfEn, "FK_NY", MapAttrAttr.FK_MapData, md.No) == false)
                {
                    attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = md.No;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = "FK_NY";
                    attr.Name = " Years "; //" Years ";
                    attr.MyDataType = BP.DA.DataType.AppString;
                    attr.UIContralType = UIContralType.TB;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.LGType = FieldTypeS.Normal;
                    //attr.UIBindKey = "BP.Pub.NYs";
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.MinLen = 0;
                    attr.MaxLen = 7;
                    attr.Insert();
                }


                if (attr.IsExit(MapAttrAttr.KeyOfEn, "MyNum", MapAttrAttr.FK_MapData, md.No) == false)
                {
                    attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = md.No;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = "MyNum";
                    attr.Name = " The number of "; // " The number of ";
                    attr.DefVal = "1";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.TB;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.Insert();
                }
            }
            string msg = "";
            if (this.FocusField != "")
            {
                if (attr.IsExit(MapAttrAttr.KeyOfEn, this.FocusField, MapAttrAttr.FK_MapData, md.No) == false)
                {
                    msg += "@ Focus field  " + this.FocusField + "  Been illegally removed .";
                    //this.FocusField = "";
                    //this.DirectUpdate();
                }
            }
            return msg;
        }
        /// <summary>
        ///  Set up map
        /// </summary>
        public void CreateMap()
        {
            // Create a node form .
            BP.Sys.MapData md = new BP.Sys.MapData();
            md.No = "ND" + this.NodeID;
            md.Delete();

            md.Name = this.Name;
            if (this.HisFlow.HisDataStoreModel == DataStoreModel.SpecTable)
                md.PTable = this.HisFlow.PTable;
            md.Insert();

            BP.Sys.MapAttr attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "OID";
            attr.Name = "WorkID";
            attr.MyDataType = BP.DA.DataType.AppInt;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.DefVal = "0";
            attr.HisEditType = BP.En.EditType.Readonly;
            attr.Insert();

            if (this.HisFlow.FlowAppType == FlowAppType.DocFlow)
            {
                this.AddDocAttr(md);
            }

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.KeyOfEn = "FID";
            attr.Name = "FID";
            attr.MyDataType = BP.DA.DataType.AppInt;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.DefVal = "0";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = WorkAttr.RDT;
            attr.Name = " Accept time ";  //" Accept time ";
            attr.MyDataType = BP.DA.DataType.AppDateTime;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.Tag = "1";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = WorkAttr.CDT;
            if (this.IsStartNode)
                attr.Name = " Start Time "; //" Start Time ";
            else
                attr.Name = " Completion Time "; //" Completion Time ";

            attr.MyDataType = BP.DA.DataType.AppDateTime;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.DefVal = "@RDT";
            attr.Tag = "1";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = WorkAttr.Rec;
            if (this.IsStartNode == false)
                attr.Name = " Record people "; // " Record people ";
            else
                attr.Name = " Sponsor "; //" Sponsor ";

            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.MaxLen = 20;
            attr.MinLen = 0;
            attr.DefVal = "@WebUser.No";
            attr.Insert();

            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = WorkAttr.Emps;
            attr.Name = "Emps";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.MaxLen = 400;
            attr.MinLen = 0;
            attr.Insert();


            attr = new BP.Sys.MapAttr();
            attr.FK_MapData = md.No;
            attr.HisEditType = BP.En.EditType.UnDel;
            attr.KeyOfEn = StartWorkAttr.FK_Dept;
            attr.Name = " Operator department "; //" Operator department ";
            attr.MyDataType = BP.DA.DataType.AppString;
            attr.UIContralType = UIContralType.TB;
            attr.LGType = FieldTypeS.Normal;
         //   attr.UIBindKey = "BP.Port.Depts";
            attr.UIVisible = false;
            attr.UIIsEnable = false;
            attr.MinLen = 0;
            attr.MaxLen = 32;
            attr.Insert();

            if (this.NodePosType == NodePosType.Start)
            {
                // Start node information .
                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.Edit;
             //   attr.edit
                attr.KeyOfEn = "Title";
                attr.Name = " Title "; // " Process title ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.UIIsLine = true;
                attr.UIWidth = 251;

                attr.MinLen = 0;
                attr.MaxLen = 200;
                attr.IDX = -100;
                attr.X = (float)174.83;
                attr.Y = (float)54.4;
                attr.Insert();

                if (Glo.IsEnablePRI)
                {
                    /*  If there is a priority  */
                    attr = new BP.Sys.MapAttr();
                    attr.FK_MapData = md.No;
                    attr.HisEditType = BP.En.EditType.UnDel;
                    attr.KeyOfEn = "PRI";
                    attr.UIBindKey = attr.KeyOfEn;
                    attr.Name = " Priority ";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.DDL;
                    attr.LGType = FieldTypeS.Enum;
                    attr.UIIsEnable = true;
                    attr.UIIsLine = false;
                    attr.MinLen = 0;
                    attr.MaxLen = 200;
                    attr.IDX = -100;
                    attr.DefVal = "2";
                    attr.X = (float)174.76;
                    attr.Y = (float)56.19;
                    attr.Insert();
                }


                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = "FK_NY";
                attr.Name = " Years "; //" Years ";
                attr.MyDataType = BP.DA.DataType.AppString;
                attr.UIContralType = UIContralType.TB;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.LGType = FieldTypeS.Normal;
                //attr.UIBindKey = "BP.Pub.NYs";
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.MinLen = 0;
                attr.MaxLen = 7;
                attr.Insert();

                attr = new BP.Sys.MapAttr();
                attr.FK_MapData = md.No;
                attr.HisEditType = BP.En.EditType.UnDel;
                attr.KeyOfEn = "MyNum";
                attr.Name = " The number of "; // " The number of ";
                attr.DefVal = "1";
                attr.MyDataType = BP.DA.DataType.AppInt;
                attr.UIContralType = UIContralType.TB;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.LGType = FieldTypeS.Normal;
                attr.UIVisible = false;
                attr.UIIsEnable = false;
                attr.Insert();
               
            }
        }
    }
    /// <summary>
    ///  Set of nodes 
    /// </summary>
    public class Nodes : EntitiesOID
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new Node();
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Set of nodes 
        /// </summary>
        public Nodes()
        {
        }
        /// <summary>
        ///  Set of nodes .
        /// </summary>
        /// <param name="FlowNo"></param>
        public Nodes(string fk_flow)
        {
            //   Nodes nds = new Nodes();
            this.Retrieve(NodeAttr.FK_Flow, fk_flow, NodeAttr.Step);
            //this.AddEntities(NodesCash.GetNodes(fk_flow));
            return;
        }
        #endregion

        #region  Query methods 
        /// <summary>
        /// RetrieveAll
        /// </summary>
        /// <returns></returns>
        public override int RetrieveAll()
        {
            Nodes nds = Cash.GetObj(this.ToString(), Depositary.Application) as Nodes;
            if (nds == null)
            {
                nds = new Nodes();
                QueryObject qo = new QueryObject(nds);
                qo.AddWhereInSQL(NodeAttr.NodeID, " SELECT Node FROM WF_Direction ");
                qo.addOr();
                qo.AddWhereInSQL(NodeAttr.NodeID, " SELECT ToNode FROM WF_Direction ");
                qo.DoQuery();

                Cash.AddObj(this.ToString(), Depositary.Application, nds);
                Cash.AddObj(this.GetNewEntity.ToString(), Depositary.Application, nds);
            }

            this.Clear();
            this.AddEntities(nds);
            return this.Count;
        }
        /// <summary>
        ///  Start node 
        /// </summary>
        public void RetrieveStartNode()
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(NodeAttr.NodePosType, (int)NodePosType.Start);
            qo.addAnd();
            qo.AddWhereInSQL(NodeAttr.NodeID, "SELECT FK_Node FROM WF_NodeStation WHERE FK_STATION IN (SELECT FK_STATION FROM Port_EmpSTATION WHERE FK_Emp='" + BP.Web.WebUser.No + "')");

            qo.addOrderBy(NodeAttr.FK_Flow);
            qo.DoQuery();
        }
        #endregion
    }
}
