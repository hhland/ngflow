using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;

namespace  BP.Web.Controls
{
	/// <summary>
	/// GenerButton  The summary .
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Web.UI.WebControls.Button))]
	public class Btn : System.Web.UI.WebControls.Button
	{
		private BtnType _ShowType=BtnType.Normal;
		[Category(" Custom "),Description(" Display Type button £¨ In order to achieve a global unified management £©")]
		public BtnType ShowType
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
		///  Message .
		/// </summary>
		public string  Hit
		{
			get
			{ 
				return ViewState["_Hit"].ToString();
			}
			set
			{
				 ViewState["_Hit"] =value;
			}
		}
		/// <summary>
		/// Btn
		/// </summary>
		/// <param name="btntype">btntype</param>
		public Btn(BtnType btntype)
		{
			this.ShowType =btntype; 
			//this.PreRender += new System.EventHandler(this.BtnPreRender);
		}
		/// <summary>
		/// Btn
		/// </summary>
        public Btn()
        {
            this.Attributes["class"] = "Btn";
           // this.PreRender += new System.EventHandler(this.BtnPreRender);
        }
		private void BtnPreRender( object sender, System.EventArgs e )
		{
			//this.Attributes["onclick"] +="javascript:showRuning();";
//			if (this.Hit!=null)
//				this.Attributes["onclick"] = "javascript: return confirm(' Whether to continue ?'); ";
			switch (this.ShowType )
			{
				case BtnType.ConfirmHit :
					if (this.Text==null || this.Text=="")
						this.Text=" Confirm (A)";
					if (this.AccessKey==null) 
						this.AccessKey="a";
					 
					this.Attributes["onclick"] = " return confirm('"+this.Hit+"');";				
					break;
			 
				case BtnType.Refurbish :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Refresh (R)";
					if (this.AccessKey==null) 	
						this.AccessKey="r";
					break;
				case BtnType.Back :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Return (B)";
					if (this.AccessKey==null) 	
						this.AccessKey="b";
					break;
				case BtnType.Edit :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Modification (E)";
					if (this.AccessKey==null) 	
						this.AccessKey="e";
					break;
				case BtnType.Close :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Shut down (Q)";
					if (this.AccessKey==null) 	
						this.AccessKey="q";

					this.Attributes["onclick"] += " window.close(); return false";
					
					break;
				case BtnType.Cancel :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Cancel (C)";
					if (this.AccessKey==null) 	
						this.AccessKey="c";
					break;				¡¡
				case BtnType.Confirm :
					if (this.Text==null || this.Text=="")
						this.Text=" Determine (O)";
					if (this.AccessKey==null)
						this.AccessKey="o";
					break;
				case BtnType.Search :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Find (F)";
					if (this.AccessKey==null)
						this.AccessKey="f";
					break;
				case BtnType.New :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" New (N)";
					if (this.AccessKey==null) 
						this.AccessKey="n";
					break;
				case BtnType.SaveAndNew :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Save and New (R)";
					if (this.AccessKey==null) 
						this.AccessKey="n";
					break;
				case BtnType.Delete :
					if (this.Text==null || this.Text=="") 			 
						this.Text= " Delete (D)";
					if (this.AccessKey==null)
						this.AccessKey="c";
					if (this.Hit==null)
						this.Attributes["onclick"] += " return confirm(' To perform this operation delete , Whether to continue ?');";
					else
						this.Attributes["onclick"] += " return confirm(' To perform this operation delete ¡¡["+this.Hit+"], Whether to continue ?');";
					break;
				case BtnType.Export :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Export (G)";
					if (this.AccessKey==null) 	
						this.AccessKey="g";
					break;
				case BtnType.Insert :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Insert (I)";
					if (this.AccessKey==null) 	
						this.AccessKey="i";
					break ;
				case BtnType.Print :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Print (P)";
					if (this.AccessKey==null) 	
						this.AccessKey="p";

					if (this.Hit==null)
						this.Attributes["onclick"] += " return confirm(' Print this operation to be performed , Whether to continue ?');";
					else
						this.Attributes["onclick"] += " return confirm(' Print this operation to be performed ¡¡["+this.Hit+"], Whether to continue ?');";
					break ;
				case BtnType.Save :
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Save (S)";
					if (this.AccessKey==null)
						this.AccessKey="s";
					break;
				case BtnType.View:
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Browse (V)";
					if (this.AccessKey==null) 	
						this.AccessKey="v";
					break;
				case BtnType.Add:
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Increase (A)";
					if (this.AccessKey==null) 	
						this.AccessKey="a";
					break;
				case BtnType.SelectAll:
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Whole selection (A)";
					if (this.AccessKey==null) 	
						this.AccessKey="a";
					break;
				case BtnType.SelectNone:
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Clear All (N)";
					if (this.AccessKey==null) 	
						this.AccessKey="n";
					break;
				case BtnType.Reomve:
					if (this.Text==null || this.Text=="") 			 
						this.Text=" Remove (M)";
					if (this.AccessKey==null) 	
						this.AccessKey="m";

					if (this.Hit==null)
						this.Attributes["onclick"] = " return confirm(' To perform this operation to remove , Whether to continue ?');";
					else
						this.Attributes["onclick"] = " return confirm(' To perform this operation to remove ¡¡["+this.Hit+"], Whether to continue ?');";

					break;
				default:
					if (this.Text==null || this.Text=="")
						this.Text=" Determine (O)";
					if (this.AccessKey==null)
						this.AccessKey="o";
					break; 
			} 
			 
		}

		 
	}
}
