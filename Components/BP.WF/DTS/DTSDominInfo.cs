using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;
// using Security.Principal.WindowsIdentity;

namespace BP.WF.DTS
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class DTSDominInfo : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public DTSDominInfo()
        {
            this.Title = " Domain data generation ";
            this.Help = " Domain data generation ( Unfinished )";
           // this.HisAttrs.AddTBString("Path", "C:\\ccflow.Template", " Path generation ", true, false, 1, 1900, 200);
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            return " Function not implemented .";

            string domainHost = "127.0.0.1";

            string sqls = "";
            sqls += "@DELETE FROM Port_Emp";
            sqls += "@DELETE FROM Port_Dept";
            sqls += "@DELETE FROM Port_Station";
            sqls += "@DELETE FROM Port_EmpStation";
            sqls += "@DELETE FROM Port_EmpDept";
            DBAccess.RunSQLs(sqls);

           
            //  Importing go inside the department .

            //DirectoryEntry de = new DirectoryEntry("LDAP://" + domain, name, pass);
            //DirectorySearcher srch = new DirectorySearcher();
            //srch.Filter = ("(objectclass=User)");

            //srch.SearchRoot = de;
            //srch.SearchScope = SearchScope.Subtree;
            //srch.PropertiesToLoad.Add("sn");
            //srch.PropertiesToLoad.Add("givenName");
            //srch.PropertiesToLoad.Add("uid");
            //srch.PropertiesToLoad.Add("telephoneNumber");
            //srch.PropertiesToLoad.Add("employeeNumber");
            //foreach (SearchResult res in srch.FindAll())
            //{
            //    string[] strArray;
            //    string str;
            //    str = "";
            //    strArray = res.Path.Split(',');
            //    for (int j = strArray.Length; j > 0; j--)
            //    {
            //        if (strArray[j - 1].Substring(0, 3) == "OU=")
            //        {
            //            str = "©¸" + strArray[j - 1].Replace("OU=", "");
            //        }
            //    }
            //}

            return " Generate success , Please open  .<br> If you want to share them, please send to compressed template£Àccflow.org";
        }
    }
}
