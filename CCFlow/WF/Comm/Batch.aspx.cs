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
using BP.En;
using BP.DA;
using BP.Web;
using BP.Port;
using BP.Web.Controls;
using BP.Sys;
using BP;

namespace CCFlow.WF.Comm
{
    public partial class Comm_Batch : BP.Web.WebPage
    {
        public bool IsS
        {
            get
            {
                string str = this.Request.QueryString["IsS"];
                if (str == null || str == "0")
                    return false;
                return true;
            }
        }
        private new Entities _HisEns = null;
        public new Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                    _HisEns = ClassFactory.GetEns(this.EnsName);
                return _HisEns;
            }
        }
        public new string EnsName
        {
            get
            {
                string s = this.Request.QueryString["EnsName"];
                if (s == null)
                    s = "BP.Port.0000";
                return s;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Entities ens = ClassFactory.GetEns(this.EnsName);
            Entity en = ens.GetNewEntity;
            Map map = en.EnMap;

            this.Title = this.HisEn.EnMap.EnDesc;

            // this.ToolBar1.AddLab("sd", this.GenerCaption(this.HisEn.EnMap.EnDesc + "" + this.HisEn.EnMap.TitleExt));
            this.ToolBar1.AddLab("sd", "<b>" + this.HisEn.EnMap.EnDesc + "</b>:");

            this.ToolBar1.InitByMapV2(map, 1);

            #region  Setting choice   Defaults 
            // Determine whether there is passed in the query set , If it is set to null .
            bool isHave = false;
            AttrSearchs searchs = map.SearchAttrs;
            foreach (AttrSearch attr in searchs)
            {
                string mykey = this.Request.QueryString[attr.Key];
                if (mykey == "" || mykey == null)
                    continue;
                isHave = true;
            }

            if (isHave == true)
            {
                this.ToolBar1.GetTBByID("TB_Key").Text = "";
                /* Empty the existing query conditions set  */
                foreach (AttrSearch attr in searchs)
                {
                    string mykey = this.Request.QueryString[attr.Key];
                    if (mykey == "" || mykey == null)
                    {
                        if (attr.Key == "FK_Dept")
                            this.ToolBar1.GetDDLByKey("DDL_" + attr.Key).SetSelectItem(WebUser.FK_Dept, attr.HisAttr);
                        else
                            this.ToolBar1.GetDDLByKey("DDL_" + attr.Key).SetSelectItem("all", attr.HisAttr);
                        continue;
                    }
                    this.ToolBar1.GetDDLByKey("DDL_" + attr.Key).SetSelectItem(mykey, attr.HisAttr);
                }
            }
            #endregion

            //   this.BPToolBar1.ButtonClick += new System.EventHandler(this.ButtonClick);
            if (en.HisUAC.IsInsert)
                this.ToolBar1.AddLab("inse",
                    "<input type=button class=Btn id='ToolBar1$Btn_New' name='ToolBar1$Btn_New' onclick=\"javascript:ShowEn('./RefFunc/UIEn.aspx?EnsName=" + this.EnsName + "','cd','" + BP.Sys.EnsAppCfgs.GetValInt(this.EnsName, "WinCardH") + "' , '" + BP.Sys.EnsAppCfgs.GetValInt(this.EnsName, "WinCardW") + "');\"  value=' New (N)' />");

            if (WebUser.No == "admin")
                this.ToolBar1.AddLab("sw", "<input type=button class=Btn  id='ToolBar1$Btn_P' class=Btn name='ToolBar1$Btn_P'  onclick=\"javascript:OpenAttrs('" + this.EnsName + "');\"  value=' Set up (P)'  />");

            //this.ToolBar1.AddLab("sw", "<input type=button class=Btn  id='ToolBar1$Btn_P' name='ToolBar1$Btn_P'  onclick=\"javascript:OpenAttrs('" + this.EnsName + "');\"  value=' Set up (P)'  />");

            // this.ToolBar1.AddLab("s", "<input type=button onclick=\"javascript:OpenAttrs('" + this.EnsName + "');\"  value=' Set up (S)'  />");
            this.SetDGData();

            // Output execution information .
            if (this.Session["Info"] != null)
            {
                if (this.Session["Info"].ToString().Contains(" Aborting a thread ")
                    || this.Session["Info"].ToString().Contains("Thread"))
                {
                    this.Session["Info"] = null;
                }
                else
                {
                    this.ResponseWriteBlueMsg(this.Session["Info"].ToString());
                    this.Session["Info"] = null;
                }
            }

            this.ToolBar1.GetBtnByID("Btn_Search").Click += new System.EventHandler(this.ButtonClick);
            //this.ToolBar1.Btn_Click();
            //   this.GenerLabel(this.Lab1, this.HisEn);
            //this.GenerLabel(this.Lab1, "<b>" + map.EnDesc + "</b>" + map.TitleExt);
            this.UCSys2.Add("<a href='./Sys/EnsDataIO.aspx?EnsName=" + this.EnsName + "' target=_blank><img src='../Img/Btn/Excel.gif' border=0 /> Importing / Export </a>");
        }
        private void ButtonClick(object sender, System.EventArgs e)
        {
            this.UCSys1.Clear();
            this.UCSys2.Clear();
            this.UCSys3.Clear();
            this.SetDGData(1, true);

            //Entities ens = this.HisEns;
            //Entity en = ens.GetNewEntity;
            //QueryObject qo = new QueryObject(ens);
            //qo = this.ToolBar1.GetnQueryObject(ens, en);
            this.Response.Redirect("Batch.aspx?EnsName=" + this.EnsName, true);
        }


        #region  Deal with 
        public Entities SetDGData()
        {
            return this.SetDGData(this.PageIdx, false);
        }
        public Entities SetDGData(int pageIdx, bool isSearch)
        {
            //  this.BPToolBar1.SaveSearchState(this.EnsName, this.Key);
            this.ToolBar1.SaveSearchState(this.EnsName, this.Key);

            Entities ens = this.HisEns;
            Entity en = ens.GetNewEntity;
            QueryObject qo = new QueryObject(ens);
            qo = this.ToolBar1.GetnQueryObject(ens, en);
            string url = this.Request.RawUrl;
            if (url.IndexOf("PageIdx") != -1)
                url = url.Substring(0, url.IndexOf("PageIdx") - 1);

            this.UCSys2.Clear();
            int maxPageNum = 0;
            try
            {
                maxPageNum = this.UCSys2.BindPageIdx(qo.GetCount(), SystemConfig.PageSize, pageIdx, "Batch.aspx?EnsName=" + this.EnsName);
            }
            catch (Exception ex)
            {
                en.CheckPhysicsTable();
                throw ex;
            }

            if (isSearch)
                return null;


            if (maxPageNum > 1)
                this.UCSys2.Add( " Flip key :← → PageUp PageDown");

            qo.DoQuery(en.PK, SystemConfig.PageSize, pageIdx);

            this.UCSys1.DataPanelDtlCheckBox(ens);

            //if (this.IsS == false)
            //    this.UCSys3.Add("[<a href='Batch.aspx?EnsName=" + this.EnsName + "&PageIdx=" + this.PageIdx + "&IsS=1'> Select all </a>]&nbsp;&nbsp;");
            //else
            //    this.UCSys3.Add("[<a href='Batch.aspx?EnsName=" + this.EnsName + "&PageIdx=" + this.PageIdx + "&IsS=0'> Clear All </a>]&nbsp;&nbsp;");

            RefMethods rms = en.EnMap.HisRefMethods;
            foreach (RefMethod rm in rms)
            {
                if (rm.IsCanBatch == false)
                    continue;

                Button btn = new Button();
                btn.ID = "Btn_" + rm.Index;
                btn.Text = rm.Title;
                btn.CssClass = "Btn";
                if (rm.Warning == null)
                    btn.Attributes["onclick"] = " return confirm(' Are you sure you want to perform ?');";
                else
                    btn.Attributes["onclick"] = " return confirm('" + rm.Warning + "');";

                this.UCSys3.Add(btn);
                btn.Click += new EventHandler(btn_Click);
            }

            UAC uac = en.HisUAC;
            if (uac.IsDelete)
            {
                Button btn = new Button();
                btn.ID = "Btn_Del";
                btn.CssClass = "Btn";

                btn.Text = " Delete ";
                btn.Attributes["onclick"] = " return confirm(' You acknowledge that you ?');";
                btn.Attributes["class"] = "Button";
                this.UCSys3.Add(btn);
                btn.Click += new EventHandler(btn_Click);
            }

            MoveToShowWay showWay = (MoveToShowWay)ens.GetEnsAppCfgByKeyInt("MoveToShowWay");

            //  Perform the move .
            if (showWay != MoveToShowWay.None)
            {

                string MoveTo = en.GetCfgValStr("MoveTo");
                if (en.EnMap.Attrs.Contains(MoveTo) == false)
                {
                    this.Alert("Moveto  Field error , Entity does not contain field :" + MoveTo);
                    return null;
                }


                Attr attr = en.EnMap.GetAttrByKey(MoveTo);
                if (showWay == MoveToShowWay.DDL)
                {
                    Button btnM = new Button();
                    btnM.ID = "Btn_Move";
                    btnM.CssClass = "Btn";

                    btnM.Text = " Move to ";
                    btnM.Attributes["onclick"] = "return confirm(' Are you sure you want to move it ?');";
                    this.UCSys3.Add("&nbsp;&nbsp;");
                    this.UCSys3.Add(btnM);

                    btnM.Click += new EventHandler(btn_Move_Click);

                    DDL ddl = new DDL();
                    ddl.ID = "DDL_MoveTo1";
                    if (attr.IsEnum)
                    {
                        ddl.BindSysEnum(attr.Key);
                        ddl.Items.Insert(0, new ListItem(" Choose " + "=>" + attr.Desc, "all"));
                    }
                    else
                    {
                        EntitiesNoName ens1 = attr.HisFKEns as EntitiesNoName;
                        ens1.RetrieveAll();
                        ddl.BindEntities(ens1);
                        ddl.Items.Insert(0, new ListItem(" Choose " + "=>" + attr.Desc, "all"));
                    }
                    this.UCSys3.Add(ddl);
                }

                if (showWay == MoveToShowWay.Panel)
                {
                    if (attr.IsEnum)
                    {
                        SysEnums ses = new SysEnums(attr.Key);
                        foreach (SysEnum se in ses)
                        {
                            Button btn = new Button();
                            btn.CssClass = "Btn";

                            btn.ID = "Btn_Move_" + se.IntKey;
                            btn.Text = " Set up " + ":" + se.Lab;
                            btn.Attributes["onclick"] = "return confirm(' Are you sure you want to perform settings [" + se.Lab + "]?');";
                            btn.Click += new EventHandler(btn_Move_Click);
                            this.UCSys3.Add(btn);
                            this.UCSys3.Add("&nbsp;&nbsp;");
                        }
                    }
                    else
                    {
                        EntitiesNoName ens1 = attr.HisFKEns as EntitiesNoName;
                        ens1.RetrieveAll();
                        foreach (EntityNoName en1 in ens1)
                        {
                            Button btn = new Button();
                            btn.CssClass = "Btn";
                            btn.ID = "Btn_Move_" + en1.No;
                            btn.Text = " Set up :" + en1.Name;
                            btn.Attributes["onclick"] = "return confirm(' Are you sure you want to set [" + en1.Name + "]?');";
                            btn.Click += new EventHandler(btn_Move_Click);
                            this.UCSys3.Add(btn);
                            this.UCSys3.Add("&nbsp;&nbsp;");
                        }
                    }
                }
            }

            int ToPageIdx = this.PageIdx + 1;
            int PPageIdx = this.PageIdx - 1;

            this.UCSys3.Add("<SCRIPT language=javascript>");
            this.UCSys3.Add("\t\n document.onkeydown = chang_page;");
            this.UCSys3.Add("\t\n function chang_page() {");
            //  this.UCSys3.Add("\t\n  alert(event.keyCode); ");
            if (this.PageIdx == 1)
            {
                this.UCSys3.Add("\t\n if (event.keyCode == 37 || event.keyCode == 33) alert(' The first page is already ');");
            }
            else
            {
                this.UCSys3.Add("\t\n if (event.keyCode == 37  || event.keyCode == 38 || event.keyCode == 33) ");
                this.UCSys3.Add("\t\n     location='Batch.aspx?EnsName=" + this.EnsName + "&PageIdx=" + PPageIdx + "';");
            }
            if (this.PageIdx == maxPageNum)
            {
                this.UCSys3.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) alert(' This is the last one ');");
            }
            else
            {
                this.UCSys3.Add("\t\n if (event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 34) ");
                this.UCSys3.Add("\t\n     location='Batch.aspx?EnsName=" + this.EnsName + "&PageIdx=" + ToPageIdx + "';");
            }

            this.UCSys3.Add("\t\n } ");
            this.UCSys3.Add("</SCRIPT>");
            return ens;
        }
        void btn_Move_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string val = "";
            if (btn.ID == "Btn_Move")
                val = this.UCSys3.GetDDLByID("DDL_MoveTo1").SelectedValue;
            else
                val = btn.ID.Substring("Btn_Move".Length + 1);


            Entity en = this.HisEns.GetNewEntity;
            Map map = en.EnMap;
            string msg = "";
            if (val == "all")
                return;

            string title = null;
            if (map.Attrs.Contains("Name"))
                title = "Name";

            if (map.Attrs.Contains("Title"))
                title = "Title";

            string moveTo = en.GetCfgValStr("MoveTo");

            foreach (Control ctl in this.UCSys1.Controls)
            {
                if (ctl == null || ctl.ID == null || ctl.ID == "")
                    continue;
                if (ctl.ID.Contains("CB_") == false)
                    continue;
                CheckBox cb = ctl as CheckBox;
                if (cb == null)
                    continue;
                if (cb.Checked == false)
                    continue;
                string id = ctl.ID.Substring(3);
                try
                {
                    en.PKVal = id;
                    en.Retrieve();
                    en.Update(moveTo, val);
                    if (title == null)
                        msg += "<hr> Mobile Success :<font color=green>" + en.PKVal + "</font>";
                    else
                        msg += "<hr> Mobile Success :<font color=green>" + en.PKVal + " : " + en.GetValStrByKey(title) + "</font>";
                }
                catch (Exception ex)
                {
                    msg += "<hr> Move failed :<font color=red>" + en.PKVal + ",  Exception Information :" + ex.Message + "</font>";
                }
            }
            if (msg == "")
                msg = " You did not select the row ...";

            this.Session["Info"] = msg;
            this.Response.Redirect("Batch.aspx?EnsName=" + this.EnsName, true);
        }
        void btn_Click(object sender, EventArgs e)
        {
            string msg = "";
            Button btn = sender as Button;
            Entity en = this.HisEns.GetNewEntity;
            if (btn.ID == "Btn_Del")
            {
                foreach (Control ctl in this.UCSys1.Controls)
                {
                    if (ctl == null || ctl.ID == null || ctl.ID == "")
                        continue;
                    if (ctl.ID.Contains("CB_") == false)
                        continue;
                    CheckBox cb = ctl as CheckBox;
                    if (cb == null)
                        continue;
                    if (cb.Checked == false)
                        continue;
                    string id = ctl.ID.Substring(3);
                    try
                    {
                        en.PKVal = id;
                        en.Delete();
                        msg += "<hr> Deleted successfully :<font color=green>" + en.PKVal + "</font>";
                    }
                    catch (Exception ex)
                    {
                        msg += "<hr> Delete error :<font color=red>" + en.PKVal + ",  Exception Information :" + ex.Message + "</font>";
                    }
                }
            }
            else
            {
                int idx = int.Parse(btn.ID.Replace("Btn_", ""));
                foreach (Control ctl in this.UCSys1.Controls)
                {
                    if (ctl == null || ctl.ID == null || ctl.ID == "")
                        continue;
                    if (ctl.ID.Contains("CB_") == false)
                        continue;

                    CheckBox cb = ctl as CheckBox;
                    if (cb == null)
                        continue;
                    if (cb.Checked == false)
                        continue;

                    string id = ctl.ID.Substring(3);
                    try
                    {
                        en.PKVal = id;
                        en.Retrieve();
                        BP.En.RefMethod rm = en.EnMap.HisRefMethods[idx];
                        rm.HisEn = en;
                        msg += "<hr> Carried out :" + en.PKVal + "  Information :<br>" + rm.Do(null);
                    }
                    catch (Exception ex)
                    {
                        msg += "<hr> Execution error :<font color=red> Primary key value :" + en.PKVal + "<br>" + ex.Message + "</font>";
                    }
                }
            }
            if (msg == "")
                msg = " You did not select the row ...";

            this.Session["Info"] = msg;
            this.Response.Redirect("Batch.aspx?EnsName=" + this.EnsName, true);
            // this.Response.Redirect(this.Request.RawUrl, true);
        }
        #endregion
    }
}