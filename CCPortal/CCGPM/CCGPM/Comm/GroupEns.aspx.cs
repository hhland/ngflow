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
using BP.DA;
using BP.En;
using BP.Web;
using BP.Web.Controls;
using BP.Sys; 

namespace BP.Web.Comm
{
	/// <summary>
	/// SimplyNoName 的摘要说明。
	/// </summary>
    public partial class GroupEns : BP.Web.WebPage
	{
		public bool IsReadonly
		{
			get
			{
				string i= this.Request.QueryString["IsReadonly"];
				if (i=="1")
					return true;
				else
					return false;
			}
		}
		public bool IsShowSum
		{
			get
			{
				string i= this.Request.QueryString["IsShowSum"];
				if (i=="1")
					return true;
				else
					return false;
			}
		}
		 
		public ToolbarDDL DDL_Page
		{
			get
			{
				return this.BPToolBar1.GetDDLByKey("DDL_Page");
			}
		}
	 
		public BP.Web.Controls.ToolbarLab Lab_Result
		{
			get
			{
				return  this.BPToolBar1.GetLabByKey("Lab_Result");
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{

			UAC uac = this.HisEn.HisUAC;
			if (uac.IsView==false)
				throw new Exception("您没有查看["+this.HisEn.EnDesc+"]数据的权限.");

			if (this.IsReadonly)
			{
				uac.IsDelete=false;
				uac.IsInsert=false;
				uac.IsUpdate=false;
			}

			if (this.IsPostBack==false)
			{ 
				try 
				{
					#region 设置tool bar 1 的contral

					#region 判断权限
					if (uac.IsView==false)
						throw new Exception("@对不起，您没有查看的权限！");
					#endregion

					Map map = this.HisEn.EnMap;
					string reAttrs=this.Request.QueryString["Attrs"];
					foreach(Attr attr in map.Attrs)
					{
						if (attr.UIContralType==UIContralType.DDL )
						{
							ListItem li = new ListItem(attr.Desc,attr.Key);
							if (reAttrs!=null)
							{
								if ( reAttrs.IndexOf(attr.Key)!=-1)
								{
									li.Selected=true;
								}
							}
							this.CheckBoxList1.Items.Add( li );
						}
					}

					if (this.CheckBoxList1.Items.Count==0)
						throw new Exception (map.EnDesc+"没有外键条件，不适合做分组查询。");

					if (this.CheckBoxList1.Items.Count==1)
					{
						this.CheckBoxList1.Items[0].Selected=true;
					}

					if (this.CheckBoxList1.Items.Count>=2)
					{
						this.CheckBoxList1.Items[0].Selected=true;
						this.CheckBoxList1.Items[1].Selected=true;
					}

					foreach(Attr attr in map.Attrs)
					{
						
						if (attr.IsPK)
							continue;

						if (attr.UIContralType==UIContralType.TB==false)
							continue;

						if (attr.MyFieldType==FieldType.FK
							)
							continue;
						if (attr.MyFieldType==FieldType.Enum )
							continue;

						if (attr.MyFieldType==FieldType.Enum )
							continue;

						if (attr.Key=="OID" || attr.Key=="WorkID"  || attr.Key=="MID"  )
							continue;


						switch(attr.MyDataType)
						{
							case DataType.AppDouble:
							case DataType.AppFloat:
							case DataType.AppInt:
							case DataType.AppMoney:
							case DataType.AppRate:
								this.DDL_GroupField.Items.Add(new ListItem(attr.Desc,attr.Key ));
								break;
							default:
								break;
						}
					}
					//this.DDL_GroupField.Items.Add(new ListItem("个数","COUNT(*)"));
					this.DDL_GroupWay.Items.Add(new ListItem("求和","0"));
					this.DDL_GroupWay.Items.Add(new ListItem("求平均","1"));

					this.DDL_Order.Items.Add(new ListItem("降序","0"));
					this.DDL_Order.Items.Add(new ListItem("升序","1"));

					if (this.Request.QueryString["OperateCol"]!=null)
					{
						string[] strs=this.Request.QueryString["OperateCol"].Split('_');
						this.DDL_GroupField.SetSelectItem(strs[0]);
						this.DDL_GroupWay.SetSelectItem(strs[1]);
					}
					#endregion 
			 
					this.BPToolBar1.InitByMapV2(this.HisEn.EnMap,1);
					//this.BPToolBar1.InitByMapVGroup(this.HisEn.EnMap);
					this.BPToolBar1.AddSpt("spt1");					
					this.BPToolBar1.AddBtn(NamesOfBtn.Excel);
					this.BPToolBar1.AddBtn(NamesOfBtn.Help);
					 
					#region 设置 DG 里面的内容
					this.SetDGData();
					#endregion
				}
				catch(Exception ex)
				{
					this.HisEns.DoDBCheck(DBCheckLevel.High);
					throw new Exception("@装载出现错误:"+ex.Message);
					//this.ResponseWriteRedMsg("@装载出现错误:"+ex.Message);
					//return;
				}
			}
			 
			this.BPToolBar1.ButtonClick += new System.EventHandler(this.BPToolBar1_ButtonClick);

			string lab=SystemConfig.GetConfigXmlEns("GroupEns",this.EnsName);
			if (lab==null)
				lab=this.HisEn.EnMap.EnDesc;

			this.Label1.Controls.Add(this.GenerLabel("<img src='../Images/Btn/DataGroup.gif' border=0  />"+lab )); 
			//this.Label1.Controls.Add(this.GenerLabel("<img src='../Images/Btn/DataGroup.gif' border=0  />"+this.HisEn.EnDesc+"<img src='../Images/Btn/Table.gif' border=0  /><a href='UIEns.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'>查询</a><img src='../Images/Btn/Table.gif' border=0  /><a href='UIEnsCols.aspx?EnsName="+this.HisEns.ToString()+"&IsReadonly=1'>选择列查询</a>")); 
			//this.Label1.Controls.Add(this.GenerLabel("<img src='../Images/Btn/DataGroup.gif' border=0  />"+this.HisEn.EnDesc+"<img src='../Images/Btn/Table.gif' border=0  /><a href='UIEns.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'>查询</a><img src='../Images/Btn/Table.gif' border=0  /><a href='UIEnsCols.aspx?EnsName="+this.HisEns.ToString()+"&Readonly=1'>选择列查询</a>")); 
		}

		#region 方法 
		
		public void SetDGData()
		{
			try
			{
				Map map = this.HisEn.EnMap;
				Attrs attrs = new Attrs();
				foreach(ListItem li in this.CheckBoxList1.Items)
				{
					if (li.Selected)
						attrs.Add( map.GetAttrByKey( li.Value));
				}

				if (attrs.Count==0)
				{
					this.UCSys1.Add("<br><br><br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='../Images/Pub/warning.gif' /><b><font color=red>分组条件不能为空。</font></b>");
					return;
				}

				Entities ens = this.HisEns;
				QueryObject qo =this.BPToolBar1.InitTableByEnsV2(ens,ens.GetNewEntity,10000,1);
				Attr attr = new Attr();
				if (this.DDL_GroupField.SelectedItemStringVal=="COUNT(*)")
				{
					attr.Key="MyNum";
					attr.Desc="个数";
					attr.Field="COUNT(*)";
				}
				else
				{
					attr =map.GetAttrByKey(this.DDL_GroupField.SelectedItemStringVal);
				}

				DataTable dt= qo.DoGroupReturnTable(this.HisEn,attrs, attr, (GroupWay)this.DDL_GroupWay.SelectedItemIntVal, (OrderWay)this.DDL_Order.SelectedItemIntVal);
			
				// 动态的构造map.
				Map tmpMap = new Map();
				tmpMap.Attrs = attrs;
				try
				{
					Attr attr1 =map.GetAttrByKey( this.DDL_GroupField.SelectedItemStringVal);
					attr1.UIVisible=true;
					tmpMap.Attrs.Add( attr1  );
				}
				catch
				{
					Attr attr6 = new Attr();
					attr6.Key="MyNum";
					attr6.Desc="个数";
					attr6.Field="MyNum";
					tmpMap.Attrs.Add( attr6 );
				}

				#region 生成要查询条件
                string url = "ContrastDtl.aspx?EnsName=" + this.EnsName;
				//Map map = ens.GetNewEntity;
				foreach(Attr attrS in map.SearchAttrs )
				{
					if (attrS.MyFieldType==FieldType.RefText)
						continue;
					//ToolbarDDL ddl = (ToolbarDDL)ctl;
					url+="&"+attrS.Key+"="+this.BPToolBar1.GetDDLByKey( "DDL_"+attrS.Key).SelectedItemStringVal;
				}
				#endregion 
				 
				this.UCSys1.ShowTableGroupEns( dt, tmpMap,int.Parse(this.TB_Top.Text ) , url, this.CheckBox1.Checked);
				dt.Dispose();
				/*
				//string showtype=this.BPToolBar1.GetDDLByKey("DDL_ShowType").SelectedItemStringVal;
				switch(showtype)
				{
					case "rpt":
						this.UCSys1.ShowTable(this.Table,tmpMap);
						return;
//					case "a":
//						this.UCGraphics1.BindHistogram(this.Table,tmpMap);
//						return;
//					case "b":
//						this.UCGraphics1.ShowTable(this.Table,tmpMap);
//						return;
//					case "c":
//						this.UCGraphics1.ShowTable(this.Table,tmpMap);
//						return;
					default:
						throw new Exception("sdfdfs");
				}
				*/
			}
			catch(Exception ex)
			{
				this.ResponseWriteRedMsg(ex);
			}
		}
		  
	   
		 
		 

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);			
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{   

		}
		#endregion
	 
		
		#endregion

		private void BPToolBar1_ButtonClick(object sender, System.EventArgs e)
		{
			ToolbarBtn btn = (ToolbarBtn)sender;
			switch(btn.ID)
			{
				case NamesOfBtn.Save:
					GroupEnsTemplates rts = new GroupEnsTemplates();
					GroupEnsTemplate rt = new GroupEnsTemplate();
                    rt.EnsName = this.EnsName;
					//rt.Name=""
					string name="";
					//string opercol="";
					string attrs="";
					foreach(ListItem li in  CheckBoxList1.Items)
					{
						if (li.Selected)
						{
							attrs+="@"+li.Value;
							name+=li.Text+"_";
						}
					}
					name=this.HisEn.EnDesc+name.Substring(0,name.Length-1);
                    if (rt.Search(WebUser.No, this.EnsName, attrs) >= 1)
					{
						this.InvokeEnManager(rts.ToString(),rt.OID.ToString(),true);
						return;
					}
					rt.Name=name;
					rt.Attrs=attrs;
					rt.OperateCol=this.DDL_GroupField.SelectedItemStringVal+"@"+this.DDL_GroupWay.SelectedItemStringVal;
					rt.Rec=WebUser.No;
					rt.EnName=this.EnsName;
					rt.EnName=this.HisEn.EnMap.EnDesc;
					rt.Save();
					this.InvokeEnManager(rts.ToString(),rt.OID.ToString(),true);
					//	this.ResponseWriteBlueMsg("当前的模板已经加入了自定义报表队列，点击这里<a href');\"编辑自己定义报表</a>");
					break;
				case NamesOfBtn.Help:
					this.Helper();
					break;
				case NamesOfBtn.Excel:
					this.Helper();
					break;
			}
			this.SetDGData();
			 
		}

		protected void CheckBoxList1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			return;
			//this.SetDGData();
		}

		 
		 
	}
}
