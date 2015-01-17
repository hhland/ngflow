using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.WF.Data;
using BP.En;
using BP.DA;

namespace CCFlow.WF.OneWork
{
    public partial class WF_WorkOpt_OneWork_Ath : BP.Web.WebPage
    {
        #region attr
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int OID
        {
            get
            {
                return int.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        public int FID
        {
            get
            {
                return int.Parse(this.Request.QueryString["FID"]);
            }
        }
        public int FK_Node
        {
            get { return int.Parse(this.Request.QueryString["FK_Node"]); }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Sys_FrmAttachmentDB WHERE FK_FrmAttachment IN (SELECT MyPK FROM Sys_FrmAttachment WHERE  " + BP.WF.Glo.MapDataLikeKey(this.FK_Flow, "FK_MapData") + "  AND IsUpload=1) AND RefPKVal='" + this.OID + "' ORDER BY RDT";

            //string sql = "SELECT * FROM Sys_FrmAttachmentDB WHERE FK_FrmAttachment IN (SELECT MyPK FROM Sys_FrmAttachment WHERE  FK_MapData ='ND"+FK_Node+"'  AND IsUpload=1) AND RefPKVal='" + this.OID + "' ORDER BY RDT";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);

            if (dt.Rows.Count > 0)
            {
                this.Pub1.AddTable("class='Table' cellpadding='0' cellspacing='0' border='0' style='width: 100%'");
                this.Pub1.AddTR();
                this.Pub1.AddTDGroupTitle("style='text-align:center'", "No.");
                this.Pub1.AddTDGroupTitle(" Annex No. ");
                this.Pub1.AddTDGroupTitle(" Node ");
                this.Pub1.AddTDGroupTitle(" Name ");
                this.Pub1.AddTDGroupTitle(" Size (kb)");
                this.Pub1.AddTDGroupTitle(" Upload people ");
                this.Pub1.AddTDGroupTitle(" Uploaded ");
                this.Pub1.AddTREnd();
                int i = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(i);
                    this.Pub1.AddTD(dr["FK_FrmAttachment"].ToString());
                    string nodeName = "";
                    try
                    {
                        int nodeID = int.Parse(dr["NodeID"].ToString());
                        Node node = new Node(nodeID);
                        nodeName = node.Name;

                    }
                    catch (Exception ex)
                    { 
                    
                    }

                    this.Pub1.AddTD(nodeName);
                    this.Pub1.AddTD("<a href='/WF/CCForm/AttachmentUpload.aspx?DoType=Down&MyPK=" + dr["MyPK"] + "' target=_sd ><img src='/WF/Img/FileType/" + dr["FileExts"] + ".gif' onerror=\"this.src='/WF/Img/FileType/Undefined.gif'\" border=0/>" + dr["FileName"].ToString() + "</a>");
                    this.Pub1.AddTD(dr["FileSize"].ToString());
                    this.Pub1.AddTD(dr["RecName"].ToString());
                    this.Pub1.AddTD(dr["RDT"].ToString());
                    this.Pub1.AddTREnd();
                }
                this.Pub1.AddTableEnd();
            }

            Bills bills = new Bills();
            bills.Retrieve(BillAttr.WorkID, this.OID);
            if (bills.Count > 0)
            {
                this.Pub1.AddTable("class='Table' cellpadding='0' cellspacing='0' border='0' style='width: 100%'");
                this.Pub1.AddTR();
                this.Pub1.AddTDGroupTitle("style='text-align:center'", "No.");
                this.Pub1.AddTDGroupTitle(" Name ");
                this.Pub1.AddTDGroupTitle(" Node ");
                this.Pub1.AddTDGroupTitle(" Print People ");
                this.Pub1.AddTDGroupTitle(" Date ");
                this.Pub1.AddTDGroupTitle(" Function ");
                this.Pub1.AddTREnd();
                int idx = 0;
                foreach (Bill bill in bills)
                {
                    idx++;
                    this.Pub1.AddTR();
                    this.Pub1.AddTDIdx(idx);

                    this.Pub1.AddTD(bill.FK_BillTypeT);

                    this.Pub1.AddTD(bill.FK_NodeT);
                    this.Pub1.AddTD(bill.FK_EmpT);
                    this.Pub1.AddTD(bill.RDT);
                    this.Pub1.AddTD("<a class='easyui-linkbutton' data-options=\"iconCls:'icon-print'\" href='" + this.Request.ApplicationPath + "WF/Rpt/Bill.aspx?MyPK=" + bill.MyPK + "&DoType=Print' > Print </a>");
                    this.Pub1.AddTREnd();
                }
                this.Pub1.AddTableEnd();
            }

            int num = bills.Count + dt.Rows.Count;
            if (num == 0)
            {
                Pub1.AddEasyUiPanelInfo(" Prompt ", "<h3> The current process is no data , Or that the process has no attachment or document .</h3>");
            }
        }
    }
}