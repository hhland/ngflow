using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP.GPM;
using BP.En;
using BP.DA;
using BP.Web;
using BP.Port;

namespace BP.WF.Template
{
    /// <summary>
    ///  Someone Rules 
    /// </summary>
    public class FindWorker
    {
        #region  Variable 
        public WorkNode town = null;
        public WorkNode currWn = null;
        public Flow fl = null;
        string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
        public Paras ps = null;
        string JumpToEmp = null;
        int JumpToNode = 0;
        Int64 WorkID = 0;
        #endregion  Variable 

        /// <summary>
        ///  Find people 
        /// </summary>
        /// <param name="fl"></param>
        /// <param name="currWn"></param>
        /// <param name="toWn"></param>
        public FindWorker()
        {
        }
        private DataTable FindByWorkFlowModel()
        {
            this.town = town;

            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            string sql;
            string FK_Emp;

            //  If you do send twice , Previous trajectory that would need to be removed , Here is to avoid errors .
            ps = new Paras();
            ps.Add("WorkID", this.WorkID);
            ps.Add("FK_Node", town.HisNode.NodeID);
            ps.SQL = "DELETE FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node =" + dbStr + "FK_Node";
            DBAccess.RunSQL(ps);

            //  If you specify a particular staff to handle .
            if (string.IsNullOrEmpty(JumpToEmp) == false)
            {
                string[] emps = JumpToEmp.Split(',');
                foreach (string emp in emps)
                {
                    if (string.IsNullOrEmpty(emp))
                        continue;
                    DataRow dr = dt.NewRow();
                    dr[0] = emp;
                    dt.Rows.Add(dr);
                }
                return  dt;
            }

            //  Press a sender node processing .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByPreviousNodeEmp)
            {
                DataRow dr = dt.NewRow();
                dr[0] = BP.Web.WebUser.No;
                dt.Rows.Add(dr);
                return dt;
            }
             

            // First, determine whether the configuration of obtaining Next to accept the staff sql.
            if (town.HisNode.HisDeliveryWay == DeliveryWay.BySQL
                || town.HisNode.HisDeliveryWay == DeliveryWay.BySQLAsSubThreadEmpsAndData)
            {
                if (town.HisNode.DeliveryParas.Length < 4)
                    throw new Exception("@ As you set the current node SQL, Decided to accept the next staff , But you do not set sql.");

                sql = town.HisNode.DeliveryParas;
                sql = sql.Clone().ToString();

                sql = Glo.DealExp(sql, this.currWn.rptGe, null);
                if (sql.Contains("@"))
                {
                    if (Glo.SendHTOfTemp != null)
                    {
                        foreach (string key in Glo.SendHTOfTemp.Keys)
                        {
                            sql = sql.Replace("@" + key, Glo.SendHTOfTemp[key].ToString());
                        }
                    }
                }

                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count == 0 && town.HisNode.HisWhenNoWorker != WhenNoWorker.Skip)
                    throw new Exception("@ Staff did not find acceptable .@ Technical Information : Execution sql Did not find staff :" + sql);
                return dt;
            }

            #region  According to the schedule , As a child thread recipient .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByDtlAsSubThreadEmps)
            {
                if (this.town.HisNode.HisRunModel != RunModel.SubThread)
                    throw new Exception("@ Node receives one way you have set for : Diversion point to the data source to determine the form of a list of recipients child thread , But the current node node Feizi thread .");
                
                BP.Sys.MapDtls dtls = new BP.Sys.MapDtls(this.currWn.HisNode.NodeFrmID);
                string msg = null;
                foreach (BP.Sys.MapDtl dtl in dtls)
                {
                    try
                    {
                        ps = new Paras();
                        ps.SQL = "SELECT UserNo FROM " + dtl.PTable + " WHERE RefPK=" + dbStr + "OID ORDER BY OID";
                        ps.Add("OID", this.WorkID);
                        dt = DBAccess.RunSQLReturnTable(ps);
                        if (dt.Rows.Count == 0 && town.HisNode.HisWhenNoWorker != WhenNoWorker.Skip)
                            throw new Exception("@ Process design errors , Arrival node （" + town.HisNode.Name + "） No data at the specified node , Staff can not find the child threads .");
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        msg += ex.Message;
                        //if (dtls.Count == 1)
                        //    throw new Exception("@ It is estimated that the process of design errors , There is no set schedule shunt node ");
                    }
                }
                throw new Exception("@ The list did not find the shunt node initiating child thread as a data source , Process design errors , Make sure that the form of the shunt node list if there UserNo Fields agreed system ."+msg);
            }
            #endregion  According to the schedule , As a child thread recipient .

            #region  By staff processing node binding .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByBindEmp)
            {
                ps = new Paras();
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.SQL = "SELECT FK_Emp FROM WF_NodeEmp WHERE FK_Node=" + dbStr + "FK_Node ORDER BY FK_Emp";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0 )
                    throw new Exception("@ Process design errors : Next node (" + town.HisNode.Name + ") Staff is not bound  . ");
                return dt;
            }
            #endregion  By staff processing node binding .

            #region  According to the selected staff to handle .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.BySelected)
            {
                ps = new Paras();
                ps.Add("FK_Node", this.town.HisNode.NodeID);
                ps.Add("WorkID", this.currWn.HisWork.OID);
                ps.SQL = "SELECT FK_Emp FROM WF_SelectAccper WHERE FK_Node=" + dbStr + "FK_Node AND WorkID=" + dbStr + "WorkID AND AccType=0 ORDER BY IDX";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    /* Last queries sent from the local settings . */
                    SelectAccpers sas = new SelectAccpers();
                    int i = sas.QueryAccepterPriSetting(this.town.HisNode.NodeID);
                    if (i == 0)
                        throw new Exception(" Please select the next step to work (" + town.HisNode.Name + ") Acceptance of staff ."); //

                    // Insert inside .
                    foreach (SelectAccper item in sas)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = item.FK_Emp;
                        dt.Rows.Add(dr);
                    }
                    return dt;
                }
                return dt;
            }
            #endregion  According to the selected staff to handle .

            #region  Calculated in accordance with the specified node processors .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.BySpecNodeEmp
                || town.HisNode.HisDeliveryWay == DeliveryWay.ByStarter)
            {
                /*  Calculated according to staff positions on the specified node  */
                string strs = "";
                if (town.HisNode.HisDeliveryWay == DeliveryWay.ByStarter)
                    strs = int.Parse(this.fl.No)+"01";
                else
                    strs=town.HisNode.DeliveryParas;

                strs = strs.Replace(";", ",");
                string[] nds = strs.Split(',');
                foreach (string nd in nds)
                {
                    if (string.IsNullOrEmpty(nd))
                        continue;

                    if (DataType.IsNumStr(nd) == false)
                        throw new Exception(" Process design errors : You set the node (" + town.HisNode.Name + ") The receiving mode for the specified node post delivery , But you do not set the node number in the access rule settings .");

                    ps = new Paras();
                    ps.SQL = "SELECT FK_Emp FROM WF_GenerWorkerList WHERE WorkID=" + dbStr + "OID AND FK_Node=" + dbStr + "FK_Node AND IsPass=1 AND IsEnable=1 ";
                    ps.Add("FK_Node", int.Parse(nd));
                    if (this.currWn.HisNode.HisRunModel == RunModel.SubThread)
                        ps.Add("OID", this.currWn.HisWork.FID);
                    else
                        ps.Add("OID", this.WorkID);

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 1)
                        return dt;

                    // Necessary to check the track list , Because there may be skipped node .
                    ps = new Paras();
                    ps.SQL = "SELECT " + TrackAttr.EmpTo + " FROM ND" + int.Parse(fl.No) + "Track WHERE ActionType=" + dbStr + "ActionType AND NDTo=" + dbStr + "NDTo AND WorkID=" + dbStr + "WorkID";
                    ps.Add("ActionType", (int)ActionType.Skip);
                    ps.Add("NDTo", int.Parse(nd));
                    ps.Add("WorkID", this.WorkID);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count != 0)
                        return dt;
                }

                throw new Exception("@ Process design errors , Arrival node （" + town.HisNode.Name + "） In the specified node (" + strs + ") No data , People unable to find work . Delivery methods :BySpecNodeEmp sql=" + ps.SQL);
            }
            #endregion  According to staff processing node binding .

            #region  According to staff on a specified field of processing nodes form .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByPreviousNodeFormEmpsField)
            {
                //  Checks accepted personnel rules , Meets the design requirements .
                string specEmpFields = town.HisNode.DeliveryParas;
                if (string.IsNullOrEmpty(specEmpFields))
                    specEmpFields = "SysSendEmps";

                if (this.currWn.HisWork.EnMap.Attrs.Contains(specEmpFields) == false)
                    throw new Exception("@ You set the current node in the specified personnel , Decided to accept the next staff , But you do not set the form in the node form " + specEmpFields + " Field .");

                // Get the recipient and the recipient format , 
                string emps = this.currWn.HisWork.GetValStringByKey(specEmpFields);
                emps = emps.Replace(" ", "");
                if (emps.Contains(",") && emps.Contains(";"))
                {
                    /* If you include ,;  Such as  zhangsan, Joe Smith ;lisi, John Doe ;*/
                    string[] myemps1 = emps.Split(';');
                    foreach (string str in    myemps1)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        string[] ss = str.Split(',');
                        DataRow dr = dt.NewRow();
                        dr[0] = ss[0];
                        dt.Rows.Add(dr);
                    }
                    if (dt.Rows.Count == 0)
                        throw new Exception("@ Acceptance of personnel information input error ;[" + emps + "].");
                    else
                        return dt;
                }

                emps = emps.Replace(";", ",");
                emps = emps.Replace(";", ",");
                emps = emps.Replace(",", ",");
                emps = emps.Replace(",", ",");
                emps = emps.Replace("@", ",");

                if (string.IsNullOrEmpty(emps))
                    throw new Exception("@ No in field [" + this.currWn.HisWork.EnMap.Attrs.GetAttrByKey(specEmpFields).Desc + "] Specified recipient , Work can not be sent down .");

                //  Add it to accept the staff list .
                string[] myemps = emps.Split(',');
                foreach (string s in myemps)
                {
                    if (string.IsNullOrEmpty(s))
                        continue;

                    //if (BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(NO) AS NUM FROM Port_Emp WHERE NO='" + s + "' or name='"+s+"'", 0) == 0)
                    //    continue;

                    DataRow dr = dt.NewRow();
                    dr[0] = s;
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            #endregion  According to staff on a specified field of processing nodes form .

            string prjNo = "";
            FlowAppType flowAppType = this.currWn.HisNode.HisFlow.HisFlowAppType;
            sql = "";
            if (this.currWn.HisNode.HisFlow.HisFlowAppType == FlowAppType.PRJ)
            {
                prjNo = "";
                try
                {
                    prjNo = this.currWn.HisWork.GetValStrByKey("PrjNo");
                }
                catch (Exception ex)
                {
                    throw new Exception("@ The current process is the engineering process , But no node form PrjNo Field ( Case sensitive ), Please confirm .@ Exception Information :" + ex.Message);
                }
            }

            #region  By department and job intersection of computing .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByDeptAndStation)
            {
                sql = "SELECT No FROM Port_Emp WHERE No IN ";
                sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                sql += ")";
                sql += "AND No IN ";
                sql += "(";
                sql += "SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ";
                sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node2 )";
                sql += ") ORDER BY No ";

                ps = new Paras();
                ps.Add("FK_Node1", town.HisNode.NodeID);
                ps.Add("FK_Node2", town.HisNode.NodeID);
                ps.SQL = sql;
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@ Node access rules error : Node (" + town.HisNode.NodeID + "," + town.HisNode.Name + "),  According to the intersection with the department job to determine the recipient of range error , Did not find the staff :SQL=" + sql);
            }
            #endregion  By department and job intersection of computing .

            #region  Determine which node department is set up department , If the , In accordance with its departments .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByDept)
            {
                if (flowAppType == FlowAppType.Normal)
                {
                    ps = new Paras();
                    ps.SQL = "SELECT No,Name FROM Port_Emp WHERE FK_Dept IN (SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                    ps.SQL += " OR ";
                    ps.SQL += " No IN (SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node2 ) )";
                    ps.SQL += " ORDER BY No";
                    ps.Add("FK_Node1", town.HisNode.NodeID);
                    ps.Add("FK_Node2", town.HisNode.NodeID);

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count > 0  && town.HisNode.HisWhenNoWorker != WhenNoWorker.Skip)
                    {
                        return dt;
                    }
                    else
                    {
                        //IsFindWorker = false;
                        //  ps.SQL = "SELECT No,Name FROM Port_Emp WHERE FK_Dept IN ( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node )";
                        throw new Exception("@ By department to determine the scope of the recipient , Did not find the staff .");
                    }
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    sql = "SELECT No FROM Port_Emp WHERE No IN ";
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                    sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                    sql += ")";
                    sql += "AND NO IN ";
                    sql += "(";
                    sql += "SELECT FK_Emp FROM Prj_EmpPrjStation WHERE FK_Station IN ";
                    sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node2) AND FK_Prj=" + dbStr + "FK_Prj ";
                    sql += ")";
                    sql += " ORDER BY No";

                    ps = new Paras();
                    ps.Add("FK_Node1", town.HisNode.NodeID);
                    ps.Add("FK_Node2", town.HisNode.NodeID);
                    ps.Add("FK_Prj", prjNo);
                    ps.SQL = sql;

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /*  If the project group would submit to no staff in the public sector to find .*/
                        sql = "SELECT NO FROM Port_Emp WHERE NO IN ";
                        sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept IN ";
                        sql += "( SELECT FK_Dept FROM WF_NodeDept WHERE FK_Node=" + dbStr + "FK_Node1)";
                        sql += ")";
                        sql += "AND NO IN ";
                        sql += "(";
                        sql += "SELECT FK_Emp FROM Port_EmpStation WHERE FK_Station IN ";
                        sql += "( SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node2)";
                        sql += ")";
                        sql += " ORDER BY No";

                        ps = new Paras();
                        ps.Add("FK_Node1", town.HisNode.NodeID);
                        ps.Add("FK_Node2", town.HisNode.NodeID);
                        ps.SQL = sql;
                    }
                    else
                    {
                        return  dt;
                    }

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count > 0)
                        return dt;
                }
            }
            #endregion  Determine which node department is set up department , If the , In accordance with its departments .

            #region  Only post by computing 
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByStationOnly)
            {
                sql = "SELECT A.FK_Emp FROM Port_EmpStation A, WF_NodeStation B WHERE A.FK_Station=B.FK_Station AND B.FK_Node=" + dbStr + "FK_Node ORDER BY A.FK_Emp";
                ps = new Paras();
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.SQL = sql;
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@ Node access rules error : Node (" + town.HisNode.NodeID + "," + town.HisNode.Name + "),  By node positions and personnel department set two latitude computing , Did not find the staff :SQL=" + sql);
            }
            #endregion

            #region  Calculation by post ( Sectoral set latitude ).
            if (town.HisNode.HisDeliveryWay == DeliveryWay.ByStationAndEmpDept)
            {
                sql = "SELECT No FROM Port_Emp WHERE NO IN "
                      + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node) )"
                      + " AND  FK_Dept IN "
                      + "(SELECT  FK_Dept  FROM Port_EmpDept WHERE FK_Emp =" + dbStr + "FK_Emp)";

                sql += " ORDER BY No";

                ps = new Paras();
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.Add("FK_Emp", WebUser.No);
                ps.SQL = sql;
                //2012.7.16 Li Jian modification 
                //+" AND  NO IN "
                //+ "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Emp = '" + WebUser.No + "')";
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    throw new Exception("@ Node access rules error : Node (" + town.HisNode.NodeID + "," + town.HisNode.Name + "),  By node positions and personnel department set two latitude computing , Did not find the staff :SQL=" + sql);
            }
            #endregion

            string empNo = WebUser.No;
            string empDept = WebUser.FK_Dept;

            #region  Press staff positions specified node , As the next step of the process to accept the people .
            if (town.HisNode.HisDeliveryWay == DeliveryWay.BySpecNodeEmpStation)
            {
                /*  Press staff positions specified node  */
                string fk_node = town.HisNode.DeliveryParas;
                if (DataType.IsNumStr(fk_node) == false)
                    throw new Exception(" Process design errors : You set the node (" + town.HisNode.Name + ") The receiving mode for the specified node delivery staff positions , But you do not set the node number in the access rule settings .");

                ps = new Paras();
                ps.SQL = "SELECT Rec,FK_Dept FROM ND" + fk_node + " WHERE OID=" + dbStr + "OID";
                ps.Add("OID", this.WorkID);
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count != 1)
                    throw new Exception("@ Process design errors , Arrival node （" + town.HisNode.Name + "） No data at the specified node , People unable to find work .");

                empNo = dt.Rows[0][0].ToString();
                empDept = dt.Rows[0][1].ToString();
            }
            #endregion  Persons specified node , As the next step of the process to accept the people .

            #region  Final judgment  -  In accordance with the job to execute .
            if (this.currWn.HisNode.IsStartNode == false)
            {
                ps = new Paras();
                if (flowAppType == FlowAppType.Normal || flowAppType == FlowAppType.DocFlow)
                {
                    //  If the current node is not the start node ,  Query from the track inside .
                    sql = "SELECT DISTINCT FK_Emp  FROM Port_EmpStation WHERE FK_Station IN "
                       + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + town.HisNode.NodeID + ") "
                       + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(town.HisNode.GroupStaNDs, true) + ") )";

                    sql += " ORDER BY FK_Emp ";

                    ps.SQL = sql;
                    ps.Add("WorkID", this.WorkID);
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    //  If the current node is not the start node ,  Query from the track inside .
                    sql = "SELECT DISTINCT FK_Emp  FROM Prj_EmpPrjStation WHERE FK_Station IN "
                       + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) AND FK_Prj=" + dbStr + "FK_Prj "
                       + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(town.HisNode.GroupStaNDs, true) + ") )";
                    sql += " ORDER BY FK_Emp ";

                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Node", town.HisNode.NodeID);
                    ps.Add("FK_Prj", prjNo);
                    ps.Add("WorkID", this.WorkID);

                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /*  If the project group would submit to no staff in the public sector to find .*/
                        sql = "SELECT DISTINCT FK_Emp  FROM Port_EmpStation WHERE FK_Station IN "
                         + "(SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) "
                         + "AND FK_Emp IN (SELECT FK_Emp FROM WF_GenerWorkerlist WHERE WorkID=" + dbStr + "WorkID AND FK_Node IN (" + DataType.PraseAtToInSql(town.HisNode.GroupStaNDs, true) + ") )";
                        sql += " ORDER BY FK_Emp ";

                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", town.HisNode.NodeID);
                        ps.Add("WorkID", this.WorkID);
                    }
                    else
                    {
                        return  dt;
                    }
                }

                dt = DBAccess.RunSQLReturnTable(ps);
                //  If you can find .
                if (dt.Rows.Count >= 1)
                {
                    if (dt.Rows.Count == 1)
                    {
                        /* If a person is only the case , That he may have to  */
                    }
                    return dt;
                }
            }

            /*  If the node is executed  与  Consistent set of nodes accept jobs  */
            if (this.currWn.HisNode.GroupStaNDs == town.HisNode.GroupStaNDs)
            {
                /*  Explanation , Put the current personnel as the next node processors .*/
                DataRow dr = dt.NewRow();
                dr[0] = WebUser.No;
                dt.Rows.Add(dr);
                return dt;
            }

            /*  If the node is executed  与  Accept node positions inconsistent collection  */
            if (this.currWn.HisNode.GroupStaNDs != town.HisNode.GroupStaNDs)
            {
                /*  Under no inquiry into the circumstances ,  According to the department to calculate .*/
                if (flowAppType == FlowAppType.Normal)
                {
                    switch (BP.Sys.SystemConfig.AppCenterDBType)
                    {
                        case DBType.MySQL:
                        case DBType.MSSQL:
                            sql = "select No from Port_Emp x inner join (select FK_Emp from Port_EmpStation a inner join WF_NodeStation b ";
                            sql += " on a.FK_Station=b.FK_Station where FK_Node=" + dbStr + "FK_Node) as y on x.No=y.FK_Emp inner join Port_EmpDept z on";
                            sql += " x.No=z.FK_Emp where z.FK_Dept =" + dbStr + "FK_Dept order by x.No";
                            break;
                        default:
                            sql = "SELECT No FROM Port_Emp WHERE NO IN "
                          + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node) )"
                          + " AND  NO IN "
                          + "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Dept =" + dbStr + "FK_Dept)";
                            sql += " ORDER BY No ";
                            break;
                    }

                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Node", town.HisNode.NodeID);
                    ps.Add("FK_Dept", empDept);
                }

                if (flowAppType == FlowAppType.PRJ)
                {
                    sql = "SELECT  FK_Emp  FROM Prj_EmpPrjStation WHERE FK_Prj=" + dbStr + "FK_Prj1 AND FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node)"
                    + " AND  FK_Prj=" + dbStr + "FK_Prj2 ";
                    sql += " ORDER BY FK_Emp ";

                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Prj1", prjNo);
                    ps.Add("FK_Node", town.HisNode.NodeID);
                    ps.Add("FK_Prj2", prjNo);
                    dt = DBAccess.RunSQLReturnTable(ps);
                    if (dt.Rows.Count == 0)
                    {
                        /*  If the project group would submit to no staff in the public sector to find . */
                        sql = "SELECT No FROM Port_Emp WHERE NO IN "
                      + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node))"
                      + " AND  NO IN "
                      + "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Dept =" + dbStr + "FK_Dept)";

                        sql += " ORDER BY No ";

                        ps = new Paras();
                        ps.SQL = sql;
                        ps.Add("FK_Node", town.HisNode.NodeID);
                        ps.Add("FK_Dept", empDept);
                        //  dt = DBAccess.RunSQLReturnTable(ps);
                    }
                    else
                    {
                        return dt;
                    }
                }

                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    NodeStations nextStations = town.HisNode.NodeStations;
                    if (nextStations.Count == 0)
                        throw new Exception(" Node has no job :" + town.HisNode.NodeID + "  " + town.HisNode.Name);
                    //else
                    //    return dt;
                }
                else
                {
                    bool isInit = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[0].ToString() == BP.Web.WebUser.No)
                        {
                            /*  If the job is not the same group , And result in the collection as well as the current staff , To explain the emergence of the current operator , Has a job on the local node to the next node also has a job 
                              Resulting in : Different groups of nodes , Delivered to the same person . */
                            isInit = true;
                        }
                    }
#warning edit by peng,  Used to determine the transfer of different positions set contains the same individual approach .
                    //  if (isInit == false || isInit == true)
                    return dt;
                }
            }

            //  Under no inquiry into the circumstances ,  Membership subordinate departments to execute a query in this sector .
            if (flowAppType == FlowAppType.Normal)
            {
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                    case DBType.MySQL:
                        sql = "SELECT No FROM Port_Emp x inner join "
                   + " (select FK_Emp from Port_empStation a inner join WF_NodeStation b on a.FK_Station=b.FK_Station where b.FK_Node=" + town.HisNode.NodeID + ") as y on x.No=y.FK_Emp "
                   + "  inner join Port_EmpDept z on x.No= z.FK_Emp and z.FK_Dept LIKE '" + empDept + "%' "
                   + " where x.No!=" + dbStr + "FK_Emp";
                        sql += " ORDER BY x.No ";
                        break;
                    default:
                        sql = "SELECT No FROM Port_Emp WHERE NO IN "
                   + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + town.HisNode.NodeID + ") )"
                   + " AND  NO IN "
                   + "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Dept LIKE '" + empDept + "%')"
                   + " AND No!=" + dbStr + "FK_Emp";
                        sql += " ORDER BY No ";
                        break;
                }

                ps = new Paras();
                ps.SQL = sql;
                ps.Add("FK_Emp", empNo);

            }

            if (flowAppType == FlowAppType.PRJ)
            {
                sql = "SELECT  FK_Emp  FROM Prj_EmpPrjStation WHERE FK_Prj=" + dbStr + "FK_Prj1 AND FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node )"
                    + " AND  FK_Prj=" + dbStr + "FK_Prj2 ";
                sql += " ORDER BY FK_Emp ";

                ps = new Paras();
                ps.SQL = sql;
                ps.Add("FK_Prj1", prjNo);
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.Add("FK_Prj2", prjNo);
                dt = DBAccess.RunSQLReturnTable(ps);
                if (dt.Rows.Count == 0)
                {
                    /*  If the project group would submit to no staff in the public sector to find .*/
                    switch (BP.Sys.SystemConfig.AppCenterDBType)
                    {
                        case DBType.MySQL:
                        case DBType.MSSQL:
                            sql = "SELECT No FROM Port_Emp x inner join "
                   + "(select FK_Emp from Port_empStation a inner join WF_NodeStation b on a.FK_Station=b.FK_Station where b.FK_Node=" + dbStr + "FK_Node) as y on x.No=y.FK_Emp "
                   + "  inner join Port_EmpDept z on x.No= z.FK_Emp and z.FK_Dept LIKE '" + empDept + "%' "
                   + "  where x.No!=" + dbStr + "FK_Emp";
                            sql += " ORDER BY x.No ";
                            break;
                        default:
                            sql = "SELECT No FROM Port_Emp WHERE No IN "
                   + "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node) )"
                   + " AND  NO IN "
                   + "(SELECT  FK_Emp  FROM Port_EmpDept WHERE FK_Dept LIKE '" + empDept + "%')"
                   + " AND No!=" + dbStr + "FK_Emp";
                            sql += " ORDER BY No ";
                            break;
                    }


                    ps = new Paras();
                    ps.SQL = sql;
                    ps.Add("FK_Node", town.HisNode.NodeID);
                    ps.Add("FK_Emp", empNo);
                }
                else
                {
                    return dt;
                }
            }

            dt = DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
            {
                NodeStations nextStations = town.HisNode.NodeStations;
                if (nextStations.Count == 0)
                    throw new Exception(" Node has no job :" + town.HisNode.NodeID + "  " + town.HisNode.Name);
            }
            else
            {
                return dt;
            }

            /*  Under no inquiry into the circumstances ,  In accordance with the maximum number of matches   Raise a level of computing , Recursive algorithm unfinished .
             *  Because : More than has been done to determine positions , There is no need determination node other types of processing of .
             * */
            string nowDeptID = empDept.Clone() as string;
            while (true)
            {
                BP.Port.Dept myDept = new BP.Port.Dept(nowDeptID);
                nowDeptID = myDept.ParentNo;
                if (nowDeptID == "-1" || nowDeptID.ToString() == "0")
                {
                    break; /* Has found the most senior still not found , Come out to cycle down from the current operator to find people department .*/
                    throw new Exception("@ According to calculations do not find jobs (" + town.HisNode.Name + ") Recipient .");
                }

                // Check whether the specified department below the staff .
                DataTable mydtTemp = this.Func_GenerWorkerList_DiGui(nowDeptID, empNo);
                if (mydtTemp == null )
                {
                    /* If the father does not grade , To find the parent of the same level . */
                    BP.Port.Depts myDepts = new BP.Port.Depts();
                    myDepts.Retrieve(BP.Port.DeptAttr.ParentNo, myDept.ParentNo);
                    foreach (BP.Port.Dept item in myDepts)
                    {
                        if (item.No == nowDeptID)
                            continue;
                       mydtTemp = this.Func_GenerWorkerList_DiGui(item.No, empNo);
                       if (mydtTemp == null)
                           continue;
                       else
                           return mydtTemp;
                    }

                    continue; /* If the same level nor ,就continue.*/
                }
                else
                    return mydtTemp;
            }

            /* If someone does not find up , Consider looking down from this level department . */
            nowDeptID = empDept.Clone() as string;
            BP.Port.Depts subDepts = new BP.Port.Depts(nowDeptID);

            // Recursive out there under sub-sector of the staff positions 
            DataTable mydt = Func_GenerWorkerList_DiGui_ByDepts(subDepts, empNo);
            if (mydt == null)
                throw new Exception("@ According to calculations do not find jobs (" + town.HisNode.Name + ") Recipient .");
            return mydt;
            #endregion   In accordance with the job to execute .
        }
        /// <summary>
        ///  Recursive out there under sub-sector of the staff positions 
        /// </summary>
        /// <param name="subDepts"></param>
        /// <param name="empNo"></param>
        /// <returns></returns>
        public DataTable Func_GenerWorkerList_DiGui_ByDepts(BP.Port.Depts subDepts, string empNo)
        {
            foreach (BP.Port.Dept item in subDepts)
            {
                DataTable dt = Func_GenerWorkerList_DiGui(item.No, empNo);
                if (dt != null)
                    return dt;

                dt = Func_GenerWorkerList_DiGui_ByDepts(item.HisSubDepts, empNo);
                if (dt != null)
                    return dt;
            }
            return null;
        }
        /// <summary>
        ///  According to department get the next operator 
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="emp1"></param>
        /// <returns></returns>
        public DataTable Func_GenerWorkerList_DiGui(string deptNo, string empNo)
        {

            string sql = "SELECT a.FK_Emp FROM "
              + " Port_EmpStation a, WF_NodeStation b , Port_EmpDept c "
              + " WHERE a.FK_Station=b.FK_Station"
              + " AND a.FK_Emp=c.FK_Emp "
              + " AND b.FK_Node=" + dbStr + "FK_Node"
              + " AND c.FK_Dept=" + dbStr + "FK_Dept"
              + " AND a.FK_Emp !=" + dbStr + "FK_Emp ";

            ps = new Paras();
            ps.SQL = sql;
            ps.Add("FK_Node", town.HisNode.NodeID);
            ps.Add("FK_Dept", deptNo);
            ps.Add("FK_Emp", empNo);

            DataTable dt = DBAccess.RunSQLReturnTable(ps);
            if (dt.Rows.Count == 0)
            {
                NodeStations nextStations = town.HisNode.NodeStations;
                if (nextStations.Count == 0)
                    throw new Exception("@ Node has no job :" + town.HisNode.NodeID + "  " + town.HisNode.Name);

                sql = "SELECT No FROM Port_Emp WHERE No IN ";
                sql += "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )";
                sql += " AND No IN ";

                if (deptNo == "1")
                {
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Emp!=" + dbStr + "FK_Emp ) ";
                }
                else
                {
                    BP.Port.Dept deptP = new BP.Port.Dept(deptNo);
                    sql += "(SELECT FK_Emp FROM Port_EmpDept WHERE FK_Emp!=" + dbStr + "FK_Emp AND FK_Dept = '" + deptP.ParentNo + "')";
                }

                ps = new Paras();
                ps.SQL = sql;
                ps.Add("FK_Node", town.HisNode.NodeID);
                ps.Add("FK_Emp", empNo);
                dt = DBAccess.RunSQLReturnTable(ps);
                //if (dt.Rows.Count == 0)
                //{
                //    sql = "SELECT No FROM Port_Emp WHERE No!=" + dbStr + "FK_Emp AND No IN ";
                //    sql += "(SELECT  FK_Emp  FROM Port_EmpStation WHERE FK_Station IN (SELECT FK_Station FROM WF_NodeStation WHERE FK_Node=" + dbStr + "FK_Node ) )";
                //    ps = new Paras();
                //    ps.SQL = sql;
                //    ps.Add("FK_Emp", empNo);
                //    ps.Add("FK_Node", town.HisNode.NodeID);
                //    dt = DBAccess.RunSQLReturnTable(ps);
                //    if (dt.Rows.Count == 0)
                //        throw new Exception("@ Post (" + town.HisNode.HisStationsStr + ") Without staff , The corresponding node :" + town.HisNode.Name);
                //}
                if (dt.Rows.Count == 0)
                    return null;
                return dt;
            }
            else
            {
                return dt;
            }
            return null;
        }
        /// <summary>
        ///  Execute someone 
        /// </summary>
        /// <returns></returns>
        public DataTable DoIt(Flow fl, WorkNode currWn, WorkNode toWn)
        {
            //  Assign values to variables .
            this.fl = fl;
            this.currWn = currWn;
            this.town = toWn;
            this.WorkID = currWn.WorkID;

            // If the arrival node is in accordance with workflow Mode .
            if (toWn.HisNode.HisDeliveryWay != DeliveryWay.ByCCFlowBPM)
                return this.FindByWorkFlowModel();

            //  Set of rules .
            FindWorkerRoles ens = new FindWorkerRoles(town.HisNode.NodeID);
            foreach (FindWorkerRole en in ens)
            {
                en.fl = this.fl;
                en.town = toWn;
                en.currWn = currWn;
                en.HisNode = currWn.HisNode;
                en.WorkID = this.WorkID;

                DataTable dt = en.GenerWorkerOfDataTable();
                if (dt==null || dt.Rows.Count == 0)
                    continue;
                return dt;
            }

            // Did not find the person situation , Return empty .
            return null;
        }
    }
}
