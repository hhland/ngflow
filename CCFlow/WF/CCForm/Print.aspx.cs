using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF.Template;
using BP.WF;
using BP.Sys;
using BP.Port;
using BP.Web.Controls;
using BP.DA;
using BP.En;
using BP.Web;
namespace CCFlow.WF.CCForm
{
    public partial class WF_CCForm_Print : BP.Web.WebPage
    {
        #region  Property 
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        public Int64 WorkID
        {
            get
            {
                try
                {
                    return Int64.Parse(this.Request.QueryString["WorkID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public Int64 FID
        {
            get
            {
                try
                {
                    return Int64.Parse(this.Request.QueryString["FID"]);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public string BillIdx
        {
            get
            {
                return this.Request.QueryString["BillIdx"];
            }
        }
        #endregion  Property 

        string ApplicationPath = null;
        public void PrintBill()
        {
            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            string path = ApplicationPath + "\\DataUser\\CyclostyleFile\\FlowFrm\\" + nd.FK_Flow + "\\" + nd.NodeID + "\\";
            if (System.IO.Directory.Exists(path) == false)
            {
                this.Pub1.AddMsgOfWarning(" Get Template Error ", " Template file not found ." + path);
                return;
            }

            string[] fls = System.IO.Directory.GetFiles(path);
            string file = fls[int.Parse(this.BillIdx)];
            file = file.Replace(ApplicationPath + @"DataUser\CyclostyleFile", "");

            FileInfo finfo = new FileInfo(file);
            string tempName = finfo.Name.Split('.')[0];
            string tempNameChinese = finfo.Name.Split('.')[1];

            string toPath = ApplicationPath + @"DataUser\Bill\FlowFrm\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (System.IO.Directory.Exists(toPath) == false)
                System.IO.Directory.CreateDirectory(toPath);

           // string billFile = toPath + "\\" + tempName + "." + this.FID + ".doc";
            string billFile = toPath + "\\" + Server.UrlDecode(tempNameChinese) + "." + this.WorkID + ".doc";

            BP.Pub.RTFEngine engine = new BP.Pub.RTFEngine();
            if (tempName.ToLower() == "all")
            {
                /*  Description taken from the flow of data on all forms .*/
                FrmNodes fns = new FrmNodes(this.FK_Flow, this.FK_Node);
                foreach (FrmNode fn in fns)
                {
                    GEEntity ge = new GEEntity(fn.FK_Frm, this.WorkID);
                    engine.AddEn(ge);
                    MapDtls mdtls = new MapDtls(fn.FK_Frm);
                    foreach (MapDtl dtl in mdtls)
                    {
                        GEDtls enDtls = dtl.HisGEDtl.GetNewEntities as GEDtls;
                        enDtls.Retrieve(GEDtlAttr.RefPK, this.WorkID);
                        engine.EnsDataDtls.Add(enDtls);
                    }
                }

                //  Increase in the main table .
                GEEntity myge = new GEEntity("ND" + nd.NodeID, this.WorkID);
                engine.AddEn(myge);
                MapDtls mymdtls = new MapDtls("ND" + nd.NodeID);
                foreach (MapDtl dtl in mymdtls)
                {
                    GEDtls enDtls = dtl.HisGEDtl.GetNewEntities as GEDtls;
                    enDtls.Retrieve(GEDtlAttr.RefPK, this.WorkID);
                    engine.EnsDataDtls.Add(enDtls);
                }
                
                // engine.MakeDoc(file, toPath, tempName + "." + this.WorkID + ".doc", null, false);
                engine.MakeDoc(file, toPath, Server.UrlDecode(tempNameChinese) + "." + this.WorkID + ".doc", null, false);
            }
            else
            {
                //  Increase in the main table .
                GEEntity myge = new GEEntity(tempName, this.WorkID);
                engine.HisGEEntity = myge;
                engine.AddEn(myge);

                MapDtls mymdtls = new MapDtls(tempName);
                foreach (MapDtl dtl in mymdtls)
                {
                    GEDtls enDtls = dtl.HisGEDtl.GetNewEntities as GEDtls;
                    enDtls.Retrieve(GEDtlAttr.RefPK, this.WorkID);
                    engine.EnsDataDtls.Add(enDtls);
                }
                //engine.MakeDoc(file, toPath, tempName + "." + this.FID + ".doc", null, false);
                engine.MakeDoc(file, toPath, Server.UrlDecode(tempNameChinese) + "." + this.WorkID + ".doc", null, false);
            }

            BP.Sys.PubClass.OpenWordDocV2(billFile, tempNameChinese + ".doc");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplicationPath = this.Request.PhysicalApplicationPath;
            this.Title = " Document Printing ";
            if (this.BillIdx != null)
            {
                this.PrintBill();
                return;
            }
            this.Pub1.AddTable();
            this.Pub1.AddTR();
            this.Pub1.AddTDTitle("ID");
            this.Pub1.AddTDTitle(" Form Number ");
            this.Pub1.AddTDTitle(" Form name ");
            this.Pub1.AddTDTitle(" Download ");
            this.Pub1.AddTREnd();

            BP.WF.Node nd = new BP.WF.Node(this.FK_Node);
            string path = ApplicationPath + @"DataUser\CyclostyleFile\FlowFrm\" + nd.FK_Flow + "\\" + nd.NodeID + "\\";
            string[] fls = null;
            try
            {
                fls = System.IO.Directory.GetFiles(path);
            }
            catch
            {
                this.Pub1.AddTableEnd();
                this.Pub1.AddMsgOfWarning(" Get Template Error ", " Template file not found ." + path);
                return;
            }

            int idx = 0;
            int fileIdx = -1;
            foreach (string f in fls)
            {
                fileIdx++;
                string myfile = f.Replace(path, "");
                //if (myfile.ToLower().Contains(".rtf") == false)
                //    continue;

                string[] strs = myfile.Split('.');
                idx++;

                this.Pub1.AddTR();
                this.Pub1.AddTDIdx(idx);
                this.Pub1.AddTD(strs[0]);
                this.Pub1.AddTD(strs[1]);

                if (f.Contains(".grf"))
                {
                    string fileName = f.Split('\\')[f.Split('\\').Length-1];
                    this.Pub1.AddTD("<a href='javascript:btnPreview_onclick(\"" + fileName + "\")' > Print </a>");
                }
                else
                {
                    this.Pub1.AddTD("<a href='Print.aspx?FK_Node=" + this.FK_Node + "&FID=" + this.FID + "&FK_Flow="+this.FK_Flow+"&WorkID=" + this.WorkID + "&BillIdx=" + fileIdx + "' target=_blank > Print </a>");    
                }
                
                this.Pub1.AddTREnd();
            }
            this.Pub1.AddTableEnd();
        }
    }
}