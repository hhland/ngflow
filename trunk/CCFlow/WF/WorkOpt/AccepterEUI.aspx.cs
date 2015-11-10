using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Web;
using System.Data;
using System.Text;
using CCPortal.DA;
using BP.WF;
using BP.WF.Template;
using BP.En;
using BP.WF.Data;

namespace CCFlow.WF.WorkOpt
{
    public partial class AccepterEUI : BP.Web.WebPage
    {
        public Selector MySelector = null;
        public string FK_Node
        {
            get
            {
                try
                {
                    string nodeid = this.Request.QueryString["NodeID"];
                    if (nodeid == null)
                        nodeid = this.Request.QueryString["FK_Node"];
                    return nodeid;
                }
                catch
                {
                    return "101"; // 0;  There may be a process to call the process a form .
                }
            }
        }
        /// <summary>
        ///  Arrival node 
        /// </summary>
        public int ToNode
        {
            get
            {

                if (this.Request.QueryString["ToNode"] == null)
                    return 0;
                return int.Parse(this.Request["ToNode"].ToString());
            }
        }
        private BP.WF.Node _HisNode = null;
        /// <summary>
        ///  Its node 
        /// </summary>
        public BP.WF.Node HisNode
        {
            get
            {
                if (_HisNode == null)
                    _HisNode = new BP.WF.Node(this.FK_Node);
                return _HisNode;
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request["WorkID"].ToString());
            }
        }
        public GERpt _wk = null;
        public GERpt wk
        {
            get
            {
                if (_wk == null)
                {
                    _wk = this.HisNode.HisFlow.HisGERpt;
                    _wk.OID = this.WorkID;
                    _wk.Retrieve();
                    _wk.ResetDefaultVal();
                }
                return _wk;
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        private bool IsMultiple = false;
        /// <summary>
        ///  Are multi-branch 
        /// </summary>
        public bool IsMFZ
        {
            get
            {
                Nodes nds = this.HisNode.HisToNodes;
                int num = 0;
                foreach (BP.WF.Node mynd in nds)
                {
                    #region  Filtered inaccessible node .
                    Cond cond = new Cond();
                    int i = cond.Retrieve(CondAttr.FK_Node, this.HisNode.NodeID, CondAttr.ToNodeID, mynd.NodeID);
                    if (i == 0)
                        continue; //  The condition is not set direction , Just let it jump .
                    cond.WorkID = this.WorkID;
                    cond.en = wk;

                    if (cond.IsPassed == false)
                        continue;
                    #endregion  Filtered inaccessible node .

                    if (mynd.HisDeliveryWay == DeliveryWay.BySelected)
                    {
                        num++;
                    }
                }
                if (num == 0)
                    return false;
                if (num == 1)
                    return false;
                return true;
            }
        }
        public Int64 FID
        {
            get
            {
                if (this.Request["FID"] != null)
                    return Int64.Parse(this.Request["FID"].ToString());

                return 0;
            }
        }
        /// <summary>
        ///  Turn on 
        /// </summary>
        public int IsWinOpen
        {
            get
            {
                string str = this.Request.QueryString["IsWinOpen"];
                if (str == "1" || str == null)
                    return 1;
                return 0;
            }
        }
        /// <summary>
        ///  Get incoming parameters 
        /// </summary>
        /// <param name="param"> Parameter name </param>
        /// <returns></returns>
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // The return value 
            string s_responsetext = string.Empty;
            s_responsetext = BindByWhichMet();

            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client   Tree 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }
        public string BindByWhichMet()
        {
            #region
            // Determine whether the need to move .
            if (this.ToNode == 0)
            {
                int num = 0;
                int tempToNodeID = 0;
                /* If you reach the point is empty  */
                /* First, determine the current node ID, Is configured to other nodes inside ,
                 * *  If you need to move the selection box to advanced , Select the person does not meet the current needs of the interface class document .*/
                string sql = "SELECT COUNT(*) FROM WF_Node WHERE FK_Flow='" + this.HisNode.FK_Flow + "' AND " + NodeAttr.DeliveryWay + "=" + (int)DeliveryWay.BySelected + " AND " + NodeAttr.DeliveryParas + " LIKE '%" + this.HisNode.NodeID + "%' ";

                if (DBAccess.RunSQLReturnValInt(sql, 0) > 0)
                {
                    /* Select the description next few nodes personnel handling  */
                    string url = "AccepterAdv.aspx?1=3" + this.RequestParas;
                    this.Response.Redirect(url, true);
                    return "";
                }

                Nodes nds = this.HisNode.HisToNodes;
                if (nds.Count == 0)
                {
                    //this.Pub1.AddFieldSetRed(" Prompt ", " The current point is the last node , You can not use this feature .");
                    return "";
                }
                else if (nds.Count == 1)
                {
                    BP.WF.Node toND = nds[0] as BP.WF.Node;
                    tempToNodeID = toND.NodeID;
                }
                else
                {
                    BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
                    foreach (BP.WF.Node mynd in nds)
                    {
                        if (mynd.HisDeliveryWay != DeliveryWay.BySelected)
                            continue;

                        #region  Filtered inaccessible node .
                        if (nd.CondModel == CondModel.ByLineCond)
                        {
                            Cond cond = new Cond();
                            int i = cond.Retrieve(CondAttr.FK_Node, this.HisNode.NodeID, CondAttr.ToNodeID, mynd.NodeID);
                            if (i == 0)
                                continue; //  The condition is not set direction , Just let it jump .
                            cond.WorkID = this.WorkID;
                            cond.en = wk;
                            if (cond.IsPassed == false)
                                continue;
                        }
                        #endregion  Filtered inaccessible node .
                        tempToNodeID = mynd.NodeID;
                        num++;
                    }
                }

                if (tempToNodeID == 0)
                {
                    this.WinCloseWithMsg("@ Process design errors :\n\n  All branches of the current node does not have a choice to accept the staff in accordance with accepted rules .");
                    return "";
                }
                this.Response.Redirect("Accepter.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&ToNode=" + tempToNodeID + "&FID=" + this.FID + "&type=1&WorkID=" + this.WorkID + "&IsWinOpen=" + this.IsWinOpen, true);
                return "";
            }


            try
            {
                /*  First determines whether there are a plurality of branch conditions .*/
                if (this.IsMFZ && ToNode == 0)
                {
                    IsMultiple = true;
                    //this.BindMStations();
                    return "";
                }

                MySelector = new Selector(this.ToNode);
                switch (MySelector.SelectorModel)
                {
                    //case SelectorModel.Station:
                    //    this.BindByStation();
                    //    break;
                    //case SelectorModel.SQL:
                    //    this.BindBySQL();
                    //    break;
                    //case SelectorModel.Dept:
                    //    this.BindByDept();
                    //    break;
                    case SelectorModel.Emp:
                        this.BindByEmp();
                        break;
                    case SelectorModel.Url:
                        if (MySelector.SelectorP1.Contains("?"))
                            this.Response.Redirect(MySelector.SelectorP1 + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        else
                            this.Response.Redirect(MySelector.SelectorP1 + "?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        return "";
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //this.Pub1.Clear();
                //this.Pub1.AddMsgOfWarning(" Error ", ex.Message);
            }

            #endregion
            return "";
        }
        /// <summary>
        /// 按BindByEmp  The way 
        /// </summary>
        public string BindByEmp()
        {
            string s_responsetext = string.Empty;
            string s_checkded = string.Empty;
            // Node department collection 
            NodeDepts nodeDepts = new NodeDepts();
            QueryObject obj = new QueryObject(nodeDepts);
            obj.AddWhere(NodeDeptAttr.FK_Node, this.FK_Node);
            obj.DoQuery();
            // The department has , String concatenation 
            if (nodeDepts != null && nodeDepts.Count > 0)
            {
                foreach (NodeDept item in nodeDepts)
                {
                    s_checkded += "," + item.FK_Dept + ",";
                }
            }
            string BindByEmpSql = string.Format("select No,Name,ParentNo  from Port_Dept   WHERE No IN (SELECT FK_Dept " +
                                                "FROM Port_Emp WHERE No in(SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node={0})) union " +
                                                "select No,Name,FK_Dept as ParentNo  from Port_Emp  WHERE No in (SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node={0})", MySelector.NodeID);
            DataTable BindByEmpDt = DBAccess.RunSQLReturnTable(BindByEmpSql);
            s_responsetext = GetTreeJsonByTable(BindByEmpDt, "NO", "NAME", "ParentNo", "0", s_checkded);
            //string sqlGroup = "SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM Port_Emp WHERE No in(SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + MySelector.NodeID + "'))";
            //string sqlDB = "SELECT No,Name,FK_Dept FROM Port_Emp WHERE No in (SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + MySelector.NodeID + "')";

            //DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            //DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);

            //if (this.MySelector.SelectorDBShowWay == SelectorDBShowWay.Table)
            //    this.BindBySQL_Table(dtGroup, dtDB);
            //else
            //    this.BindBySQL_Tree(dtGroup, dtDB);
            return s_responsetext;
        }
        /// <summary>
        /// 按table The way .
        /// </summary>
        //public void BindBySQL_Table(DataTable dtGroup, DataTable dtObj)
        //{
        //    int col = 4;
        //    this.Pub1.AddTable("style='border:0px;width:100%'");
        //    foreach (DataRow drGroup in dtGroup.Rows)
        //    {
        //        string ctlIDs = "";
        //        string groupNo = drGroup[0].ToString();

        //        // Select all increase .
        //        this.Pub1.AddTR();
        //        CheckBox cbx = new CheckBox();
        //        cbx.ID = "CBs_" + drGroup[0].ToString();
        //        cbx.Text = drGroup[1].ToString();
        //        this.Pub1.AddTDTitle("align=left", cbx);
        //        this.Pub1.AddTREnd();

        //        this.Pub1.AddTR();
        //        this.Pub1.AddTDBegin("nowarp=false");

        //        this.Pub1.AddTable("style='border:0px;width:100%'");
        //        int colIdx = -1;
        //        foreach (DataRow drObj in dtObj.Rows)
        //        {
        //            string no = drObj[0].ToString();
        //            string name = drObj[1].ToString();
        //            string group = drObj[2].ToString();
        //            if (group.Trim() != groupNo.Trim())
        //                continue;

        //            colIdx++;
        //            if (colIdx == 0)
        //                this.Pub1.AddTR();

        //            CheckBox cb = new CheckBox();
        //            cb.ID = "CB_" + no;
        //            ctlIDs += cb.ID + ",";
        //            cb.Attributes["onclick"] = "isChange=true;";
        //            cb.Text = name;
        //            cb.Checked = false;
        //            if (cb.Checked)
        //                cb.Text = "<font color=green>" + cb.Text + "</font>";
        //            this.Pub1.AddTD(cb);
        //            if (col - 1 == colIdx)
        //            {
        //                this.Pub1.AddTREnd();
        //                colIdx = -1;
        //            }
        //        }
        //        cbx.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')";

        //        if (colIdx != -1)
        //        {
        //            while (colIdx != col - 1)
        //            {
        //                colIdx++;
        //                this.Pub1.AddTD();
        //            }
        //            this.Pub1.AddTREnd();
        //        }
        //        this.Pub1.AddTableEnd();
        //        this.Pub1.AddTDEnd();
        //        this.Pub1.AddTREnd();
        //    }
        //    this.Pub1.AddTableEnd();

        //    this.BindEnd();
        //}
        public string BindBySQL_Tree(DataTable dtGroup, DataTable dtDB)
        {
            return "";
        }
        /// <summary>
        ///  According to DataTable Generate Json Tree 
        /// </summary>
        /// <param name="tabel"> Data Sources </param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela"> Relationship field </param>
        /// <param name="pId">父ID</param>
        ///<returns>easyui tree json Format </returns>
        StringBuilder treeResult = new StringBuilder();
        StringBuilder treesb = new StringBuilder();
        public string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string CheckedString)
        {
            string treeJson = string.Empty;
            treeResult.Append(treesb.ToString());

            treesb.Clear();
            if (tabel.Rows.Count > 0)
            {
                treesb.Append("[");
                string filer = string.Empty;
                if (pId.ToString() == "")
                {
                    filer = string.Format("{0} is null", rela);
                }
                else
                {
                    filer = string.Format("{0}='{1}'", rela, pId);
                }
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string deptNo = row[idCol].ToString();

                        if (treeResult.Length == 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                 + "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                        }
                        else if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                 + "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"closed\"");
                        }
                        else
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                 + "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower());
                        }


                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], CheckedString);
                            treeResult.Append(treesb.ToString());
                            treesb.Clear();
                        }
                        treeResult.Append(treesb.ToString());
                        treesb.Clear();
                        treesb.Append("},");
                    }
                    treesb = treesb.Remove(treesb.Length - 1, 1);
                }
                treesb.Append("]");
                treeResult.Append(treesb.ToString());
                treeJson = treeResult.ToString();
                treesb.Clear();
            }
            return treeJson;
        }


    }
}