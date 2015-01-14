using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.WF;
using BP.Web;

namespace CCFlow.WF.WorkOpt
{
    /// <summary>
    /// liuxc
    /// </summary>
    public partial class TransferCustomUI : System.Web.UI.Page
    {
        #region  Parameters 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var haveNameText = false;
            var mustFilter = false;
            var step = 0;

            // Get the node with the current node has completed the 
            var sql = string.Format("SELECT wgw.FK_Node,wgw.FK_NodeText,wgw.FK_Emp,wgw.FK_EmpText,wgw.RDT,wgw.IsPass,wn.Step FROM WF_GenerWorkerlist wgw INNER JOIN WF_Node wn ON wn.NodeID = wgw.FK_Node WHERE WorkID = {0} ORDER BY wgw.RDT", WorkID);
            var dtWorkers = DBAccess.RunSQLReturnTable(sql);
            var overWorks = CreateDataTableFromDataRow(dtWorkers.Clone(), dtWorkers.Select("IsPass=1", "Step ASC"));
            var currWorks = CreateDataTableFromDataRow(dtWorkers.Clone(), dtWorkers.Select("IsPass=0"));

            step += overWorks.Rows.Count + currWorks.Rows.Count;

            // If the sponsor , Is here 0
            if (currWorks.Rows.Count == 0)
            {
                sql = string.Format("SELECT NodeID AS FK_Node,Name AS FK_NodeText,FK_Emp='',FK_EmpText='{0}',RDT=CAST(GETDATE() AS NVARCHAR(50)),IsPass=0,Step FROM WF_Node WHERE FK_Flow='{1}' AND NodeID = {2}", WebUser.Name, FK_Flow, FK_Node);
                currWorks = DBAccess.RunSQLReturnTable(sql);
                step += currWorks.Rows.Count;
            }

            litCurrentStep.Text = currWorks.Rows[0]["Step"].ToString();
            lblFK_NodeText.Text = currWorks.Rows[0]["FK_NodeText"].ToString();
            lblFK_EmpText.Text = currWorks.Rows[0]["FK_EmpText"].ToString();
            lblRDT.Text = DateTime.Parse(currWorks.Rows[0]["RDT"].ToString()).ToString("yyyy-MM-dd");

            // Get process custom list 
            sql = string.Format("SELECT wfc.FK_Node,wn.Name AS FK_NodeText,wfc.SubFlowNo,ISNULL(wf.Name,'——') AS SubFlowName, wfc.Worker,WorkerText='',Idx = wfc.Idx + {0} + 1 FROM WF_TransferCustom wfc INNER JOIN WF_Node wn ON wn.NodeID = wfc.FK_Node LEFT JOIN WF_Flow wf ON wf.No = wfc.SubFlowNo WHERE wfc.WorkID = {1} ORDER BY wfc.Idx ASC", step, WorkID);
            var dtTCs = DBAccess.RunSQLReturnTable(sql);

            // If the process is not the custom data , From WF_SelectAccper Get automatic calculation of each process node processing personal information 
            if (dtTCs.Rows.Count == 0)
            {
                haveNameText = true;
                mustFilter = true;
                sql = string.Format("SELECT wsa.FK_Node,wn.Name AS FK_NodeText,SubFlowNo='',SubFlowName='——',wsa.FK_Emp AS Worker,EmpName AS WorkerText,wn.Step AS Idx  FROM WF_SelectAccper wsa INNER JOIN WF_Node wn ON wn.NodeID = wsa.FK_Node WHERE wsa.WorkID = {0}", WorkID);
                dtTCs = DBAccess.RunSQLReturnTable(sql);
            }

            // In case WF_SelectAccper Also did not generate information processors of each node , From WF_Node Gets all the nodes , To choose 
            if (dtTCs.Rows.Count == 0)
            {
                haveNameText = true;
                mustFilter = true;
                sql = string.Format("SELECT NodeID AS FK_Node,Name AS FK_NodeText,SubFlowNo='',SubFlowName='——',Worker='',WorkerText='',Step AS Idx FROM WF_Node WHERE FK_Flow='{0}'", FK_Flow);
                dtTCs = DBAccess.RunSQLReturnTable(sql);
            }

            // Removal has been completed + The current node in step 
            if (mustFilter)
            {
                overWorks.Rows.Add(currWorks.Rows[0].ItemArray);
                var removeSteps = 0;

                for (var i = 0; i < overWorks.Rows.Count; i++)
                {
                    if (dtTCs.Rows[i]["FK_Node"].Equals(overWorks.Rows[i]["FK_Node"]))
                    {
                        removeSteps++;
                    }
                }

                overWorks.Rows.RemoveAt(overWorks.Rows.Count - 1);

                while (removeSteps > 0)
                {
                    dtTCs.Rows.RemoveAt(0);
                    removeSteps--;
                }
            }

            // If it is removed from the process of custom data , Because the saved node processors Number , So it is necessary to reacquire names 
            if (!haveNameText)
            {
                sql = "SELECT No,Name FROM Port_Emp";
                var dtEmps = DBAccess.RunSQLReturnTable(sql);
                string[] empArr = null;
                DataRow[] drs = null;

                foreach (DataRow w in dtTCs.Rows)
                {
                    empArr = (w["Worker"] + "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (var emp in empArr)
                    {
                        drs = dtEmps.Select(string.Format("No='{0}'", emp));
                        w["WorkerText"] += (drs.Length == 0 ? emp : drs[0]["Name"].ToString()) + ",";
                    }

                    w["WorkerText"] = w["WorkerText"].ToString().TrimEnd(',');
                }
            }

            rptOverNodes.DataSource = overWorks;
            rptOverNodes.DataBind();

            rptNextNodes.DataSource = dtTCs;
            rptNextNodes.DataBind();
        }

        public void Cancel()
        {

            this.Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID, true);
        }

        protected void lbtnUseAutomic_Click(object sender, EventArgs e)
        {
            Dev2Interface.Flow_SetFlowTransferCustom(FK_Flow, WorkID, true, hid_idx_all.Value);
            Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID, true);
        }

        protected void lbtnUseManual_Click(object sender, EventArgs e)
        {
            Dev2Interface.Flow_SetFlowTransferCustom(FK_Flow, WorkID, false, hid_idx_all.Value);
            Response.Redirect("../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FID=" + this.FID, true);
        }

        private DataTable CreateDataTableFromDataRow(DataTable emptyTable, DataRow[] drs)
        {
            foreach (var dr in drs)
            {
                emptyTable.Rows.Add(dr.ItemArray);
            }

            return emptyTable;
        }
    }
}