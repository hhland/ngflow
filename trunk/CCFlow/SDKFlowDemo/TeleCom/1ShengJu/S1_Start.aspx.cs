using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Demo;
using BP.Web;
using BP.Port;

namespace CCFlow.SDKFlowDemo.TelecomDemo.Parent
{
    public partial class S1_Start : System.Web.UI.Page
    {
        #region  Variable process engine came .
        /// <summary>
        ///  The work ID, In establishing the draft has produced .
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["WorkID"]);
            }
        }
        /// <summary>
        ///  Process ID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return Int64.Parse(this.Request.QueryString["FID"]);
            }
        }
        /// <summary>
        ///   Process ID .
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        /// <summary>
        ///  The current node ID
        /// </summary>
        public int FK_Node
        {
            get
            {
                return int.Parse(this.Request.QueryString["FK_Node"]);
            }
        }
        #endregion  Variable process engine came 

        public void CheckPhysicsTable()
        {
            BP.Demo.tab_wf_commonkpiopti tab_wf_commonkpiopti = new BP.Demo.tab_wf_commonkpiopti();
            tab_wf_commonkpiopti.CheckPhysicsTable();

            BP.Demo.tab_wf_commonkpiopti_main aa = new BP.Demo.tab_wf_commonkpiopti_main();
            aa.CheckPhysicsTable();

            BP.Demo.tab_wf_commonkpioptivalue bb = new BP.Demo.tab_wf_commonkpioptivalue();
            bb.CheckPhysicsTable();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check the physical table .
            CheckPhysicsTable();

            if (BP.Web.WebUser.No == null)
                throw new Exception("@ Login information is lost .");

            //  First, according to WorkID Get  tab_wf_commonkpiopti_main  Data ,
            tab_wf_commonkpiopti_main main = new tab_wf_commonkpiopti_main();
            main.WorkID = this.WorkID;
            if (main.Retrieve(tab_wf_commonkpiopti_mainAttr.WorkID,this.WorkID) == 0)
            {
                /* Description not find this data , The database should be made to insert a corresponding process data records .*/
                main.wf_title = " Test :WF_title ";
                main.wf_send_time = DataType.CurrentDataCNOfShort;
                main.wf_send_phone = "18660153393"; // User departments .
                main.wf_send_department = WebUser.FK_Dept; // User departments .
                main.wf_send_user = WebUser.No;
                main.wf_no = "Bill" + DateTime.Now.ToString("yyyyMMddHH"); /* Document Number */
                //  There are other values are not set , Set them according to the business situation .
                //main.Insert(); /* Implementation of the main flow to insert a */
            }
            else
            {
                /* 
                 *  Here the information you want to modify some of the changes , Such as : Document number or the sender phone , Transmission time .
                 *  There may be this person n Days before the start of a draft quit , Some fields are based on the current environmental changes varies .
                 */
                main.wf_title = " Test :wf_title ";
                main.wf_send_time = DataType.CurrentDataCNOfShort;
                main.wf_send_phone = "18660153393"; // User departments .
                main.wf_send_department = WebUser.FK_Dept; // User departments .
                main.wf_send_user = WebUser.No;
                main.wf_no = "Bill" + DateTime.Now.ToString("yyyyMMddHH"); /* Document Number */
            }
        }
        /// <summary>
        ///  Binding Information Form 
        /// </summary>
        /// <param name="main"></param>
        public void BindSheetInfo(tab_wf_commonkpiopti_main main)
        {
            this.TB_wf_send_time.Text = main.wf_send_time; /* Start Time */
            this.TB_wf_no.Text = main.wf_no;  /* Document Number */

            this.TB_FaQiRen.Text = main.wf_send_user;
            this.TB_ZBName.Text = main.techology; /*  Technical Information , Here to assign for each field 
                                                   *  Including from table from the table and from the table .
                                                   */
        }
        /// <summary>
        ///  Performing transmission 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Send_Click(object sender, EventArgs e)
        {
            // Call the Save method .
            Btn_Save_Click(null, null);

            //  Find out the main table data has been saved .
             tab_wf_commonkpiopti_main tab_wf_commonkpiopti_main = new BP.Demo.tab_wf_commonkpiopti_main();
            tab_wf_commonkpiopti_main.Retrieve(tab_wf_commonkpiopti_mainAttr.WorkID, this.WorkID);

            //  Hair current work , Let him send to , A node up the main thread .
            string msg = BP.WF.Dev2Interface.Node_SendWork(this.FK_Flow, this.WorkID).ToMsgOfHtml();

            // To start the task PUC ,  Check out the collection , Collection of the form .
             tab_wf_commonkpioptis tab_wf_commonkpioptis = new tab_wf_commonkpioptis();
            tab_wf_commonkpioptis.Retrieve(tab_wf_commonkpioptiAttr.tab_wf_commonkpiopti_main, 
                tab_wf_commonkpiopti_main.OID);

            //  PUC traverse this collection .
            foreach (tab_wf_commonkpiopti tab_wf_commonkpiopti in tab_wf_commonkpioptis)
            {
                //  Calling   Create a blank , To generate a start node PUC work to be done , And accept it WorkID.
                Int64 subFlowWorkID = BP.WF.Dev2Interface.Node_CreateBlankWork("026", null,
                    null, tab_wf_commonkpiopti.wf_send_user, " Automatically initiates the task :" + WebUser.No, this.WorkID,this.FK_Flow,0,null);

                //  Assigned to the sub-processes WorkID.
                tab_wf_commonkpiopti.WorkID = subFlowWorkID;
                tab_wf_commonkpiopti.ParentWorkID = this.WorkID;
                tab_wf_commonkpiopti.Update();

                //  Carried out sql  Equipment updates  ParentWorkID .
                string sql = "UPDATE tab_wf_commonkpioptivalue SET ParentWorkID=" + subFlowWorkID + " WHERE wf_commonkpioptivalue_id=" + tab_wf_commonkpiopti.OID;
                DBAccess.RunSQL(sql);

                msg += "@ Subprocess  -  PUC :" + tab_wf_commonkpiopti.region_id + " Has started , Task has been sent to the " + tab_wf_commonkpiopti.wf_send_user + "  Deal with  .";
            }

            //  Here one should turn to interface to display information .
            this.Session["info"] = msg;
            this.Response.Redirect("ShowMsg.aspx?ss=" + DataType.CurrentDataTime, true);
        }
        /// <summary>
        ///  Save the work order execution 
        /// 1, Write to the provincial bureau of business master data table .
        /// 2, PUC ticket to write data to a temporary table .
        /// 3, Ticket temporary table to the device to write data .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Save_Click(object sender, EventArgs e)
        {
            /* 
             *  Save Description :  Write data to the provincial bureau of the main table ,  Here with En30 Framework to describe the business logic implementation steps . 
             *  You can use sql, Or use the data in their own way  tab_wf_commonkpiopti_main Exterior and interior .
             */

            #region  First step :  Save the main table data .
            //  Create a blank data .
            tab_wf_commonkpiopti_main mainFlow = new  tab_wf_commonkpiopti_main();

            //  According to WorkID  Check out this data ,  Implementation of this method , You can get the main construction OID,
            //  Will be able to check out a data , Because page_load Has been judged .
             mainFlow.Retrieve(tab_wf_commonkpiopti_mainAttr.WorkID, this.WorkID); /* Check out the data .*/

            // First, give the master table ,  Gives basic information about the process . 
            mainFlow.WorkID = this.WorkID;
            mainFlow.fk_flow = this.FK_Flow; //  Process ID 
            mainFlow.wf_category= "02"; //  Process category number .

            mainFlow.wf_title = " Custom Process title :"+DataType.CurrentData+" , "+WebUser.No; //  Process title 
            mainFlow.wf_no = this.TB_wf_no.Text; //  Document Number 
            mainFlow.wf_send_user = WebUser.No; //  Current operator .
            mainFlow.wf_send_department = WebUser.FK_Dept; //  Current operating personnel department .
            mainFlow.wf_send_phone = "18660153393"; // Current operator phone 
            mainFlow.wf_send_time = DataType.CurrentDataTime; //  Current time .
            //  Secondly, to give other business field assignment .
            mainFlow.techology = this.TB_ZBName.Text; /* Index information .*/

            // The last update to the database ,  Complete data is written to the main table .
            int i= mainFlow.Update(); //  Perform an update 
            if (i == 0)
                mainFlow.Insert();
            #endregion  Save the main table data .


            #region  The second step :  Clear targets from the table data from the data from the table ,  Maybe you approach and our approach different .
            tab_wf_commonkpioptis shiJus = new tab_wf_commonkpioptis();
            shiJus.Retrieve(tab_wf_commonkpioptiAttr.tab_wf_commonkpiopti_main, mainFlow.OID); //  Clear PUC data .
            foreach (tab_wf_commonkpiopti shiju in shiJus)
            {
                // Delete the Council following device information .
                 tab_wf_commonkpioptivalue shebeiEn = new  tab_wf_commonkpioptivalue();
                shebeiEn.Delete(tab_wf_commonkpioptivalueAttr.wf_commonkpioptivalue_id, shiju.OID);
                
                // Remove the PUC data .
                shiju.Delete();
            }
            #endregion  Clear targets from the table data from the data from the table .


            #region  The third step :  Save the data from the table .( PUC data )
            // new  PUC data .
            tab_wf_commonkpiopti shijuEn = new  tab_wf_commonkpiopti();

            //  Setting basic information 
            shijuEn.WorkID = this.WorkID;
            shijuEn.tab_wf_commonkpiopti_main = mainFlow.OID; //  Primary key association 
            shijuEn.fk_flow = this.FK_Flow;
            shijuEn.wf_no = "111-222-3333"; // Document Number 
            shijuEn.WorkID = 0; //  The time has not yet produced WorkID
            shijuEn.wf_send_user = "zhoutianjiao"; //  Sub-thread processors .

            // Set of business data .
            shijuEn.region_id = " Jinan ";

            //  The PUC data - Inserted into the database .
            shijuEn.Insert();
            #endregion  The third step :  Save the data from the table . ( PUC data )


            #region  The fourth step :  Save data from Table .( PUC - Device Data )
            //  The first generation of Jinan 1 A device information ,  This information is placed in the child thread to go , new  A device information .
            tab_wf_commonkpioptivalue shebei = new  tab_wf_commonkpioptivalue();

            //  Give him an assignment   -  Process information field 
            shebei.wf_commonkpioptivalue_id = shijuEn.OID; // Associated with the primary key 
            shebei.fk_flow =this.FK_Flow;
            shebei.WorkID =0; //  The time has not yet produced WorkID
           // shebei.fid = 0; //  The time has not yet produced fid
            shebei.fuzeren = "guobaogeng"; //  Staff now specify the next node 

            //  Give him an assignment   -  Business Field .
            shebei.addr = " Jinan High-tech Zone xx路xx号";
            shebei.remark = "abc-abc";

            //  Inserted into the database .
            shebei.Insert();

            //  In establishing a device information .
            shebei = new tab_wf_commonkpioptivalue();
            //  Give him an assignment   -  Process information field 
            shebei.wf_commonkpioptivalue_id = shijuEn.OID; // Associated with the primary key 
            shebei.fk_flow = this.FK_Flow;
            shebei.WorkID = 0; //  The time has not yet produced WorkID
           // shebei.fid = 0; //  The time has not yet produced fid
            shebei.fuzeren = "fuhui"; //  Staff now specify the next node 

            //  Give him an assignment   -  Business Field .
            shebei.addr = " Jinan Licheng District xx路xx号";
            shebei.remark = "abc-123";

            //  Inserted into the database .
            shebei.Insert();

            #endregion  Save data from Table . ( PUC - Device Data )

            if (sender == null)
                this.Response.Write(" Saved successfully ");
        }

        protected void Btn_Chat_Click(object sender, EventArgs e)
        {
            BP.WF.Dev2Interface.UI_Window_FlowChart(this.FK_Flow);
        }
    }
}