using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;

namespace CCFlow.WF.Rpt
{
    public partial class OneFlow : System.Web.UI.Page
    {
        #region  Property .
        public string FK_Flow
        {
            get
            {
                string s = this.Request.QueryString["FK_Flow"];
                if (s == null)
                    s = "007";
                return s;
            }
        }
        public string FK_MapData
        {
            get
            {
                string s = this.Request.QueryString["FK_MapData"];
                if (s == null)
                    s = "ND" + int.Parse(this.FK_Flow) + "Rpt";
                return s;
            }
        }
        public string RptNo
        {
            get
            {
                string s = this.Request.QueryString["RptNo"];
                if (s == null)
                    s = "ND" + int.Parse(this.FK_Flow) + "MyRpt";
                return s;
            }
        }
        #endregion  Property .

        protected void Page_Load(object sender, EventArgs e)
        {
            var fl = new Flow(FK_Flow);
            Title = " Report Design :" + fl.Name;
        }
    }
}