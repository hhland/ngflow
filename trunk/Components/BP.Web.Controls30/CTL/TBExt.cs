using System;
using System.Web.UI.WebControls;
using System.Drawing;

namespace BP.Web.Controls
{
//	public enum TBExtType
//	{
//		/// <summary>
//		///  Normal 
//		/// </summary>
//		Normal,	 
//		/// <summary>
//		///  User-defined 
//		/// </summary>
//		Custom,
//		/// <summary>
//		///  Taxpayer 
//		/// </summary>
//		TaxpayerKey,
//		/// <summary>
//		///  Real estate construction unit 
//		/// </summary>
//		FDCUnit,
//		/// <summary>
//		///  Jian Gong unit building projects 
//		/// </summary>
//		FDCTaxpayer,
//		/// <summary>
//		///  Building project 
//		/// </summary>
//        FDCBulidPrjNo,		
//		/// <summary>
//		///  Real estate projects 
//		/// </summary>
//		FDCRealtyPrjNo
//	} 
	/// <summary>
	/// BPListBox  The summary .
	/// </summary>
	public class TBExt:System.Web.UI.WebControls.TextBox
	{
		public TBExt()
		{
			this.PreRender += new System.EventHandler(this.TBPreRender);
		}
		private void TBPreRender( object sender, System.EventArgs e )
		{
			#region 
			#endregion 
		}
		
		private string  _DataHelpKey="";
		public string DataHelpKey 
		{
			get
			{
			   return _DataHelpKey;				
			}
			set
			{
				this._DataHelpKey=value;
			}
		}
		public object TextExt
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text=value.ToString();
			}
		}
		public int TextExtInt
		{
			get
			{
				return int.Parse(this.Text);
			}
			set
			{
				this.Text=value.ToString();
			}
		}
		public float TextExtFloat
		{
			get
			{
				return float.Parse(this.Text);
			}
			set
			{
				this.Text=value.ToString();
			}
		}

		public decimal TextExtDecimal
		{
			get
			{
				return decimal.Parse(this.Text);
			}
			set
			{
				this.Text=value.ToString();
			}
		} 

	}
	
}
