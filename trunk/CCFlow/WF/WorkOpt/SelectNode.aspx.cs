using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.WF;

namespace CCFlow.WF.WorkOpt
{
    public partial class SelectNode : System.Web.UI.Page
    {
        #region  Parameters 
        /// <summary>
        ///  Staff numbers 
        /// </summary>
        public string WorkerId
        {
            get
            {
                return this.Request.QueryString["WorkerId"];
            }
        }
        /// <summary>
        ///  Current process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  The current flow to the node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        /// <summary>
        ///  Show only select the node list of steps 
        /// </summary>
        public bool ShowNodeOnly
        {
            get
            {
                return bool.Parse(Request.QueryString["ShowNodeOnly"] ?? "false");
            }
        }
        /// <summary>
        ///  Show only selection process list 
        /// </summary>
        public bool ShowFlowOnly
        {
            get
            {
                return bool.Parse(Request.QueryString["ShowFlowOnly"] ?? "false");
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShowFlowOnly)
            {
                if (string.IsNullOrWhiteSpace(FK_Flow))
                    throw new Exception("FK_Flow Parameter can not be null , And the process must be a valid number ");

                var sql = string.Format("SELECT NodeID, Name,Step FROM WF_Node WHERE FK_Flow='{0}'", FK_Flow);
                var dtNodes = DBAccess.RunSQLReturnTable(sql);

                foreach (DataRow dr in dtNodes.Rows)
                {
                    if (FK_Node == Convert.ToInt32(dr["NodeID"])) continue;

                    lbNodes.Items.Add(
                        new ListItem(
                            string.Format("{0}. {1}", dr["Step"], dr["Name"]),
                            string.Format("{0},{1},{2}", dr["Step"], dr["NodeID"], dr["Name"])));
                }
            }

            if (!ShowNodeOnly)
            {
                var sql = "SELECT wfs.No,wfs.Name,wfs.ParentNo FROM WF_FlowSort wfs";
                var dtFlowSorts = DBAccess.RunSQLReturnTable(sql);

                DataTable dtFlows = null;

                sql = "SELECT wf.No,wf.[Name],wf.FK_FlowSort FROM WF_Flow wf";

                if (!string.IsNullOrWhiteSpace(WorkerId) && WorkerId.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length == 1)
                    dtFlows = Dev2Interface.DB_GenerCanStartFlowsOfDataTable(WorkerId);
                else
                    dtFlows = DBAccess.RunSQLReturnTable(sql);

                // Data collection is defined list of organizations 
                // Tuple definition : Is the process , Tree level [0 Begin ], Text ,值
                var items = new List<Tuple<bool, int, string, string>>();

                GenerateItems(items, dtFlowSorts.Select("ParentNo=0")[0], 0, dtFlows, dtFlowSorts);

                foreach (var item in items)
                {
                    lbFlows.Items.Add(new ListItem(item.Item3, item.Item4));
                }

                // Generated the following list style :
                //┌ Process tree 
                //├─ Linear Process 
                //├┄┄001. Financial reimbursement demo 
            }
        }

        /// <summary>
        ///  Generate a list of the process tree 
        /// </summary>
        /// <param name="items"> Saved tree list data set </param>
        /// <param name="drFlowSort"> Current category tree </param>
        /// <param name="level"> Current category level </param>
        /// <param name="dtFlows"> Collection process </param>
        /// <param name="dtFlowSorts"> Category collections </param>
        private void GenerateItems(List<Tuple<bool, int, string, string>> items, DataRow drFlowSort, int level, DataTable dtFlows, DataTable dtFlowSorts)
        {
            var prefix = (level == 0 ? "┏" : "┣");
            items.Add(
                Tuple.Create(
                    false,
                    level,
                    prefix.PadRight(level + 1, '━') + drFlowSort["Name"],
                    drFlowSort["No"].ToString()));

            var drSorts = dtFlowSorts.Select(string.Format("ParentNo='{0}'", drFlowSort["No"]));

            foreach (var drSort in drSorts)
                GenerateItems(items, drSort, level + 1, dtFlows, dtFlowSorts);

            var drFlows = dtFlows.Select(string.Format("FK_FlowSort='{0}'", drFlowSort["No"]), "No ASC");

            foreach (var drFlow in drFlows)
            {
                // Exclude the specified process 
                if (FK_Flow == drFlow["No"].ToString()) continue;

                items.Add(
                    Tuple.Create(
                        true,
                        level,
                        string.Format("{0}{1}{2}", prefix, string.Empty.PadLeft(level + 1, '┅'),
                                      drFlow["No"] + "." + drFlow["Name"]),
                        string.Format("{0},{1}", drFlow["No"], drFlow["Name"])));
            }
        }
    }
}