using System;
using System.Web.UI.WebControls;
using System.Data ; 
using BP.DA;
using BP.En;
using System.ComponentModel;

namespace BP.Web.Controls
{
	/// <summary>
	/// BPListBox  The summary .
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Web.UI.WebControls.ListBox))]
	public class LB:System.Web.UI.WebControls.ListBox
	{
		private void LBPreRender( object sender, System.EventArgs e )
		{

		}
		public LB( Attr attr )
		{
//			this.MaxLength =attr.MaxLength;
//			//this.Width = Unit.Pixel(attr.UIWidth ); 
//			this.DefaultWith = attr.UIWidth;
//			 
//			this.ReadOnly = attr.UIIsReadonly ;
//			this.ShowType=attr.UITBShowType ;
			this.Attributes["size"]=attr.UIWidth.ToString();
		
			this.Visible =attr.UIVisible ;			 
//			this.DataHelpKey=attr.UIBindKey ;
//			this.ShowType = attr.UITBShowType ;
//			this.DataHelpKey = attr.UIBindKey;
		   
			this.Style.Clear();
			//this.Style.Add("width",attr.UIWidth.ToString()+"px") ;			
			  
			this.CssClass="DGLB"+WebUser.Style;
			this.PreRender += new System.EventHandler(this.LBPreRender);
		}
		public LB()
		{
			this.CssClass="LB"+WebUser.Style;
			//
			// TODO:  Add constructor logic here 
			//
		}
		/// <summary>
		/// OID , Name . 
		/// </summary>
		/// <param name="dt"></param>
		public void BindByTable(DataTable dt )
		{
			foreach (DataRow dr in dt.Rows)
			{
				//ListItem li = new ListItem();
				this.Items.Add(new ListItem( dr["Name"].ToString(),dr["OID"].ToString()));
			}
		}
		public void BindByTableNoName(DataTable dt )
		{
			foreach (DataRow dr in dt.Rows)
			{
				//ListItem li = new ListItem();
				this.Items.Add(new ListItem( dr["Name"].ToString(),dr["No"].ToString()));
			}
		}
		public void BindAppTaxpayerTax(string taxpayerNo)
		{
			this.Items.Clear();
			string sql = " SELECT DISTINCT  TaxTypeNo, TaxTypeName as Name, TaxTypeNo as No FROM V_IncMapTax WHERE TaxpayerNo='"+taxpayerNo+"'";
			DataTable dt = DA.DBAccess.RunSQLReturnTable(sql);
			this.BindByTableNoName(dt);
		}
		/// <summary>
		///  Scope of the accounting period set 
		/// Evaluate AND Check
		/// </summary>
		/// <param name="type">Evaluate / Check</param>
		public void Bind_AppPeriodScope( string type )
		{
			this.Items.Clear();
			string sql="select * from B_PeriodScope WHERE type='"+type+"'";
			DataTable dt = DBAccess.RunSQLReturnTable(sql);
			foreach (DataRow dr in dt.Rows)
			{
				this.Items.Add( new ListItem(dr["FromYear"].ToString()+"年"+dr["ToMonth"].ToString()+"月 -- "+dr["ToYear"].ToString()+"年"+dr["ToMonth"].ToString()+"月;  Set Date :"+dr["CreateDate"].ToString(),dr["PSID"].ToString() ));
			}
		}
		#region  Remove the selected value 
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
				return this.SelectedItem.Value;
			}
		}
		#endregion 
		 
		#region  And collections bind
		public void BindAppEntities(BP.En.EntitiesNoName ens)
		{
			this.Items.Clear();
			foreach (EntityNoName  en in ens)
			{
				this.Items.Add(new ListItem(en.No+" "+en.Name, en.No) ) ; 
			}
		}
		/// <summary>
		///  Set selected value 
		/// </summary>
		/// <param name="val"></param>
		public void SetSelectItem(object val)
		{
			foreach(ListItem li in this.Items)
			{
				li.Selected=false;
				 
			}

			foreach(ListItem li in this.Items)
			{
				 
				if (li.Value==  val.ToString() )
				{
					li.Selected=true;
					break;
				}
				else
				{
					li.Selected=false;
				}
			}
		}
		#endregion 
		 	
		 
	}
}
