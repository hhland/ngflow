using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP.DA;
using BP;
using BP.Sys;
using BP.En;

namespace BP.WF
{
    /// <summary>
    ///  Button State 
    /// </summary>
    public class ButtonState
    {
        public Int64 WorkID = 0;
        public int CurrNodeIDOfUI = 0;
        public int CurrNodeIDOfFlow = 0;
        public string FK_Flow = null;
        public void InitNodeIsCurr()
        {
            //  Get .
            Node nd = new Node(this.CurrNodeIDOfFlow);
            if (nd.IsStartNode)
            {
                /*  Start node allows deletion process  */
                this.Btn_DelFlow = true;
                this.Btn_Send = true;
                this.Btn_Save = true;
                return;
            }

            #region  Determine whether they can withdraw send .
            WorkNode wn = new WorkNode(this.WorkID, this.CurrNodeIDOfFlow);
            WorkNode wnPri = wn.GetPreviousWorkNode();

            //  Determine whether it can handle the work of the previous step .
            GenerWorkerList wl = new GenerWorkerList();
            int num = wl.Retrieve(GenerWorkerListAttr.FK_Emp, BP.Web.WebUser.No,
                GenerWorkerListAttr.FK_Node, wnPri.HisNode.NodeID,
                GenerWorkerListAttr.WorkID, this.WorkID);
            if (num >= 1)
            {
                /* If you can handle the work of the previous step */
            }
            else
            {
                /* Previous work can not handle ,  Can be allowed to return */
                this.Btn_Return = nd.IsCanReturn;
                this.Btn_Send = true;
                this.Btn_Save = true;
            }
            #endregion
        }
        public void InitNodeIsNotCurr()
        {
            string sql = "SELECT count(*) FROM WF_GenerWorkerlist WHERE FK_Node=" + this.CurrNodeIDOfUI + " AND WorkID=" + this.WorkID;
            if (DBAccess.RunSQLReturnValInt(sql, 0) >= 1)
                this.Btn_UnSend = true;
        }
        public ButtonState(string fk_flow, int currNodeID, Int64 workid)
        {
            this.FK_Flow = fk_flow;
            this.CurrNodeIDOfUI = currNodeID;
            this.WorkID = workid;
            if (workid != 0)
                this.Btn_Track = true;

            string sql = "SELECT FK_Node FROM WF_GenerWorkFlow WHERE WorkID=" + workid;
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            if (dt.Rows.Count == 0)
            {
                /*  No explanation  workid  Initialization work situation ,  Save and send only two buttons are available  */
                this.Btn_Send = true;
                this.Btn_Save = true;
                return;
            }

            //  Set the current process node .
            this.CurrNodeIDOfFlow = int.Parse(dt.Rows[0][0].ToString());

            if (this.CurrNodeIDOfUI == this.CurrNodeIDOfFlow)
            {
                /* If the node and the current node is equal to the running process */
                InitNodeIsCurr();
            }
            else
            {
                InitNodeIsNotCurr();
            }
        }
        /// <summary>
        ///  Save button 
        /// </summary>
        public bool Btn_Send = false;
        /// <summary>
        ///  Save button 
        /// </summary>
        public bool Btn_Save = false;
        /// <summary>
        ///  Forwarding 
        /// </summary>
        public bool Btn_Forward = false;
        /// <summary>
        ///  Return 
        /// </summary>
        public bool Btn_Return = false;
        /// <summary>
        ///  Send revocation 
        /// </summary>
        public bool Btn_UnSend = false;
        /// <summary>
        ///  Delete Process 
        /// </summary>
        public bool Btn_DelFlow = false;
        /// <summary>
        ///  New Process 
        /// </summary>
        public bool Btn_NewFlow = false;
        /// <summary>
        ///  Work trajectory 
        /// </summary>
        public bool Btn_Track = false;
    }
}
