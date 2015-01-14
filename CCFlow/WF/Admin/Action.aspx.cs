using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.WF.XML;
using BP.En;
using BP.Port;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;

namespace CCFlow.WF.Admin
{
    public partial class WF_Admin_Action : BP.Web.WebPage
    {
        public string Event
        {
            get
            {
                return this.Request.QueryString["Event"];
            }
        }
        public string NodeID
        {
            get
            {
                return this.Request.QueryString["NodeID"];
            }
        }
        public string FK_MapData
        {
            get
            {
                return "ND" + this.Request.QueryString["NodeID"];
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }

        /// <summary>
        ///  The name of the current event set 
        /// </summary>
        public string CurrentEvent { get; set; }
        /// <summary>
        ///  Current events affiliated event source name 
        /// </summary>
        public string CurrentEventGroup { get; set; }

        public bool HaveMsg { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.DoType == "Del")
            {
                FrmEvent delFE = new FrmEvent();
                delFE.MyPK = this.FK_MapData + "_" + this.Request.QueryString["RefXml"];
                delFE.Delete();
            }

            FrmEvents ndevs = new FrmEvents();
            ndevs.Retrieve(FrmEventAttr.FK_MapData, this.FK_MapData);

            EventLists xmls = new EventLists();
            xmls.RetrieveAll();

            BP.WF.XML.EventSources ess = new EventSources();
            ess.RetrieveAll();

            string myEvent = this.Event;
            BP.WF.XML.EventList myEnentXml = null;

            #region // Generate a list of events 

            foreach (EventSource item in ess)
            {
                Pub1.Add(string.Format("<div title='{0}' style='padding:10px; overflow:auto' data-options=''>", item.Name));
                Pub1.AddUL("class='navlist'");

                foreach (BP.WF.XML.EventList xml in xmls)
                {
                    if (xml.EventType != item.No)
                        continue;

                    FrmEvent nde = ndevs.GetEntityByKey(FrmEventAttr.FK_Event, xml.No) as FrmEvent;

                    if (nde == null)
                    {
                        if (myEvent == xml.No)
                        {
                            CurrentEventGroup = item.Name;
                            myEnentXml = xml;
                            Pub1.AddLi(
                string.Format("<div style='font-weight:bold'><a href='javascript:void(0)'><span class='nav'>{0}</span></a></div>{1}", xml.Name, Environment.NewLine));
                        }
                        else
                        {
                            Pub1.AddLi(
                string.Format("<div><a href='Action.aspx?NodeID={0}&Event={1}&FK_Flow={2}&tk={5}'><span class='nav'>{3}</span></a></div>{4}", NodeID, xml.No, FK_Flow, xml.Name, Environment.NewLine, new Random().NextDouble()));
                        }
                    }
                    else
                    {
                        if (myEvent == xml.No)
                        {
                            CurrentEventGroup = item.Name;
                            myEnentXml = xml;
                            Pub1.AddLi(
                                                string.Format("<div style='font-weight:bold'><a href='javascript:void(0)'><span class='nav'>{0}</span></a></div>{1}", xml.Name, Environment.NewLine));
                        }
                        else
                        {
                            Pub1.AddLi(
                string.Format("<div><a href='Action.aspx?NodeID={0}&Event={1}&FK_Flow={2}&MyPK={3}&tk={6}'><span class='nav'>{4}</span></a></div>{5}", NodeID, xml.No, FK_Flow, nde.MyPK, xml.Name, Environment.NewLine, new Random().NextDouble()));
                        }
                    }
                }

                Pub1.AddULEnd();
                Pub1.AddDivEnd();
            }
            #endregion

            if (myEnentXml == null)
            {
                CurrentEvent = " Help ";

                Pub2.Add("<div style='width:100%; text-align:center' data-options='noheader:true'>");
                Pub2.AddH2(" Events ccflow Interface with your application ");

                this.Pub2.AddUL();
                this.Pub2.AddLi(" In the process of moving the process will produce a lot of events , Such as : Node before sending , When sent successfully , When sending failed , Return ago , After back .");
                this.Pub2.AddLi(" In these events in ccflow Allows you to write business logic call , Complete with interactive interface , Interaction with other systems , And other processes involved in human interaction .");
                this.Pub2.AddLi(" According to the type of event ,ccflow The event is divided into : Node , Form , Process three events .");
                this.Pub2.AddULEnd();


                Pub2.AddDivEnd();
                return;
            }

            FrmEvent mynde = ndevs.GetEntityByKey(FrmEventAttr.FK_Event, myEvent) as FrmEvent;
            if (mynde == null)
            {
                mynde = new FrmEvent();
                mynde.FK_Event = myEvent;
            }

            this.Title = " Set up : Event interface =》" + myEnentXml.Name;
            this.CurrentEvent = myEnentXml.Name;
            int col = 50;

            Pub2.Add("<div id='tabMain' class='easyui-tabs' data-options='fit:true'>");

            Pub2.Add("<div title=' Event interface ' style='padding:5px'>" + Environment.NewLine);
            Pub2.Add("<iframe id='src1' frameborder='0' src='' style='width:100%;height:100%' scrolling='auto'></iframe>");
            Pub2.Add("</div>" + Environment.NewLine);

            if (myEnentXml.IsHaveMsg == true)
            {
                HaveMsg = true;
                Pub2.Add("<div title=' Push messages to the parties ' style='padding:5px'>" + Environment.NewLine);
                Pub2.Add("<iframe id='src2' frameborder='0' src='' style='width:100%;height:100%' scrolling='auto'></iframe>");
                Pub2.Add("</div>" + Environment.NewLine);

                Pub2.Add("<div title=' Other designated person to push message ' style='padding:5px'>" + Environment.NewLine);
                Pub2.Add("<iframe id='src3' frameborder='0' src='' style='width:100%;height:100%' scrolling='auto'></iframe>");
                Pub2.Add("</div>" + Environment.NewLine);
            }

            BP.WF.Dev2Interface.Port_Login("zhoupeng");

            BP.WF.Dev2Interface.Port_SigOut();

            Pub2.Add("</div>");
        }
    }
}