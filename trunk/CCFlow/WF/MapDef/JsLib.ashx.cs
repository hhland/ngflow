using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;

namespace CCFlow.WF.MapDef
{
    /// <summary>
    /// JsLib 的摘要说明
    /// </summary>
    public class JsLib : IHttpHandler
    {
        protected FileInfo _read(string _event, string filename) {
            string filepath = string.Format("/DataUser/JsLib/{0}/{1}", _event, filename)
                 , mappath = HttpContext.Current.Server.MapPath(filepath);
            return new FileInfo(mappath);
        }

        public void read(HttpRequest req,HttpResponse res) {
            res.ContentType = "text/javascript";
            string _event = req.Params["event"];
            string filename = req.Params["filename"];
            
            FileInfo file = _read(_event,filename);
            if (!file.Exists) file.Create().Close();
            string content="";
            using (FileStream fs = file.OpenRead()) {
                using (StreamReader sr = new StreamReader(fs)) {
                    content = sr.ReadToEnd();
                }
            }
           
            res.Write(content);
        }

        public void create(HttpRequest req, HttpResponse res)
        {

            string _event = req.Params["event"];
            string filename = req.Params["filename"];

            FileInfo file = _read(_event, filename);
            if (!file.Exists) file.Create().Close();
        }

        public void delete(HttpRequest req, HttpResponse res)
        {

            string _event = req.Params["event"];
            string filename = req.Params["filename"];

            FileInfo file = _read(_event, filename);
            if (file.Exists) WipeFile(file.FullName,1);
            int code = 1;
            string msg = string.Format("file {0} delete ", file.FullName);
            res.Write("{code:" + code + ",msg:'" + msg + "'}");
        }

        public void save(HttpRequest req, HttpResponse res)
        {

            string _event = req.Params["event"];
            string filename = req.Params["filename"];
            string content = req.Form["content"];
            
            FileInfo file = _read(_event, filename);
            int code = 1;
            string msg = string.Format("file {0} saved ",file.FullName);
            try
            {
                
                if (!file.Exists) file.Create().Close();
                using (FileStream fs = file.OpenRead()) {
                
                    using (StreamWriter sw = new StreamWriter(fs)) {
                      sw.Write(content);
                    }
                }
            }
            catch (Exception ex) {
                code = 0;
                msg = ex.Message;
            }
            res.Write("{code:"+code+",msg:'"+msg+"'}");
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            string action = req.Params["action"];
            this.GetType().GetMethod(action).Invoke(this,new object[]{req,res});
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void WipeFile(string filename, int timesToWrite)
        {
            
                if (File.Exists(filename))
                {
                    //设置文件的属性为正常，这是为了防止文件是只读
                    File.SetAttributes(filename, FileAttributes.Normal);
                    //计算扇区数目
                    double sectors = Math.Ceiling(new FileInfo(filename).Length / 512.0);
                    // 创建一个同样大小的虚拟缓存
                    byte[] dummyBuffer = new byte[512];
                    // 创建一个加密随机数目生成器
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    // 打开这个文件的FileStream
                    FileStream inputStream = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    for (int currentPass = 0; currentPass < timesToWrite; currentPass++)
                    {
                        // 文件流位置
                        inputStream.Position = 0;
                        //循环所有的扇区
                        for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                        {
                            //把垃圾数据填充到流中
                            rng.GetBytes(dummyBuffer);
                            // 写入文件流中
                            inputStream.Write(dummyBuffer, 0, dummyBuffer.Length);
                        }
                    }
                    // 清空文件
                    inputStream.SetLength(0);
                    // 关闭文件流
                    inputStream.Close();
                    // 清空原始日期需要
                    DateTime dt = new DateTime(2037, 1, 1, 0, 0, 0);
                    File.SetCreationTime(filename, dt);
                    File.SetLastAccessTime(filename, dt);
                    File.SetLastWriteTime(filename, dt);
                    // 删除文件
                    File.Delete(filename);
                }
            
        }
    }
}