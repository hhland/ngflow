using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
namespace BP.WF.DTS
{
    /// <summary>
    ///  Repair Form physical table field length   The summary 
    /// </summary>
    public class ReLoadNDxxxxxxRpt : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public ReLoadNDxxxxxxRpt()
        {
            this.Title = " Clear and reload flow statements ";
            this.Help = " Delete NDxxxRpt Table data , Reload , This feature is estimated to be executed for a long time , If a large amount of data is possible in web Failure to perform on the program .";
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
                if (BP.Web.WebUser.No == "admin")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            string msg = "";
            msg+=Flow.RepareV_FlowData_View();

            Flows fls = new Flows();
            fls.RetrieveAllFromDBSource();
            foreach (Flow fl in fls)
            {
                try
                {
                    msg += fl.DoReloadRptData();
                }
                catch(Exception ex)
                {
                    msg += "@ In the process flow (" + fl.Name + ") Abnormal " + ex.Message;
                }
            }
            return " Prompt :"+fls.Count+" A process involved in the examination , The following information :@"+msg;
        }
    }
}
