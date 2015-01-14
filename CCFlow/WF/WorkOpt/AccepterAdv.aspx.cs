using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BP.DA;
using BP.WF.Template;
using BP.WF;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.WorkOpt
{
    public partial class AccepterAdv : System.Web.UI.Page
    {
        #region  Property 
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        private string FK_Flow
        {
            get
            {
                return this.Request["FK_Flow"];
            }
        }
        /// <summary>
        ///  Process node number 
        /// </summary>
        private string FK_Node
        {
            get
            {
                return this.Request["FK_Node"];
            }
        }
        /// <summary>
        ///  Job No. 
        /// </summary>
        private long WorkID
        {
            get
            {
                string workId = this.Request["WorkID"];
                if (!string.IsNullOrEmpty(workId))
                    return long.Parse(workId);
                return 0;
            }
        }
        /// <summary>
        ///  Working Parent ID number 
        /// </summary>
        private string FID
        {
            get
            {
                return this.Request["FID"];
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (string.IsNullOrEmpty(Request["method"]))
                return;
            method = Request["method"].ToString();

            switch (method)
            {
                case "getreservewords":// Get Common Vocabulary 
                    s_responsetext = GetReserveWords();
                    break;
                case "getdeliverynode":// Get node 
                    s_responsetext = GetDeliveryNodes();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }

        /// <summary>
        ///  Get spare vocabulary 
        /// </summary>
        /// <returns></returns>
        private string GetReserveWords()
        {
            StringBuilder append = new StringBuilder();
            Node curNode = new Node();
            bool isHave = curNode.RetrieveByAttr(NodeAttr.NodeID, FK_Node.Replace("ND", "").Replace("_CC", ""));
            StringBuilder leftWords = new StringBuilder();
            StringBuilder rightWords = new StringBuilder();

            // Node exists 
            if (isHave)
            {
                // Official word on the left 
                if (!string.IsNullOrEmpty(curNode.DocLeftWord))
                {
                    string[] arraryWord = curNode.DocLeftWord.Split('@');
                    foreach (string str in arraryWord)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        if (leftWords.Length > 0) leftWords.Append(",");

                        leftWords.Append("{word:'" + str + "'}");
                    }
                }
                // The right of the word processing document 
                if (!string.IsNullOrEmpty(curNode.DocRightWord))
                {
                    string[] arraryWord = curNode.DocRightWord.Split('@');
                    foreach (string str in arraryWord)
                    {
                        if (string.IsNullOrEmpty(str))
                            continue;

                        if (rightWords.Length > 0) rightWords.Append(",");

                        rightWords.Append("{word:'" + str + "'}");
                    }
                }
            }
            append.Append("{");
            append.Append("LeftWords:[");
            append.Append(leftWords);
            append.Append("],");
            append.Append("RightWords:[");
            append.Append(rightWords);
            append.Append("]");
            append.Append("}");
            // The return value 
            return BP.Tools.Entitis2Json.Instance.ReplaceIllgalChart(append.ToString());
        }

        /// <summary>
        ///  Get node 
        /// </summary>
        /// <returns></returns>
        private string GetDeliveryNodes()
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            string sql = "SELECT NodeID,Name FROM WF_Node WHERE FK_Flow='" + nd.FK_Flow + "' AND DeliveryWay=" + (int)DeliveryWay.BySelected + " AND DeliveryParas LIKE '%" + nd.NodeID + "%' order by Step";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            GenerWorkerLists gwls = new GenerWorkerLists(this.WorkID);

            //Flow fl = new Flow(nd.FK_Flow);
            //GERpt rpt = fl.HisGERpt;
            //rpt.OID = this.WorkID;
            //rpt.RetrieveFromDBSources();
            StringBuilder append = new StringBuilder();
            append.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                BP.WF.Node mynd = new BP.WF.Node(int.Parse(dr["NodeID"].ToString()));
                if (mynd.IsStartNode)
                    continue;

                if (gwls.IsExits(GenerWorkerListAttr.FK_Node, mynd.NodeID) == true)
                    continue;

                if (append.Length > 1) append.Append(",");

                append.Append("{NodeID:'" + dr["NodeID"] + "',Name:'" + dr["Name"] + "'}");

                if (mynd.HisCCRole != CCRole.UnCC)
                {
                    /* Can cc */
                    append.Append(",{NodeID:'" + dr["NodeID"] + "_CC',Name:' Read Office '}");    
                }
            }
            append.Append("]");
            return append.ToString();
        }
    }
}