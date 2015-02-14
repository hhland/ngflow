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

        protected string Event
        {
            get
            {
                return Request.Params["event"];
            }
        }

        protected string selfjs_url
        {
            get
            {
                return string.Format("/DataUser/JsLibData/{0}_Self.js",FK_MapData);
            }
        }

        protected FileInfo[] jslibfiles
        {
            get
            {
                DirectoryInfo dir = new DirectoryInfo(jslib_dir);
                if(!dir.Exists)return new FileInfo[]{};
               return dir.GetFiles();
            }
        }

        protected string jslib_url
        {
            get
            {
                return string.Format("/DataUser/JsLib/{0}", Event);
            }
        }

        protected string jslib_dir
        {
            get
            {
                return HttpContext.Current.Server.MapPath(jslib_url);
            }
        }

       

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.initFile();
            
        }
    }
}