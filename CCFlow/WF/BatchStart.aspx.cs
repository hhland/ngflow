using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF
{
    public partial class BatchStartMy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.RegisterClientScriptBlock("b81",
               "<script language='JavaScript' src='./Comm/JS/Calendar/WdatePicker.js' defer='defer' type='text/javascript' ></script>");

            this.Page.RegisterClientScriptBlock("bd081",
               "<script language='JavaScript' src='./CCForm/MapExt.js' defer='defer' type='text/javascript' ></script>");
        }
    }
}