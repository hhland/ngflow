using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_CondPRI : BP.Web.WebPage
    {
        #region  Property 
        /// <summary>
        ///  Primary key 
        /// </summary>
        public new string MyPK
        {
            get
            {
                return this.Request.QueryString["MyPK"];
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int FK_MainNode
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_MainNode"]);
            }
        }
        public int ToNodeID
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["ToNodeID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion  Property 

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (this.DoType)
            {
                case "Up":
                    Cond up = new Cond(this.MyPK);
                    up.DoUp(this.FK_MainNode);
                    up.RetrieveFromDBSources();
                    DBAccess.RunSQL("UPDATE WF_Cond SET PRI=" + up.PRI + " WHERE ToNodeID=" + up.ToNodeID);
                    break;
                case "Down":
                    Cond down = new Cond(this.MyPK);
                    down.DoDown(this.FK_MainNode);
                    down.RetrieveFromDBSources();
                    DBAccess.RunSQL("UPDATE WF_Cond SET PRI=" + down.PRI + " WHERE ToNodeID=" + down.ToNodeID);
                    break;
                default:
                    break;
            }

            BP.WF.Node nd = new BP.WF.Node(this.FK_MainNode);

            this.Pub1.AddTable("class='Table' cellSpacing='1' cellPadding='1'  border='1' style='width:100%'");
            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle' colspan='7'", nd.Name);
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTD("class='GroupTitle'", " From node ID");
            this.Pub1.AddTD("class='GroupTitle'", " From the node name ");
            this.Pub1.AddTD("class='GroupTitle'", " To node ID");
            this.Pub1.AddTD("class='GroupTitle'", " To the node name ");
            this.Pub1.AddTD("class='GroupTitle'", " Priority ");
            this.Pub1.AddTD("class='GroupTitle' colspan=2", " Operating ");
            this.Pub1.AddTREnd();

            Conds cds = new Conds();
            //BP.En.QueryObject qo = new QueryObject(cds);
            //qo.AddWhere(CondAttr.FK_Node, this.FK_MainNode);
            //qo.addAnd();
            //qo.AddWhere(CondAttr.FK_Node, this.FK_MainNode);

            cds.Retrieve(CondAttr.FK_Node, this.FK_MainNode, CondAttr.CondType, 2, CondAttr.PRI);
            string strs = "";

            foreach (Cond cd in cds)
            {
                if (strs.Contains("," + cd.ToNodeID.ToString()))
                    continue;

                strs += "," + cd.ToNodeID.ToString();

                BP.WF.Node mynd = new BP.WF.Node(cd.ToNodeID);

                this.Pub1.AddTR();
                this.Pub1.AddTD(nd.NodeID);
                this.Pub1.AddTD(nd.Name);
                this.Pub1.AddTD(mynd.NodeID);
                this.Pub1.AddTD(mynd.Name);
                this.Pub1.AddTD(cd.PRI);
                this.Pub1.AddTD("<a href='CondPRI.aspx?CondType=2&DoType=Up&FK_Flow=" + this.FK_Flow + "&FK_MainNode=" + this.FK_MainNode + "&ToNodeID=" + this.ToNodeID + "&MyPK=" + cd.MyPK + "' class='easyui-linkbutton' data-options=\"iconCls:'icon-up'\"> Move </a>");
                this.Pub1.AddTD("<a href='CondPRI.aspx?CondType=2&DoType=Down&FK_Flow=" + this.FK_Flow + "&FK_MainNode=" + this.FK_MainNode + "&ToNodeID=" + this.ToNodeID + "&MyPK=" + cd.MyPK + "' class='easyui-linkbutton' data-options=\"iconCls:'icon-down'\"> Down </a>");
                this.Pub1.AddTREnd();
            }

            this.Pub1.AddTableEnd();
            this.Pub1.AddBR();

            string help = "";
            help += "<ul>";
            help += "<li> In turn the , If more than one line appears all set up from time to time , The system will be calculated in accordance with the first route , That an arrangement on the top of a calculation in accordance with that .</li>";
            help += "<li> Such as :在demo中002. Leave Process , If a person has both grassroots Gang , Then there are the middle reaches the grassroots and middle Kong route will be set up , If the direction of the condition of the priority , The system will follow the route priority to meet the conditions of the calculation .</li>";
            help += "</ul>";

            this.Pub1.AddEasyUiPanelInfo(" Help ", "<span style='font-weight:bold'> What is the direction of the condition of the priority ?</span><br />" + Environment.NewLine
                + help, "icon-help");
        }
    }
}