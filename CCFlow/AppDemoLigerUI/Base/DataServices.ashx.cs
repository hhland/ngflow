using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;

namespace CCFlow.AppDemoLigerUI.Base
{
    /// <summary>
    /// DataServices  The summary 
    /// </summary>
    public class DataServices : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string type = context.Request["action"];
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("getQiYeList"))// Get business listings 
                {
                    GetQiYeList(context);
                }
                else if (type.Equals("getEmp"))// Get Users 
                {
                    GetEmps(context);
                }
                else if (type.Equals("getCityAndRoad"))// Get business cities and streets 
                {
                    GetCityAndRoad(context);
                }
                else if (type.Equals("GetState"))// Get state enterprises 
                {
                    GetState(context);
                }
                else if (type.Equals("removeParts"))// Are there pieces of the process to obtain the withdrawal 
                {
                    GetRemoveParts(context);
                }
                else if (type.Equals("cleare"))// Get the existence of the clearance process 
                {
                    GetCleare(context);
                }
                else if (type.Equals("returnParts"))// Gets whether the process already exists, please return 
                {
                    GetReturn(context);
                }
                else if (type.Equals("unFix"))// Get the existence of thawing process 
                {
                    GetUnFix(context);
                }
                else if (type.Equals("GetJieDao"))// Obtain the appropriate street 
                {
                    GetJieDao(context);
                }
                else if (type.Equals("GetFlow"))// Gets the type of all processes 
                {
                    GetFlow(context);
                }
                else if (type.Equals("GetSurveyStatus"))// Get table status field content 
                {
                    GetSurveyStatus(context);
                }
                else if (type.Equals("GetRemarkStatus"))// Get the contents of a memo field in the table 
                {
                    GetRemarkStatus(context);
                }
                else if (type.Equals("GetSchedule"))// Get progress status 
                {
                    GetSchedule(context);
                }
            }
        }

        /// <summary>
        ///  Gets the type of all processes 
        /// </summary>
        /// <param name="context"></param>
        private void GetFlow(HttpContext context)
        {
            string sql = "select No,Name from Wf_flow";

            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

            DataRow newRow = dt.NewRow();
            newRow["No"] = "";
            newRow["Name"] = " Whole ";
            dt.Rows.Add(newRow);

            DataView view = dt.AsDataView();
            view.Sort = "No Asc";
            dt = view.ToTable();


            System.Collections.ArrayList dic = new System.Collections.ArrayList();

            foreach (DataRow row in dt.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    drow.Add(dc.ColumnName, row[dc.ColumnName]);
                }
                dic.Add(drow);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            string result = jss.Serialize(dic);

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }
        /// <summary>
        ///   Obtain the appropriate street 
        /// </summary>
        /// <param name="context"></param>
        private void GetJieDao(HttpContext context)
        {
            string key = context.Request["key"];
            string result = null;
            DataTable dt = new DataTable();
            string sql = "select R_Id,R_Name from V_Gl2_Area  where r_pid= '" + key + "'";
            dt = BP.DA.DBAccess.RunSQLReturnTable(sql);


            DataRow row = dt.NewRow();
            row[0] = "-1";
            row[1] = " Please select ";
            dt.Rows.Add(row);


            DataView view = dt.DefaultView;
            view.Sort = "R_Id asc";
            dt = view.ToTable();

            System.Collections.ArrayList dic = new System.Collections.ArrayList();

            foreach (DataRow dr in dt.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                dic.Add(drow);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            result = jss.Serialize(dic);

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();

        }
        /// <summary>
        ///  Get the existence of thawing process 
        /// </summary>
        /// <param name="context"></param>
        private void GetUnFix(HttpContext context)
        {
            string key = context.Request["key"];
            string result = null;
            if (!string.IsNullOrEmpty(key))// Judgment passed workid  Is empty 
            {
                /* Query whether this WorkID Process */


                string sql = "select COUNT(*) from WF_GenerWorkFlow where (WFState!=7 or WFState!=3) and  FK_Flow='005'  and PWorkID='" + key + "'";

                result = BP.DA.DBAccess.RunSQLReturnValInt(sql.ToString()).ToString();
            }
            else
            {
                result = "error";
            }

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();
        }
        /// <summary>
        ///  If there is, please get back flow 
        /// </summary>
        /// <param name="context"></param>
        private void GetReturn(HttpContext context)
        {
            string key = context.Request["key"];
            string result = null;
            if (!string.IsNullOrEmpty(key))// Judgment passed workid  Is empty 
            {
                /* Query whether this WorkID Process */


                string sql = "select COUNT(*) from WF_GenerWorkFlow where (WFState!=7 or WFState!=3) and  FK_Flow='008' and PWorkID='" + key + "'";

                result = BP.DA.DBAccess.RunSQLReturnValInt(sql.ToString()).ToString();
            }
            else
            {
                result = "error";
            }

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();

        }
        /// <summary>
        ///  Get the existence of the clearance process 
        /// </summary>
        /// <param name="context"></param>
        private void GetCleare(HttpContext context)
        {
            string key = context.Request["key"];
            string result = null;
            if (!string.IsNullOrEmpty(key))// Judgment passed workid  Is empty 
            {
                /* Query whether this WorkID Process */

                StringBuilder sql = new StringBuilder();

                sql.Append("select count(*) from( ");
                sql.Append(" select * from WF_GenerWorkFlow where PWorkID in (select WorkID from WF_GenerWorkFlow where PWorkID='" + key + "' and FK_Flow='004') ");
                sql.Append(" union ");
                sql.Append(" select * from WF_GenerWorkFlow where PWorkID='" + key + "' and FK_Flow='006' )as WF_GenerWorkFlow where WFState!=7 and WFState!=3");

                result = BP.DA.DBAccess.RunSQLReturnValInt(sql.ToString()).ToString();
            }
            else
            {
                result = "error";
            }

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();

        }

        /// <summary>
        ///  Gets whether they contain the running revocation process 
        /// </summary>
        /// <param name="context"></param>
        private void GetRemoveParts(HttpContext context)
        {
            string key = context.Request["key"];
            string result = null;
            if (!string.IsNullOrEmpty(key))// Judgment passed workid  Is empty 
            {
                /* Query whether this WorkID Process */
                string sql =
                    "select count(*) from (select * from WF_GenerWorkFlow where WFState!=7 or WFState!=3 )as WF_GenerWorkFlow where  PWorkID= '" +
                    key + "'";

                result = BP.DA.DBAccess.RunSQLReturnValInt(sql).ToString();
            }
            else
            {
                result = "error";
            }

            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();


        }

        /// <summary>
        ///  Get business city numbers 
        /// </summary>
        /// <param name="context"></param>
        private void GetCityAndRoad(HttpContext context)
        {
            try
            {
                /*Type=0 Representation is to obtain City Serial number  Type=1 Representation is to obtain JieDao Serial number */
                string key = context.Request["key"];// Keyword 
                string type = context.Request["type"];// Query Type 
                string sql = null;//sql  Statement 
                string result = null;// Back to Results 

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(type))
                {
                    if (type.Equals("0"))
                    {
                        sql = "select R_PId as result from V_Gl2_Area where R_Id='" + key + "'";
                    }
                    else if (type.Equals("1"))
                    {
                        sql = "select R_Id as result from V_Gl2_Area where R_PId='" + key + "'";
                    }

                    DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

                    if (dt.Rows.Count > 0)
                    {
                        result = dt.Rows[0]["result"].ToString();
                    }
                    else
                    {
                        result = "-1";
                    }
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();

                result = jss.Serialize(int.Parse(result));

                context.Response.Clear();
                context.Response.Write(result);
                context.Response.End();
            }
            catch
            {

            }
        }
        /// <summary>
        ///  Get personnel information 
        /// </summary>
        private void GetEmps(HttpContext context)
        {
            try
            {
                //string result = "";
                //string[] str = context.Request.RawUrl.Split('&');
                //string key3 = str[str.Length - 1].Split('=')[1];
                //string realKey = HttpUtility.UrlDecode(key3, System.Text.Encoding.UTF8);

                int count = 5;
                string result = "";
                string[] str = context.Request.RawUrl.Split('&');
                string key3 = null;
                string top = null;
                foreach (string single in str)
                {
                    if (single.StartsWith("q="))
                    {
                        key3 = single.Split('=')[1];
                    }
                    else if (single.StartsWith("limit="))
                    {
                        top = single.Split('=')[1];
                    }
                }
                if (string.IsNullOrEmpty(top))
                {
                    top = "10";
                }

                string realKey = HttpUtility.UrlDecode(key3, System.Text.Encoding.UTF8);

                string sql = "select top " + top + " * from port_emp where Name like '%" + realKey + "%' or No like'%" + realKey + "%'";

                System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                System.Collections.ArrayList dic = new System.Collections.ArrayList();

                foreach (DataRow dr in dt.Rows)
                {
                    System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    dic.Add(drow);
                }

                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = jss.Serialize(dic);

                context.Response.Clear();
                context.Response.Write(result);
                context.Response.End();

            }
            catch
            {

            }
        }
        /// <summary>
        ///  Access to enterprise information  
        /// </summary>
        /// <param name="context"></param>
        private void GetQiYeList(HttpContext context)
        {
            try
            {
                int count = 5;
                string result = "";
                string[] str = context.Request.RawUrl.Split('&');
                string key3 = null;
                string top = null;
                foreach (string single in str)
                {
                    if (single.StartsWith("q="))
                    {
                        key3 = single.Split('=')[1];
                    }
                    else if (single.StartsWith("limit="))
                    {
                        top = single.Split('=')[1];
                    }
                }
                if (string.IsNullOrEmpty(top))
                {
                    top = "10";
                }

                string realKey = HttpUtility.UrlDecode(key3, System.Text.Encoding.UTF8);

                string sql = "SELECT  top " + top + " * FROM V_Inc where Name like '%" + realKey + "%'";
                System.Data.DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                System.Collections.ArrayList dic = new System.Collections.ArrayList();
                foreach (DataRow dr in dt.Rows)
                {
                    System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        drow.Add(dc.ColumnName, dr[dc.ColumnName]);
                    }
                    dic.Add(drow);
                }
                JavaScriptSerializer jss = new JavaScriptSerializer();
                result = jss.Serialize(dic);
                // result = CommonDbOperator.GetJsonFromTableEasy(dt,count);

                context.Response.Clear();
                context.Response.Write(result);
                context.Response.End();
            }
            catch
            {
            }
        }
        /// <summary>
        ///  Gets the current state of the enterprise 
        /// </summary>
        private void GetState(HttpContext context)
        {
            string key = context.Request["key"];
            string result = "true";
            if (!string.IsNullOrEmpty(key))
            {
                string sql = "select * from JXW_Inc where no = '" + key + "' and IncSta != 1";

                int count = BP.DA.DBAccess.RunSQLReturnCOUNT(sql);

                if (count > 0)
                {
                    result = "true";
                }
                else
                {
                    result = "false";
                }

            }
            else
            {
                result = "error";
            }
            context.Response.Clear();
            context.Response.Write(result);
            context.Response.End();

        }

        /// <summary>
        ///  Get status information 
        /// </summary>
        /// <returns></returns>
        private void GetSurveyStatus(HttpContext context)
        {
            string TableName = "";
            string Status = "";
            try
            {
                string FK_Flow = context.Request["FK_Flow"];
                string WorkID = context.Request["WorkID"];
                int _FK_Flow = int.Parse(FK_Flow);
                TableName = "ND" + _FK_Flow + "Rpt";
                string sqlSort = "SELECT Status FROM " + TableName + " WHERE OID=" + WorkID;
                Status = BP.DA.DBAccess.RunSQLReturnString(sqlSort);
            }
            catch { }
            context.Response.Clear();
            context.Response.Write(Status);
            context.Response.End();

        }

        /// <summary>
        ///  Get information Remarks 
        /// </summary>
        /// <returns></returns>
        private void GetRemarkStatus(HttpContext context)
        {
            string FK_Flow = context.Request["FK_Flow"];
            string WorkID = context.Request["WorkID"];
            if (FK_Flow == "001")
            {
                GetSchedule(context);
            }
            else
            {
                string TableName = "";
                string Status = "";
                try
                {
                    int _FK_Flow = int.Parse(FK_Flow);
                    TableName = "ND" + _FK_Flow + "Rpt";
                    string sqlSort = "SELECT Remark FROM " + TableName + " WHERE OID=" + WorkID;
                    Status = BP.DA.DBAccess.RunSQLReturnString(sqlSort);
                }
                catch { }
                context.Response.Clear();
                context.Response.Write(Status);
                context.Response.End();
            }
        }

        // Get Progress 
        private void GetSchedule(HttpContext context)
        {
            string Remark = "error";
            string WorkID = context.Request["WorkID"];
            try
            {
                int wfstate = 0;
                StringBuilder sql = new StringBuilder();

                sql.Append("select '002' as FlowNo,' Governments are justified ' as FlowName,OID as WorkID,Title,WFState,BillNo,FLowStartRDT,FlowEndNode,FlowEnder,GovUserID as GuestNo, GovUserName as GuestName,GovNo as GuestDepNo,GovName as GuestDepName from ND2Rpt ");
                sql.Append(" where PFlowNo='001' and PWorkId='" + WorkID + "' and wfstate!=7 ");
                sql.Append(" UNION ");
                sql.Append(" select '012' as FlowNo,' Child care platform ' as FlowName,OID as WorkID,Title,WFState,BillNo,FlowStartRDT,FlowEndNode,FlowENder,'' as GuestNo,''  as GuestName,FlowStarter as GuestDepNo, '' as GuestDepName from ND12Rpt ");
                sql.Append(" where PFlowNo='001' and PWorkId='" + WorkID + "' and wfstate!=7 ");

                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["wfstate"].ToString() == "3")
                        wfstate++;
                }
                Remark = " Progress :" + wfstate + "/" + dt.Rows.Count;
                string str = "update WF_GenerWorkFlow set Remark='" + Remark + "' where WorkID=" + WorkID;
                BP.DA.DBAccess.RunSQL(str);
            }
            catch { }
            context.Response.Clear();
            context.Response.Write(Remark);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}