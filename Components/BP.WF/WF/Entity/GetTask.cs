using System;
using System.Collections.Generic;
using System.Text;
using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web;
using BP.Port;
using BP.WF.Data;
using BP.WF.Template;


namespace BP.WF
{
    /// <summary>
    ///   Property 
    /// </summary>
    public class GetTaskAttr
    {
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Audit node 
        /// </summary>
        public const string MainNode = "MainNode";
        /// <summary>
        ///  To audit node 
        /// </summary>
        public const string CheckNodes = "CheckNodes";
    }
    /// <summary>
    ///  Retrieve tasks 
    /// </summary>
    public class GetTask : BP.En.Entity
    {
        /// <summary>
        ///  I can deal with the current work it ?
        /// </summary>
        /// <returns></returns>
        public bool Can_I_Do_It()
        {
            /*  Judge whether I can handle the current point data ? */
            switch (this.HisDeliveryWay)
            {
                case DeliveryWay.ByPreviousNodeFormEmpsField:
                    NodeEmps ndemps = new NodeEmps(this.NodeID);
                    if (ndemps.Contains(NodeEmpAttr.FK_Emp, WebUser.No) == false)
                        return false;
                    else
                        return true;
                case DeliveryWay.ByStation:
                    Stations sts = WebUser.HisStations;
                    string myStaStrs = "@";
                    foreach (Station st in sts)
                        myStaStrs += "@" + st.No;
                    myStaStrs = myStaStrs + "@";

                    NodeStations ndeStas = new NodeStations(this.NodeID);
                    bool isHave = false;
                    foreach (NodeStation ndS in ndeStas)
                    {
                        if (myStaStrs.Contains("@" + ndS.FK_Station + "@") == true)
                        {
                            isHave = true;
                            break;
                        }
                    }
                    if (isHave == false)
                        return false;
                    return true;
                default: /*  The other case is the judgment .*/
                    // jc.Delete(); //  Setting is illegal , Delete .
                    return false;
                    break;
            }
        }

        #region attrs
        /// <summary>
        ///  Delivery methods 
        /// </summary>
        public DeliveryWay HisDeliveryWay
        {
            get
            {
                return (DeliveryWay)this.GetValIntByKey(NodeAttr.DeliveryWay);
            }
        }
        /// <summary>
        ///  Node ID
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
        /// <summary>
        ///  Name 
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.Name);
            }
            set
            {
                this.SetValByKey(NodeAttr.Name, value);
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(GetTaskAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(GetTaskAttr.FK_Flow, value);
            }
        }
        /// <summary>
        ///  Step 
        /// </summary>
        public int Step
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.Step);
            }
        }
        public string CheckNodes
        {
            get
            {
                string s= this.GetValStringByKey(GetTaskAttr.CheckNodes);
                s = s.Replace("~", "'");
                return s;
            }
            set
            {
                this.SetValByKey(GetTaskAttr.CheckNodes, value);
            }
        }
        #endregion attrs

        #region  Property 
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();

                #region  Basic properties 
                map.EnDBUrl = new DBUrl(DBUrlType.AppCenterDSN); // To connect to a data source （ Indicate that the system you want to connect to the database ）.
                map.PhysicsTable = "WF_Node"; //  To physical table .
                map.EnDesc = " Retrieve tasks ";
                map.EnType = EnType.App;
                #endregion

                #region  Field 
                map.AddTBIntPK(NodeAttr.NodeID, 0,"NodeID", true, true);
                map.AddTBString(NodeAttr.Name, null," Node Name ", true, false, 0, 100, 10);
                map.AddTBInt(NodeAttr.Step,0, " Step ", true, false);
                map.AddTBString(NodeAttr.FK_Flow, null, " Process ID ", true, false, 0, 10, 10);
                map.AddTBString(GetTaskAttr.CheckNodes, null, " Work node s", true, false, 0, 800, 100);

                map.AddTBInt(NodeAttr.DeliveryWay, 0, " Access Rules ", true, true);

                #endregion  Field 

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Retrieve tasks 
        /// </summary>
        public GetTask()
        {
        }
        public GetTask(int nodeId)
        {
            this.NodeID = nodeId;
            this.Retrieve();
        }
        #endregion attrs
    }
    /// <summary>
    ///  Retrieve tasks set 
    /// </summary>
    public class GetTasks : BP.En.Entities
    {
        public GetTasks()
        {
        }
        /// <summary>
        ///  Retrieve tasks set 
        /// </summary>
        public GetTasks(string fk_flow)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(GetTaskAttr.FK_Flow, fk_flow);
            qo.addAnd();
            qo.AddWhereLen(GetTaskAttr.CheckNodes, " >= ", 3, SystemConfig.AppCenterDBType);
            qo.DoQuery();
        }
        /// <summary>
        ///  Get real 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new GetTask();
            }
        }
    }
}
