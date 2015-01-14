using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;

namespace BP.Web.Controls
{
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:ImgEn runat=server></{0}:ImgEn>")]
	public class ImgEn : System.Web.UI.WebControls.WebControl , IPostBackEventHandler
	{
		private const string _BUTTONDEFAULTSTYLE = "BORDER-RIGHT: gray 1px solid; BORDER-TOP: gray 1px solid; BORDER-LEFT: gray 1px solid; CURSOR:hand; BORDER-BOTTOM: gray 1px solid;";
		// Choose Label Default style buttons s
¡¡   ¡¡/// The default button text 
		private const string _BUTTONDEFAULTTEXT = "...";
		private System.Web.UI.WebControls.Label _Label;
		/// <summary>
		/// Controls
		/// </summary>
		public override ControlCollection Controls
		{
			get
			{
				EnsureChildControls(); // Confirm child controls have been created sets 
				return base.Controls;
			}
		}

		// Creating child controls £¨ Calendar control server £©
		protected override void CreateChildControls()
		{
			Controls.Clear();
			_Label = new Label();
			_Label.ID = MyLabelID;
			_Label.Font.Size = FontUnit.Parse("9pt");
			_Label.Font.Name = "Verdana";

			this.Controls.Add(_Label);
		}
		[Category("Appearance"), // The property belongs to the category , Referring to FIG. 
		DefaultValue(""), // Property Default 
		Description(" Setting this Label Value of the control .") // Description of the property 
		]
		public string Text
		{
			get
			{
				EnsureChildControls();
				return (string)ViewState["Text"];
			}
			set
			{
				ViewState["Text"] = value;
			}
		}
		// Overloaded server controls Enabled Property , The choice Label Button is grayed out £¨ Disable £©
		public override bool Enabled
		{
			get{EnsureChildControls();return ViewState["Enabled"] == null?true:(bool)ViewState["Enabled"];}
			set{EnsureChildControls();ViewState["Enabled"] = value;}
		}
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string MyLabelID // Composite Control ID
		{
			get
			{
				EnsureChildControls();
				return this.ClientID+"_MyLabel";
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string MyLabelName // Composite control name 
		{
			get
			{
				EnsureChildControls();
				return this.UniqueID+":MyLabel";
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string ImgEnInputID // Composite control input box ID
		{
			get
			{
				EnsureChildControls();
				return this.ClientID+"_DateInput";
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string ImgEnInputName // Enter the name of a composite control box 
		{
			get
			{
				EnsureChildControls();
				return this.UniqueID+":DateInput";
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public string ImgEnButtonID // Composite control buttons ID
		{
			get
			{
				EnsureChildControls();
				return this.ClientID+"_DateButton";
			}
		}

		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]

		public string ImgEnButtonName // Composite control buttons name 
		{
			get
			{
				EnsureChildControls();
				return this.UniqueID+":DateButton";
			}
		}

		public string ButtonText
		{
			get
			{
				EnsureChildControls();
				return ViewState["ButtonText"] == null?_BUTTONDEFAULTTEXT:(string)ViewState["ButtonText"];
			}
			set
			{
				EnsureChildControls();
				ViewState["ButtonText"] = value;
			}
		}

		/// <summary>
		///  Presents this control to the specified output parameter .
		/// </summary>
		/// <param name="output">  To write to the  HTML  Writer  </param>

		protected override void Render(HtmlTextWriter output)
		{
			// When the output controls in the page , Generate a table £¨ Two rows two £©, The following is a table of style 
			output.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			output.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");

			output.AddStyleAttribute("LEFT", this.Style["LEFT"]);
			output.AddStyleAttribute("TOP", this.Style["TOP"]);
			output.AddStyleAttribute("POSITION", "absolute");

			if (Width != Unit.Empty)
			{
				output.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
			}
			else
			{
				output.AddStyleAttribute(HtmlTextWriterStyle.Width, "200px");
			}

			output.RenderBeginTag(HtmlTextWriterTag.Table); // Output table 
			output.RenderBeginTag(HtmlTextWriterTag.Tr); // The first line of the table 
			output.AddAttribute(HtmlTextWriterAttribute.Width, "90%");
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			// The following are the property of their style settings of the first row and first column of Chinese boxes 

			if (!Enabled)
			{
				output.AddAttribute(HtmlTextWriterAttribute.ReadOnly, "true");
			}

			output.AddAttribute(HtmlTextWriterAttribute.Type, "Text");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ImgEnInputID);
			output.AddAttribute(HtmlTextWriterAttribute.Name, ImgEnInputName);
			output.AddAttribute(HtmlTextWriterAttribute.Value, Text);
			output.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			output.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
			output.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, Font.Name);
			output.AddStyleAttribute(HtmlTextWriterStyle.FontSize, Font.Size.ToString());
			output.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, Font.Bold?"bold":"");
			output.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(BackColor));
			output.AddStyleAttribute(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(ForeColor));
			output.RenderBeginTag(HtmlTextWriterTag.Input); // Output text box 
			output.RenderEndTag();
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Width, "*");
			output.RenderBeginTag(HtmlTextWriterTag.Td);

			// The following are the first line of the property and style settings button in the second column 

			if (!Enabled)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Disabled, "true");
			}

			output.AddAttribute(HtmlTextWriterAttribute.Type, "Submit");
			output.AddAttribute(HtmlTextWriterAttribute.Id, ImgEnButtonID);
			output.AddAttribute(HtmlTextWriterAttribute.Name, ImgEnButtonName);
			output.AddAttribute(HtmlTextWriterAttribute.Value, ButtonText);
			output.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			//output.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.GetPostBackEventReference(this)); // Click the button needs to return the server to trigger behind OnClick Event 

			//output.AddAttribute(HtmlTextWriterAttribute.Style, ButtonStyle);
			output.RenderBeginTag(HtmlTextWriterTag.Input); // Output button 
			output.RenderEndTag();
			output.RenderEndTag();

			output.RenderEndTag();
			output.RenderBeginTag(HtmlTextWriterTag.Tr);
			output.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
			output.RenderBeginTag(HtmlTextWriterTag.Td);
			_Label.RenderControl(output); // The calendar child control output 
			output.RenderEndTag();
			output.RenderEndTag();
			output.RenderEndTag();
		}

		// Composite control must inherit IpostBackEventHandler Interface , To inherit RaisePostBackEvent Event 
		public void RaisePostBackEvent(string eventArgument)
		{
			OnClick(EventArgs.Empty);
		}
		protected virtual void OnClick(EventArgs e)
		{
			//			// Click on the button when you select a date , If the calendar does not show the child controls displayed text box value and the child controls assigned to the calendar 
			//			if (_Calendar.Attributes["display"] != "")
			//			{
			//				_Calendar.SelectedDate = DateTime.Parse(Text);
			//				_Calendar.Style.Add("display","");
			//			}
		}

		// Composite control calendar control Label Change event 
		private void _Label_SelectionChanged(object sender, EventArgs e)
		{
			return ;

			// When selecting the Label When changes , Selected Label Assigned to the text box and calendar child controls hidden 
			//Text = _Label.SelectedDate.ToString();
			//_Label.Style.Add("display","none");
		}
	}
}

	
	 
