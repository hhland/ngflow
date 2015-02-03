using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCFlow.WF.MapDef
{
    using System.IO;

    public partial class FrmSelfJs : System.Web.UI.Page
    {
        protected string FK_MapData
        {
            get
            {
                return Request.Params["FK_MapData"];
            }
        }

        protected string selfjs_url
        {
            get
            {
                return string.Format("/DataUser/JsLibData/{0}_Self.js",FK_MapData);
            }
        }

        protected void initFile()
        {
            string filepath = Server.MapPath(selfjs_url);
            FileInfo file=new FileInfo(filepath);
            if (!file.Exists)
            {
                file.Create();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.initFile();
        }
    }
}