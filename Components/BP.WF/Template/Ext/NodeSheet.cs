using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.Sys;
using System.Collections;
using BP.Port;

namespace BP.WF.Template.Ext
{
    /// <summary>
    ///  Here each node to store information .
    /// </summary>
    public class NodeSheet : Entity
    {
        #region Index
        /// <summary>
        ///  Get help with node url
        /// <para></para>
        /// <para>added by liuxc,2014-8-19</para> 
        /// </summary>
        /// <param name="sysNo"> Help website belongs System No</param>
        /// <param name="searchTitle"> Help Topic Title </param>
        /// <returns></returns>
        private string this[string sysNo, string searchTitle]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(sysNo) || string.IsNullOrWhiteSpace(searchTitle))
                    return "javascript:alert(' No help here !')";

                return string.Format("http://online.ccflow.org/KM/Tree.aspx?no={0}&st={1}", sysNo, Uri.EscapeDataString(searchTitle));
            }
        }
        #endregion

        #region Const
        /// <summary>
        /// CCFlow Process Engine 
        /// </summary>
        private const string SYS_CCFLOW = "001";
        /// <summary>
        /// CCForm Form Engine 
        /// </summary>
        private const string SYS_CCFORM = "002";
        #endregion

        #region  Property .
        ///// <summary>
        /////  Node labeled 
        ///// </summary>
        //public string NodeMark
        //{
        //    get
        //    {
        //        return this.GetValStrByKey(NodeAttr.NodeMark);
        //    }
        //}

        /// <summary>
        ///  Timeout handling 
        /// </summary>
        public OutTimeDeal HisOutTimeDeal
        {
            get
            {
                return (OutTimeDeal)this.GetValIntByKey(NodeAttr.OutTimeDeal);
            }
            set
            {
                this.SetValByKey(NodeAttr.OutTimeDeal, (int)value);
            }
        }
        /// <summary>
        ///  Access Rules 
        /// </summary>
        public ReturnRole HisReturnRole
        {
            get
            {
                return (ReturnRole)this.GetValIntByKey(NodeAttr.ReturnRole);
            }
            set
            {
                this.SetValByKey(NodeAttr.ReturnRole, (int)value);
            }
        }

        /// <summary>
        ///  Access Rules 
        /// </summary>
        public DeliveryWay HisDeliveryWay
        {
            get
            {
                return (DeliveryWay)this.GetValIntByKey(NodeAttr.DeliveryWay);
            }
            set
            {
                this.SetValByKey(NodeAttr.DeliveryWay, (int)value);
            }
        }
        public int Step
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.Step);
            }
            set
            {
                this.SetValByKey(NodeAttr.Step, value);
            }
        }
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(NodeAttr.NodeID);
            }
            set
            {
                this.SetValByKey(NodeAttr.NodeID, value);
            }
        }
        /// <summary>
        ///  Timeout processing content 
        /// </summary>
        public string DoOutTime
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.DoOutTime);
            }
            set
            {
                this.SetValByKey(NodeAttr.DoOutTime, value);
            }
        }
        /// <summary>
        ///  Timeout Handling Conditions 
        /// </summary>
        public string DoOutTimeCond
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.DoOutTimeCond);
            }
            set
            {
                this.SetValByKey(NodeAttr.DoOutTimeCond, value);
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.Name);
            }
            set
            {
                this.SetValByKey(NodeAttr.Name, value);
            }
        }
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(NodeAttr.FK_Flow, value);
            }
        }
        public string FlowName
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.FlowName);
            }
            set
            {
                this.SetValByKey(NodeAttr.FlowName, value);
            }
        }
        /// <summary>
        ///  Recipient sql
        /// </summary>
        public string DeliveryParas
        {
            get
            {
                return this.GetValStringByKey(NodeAttr.DeliveryParas);
            }
            set
            {
                this.SetValByKey(NodeAttr.DeliveryParas, value);
            }
        }
        /// <summary>
        ///  Can I return 
        /// </summary>
        public bool ReturnEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.ReturnRole);
            }
        }

        public override string PK
        {
            get
            {
                return "NodeID";
            }
        }
        #endregion  Property .

        #region  Preliminary examination of the global  Node
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                Flow fl = new Flow(this.FK_Flow);
                if (BP.Web.WebUser.No == "admin")
                    uac.IsUpdate = true;
                return uac;
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Node 
        /// </summary>
        public NodeSheet() { }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map();
                //map  Basic information .
                map.PhysicsTable = "WF_Node";
                map.EnDesc = " Node ";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;

                #region   Basic properties 
                map.AddTBIntPK(NodeAttr.NodeID, 0, " Node ID", true, true);
                map.AddTBInt(NodeAttr.Step, 0, " Step ( No calculation significance )", true, false);
                map.SetHelperAlert(NodeAttr.Step, " It is used to sort nodes , Correct setup steps can make the process easier to read and write ."); // Use alert Way to display help information .
                map.AddTBString(NodeAttr.FK_Flow, null, " Process ID ", false, false, 3, 3, 10, false);

                map.AddTBString(NodeAttr.Name, null, " Name ", true, true, 0, 200, 10, true);

                string str = "";
                str += "@0=01. According to the current operator organization structure step by step to find jobs ";
                str += "@1=02. Computing node bound by sector ";
                str += "@2=03. By setting SQL Get recipient computing ";
                str += "@3=04. Computing node bound by staff ";
                str += "@4=05. On a node by sending people through [ Personnel selector ] Select recipient ";
                str += "@5=06. Field value is specified by the form of a node as the recipient of this step ";
                str += "@6=07. On a node with the same personnel ";
                str += "@7=08. And began to deal with people the same node ";
                str += "@8=09. The same person with the specified node processing ";
                str += "@9=10. Job with the department is bound by the intersection of computing ";
                str += "@10=11. Calculated according to the binding posts and sector-bound set of latitude ";
                str += "@11=12. Press staff positions specified node computing ";
                str += "@12=13.按SQL Determine the child thread to accept people with a data source ";
                str += "@13=14. From the list on a node to determine the recipient of the child thread ";
                str += "@14=15. Only by calculating the binding posts ";
                str += "@15=16.由FEE To decide ";

                str += "@100=16.按ccflow的BPM Mode processing ";
                map.AddDDLSysEnum(NodeAttr.DeliveryWay, 0, " Node access rules ", true, true, NodeAttr.DeliveryWay,str);

                map.AddTBString(NodeAttr.DeliveryParas, null, " Access Rules set content ", true, false, 0, 500, 10, true);
                map.AddDDLSysEnum(NodeAttr.WhoExeIt, 0, " Who performed it ",true, true, NodeAttr.WhoExeIt, "@0= Operator performs @1= Machine to perform @2= Mixed execution ");
                map.AddDDLSysEnum(NodeAttr.TurnToDeal, 0, " After sending the steering ",
                 true, true, NodeAttr.TurnToDeal, "@0= Prompt ccflow Default information @1= Prompted to specify the information @2= Steering specified url@3= Accordance with the conditions steering ");
                map.AddTBString(NodeAttr.TurnToDealDoc, null, " Steering processing content ", true, false, 0, 1000, 10, true);
                map.AddDDLSysEnum(NodeAttr.ReadReceipts, 0, " Read Receipts ", true, true, NodeAttr.ReadReceipts,
                    "@0= No receipt @1= Automatic receipt @2= Determined by a node on a form field @3=由SDK Developers parameter determines ");
                map.SetHelperUrl(NodeAttr.ReadReceipts, this[SYS_CCFLOW, " Read Receipts "]);

                map.AddDDLSysEnum(NodeAttr.CondModel, 0, " Conditions direction control rules ", true, true, NodeAttr.CondModel,
                 "@0= Controlled by cable conditions @1= Allow users to manually select ");
                map.SetHelperUrl(NodeAttr.CondModel, this[SYS_CCFLOW, " Conditions direction control rules "]); // Help increase 

                //  Avoidance rules .
                map.AddDDLSysEnum(NodeAttr.CancelRole,(int)CancelRole.OnlyNextStep, " Avoidance rules ", true, true,
                    NodeAttr.CancelRole,"@0= You can undo the last step @1= Can not be undone @2= Back with the start node can be revoked @3= Specified node can be revoked ");

                //  Batch nodes work . edit by peng, 2014-01-24.
                map.AddDDLSysEnum(NodeAttr.BatchRole, (int)BatchRole.None, " Batch job ", true, true, NodeAttr.BatchRole, "@0= Not batch @1= Batch review @2= Grouping batches review ");
                map.SetHelperUrl(NodeAttr.BatchRole, this[SYS_CCFLOW, " Batch nodes work "]); // Help increase 
                map.AddTBString(NodeAttr.BatchParas, null, " Batch parameters ", true, false, 0, 300, 10, true);


                map.AddBoolean(NodeAttr.IsTask, true, " No work permit allocation ?", true, true, false);
                map.SetHelperBaidu(NodeAttr.IsTask); // Link to baidu Search for .
                map.AddBoolean(NodeAttr.IsRM, true, " Whether automatic memory function is enabled the delivery path ?", true, true, false);
                map.SetHelperBaidu(NodeAttr.IsRM); // Link to baidu Search for .

                map.AddTBDateTime("DTFrom", " Lifecycle from ", true, true);
                map.AddTBDateTime("DTTo", " Life cycle to ", true, true);
                #endregion   Basic properties 

                #region  Form .
                map.AddDDLSysEnum(NodeAttr.FormType, (int)NodeFormType.FixForm, " Nodes form type ", true, true,
                 "NodeFormType", "@0= Fool form @1= Freedom Form @2= Custom Form @3=SDK Form @4=SL Form ( Beta version )@5= Form tree @6= Official Forms (WebOffice)@7=Excel Form @9= Disable ( Form for more efficient processes )@10=Excel Form ( Beta )");
                map.AddTBString(NodeAttr.FormUrl, null, " Form URL", true, false, 0, 200, 10, true);

                map.AddTBString(NodeAttr.FocusField, null, " Focus field ", true, false, 0, 200, 10, true);
                map.SetHelperBaidu(NodeAttr.FocusField); // Link to baidu Search for .

                map.AddTBString(NodeAttr.NodeFrmID, null, " Node Form ID", true, false, 0, 200, 10);
                map.AddDDLSysEnum(NodeAttr.SaveModel, 0, " Save mode ", true, true);
                #endregion  Form .

                #region  Points confluence child thread property 
                map.AddDDLSysEnum(NodeAttr.RunModel, 0, " Run mode ",
                    true, true, NodeAttr.RunModel, "@0= General @1= Confluence @2= Bypass @3= Confluence points @4= Child thread ");
                map.SetHelperUrl(NodeAttr.RunModel, this[SYS_CCFLOW, " Run mode "]); // Help increase .

        
                
                // Child thread type .
                map.AddDDLSysEnum(NodeAttr.SubThreadType, 0, " Child thread type ", true, true, NodeAttr.SubThreadType, "@0= With Form @1= Different forms ");
                map.SetHelperUrl(NodeAttr.SubThreadType, this[SYS_CCFLOW, " Child thread type "]); // Help increase 


                map.AddTBDecimal(NodeAttr.PassRate, 0, " Completed by rate ", true, false);
                map.SetHelperUrl(NodeAttr.PassRate, this[SYS_CCFLOW, " Completed by rate "]); // Help increase .

                //  Promoter thread parameters  2013-01-04
                map.AddDDLSysEnum(NodeAttr.SubFlowStartWay, (int)SubFlowStartWay.None, " Child thread startup mode ", true, true,
                    NodeAttr.SubFlowStartWay, "@0= Does not start @1= The specified field to start @2= Start by schedule ");
                map.AddTBString(NodeAttr.SubFlowStartParas, null, " Startup Parameters ", true, false, 0, 200, 10, true);
                map.SetHelperUrl(NodeAttr.SubFlowStartWay, this[SYS_CCFLOW, " Child thread startup mode "]); // Help increase 

                // Upcoming processing mode .
                map.AddDDLSysEnum(NodeAttr.TodolistModel, (int)TodolistModel.QiangBan, " Upcoming processing mode ", true, true, NodeAttr.TodolistModel,
                    "@0= Office rush mode @1= Collaborative model @2= Queue Mode @3= Sharing Mode ");
                map.SetHelperUrl(NodeAttr.TodolistModel, this[SYS_CCFLOW, " Upcoming processing mode "]); // Help increase .

                ////  add 2013-09-14 
                //map.AddBoolean(NodeAttr.IsEnableTaskPool, true,
                //    " Whether to enable shared task pool (与web.config The IsEnableTaskPool Configuration is enabled to be effective , Nothing to do with the child thread )?", true, true, true);
                //map.SetHelperBaidu(NodeAttr.IsEnableTaskPool); // Help increase .
                map.AddBoolean(NodeAttr.IsCheckSubFlowOver, false, "( When the current node promoter process ) Check that all the sub-process after the end of , The node can send down ?",
                 true, true, true);

                map.AddBoolean(NodeAttr.IsAllowRepeatEmps, false, " Whether to allow the child to accept the staff duplicate threads ( Effective only when the diversion point to send the child thread )?", true, true, true);
                map.AddBoolean(NodeAttr.IsGuestNode, false, " Is the client node is executed ( Node personnel involved in the organizational structure of the non-working )?", true, true, true);
                #endregion  Points confluence child thread property 

                #region  Automatically jump Rules 
                map.AddBoolean(NodeAttr.AutoJumpRole0, false, " Who is the sponsor deal ", true, true, false);
                map.SetHelperUrl(NodeAttr.AutoJumpRole0, this[SYS_CCFLOW, " Automatically jump Rules "]); // Help increase 

                map.AddBoolean(NodeAttr.AutoJumpRole1, false, " Treatment have occurred ", true, true, false);
                map.AddBoolean(NodeAttr.AutoJumpRole2, false, " People with the same processing step ", true, true, false);
                map.AddDDLSysEnum(NodeAttr.WhenNoWorker, 0, " People can not find the handle processing rules ",
       true, true, NodeAttr.WhenNoWorker, "@0= An error @1= Automatically go to the next step ");
                #endregion

                #region   Function button state 
                map.AddTBString(BtnAttr.SendLab, " Send ", " Send button labels ", true, false, 0, 200, 10);
                map.AddTBString(BtnAttr.SendJS, "", " Push button JS Function ", true, false, 0, 200, 10);
                //map.SetHelperBaidu(BtnAttr.SendJS, "ccflow  Judgment before sending data integrity "); // Help increase .
                map.SetHelperUrl(BtnAttr.SendJS, this[SYS_CCFLOW, " Push button JS Function "]);

                map.AddTBString(BtnAttr.SaveLab, " Save ", " Save button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.SaveEnable, true, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.SaveLab, this[SYS_CCFLOW, " Save "]); // Help increase 

                map.AddTBString(BtnAttr.ThreadLab, " Child thread ", " Child thread button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.ThreadEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.ThreadLab, this[SYS_CCFLOW, " Child thread button labels "]); // Help increase 


                map.AddDDLSysEnum(NodeAttr.ThreadKillRole, (int)ThreadKillRole.None, " Delete the way the child thread ", true, true,
           NodeAttr.ThreadKillRole, "@0= You can not delete @1= Manually delete @2= Automatically deleted ",true);
                map.SetHelperUrl(NodeAttr.ThreadKillRole, this[SYS_CCFLOW, " Delete the way the child thread "]); // Help increase 
               

                map.AddTBString(BtnAttr.SubFlowLab, " Subprocess ", " Subprocess button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.SubFlowCtrlRole, 0, " Control rules ", true, true, BtnAttr.SubFlowCtrlRole, "@0=无@1= You can not delete sub-processes @2= You can delete the sub-processes ");

                map.AddTBString(BtnAttr.JumpWayLab, " Jump ", " Jump button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(NodeAttr.JumpWay, 0, " Jump Rules ", true, true, NodeAttr.JumpWay);
                map.AddTBString(NodeAttr.JumpToNodes, null, " Redirect node ", true, false, 0, 200, 10, true);
                map.SetHelperUrl(NodeAttr.JumpWay, this[SYS_CCFLOW, " Jump Rules "]); // Help increase .

                map.AddTBString(BtnAttr.ReturnLab, " Return ", " Return button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(NodeAttr.ReturnRole, 0," Return rules ",true, true, NodeAttr.ReturnRole);
              //  map.AddTBString(NodeAttr.ReturnToNodes, null, " Returnable node ", true, false, 0, 200, 10, true);
                map.SetHelperUrl(NodeAttr.ReturnRole, this[SYS_CCFLOW, "222"]); // Help increase .

                map.AddBoolean(NodeAttr.IsBackTracking, false, " Can backtrack ( Enabling return function is effective )", true, true, false);
                map.AddTBString(BtnAttr.ReturnField, "", " Return information to fill in the fields ", true, false, 0, 200, 10);
                map.SetHelperUrl(NodeAttr.IsBackTracking, this[SYS_CCFLOW, " Can backtrack "]); // Help increase .

                map.AddTBString(BtnAttr.CCLab, " Cc ", " Cc button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(NodeAttr.CCRole, 0, " CC rules ", true, true, NodeAttr.CCRole);
                map.SetHelperUrl(NodeAttr.CCRole, this[SYS_CCFLOW, " CC rules "]); // Help increase .

                // add 2014-04-05.
                map.AddDDLSysEnum(NodeAttr.CCWriteTo, 0, " Cc write rules ",
             true, true, NodeAttr.CCWriteTo, "@0= Write CC list @1= Write to-do @2= Write to-do list and CC ", true);
                map.SetHelperUrl(NodeAttr.CCWriteTo, this[SYS_CCFLOW, " Cc write rules "]); // Help increase 

                map.AddTBString(BtnAttr.ShiftLab, " Transfer ", " Transfer button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.ShiftEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.ShiftLab, this[SYS_CCFLOW, " Transfer "]); // Help increase .note:none

                map.AddTBString(BtnAttr.DelLab, " Delete ", " Delete button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.DelEnable, 0, " Delete Rule ", true, true, BtnAttr.DelEnable);
                map.SetHelperUrl(BtnAttr.DelLab, this[SYS_CCFLOW, " Delete "]); // Help increase .

                map.AddTBString(BtnAttr.EndFlowLab, " End Process ", " End Process button label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.EndFlowEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.EndFlowLab, this[SYS_CCFLOW, " End Process "]); // Help increase 

                map.AddTBString(BtnAttr.PrintDocLab, " Printing documents ", " Print Documents button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.PrintDocEnable, 0, " Printing Methods ", true,
                    true, BtnAttr.PrintDocEnable, "@0= Do not print @1= Print page @2= Print RTF Template @3= Print Word Stencil ");
                map.SetHelperUrl(BtnAttr.PrintDocEnable, this[SYS_CCFLOW, " Form Printing method "]); // Help increase 

                // map.AddBoolean(BtnAttr.PrintDocEnable, false, " Whether to enable ", true, true);
                //map.AddTBString(BtnAttr.AthLab, " Accessory ", " Accessories button labels ", true, false, 0, 200, 10);
                //map.AddDDLSysEnum(NodeAttr.FJOpen, 0, this.ToE("FJOpen", " Accessories Permissions "), true, true, 
                //    NodeAttr.FJOpen, "@0= Close Accessories @1= The operator @2= The work ID@3= Process ID");

                map.AddTBString(BtnAttr.TrackLab, " Locus ", " Track button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.TrackEnable, true, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.TrackLab, this[SYS_CCFLOW, " Locus "]); // Help increase 


                map.AddTBString(BtnAttr.HungLab, " Pending ", " Suspend button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.HungEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.HungLab, this[SYS_CCFLOW, " Pending "]); // Help increase .

                map.AddTBString(BtnAttr.SelectAccepterLab, " Recipient ", " Recipient button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.SelectAccepterEnable, 0, " Work ",
          true, true, BtnAttr.SelectAccepterEnable);
                map.SetHelperUrl(BtnAttr.SelectAccepterLab, this[SYS_CCFLOW, " Recipient "]); // Help increase 


                map.AddTBString(BtnAttr.SearchLab, " Inquiry ", " Query button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.SearchEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.SearchLab, this[SYS_CCFLOW, " Inquiry "]); // Help increase 


                map.AddTBString(BtnAttr.WorkCheckLab, " Check ", " Audit button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.WorkCheckEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.BatchLab, " Batch ", " Batch button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.BatchEnable, false, " Whether to enable ", true, true);
                map.SetHelperUrl(BtnAttr.BatchLab, this[SYS_CCFLOW, " Batch "]); // Help increase 

                map.AddTBString(BtnAttr.AskforLab, " Plus sign ", " Plus sign button label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.AskforEnable, false, " Whether to enable ", true, true);

                // add by  Zhou Peng  2014-11-21.  Allows users to define their own circulation .
                map.AddTBString(BtnAttr.TCLab, " Circulation Custom ", " Circulation Custom ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.TCEnable, false, " Whether to enable ", true, true);



                //map.AddTBString(BtnAttr.AskforLabRe, " Carried out ", " Plus sign button label ", true, false, 0, 200, 10);
                //map.AddBoolean(BtnAttr.AskforEnable, false, " Whether to enable ", true, true);

                map.SetHelperUrl(BtnAttr.AskforLab, this[SYS_CCFLOW, " Plus sign "]); // Help increase 
                map.AddTBString(BtnAttr.WebOfficeLab, " Document ", " Document button labels ", true, false, 0, 200, 10);
                //  map.AddBoolean(BtnAttr.WebOfficeEnable, false, " Whether to enable ", true, true);
                map.AddDDLSysEnum(BtnAttr.WebOfficeEnable, 0, " Activating document ", true, true, BtnAttr.WebOfficeEnable,
                  "@0= Not enabled @1= Button mode @2= Tab way ");
                map.SetHelperUrl(BtnAttr.WebOfficeLab, this[SYS_CCFLOW, " Document "]);

                //map.AddBoolean(BtnAttr.SelectAccepterEnable, false, " Whether to enable ", true, true);
                #endregion   Function button state 

                #region  Property assessment 
                //  Property assessment 
                map.AddTBFloat(NodeAttr.WarningDays, 0, " Warning period (0 Without warning )", true, false); // " Warning period (0 Without warning )"
                map.AddTBFloat(NodeAttr.DeductDays, 1, " Deadline (天)", true, false); //" Deadline (天)"
                map.AddTBFloat(NodeAttr.DeductCent, 2, " Deduction ( Each extension 1 Days buckle )", true, false); //" Deduction ( Each extension 1 Days buckle )"

                map.AddTBFloat(NodeAttr.MaxDeductCent, 0, " Maximum deduction ", true, false);   //" Maximum deduction "
                map.AddTBFloat(NodeAttr.SwinkCent, float.Parse("0.1"), " Working score ", true, false); //" Working score "
                map.AddDDLSysEnum(NodeAttr.OutTimeDeal, 0, " Timeout Handling ",
                true, true, NodeAttr.OutTimeDeal,
                "@0= Does not deal with @1= Automatic downward movement ( Or move to a specified node )@2= Automatically jump specified point @3= Automatically go to designated personnel @4= Message to the designated staff @5= Delete Process @6= Carried out SQL");

                map.AddTBString(NodeAttr.DoOutTime, null, " Processing content ", true, false, 0, 300, 10, true);
                map.AddTBString(NodeAttr.DoOutTimeCond, null, " Timeout condition ", true, false, 0, 200, 10, true);

                //map.AddTBString(NodeAttr.FK_Flows, null, "flow", false, false, 0, 200, 10);

                map.AddDDLSysEnum(NodeAttr.CHWay, 0, " Assessment methods ", true, true, NodeAttr.CHWay, "@0= No assessment @1= By age @2= According to the workload ");
                map.AddTBFloat(NodeAttr.Workload, 0, " Workload ( Unit : Minute )", true, false);

                //  Whether the quality assessment point ?
                map.AddBoolean(NodeAttr.IsEval, false, " Whether the quality assessment point ", true, true, true);
                #endregion  Property assessment 

                #region  Audit component properties ,  Here changed BP.Sys.FrmWorkCheck  Have changed .
                // BP.Sys.FrmWorkCheck

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCSta, (int)FrmWorkCheckSta.Disable, " Audit Component Status ",
                    true, true, FrmWorkCheckAttr.FWCSta, "@0= Disable @1= Enable @2= Read-only ");

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCShowModel, (int)FrmWorkShowModel.Free, " Display mode ",
                    true, true, FrmWorkCheckAttr.FWCShowModel, "@0= Tabular form @1= Free Mode "); // This property has no use .

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCType, (int)FWCType.Check, " Audit Components ", true, true, FrmWorkCheckAttr.FWCType, "@0= Audit Components @1= Logging component ");

                map.AddBoolean(FrmWorkCheckAttr.FWCTrackEnable, true, " Trajectories is displayed ?", true, true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCListEnable, true, " Historical audit information is displayed ?(否, Historical information appears only comments box )", true, true, true);

                map.AddBoolean(FrmWorkCheckAttr.FWCIsShowAllStep, false, " All the steps in the track list is displayed ?", true, true);

                map.AddTBString(FrmWorkCheckAttr.FWCOpLabel, " Check ", " Operating nouns ( Check / Review / Instructions )", true, false, 0, 200, 10);
                map.AddTBString(FrmWorkCheckAttr.FWCDefInfo, " Agree ", " Default audit information ", true, false, 0, 200, 10);
                map.AddBoolean(FrmWorkCheckAttr.SigantureEnabel, false, " The operator is displayed as a picture signature ?", true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCIsFullInfo, true, " If the user does not audit opinion is populated by default ?", true, true, true);

                //map.AddTBFloat(FrmWorkCheckAttr.FWC_X, 5, " Location X", true, false);
                //map.AddTBFloat(FrmWorkCheckAttr.FWC_Y, 5, " Location Y", true, false);


                map.AddTBFloat(FrmWorkCheckAttr.FWC_H, 300, " Height ", true, false);
                map.AddTBFloat(FrmWorkCheckAttr.FWC_W, 400, " Width ", true, false);
                #endregion  Audit component properties .

                #region  Document button 
                map.AddTBString(BtnAttr.OfficeOpen, " Open Local ", " Open the local label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeOpenEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeOpenTemplate, " Open the template ", " Open the template tag ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeOpenTemplateEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeSave, " Save ", " Save the label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeSaveEnable, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeAccept, " Accept Change ", " Accept the revised label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeAcceptEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeRefuse, " Reject Changes ", " Reject Changes Label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeRefuseEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeOver, " Tao Hong button ", " Tao Hong button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeOverEnable, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeMarks, true, " View whether a user traces ", true, true);
                map.AddBoolean(BtnAttr.OfficeReadOnly, false, " Is read-only ", true, true);

                map.AddTBString(BtnAttr.OfficePrint, " Print button ", " Print button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficePrintEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeSeal, " Signature button ", " Signature button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeSealEnabel, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeInsertFlow, " Insertion process ", " Insertion process tag ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeInsertFlowEnabel, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeNodeInfo, false, " Whether the record node information ", true, true);
                map.AddBoolean(BtnAttr.OfficeReSavePDF, false, " Whether the automatically saved as PDF", true, true);

                map.AddTBString(BtnAttr.OfficeDownLab, " Download ", " Download button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeIsDown, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeIsMarks, true, " Whether to enter the traces mode ", true, true);
                map.AddTBString(BtnAttr.OfficeTemplate, "", " Specify the document template ", true, false, 0, 200, 10);

                map.AddBoolean(BtnAttr.OfficeIsParent, true, " Whether the parent process documentation ", true, true);

                if (Glo.IsEnableZhiDu)
                {
                    map.AddTBString(BtnAttr.OfficeFengXianTemplate, "", " Risk point template ", true, false, 0, 200, 10);
                    map.AddTBString(BtnAttr.OfficeInsertFengXian, " Risk insertion point ", " Risk insertion point labels ", true, false, 0, 200, 10);
                    map.AddBoolean(BtnAttr.OfficeInsertFengXianEnabel, false, " Whether to enable ", true, true);
                }
                map.AddBoolean(BtnAttr.OfficeIsTrueTH, false, " Whether automatic Taohong ", true, true);
                map.AddTBString(BtnAttr.OfficeTHTemplate, "", " Tao Hong template automatically ", true, false, 0, 200, 10);
                #endregion

                #region  Mobile settings .
                map.AddDDLSysEnum(NodeAttr.MPhone_WorkModel, 0, " Phone mode ", true, true, NodeAttr.MPhone_WorkModel, "@0= The original ecology @1= Browser @2= Disable ");
                map.AddDDLSysEnum(NodeAttr.MPhone_SrcModel, 0, " Phone screen mode ", true, true, NodeAttr.MPhone_SrcModel, "@0= Forced horizontal screen @1= Forced portrait @2= Determined by the gravity sensor ");

                map.AddDDLSysEnum(NodeAttr.MPad_WorkModel, 0, " Tablet mode ", true, true, NodeAttr.MPad_WorkModel, "@0= The original ecology @1= Browser @2= Disable ");
                map.AddDDLSysEnum(NodeAttr.MPad_SrcModel, 0, " Flat-screen mode ", true, true, NodeAttr.MPad_SrcModel, "@0= Forced horizontal screen @1= Forced portrait @2= Determined by the gravity sensor ");
                map.SetHelperUrl(NodeAttr.MPhone_WorkModel, "http://bbs.ccflow.org/showtopic-2866.aspx");
                #endregion  Mobile settings .

                // Node Toolbar 
                map.AddDtl(new NodeToolbars(), NodeToolbarAttr.FK_Node);

                #region  Correspondence between 
                //  Related functions .
                if (Glo.OSModel == OSModel.WorkFlow)
                {
                    map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeStations(), new BP.WF.Port.Stations(),
                        NodeStationAttr.FK_Node, NodeStationAttr.FK_Station,
                        DeptAttr.Name, DeptAttr.No, " Node binding posts ");

                    // Determine whether the Group uses , Group opened a new page in a tree display 
                    if (Glo.IsUnit == true)
                    {
                        RefMethod rmDept = new RefMethod();
                        rmDept.Title = " Node binding department ";
                        rmDept.ClassMethodName = this.ToString() + ".DoDepts";
                        rmDept.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                        map.AddRefMethod(rmDept);
                    }
                    else
                    {
                        map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeDepts(), new BP.WF.Port.Depts(), NodeDeptAttr.FK_Node, NodeDeptAttr.FK_Dept, DeptAttr.Name,
            DeptAttr.No, " Node binding department ");
                    }
                }
                else
                {
                    // Node positions .
                    map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeStations(),
                        new BP.GPM.Stations(),
                      NodeStationAttr.FK_Node, NodeStationAttr.FK_Station,
                      DeptAttr.Name, DeptAttr.No, " Node binding posts ");
                    // Determine whether the Group uses , Group opened a new page in a tree display 
                    if (Glo.IsUnit == true)
                    {
                        RefMethod rmDept = new RefMethod();
                        rmDept.Title = " Node binding department ";
                        rmDept.ClassMethodName = this.ToString() + ".DoDepts";
                        rmDept.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                        map.AddRefMethod(rmDept);
                    }
                    else
                    {
                        // Node department .
                        map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeDepts(), new BP.GPM.Depts(),
                            NodeDeptAttr.FK_Node, NodeDeptAttr.FK_Dept, DeptAttr.Name,
            DeptAttr.No, " Node binding department ");
                    }
                }


                map.AttrsOfOneVSM.Add(new BP.WF.Template.NodeEmps(), new BP.WF.Port.Emps(), NodeEmpAttr.FK_Node, EmpDeptAttr.FK_Emp, DeptAttr.Name,
                    DeptAttr.No, " Node Binding recipient ");

                //  Sub-processes can be called a fool form . 2014.10.19  Remove .
                //map.AttrsOfOneVSM.Add(new BP.WF.NodeFlows(), new Flows(), NodeFlowAttr.FK_Node, NodeFlowAttr.FK_Flow, DeptAttr.Name, DeptAttr.No,
                //    " Subprocess fool form callable ");
                #endregion

                RefMethod rm = new RefMethod();
                rm.Title = " Returnable node ( When a rule is set to return returnable specified node , This setting is valid .)"; // " Design a Form ";
                rm.ClassMethodName = this.ToString() + ".DoCanReturnNodes";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.Visable = true;
                rm.RefMethodType = RefMethodType.LinkModel;
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.ReturnRole;
                rm.RefAttrLinkLabel = " Set returnable node ";
                rm.Target = "_blank";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Revocable sending node ( Only withdrawal rule is specified node can be revoked , This setting is valid .)"; // " Revocable node sends the ";
                rm.ClassMethodName = this.ToString() + ".DoCanCancelNodes";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.Visable = true;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.CancelRole;
                rm.RefAttrLinkLabel = "";
                rm.Target = "_blank";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Set up automatic CC rules ( When a node is automatically when CC , This setting is valid .)"; // " CC rules ";
                rm.ClassMethodName = this.ToString() + ".DoCCRole";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.CCRole;
                rm.RefAttrLinkLabel = " Cc automatic settings ";
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.Target = "_blank";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Form design fool ( When a node is set to fool Form Form Type , This setting is valid .)"; // " Design a Form ";
                rm.ClassMethodName = this.ToString() + ".DoFormCol4";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.SaveModel;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.Target = "_blank";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Form design freedom ( When a node is set to freely form form type , This setting is valid .)"; // " Design a Form ";
                rm.ClassMethodName = this.ToString() + ".DoFormFree";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.SaveModel;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.Target = "_blank";
                map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Binding Process Form ( When the node type is set to form a tree form , This setting is valid .)"; // " Design a Form ";
                rm.ClassMethodName = this.ToString() + ".DoFormTree";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.SaveModel;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.Target = "_blank";
                map.AddRefMethod(rm);
                

                rm = new RefMethod();
                rm.Title = " Binding rtf Print format template ( When printing method for printing RTF When the template format , This setting is valid )"; //" Invoice & Invoice ";
                rm.ClassMethodName = this.ToString() + ".DoBill";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/doc.gif";
                rm.RefMethodType = RefMethodType.LinkeWinOpen;

                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.PrintDocEnable;
                rm.RefAttrLinkLabel = "";
                rm.Target = "_blank";
                map.AddRefMethod(rm);
                if (BP.Sys.SystemConfig.CustomerNo == "HCBD")
                {
                    /*  For the individual needs of the sea into the Banda settings . */
                    rm = new RefMethod();
                    rm.Title = "DXReport Set up ";
                    rm.ClassMethodName = this.ToString() + ".DXReport";
                    rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/FileType/doc.gif";
                    map.AddRefMethod(rm);
                }

                rm = new RefMethod();
                rm.Title = " Set Event "; // " Call the event interface ";
                rm.ClassMethodName = this.ToString() + ".DoAction";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Event.png";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Push messages to the parties "; // " Call the event interface ";
                rm.ClassMethodName = this.ToString() + ".DoPush2Current";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Message24.png";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
              //  map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Push messages to the nominee "; // " Call the event interface ";
                rm.ClassMethodName = this.ToString() + ".DoPush2Spec";
              //  rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Message32.png";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
              //  map.AddRefMethod(rm);


                rm = new RefMethod();
                rm.Title = " Listen news "; // " Call the event interface ";
                rm.ClassMethodName = this.ToString() + ".DoListen";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Process closing conditions "; // " Process closing conditions ";
                rm.ClassMethodName = this.ToString() + ".DoCond";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                map.AddRefMethod(rm);
             

                rm = new RefMethod();
                rm.Title = " Send successful transition condition "; // " Steering condition ";
                rm.ClassMethodName = this.ToString() + ".DoTurn";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.TurnToDealDoc;
                rm.RefAttrLinkLabel = "";
                rm.Target = "_blank";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Set up [ Recipient selector ] Personnel selection range ."; // " Personalized recipient window "; //( Access rules set for the first 05 Term effective )
                rm.ClassMethodName = this.ToString() + ".DoAccepter";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                // Setting the relevant fields .
                rm.RefAttrKey = NodeAttr.DeliveryWay;
                rm.RefMethodType = RefMethodType.LinkeWinOpen;
                rm.RefAttrLinkLabel = "";
                rm.Target = "_blank";
                map.AddRefMethod(rm);

                if (Glo.OSModel==OSModel.BPM)
                {
                    rm = new RefMethod();
                    rm.Title = "BPM Mode setting rules recipient ";
                    rm.ClassMethodName = this.ToString() + ".DoAccepterRole";
                    rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";

                    // Setting the relevant fields .
                    //rm.RefAttrKey = NodeAttr.WhoExeIt;
                    rm.RefMethodType = RefMethodType.RightFrameOpen;
                    rm.RefAttrLinkLabel = "";
                    rm.Target = "_blank";
                    map.AddRefMethod(rm);
                }

                rm = new RefMethod();
                rm.Title = " Set permissions process tree form ";
                rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                rm.ClassMethodName = this.ToString() + ".DoNodeFormTree";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                map.AddRefMethod(rm);

                if (Glo.IsEnableZhiDu)
                {
                    rm = new RefMethod();
                    rm.Title = " Section corresponding system "; // " Personalized recipient window ";
                    rm.ClassMethodName = this.ToString() + ".DoZhiDu";
                    rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                    map.AddRefMethod(rm);

                    rm = new RefMethod();
                    rm.Title = " Risk points "; // " Personalized recipient window ";
                    rm.ClassMethodName = this.ToString() + ".DoFengXianDian";
                    rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                    map.AddRefMethod(rm);

                    rm = new RefMethod();
                    rm.Title = " Responsibilities "; // " Personalized recipient window ";
                    rm.ClassMethodName = this.ToString() + ".DoGangWeiZhiZe";
                    rm.Icon = BP.WF.Glo.CCFlowAppPath + "WF/Img/Btn/DTS.gif";
                    map.AddRefMethod(rm);
                }

                this._enMap = map;
                return this._enMap;
            }
        }
        /// <summary>
        ///  Group division tree 
        /// </summary>
        /// <returns></returns>
        public string DoDepts()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath + "WF/Comm/Port/DeptTree.aspx?s=d34&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&RefNo=" + DataType.CurrentDataTime, 500, 550);
            return null;
        }
        /// <summary>
        ///  Set permissions process tree form 
        /// </summary>
        /// <returns></returns>
        public string DoNodeFormTree()
        {
            return Glo.CCFlowAppPath + "WF/Admin/FlowFormTree.aspx?s=d34&FK_Flow=" + this.FK_Flow + "&FK_Node=" +
                   this.NodeID + "&RefNo=" + DataType.CurrentDataTime;
        }
        /// <summary>
        ///  System 
        /// </summary>
        /// <returns></returns>
        public string DoZhiDu()
        {
            PubClass.WinOpen(Glo.CCFlowAppPath + "ZhiDu/NodeZhiDuDtl.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow, " System ", "Bill", 700, 400, 200, 300);
            return null;
        }
        /// <summary>
        ///  Risk points 
        /// </summary>
        /// <returns></returns>
        public string DoFengXianDian()
        {
            // PubClass.WinOpen(Glo.CCFlowAppPath + "ZhiDu/NodeFengXianDian.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow, " System ", "Bill", 700, 400, 200, 300);
            return null;
        }
        /// <summary>
        ///  Someone Rules 
        /// </summary>
        /// <returns></returns>
        public string DoAccepterRole()
        {
            BP.WF.Node nd = new BP.WF.Node(this.NodeID);

            if (nd.HisDeliveryWay != DeliveryWay.ByCCFlowBPM)
                return " Node access rules you do not set in accordance with the bpm Mode , So you can do this . To do this, select a node in the node properties in accordance with the rules of access and choice bpm Model calculations , Point the Save button .";

            return Glo.CCFlowAppPath + "WF/Admin/FindWorker/List.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow; 
         //   return null;
        }
        public string DoTurn()
        {
            return Glo.CCFlowAppPath + "WF/Admin/TurnTo.aspx?FK_Node=" + this.NodeID;
            //, " Complete shift processing nodes ", "FrmTurn", 800, 500, 200, 300);
            //BP.WF.Node nd = new BP.WF.Node(this.NodeID);
            //return nd.DoTurn();
        }
        /// <summary>
        ///  CC rules 
        /// </summary>
        /// <returns></returns>
        public string DoCCRole()
        {
            return Glo.CCFlowAppPath + "WF/Comm/RefFunc/UIEn.aspx?EnName=BP.WF.Template.CC&PK=" + this.NodeID; 
            //PubClass.WinOpen("./RefFunc/UIEn.aspx?EnName=BP.WF.CC&PK=" + this.NodeID, " CC rules ", "Bill", 800, 500, 200, 300);
            //return null;
        }
        /// <summary>
        ///  Personalized recipient window 
        /// </summary>
        /// <returns></returns>
        public string DoAccepter()
        {
            return Glo.CCFlowAppPath + "WF/Comm/RefFunc/UIEn.aspx?EnName=BP.WF.Template.Selector&PK=" + this.NodeID;
            //return null;
        }
        /// <summary>
        ///  Return node 
        /// </summary>
        /// <returns></returns>
        public string DoCanReturnNodes()
        {
            return Glo.CCFlowAppPath + "WF/Admin/CanReturnNodes.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow;
        }
        /// <summary>
        ///  Revoked node sends the 
        /// </summary>
        /// <returns></returns>
        public string DoCanCancelNodes()
        {
            return Glo.CCFlowAppPath + "WF/Admin/CanCancelNodes.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow; 
        }
        /// <summary>
        /// DXReport
        /// </summary>
        /// <returns></returns>
        public string DXReport()
        {
            return Glo.CCFlowAppPath + "WF/Admin/DXReport.aspx?FK_Node=" + this.NodeID + "&FK_Flow=" + this.FK_Flow;
        }
        public string DoPush2Current()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Listen.aspx?CondType=0&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=";
        }
        public string DoPush2Spec()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Listen.aspx?CondType=0&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=";
        }
        /// <summary>
        ///  Listen to perform message 
        /// </summary>
        /// <returns></returns>
        public string DoListen()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Listen.aspx?CondType=0&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=";
        }
        public string DoFeatureSet()
        {
            return Glo.CCFlowAppPath + "WF/Admin/FeatureSetUI.aspx?CondType=0&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=";
        }
        public string DoShowSheets()
        {
            return Glo.CCFlowAppPath + "WF/Admin/ShowSheets.aspx?CondType=0&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=";
        }
        public string DoCond()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Condition.aspx?CondType=" + (int)CondType.Flow + "&FK_Flow=" + this.FK_Flow + "&FK_MainNode=" + this.NodeID + "&FK_Node=" + this.NodeID + "&FK_Attr=&DirType=&ToNodeID=" + this.NodeID;
        }
        /// <summary>
        ///  Form design fool 
        /// </summary>
        /// <returns></returns>
        public string DoFormCol4()
        {
            return Glo.CCFlowAppPath + "WF/MapDef/MapDef.aspx?PK=ND" + this.NodeID;
        }
        /// <summary>
        ///  Form design freedom 
        /// </summary>
        /// <returns></returns>
        public string DoFormFree()
        {
            return Glo.CCFlowAppPath + "WF/MapDef/CCForm/Frm.aspx?FK_MapData=ND" + this.NodeID + "&FK_Flow=" + this.FK_Flow;
        }
        /// <summary>
        ///  Binding Process Form 
        /// </summary>
        /// <returns></returns>
        public string DoFormTree()
        {
            return Glo.CCFlowAppPath + "WF/Admin/FlowFrms.aspx?ShowType=FlowFrms&FK_Flow=" + this.FK_Flow + "&FK_Node=" + this.NodeID + "&Lang=CH";
        }
        
        public string DoMapData()
        {
            int i = this.GetValIntByKey(NodeAttr.FormType);

            //  Type .
            NodeFormType type = (NodeFormType)i;
            switch (type)
            {
                case NodeFormType.FreeForm:
                    PubClass.WinOpen(Glo.CCFlowAppPath + "WF/MapDef/CCForm/Frm.aspx?FK_MapData=ND" + this.NodeID + "&FK_Flow=" + this.FK_Flow, " Design a Form ", "sheet", 1024, 768, 0, 0);
                    break;
                default:
                case NodeFormType.FixForm:
                    PubClass.WinOpen(Glo.CCFlowAppPath + "WF/MapDef/MapDef.aspx?PK=ND" + this.NodeID, " Design a Form ", "sheet", 800, 500, 210, 300);
                    break;
            }
            return null;
        }
        public string DoAction()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Action.aspx?NodeID=" + this.NodeID + "&FK_Flow=" + this.FK_Flow + "&tk=" + new Random().NextDouble();
        }
        /// <summary>
        ///  Document Printing 
        /// </summary>
        /// <returns></returns>
        public string DoBill()
        {
            return Glo.CCFlowAppPath + "WF/Admin/Bill.aspx?NodeID=" + this.NodeID + "&FK_Flow=" + this.FK_Flow;
        }
        /// <summary>
        ///  Set up 
        /// </summary>
        /// <returns></returns>
        public string DoFAppSet()
        {
            return Glo.CCFlowAppPath + "WF/Admin/FAppSet.aspx?NodeID=" + this.NodeID + "&FK_Flow=" + this.FK_Flow;
        }

        protected override bool beforeUpdate()
        {
            // Version update process 
            Flow.UpdateVer(this.FK_Flow);
           
            #region  // Get  NEE  Entity .
            //if (string.IsNullOrEmpty(this.NodeMark) == false)
            //{
            //    Flow fl = new Flow(this.FK_Flow);

            //    object obj = Glo.GetNodeEventEntityByNodeMark( fl.FlowMark, this.NodeMark);
            //    if (obj == null)
            //        throw new Exception("@ Node tag error : Did not find the node labeled (" + this.NodeMark + ") Entity nodes event .");
            //    this.NodeEventEntity = obj.ToString();
            //}
            //else
            //{
            //    this.NodeEventEntity = "";
            //}
            #endregion  Synchronization event entity 

            #region  Data processing nodes .
            Node nd = new Node(this.NodeID);
            if (nd.IsStartNode == true)
            {
                /* Deal with the problem button */
                // Can not be returned ,  Plus sign , Transfer , Return ,  Child thread .
                this.SetValByKey(BtnAttr.ReturnRole,(int)ReturnRole.CanNotReturn);
                this.SetValByKey(BtnAttr.HungEnable, false);
                this.SetValByKey(BtnAttr.ThreadEnable, false); // Child thread .
            }

            if (nd.HisRunModel == RunModel.HL || nd.HisRunModel == RunModel.FHL)
            {
                /* If it is the confluence point */
            }
            else
            {
                this.SetValByKey(BtnAttr.ThreadEnable, false); // Child thread .
            }
            #endregion  Data processing nodes .

            //#region  Processing the message parameter fields .
            //this.SetPara(NodeAttr.MsgCtrl, this.GetValIntByKey(NodeAttr.MsgCtrl));
            //this.SetPara(NodeAttr.MsgIsSend, this.GetValIntByKey(NodeAttr.MsgIsSend));
            //this.SetPara(NodeAttr.MsgIsReturn, this.GetValIntByKey(NodeAttr.MsgIsReturn));
            //this.SetPara(NodeAttr.MsgIsShift, this.GetValIntByKey(NodeAttr.MsgIsShift));
            //this.SetPara(NodeAttr.MsgIsCC, this.GetValIntByKey(NodeAttr.MsgIsCC));

            //this.SetPara(NodeAttr.MsgMailEnable, this.GetValIntByKey(NodeAttr.MsgMailEnable));
            //this.SetPara(NodeAttr.MsgMailTitle, this.GetValStrByKey(NodeAttr.MsgMailTitle));
            //this.SetPara(NodeAttr.MsgMailDoc, this.GetValStrByKey(NodeAttr.MsgMailDoc));

            //this.SetPara(NodeAttr.MsgSMSEnable, this.GetValIntByKey(NodeAttr.MsgSMSEnable));
            //this.SetPara(NodeAttr.MsgSMSDoc, this.GetValStrByKey(NodeAttr.MsgSMSDoc));
            //#endregion

            return base.beforeUpdate();
        }
        #endregion
    }
    /// <summary>
    ///  Set of nodes 
    /// </summary>
    public class NodeSheets : Entities
    {
        #region  Constructor 
        /// <summary>
        ///  Set of nodes 
        /// </summary>
        public NodeSheets()
        {
        }
        #endregion

        public override Entity GetNewEntity
        {
            get { return new NodeSheet(); }
        }
    }
}
