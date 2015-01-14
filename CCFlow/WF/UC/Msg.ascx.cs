using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BP.WF;
using BP.Port;
using BP.Sys;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.UC
{
    public partial class Msg : BP.Web.UC.UCBase3
    {
        public int MsgSta
        {
            get
            {
                string msg = this.Request.QueryString["Sta"];
                if (msg == null || msg == "1")
                    return 1;
                else
                    return 0;
            }
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            this.Page.Title = " System Messages ";
            switch (this.DoType)
            {
                case "Del":
                    //BP.Sys.Msg msg = new BP.Sys.Msg();
                    //msg.OID = this.RefOID;
                    //msg.Retrieve();
                    //if (msg.Accepter == WebUser.No)
                    //{
                    //    msg.Delete();
                    //}
                    break;
                default:
                    break;
            }
            this.Bind();
        }
        public void Bind()
        {



            if (WebUser.IsWap)
                this.Left.Add("<a href='Home.aspx'><img src='/WF/Img/Home.gif' border=0/>Home</a>");

            this.Left.AddUL();
            this.Left.AddLi("Msg.aspx?Sta=0", " Unread ");
            this.Left.AddLi("Msg.aspx?Sta=1", " Read ");
            this.Left.AddLi("Msg.aspx?Sta=9", " Sent ");
            this.Left.AddLi("javascript:WinOpen('./../WF/Msg/Write.aspx')", " Write ");
            this.Left.AddULEnd();

            int colspan = 5;
            //BP.TA.Msgs ens = new BP.Sys.Msgs();
            //if (this.MsgSta == 9)
            //    ens.Retrieve(BP.Sys.MsgAttr.Sender, WebUser.No);
            //else
            //    ens.Retrieve(BP.Sys.MsgAttr.Accepter, WebUser.No, BP.Sys.MsgAttr.MsgSta, this.MsgSta);


            this.Pub1.AddTable("width='90%'");
            this.Pub1.AddTR();
            this.Pub1.Add("<TD class=TitleTop colspan=" + colspan + "></TD>");
            this.Pub1.AddTREnd();

            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("");
            this.Pub1.AddTDTitle("");
            this.Pub1.AddTDTitle(" Title ");
            this.Pub1.AddTDTitle(" Sender ");
            this.Pub1.AddTDTitle(" Send date ");
            this.Pub1.AddTREnd();
            int i = 0;
            bool is1 = false;
            //foreach (BP.Sys.Msg en in ens)
            //{
            //    i++;
            //    is1 = this.Pub1.AddTR(is1);
            //    CheckBox cb = new CheckBox();
            //    cb.ID = "CB_" + en.OID;
            //    this.Pub1.AddTDIdx(i);
            //    this.Pub1.AddTD(cb);
            //    this.Pub1.AddTDA("javascript:WinOpen('./Msg/Read.aspx?RefOID=" + en.OID + "','sd');", en.Title);
            //    this.Pub1.AddTD(en.SenderText);
            //    this.Pub1.AddTD(en.RDT);
            //    this.Pub1.AddTREnd();
            //}
            this.Pub1.AddTableEnd();
        }

    }

}