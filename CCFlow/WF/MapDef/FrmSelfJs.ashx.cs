using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCFlow.WF.MapDef
{
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// FrmSelfJs1 的摘要说明
    /// </summary>
    public class FrmSelfJs1 : IHttpHandler
    {

        protected string FK_MapData, OperAttrKey,Event;

        protected string selfjs_url
        {
            get
            {
                return string.Format("/DataUser/JsLibData/{0}_Self.js", FK_MapData);
            }
        }

        protected string selfjs_file
        {
            get
            {
                return HttpContext.Current.Server.MapPath(selfjs_url);
            }
        }

        protected string functionname
        {
            get
            {
                return string.Format("{0}_{1}",OperAttrKey,Event);
            }
        }

        protected string region_start
        {
            get
            {
                return string.Format("//regionstart::{0}",functionname);
            }
        }

        protected string region_end
        {
            get
            {
                return string.Format("//regionend::{0}", functionname);
            }
        }

        protected string readRegion()
        {
            string region = "";
            FileInfo file = new FileInfo(selfjs_file);
            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);
            string content= sr.ReadToEnd();
            Regex regex = new Regex(region_start + @"([\s\S]*)" + region_end);
            if (regex.IsMatch(content))
            {
                string _re = regex.Match(content).Value;
                region = _re.Substring(region_start.Length, _re.Length - region_start.Length - region_end.Length);
            }
            sr.Close();
            fs.Close();
            return region;
        }

        protected bool replaceRegion(string replaceRegion,out string content)
        {
            bool replaced = false;
            FileInfo file = new FileInfo(selfjs_file);
            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);
            content = sr.ReadToEnd();
            Regex regex = new Regex(region_start + @"([\s\S]*)" + region_end);
            if (regex.IsMatch(content))
            {
                content = regex.Replace(content, region_start + replaceRegion+region_end);
                replaced = true;
            }
            sr.Close();
            fs.Close();
            return replaced;
        }

        public void read(HttpRequest req, HttpResponse res)
        {
            string region = this.readRegion();
            if (string.IsNullOrWhiteSpace(region))
            {
                region += "function "+functionname+"() {\n\n } ";
            }
            res.Write(region);
        }

        public void save(HttpRequest req, HttpResponse res)
        {
            //res.ContentType = "text/json";
            string region = req.Form["region"];
            FileInfo file= new FileInfo(selfjs_file);
            
            string content = "";
            
            if (this.replaceRegion((region.StartsWith("\n")?"":"\n")+region+(region.EndsWith("\n")?"":"\n")
                , out content))
            {

            }
            else
            {
                content += "\n\n"
                    + region_start +"\n"
                    + region
                    + region_end +"\n\n"
                    ;
            }
            Byte[] bContent = System.Text.Encoding.UTF8.GetBytes(content);
            FileStream fs = file.OpenWrite();
            fs.Write(bContent, 0, bContent.Length);
            fs.Flush();
            fs.Close();
            int code = 1;
            string msg = selfjs_file+" saved.";
            res.Write("{code:"+code+",msg:\""+msg+"\"}");

        }

       
        public void ProcessRequest(HttpContext context)
        {
            
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            res.ContentType = "text/html";
            string action = req.Params["action"];
            FK_MapData = req.Params["FK_MapData"];
            OperAttrKey = req.Params["OperAttrKey"];
            Event = req.Params["event"];
            this.GetType().GetMethod(action).Invoke(this, new object[] { req, res });
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}