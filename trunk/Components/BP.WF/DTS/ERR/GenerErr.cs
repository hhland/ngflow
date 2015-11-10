using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;
namespace BP.WF
{
    /// <summary>
    ///  Regenerate title   The summary 
    /// </summary>
    public class GenerTitle : Method
    {
        /// <summary>
        ///  Method with no arguments 
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
                return true;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            BP.WF.Ext.FlowSheets ens = new Ext.FlowSheets();
            foreach (BP.WF.Ext.FlowSheet en in ens)
            {
                en.DoGenerTitle();
            }
            return " Successful implementation ...";
        }
    }
}
