using System;
using System.Data;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;
using System.Drawing ;
using BP ;
using BP.Sys;
using System.ComponentModel;


namespace BP.Web.Controls  
{
	
	/// <summary>
	/// BPListBox  The summary .
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Web.UI.WebControls.DropDownList))]
	public class DDL:System.Web.UI.WebControls.DropDownList
	{
		#region  Deal with showtype
		/// <summary>
		///  Associated Key .
		/// </summary>
		public string SelfEnsRefKey
		{
			get
			{
				return (string)ViewState["EnsKey"];
			}
			set
			{
				ViewState["EnsKey"]=value;
			}
		}
		/// <summary>
		///  Associated Text
		/// </summary>
		public string SelfEnsRefKeyText
		{
			get
			{
				return (string)ViewState["SelfEnsRefKeyText"];
			}
			set
			{
				ViewState["SelfEnsRefKeyText"]=value;
			}
		}
		public void BindThisYearMonth()
		{
			string year=DataType.CurrentYear ;
			int i  = 0;
			this.Items.Clear();
            while (i < 12)
            {
                i++;
                string val = year + "-" + i.ToString().PadLeft(2, '0');
                this.Items.Add(new ListItem(val, val));
            }

			int myyear= int.Parse(year)+1;
			year =myyear.ToString();
			i = 0;
			while (i < 12)
			{
				i++;
				string val=year+"-"+i.ToString().PadLeft(2 , '0' ) ;
				this.Items.Add( new ListItem( val,val  ) );
			}
		}
		public void BindEntities(Entities ens, string refkey, string reftext, bool isShowKey)
		{
			this.Items.Clear();
            foreach (Entity en in ens)
            {
                if (isShowKey)
                    this.Items.Add(new ListItem(en.GetValStringByKey(refkey) + " " + en.GetValStringByKey(reftext), en.GetValStringByKey(refkey)));
                else
                    this.Items.Add(new ListItem(en.GetValStringByKey(reftext), en.GetValStringByKey(refkey)));
            }
		}
		public void BindEntities(Entities ens, string refkey, string reftext,bool isShowKey, AddAllLocation where)
		{
			this.Items.Clear();
			if (where==AddAllLocation.Top || where==AddAllLocation.TopAndEnd )
			{
				ListItem li = new ListItem("-= Whole =-","all") ;
				this.Items.Add(li);
			}

			foreach(Entity en in ens)
			{
				if (isShowKey)
					this.Items.Add(new ListItem(en.GetValStringByKey(refkey)+" "+en.GetValStringByKey(reftext), en.GetValStringByKey(refkey)) )  ; 
				else 
					this.Items.Add(new ListItem( en.GetValStringByKey(reftext), en.GetValStringByKey(refkey)) )  ; 
			}

			if (where==AddAllLocation.End  || where==AddAllLocation.TopAndEnd )
			{
				ListItem li = new ListItem("-= Whole =-","all") ;
				this.Items.Add(li);
				//this.Items.
			}
		}
		private DDLShowType _ShowType=DDLShowType.None;
		public DDLShowType SelfShowType
		{
			get
			{
				return _ShowType;				
			}
			set
			{
				this._ShowType=value;
			}
		}
		/// <summary>
		/// BindBindSysEnum
		/// </summary>
		/// <param name="enumKey"></param>
        public void BindSysEnum(string enumKey)
        {
            SelfBindSysEnum(enumKey);
        }
		public void BindSysEnum(string enumKey, int selecVal)
		{
			this.SelfDefaultVal=selecVal.ToString();
			SelfBindSysEnum(enumKey);
		}
        public void BindSysEnum(string key, bool isShowKey, AddAllLocation alllocal)
        {
            Sys.SysEnums ens = new BP.Sys.SysEnums(key);
            this.Items.Clear();
            if (alllocal == AddAllLocation.TopAndEnd
                || alllocal == AddAllLocation.Top 
                || alllocal == AddAllLocation.TopAndEndWithMVal)
                this.Items.Add(new ListItem(">>", "all"));

            foreach (SysEnum en in ens)
            {
                if (this.SelfIsShowVal)
                {
                    this.Items.Add(new ListItem(en.IntKey + " " + en.Lab, en.IntKey.ToString()));
                }
                else
                {
                    ListItem li = new ListItem(en.Lab, en.IntKey.ToString());
                    //li.Attributes.CssStyle.Add("style", "color:" + en.Style);
                    //li.Attributes.Add("color", en.Style);
                    //li.Attributes.Add("style", "color:" + en.Style);

                    this.Items.Add(li);
                }
            }

            if (alllocal == AddAllLocation.TopAndEndWithMVal && this.Items.Count >= 4)
            {
                ListItem liMvals = new ListItem("* Number of combinations ..", "mvals");
                liMvals.Attributes.CssStyle.Add("style", "color:green");
                liMvals.Attributes.Add("color", "green");
                liMvals.Attributes.Add("style", "color:green");
                this.Items.Add(liMvals); //new ListItem("* Specify options ...", "mvals"));
            }

            if (alllocal == AddAllLocation.TopAndEnd
                || alllocal == AddAllLocation.End
                || alllocal == AddAllLocation.TopAndEndWithMVal)
                this.Items.Add(new ListItem("-= Whole =-", "all"));
        }
		public void SelfBindSysEnum()
		{
			this.SelfBindSysEnum(this.SelfBindKey.Trim());
		}
		/// <summary>
		///  According to enum
		/// </summary>
		public void SelfBindSysEnum(string enumKey)
		{			
			this.Items.Clear();

			if (this.Enabled)
			{
				SysEnums ens = new SysEnums(enumKey);
				foreach(SysEnum en in ens)
				{
					if (this.SelfIsShowVal)
					{
						this.Items.Add(new ListItem(en.IntKey+" "+en.Lab, en.IntKey.ToString() ));	
					}
					else
					{
						ListItem li = new ListItem( en.Lab , en.IntKey.ToString() );
                        //li.Attributes.Add("color",en.Style);
                        //li.Attributes.CssStyle.Add("color",en.Style);
                        ////li.Attributes.Add("onclick","alert('hello')");
						this.Items.Add(li );
					}
				}
				this.SetSelectItem(this.SelfDefaultVal) ;
			}
			else
			{
				this.Items.Add( new ListItem(this.SelfDefaultText,this.SelfDefaultVal) ) ; 
			}
		}
		public void SelfBind(string key)
		{
			this.SelfBindKey=key;
			this.Items.Clear();
			this.SelfBind();
		}
        public void SelfBind()
        {
            switch (this.SelfShowType)
            {
                case DDLShowType.Boolean:
                    this.Items.Add(new ListItem("Yes", "1"));
                    this.Items.Add(new ListItem("No", "0"));
                    break;
                case DDLShowType.Gender:
                    this.Items.Add(new ListItem("Male", "1"));
                    this.Items.Add(new ListItem("Female", "0"));
                    break;
                case DDLShowType.SysEnum: ///  Enumerated type 
                    SelfBindSysEnum();
                    break;
                case DDLShowType.Self: ///  Enumerated type 
                    SelfBindSysEnum();
                    break;
                case DDLShowType.BindTable: /// 于Table Bind .
                    this.SelfBindTable();
                    break;
                case DDLShowType.Ens: ///  To the entity .
                    this.SelfBindEns();
                    break;
            }

            //if (this.SelfAddAllLocation == AddAllLocation.TopAndEnd)
            //    this.Items.Add(new ListItem("-= Whole =-", "all"));

            if (this.SelfDefaultVal != null && this.SelfDefaultVal.Length > 0)
                this.SetSelectItem(this.SelfDefaultVal);

            //			if (this.Enabled==false)		
            //				this.BackColor=Color.FromName("#E4E3E6");
        }
		#endregion   

        public void BindSQL(string sql, string val, string text, string selecVal)
        {
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            this.Items.Clear();
            foreach (DataRow dr in dt.Rows)
            {
               ListItem li= new ListItem();
                li.Value = dr[val].ToString(); // as string;
                li.Text = dr[text].ToString(); // as string;
                //if (li.Value==selecVal)
                //    li.Selected=true;
                this.Items.Add(li);
            }
            this.SetSelectItem(selecVal);
        }
        /// <summary>
        ///  Binding atPara.
        /// </summary>
        /// <param name="AtPara"></param>
        public void BindAtParas(string atParas)
        {
            string[] strs = atParas.Split('@');
            foreach (string str in strs)
            {
                if (string.IsNullOrEmpty(str) == true)
                    continue;
                string[] mystrs = str.Split('=');
                this.Items.Add(new ListItem(mystrs[1], mystrs[0]));
            }
        }
        public void Bind(EntitiesTree ens, string selecVal)
        {
            this.Items.Clear();
            foreach (EntityTree en in ens)
            {
                this.Items.Add(new ListItem(en.Name, en.No));
            }

            foreach (ListItem li in this.Items)
            {
                if (li.Value == selecVal)
                    li.Selected = true;
            }
        }
        /// <summary>
        ///  Binding datatable.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="val"></param>
        /// <param name="text"></param>
        public void Bind(DataTable dt, string val, string text)
        {
            if (this.Items.Count >= dt.Rows.Count)
            {
                bool isHave = false;
                foreach (ListItem li in this.Items)
                {
                    isHave = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[val].ToString() == li.Value)
                        {
                            isHave = true;
                            break;
                        }
                    }
                    //if (isHave==false)
                    //    li.Attributes["display"] = "none";

                    //else
                    //    li.Attributes["visibility"] = "false";

                    if (isHave)
                        li.Attributes["visibility"] = "true";
                    else
                        li.Attributes["visibility"] = "false";
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    this.Items.Add(new ListItem(dr[text].ToString(), dr[val].ToString()));
                }
            }
        }
		public void SetSelectItemByIndex(int index)
		{
			foreach(ListItem li in this.Items)
			{
				li.Selected=false;
			}
			int i=0 ;
			foreach(ListItem li in this.Items)
			{

				if (i==index)
				{
					li.Selected=true;
				}
				i++; 
			}

		}

		#region  Deal with BindKey
		/// <summary>
		/// -== Whole ==-  Display position .
		/// </summary>		
		private AddAllLocation _SelfAddAllLocation=AddAllLocation.None ;
		/// <summary>
		/// -== Whole ==-  Display position .
		/// </summary>		 
		public AddAllLocation SelfAddAllLocation
		{
			get
			{
				return _SelfAddAllLocation;
			}
			set
			{
				_SelfAddAllLocation=value;
			}
		}
		/// <summary>
		/// 要bind的key.
		///  There 3 Case .
		/// 1, Enumerated type .
		/// 2,Table Type of .
		/// 3, Entity type .
		///  Only for 2,3 Two types of SelfRefKey, SelfRefText. Makes sense .
		/// </summary>
		public string SelfBindKey
		{
			get
			{
				return (string)ViewState["SelfBindKey"];
			}
			set
			{
				ViewState["SelfBindKey"]=value;
			}
		}
		public string AppPath
		{
			get
			{
				return (string)ViewState["AppPath"];
			}
			set
			{
				ViewState["AppPath"]=value;
			}
		}
		/// <summary>
		/// 为attr ,  Properties set 
		/// </summary>
		private Entities HisFKEns=null;
		/// <summary>
		///  Defaults 
		/// </summary>
		public string SelfDefaultVal
		{
			get
			{
				return (string)ViewState["SelfDefaultVal"];
			}
			set
			{
				ViewState["SelfDefaultVal"]=value;
			}
		}
		/// <summary>
		///  Default Text
		/// </summary>
		public string SelfDefaultText
		{
			get
			{
				return (string)ViewState["SelfDefaultText"];
			}
			set
			{
				ViewState["SelfDefaultText"]=value;
			}
		}
		 
		/// <summary>
		///  Do you want to display  Bind  Value .
		/// </summary>
		public bool SelfIsShowVal
		{
			get
			{
				try
				{
					return (bool)ViewState["SelfIsShowVal"];
				}
				catch
				{
					return false;
				}			
			}
			set
			{
				ViewState["SelfIsShowVal"]=value;
			}
		}
		/// <summary>
		///  Uses DDL 于 Ens  Defined to help set 
		/// </summary>
        private void SelfBindEns()
        {
            if (this.SelfBindKey == "")
                throw new Exception("@ It is not set Key.");

            if (this.SelfAddAllLocation == AddAllLocation.Top || this.SelfAddAllLocation == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem("-= Whole =-", "all"));

            if (this.Enabled == true)
            {
                Entities ens = this.HisFKEns;
                ens.RetrieveAll();
               
                this.BindEntities(ens, this.SelfEnsRefKey, this.SelfEnsRefKeyText);
            }
            else
            {
                this.Items.Add(new ListItem(this.SelfDefaultText, this.SelfDefaultVal));
            }

            if (this.SelfAddAllLocation == AddAllLocation.End || this.SelfAddAllLocation == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem("-= Whole =-", "all"));
        }
		/// <summary>
		/// DDLDataHelp  Uses DDL Custom help set .
		/// </summary>
		private void SelfBindTable()
		{
			if (this.SelfBindKey=="")
				return ;
			 
			/*

			DDLDataHelp en = new DDLDataHelp(this.SelfBindKey);

			string sql = "SELECT TOP 100  "+en.NoCol+"  , "+en.NameCol+"  FROM  "+en.RefTable ;
			DataTable dt = DBAccess.RunSQLReturnTable(sql);

			string name, val;
			foreach(DataRow dr in dt.Rows)
			{
				if (this.SelfIsShowVal)
				{
					name=dr[0].ToString()+" "+dr[1].ToString() ; 
					val=dr[0].ToString().Trim() ;
				}
				else
				{
					name=dr[0].ToString().Trim()+" "+dr[1].ToString() ; 
					val=dr[0].ToString().Trim() ;
				}
				ListItem li = new ListItem(name,val);
				this.Items.Add(li);	
			}			
			///   It is not a requirement to determine double-click event .
			if (en.FK_TBDataHelp.Trim().Length==0)
				this.DealRightKey(en.FK_TBDataHelp) ; 
				*/
			 
		}
        private void DealRightKey(string TBDataHelpKey)
        {
            /*
            DataHelp en = new DataHelp(TBDataHelpKey) ; 
            string script ,  url ;
            string appPath =this.Page.Request.ApplicationPath;
			
            this.Font.Bold=false;
            this.BorderStyle=System.Web.UI.WebControls.BorderStyle.Groove;
            //this.Width=Unit.Pixel(150);
            this.ForeColor=Color.FromName("#006699");
					 
            url =appPath+"Comm/RefFunc/DataHelp.htm?"+appPath+en.FileUrl;
            script=" if ( event.button != 2)  return;  val=window.showModalDialog('"+url+"','','dialogHeight: "+en.Height+"px; dialogWidth: "+en.Width+"px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no'); if ( val==undefined) return ; "+this.ClientID+".value=val ; ";
            this.Attributes["onmousedown"]=script;
            //this.ToolTip=en.ToolTip;
            */

        }


        private void TBInit(object sender, System.EventArgs e)
        {
            this.SelfBind();
            return;
            /*  You can retain the following code . */
            //if (this.Enabled)
            //{
            //    //url =appPath+"Comm/DataHelp.htm?"+appPath+"Comm/HelperOfTB.aspx?EnsName="+this.EnsName+"&AttrKey="+this.AttrKey ;
            //    //script=" if ( event.button != 2)  return; str="+this.ClientID+".value;str= window.showModalDialog('"+url+"&Key=\'+str, '','dialogHeight: 500px; dialogWidth:850px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no'); if ( str==undefined) return ; "+this.ClientID+".value=str ; ";
            //    //	this.Attributes["onmousedown"]=script;
            //    //string appPath =this.Page.Request.ApplicationPath;
            //    //string url=appPath+"Comm/DataHelp.htm?"+appPath+"Comm/HelperOfDDL.aspx?EnsName="+attr.UIBindKey+"&RefKey="+attr.UIRefKeyValue+"&RefText="+attr.UIRefKeyText;
            //    //string script=" if ( event.button != 2 )  return; str="+this.ClientID+".DataTextField; str= window.showModalDialog('"+url+"&Key=\'+str , '','dialogHeight: 500px; dialogWidth:800px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no'); if ( str==undefined) return ;  "+this.ClientID+".DataTextField=str ; ";
            //    //string script=" if ( event.button != 2 )  return; window.showModalDialog('"+url+"' , '','dialogHeight: 500px; dialogWidth:800px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no'); SetDDLVal('"+this.ClientID+"',str ) ; ";

            //    if (this.SelfBindKey != null)
            //    {
            //        this.Attributes["onmousedown"] = " if ( event.button != 2 ) return; HalperOfDDL('" + this.AppPath + "','" + this.SelfBindKey + "','" + this.SelfEnsRefKey + "','" + this.SelfEnsRefKeyText + "','" + this.ClientID.ToString() + "' )";
            //    }
            //}
            //this.SelfBind();
        }
		#endregion		 

		/// <summary>
		/// new ddl
		/// </summary>
		/// <param name="attr"> Property </param>
		/// <param name="DefaultVal">DefaultVal</param>
		/// <param name="DefaultText">DefaultText</param>
		/// <param name="enable">enable</param>
		public DDL(Attr attr, string DefaultVal, string DefaultText, bool enable, string appPath)
		{
			this.AppPath =appPath;
			this.ID="DDL_"+attr.Key;
			this.BorderStyle=BorderStyle.None;
			this.CssClass="DDL"+WebUser.Style;

			this.SelfDefaultText = DefaultText;
			this.SelfDefaultVal = DefaultVal;

			this.Enabled =enable;
			this.SelfShowType = attr.UIDDLShowType;

			this.SelfBindKey=attr.UIBindKey;
			this.SelfEnsRefKey =attr.UIRefKeyValue;
			this.SelfEnsRefKeyText =attr.UIRefKeyText;
			this.HisFKEns = attr.HisFKEns;
			this.SelfIsShowVal = false; /// Not to show ID 

			//this.PreRender +=new System.EventHandler(this.DDLPreRender);
			this.Init += new System.EventHandler(this.TBInit);
		}
        public void LoadMapAttr(Attr attr)
        {
            this.ID = "DDL_" + attr.Key;
           // this.AppPath = BP.Sys.SystemConfig.AppName;
            this.BorderStyle = BorderStyle.None;

            this.SelfDefaultText = attr.UIRefKeyText;
            this.SelfDefaultVal = attr.UIRefKeyValue;

            this.Enabled = attr.UIIsReadonly;
            if (attr.MyDataType == DataType.AppInt)
                this.SelfShowType = DDLShowType.SysEnum;
            else
                this.SelfShowType = DDLShowType.Ens;

            this.HisFKEns = attr.HisFKEns;

            this.SelfBindKey = attr.UIBindKey;
            this.SelfEnsRefKey = attr.UIRefKeyValue;
            this.SelfEnsRefKeyText = attr.UIRefKeyText;
            this.SelfIsShowVal = false; /// Not to show ID 
            this.Init += new System.EventHandler(this.TBInit);
        }

       

		public DDL(Attr attr, string DefaultVal, string DefaultText, bool enable)
		{
			this.ID="DDL_"+attr.Key;
			this.AppPath =this.Page.Request.ApplicationPath;
			this.BorderStyle=BorderStyle.None;
			this.CssClass="DDL"+WebUser.Style;

			this.SelfDefaultText = DefaultText;
			this.SelfDefaultVal = DefaultVal;

			this.Enabled =enable;
			this.SelfShowType = attr.UIDDLShowType;

			this.SelfBindKey=attr.UIBindKey;
			this.SelfEnsRefKey =attr.UIRefKeyValue;
			this.SelfEnsRefKeyText =attr.UIRefKeyText;
			this.HisFKEns = attr.HisFKEns;			

			this.SelfIsShowVal = false; /// Not to show ID 
			//this.PreRender +=new System.EventHandler(this.DDLPreRender);
			this.Init += new System.EventHandler(this.TBInit);
		}
        public void LoadMapAttr(BP.Sys.MapAttr attr)
        {
            this.ID = "DDL_" + attr.KeyOfEn;
            // this.AppPath = BP.Sys.SystemConfig.AppName;
            this.BorderStyle = BorderStyle.None;

            this.SelfDefaultText = attr.UIRefKeyText;
            this.SelfDefaultVal = attr.UIRefKey;

            this.Enabled = attr.UIIsEnable;
            if (attr.MyDataType == DataType.AppInt)
            {
                this.SelfShowType = DDLShowType.SysEnum;
                //SysEnums ses = new SysEnums(attr.UIBindKey);
               // this.BindSysEnum(attr.UIBindKey);
            }
            else
            {
                this.SelfShowType = DDLShowType.Ens;
                this.HisFKEns = BP.En.ClassFactory.GetEns(attr.UIBindKey );

                //this.HisFKEns.RetrieveAll();
                //this.BindEntities(this.HisFKEns, "No", "Name", false);
            }


          //  SelfEnsRefKey
            this.SelfBindKey = attr.UIBindKey;
            this.SelfEnsRefKey = attr.UIRefKey;
            this.SelfEnsRefKeyText = attr.UIRefKeyText;
            this.SelfIsShowVal = false; /// Not to show ID 
                                        ///
            this.Init += new System.EventHandler(this.TBInit);
        }


        public DDL(Attr attr, string defSelectVal)
        {
            this.ID = "DDL_" + attr.Key;
            this.SelfShowType = attr.UIDDLShowType;
            this.SelfBindKey = attr.UIBindKey;
            this.SelfEnsRefKey = attr.UIRefKeyValue;
            this.SelfEnsRefKeyText = attr.UIRefKeyText;
            this.SelfDefaultVal = defSelectVal;
            this.HisFKEns = attr.HisFKEns;


            // this.SelfAddAllLocation = AddAllLocation.Top;

            if (attr.UIBindKey == "BP.Port.SJs")
            {

            }
            else
            {
                this.SelfAddAllLocation = AddAllLocation.Top;
            }
            this.SelfIsShowVal = false; /// Not to show ID 
        }
 
		public DDL()
		{			
			//this.PreRender +=new System.EventHandler(this.DDLPreRender);
			this.Init += new System.EventHandler(this.TBInit);
			//this.Load +=new System.EventHandler(this.TBPreRender);			 
		}
        public void Bind(EntitiesNoName ens, string seleVal)
        {
            this.BindEntitiesNoName(ens, seleVal);
        }
        public void Bind(XML.XmlEns xmls, string key, string name)
        {
            foreach (XML.XmlEn xml in xmls)
            {
                this.Items.Add(new ListItem(xml.GetValStringByKey(name), xml.GetValStringByKey(key)));
            }
        }
        public void Bind(Entities ens, string key, string name)
        {
            this.Items.Clear();
            foreach (Entity en in ens)
            {
                this.Items.Add(new ListItem(en.GetValStringByKey(name), en.GetValStringByKey(key)));
            }
        }

        protected override void OnPreRender(System.EventArgs e)
        {

            if (this.SelfAddAllLocation == AddAllLocation.None)
                return;

            if (this.Items.FindByValue("all") == null)
                return;

            if (this.SelfAddAllLocation == AddAllLocation.Top || this.SelfAddAllLocation == AddAllLocation.TopAndEnd)
            {
                ListItem li = new ListItem("-= Whole =-", "all");
                this.Items.Add(li);
            }

            if (this.SelfAddAllLocation == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem("-= Whole =-", "all"));
            base.OnPreRender(e);
        }


		#region  Handling user style 
		 
		public void Style3()
		{
			this.CssClass="DDL3";
			/*
			//this.BorderColor=Color.Transparent;
			this.BackColor=Color.FromName("#006699");
			this.ForeColor=Color.White;		
			*/	 
		}
		public void Style2()
		{
			this.CssClass="DDL2";
			/*
			this.BorderColor=System.Drawing.Color.Transparent;
			this.BackColor=Color.FromName("#DEBA84");
			this.ForeColor=Color.Black;
			*/
		}
		public void Style1()
		{ 
			this.CssClass="DDL1";
			//this.BorderColor=System.Drawing.Color.FromName("#DEBA84");
			//this.BackColor=Color.FromName("#DEBA84");
			//this.ForeColor=Color.Black;			 			 
		}
		#endregion 

		 
		 
		/// <summary>
		/// OID, No, Name 
		/// </summary>
		/// <param name="dt"></param>
		public void BindWithOIDNoNameCol(DataTable dt)
		{
			this.Items.Clear();
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li =new ListItem(dr["No"].ToString()+" "+dr["Name"].ToString(),dr["OID"].ToString()) ; 
				this.Items.Add(li);
			}
		}
		/// <summary>
		/// OID,   Name 
		/// </summary>
		/// <param name="dt"></param>
		public void BindWithOIDNameCol(DataTable dt)
		{
			this.Items.Clear();
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li =new ListItem( dr["Name"].ToString(),dr["OID"].ToString()) ; 
				this.Items.Add(li);
			}
		}
		/// <summary>
		/// OID,   Name 
		/// </summary>
		/// <param name="dt"></param>
		public void BindWithOIDNameCol(DataTable dt, string title, string selectVal)
		{
			this.Items.Clear();			
			this.Items.Add(new ListItem("-="+title+"( Whole )=-","all"));
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li =new ListItem( dr["Name"].ToString(),dr["OID"].ToString()) ; 
				this.Items.Add(li);
			}
			this.Items.Add(new ListItem("-="+title+"( Whole )=-","all"));
			foreach( ListItem li in this.Items )
			{
				if (li.Value==selectVal)
				{
					li.Selected=true;				
					break;
				}
			}
		}

		/// <summary>
		/// No ,Name  bind
		/// </summary>
		/// <param name="dt"></param>
		public void BindWithNoNameCol(DataTable dt, string title)
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("-="+title+"( Whole )=-","all"));
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li =new ListItem(dr["Name"].ToString().Trim(),dr["No"].ToString().Trim()); 
				this.Items.Add(li);
			}
			this.Items.Add(new ListItem("-="+title+"( Whole )=-","all"));
		}
		/// <summary>
		/// No ,Name bind
		/// </summary>
		/// <param name="dt"></param>
		public void BindWithNoNameCol(DataTable dt )
		{
			this.Items.Clear();			
			foreach (DataRow dr in dt.Rows)
			{
				ListItem li =new ListItem(dr["Name"].ToString().Trim(),dr["No"].ToString().Trim()); 
				this.Items.Add(li);
			}			
		}
		public void BindWithNoNameCol(DataTable dt, string title , string selectNo)
		{
			this.Items.Clear();
			BindWithNoNameCol(dt,title);
			foreach(ListItem li in this.Items)
			{
				if (li.Value.Equals(selectNo))
				{
					li.Selected=true;
					break;
				}
			}
		}
		public void BindMonth(int month)
		{
			this.Items.Clear();
			int i = 0 ;
			string str="";
			while ( i < 12)
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;
				ListItem li =new ListItem( i.ToString()+"月", str ); 

				if (i==month)
					li.Selected=true;

				this.Items.Add(li);
			}
		}
		public void Bindhh(int hh)
		{
			this.Items.Clear();
			int i = 7 ;
			string str="";
			while ( i < 20 )
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;
				ListItem li =new ListItem( i.ToString()+"h", str ); 

				if (i==hh)
					li.Selected=true;
				this.Items.Add(li);
			}
		}
        public void BindNumFromTo(int from ,int to )
        {
            this.Items.Clear();
            int i = from;
            string str = "";
            while (i <= to )
            {
                i++;
                str = i.ToString();
                if (str.Length == 1)
                    str = "0" + str;

                ListItem li = new ListItem(i.ToString() , str);
                this.Items.Add(li);
            }
        }
		public void Bindmm()
		{
			this.Items.Clear();
			int i = 0 ;
			string str="";
			while ( i < 59)
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;
				ListItem li =new ListItem( i.ToString()+"min", str ); 
				 
				this.Items.Add(li);
			}
		}
		public void BindQuector(int selectVal)
		{
			this.Items.Clear();
		 
			this.Items.Add(new ListItem(":00","00"));  
			this.Items.Add( new ListItem(":15","15"));  
			this.Items.Add( new ListItem(":30","30"));  
			this.Items.Add( new ListItem(":45","45")); 

			this.SetSelectItem( selectVal );
		}
		public void BindWeek(int selectVal)
		{
			this.Items.Clear();
		 
			this.Items.Add(new ListItem(" Sun. ","0"));  
			this.Items.Add(new ListItem(" On Monday ","1"));  
			this.Items.Add(new ListItem(" On Tuesday ","2"));  
			this.Items.Add(new ListItem(" On Wednesday ","3"));  
			this.Items.Add(new ListItem(" Thursday ","4"));  
			this.Items.Add(new ListItem(" Fri. ","5"));  
			this.Items.Add(new ListItem(" On Saturday ","6"));  

			this.SetSelectItem( selectVal );
		}
		public void BindDay( int day )
		{
			this.Items.Clear();
			int i = 0 ;
			string str="";
			
			while ( i < 31)
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;
				
				ListItem li =new ListItem( i.ToString()+"日", str ); 
				if (i==day)
					li.Selected=true;
				this.Items.Add(li);
			}
		}
		/// <summary>
		/// bind month , To the current month .
		/// </summary>
		public void BindMonthToThisMonth()
		{
			this.Items.Clear();
			int m = DateTime.Now.Month;
			int i = 0 ;
			while ( i < 12)
			{
				i++;
				ListItem li =new ListItem( i.ToString()+"月", i.ToString() ); 
				if (i==m) 
				{
					this.Items.Add(li);
					li.Selected=true;
					break;
				}				
				this.Items.Add(li);
			}
			
		}
		/// <summary>
		/// bind day .
		/// </summary>
		public void BindAppDaySelectedToday()
		{
			this.Items.Clear();
			int m = DateTime.Now.Day ;
			int i = 0 ;
			while ( i < 31)
			{
				i++;
				ListItem li =new ListItem( i.ToString()+"日", i.ToString() ); 
				if (i==m) 
				{
					this.Items.Add(li);
					li.Selected=true;
					continue;
				}				
				this.Items.Add(li);
			}
			
		}
		/// <summary>
		///  Quarter 
		/// </summary>
		public void BindQuarter()
		{
			this.Items.Clear();
			this.Items.Add( new ListItem(" First quarter ","1"));
			this.Items.Add( new ListItem(" Second quarter ","2"));
			this.Items.Add( new ListItem(" The third quarter ","3"));
			this.Items.Add( new ListItem(" Fourth quarter ","4"));

			int month = DateTime.Now.Month ;
			if (month < 4)
				this.SetSelectItem("1");
			else if (month >=4 && month < 7)
			{
				this.SetSelectItem("2");
			}
			else if (month >=8 && month < 10)
			{
				this.SetSelectItem("3");
			}
			else
			{
				this.SetSelectItem("4");
			}			 
		}
        public void BindStrs(string[] strs )
        {
            int i = -1;
            foreach (string s in strs)
            {
                i++;
                ListItem li = new ListItem(s, i.ToString());
                this.Items.Add(li);
            }
        }
		public void BindAppDay()
		{
			int i = 1;
			while ( i < 31)
			{
				i++;
				ListItem li =new ListItem( i.ToString()+"日", i.ToString() );				 
				this.Items.Add(li);
			}
		}
		public void BindAppDay(string selectedDay)
		{
			int i =1;
			int m = int.Parse(selectedDay);
			string str="";
			while ( i < 31)
			{
				i++;
				
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;

				ListItem li =new ListItem( i.ToString()+"日", str ); 
				if (i==m) 
				{
					this.Items.Add(li);
					continue;
				}
				this.Items.Add(li);
			}
		}

		public void BindMonthSelectCurrentMonth()
		{
			this.Items.Clear();
			int m = DateTime.Now.Month ;
			int i = 0 ;
			
			string str="";
			while ( i < 12)
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;

				ListItem li =new ListItem( i.ToString()+"月", str ); 
				if (i==m) 
				{
					li.Selected=true;
				}
				this.Items.Add(li);
			}
		}
		/// <summary>
		///  Month   1 , 2, 
		/// </summary>
		/// <param name="selectM"></param>
		public void BindMonth( string selectM)
		{
			this.Items.Clear();
			int i = 0 ;
			string str="";
	 
			int m = int.Parse(selectM);
			while ( i < 12)
			{
				i++;
				str=i.ToString();
				if (str.Length==1)
					str="0"+str;
				
				ListItem li =new ListItem( i.ToString()+"月", str ); 
				if (i==m)
					li.Selected=true;

				this.Items.Add(li);
			}
		}

        /// <summary>
        /// bind  Nearly three years ,  The default value is the current year 
        /// </summary>
        public void BindYearMonth(int nearM)
        {
            DateTime dt = System.DateTime.Now;
            for (int i = 0; i <= nearM; i++)
            {
                this.Items.Add(new ListItem(dt.ToString("yyyy-MM"), dt.ToString("yyyy-MM")));
               dt= dt.AddMonths(-1);
                i++;
            }
        }
		/// <summary>
		/// bind  Nearly three years ,  The default value is the current year 
		/// </summary>
        public void BindYear()
        {
            this.Items.Clear();
            int i1 = System.DateTime.Now.Year;
            int i2 = i1 - 1;
            int i3 = i1 - 2;
            int i4 = i1 - 3;
            int i5 = i1 - 4;

            this.Items.Add(new ListItem(i1.ToString() + "", i1.ToString()));
            this.Items.Add(new ListItem(i2.ToString() + "", i2.ToString()));
            this.Items.Add(new ListItem(i3.ToString() + "", i3.ToString()));
            this.Items.Add(new ListItem(i4.ToString() + "", i4.ToString()));
            this.Items.Add(new ListItem(i5.ToString() + "", i5.ToString()));
        }
		
		/// <summary>
		/// bind  Nearly three years .
		/// </summary>
		/// <param name="selectYear"> Year selection </param>
        public void BindYear(int selectYear)
        {
            this.BindYear();
            foreach (ListItem li in this.Items)
            {
                if (li.Value.Equals(selectYear.ToString()))
                {
                    li.Selected = true;
                }
            }
        }
		/// <summary>
		/// bind   Nearly two years .
		/// </summary>
		public void BindYearNear2()
		{			 
			this.Items.Clear();
		}
        public void BindAppPrecent(int selectVal)
        {
            int i = 0;
            while (true)
            {
                this.Items.Add(new ListItem(i.ToString() + "%", i.ToString()));
                i++;
                if (i == 100)
                    break;
            }
        }
		public int SelectedItemIntVal
		{
			get
			{
				return int.Parse(this.SelectedItem.Value);
			}
		}
        public string SelectedItemStringVal
        {
            get
            {
                return this.SelectedItem == null ? null : this.SelectedItem.Value;
            }
        }

		#region bind  Entity .

		public void BindEntities(Entities cateEns, string cateKey, string cateText,Entities subEns, string subKey, string subText , string refKey )
		{
			this.Items.Clear();
			foreach(Entity en in cateEns)
			{
				ListItem li=new ListItem( "==="+en.GetValStringByKey(cateText)+"=====" ,  en.GetValStringByKey(cateKey)  );
				li.Attributes["background-color"]="Green";
				li.Attributes["color"]="Green";

				this.Items.Add(li);
				foreach(Entity suben in subEns)
				{
					if (suben.GetValStringByKey(refKey)==en.GetValStringByKey(cateKey))
					{
						this.Items.Add(new ListItem( "|-"+suben.GetValStringByKey(subText) ,  suben.GetValStringByKey(subKey)  )) ;
					}
				}
			}
		}

        /// <summary>
        ///  There is bound to generate a drop-down menu tree structure 
        /// </summary>
        /// <param name="dtNodeSets"> Table menu record data resides </param>
        /// <param name="strParentColumn"> Fields table for the parent record mark </param>
        /// <param name="strRootValue"> Parent record values in the first record ( Usually designed to 0 Or -1 Or Null) Is used to indicate no parent record </param>
        /// <param name="strIndexColumn"> Index field , Is placed DropDownList的Value Inside the field </param>
        /// <param name="strTextColumn"> Display a text field , Is placed DropDownList的Text Inside the field </param>
        /// <param name="drpBind"> Need to bind DropDownList</param>
        /// <param name="i"> Used to control the amount of value retracted , Please enter -1</param> 
        public static void MakeTree(DataTable dtNodeSets, string strParentColumn, string strRootValue, string strIndexColumn, string strTextColumn, DropDownList drpBind, int i)
        {
            // Each layer down , Indented one more unit  
            i++;

            DataView dvNodeSets = new DataView(dtNodeSets);
            dvNodeSets.RowFilter = strParentColumn + "=" + strRootValue;

            string strPading = ""; // Retracted into character  

            // By i To control shrink into characters in length , I set here is a full-width space  
            for (int j = 0; j < i; j++)
                strPading += "　";// If you want to increase the length of retraction , Into two full-width space on it  

            foreach (DataRowView drv in dvNodeSets)
            {
                TreeNode tnNode = new TreeNode();
                ListItem li = new ListItem(strPading + "├" + drv[strTextColumn].ToString(), drv[strIndexColumn].ToString());
                drpBind.Items.Add(li);
                MakeTree(dtNodeSets, strParentColumn, drv[strIndexColumn].ToString(), strIndexColumn, strTextColumn, drpBind, i);
            }
            // End recursion , To return to the previous , Therefore, the amount of shrinkage is reduced by one unit  
            i--;
        }


        public void BindEntities(Entities ens, string refKey, string refText)
        {

#warning  Went in 2009-03-09
            this.Items.Clear();

            if (ens.Count == 0)
                ens.RetrieveAll();

#warning  Here an error .

            //EntitiesTree treeEns = ens as EntitiesTree;
            //if (treeEns != null)
            //{
            //    DataTable dt = ens.ToDataTableField();
            //    MakeTree(dt, "ParentNo", "0", "No", "Name", this, -1);
            //    return;
            //}

            //EntitiesSimpleTree treeSimpEns = ens as EntitiesSimpleTree;
            //if (treeSimpEns != null)
            //{
            //    DataTable dt = ens.ToDataTableField();
            //    MakeTree(dt, "ParentNo", "0", "No", "Name", this, -1);
            //    return;
            //}

            foreach (Entity en in ens)
            {
                this.Items.Add(new ListItem(en.GetValStringByKey(refText), en.GetValStringByKey(refKey)));
            }
        }
		public void BindEntities( EntitiesOIDName  ens)
		{
			this.Items.Clear();
			foreach(EntityOIDName en in ens)
			{
				this.Items.Add(new ListItem( en.Name,en.OID.ToString() )) ; 
			}
		}
        public void BindEntities(EntitiesNoName ens, AddAllLocation where)
        {
            this.Items.Clear();
            if (where== AddAllLocation.Top || where== AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem(" Whole ", "all"));

            foreach (EntityNoName en in ens)
            {
                this.Items.Add(new ListItem(en.Name, en.No));
            }

            if (where == AddAllLocation.End || where == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem(" Whole ", "all"));
        }
        public void BindEntities(EntitiesOIDName ens, AddAllLocation where)
        {
            this.Items.Clear();
            if (where == AddAllLocation.Top || where == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem(" Whole ", "all"));

            foreach (EntityOIDName en in ens)
                this.Items.Add(new ListItem(en.Name, en.OID.ToString()));

            if (where == AddAllLocation.End || where == AddAllLocation.TopAndEnd)
                this.Items.Add(new ListItem(" Whole ", "all"));
        }
		public void BindEntities(EntitiesNoName ens)
		{
			this.Items.Clear();
            foreach (EntityNoName en in ens)
            {
                this.Items.Add(new ListItem(en.Name, en.No));
            }
		}
		public void BindEntitiesNoName( EntitiesNoName ens,bool isShowKey)
		{
			this.Items.Clear();
			foreach(EntityNoName en in ens)
			{
				ListItem li = new ListItem() ;
				if (isShowKey)
				{
					li.Value = en.No ;
					li.Text=en.No+" "+en.Name;					
				}
				else
				{
					li.Value=en.No ;
					li.Text=en.Name;	
				}
				this.Items.Add( li); 
			}
		}
		public void BindEntitiesNoNameWithSelectAll(EntitiesNoName ens ,bool IsShowKey)
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("-= Please select =-","all")) ; 
			
			foreach(EntityNoName en in ens)
			{
				if (IsShowKey)
					this.Items.Add(new ListItem(en.No+en.Name,en.No)) ; 
				else
					this.Items.Add(new ListItem(en.Name,en.No)) ; 
			}
		}
		public void BindEntitiesNoName(EntitiesNoName ens, string selecVal)
		{
			this.Items.Clear();
			foreach(EntityNoName en in ens)
			{
				this.Items.Add(new ListItem(en.Name,en.No)) ; 
			}

			foreach (ListItem li in this.Items)
			{
				if (li.Value==selecVal ) 
					li.Selected=true;
			}
		}
		#endregion

		#region app
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ens"></param>
		public void BindEntitiesOIDName(EntitiesOIDName ens)
		{
			this.Items.Clear();
			foreach(EntityOIDName en in ens)
			{
				this.Items.Add( new ListItem(en.Name,en.OID.ToString()) ) ;
			}
		}
		public void BindEntitiesOIDName(EntitiesOIDName ens, int seleOID)
		{
			this.Items.Clear();
			 this.BindEntitiesOIDName(ens);
			foreach(ListItem li in this.Items)
			{
				if (li.Value==seleOID.ToString())
					li.Selected=true;
			}
		}

		public void BindEntitiesNoName(EntitiesNoName ens)
		{
			this.Items.Clear();
			foreach(EntityNoName en in ens)
			{
				this.Items.Add( new ListItem(en.Name, en.No ) ) ;
			}
		}

	 
	 
		public void BindAppYesOrNo(int selectedVal)
		{
			this.Items.Clear();

			this.Items.Add(new ListItem("Yes", "1") );
			this.Items.Add(new ListItem("No", "0") );

			foreach(ListItem li in this.Items)
			{
				if (li.Value.Equals(selectedVal.ToString()))
				{
					li.Selected=true;
					break;
				}
			}		    
		}	
		　
		 
		 
		public void SetSelectItem(int val)
		{
			this.SetSelectItem(val.ToString() ) ;
		}

        public static bool SetSelectItem( DropDownList ddl, string val)
        {
            try
            {
                foreach (ListItem li in ddl.Items)
                {
                    li.Selected = false;
                }

                foreach (ListItem li in ddl.Items)
                {

                    if (li.Value == val)
                    {
                        li.Selected = true;
                        return true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }

            }
            catch
            {

            }
            return false;
        }
        public void SetSelectItem(string val, Attr attr)
        {
            if (attr.UIBindKey == "BP.Port.Depts" )
            {
                if (val.Contains(WebUser.FK_Dept) == false)
                    return;

                this.Items.Clear();
                BP.Port.Dept detps = new BP.Port.Dept(val);

                ListItem li1 = new ListItem();
                li1.Text = detps.GetValStrByKey(attr.UIRefKeyText);
                li1.Value = detps.GetValStrByKey(attr.UIRefKeyValue);
                this.Items.Add(li1);
                return;
            }

            if ( attr.UIBindKey == "BP.Port.Units")
            {
                //if (val.Contains(WebUser.FK_Unit) == false)
                //    return;

                //this.Items.Clear();
                //BP.Port.Unit units = new BP.Port.Unit(val);

                //ListItem li1 = new ListItem();
                //li1.Text = units.GetValStrByKey(attr.UIRefKeyText);
                //li1.Value = units.GetValStrByKey(attr.UIRefKeyValue);
                //this.Items.Add(li1);
                return;
            }

            //this.SetSelectItem(val);
            //return;

            if (this.SetSelectItem(val))
                return;

            return;
           
//                Entity en = attr.HisFKEn; // ClassFactory.GetEn(attr.UIBindKey);
//                en.PKVal = val;
//                en.Retrieve();
          

//#warning edit: 2008-06-01  en.RetrieveFromDBSources();


//            ListItem li = new ListItem();
//            li.Text = en.GetValStrByKey(attr.UIRefKeyText);
//            li.Value = en.GetValStrByKey(attr.UIRefKeyValue);

//            if (this.Items.Contains(li))
//            {
//                this.SetSelectItem(val);
//                return;
//            }



//            ListItem liall = this.GetItemByText(" Please use more ...");
//            ListItem myall = this.GetItemByVal("all");
//            if (myall != null)
//            {
//                this.Items.Clear();
//                this.Items.Add(li);
//                this.Items.Add(myall);

//                // this.Items.Remove(liall);
//            }

//            //  this.Items.Add(li);
//            this.SetSelectItem(val);
        }
		/// <summary>
		///  Set selected value 
		/// </summary>
		/// <param name="val"></param>
        public bool SetSelectItem(string val)
        {
            if (val == null)
                return false;

            foreach (ListItem li in this.Items)
                li.Selected = false;

            foreach (ListItem li in this.Items)
            {
                if (li.Value == val)
                {
                    li.Selected = true;
                    return true;
                }
                else
                {
                    li.Selected = false;
                }
            }
            return false;
        }
        public ListItem GetItemByVal(string val)
        {
            foreach (ListItem li in this.Items)
            {
                if (li.Value == val)
                    return li;
            }
            return null;
        }
        public ListItem GetItemByText(string text)
        {
            foreach (ListItem li in this.Items)
            {
                if (li.Text == text)
                    return li;
            }
            return null;
        }
		public void ReomveItem(string val)
		{
			//int  i = -1 ;
			for(int ii = 0 ; ii < this.Items.Count;  ii++ )
			{
				if ( this.Items[ii].Value==val )
				{
					this.Items.RemoveAt(ii);
					break;
				}	 
			}
		}
		/// <summary>
		/// Number of violations 
		/// </summary>
		public void BindAppWGCS(int selected )
		{
			this.Items.Clear();
			for(int i = 0 ; i <=12 ; i ++)
			{
				ListItem li = new ListItem(i.ToString(), i.ToString());
				if (i ==selected )
					li.Selected=true;				
				this.Items.Add(li);
			}			
		}
		/// <summary>
		///  Descending / Ascending 
		/// </summary>
		public void BindAppDESCandASC()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem(" Descending ","DESC"));
			this.Items.Add(new ListItem(" Ascending ","ASC"));
		}
	 
		#endregion

	}	 
}
