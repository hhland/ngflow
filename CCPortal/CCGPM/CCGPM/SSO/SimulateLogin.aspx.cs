using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace GMP2.SSO
{
    public partial class SimulateLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userid = "admin";
            string password = "pub";
            string host = "";
            CookieContainer cc = new CookieContainer();
            string postData = string.Format("txtUserName={0}&txtPassword={1}", userid, password); // 要发放的数据

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
            HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:50572/AppDemoLigerUI/Login.aspx"); //发送地址
            objWebRequest.Method = "POST";//提交方式
            objWebRequest.ContentType = "application/x-www-form-urlencoded";
            objWebRequest.ContentLength = byteArray.Length;
            objWebRequest.AllowAutoRedirect = false;
            objWebRequest.CookieContainer = cc;
            objWebRequest.KeepAlive = true;
            host = objWebRequest.Host;

            Stream newStream = objWebRequest.GetRequestStream(); // Send the data.
            newStream.Write(byteArray, 0, byteArray.Length); //写入参数
            newStream.Close();

            HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();//获取响应

            response.Cookies = objWebRequest.CookieContainer.GetCookies(objWebRequest.RequestUri);
            CookieCollection cook = response.Cookies;
            string strcrook = objWebRequest.CookieContainer.GetCookieHeader(objWebRequest.RequestUri);

            string strChar = response.CharacterSet;
            StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
            string textResponse = sr.ReadToEnd() + "返回数据"; // 返回的数据
            response.Close();
            string[] substr = textResponse.Split(new char[] { '"' });
            string hostPage = substr[1];
            //Response.Write(hostPage);//打印返回值
            string redirectPage = "http://" + host + hostPage;
            Response.Redirect(redirectPage);
        }

        public static string PostDataGetHtml(string uri, string postData)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uRI = new Uri(uri);
                HttpWebRequest req = WebRequest.Create(uRI) as HttpWebRequest;
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;

                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;
                Stream inStream = res.GetResponseStream();
                StreamReader sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

                return htmlResult;
            }
            catch (Exception ex)
            {
                return "网络错误：" + ex.Message.ToString();
            }
        }

    }
}