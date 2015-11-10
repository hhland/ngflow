using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  Audit node 
    /// </summary>
    public class WorkCheck
    {
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID=0;
        public Int64 FID = 0;
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID = 0;
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FlowNo = null;
        public WorkCheck(string flowNo, int nodeID, Int64 workid,Int64 fid)
        {
            this.FlowNo = flowNo;
            this.NodeID = nodeID;
            this.WorkID = workid;
            this.FID = fid;
        }
        /// <summary>
        ///  Get primary key 32位
        /// </summary>
        /// <returns></returns>
        public int GetMyPK32()
        {
            try
            {
                int newPK = int.Parse(this.WorkID.ToString()) + this.NodeID + int.Parse(this.FlowNo);
                string myPk = "";
                string sql = "SELECT TOP 1 RDT FROM WF_GenerWorkerlist WHERE WorkID={0} AND FK_Node={1} AND FK_Flow='{2}' ORDER BY RDT DESC";
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(string.Format(sql, this.WorkID, this.NodeID, this.FlowNo));
                if (dt != null && dt.Rows.Count > 0)
                {
                    myPk = dt.Rows[0]["RDT"].ToString();
                    myPk = myPk.Replace("-", "").Replace(":", "").Replace(" ", "");
                    myPk = myPk.Substring(4);
                    newPK = int.Parse(this.WorkID.ToString()) + this.NodeID + int.Parse(this.FlowNo) + int.Parse(myPk);
                }
                return newPK;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        ///  Get primary key 
        /// </summary>
        /// <returns></returns>
        public Int64 GetMyPK()
        {
            try
            {
                Int64 newPK = Int64.Parse(this.WorkID.ToString()) + this.NodeID + Int64.Parse(this.FlowNo);
                string myPk = "";
                string sql = "SELECT TOP 1 RDT FROM WF_GenerWorkerlist WHERE WorkID={0} AND FK_Node={1} AND FK_Flow='{2}' ORDER BY RDT DESC";
                DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(string.Format(sql, this.WorkID, this.NodeID, this.FlowNo));
                if (dt != null && dt.Rows.Count > 0)
                {
                    myPk = dt.Rows[0]["RDT"].ToString();
                    myPk = myPk.Replace("-", "").Replace(":", "").Replace(" ", "");
                    myPk = myPk.Substring(2);
                    newPK = Int64.Parse(this.WorkID.ToString()) + this.NodeID + Int64.Parse(this.FlowNo) + Int64.Parse(myPk);
                }
                return newPK;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private Tracks _HisWorkChecks = null;
        public Tracks HisWorkChecks
        {
            get
            {
                if (_HisWorkChecks == null)
                {
                    _HisWorkChecks = new Tracks();
                    BP.En.QueryObject qo = new En.QueryObject(_HisWorkChecks);

                    if (this.FID != 0)
                    {
                        qo.AddWhereIn(TrackAttr.WorkID, "(" + this.WorkID + "," + this.FID + ")");
                    }
                    else
                    {
                        qo.AddWhere(TrackAttr.WorkID, this.WorkID);
                        qo.addOr();
                        qo.AddWhere(TrackAttr.FID, this.WorkID);
                    }

                    qo.addOrderByDesc(TrackAttr.RDT);
                    
                    string sql=qo.SQL;
                    sql = sql.Replace("WF_Track", "ND"+int.Parse(this.FlowNo)+"Track");
                    DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql,qo.MyParas);

                    dt.DefaultView.Sort = "RDT desc";
                   
                    BP.En.Attrs attrs=_HisWorkChecks.GetNewEntity.EnMap.Attrs;
                     foreach (DataRow dr in dt.Rows)
                    {
                        Track en = new Track();  
                        foreach (BP.En.Attr attr in attrs)
                            en.Row.SetValByKey(attr.Key, dr[attr.Key]);

                        _HisWorkChecks.AddEntity(en);
                    }
                }
                return _HisWorkChecks;
            }
        }
    }
}
