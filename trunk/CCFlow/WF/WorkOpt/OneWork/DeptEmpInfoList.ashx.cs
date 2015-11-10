using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCFlow.WF.WorkOpt.OneWork
{
    using System.Data;

    using BP.DA;

    /// <summary>
    /// DeptEmpInfoList 的摘要说明
    /// </summary>
    public class DeptEmpInfoList : IHttpHandler
    {

        public void depemps(HttpRequest req, HttpResponse res)
        {
            string depemps = req.Params["depemps"];
            string[] arrdepmps = depemps.Split('@');
            string empnos = string.Join("','", arrdepmps);
            string sql =
                "SELECT        PORT_EMP.NO as id, PORT_EMP.EMPNO, PORT_EMP.NAME, PORT_EMP.PASS, PORT_EMP.FK_DEPT, PORT_EMP.FK_DUTY, PORT_EMP.LEADER, PORT_EMP.SID, "
                + "  PORT_EMP.TEL, PORT_EMP.EMAIL, PORT_EMP.NUMOFDEPT, PORT_EMP.IDX, PORT_DEPT.NAME AS DEPT_NAME"
                + " FROM  PORT_EMP, PORT_DEPT  WHERE PORT_EMP.FK_DEPT = PORT_DEPT.NO and ('('||PORT_DEPT.NAME||','||PORT_EMP.NAME||')') in ( '" + empnos + "' ) "
                + " order by PORT_DEPT.NAME asc,PORT_EMP.NAME asc ";
            //First Order by Department name, second order by Name
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            //string rows = Json.ToJson(dt);
            //string grid = "{ \"total\":\"" + dt.Rows.Count + "\",\"rows\":" + rows + "}";
            //res.Write(grid);
            //res.Write("{\"total\":\"2\",\"rows\":[{\"id\":\"176595\",\"firstname\":\"kj \",\"lastname\":\"m 333\",\"phone\":\"\",\"email\":\"\"},{\"id\":\"176596\",\"firstname\":\"bambang\",\"lastname\":\"gentholet\",\"phone\":\"1123\",\"email\":\"gentho@let.com\"}]}");

            string table = "";
            foreach (DataRow row in dt.Rows)
            {
                table += "<tr><td>" + row["DEPT_NAME"] + "</td>"
                      + "<td>" + row["NAME"] + "</td>"
                      + "<td>" + row["EMAIL"] + "</td>"
                      + "<td>" + row["TEL"] + "</td></tr>"
                    ;
            }
            //table += "<tbody></table>";
            res.Write(table);
        }

        /// <summary>
        /// (admin,admin)(zhanghaicheng,张海成)(zhangyifan,张一帆)(zhoupeng,周朋)(zhoushengyu,周升雨)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="res"></param>
        public void emps(HttpRequest req, HttpResponse res)
        {
            string emps = req.Params["emps"];
            string[] arremps = emps.Split(')');

            string empnos = "''";
            foreach (string arremp in arremps)
            {
                var tmp = arremp.Trim('(').Split(',')[0];
                empnos += ",'" + tmp + "'";
            }

            string sql =
                "SELECT        PORT_EMP.NO as id, PORT_EMP.EMPNO, PORT_EMP.NAME,  PORT_EMP.FK_DEPT,  "
                + "  PORT_EMP.TEL, PORT_EMP.EMAIL, PORT_DEPT.NAME AS DEPT_NAME"
                + " FROM  PORT_EMP, PORT_DEPT  WHERE (PORT_EMP.NO) in ( " + empnos + ") and PORT_EMP.FK_DEPT = PORT_DEPT.NO  "
                + " order by PORT_DEPT.NAME asc,PORT_EMP.NAME asc ";
            //First Order by Department name, second order by Name
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            //string rows = Json.ToJson(dt);
            //string grid = "{ \"total\":\"" + dt.Rows.Count + "\",\"rows\":" + rows + "}";
            //res.Write(grid);
            //res.Write("{\"total\":\"2\",\"rows\":[{\"id\":\"176595\",\"firstname\":\"kj \",\"lastname\":\"m 333\",\"phone\":\"\",\"email\":\"\"},{\"id\":\"176596\",\"firstname\":\"bambang\",\"lastname\":\"gentholet\",\"phone\":\"1123\",\"email\":\"gentho@let.com\"}]}");

            string table = "";
            foreach (DataRow row in dt.Rows)
            {
                table += "<tr><td>" + row["DEPT_NAME"] + "</td>"
                      + "<td>" + row["NAME"] + "</td>"
                      + "<td>" + row["EMAIL"] + "</td>"
                      + "<td>" + row["TEL"] + "</td></tr>"
                    ;
            }
            //table += "<tbody></table>";
            res.Write(table);
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            res.ContentType = "text/html; charset=UTF-8";
            string pformat = req.Params["pformat"];
            switch (pformat)
            {
                case  "emps":  this.emps(req,res);
                    break;
                case "depemps": this.depemps(req,res);break;
                default:
                    break;
            }

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