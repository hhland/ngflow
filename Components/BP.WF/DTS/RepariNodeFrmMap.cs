using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.Sys;
using BP.En;
namespace BP.WF.DTS
{
    /// <summary>
    ///  Repair Node Form map  The summary 
    /// </summary>
    public class RepariNodeFrmMap : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public RepariNodeFrmMap()
        {
            this.Title = " Repair Node Form ";
            this.Help = " Check whether the nodes form system fields are illegally removed , If illegally removed automatically increase its , These fields include :Rec,Title,OID,FID,WFState,RDT,CDT";
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
            Nodes nds = new Nodes();
            nds.RetrieveAllFromDBSource();

            string info = "";
            foreach (Node nd in nds)
            {
                string msg = nd.RepareMap();
                if (msg != "")
                    info += "<b> For the process " + nd.FlowName + ", Node (" + nd.NodeID + ")(" + nd.Name + "),  Check the results are as follows :</b>" + msg + "<hr>";
            }
            return info + " Execution is completed ...";
        }
    }
}
