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
    public class ClearDB : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public ClearDB()
        {
            this.Title = " Data purge process to run ( This function is to run in a test environment )";
            this.Help = " Clear all processes running data , Including the work to be done .";
            this.Warning = " This function is to be performed in a test environment , Confirmed that it is a test environment ?";
            this.Icon = "<img src='/WF/Img/Btn/Delete.gif'  border=0 />";
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

            //DA.DBAccess.RunSQL("DELETE FROM WF_CHOfFlow");

            DA.DBAccess.RunSQL("DELETE FROM WF_Bill");
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerWorkerlist");
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerWorkFlow");
            DA.DBAccess.RunSQL("DELETE FROM WF_ReturnWork");
            DA.DBAccess.RunSQL("DELETE FROM WF_GenerFH");
            DA.DBAccess.RunSQL("DELETE FROM WF_SelectAccper");
            DA.DBAccess.RunSQL("DELETE FROM WF_TransferCustom");
            DA.DBAccess.RunSQL("DELETE FROM WF_RememberMe");
            DA.DBAccess.RunSQL("DELETE FROM Sys_FrmAttachmentDB");
            DA.DBAccess.RunSQL("DELETE FROM WF_CCList");

            Flows fls = new Flows();
            fls.RetrieveAll();
            foreach (Flow item in fls)
            {
                try
                {
                    DA.DBAccess.RunSQL("DELETE FROM ND" + int.Parse(item.No) + "Track");
                }
                catch
                {
                }
            }

            Nodes nds = new Nodes();
            foreach (Node nd in nds)
            {
                try
                {
                    Work wk = nd.HisWork;
                    DA.DBAccess.RunSQL("DELETE FROM " + wk.EnMap.PhysicsTable);
                }
                catch
                {
                }
            }

            MapDatas mds = new MapDatas();
            mds.RetrieveAll();
            foreach (MapData nd in mds)
            {
                try
                {
                    DA.DBAccess.RunSQL("DELETE FROM " + nd.PTable);
                }
                catch
                {
                }
            }

            MapDtls dtls = new MapDtls();
            dtls.RetrieveAll();
            foreach (MapDtl dtl in dtls)
            {
                try
                {
                    DA.DBAccess.RunSQL("DELETE FROM " + dtl.PTable);
                }
                catch
                {
                }
            }
            return " Successful implementation ...";
        }
    }
}
