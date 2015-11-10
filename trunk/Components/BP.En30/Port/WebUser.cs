using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;
using System.Web;
using System.Data;
using BP.En;
using BP.DA;
using System.Configuration;
using BP.Port;
using BP.Sys;


namespace BP.Web
{
    /// <summary>
    /// User  The summary .
    /// </summary>
    public class WebUser
    {
        /// <summary>
        ///  Password decryption 
        /// </summary>
        /// <param name="pass"> User password </param>
        /// <returns> Decrypted password </returns>
        public static string ParsePass(string pass)
        {
            if (pass == "")
                return "";

            string str = "";
            char[] mychars = pass.ToCharArray();
            int i = 0;
            foreach (char c in mychars)
            {
                i++;

                //step 1 
                long A = Convert.ToInt64(c) * 2;

                // step 2
                long B = A - i * i;

                // step 3 
                long C = 0;
                if (B > 196)
                    C = 196;
                else
                    C = B;

                str = str + Convert.ToChar(C).ToString();
            }
            return str;
        }
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="em"></param>
        public static void SignInOfGener(Emp em)
        {
            SignInOfGener(em, "CH", null, true, false);
        }

        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="em"></param>
        /// <param name="isRememberMe"></param>
        public static void SignInOfGener(Emp em, bool isRememberMe)
        {
            SignInOfGener(em, "CH", null, isRememberMe, false);
        }
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="em"></param>
        /// <param name="auth"></param>
        public static void SignInOfGenerAuth(Emp em, string auth)
        {
            SignInOfGener(em, "CH", auth, true, false);
        }
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="em"></param>
        /// <param name="lang"></param>
        public static void SignInOfGenerLang(Emp em, string lang, bool isRememberMe)
        {
            SignInOfGener(em, lang, null, isRememberMe, false);
        }
        /// <summary>
        ///  Landed 
        /// </summary>
        /// <param name="em"></param>
        /// <param name="lang"></param>
        public static void SignInOfGenerLang(Emp em, string lang)
        {
            SignInOfGener(em, lang, null, true, false);
        }
        public static void SignInOfGener(Emp em, string lang)
        {
            SignInOfGener(em, lang, em.No, true, false);
        }
        /// <summary>
        ///  Log in 
        /// </summary>
        /// <param name="em"> Log people </param>
        /// <param name="lang"> Language </param>
        /// <param name="auth"> Login authorized person </param>
        /// <param name="isRememberMe"> Whether remember me </param>
        public static void SignInOfGener(Emp em, string lang, string auth, bool isRememberMe)
        {
            SignInOfGener(em, lang, auth, isRememberMe, false);
        }
        /// <summary>
        ///  GM landing 
        /// </summary>
        /// <param name="em"> Staff </param>
        /// <param name="lang"> Language </param>
        /// <param name="auth"> Authorized person </param>
        /// <param name="isRememberMe"> Are records cookies</param>
        /// <param name="IsRecSID"> Are records SID</param>
        public static void SignInOfGener(Emp em, string lang, string auth, bool isRememberMe, bool IsRecSID)
        {
            if (System.Web.HttpContext.Current == null)
                SystemConfig.IsBSsystem = false;
            else
                SystemConfig.IsBSsystem = true;

            if (SystemConfig.IsBSsystem)
                BP.Sys.Glo.WriteUserLog("SignIn", em.No, " Log in ");

            WebUser.Auth = auth;
            WebUser.No = em.No;
            WebUser.Name = em.Name;

          
            WebUser.FK_Dept = em.FK_Dept;
            WebUser.FK_DeptName = em.FK_DeptText;
            WebUser.HisDepts = null;
            WebUser.HisStations = null;
            if (IsRecSID)
            {
                /* If the record sid*/
                string sid1 = DateTime.Now.ToString("MMddHHmmss");

                DBAccess.RunSQL("UPDATE Port_Emp SET SID='" + sid1 + "' WHERE No='" + WebUser.No + "'");
                WebUser.SID = sid1;
            }

            WebUser.SysLang = lang;
            if (BP.Sys.SystemConfig.IsBSsystem)
            {
                //System.Web.HttpContext.Current.Response.Cookies.Clear();

                HttpCookie hc = BP.Sys.Glo.Request.Cookies["CCS"];
                if (hc != null)
                    BP.Sys.Glo.Request.Cookies.Remove("CCS");

                HttpCookie cookie = new HttpCookie("CCS");
                cookie.Expires = DateTime.Now.AddDays(2);
                cookie.Values.Add("No", em.No);
                cookie.Values.Add("Name", HttpUtility.UrlEncode(em.Name));


                if (isRememberMe)
                    cookie.Values.Add("IsRememberMe", "1");
                else
                    cookie.Values.Add("IsRememberMe", "0");

                cookie.Values.Add("FK_Dept", em.FK_Dept);
                cookie.Values.Add("FK_DeptName", HttpUtility.UrlEncode(em.FK_DeptText));

                if (System.Web.HttpContext.Current.Session != null)
                {
                    cookie.Values.Add("Token", System.Web.HttpContext.Current.Session.SessionID);
                    cookie.Values.Add("SID", System.Web.HttpContext.Current.Session.SessionID);
                }

                cookie.Values.Add("Lang", lang);

                //string isEnableStyle = SystemConfig.AppSettings["IsEnableStyle"];
                //if (isEnableStyle == "1")
                //{
                //    try
                //    {
                //        string sql = "SELECT Style FROM WF_Emp WHERE No='" + em.No + "' ";
                //        int val = DBAccess.RunSQLReturnValInt(sql, 0);
                //        cookie.Values.Add("Style", val.ToString());
                //        WebUser.Style = val.ToString();
                //    }
                //    catch
                //    {
                //    }
                //}
                if (auth == null)
                    auth = "";
                cookie.Values.Add("Auth", auth); // Authorized person .
                System.Web.HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        #region  Static methods 
        /// <summary>
        ///  By key, Take out session.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="isNullAsVal"> In the case of Null,  The value returned .</param>
        /// <returns></returns>
        public static string GetSessionByKey(string key, string isNullAsVal)
        {
            if (IsBSMode && System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null)
            {
                string str = System.Web.HttpContext.Current.Session[key] as string;
                if (string.IsNullOrEmpty(str))
                    str = isNullAsVal;
                return str;
            }
            else
            {
                if (BP.Port.Win.Current.Session[key] == null || BP.Port.Win.Current.Session[key].ToString() == "")
                {
                    BP.Port.Win.Current.Session[key] = isNullAsVal;
                    return isNullAsVal;
                }
                else
                    return (string)BP.Port.Win.Current.Session[key];
            }
        }
        public static object GetObjByKey(string key)
        {
            if (IsBSMode)
            {
                return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                return BP.Port.Win.Current.Session[key];
            }
        }
        #endregion

        /// <summary>
        ///  Is not it b/s  Mode .
        /// </summary>
        protected static bool IsBSMode
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                    return false;
                else
                    return true;
            }
        }
        public static object GetSessionByKey(string key, Object defaultObjVal)
        {
            if (IsBSMode == true
                && System.Web.HttpContext.Current != null
                && System.Web.HttpContext.Current.Session != null)
            {
                if (System.Web.HttpContext.Current.Session[key] == null)
                    return defaultObjVal;
                else
                    return System.Web.HttpContext.Current.Session[key];
            }
            else
            {
                if (BP.Port.Win.Current.Session[key] == null)
                    return defaultObjVal;
                else
                    return BP.Port.Win.Current.Session[key];
            }
        }
        /// <summary>
        ///  Set up session
        /// </summary>
        /// <param name="key">¼ü</param>
        /// <param name="val">Öµ</param>
        public static void SetSessionByKey(string key, object val)
        {
            if (val == null)
                return;

            if (IsBSMode == true
                && System.Web.HttpContext.Current != null
                && System.Web.HttpContext.Current.Session != null)
                System.Web.HttpContext.Current.Session[key] = val;
            else
                BP.Port.Win.Current.SetSession(key, val);
        }
        /// <summary>
        ///  Return 
        /// </summary>
        public static void Exit()
        {
            if (IsBSMode == false)
            {
                try
                {
                    string token = WebUser.Token;
                    System.Web.HttpContext.Current.Response.Cookies.Clear();
                    BP.Sys.Glo.Request.Cookies.Clear();
                    HttpCookie cookie = new HttpCookie("CCS", string.Empty);
                    cookie.Expires = DateTime.Now.AddDays(2);
                    cookie.Values.Add("No", string.Empty);
                    cookie.Values.Add("Name", string.Empty);
                    // 2013.06.07 H
                    cookie.Values.Add("Pass", string.Empty);
                    cookie.Values.Add("IsRememberMe", "0");
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    WebUser.Token = token;
                    BP.Port.Win.Current.Session.Clear();
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    string token = WebUser.Token;
                    System.Web.HttpContext.Current.Response.Cookies.Clear();
                    BP.Sys.Glo.Request.Cookies.Clear();

                    // System.Web.HttpContext.Current.Application.Clear();

                    System.Web.HttpContext.Current.Session.Clear();

                    HttpCookie cookie = new HttpCookie("CCS", string.Empty);
                    cookie.Expires = DateTime.Now.AddDays(2);
                    cookie.Values.Add("No", string.Empty);
                    cookie.Values.Add("Name", string.Empty);
                    // 2013.06.07 H
                    cookie.Values.Add("Pass", string.Empty);
                    cookie.Values.Add("IsRememberMe", "0");
                    cookie.Values.Add("Auth", string.Empty); // Authorized person .
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                    WebUser.Token = token;
                }
                catch
                {
                }
            }
        }
        /// <summary>
        ///  Authorized person 
        /// </summary>
        public static string Auth
        {
            get
            {
                string val = GetValFromCookie("Auth", null, false);
                if (val == null)
                    val = GetSessionByKey("Auth", null);
                return val;
            }
            set
            {
                if (value == "")
                    SetSessionByKey("Auth", null);
                else
                    SetSessionByKey("Auth", value);
            }
        }
        public static string FK_DeptName
        {
            get
            {
                try
                {
                    string val = GetValFromCookie("FK_DeptName", null, true);
                    return val;
                }
                catch
                {
                    return "None";
                }
            }
            set
            {
                SetSessionByKey("FK_DeptName", value);
            }
        }
        /// <summary>
        ///  Department full name 
        /// </summary>
        public static string FK_DeptNameOfFull
        {
            get
            {
                string val = GetValFromCookie("FK_DeptNameOfFull", null, true);
                if (string.IsNullOrEmpty(val))
                {
                    try
                    {
                        val = DBAccess.RunSQLReturnStringIsNull("SELECT NameOfFull FROM Port_Dept WHERE No='" + WebUser.FK_Dept + "'", null);
                        return val;
                    }
                    catch
                    {
                        val = WebUser.FK_DeptName;
                    }

                    // Assigned to it .
                    FK_DeptNameOfFull = val;
                }
                return val;
            }
            set
            {
                SetSessionByKey("FK_DeptNameOfFull", value);
            }
        }
        public static string SysLang
        {
            get
            {
                return "CH";
                string no = GetSessionByKey("Lang", null);
                if (no == null || no == "")
                {
                    if (IsBSMode)
                    {
                        HttpCookie hc1 = BP.Sys.Glo.Request.Cookies["CCS"];
                        if (hc1 == null)
                            return "CH";
                        SetSessionByKey("Lang", hc1.Values["Lang"]);
                    }
                    else
                    {
                        return "CH";
                    }
                    return GetSessionByKey("Lang", "CH");
                }
                else
                {
                    return no;
                }
            }
            set
            {
                SetSessionByKey("Lang", value);
            }
        }
        /// <summary>
        /// sessionID
        /// </summary>
        public static string NoOfSessionID
        {
            get
            {
                string s = GetSessionByKey("No", null);
                if (s == null)
                    return System.Web.HttpContext.Current.Session.SessionID;
                return s;
            }
        }
        /// <summary>
        /// FK_Dept
        /// </summary>
        public static string FK_Dept
        {
            get
            {
                string val = GetValFromCookie("FK_Dept", null, false);
                if (val == null)
                    throw new Exception("@err-003 FK_Dept  Log information is lost , Make sure that the current operator of department information is complete , Checklist :Port_Emp Field FK_Dept.");
                return val;
            }
            set
            {
                SetSessionByKey("FK_Dept", value);
            }
        }
        /// <summary>
        ///  Current parent node TagLog staff 
        /// </summary>
        public static string DeptParentNo
        {
            get
            {
                string val = GetValFromCookie("DeptParentNo", null, false);
                if (val == null)
                    throw new Exception("@err-001 DeptParentNo  Log information is lost .");
                return val;
            }
            set
            {
                SetSessionByKey("DeptParentNo", value);
            }
        }
        /// <summary>
        ///  Check the access control 
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static bool CheckSID(string userNo, string sid)
        {
            string mysid = DBAccess.RunSQLReturnStringIsNull("SELECT SID FROM Port_Emp WHERE No='" + userNo + "'", null);
            if (sid == mysid)
                return true;
            else
                return false;
        }
        public static string NoOfRel
        {
            get
            {
                return GetSessionByKey("No", null);
            }
        }
        public static string GetValFromCookie(string valKey, string isNullAsVal, bool isChinese)
        {
            if (IsBSMode == false)
                return BP.Port.Win.Current.GetSessionStr(valKey, isNullAsVal);

            //if (System.Web.HttpContext.Current.Session == null)
            //    return isNullAsVal;

            try
            {
                // Start session Inside take .
                string v = System.Web.HttpContext.Current.Session[valKey] as string;
                if (string.IsNullOrEmpty(v) == false)
                    return v;
            }
            catch
            {
            }

            string key = "CCS";
            HttpCookie hc = BP.Sys.Glo.Request.Cookies[key];
            if (hc == null)
                return null;

            try
            {
                string val = null;
                if (isChinese)
                {
                    val = HttpUtility.UrlDecode(hc[valKey]);
                    if (val == null)
                        val = hc.Values[valKey];
                }
                else
                    val = hc.Values[valKey];

                if (string.IsNullOrEmpty(val))
                    return isNullAsVal;
                return val;
            }
            catch
            {
                return isNullAsVal;
            }
            throw new Exception("@err-001  Log information is lost .");
        }
        /// <summary>
        ///  Serial number 
        /// </summary>
        public static string No
        {
            get
            {
                string val = GetValFromCookie("No", null, true);
               return val;
                   
                string no = null; // GetSessionByKey("No", null);
                if (no == null || no == "")
                {
                    if (IsBSMode == false)
                        return "admin";
                    //return "admin";
                    //string key = "CCS";
                    string key = "CCS";


                    HttpCookie hc = BP.Sys.Glo.Request.Cookies[key];
                    if (hc == null)
                        return null;

                    if (hc.Values["No"] != null)
                    {
                        WebUser.No = hc["No"];
                        WebUser.FK_Dept = hc["FK_Dept"];
                        WebUser.Auth = hc["Auth"];
                        WebUser.FK_DeptName = HttpUtility.UrlDecode(hc["FK_DeptName"]);
                        WebUser.Name = HttpUtility.UrlDecode(hc["Name"]);
                     

                        //if (BP.Sys.SystemConfig.IsUnit)
                        //{
                        //    WebUser.FK_Unit = HttpUtility.UrlDecode(hc["FK_Unit"]);
                        //    WebUser.FK_UnitName = HttpUtility.UrlDecode(hc["FK_UnitName"]);
                        //}

                        return hc.Values["No"];
                    }
                    throw new Exception("@err-001 No  Log information is lost .");
                }
                return no;
            }
            set
            {
                SetSessionByKey("No", value);
            }
        }
        /// <summary>
        ///  Name 
        /// </summary>
        public static string Name
        {
            get
            {
                string val = GetValFromCookie("Name", null, true);
                if (val == null)
                    throw new Exception("@err-002 Name  Log information is lost .");
                return val;
            }
            set
            {
                SetSessionByKey("Name", value);
            }
        }
        /// <summary>
        /// Óò
        /// </summary>
        public static string Domain
        {
            get
            {
                string val = GetValFromCookie("Domain", null, true);
                if (val == null)
                    throw new Exception("@err-003 Domain  Log information is lost .");
                return val;
            }
            set
            {
                SetSessionByKey("Domain", value);
            }
        }
        /// <summary>
        ///  Token 
        /// </summary>
        public static string Token
        {
            get
            {

                return GetSessionByKey("token", "null");
            }
            set
            {
                SetSessionByKey("token", value);
            }
        }
        public static string Style
        {
            get
            {
                return GetSessionByKey("Style", "0");
            }
            set
            {
                SetSessionByKey("Style", value);
            }
        }
        /// <summary>
        ///  The current staff of the entity 
        /// </summary>
        public static Emp HisEmp
        {
            get
            {
                return new Emp(WebUser.No);
            }
        }
        public static Stations HisStations
        {
            get
            {
                object obj = null;
                obj = GetSessionByKey("HisSts", obj);
                if (obj == null)
                {
                    Stations sts = new Stations();
                    QueryObject qo = new QueryObject(sts);
                    qo.AddWhereInSQL("No", "SELECT FK_Station FROM Port_EmpStation WHERE FK_Emp='" + WebUser.No + "'");
                    qo.DoQuery();
                    SetSessionByKey("HisSts", sts);
                    return sts;
                }
                return obj as Stations;
            }
            set
            {
                SetSessionByKey("HisSts", value);
            }
        }
        public static Depts HisDepts
        {
            get
            {
                object obj = null;
                obj = GetSessionByKey("HisDepts", obj);
                if (obj == null)
                {
                    Depts sts = WebUser.HisEmp.HisDepts;
                    SetSessionByKey("HisDepts", sts);
                    return sts;
                }
                return obj as Depts;
            }
            set
            {
                SetSessionByKey("HisDepts", value);
            }
        }
        /// <summary>
        /// SID
        /// </summary>
        public static string SID
        {
            get
            {
                string val = GetValFromCookie("SID", null, true);
                if (val == null)
                    return "";
                return val;
            }
            set
            {
                SetSessionByKey("SID", value);
            }
        }
        /// <summary>
        ///  Set up SID
        /// </summary>
        /// <param name="sid"></param>
        public static void SetSID(string sid)
        {
            Paras ps = new Paras();
            ps.SQL = "UPDATE Port_Emp SET SID=" + SystemConfig.AppCenterDBVarStr + "SID WHERE No=" + SystemConfig.AppCenterDBVarStr + "No";
            ps.Add("SID", sid);
            ps.Add("No", WebUser.No);
            BP.DA.DBAccess.RunSQL(ps);
            WebUser.SID = sid;
        }
        /// <summary>
        ///  Whether it is authorized to state 
        /// </summary> 
        public static bool IsAuthorize
        {
            get
            {
                if (Auth == null || Auth == "")
                    return false;
                return true;
            }
        }
        /// <summary>
        ///  Use the authorized person ID
        /// </summary>
        public static string AuthorizerEmpID
        {
            get
            {
                return (string)GetSessionByKey("AuthorizerEmpID", null);

            }
            set
            {
                SetSessionByKey("AuthorizerEmpID", value);
            }
        }
        /// <summary>
        /// IsWap
        /// </summary>
        public static bool IsWap
        {
            get
            {
                if (BP.Sys.SystemConfig.IsBSsystem == false)
                    return false;
                int s = (int)GetSessionByKey("IsWap", 9);
                if (s == 9)
                {
                    bool b = BP.Sys.Glo.Request.RawUrl.ToLower().Contains("wap");
                    IsWap = b;
                    if (b)
                    {
                        SetSessionByKey("IsWap", 1);
                    }
                    else
                    {
                        SetSessionByKey("IsWap", 0);
                    }
                    return b;
                }
                if (s == 1)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    SetSessionByKey("IsWap", 1);
                else
                    SetSessionByKey("IsWap", 0);
            }
        }

        #region  Department Permissions 
        /// <summary>
        ///  Department Permissions 
        /// </summary>
        public static Depts HisDeptsOfPower
        {
            get
            {
                EmpDepts eds = new EmpDepts();
                return eds.GetHisDepts(WebUser.No);
            }
        }
        #endregion  Department Permissions 

        #region  The current method of operation staff .
        public static void DeleteTempFileOfMy()
        {
            HttpCookie hc = BP.Sys.Glo.Request.Cookies["CCS"];
            if (hc == null)
                return;
            string usr = hc.Values["No"];
            string[] strs = System.IO.Directory.GetFileSystemEntries(SystemConfig.PathOfTemp);
            foreach (string str in strs)
            {
                if (str.IndexOf(usr) == -1)
                    continue;

                try
                {
                    System.IO.File.Delete(str);
                }
                catch
                {
                }
            }
        }
        public static void DeleteTempFileOfAll()
        {
            string[] strs = System.IO.Directory.GetFileSystemEntries(SystemConfig.PathOfTemp);
            foreach (string str in strs)
            {
                try
                {
                    System.IO.File.Delete(str);
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}
