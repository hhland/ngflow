using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;

namespace BP.WF.DTS
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class UpdateCCFlow : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public UpdateCCFlow()
        {
            this.Title = " Upgrade ccflow";
            this.Help = " Perform ccflow Upgrade , If you update down the latest code , You will need to perform this function , Carried on ccflow The database upgrade .";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
            //this.Warning = " Are you sure you want to perform ?";
            //HisAttrs.AddTBString("P1", null, " Old Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P2", null, " New Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P3", null, " Confirm ", true, false, 0, 10, 10);
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
            if (BP.Web.WebUser.No != "admin")
                return " Illegal users to perform .";

            BP.WF.Glo.UpdataCCFlowVer();
 
            return " Successful implementation , System has been fixed with the latest version of the database .";
        }
    }
}
