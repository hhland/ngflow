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
    public class DTSCheckFlowAll : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public DTSCheckFlowAll()
        {
            this.Title = " All medical procedures ";
            this.Help = " Can only function with the same individual physical processes , Examination process does not harm the data .";
            this.Help += "<br>1, Repair Node Form , Flow statements physical table .";
            this.Help += "<br>2, Generate pre-process data and computing nodes , In order to optimize process execution speed .";
            this.Help += "<br>3, Report data restoration process .";
            this.Help += "<br>4, The system does not prompt examination results .";
            this.Help += "<br>5, The length of time and number of physical processes , The number of nodes , How many form fields relationship , Please wait .";
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
            Flows fls = new Flows();
            fls.RetrieveAllFromDBSource();
            foreach (Flow fl in fls)
            {
                fl.DoCheck();
            }

            return " Prompt :"+fls.Count+" A process involved in the examination .";
        }
    }
}
