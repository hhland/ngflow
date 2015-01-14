using System;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace  BP.Web.Controls
{
	/// <summary>
	/// BPHyperLink  The summary .
	/// </summary>
	[System.Drawing.ToolboxBitmap(typeof(System.Web.UI.WebControls.HyperLink))]
	public class BPHyperLink : System.Web.UI.WebControls.HyperLink
	{
		public BPHyperLink()	   
		{
			this.CssClass = "HyperLink"+WebUser.Style;
		}
	}
}
