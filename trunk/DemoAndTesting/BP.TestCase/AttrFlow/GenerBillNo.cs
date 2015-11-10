using System;
using System.Collections.Generic;
using System.Text;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Web;
using System.Data;
using System.Collections;
using BP.CT;

namespace BP.CT.Model
{
    /// <summary>
    ///  Generated document number 
    /// </summary>
    public class GenerBillNo : TestBase
    {
        /// <summary>
        ///  Generated document number 
        /// </summary>
        public GenerBillNo()
        {
            this.Title = " Generated document number ";
            this.DescIt = " Process : 以demo  Process 023  Document number, for example test generation .";
            this.EditState = CT.EditState.Editing;
        }

        #region  Global Variables 
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string fk_flow = "";
        /// <summary>
        ///  User ID 
        /// </summary>
        public string userNo = "";
        /// <summary>
        ///  All processes 
        /// </summary>
        public Flow fl = null;
        /// <summary>
        ///  The main thread ID
        /// </summary>
        public Int64 workID = 0;
        /// <summary>
        ///  After sending the return object 
        /// </summary>
        public SendReturnObjs objs = null;
        /// <summary>
        ///  Staff List 
        /// </summary>
        public GenerWorkerList gwl = null;
        /// <summary>
        ///  Process Registry 
        /// </summary>
        public GenerWorkFlow gwf = null;
        #endregion  Variable 

        /// <summary>
        ///  Test Case Description :
        /// 1,  Different formats , Generate different numbers .
        /// </summary>
        public override void Do()
        {
            this.fk_flow = "023";
            fl = new Flow("023");
            string sUser = "zhoupeng";
            BP.WF.Dev2Interface.Port_Login(sUser);

            // Deleting Data .
            fl.DoDelData();

            // Create .
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fl.No, null, null, WebUser.No, null, 0, null, 0, null);

            // Define Document Format .
            string billFormat = "CN{yyyy}-{MM}-{dd}IDX-{LSH4}";
            fl.BillNoFormat = billFormat;
            fl.Update();

            // Performing transmission .
            SendReturnObjs objs = BP.WF.Dev2Interface.Node_SendWork(fl.No, workID);

            //  Should be .
            billFormat = billFormat.Replace("{yyyy}", DateTime.Now.ToString("yyyy"));
            billFormat = billFormat.Replace("{MM}", DateTime.Now.ToString("MM"));
            billFormat = billFormat.Replace("{dd}", DateTime.Now.ToString("dd"));
            billFormat = billFormat.Replace("{LSH4}", "0001");

            GenerWorkFlow gwf = new GenerWorkFlow(workID);
            if (gwf.BillNo != billFormat)
                throw new Exception("@ It should be :" + billFormat + " It is :" + gwf.BillNo);

            //  Generating sub-processes .
            Flow flSub = new Flow("024");
            flSub.DoCheck();
            flSub.CheckRpt();

            //  Produce sub-process ID .
            for (int i = 1; i < 5; i++)
            {
                flSub.BillNoFormat = "{ParentBillNo}-{LSH4}";
                Int64 subWorkID = BP.WF.Dev2Interface.Node_CreateBlankWork(flSub.No, null, null,
                    WebUser.No, null, workID, "023",0,null);
                objs = BP.WF.Dev2Interface.Node_SendWork(flSub.No, subWorkID);

                // Setting process information .
                BP.WF.Dev2Interface.SetParentInfo(flSub.No, subWorkID, "023", workID,0,null);
                if (i == 2)
                    continue;

                string subFlowBillNo = DBAccess.RunSQLReturnStringIsNull("SELECT BillNo FROM " + flSub.PTable + " WHERE OID=" + subWorkID, "");
                if (subFlowBillNo != billFormat + "-000" + i)
                    throw new Exception("@ It should be :" + billFormat + "-000" + i + " ,  It is :" + subFlowBillNo);
            }

        }
    }
}
