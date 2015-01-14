using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using System.Data;
using BP.DA;
using BP.Web;
using BP.En;
using System.Text;
using BP.Port;
using BP.Sys;
using BP.WF.Template;
using BP.WF.Data;
namespace CCFlow.WF
{
    /// <summary>
    ///  Recipient 
    /// </summary>
    public partial class WF_Accepter : BP.Web.WebPage
    {
        #region  Property .
        /// <summary>
        ///  Turn on 
        /// </summary>
        public int IsWinOpen
        {
            get
            {
                string str = this.Request.QueryString["IsWinOpen"];
                if (str == "1" || str == null || str == "")
                    return 1;
                return 0;
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
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request["FK_Node"].ToString());
            }
        }
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request["WorkID"].ToString());
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
        public string FK_Dept
        {
            get
            {
                string s = this.Request.QueryString["FK_Dept"];
                if (s == null)
                    s = WebUser.FK_Dept;
                return s;
            }
        }
        public string FK_Station
        {
            get
            {
                return this.Request.QueryString["FK_Station"];
            }
        }
        public string WorkIDs
        {
            get
            {
                return this.Request.QueryString["WorkIDs"];
            }
        }
        public string DoFunc
        {
            get
            {
                return this.Request.QueryString["DoFunc"];
            }
        }
        public string CFlowNo
        {
            get
            {
                return this.Request.QueryString["CFlowNo"];
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
        ///  Get incoming parameters 
        /// </summary>
        /// <param name="param"> Parameter name </param>
        /// <returns></returns>
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(this.Request[param], System.Text.Encoding.UTF8);
        }
        #endregion  Property .

        //public DataTable GetTable()
        //{
        //    if (this.ToNode == 0)
        //        throw new Exception("@ Process design errors , Node does not turn . Illustration :  Currently A Node . If you are A Attribute points in enabled [ Recipient ] Push button , Then he turned to the node-set ( Is A For example, you can go to the set of nodes :A到B,A到C,  Then B,C Node is turned to a set of nodes ), There must be a node is the node attributes [ Access Rules ] Set [ Select from the previous step to send staff ]");

        //    NodeStations stas = new NodeStations(this.ToNode);
        //    if (stas.Count == 0)
        //    {
        //        BP.WF.Node toNd = new BP.WF.Node(this.ToNode);
        //        throw new Exception("@ Process design errors : Designers do not design nodes [" + toNd.Name + "], Jobs range recipients .");
        //    }

        //    string sql = "";
        //    if (this.Request.QueryString["IsNextDept"] != null)
        //    {
        //        int len = this.FK_Dept.Length + 2;
        //        string sqlDept = "SELECT No FROM Port_Dept WHERE " + SystemConfig.AppCenterDBLengthStr + "(No)=" + len + " AND No LIKE '" + this.FK_Dept + "%'";
        //        sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
        //        sql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
        //        sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + this.ToNode + ") ";
        //        sql += ") AND A.No IN( SELECT No FROM Port_Emp WHERE  " + SystemConfig.AppCenterDBLengthStr + "(FK_Dept)=" + len + " AND FK_Dept LIKE '" + this.FK_Dept + "%')";
        //        sql += " ORDER BY FK_DEPT ";
        //        return BP.DA.DBAccess.RunSQLReturnTable(sql);
        //    }


        //    //  Priority to solve the problem in this sector .
        //    if (this.FK_Dept == WebUser.FK_Dept)
        //    {
        //        if (BP.WF.Glo.OSModel == OSModel.BPM)
        //        {
        //            sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
        //            sql += "SELECT FK_EMP FROM Port_DeptEmpStation WHERE FK_STATION ";
        //            sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
        //            sql += ") AND a.No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='" + WebUser.FK_Dept + "')";
        //            sql += " ORDER BY FK_DEPT ";
        //        }
        //        else
        //        {
        //            sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
        //            sql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
        //            sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
        //            sql += ") AND a.No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='" + WebUser.FK_Dept + "')";
        //            sql += " ORDER BY FK_DEPT ";
        //        }

        //        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        //        if (dt.Rows.Count != 0)
        //            return dt;
        //    }

        //    sql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
        //    sql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
        //    sql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + ToNode + ") ";
        //    sql += ") ORDER BY FK_DEPT ";
        //    return BP.DA.DBAccess.RunSQLReturnTable(sql);
        //}
        public DataTable GetTable()
        {
            if (this.ToNode == 0)
                throw new Exception("@ Process design errors , Node does not turn . Illustration :  Currently A Node . If you are A Attribute points in enabled [ Recipient ] Push button , Then he turned to the node-set ( Is A For example, you can go to the set of nodes :A到B,A到C,  Then B,C Node is turned to a set of nodes ), There must be a node is the node attributes [ Access Rules ] Set [ Select from the previous step to send staff ]");

            NodeStations stas = new NodeStations(this.ToNode);
            if (stas.Count == 0)
            {
                BP.WF.Node toNd = new BP.WF.Node(this.ToNode);
                throw new Exception("@ Process design errors : Designers do not design nodes [" + toNd.Name + "], Jobs range recipients .");
            }

            string BindByStationSql = "";
            if (this.Request.QueryString["IsNextDept"] != null)
            {
                int len = this.FK_Dept.Length + 2;
                string sqlDept = "SELECT No FROM Port_Dept WHERE " + SystemConfig.AppCenterDBLengthStr + "(No)=" + len + " AND No LIKE '" + this.FK_Dept + "%'";
                BindByStationSql = "SELECT A.No,A.Name, A.FK_Dept, B.Name as DeptName FROM Port_Emp A,Port_Dept B WHERE A.FK_Dept=B.No AND a.NO IN ( ";
                BindByStationSql += "SELECT FK_EMP FROM Port_EmpSTATION WHERE FK_STATION ";
                BindByStationSql += "IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node=" + this.ToNode + ") ";
                BindByStationSql += ") AND A.No IN( SELECT No FROM Port_Emp WHERE  " + SystemConfig.AppCenterDBLengthStr + "(FK_Dept)=" + len + " AND FK_Dept LIKE '" + this.FK_Dept + "%')";
                BindByStationSql += " ORDER BY FK_DEPT ";
                return BP.DA.DBAccess.RunSQLReturnTable(BindByStationSql);
            }

            string ParSql = "select No from Port_Dept where ParentNo='0'";
            DataTable ParDt = DBAccess.RunSQLReturnTable(ParSql);
            //if (ParDt.Rows.Count == 0)// Wrong organizational structure 
            //{
            //}

            //  Priority to solve the problem in this sector .
            BindByStationSql = string.Format("select No,Name,ParentNo,'1' IsParent from Port_Dept where ParentNo='0' union" +
                                                  " select No,Name,b.FK_Station as ParentNo,'0' IsParent  from Port_Emp a inner" +
                                                  " join Port_DeptEmpStation b on a.No=b.FK_Emp and b.FK_Station in" +
                                                  " (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')  WHERE No in" +
                                                  "  (SELECT FK_EMP FROM Port_DeptEmpStation " +
                                                  " WHERE FK_STATION IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}'))" +
                                                  " AND No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='{1}') " +
                                                  " union select No,Name,'{2}' ParentNo,'1' IsParent  from Port_Station where no " +
                                                  "in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')", ToNode, WebUser.FK_Dept, ParDt.Rows[0][0].ToString());
            DdlEmpSql = string.Format("select No,Name from Port_Emp a inner" +
                                                " join Port_DeptEmpStation b on a.No=b.FK_Emp and b.FK_Station in" +
                                                " (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')  WHERE No in" +
                                                "  (SELECT FK_EMP FROM Port_DeptEmpStation " +
                                                " WHERE FK_STATION IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}'))" +
                                                " AND No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept ='{1}')", ToNode, WebUser.FK_Dept);

            if (this.FK_Dept == WebUser.FK_Dept)
            {

                if (BP.WF.Glo.OSModel == OSModel.BPM)
                {

                }
                else
                {
                    BindByStationSql.Replace("Port_DeptEmpStation", "Port_EmpSTATION");
                    DdlEmpSql.Replace("Port_DeptEmpStation", "Port_EmpSTATION");
                }
                return DBAccess.RunSQLReturnTable(BindByStationSql);
            }

            BindByStationSql = string.Format("select No,Name,ParentNo,'1' IsParent from Port_Dept where ParentNo='0' union" +
                                               " select No,Name,b.FK_Station as ParentNo,'0' IsParent  from Port_Emp a inner" +
                                               " join Port_EmpSTATION b on a.No=b.FK_Emp and b.FK_Station in" +
                                               " (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')  WHERE No in" +
                                               "  (SELECT FK_EMP FROM Port_EmpSTATION " +
                                               " WHERE FK_STATION IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}'))" +
                                               " AND No IN (SELECT FK_Emp FROM Port_EmpDept) " +
                                               " union select No,Name,'{2}' ParentNo,'1' IsParent  from Port_Station where no " +
                                               "in(SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')", ToNode, WebUser.FK_Dept, ParDt.Rows[0][0].ToString());

            DdlEmpSql = string.Format("select No,Name,b.FK_Station as ParentNo,'0' IsParent  from Port_Emp a inner" +
                                               " join Port_EmpSTATION b on a.No=b.FK_Emp and b.FK_Station in" +
                                               " (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}')  WHERE No in" +
                                               "  (SELECT FK_EMP FROM Port_EmpSTATION " +
                                               " WHERE FK_STATION IN (SELECT FK_STATION FROM WF_NodeStation WHERE FK_Node='{0}'))" +
                                               " AND No IN (SELECT FK_Emp FROM Port_EmpDept) ", ToNode, WebUser.FK_Dept, ParDt.Rows[0][0].ToString());

            return DBAccess.RunSQLReturnTable(BindByStationSql);
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
        /// <summary>
        ///  Binding multi-branch 
        /// </summary>
        public void BindMStations()
        {

            this.BindByStation();

            Nodes mynds = this.HisNode.HisToNodes;
            this.Left.Add("<fieldset><legend>&nbsp; Select direction : Personnel lists of the selected direction &nbsp;</legend>");
            string str = "<p>";
            foreach (BP.WF.Node mynd in mynds)
            {
                if (mynd.HisDeliveryWay != DeliveryWay.BySelected)
                    continue;

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

                if (this.ToNode == mynd.NodeID)
                    str += "&nbsp;&nbsp;<b class='l-link'><font color='red' >" + mynd.Name + "</font></b>";
                else
                    str += "&nbsp;&nbsp;<b><a class='l-link' href='Accepter.aspx?FK_Node=" + this.FK_Node + "&type=1&ToNode=" + mynd.NodeID + "&WorkID=" + this.WorkID + "' >" + mynd.Name + "</a></b>";
            }
            this.Left.Add(str + "</p>");
            this.Left.AddFieldSetEnd();
        }

        public Selector MySelector = null;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Pub1.Clear();
            this.Title = " Select the next step to accept staff ";

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
                    return;
                }

                Nodes nds = this.HisNode.HisToNodes;
                if (nds.Count == 0)
                {
                    this.Pub1.AddFieldSetRed(" Prompt ", " The current point is the last node , You can not use this feature .");
                    return;
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
                    return;
                }


                this.Response.Redirect("Accepter.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&ToNode=" + tempToNodeID + "&FID=" + this.FID + "&type=1&WorkID=" + this.WorkID + "&IsWinOpen=" + this.IsWinOpen, true);
                return;
            }


            try
            {
                /*  First determines whether there are a plurality of branch conditions .*/
                if (this.IsMFZ && ToNode == 0)
                {
                    IsMultiple = true;
                    //this.BindMStations();
                    return;
                }
                MySelector = new Selector(this.ToNode);
                switch (MySelector.SelectorModel)
                {
                    case SelectorModel.Station:
                        //this.BindByStation();
                        returnValue("BindByStation");
                        break;
                    case SelectorModel.SQL:
                        //this.BindBySQL();
                        returnValue("BindBySQL");
                        break;
                    case SelectorModel.Dept:
                        //this.BindByDept();
                        returnValue("BindByDept");
                        break;
                    case SelectorModel.Emp:
                        //this.BindByEmp();
                        returnValue("BindByEmp");
                        break;
                    case SelectorModel.Url:
                        if (MySelector.SelectorP1.Contains("?"))
                            this.Response.Redirect(MySelector.SelectorP1 + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        else
                            this.Response.Redirect(MySelector.SelectorP1 + "?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node, true);
                        return;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.Pub1.Clear();
                this.Pub1.AddMsgOfWarning(" Error ", ex.Message);
            }
        }
        /// <summary>
        /// 按sql The way 
        /// </summary>
        public string BindBySQL()
        {
            //string sqlGroup = MySelector.SelectorP1;
            //sqlGroup = sqlGroup.Replace("@WebUser.No", WebUser.No);
            //sqlGroup = sqlGroup.Replace("@WebUser.Name", WebUser.Name);
            //sqlGroup = sqlGroup.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);

            //string sqlDB = MySelector.SelectorP2;
            //sqlDB = sqlDB.Replace("@WebUser.No", WebUser.No);
            //sqlDB = sqlDB.Replace("@WebUser.Name", WebUser.Name);
            //sqlDB = sqlDB.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);

            //DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            //DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);

            //if (this.MySelector.SelectorDBShowWay == SelectorDBShowWay.Table)
            //    this.BindBySQL_Table(dtGroup, dtDB);
            //else
            //    this.BindBySQL_Tree(dtGroup, dtDB);


            string BindBySQL = MySelector.SelectorP1;
            BindBySQL = BindBySQL.Replace("@WebUser.No", WebUser.No);
            BindBySQL = BindBySQL.Replace("@WebUser.Name", WebUser.Name);
            BindBySQL = BindBySQL.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);


            DataTable BindByEmpDt = DBAccess.RunSQLReturnTable(BindBySQL);

            return GetTreeJsonByTable(BindByEmpDt, "NO", "NAME", "ParentNo", "0", "IsParent", "");

        }
        /// <summary>
        /// 按BindByEmp  The way 
        /// </summary>
        public string BindByEmp()
        {
            //string sqlGroup = "SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM Port_Emp WHERE No in(SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + MySelector.NodeID + "'))";
            //string sqlDB = "SELECT No,Name,FK_Dept FROM Port_Emp WHERE No in (SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node='" + MySelector.NodeID + "')";

            //DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            //DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);

            //if (this.MySelector.SelectorDBShowWay == SelectorDBShowWay.Table)
            //    this.BindBySQL_Table(dtGroup, dtDB);
            //else
            //    this.BindBySQL_Tree(dtGroup, dtDB);

            string BindByEmpSql = string.Format("select No,Name,ParentNo,'1' IsParent  from Port_Dept   WHERE No IN (SELECT FK_Dept FROM " +
                                              "Port_Emp WHERE No in(SELECT FK_EMP FROM WF_NodeEmp WHERE FK_Node={0})) or ParentNo=0 union " +
                                              "select No,Name,FK_Dept as ParentNo,'0' IsParent  from Port_Emp  WHERE No in (SELECT FK_EMP " +
                                              "FROM WF_NodeEmp WHERE FK_Node={0})", MySelector.NodeID);
            DdlEmpSql = string.Format("select No,Name from Port_Emp  WHERE No in (SELECT FK_EMP " +
                                              "FROM WF_NodeEmp WHERE FK_Node={0})", MySelector.NodeID);
            DataTable BindByEmpDt = DBAccess.RunSQLReturnTable(BindByEmpSql);
            DataTable ParDt = DBAccess.RunSQLReturnTable("select No from Port_Dept where ParentNo='0'");
            foreach (DataRow r in BindByEmpDt.Rows)
            {
                if (r["IsParent"].ToString() == "1" && r["ParentNo"].ToString() != "0")
                {
                    r["ParentNo"] = ParDt.Rows[0][0].ToString();
                }
            }
            return GetTreeJsonByTable(BindByEmpDt, "NO", "NAME", "ParentNo", "0", "IsParent", "");
        }
        public string DdlEmpSql = "";
        /// <summary>
        ///  The return value 
        /// </summary>
        private void returnValue(string whichMet)
        {
            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;

            if (string.IsNullOrEmpty(Request["method"]))
                return;

            method = Request["method"].ToString();
            switch (method)
            {
                case "getTreeDateMet":// Get Data 
                    s_responsetext = getTreeDateMet(whichMet);
                    break;
                case "saveMet":
                    saveMet();
                    break;
            }

            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            s_responsetext = AppendJson(s_responsetext);
            s_responsetext = DdlValue(s_responsetext, DdlEmpSql);
            // Assembly ajax String format , Return to the calling client   Tree 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }
        public string AppendJson(string json)
        {
            StringBuilder AppendJson = new StringBuilder();
            AppendJson.Append(json);
            AppendJson.Append(",CheId:");
            string alreadyHadEmps = string.Format("select No, Name from Port_Emp where No in( select FK_Emp from WF_SelectAccper " +
                                                "where FK_Node={0} and WorkID={1})", this.ToNode, this.WorkID);
            DataTable dt = DBAccess.RunSQLReturnTable(alreadyHadEmps);
            AppendJson.Append("[{\"id\":\"CheId\",\"iconCls\":\"icon-save\",\"text\":\" Selected staff \",\"children\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AppendJson.Append("{id:\"" + dt.Rows[i][0].ToString() + "\",iconCls:\"icon-user\"" + ",text:\"" + dt.Rows[i][1].ToString() + "\"");
                if (i == dt.Rows.Count - 1)
                {
                    AppendJson.Append("}");
                    break;
                }
                AppendJson.Append("},");
            }
            //AppendJson.Append("]}]}");

            AppendJson.Append("]}]");

            AppendJson.Insert(0, "{tt:");
            return AppendJson.ToString();
        }
        public string DdlValue(string StrJson, string Str)
        {
            StringBuilder SBuilder = new StringBuilder();
            SBuilder.Append(StrJson);
            DataTable dt = DBAccess.RunSQLReturnTable(Str);

            SBuilder.Append(",ddl:[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    SBuilder.Append("{\"id\":" + dt.Rows[i]["No"].ToString() + ",\"text\":\"" + dt.Rows[i]["Name"].ToString() + "\",\"selected\":\"selected\"}");
                }
                else
                {
                    SBuilder.Append("{\"id\":" + dt.Rows[i]["No"].ToString() + ",\"text\":\"" + dt.Rows[i]["Name"].ToString() + "\"}");
                }
                if (i == dt.Rows.Count - 1)
                {
                    SBuilder.Append("");
                    continue;
                }
                SBuilder.Append(",");
            }
            SBuilder.Append("]}");
            return SBuilder.ToString();
        }
        public string getTreeDateMet(string Met)
        {
            switch (Met)
            {
                case "BindByEmp":
                    return BindByEmp();
                case "BindByDept":
                    return BindByDept();
                case "BindByStation":
                    return BindByStation();
                case "BindBySQL":
                    return BindBySQL();
                default:
                    return "";
            }
        }
        public string BindByDept()
        {
            //string sqlGroup = "SELECT No,Name FROM Port_Dept WHERE No IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node='" + MySelector.NodeID + "')";
            //string sqlDB = "SELECT No,Name, FK_Dept FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node='" + MySelector.NodeID + "')";

            //DataTable dtGroup = DBAccess.RunSQLReturnTable(sqlGroup);
            //DataTable dtDB = DBAccess.RunSQLReturnTable(sqlDB);

            //if (this.MySelector.SelectorDBShowWay == SelectorDBShowWay.Table)
            //this.BindBySQL_Table(dtGroup, dtDB);
            //else
            //    this.BindBySQL_Tree(dtGroup, dtDB);


            string BindByDeptSql = string.Format("SELECT  No,Name,ParentNo,'1' IsParent  FROM Port_Dept WHERE No IN (SELECT " +
                                                 "FK_Dept FROM WF_NodeDept WHERE FK_Node={0}) or ParentNo=0 union SELECT No,Name,FK_Dept " +
                                                 "as ParentNo,'0' IsParent FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node={0})", MySelector.NodeID);

            DdlEmpSql = string.Format("SELECT No,Name FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node={0})", MySelector.NodeID);


            DataTable BindByDeptDt = DBAccess.RunSQLReturnTable(BindByDeptSql);
            DataTable ParDt = DBAccess.RunSQLReturnTable("select No from Port_Dept where ParentNo='0'");
            foreach (DataRow r in BindByDeptDt.Rows)
            {
                if (r["IsParent"].ToString() == "1" && r["ParentNo"].ToString() != "0")
                {
                    r["ParentNo"] = ParDt.Rows[0][0].ToString();
                }
            }
            return GetTreeJsonByTable(BindByDeptDt, "NO", "NAME", "ParentNo", "0", "IsParent", "");
        }
        /// <summary>
        /// 按table The way .
        /// </summary>
        public void BindBySQL_Table(DataTable dtGroup, DataTable dtObj)
        {
            int col = 4;
            this.Pub1.AddTable("style='border:0px;width:100%'");
            foreach (DataRow drGroup in dtGroup.Rows)
            {
                string ctlIDs = "";
                string groupNo = drGroup[0].ToString();

                // Select all increase .
                this.Pub1.AddTR();
                CheckBox cbx = new CheckBox();
                cbx.ID = "CBs_" + drGroup[0].ToString();
                cbx.Text = drGroup[1].ToString();
                this.Pub1.AddTDTitle("align=left", cbx);
                this.Pub1.AddTREnd();

                this.Pub1.AddTR();
                this.Pub1.AddTDBegin("nowarp=false");

                this.Pub1.AddTable("style='border:0px;width:100%'");
                int colIdx = -1;
                foreach (DataRow drObj in dtObj.Rows)
                {
                    string no = drObj[0].ToString();
                    string name = drObj[1].ToString();
                    string group = drObj[2].ToString();
                    if (group.Trim() != groupNo.Trim())
                        continue;

                    colIdx++;
                    if (colIdx == 0)
                        this.Pub1.AddTR();

                    CheckBox cb = new CheckBox();
                    cb.ID = "CB_" + no;
                    ctlIDs += cb.ID + ",";
                    cb.Attributes["onclick"] = "isChange=true;";
                    cb.Text = name;
                    cb.Checked = false;
                    if (cb.Checked)
                        cb.Text = "<font color=green>" + cb.Text + "</font>";
                    this.Pub1.AddTD(cb);
                    if (col - 1 == colIdx)
                    {
                        this.Pub1.AddTREnd();
                        colIdx = -1;
                    }
                }
                cbx.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')";

                if (colIdx != -1)
                {
                    while (colIdx != col - 1)
                    {
                        colIdx++;
                        this.Pub1.AddTD();
                    }
                    this.Pub1.AddTREnd();
                }
                this.Pub1.AddTableEnd();
                this.Pub1.AddTDEnd();
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();

            this.BindEnd();
        }

        public void BindBySQL_Tree(DataTable dtGroup, DataTable dtDB)
        {
        }

        public string BindByStation()
        {
            return GetTreeJsonByTable(this.GetTable(), "No", "Name", "ParentNo", "0", "IsParent", "");


            //DataTable dt = this.GetTable(); // Get a list of staff .
            //SelectAccpers accps = new SelectAccpers();
            //accps.QueryAccepter(this.FK_Node, WebUser.No, this.WorkID);

            //Dept dept = new Dept();
            //string fk_dept = "";
            //string info = "";

            //if (IsMultiple)
            //    this.Pub1.AddTable("width=400px");
            //else
            //    this.Pub1.AddTable("width=100%");

            //if (WebUser.FK_Dept.Length > 2)
            //{
            //    if (this.FK_Dept == WebUser.FK_Dept)
            //        info = "<b><a href='Accepter.aspx?ToNode=" + this.ToNode + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&type=1&FK_Dept=" + WebUser.FK_Dept.Substring(0, WebUser.FK_Dept.Length - 2) + "'> On a department staff </b></a>|<b><a href='Accepter.aspx?ToNode=" + this.ToNode + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&type=1&FK_Dept=" + this.FK_Dept + "&IsNextDept=1' > Under a department staff </b></a>";
            //    else
            //        info = "<b><a href='Accepter.aspx?ToNode=" + this.ToNode + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&type=1&FK_Dept=" + WebUser.FK_Dept + "'> The department staff </a></b>";
            //}
            //else
            //{
            //    info = "<b><a href='Accepter.aspx?ToNode=" + this.ToNode + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&type=1&FK_Dept=" + WebUser.FK_Dept + "'> The department staff </a> | <a href='Accepter.aspx?ToNode=" + this.ToNode + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&type=1&FK_Dept=" + this.FK_Dept + "&IsNextDept=1' > Under a department staff </b></a>";
            //}


            //BP.WF.Node toNode = new BP.WF.Node(this.ToNode);
            //this.Pub1.AddCaptionLeft("<span style='color:red'> Arrives at a node :[" + toNode.Name + "]</span>");
            //this.Pub1.AddCaptionLeft(" Selectable range :" + dt.Rows.Count + " 位." + info);

            //if (dt.Rows.Count > 50)
            //{
            //    /* More than a certain number of , To display the navigation .*/
            //    this.Pub1.AddTRSum();
            //    this.Pub1.Add("<TD class=BigDoc colspan=5>");
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (fk_dept != dr["FK_Dept"].ToString())
            //        {
            //            fk_dept = dr["FK_Dept"].ToString();
            //            dept = new Dept(fk_dept);
            //            dr["DeptName"] = dept.Name;
            //            this.Pub1.Add("<a href='#d" + dept.No + "' >" + dept.Name + "</a>&nbsp;");
            //        }
            //    }
            //    this.Pub1.AddTDEnd();
            //    this.Pub1.AddTREnd();
            //}

            //int idx = -1;
            //bool is1 = false;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    idx++;
            //    if (fk_dept != dr["FK_Dept"].ToString())
            //    {
            //        switch (idx)
            //        {
            //            case 0:
            //                break;
            //            case 1:
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTREnd();
            //                break;
            //            case 2:
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTREnd();
            //                break;
            //            case 3:
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTREnd();
            //                break;
            //            case 4:
            //                this.Pub1.AddTD();
            //                this.Pub1.AddTREnd();
            //                break;
            //            default:
            //                throw new Exception("error");
            //        }

            //        this.Pub1.AddTRSum();
            //        fk_dept = dr["FK_Dept"].ToString();
            //        string deptName = dr["DeptName"].ToString();
            //        this.Pub1.AddTD("colspan=5 aligen=left class=FDesc ", "<a name='d" + dept.No + "'>" + deptName + "</a>");
            //        this.Pub1.AddTREnd();
            //        is1 = false;
            //        idx = 0;
            //    }

            //    string no = dr["No"].ToString();
            //    string name = dr["Name"].ToString();

            //    CheckBox cb = new CheckBox();
            //    cb.Text = BP.WF.Glo.DealUserInfoShowModel(no, name);

            //    cb.ID = "CB_" + no;
            //    if (accps.Contains("FK_Emp", no))
            //        cb.Checked = true;

            //    switch (idx)
            //    {
            //        case 0:
            //            is1 = this.Pub1.AddTR(is1);
            //            this.Pub1.AddTD(cb);
            //            break;
            //        case 1:
            //        case 2:
            //        case 3:
            //            this.Pub1.AddTD(cb);
            //            break;
            //        case 4:
            //            this.Pub1.AddTD(cb);
            //            this.Pub1.AddTREnd();
            //            idx = -1;
            //            break;
            //        default:
            //            throw new Exception("error");
            //    }
            //    this.Pub1.AddTREnd();
            //}
            //this.Pub1.AddTableEnd();

            //this.BindEnd();
        }
        /// <summary>
        ///  End processing Bind 
        /// </summary>
        public void BindEnd()
        {
            Button btn = new Button();
            if (this.IsWinOpen == 1)
            {
                btn.Text = " OK and Close ";
                btn.ID = "Btn_Save";
                btn.CssClass = "Btn";
                btn.Click += new EventHandler(btn_Save_Click);
                this.Pub1.Add(btn);
            }
            else
            {
                btn = new Button();
                btn.Text = " Identify and send ";
                btn.ID = "Btn_Save";
                btn.CssClass = "Btn";
                btn.Click += new EventHandler(btn_Save_Click);
                this.Pub1.Add(btn);

                btn = new Button();
                btn.Text = " Cancellation and return ";
                btn.ID = "Btn_Cancel";
                btn.CssClass = "Btn";
                btn.Click += new EventHandler(btn_Save_Click);
                this.Pub1.Add(btn);
            }

            CheckBox mycb = new CheckBox();
            mycb.ID = "CB_IsSetNextTime";
            mycb.Text = " After sending are calculated in accordance with this set ";
            this.Pub1.Add(mycb);

            //CheckBox mycb = new CheckBox();
            //mycb.ID = "CB_IsSetNextTime";
            //mycb.Text = " After sending are calculated in accordance with this set ";
            //mycb.Checked = accps.IsSetNextTime;
            //this.Pub1.Add(mycb);

        }
        // Save 
        public void saveMet()
        {
            string getSaveNo = getUTF8ToString("getSaveNo");

            // Here to make a judgment , Delete checked Sectoral data 
            string[] getSaveNoArray = getSaveNo.Split(',');
            List<string> getSaveNoList = new List<string>();

            for (int i = 0; i < getSaveNoArray.Length; i++)
            {
                getSaveNoList.Add(getSaveNoArray[i]);
            }

            getSaveNo = null;
            string ziFu = ",";
            for (int i = 0; i < getSaveNoList.Count; i++)
            {
                if (i == getSaveNoList.Count - 1)
                {
                    ziFu = null;
                }
                getSaveNo += (getSaveNoList[i] + ziFu);
            }

            // Setting staff .
            BP.WF.Dev2Interface.WorkOpt_SetAccepter(this.ToNode, this.WorkID, this.FID, getSaveNo, false);





            if (this.IsWinOpen == 0)
            {
                /* In the case of  MyFlow.aspx  Calls ,  Send logic should call . */
                this.DoSend();
                return;
            }


            if (this.Request.QueryString["IsEUI"] == null)
            {
                this.WinClose();
            }
            else
            {
                PubClass.ResponseWriteScript("window.parent.$('windowIfrem').window('close');");

            }

#warning  Liu Wenhui   After you save the recipient calls the send button 

            BtnLab nd = new BtnLab(this.FK_Node);
            if (nd.SelectAccepterEnable == 1)
            {
                if (this.Request.QueryString["IsEUI"] == null)
                {

                    /* In the case of 1 It does not explain directly off .*/
                    this.WinClose();
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "", "this.close();", true);
                }
                else
                {
                    PubClass.ResponseWriteScript("window.parent.$('windowIfrem').window('close');");

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "", "send();", true);
            }
        }

        /// <summary>
        ///  Save 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_Save_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.ID == "Btn_Cancel")
            {
                string url = "../MyFlow.aspx?FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID + "&FID=" + this.FID;
                this.Response.Redirect(url, true);
                return;
            }

            //DataTable dt = this.GetTable();
            string emps = "";
            foreach (Control ctl in this.Pub1.Controls)
            {
                CheckBox cb = ctl as CheckBox;
                if (cb == null || cb.ID == null || cb.ID.Contains("CBs_") || cb.ID == "CB_IsSetNextTime")
                    continue;

                if (cb.Checked == false)
                    continue;
                emps += cb.ID.Replace("CB_", "") + ",";
            }

            if (emps.Length < 2)
            {
                this.Alert(" You did not select staff .");
                return;
            }

            // Gets whether the next automatic settings .
            bool isNextTime = this.Pub1.GetCBByID("CB_IsSetNextTime").Checked;

            // Setting staff .
            BP.WF.Dev2Interface.WorkOpt_SetAccepter(this.ToNode, this.WorkID, this.FID, emps, isNextTime);

            if (this.IsWinOpen == 0)
            {
                /* In the case of  MyFlow.aspx  Calls ,  Send logic should call . */
                this.DoSend();
                return;
            }


            if (this.Request.QueryString["IsEUI"] == null)
            {
                this.WinClose();
            }
            else
            {
                PubClass.ResponseWriteScript("window.parent.$('windowIfrem').window('close');");
            }

#warning  Liu Wenhui   After you save the recipient calls the send button 

            BtnLab nd = new BtnLab(this.FK_Node);
            if (nd.SelectAccepterEnable == 1)
            {
                if (this.Request.QueryString["IsEUI"] == null)
                {

                    /* In the case of 1 It does not explain directly off .*/
                    this.WinClose();
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "", "this.close();", true);
                }
                else
                {
                    PubClass.ResponseWriteScript("window.parent.$('windowIfrem').window('close');");

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "", "send();", true);
            }
        }

        public void DoSend()
        {
            //  The following code is from  MyFlow.aspx Send  Method copy  Over , The need to maintain the consistency of the business logic , So the code needs to be consistent .

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            Work wk = nd.HisWork;
            wk.OID = this.WorkID;
            wk.Retrieve();

            WorkNode firstwn = new WorkNode(wk, nd);
            string msg = "";
            try
            {
                msg = firstwn.NodeSend().ToMsgOfHtml();
            }
            catch (Exception exSend)
            {
                this.Pub1.AddFieldSetGreen(" Error ");
                this.Pub1.Add(exSend.Message.Replace("@@", "@").Replace("@", "<BR>@"));
                this.Pub1.AddFieldSetEnd();
                return;
            }

            #region  Business logic methods to handle the general sent successfully after , This method may throw an exception .
            try
            {
                // Business logic methods to handle the general sent successfully after , This method may throw an exception .
                BP.WF.Glo.DealBuinessAfterSendWork(this.FK_Flow, this.WorkID, this.DoFunc, WorkIDs, this.CFlowNo, 0, null);
            }
            catch (Exception ex)
            {
                this.ToMsg(msg, ex.Message);
                return;
            }
            #endregion  Business logic methods to handle the general sent successfully after , This method may throw an exception .


            /* Processing steering problems .*/
            switch (firstwn.HisNode.HisTurnToDeal)
            {
                case TurnToDeal.SpecUrl:
                    string myurl = firstwn.HisNode.TurnToDealDoc.Clone().ToString();
                    if (myurl.Contains("&") == false)
                        myurl += "?1=1";
                    Attrs myattrs = firstwn.HisWork.EnMap.Attrs;
                    Work hisWK = firstwn.HisWork;
                    foreach (Attr attr in myattrs)
                    {
                        if (myurl.Contains("@") == false)
                            break;
                        myurl = myurl.Replace("@" + attr.Key, hisWK.GetValStrByKey(attr.Key));
                    }
                    if (myurl.Contains("@"))
                        throw new Exception(" Process design errors , In node steering url The parameters are not to be replaced .Url:" + myurl);

                    myurl += "&FromFlow=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                    this.Response.Redirect(myurl, true);
                    return;
                case TurnToDeal.TurnToByCond:
                    TurnTos tts = new TurnTos(this.FK_Flow);
                    if (tts.Count == 0)
                        throw new Exception("@ You do not turn to the conditions set after the completion of the node .");
                    foreach (TurnTo tt in tts)
                    {
                        tt.HisWork = firstwn.HisWork;
                        if (tt.IsPassed == true)
                        {
                            string url = tt.TurnToURL.Clone().ToString();
                            if (url.Contains("&") == false)
                                url += "?1=1";
                            Attrs attrs = firstwn.HisWork.EnMap.Attrs;
                            Work hisWK1 = firstwn.HisWork;
                            foreach (Attr attr in attrs)
                            {
                                if (url.Contains("@") == false)
                                    break;
                                url = url.Replace("@" + attr.Key, hisWK1.GetValStrByKey(attr.Key));
                            }
                            if (url.Contains("@"))
                                throw new Exception(" Process design errors , In node steering url The parameters are not to be replaced .Url:" + url);

                            url += "&PFlowNo=" + this.FK_Flow + "&FromNode=" + this.FK_Node + "&PWorkID=" + this.WorkID + "&UserNo=" + WebUser.No + "&SID=" + WebUser.SID;
                            this.Response.Redirect(url, true);
                            return;
                        }
                    }
#warning  For Shanghai modify it if you can not find the path information prompted by the system .
                    this.ToMsg(msg, "info");
                    //throw new Exception(" You define the steering condition is not satisfied , No Exit .");
                    break;
                default:
                    this.ToMsg(msg, "info");
                    break;
            }
            return;
        }

        public void ToMsg(string msg, string type)
        {
            this.Session["info"] = msg;
            this.Application["info" + WebUser.No] = msg;

            BP.WF.Glo.SessionMsg = msg;
            this.Response.Redirect("./../MyFlowInfo" + BP.WF.Glo.FromPageType + ".aspx?FK_Flow=" + this.FK_Flow + "&FK_Type=" + type + "&FK_Node=" + this.FK_Node + "&WorkID=" + this.WorkID, false);
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
        public string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string IsParent, string CheckedString)
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
                if (rows.Length > 0)// Modification 
                {
                    foreach (DataRow row in rows)
                    {
                        string deptNo = row[idCol].ToString();

                        if (treeResult.Length == 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                    + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                                    + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                        }
                        else if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                    + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                                + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                        }
                        else
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                    + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                              + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower());
                        }


                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], IsParent, CheckedString);
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