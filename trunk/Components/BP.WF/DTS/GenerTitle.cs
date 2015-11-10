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
    ///  Regenerate title 
    /// </summary>
    public class GenerTitle : Method
    {
        /// <summary>
        ///  Regenerate title 
        /// </summary>
        public GenerTitle()
        {
            this.Title = " Regenerate title £¨ For all processes , Title generation process under the new rules £©";
            this.Help = " You can also open the flow properties of one of the executed individually .";
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
                if (BP.Web.WebUser.No == "admin")
                    return true;
                return false;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            BP.WF.Template.Ext.FlowSheets ens = new BP.WF.Template.Ext.FlowSheets();
            foreach (BP.WF.Template.Ext.FlowSheet en in ens)
            {
                en.DoGenerTitle();
            }
            return " Successful implementation ...";
        }
    }
}
