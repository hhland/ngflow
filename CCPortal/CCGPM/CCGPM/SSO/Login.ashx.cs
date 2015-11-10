using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GMP2.SSO
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler
    {
        //******************************************************
        /// <summary>
        /// 获取Auth,需要开发
        /// </summary>
        /// <returns></returns>
        private string GetCurAuth()
        {

            return this.GetUserAuth(this.GetCurUserNo());
        }
        /// <summary>
        /// 获取当前用户，需要开发
        /// </summary>
        /// <returns></returns>
        private string GetCurUserNo()
        {
            return BP.Web.WebUser.No;
        }
        private string rUrl
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Request.QueryString["Url"]);
            }
        }
        private bool IsOnline
        {
            get 
            {
                return true;
            }
        }
        //**************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            //string url_From = context.Request.UrlReferrer.ToString();
            /////判断当前是否是登录状态。
            //if (!this.IsOnline)
            //{
            //    //context.Response.Redirect(url_From);
            //    context.Response.Write("dddddddddddd");
            //    return;
            //}
            ///如果没有，不变
            ///
           
            string url = this.rUrl;
            string user = this.GetCurUserNo(); //获取
            string auth = this.GetCurAuth();//获取
            //转出
            context.Response.Redirect(String.Format("{0}?userid={1}&sid={2}", url, user, auth),true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string GetUserAuth(string userNo)
        {
            string sql = String.Format("select [SID] from Port_Emp where [No]='{0}'", userNo);
            return BP.DA.DBAccess.RunSQLReturnString(sql);
        }
    }
}