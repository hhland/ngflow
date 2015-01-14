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
    ///  Garbage data generated template  
    /// </summary>
    public class GenerDBTemplete : Method
    {
        /// <summary>
        ///  Garbage data generated template 
        /// </summary>
        public GenerDBTemplete()
        {
            this.Title = " Garbage data generated template ";
            this.Help = " You can view and clear them manually .";
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

            MapDatas mds = new MapDatas();
            mds.RetrieveAll();

            string msg = "";
            Node nd = new Node();
            foreach (MapData item in mds)
            {
                if (item.No.Contains("ND") == false)
                    continue;

                string temp = item.No.Replace("ND", "");
                int nodeID = 0;
                try
                {
                    nodeID = int.Parse(temp);
                }
                catch
                {
                    continue;
                }

                nd.NodeID = nodeID;
                if (nd.IsExits == false)
                {
                    msg += "@" + item.No + "," + item.Name;
                    // Remove the stencil .
                    item.Delete();
                }
            }
            if (msg == "")
                msg = " Good data .";
            else
                msg = " Following nodes have been removed , However, the form data is not deleted node ." + msg;

            return msg;
        }
    }
}
