using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;
using BP.Web;
using BP.WF.Template;
using BP.WF;

namespace CCFlow.WF.UC
{
    public partial class UCJumpWay : BP.Web.UC.UCBase3
    {
        #region  Property .
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
        public int GoNode
        {
            get
            {
                return int.Parse(this.Request.QueryString["GoNode"]);
            }
        }
        #endregion  Property .
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.DoType != null)
            {
                Node ndJump = new Node(this.GoNode);
                BP.WF.WorkNode wn = new BP.WF.WorkNode(this.WorkID, this.FK_Node);
                string msg = wn.NodeSend(ndJump, null).ToMsgOfHtml();
                this.AddFieldSet(" Send tips ");
                this.Add(msg.Replace("@", "<br>@"));
                this.AddFieldSetEnd();
                return;
            }

            Node nd = new Node(this.FK_Node);
            BP.WF.Template.BtnLab lab = new BtnLab(this.FK_Node);

            string sql = "SELECT NodeID,Name FROM WF_Node WHERE FK_Flow='" + this.FK_Flow + "'";
            switch (lab.JumpWayEnum)
            {
                case JumpWay.Previous:
                    sql = "SELECT NodeID,Name FROM WF_Node WHERE NodeID IN (SELECT FK_Node FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " )";
                    break;
                case JumpWay.Next:
                    sql = "SELECT NodeID,Name FROM WF_Node WHERE NodeID NOT IN (SELECT FK_Node FROM WF_GenerWorkerlist WHERE WorkID=" + this.WorkID + " ) AND FK_Flow='" + this.FK_Flow + "'";
                    break;
                case JumpWay.AnyNode:
                    sql = "SELECT NodeID,Name FROM WF_Node WHERE FK_Flow='" + this.FK_Flow + "' ORDER BY STEP";
                    break;
                case JumpWay.JumpSpecifiedNodes:
                    sql = nd.JumpToNodes;
                    sql = sql.Replace("@WebUser.No", WebUser.No);
                    sql = sql.Replace("@WebUser.Name", WebUser.Name);
                    sql = sql.Replace("@WebUser.FK_Dept", WebUser.FK_Dept);
                    if (sql.Contains("@"))
                    {
                        Work wk = nd.HisWork;
                        wk.OID = this.WorkID;
                        wk.RetrieveFromDBSources();
                        foreach (Attr attr in wk.EnMap.Attrs)
                        {
                            if (sql.Contains("@") == false)
                                break;
                            sql = sql.Replace("@" + attr.Key, wk.GetValStrByKey(attr.Key));
                        }
                    }
                    break;
                case JumpWay.CanNotJump:
                    this.WinCloseWithMsg(" This node is not allowed to jump .");
                    return;
                default:
                    throw new Exception(" No judgment .");
            }

            string small = this.PageID;
            small = small.Replace("JumpWay", "");
            sql = sql.Replace("~", "'");

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            this.Add("<div align='center' ><div style='width:100%;text-align:left'>");

            this.AddFieldSet(" Please select a node to jump ");
            this.AddUL();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[0].ToString() == this.FK_Node.ToString())
                    continue;
                this.AddLi("&nbsp;&nbsp;&nbsp;<img src='/WF/Img/MyWork.gif' border=0 /><a href='JumpWay" + small + ".aspx?DoType=Go&GoNode=" + dr[0].ToString() + "&WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "' >" + dr[0].ToString() + " - " + dr[1].ToString() + "</a><br>");
            }
            this.AddULEnd();

            this.AddHR();
            this.AddBR("&nbsp;&nbsp;&nbsp;<a href='MyFlow" + small + ".aspx?WorkID=" + this.WorkID + "&FK_Node=" + this.FK_Node + "&FK_Flow=" + this.FK_Flow + "&FID=" + this.FID + "' ><img src='/WF/Img/Btn/Back.gif' border=0 /> Return </a>");
            this.AddFieldSetEnd();

            this.Add("</div>");
            this.Add("</div>");
        }
    }
}