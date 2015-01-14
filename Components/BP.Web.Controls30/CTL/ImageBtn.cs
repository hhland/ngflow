using System;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;


namespace  BP.Web.Controls
{
	/// <summary>
	/// GenerButton  The summary .
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Web.UI.WebControls.ImageButton))]
	public class ImageBtn : System.Web.UI.WebControls.ImageButton 
	{
		public enum ImageBtnType
		{
			Normal,			 
			Confirm,
			Save,
			Search,
		    Cancel,
			Delete,
			Update,
			Insert,
			Edit,
			New,
			View,
			Close,
			Export,
			Print,
			Add,
			Reomve
		}		
		private ImageBtnType _ShowType=ImageBtnType.Normal;
		public ImageBtnType ShowType
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
		private string _Hit=null;
		/// <summary>
		///  Message .
		/// </summary>
		public string  Hit
		{
			get
			{ 
				return _Hit;
			}
			set
			{
				this._Hit=value;
			}
		}
		public ImageBtn()
		{	
			this.CssClass="ImageBtn"+WebUser.Style;
			this.PreRender += new System.EventHandler(this.LinkBtnPreRender);
		}
		private void LinkBtnPreRender( object sender, System.EventArgs e )
		{
			if (this.Hit!=null)
				this.Attributes["onclick"] = "javascript: return confirm(' Whether to continue ?'); ";

			switch (this.ShowType )
			{
				case ImageBtnType.Edit :
					this.ImageUrl=" Modification ";
					 
					if (this.AccessKey==null)
						this.AccessKey="e";
					break;
				case ImageBtnType.Close :
					this.ImageUrl=" Shut down ";
					 
					if (this.AccessKey==null) 	
						this.AccessKey="q";					 
					break;
				case ImageBtnType.Cancel :
					this.ImageUrl=" Cancel ";
					 
					if (this.AccessKey==null) 	
						this.AccessKey="c";
					break;				　
				case ImageBtnType.Confirm :
					this.ImageUrl=" Determine ";
					 
					if (this.AccessKey==null)
						this.AccessKey="o";
				    break;
				case ImageBtnType.Search :
					this.ImageUrl=" Find ";
					 
					if (this.AccessKey==null)
						this.AccessKey="f";
					break;
				case ImageBtnType.New :
					this.ImageUrl=" New ";
					 
					if (this.AccessKey==null) 
						this.AccessKey="n";
					break;
				case ImageBtnType.Delete :
					this.ImageUrl=" Delete ";
					 
					if (this.AccessKey==null)
						this.AccessKey="c";
					if (this.Hit==null)
					    this.Attributes["onclick"] = " return confirm(' To perform this operation delete , Whether to continue ?');";
					else
						this.Attributes["onclick"] = " return confirm(' To perform this operation delete 　["+this.Hit+"], Whether to continue ?');";

					break;
				case ImageBtnType.Export :
					this.ImageUrl=" Export ";
					 
					if (this.AccessKey==null) 	
						this.AccessKey="g";
					break;
				case ImageBtnType.Insert :
					 
					this.ImageUrl=" Insert ";
					 
					if (this.AccessKey==null) 	
						this.AccessKey="i";
					break ;
				case ImageBtnType.Print :
					this.ImageUrl="ssss";
					if (this.AccessKey==null) 	
						this.AccessKey="p";

					if (this.Hit==null)
						this.Attributes["onclick"] = " return confirm(' Print this operation to be performed , Whether to continue ?');";
					else
						this.Attributes["onclick"] = " return confirm(' Print this operation to be performed 　["+this.Hit+"], Whether to continue ?');";
					break ;
				case ImageBtnType.Save :
					this.ImageUrl="ssss";
					if (this.AccessKey==null)
						this.AccessKey="s";
					break;
				case ImageBtnType.View:
					this.ImageUrl="ssss";
					if (this.AccessKey==null) 	
						this.AccessKey="v";
					break;
				case ImageBtnType.Add:
					this.ImageUrl="ssss";
					if (this.AccessKey==null) 	
						this.AccessKey="a";
					break;
				case ImageBtnType.Reomve:
					this.ImageUrl=" Remove ";
					 

					if (this.Hit==null)
						this.Attributes["onclick"] = " return confirm(' To perform this operation to remove , Whether to continue ?');";
					else
						this.Attributes["onclick"] = " return confirm(' To perform this operation to remove 　["+this.Hit+"], Whether to continue ?');";
					break;
				default:
					 this.ImageUrl=" Determine ";
					if (this.AccessKey==null)
						this.AccessKey="o";
					break; 
			}		 
			
			 
		}	
	 
		
		 
	}
}
