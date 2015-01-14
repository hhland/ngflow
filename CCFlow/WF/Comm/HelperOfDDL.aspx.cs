using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.En;
using BP.DA;
using BP.Web.Controls;
using BP.Sys;
using BP;

namespace CCFlow.Web.Comm.UI
{
	/// <summary>
	/// UIEn1ToM  The summary .
	/// </summary>
	public partial class HelperOfDDL : System.Web.UI.Page
	{

		#region  Property .		
		public AttrOfOneVSM AttrOfOneVSM 
		{
			get
			{
				Entity en = ClassFactory.GetEn(this.EnsName) ;

				foreach(AttrOfOneVSM attr in en.EnMap.AttrsOfOneVSM)
				{
					if (attr.EnsOfMM.ToString()==this.AttrKey)
					{
						return attr;
					}
				}
				throw new Exception(" Error not found property £® "); 
			}
		}
		/// <summary>
		///  A working class 
		/// </summary>
		public string EnsName
		{
			get
			{			 
				return this.Request.QueryString["EnsName"]  ; 
			}
		}
		public string AttrKey
		{
			get
			{
				return this.Request.QueryString["AttrKey"]  ; 
			}
		}
		public   string PK
		{
			get
			{
				if (ViewState["PK"]==null)
				{
					if (this.Request.QueryString["PK"]!=null)
					{
						ViewState["PK"]=this.Request.QueryString["PK"];
					}
					else
					{
						Entity mainEn=BP.En.ClassFactory.GetEn( this.EnsName ) ;
						ViewState["PK"]=this.Request.QueryString[mainEn.PK];
					}
				}
				return (string)ViewState["PK"];
			}
		}
		#endregion	

		public bool IsLine
		{
			get
			{
				try
				{
					return (bool)ViewState["IsLine"];
				}
				catch
				{
					return false;
				}
			}
			set
			{
				ViewState["IsLine"]=value;
			}
		}

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                //this.GenerLabel(this.Label1," Data Quick Selection ");
                Entities ens = BP.En.ClassFactory.GetEns(this.Request.QueryString["EnsName"]);
                Entity en = ens.GetNewEntity; // = BP.En.ClassFactory.GetEns(this.Request.QueryString["EnsName"] );
                Map map = en.EnMap;
                foreach (Attr attr in map.Attrs)
                {
                    /* map */
                    if (attr.MyFieldType == FieldType.FK || attr.MyFieldType == FieldType.Enum)
                        this.DropDownList1.Items.Add(new ListItem(attr.Desc, attr.Key));
                }
                this.DropDownList1.Items.Add(new ListItem("ÎÞ", "None"));
            }

            try
            {

                this.SetDataV2();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(" Enumeration operation ") || ex.Message.Contains(" The collection has been modified "))
                {
                    System.Threading.Thread.Sleep(3000);
                    this.Response.Redirect(this.Request.RawUrl);
                    return;
                }
            }
        }
		public string RefKey
		{
			get
			{
				return this.Request.QueryString["RefKey"];
			}
		}
		public string RefText
		{
			get
			{
				return this.Request.QueryString["RefText"];
			}
		}
        public void SetDataV2()
        {
            this.UCSys1.Clear();

            Entities ens = BP.En.ClassFactory.GetEns(this.Request.QueryString["EnsName"]);
            ens.RetrieveAll();

            Entity en = ens.GetNewEntity;
            string space = "";
            if (this.DropDownList1.SelectedValue == "None")
            {
                bool isGrade = ens.IsGradeEntities;
                if (isGrade)
                {
                    this.UCSys1.Add("<a name='top' ></a>");
                    int num = ens.GetCountByKey("Grade", 2);
                    if (num > 1)
                    {
                        int i = 0;
                        this.UCSys1.AddTable();
                        this.UCSys1.AddTR();
                        this.UCSys1.AddTDTitle(" No. ");
                        this.UCSys1.AddTDTitle("<img src='../Images/Home.gif' border=0 /> Data Selection Navigation ");
                        this.UCSys1.AddTREnd();
                        foreach (Entity myen in ens)
                        {
                            if (myen.GetValIntByKey("Grade") != 2)
                                continue;

                            i++;
                            this.UCSys1.AddTR();
                            this.UCSys1.AddTDIdx(i);
                            this.UCSys1.AddTD("<a href='#ID" + myen.GetValStringByKey(this.RefKey) + "' >&nbsp;&nbsp;" + myen.GetValStringByKey(this.RefKey) + "&nbsp;&nbsp;" + myen.GetValStringByKey(this.RefText) + "</a>");
                            this.UCSys1.AddTREnd();
                        }
                        this.UCSys1.AddTableEnd();
                    }
                }

                this.UCSys1.AddTable();
                this.UCSys1.AddTR();
                this.UCSys1.AddTDTitle("IDX");
                this.UCSys1.AddTDTitle("");
                this.UCSys1.AddTREnd();

                bool is1 = false;

                int idx = 0;
                foreach (Entity myen in ens)
                {
                    idx++;
                    is1 = this.UCSys1.AddTR(is1);
                    this.UCSys1.AddTDIdx(idx);
                    RadioBtn rb = new RadioBtn();
                    rb.GroupName = "s";
                    if (isGrade)
                    {
                        int grade = myen.GetValIntByKey("Grade");
                        space = "";
                        space = space.PadLeft(grade - 1, '-');
                        space = space.Replace("-", "&nbsp;&nbsp;&nbsp;");
                        //    this.UCSys1.AddTD(space);
                        switch (grade)
                        {
                            case 2:
                                rb.Text = "<a href='#top' name='ID" + myen.GetValStringByKey(this.RefKey) + "' ><Img src='../Images/Top.gif' border=0 /></a><b><font color=green>" + myen.GetValStringByKey(this.RefKey) + myen.GetValStringByKey(this.RefText) + "</font></b>";
                                break;
                            case 3:
                                rb.Text = "<b>" + myen.GetValStringByKey(this.RefKey) + myen.GetValStringByKey(this.RefText) + "</b>";
                                break;
                            default:
                                rb.Text = myen.GetValStringByKey(this.RefKey) + myen.GetValStringByKey(this.RefText);
                                break;
                        }
                    }
                    else
                    {
                        rb.Text = myen.GetValStringByKey(this.RefText);
                    }
                    rb.ID = "RB_" + myen.GetValStringByKey(this.RefKey);

                    string clientscript = "window.returnValue = '" + myen.GetValStringByKey(this.RefKey) + "';window.close();";
                    rb.Attributes["onclick"] = clientscript;
                    //this.UCSys1.Add(rb);
                    //this.UCSys1.AddBR();
                    this.UCSys1.AddTD(rb);
                    this.UCSys1.AddTREnd();
                }
                this.UCSys1.AddTableEnd();
                return;
            }

            string key = this.DropDownList1.SelectedValue;
            Attr attr = en.EnMap.GetAttrByKey(key);
            if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum)
            {
                SysEnums ses = new SysEnums(attr.Key);
                this.UCSys1.AddTable(); //("<TABLE border=1 >"); 
                foreach (SysEnum se in ses)
                {
                    this.UCSys1.Add("<TR><TD class='Toolbar'>");
                    this.UCSys1.Add(se.Lab);
                    this.UCSys1.Add("</TD></TR>");

                    this.UCSys1.Add("<TR><TD>");

                    #region add dtl
                    this.UCSys1.AddTable();
                    int i = -1;
                    foreach (Entity myen in ens)
                    {
                        if (myen.GetValIntByKey(attr.Key) != se.IntKey)
                            continue;

                        i++;
                        if (i == 3)
                            i = 0;
                        if (i == 0)
                            this.UCSys1.Add("<TR>");

                        RadioBtn rb = new RadioBtn();
                        rb.GroupName = "dsfsd";
                        rb.Text = myen.GetValStringByKey(this.RefText);
                        rb.ID = "RB_" + myen.GetValStringByKey(this.RefKey);

                        string clientscript = "window.returnValue = '" + myen.GetValStringByKey(this.RefKey) + "';window.close();";
                        // rb.Attributes["ondblclick"] = clientscript;
                        rb.Attributes["onclick"] = clientscript;

                        this.UCSys1.AddTD(rb);

                        if (i == 2)
                            this.UCSys1.Add("</TR>");
                    }
                    this.UCSys1.Add("</TABLE>");
                    #endregion add dtl.

                    this.UCSys1.Add("</TD></TR>");
                }
                this.UCSys1.Add("</TABLE>");
                return;
            }

            if (attr.Key == "FK_Dept")
            {
                BP.Port.Depts Depts = new BP.Port.Depts();
                Depts.RetrieveAll();

                this.UCSys1.AddTR();
                this.UCSys1.AddTDToolbar(" A grouping ");
                this.UCSys1.AddTREnd();

                this.UCSys1.AddTR();
                this.UCSys1.AddTDBegin();

                this.UCSys1.AddTable();
                /*  Displays navigation information  */
                int i = 0;
                //int span = 2;
                foreach (BP.Port.Dept Dept in Depts)
                {
                    if (Dept.Grade == 2 || Dept.Grade == 1)
                    {
                        i++;
                        this.UCSys1.Add("<TR>");
                        this.UCSys1.AddTDIdx(i);
                        this.UCSys1.AddTD("<a href='#ID_2" + Dept.No + "' >&nbsp;&nbsp;" + Dept.No + "&nbsp;&nbsp;" + Dept.Name + "</a><BR>");
                        this.UCSys1.Add("</TR>");
                    }
                }
                this.UCSys1.AddTableEnd();
                this.UCSys1.AddTDEnd();
                this.UCSys1.AddTREnd();


                // ===================== 
                this.UCSys1.AddTR();
                this.UCSys1.AddTDToolbar(" Two groups ");
                this.UCSys1.AddTREnd();

                this.UCSys1.AddTDBegin();
                this.UCSys1.AddTable();
                /*  Displays navigation information  */
                // int i = 0;
                //int span = 2;
                i = 0;
                foreach (BP.Port.Dept Dept in Depts)
                {
                    i++;
                    this.UCSys1.Add("<TR>");
                    this.UCSys1.AddTDIdx(i);
                    if (Dept.Grade == 2)
                        this.UCSys1.AddTD("&nbsp;&nbsp;<a name='ID_2" + Dept.No + "' >" + Dept.No + "</A>&nbsp;&nbsp;<a href='#ID" + Dept.No + "' ><b>" + Dept.Name + "</b></a><A HREF='#top'><Img src='../Images/Top.gif' border=0 /></a><BR>");
                    else
                        this.UCSys1.AddTD("&nbsp;&nbsp;" + Dept.No + "&nbsp;&nbsp;<a href='#ID" + Dept.No + "' name='#ID_2" + Dept.No + "' >" + Dept.Name + "</a><BR>");

                    this.UCSys1.Add("</TR>");
                }
                this.UCSys1.Add("</Table>");
                this.UCSys1.Add("</TD></TR>");


                //============  Data 
                foreach (BP.Port.Dept groupen in Depts)
                {
                    this.UCSys1.Add("<TR><TD class='Toolbar' >");
                    this.UCSys1.Add("<a href='#ID_2" + groupen.No + "' name='ID" + groupen.No + "' ><Img src='../Images/Top.gif' border=0 /></a>&nbsp;&nbsp;" + groupen.GetValStringByKey(attr.UIRefKeyText));
                    this.UCSys1.Add("</TD></TR>");
                    this.UCSys1.Add("<TR><TD>");

                    #region add info .
                    this.UCSys1.AddTable();
                    i = -1;
                    foreach (Entity myen in ens)
                    {
                        if (myen.GetValStringByKey(attr.Key) != groupen.GetValStringByKey(attr.UIRefKeyValue))
                            continue;

                        i++;
                        if (i == 3)
                            i = 0;

                        if (i == 0)
                            this.UCSys1.Add("<TR>");

                        RadioBtn rb = new RadioBtn();
                        rb.GroupName = "dsfsd";
                        rb.Text = myen.GetValStringByKey(this.RefText);
                        rb.ID = "RB_" + myen.GetValStringByKey(this.RefKey);

                        string clientscript = "window.returnValue = '" + myen.GetValStringByKey(this.RefKey) + "';window.close();";
                        // rb.Attributes["ondblclick"] = clientscript;
                        rb.Attributes["onclick"] = clientscript;

                        this.UCSys1.AddTD(rb);

                        if (i == 2)
                            this.UCSys1.Add("</TR>");
                    }
                    this.UCSys1.Add("</Table>");
                    #endregion add info .

                    this.UCSys1.Add("</TD></TR>");
                }
                this.UCSys1.Add("</TABLE>");
            }
            else
            {
                Entities groupens = ClassFactory.GetEns(attr.UIBindKey);
                groupens.RetrieveAll();

                this.UCSys1.AddTable(); //("<TABLE border=1 >"); 
                if (groupens.Count > 19)
                {
                    this.UCSys1.Add("<TR><TD class='Toolbar' ><img src='../Images/Home.gif' border=0 /> Data Selection Navigation &nbsp;&nbsp;&nbsp;<font size='2'> Prompt : Point packet data packet connection can be reached </font></TD></TR>");

                    this.UCSys1.Add("<TR><TD>");
                    this.UCSys1.AddTable();
                    /*  Displays navigation information  */
                    int i = 0;
                    //int span = 2;
                    foreach (Entity groupen in groupens)
                    {
                        i++;
                        this.UCSys1.AddTR();
                        this.UCSys1.AddTDIdx(i);
                        this.UCSys1.AddTD("<a href='#ID" + groupen.GetValStringByKey(attr.UIRefKeyValue) + "' >&nbsp;&nbsp;" + groupen.GetValStringByKey(attr.UIRefKeyValue) + "&nbsp;&nbsp;" + groupen.GetValStringByKey(attr.UIRefKeyText) + "</a><BR>");
                        this.UCSys1.AddTREnd();
                    }
                    this.UCSys1.Add("</Table>");
                    this.UCSys1.Add("</TD></TR>");
                }

                foreach (Entity groupen in groupens)
                {
                    this.UCSys1.Add("<TR><TD class='Toolbar' >");
                    this.UCSys1.Add("<a href='#top' name='ID" + groupen.GetValStringByKey(attr.UIRefKeyValue) + "' ><Img src='../Images/Top.gif' border=0 /></a>&nbsp;&nbsp;" + groupen.GetValStringByKey(attr.UIRefKeyText));
                    this.UCSys1.Add("</TD></TR>");

                    this.UCSys1.Add("<TR><TD>");

                    #region add info .
                    this.UCSys1.AddTable();
                    int i = -1;
                    foreach (Entity myen in ens)
                    {
                        if (myen.GetValStringByKey(attr.Key) != groupen.GetValStringByKey(attr.UIRefKeyValue))
                            continue;

                        i++;
                        if (i == 3)
                            i = 0;

                        if (i == 0)
                            this.UCSys1.AddTR();

                        RadioBtn rb = new RadioBtn();
                        rb.GroupName = "dsfsd";
                        rb.Text = myen.GetValStringByKey(this.RefText);
                        rb.ID = "RB_" + myen.GetValStringByKey(this.RefKey);

                        string clientscript = "window.returnValue = '" + myen.GetValStringByKey(this.RefKey) + "';window.close();";
                        // rb.Attributes["ondblclick"] = clientscript;
                        rb.Attributes["onclick"] = clientscript;

                        this.UCSys1.AddTD(rb);

                        if (i == 2)
                            this.UCSys1.AddTREnd();
                    }

                    this.UCSys1.AddTableEnd();
                    #endregion add info .

                    this.UCSys1.Add("</TD></TR>");
                }
                this.UCSys1.AddTableEnd();
            }
        }
		 
		 

		#region  Operating  
		public void EditMEns()
		{
			//this.WinOpen(this.Request.ApplicationPath+"/Comm/UIEns.aspx?EnsName="+this.AttrOfOneVSM.EnsOfM.ToString());
		}
		public void Save()
		{

			AttrOfOneVSM attr = this.AttrOfOneVSM ;			 
			Entities ensOfMM = attr.EnsOfMM;
			QueryObject qo = new QueryObject(ensOfMM);
			qo.AddWhere(attr.AttrOfOneInMM,this.PK);
			qo.DoQuery();
			ensOfMM.Delete();  //  Delete previously saved data obtained .

			AttrOfOneVSM attrOM = this.AttrOfOneVSM;
			Entities ensOfM = attrOM.EnsOfM;
			ensOfM.RetrieveAll();
			foreach(Entity en in ensOfM)
			{
				string pk = en.GetValStringByKey( attr.AttrOfMValue ); 
				CheckBox cb = (CheckBox)this.UCSys1.FindControl("CB_"+ pk );
				if (cb.Checked==false)
					continue;

				Entity en1 =ensOfMM.GetNewEntity;
				en1.SetValByKey(attr.AttrOfOneInMM,this.PK);
				en1.SetValByKey(attr.AttrOfMInMM,  pk  );
				en1.Insert();
			}

            Entity enP = BP.En.ClassFactory.GetEn(this.Request.QueryString["EnsName"]);
			if (enP.EnMap.EnType!=EnType.View)
			{
				enP.SetValByKey(enP.PK, this.PK) ;// =this.PK;
				enP.Retrieve(); // Inquiry .
				enP.Update(); //  Perform an update , Write deal   Parent entity   Business logic .
			}
		}
		#endregion 

		#region Web  Form Designer generated code 
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN:  This call is  ASP.NET Web  Form Designer required .
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///  Required method for Designer support  -  Do not use the code editor to modify 
		///  Contents of this method .
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion


        protected void Btn_OK_Click(object sender, EventArgs e)
        {
            Entities ens = BP.En.ClassFactory.GetEns(this.Request.QueryString["EnsName"]);
            ens.RetrieveAll();
            foreach (Entity en in ens)
            {
                RadioBtn rb = (RadioBtn)this.UCSys1.FindControl("RB_" + en.GetValStringByKey(this.RefKey));
                if (rb.Checked == false)
                    continue;

                string val=en.GetValStringByKey(this.RefKey);
                string ddl=this.Request.QueryString["DDLID"];

                if (ddl != null)
                {
                    /*     */
                    //  ddl = ddl.Replace("DDL_");
                    string mainEns=this.Request.QueryString["MainEns"];

                    BP.Sys.UserRegedit ur = new UserRegedit(BP.Web.WebUser.No, mainEns + "_SearchAttrs");
                    string cfgval = ur.Vals;
                    int idx = cfgval.IndexOf(ddl + "=");
                    string start = cfgval.Substring(0, idx);

                    string end = cfgval.Substring(idx);
                    end = end.Substring(end.IndexOf("@"));

                    ur.Vals = start + val + end;
                    ur.Update();
                }
                 

                string clientscript = "<script language='javascript'> window.returnValue = '" + val + "'; window.close(); </script>";
                this.Page.Response.Write(clientscript);
                return;
            }
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetDataV2();
        }
}
}
