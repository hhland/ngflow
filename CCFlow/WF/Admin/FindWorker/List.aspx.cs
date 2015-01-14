using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.En;

namespace CCFlow.WF.Admin.FindWorker
{
    public partial class UIFindWorkerRoles :BP.Web.WebPage
    {
        public int FK_Node
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["FK_Node"]);
                }
                catch
                {
                    return 101;
                }
            }
        }
        public string FK_Flow
        {
            get
            {
                string str= this.Request.QueryString["FK_Flow"];
                if (string.IsNullOrEmpty(str))
                    return "001";
                return str;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            #region  Processing functions .
            FindWorkerRole en = new FindWorkerRole();
            switch (this.DoType)
            {
                case "Del": // Delete .
                    en.OID = this.RefOID;
                    en.Delete();
                    this.WinClose();
                    return;
                case "Up": //Up.
                    en.OID = this.RefOID;
                    en.Retrieve();
                    en.DoUp();
                    this.WinClose();
                    return;
                case "Down": //Down.
                    en.OID = this.RefOID;
                    en.Retrieve();
                    en.DoDown();
                    this.WinClose();
                    return;
                case "UnEnable": //Down.
                    en.OID = this.RefOID;
                    en.Retrieve();
                    en.IsEnable = false;
                    en.Update();
                    this.WinClose();
                    return;
                case "Enable": //Down.
                    en.OID = this.RefOID;
                    en.Retrieve();
                    en.IsEnable = true;
                    en.Update();
                    this.WinClose();
                    return;
                default:
                    break;
            }
            #endregion  Processing functions .


          

          //  this.Pub1.AddH2(nd.Name);

            this.Pub1.AddTable();
            this.Pub1.AddCaption(" Determine the scope of the rules of the recipient , Multiple rules can be used in parallel .");
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("Idx");

            this.Pub1.AddTDTitle(" Main rules ");
            this.Pub1.AddTDTitle("2 Class Rules ");
            this.Pub1.AddTDTitle("2 Level parameters ");

            this.Pub1.AddTDTitle("3 Class Rules ");
            this.Pub1.AddTDTitle("3 Level parameters ");

            this.Pub1.AddTDTitle("4 Class Rules ");
            this.Pub1.AddTDTitle("4 Level parameters ");

            this.Pub1.AddTDTitle(" No Enable ?");
            this.Pub1.AddTDTitle(" Mobile ");
            this.Pub1.AddTDTitle(" Delete ");
            this.Pub1.AddTDTitle(" Editor ");
            this.Pub1.AddTREnd();

            FindWorkerRoles ens = new FindWorkerRoles(this.FK_Node);
            int idx = 0;
            foreach (FindWorkerRole myen in ens)
            {
                idx++;
                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);

                this.Pub1.AddTD(myen.SortText0);

                this.Pub1.AddTD(myen.SortText1);
                this.Pub1.AddTD(myen.TagText1);

                this.Pub1.AddTD(myen.SortText2);
                this.Pub1.AddTD(myen.TagText2);

                this.Pub1.AddTD(myen.SortText3);
                this.Pub1.AddTD(myen.TagText3);

                if (myen.IsEnable == true)
                    this.Pub1.AddTD(myen.IsEnable + "<a href=\"javascript:UnEnable('" + myen.OID + "')\" > Disable </a>");
                else
                    this.Pub1.AddTD(myen.IsEnable + "<a href=\"javascript:Enable('" + myen.OID + "')\" > Enable </a>");

                this.Pub1.AddTD("<a href=\"javascript:Up('" + myen.OID + "')\" ><img src='../../Img/Btn/Up.gif' border=0 /> Move </a>|<a href=\"javascript:Down('" + myen.OID + "')\" ><img src='../../Img/Btn/Down.gif' border=0 /> Down </a>");
                this.Pub1.AddTD("<a href=\"javascript:Del('" + myen.OID + "')\" ><img src='../../Img/Btn/Delete.gif' border=0 /> Delete </a>");
                this.Pub1.AddTD("<a href=\"javascript:Edit('" + myen.SortVal0 + "','" + this.FK_Flow + "','" + this.FK_Node + "','" + myen.OID + "')\" ><img src='../../Img/Btn/Edit.gif' border=0 /> Editor </a>");
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();

            this.Pub1.Add("<a href=\"javascript:New('" + this.FK_Flow + "','" + this.FK_Node + "');\" ><img src='../../Img/Btn/New.gif' border=0 /> New rules to find someone </a>");


        }
    }
}