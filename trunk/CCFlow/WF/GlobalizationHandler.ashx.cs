using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BP.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CCFlow.WF
{
    /// <summary>
    /// Summary description for GlobalizationHandler
    /// </summary>
    public class GlobalizationHandler : IHttpHandler
    {

        public static string defaultCluture = "en-us";

        public static Dictionary<string, string> clutures
        {
            get
            {
                Dictionary<string,string> dict=new Dictionary<string, string>();
                dict.Add("en-us", "English");
                dict.Add("fa-ir", "فارسی");
                dict.Add("zh-cn", "中文");
                 dict.Add("ko-kr", "한국어");
                 
                return dict;
            }
        }
    

        public static string globares_dir
        {
            get { return HttpContext.Current.Server.MapPath("~") + "App_GlobalResources"; }

        }


        protected string localres_dir
        {
            get { return HttpContext.Current.Server.MapPath("~") + "App_LocalResources"; }

        }



        public static string[] _globalResKeys()
        {
            string[] fnames = Directory.GetFiles(globares_dir);
            HashSet<string> files=new HashSet<string>();
            foreach (string fname in fnames.Where(f=>f.EndsWith(".resx")))
            {
                string tempname = Path.GetFileName(fname);
                string keyname = tempname.Substring(0, tempname.IndexOf("."));
                files.Add(keyname);
            }

            return files.ToArray();
        }


        public static string[] loadNames(string key)
        {

            HashSet<string> names=new HashSet<string>();
             string path = globares_dir + "\\" + key + ".resx";
            
            XElement elroot = XElement.Load(path);
            List<XElement> eldatas = elroot.Elements("data").ToList();
            //List<Lang> langs = new List<Lang>();
            foreach (XElement eldata in eldatas)
            {
                names.Add(eldata.Attribute("name").Value);
            }
            return names.ToArray();
        }


        public static List<Lang> loadLangs(string key, string cluture)
        {
            
            string path = globares_dir + "\\"+key+"."+cluture+".resx";
            if (cluture == defaultCluture)
            {
                path = globares_dir + "\\" + key + ".resx";
            }
            XElement elroot = XElement.Load(path);
            List<XElement> eldatas = elroot.Elements("data").ToList();
            List<Lang> langs=new List<Lang>();
            foreach (XElement eldata in eldatas)
            {
                XElement elValue = eldata.Element("value");
                langs.Add(new Lang
                {
                    name = eldata.Attribute("name").Value
                    ,text = elValue.Value
                });
            }
            return langs;
        }

        public static int _saveLang(string key, string cluture,string name ,string text)
        {
            int eff = 0;
            string path = globares_dir + "\\" + key + "." + cluture + ".resx";
            if (cluture == defaultCluture)
            {
                path = globares_dir + "\\" + key + ".resx";
            }
            
            XElement elroot = XElement.Load(path);
            XElement eldata= elroot.Elements("data").SingleOrDefault(el=>el.Attribute("name").Value==name);
            //XElement elValue = eldata.Element("value");
            if (eldata == null)
            {
                XElement newdata=new XElement("data");
                foreach (var attr in elroot.Elements("data").First().Attributes())
                {
                    newdata.SetAttributeValue(attr.Name,attr.Value);  
                }
                //newdata.SetAttributeValue("xml:space","preserve");
                //newdata.SetAttributeValue(xnas,"preserve" );
                newdata.SetAttributeValue("name",name);
                XElement newvalue=new XElement("value");
                newvalue.SetValue(text);
                newdata.Add(newvalue);
                elroot.Add(newdata);
                elroot.Save(path);
                eff++;
            }else if( text!= eldata.Element("value").Value.ToString())
            {
                eldata.Element("value").SetValue(text);
                elroot.Save(path);
                eff++;
            }
            

            return eff;
        }

        public void clutureLangs(HttpRequest request,HttpResponse response)
        {
            string key = request.Params["key"];
            JObject grid = new JObject();
            //grid["total"] = keys.Length;
            JArray rows = new JArray();
            Dictionary<string ,List<Lang>> clutureLangs=new Dictionary<string, List<Lang>>();
            foreach (var cluture in clutures.Keys)
            {
                List<Lang> langs = loadLangs(key, cluture);
                clutureLangs.Add(cluture,langs);
            }

            foreach (var name in loadNames(key))
            {
                JObject row = new JObject();
                row["name"] = name;
                foreach (var clutureLang in clutureLangs.Keys)
                {
                    List<Lang> langs = clutureLangs[clutureLang];
                    Lang lang = langs.SingleOrDefault(l => l.name == name);
                    if (lang != null)
                    {
                        row[clutureLang] = lang.text;
                    }
                }
                rows.Add(row);
            }
            
            grid["rows"] = rows;
            grid["total"] = rows.Count;
            response.Write(grid);
        } 
        
        public void saveLang(HttpRequest request,HttpResponse response)
        {
            string name=request.Params["name"],key=request.Params["key"];
            int eff = 0;
            foreach(string en in clutures.Keys)
            {
                string cluture = clutures[en];
                string val = request.Params[en];
                eff+= _saveLang(key, en,name, val);
            }
            string code="0",msg=string.Format("{0} lang saved",eff)
                ,json = "{eff:"+eff+",code:" +code+ ",msg:'"+msg+"'}";
            response.Write(json);
        }
         
        public void globalResKeys(HttpRequest request, HttpResponse response)
        {
            string[] keys=_globalResKeys();
            JObject grid=new JObject();
            grid["total"]=keys.Length;
            JArray rows=new JArray();
            foreach (var key in keys)
            {
                JObject row=new JObject();
                row["key"] = key;
                rows.Add(row);
            }
            grid["rows"]=rows;
            response.Write(grid);
        }

        public void listLocalRes(HttpRequest request,HttpResponse response)
        {
            DirectoryInfo dir=new DirectoryInfo(localres_dir);
            FileInfo[] fs= dir.GetFiles();
            
            string[] fnames = Directory.GetFiles(localres_dir);
            response.Write(JsonConvert.SerializeObject(fnames));
        }

        public void loadLang(HttpRequest request,HttpResponse response)
        {

            string path = localres_dir + "\\Default.aspx.resx";
            XElement elroot = XElement.Load(path);
            List<XElement> eldatas= elroot.Elements("data").ToList();
            response.Write(Json.ToArrayString(eldatas.Select(el=>el.Attribute("name"))));

        }

        public void changeCulture(HttpRequest request, HttpResponse response)
        {
            string culture = request.Params["culture"];
            HttpContext.Current.Session["culture"] = culture;
           
            
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
           
            string action = req.Params["action"];
            this.GetType().GetMethod(action).Invoke(this, new object[] { req, res });
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class Lang
        {
            public string name, text;
        }
    }
}