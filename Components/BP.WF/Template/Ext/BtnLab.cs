using System;
using System.Collections;
using BP.DA;
using BP.Sys;
using BP.En;
using BP.WF.Port;

namespace BP.WF.Template
{
    /// <summary>
    /// Btn
    /// </summary>
    public class BtnLab : Entity
    {
        /// <summary>
        /// but
        /// </summary>
        public static string Btns
        {
            get
            {
                return "Send,Save,Thread,Return,CC,Shift,Del,Rpt,Ath,Track,Opt,EndFlow,SubFlow";
            }
        }
        /// <summary>
        /// PK
        /// </summary>
        public override string PK
        {
            get
            {
                return NodeAttr.NodeID;
            }
        }

        #region  Basic properties 
        /// <summary>
        ///  Node ID
        /// </summary>
        public int NodeID
        {
            get
            {
                return this.GetValIntByKey(BtnAttr.NodeID);
            }
            set
            {
                this.SetValByKey(BtnAttr.NodeID, value);
            }
        }
        /// <summary>
        ///  Querytags 
        /// </summary>
        public string SearchLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SearchLab);
            }
            set
            {
                this.SetValByKey(BtnAttr.SearchLab, value);
            }
        }
        /// <summary>
        ///  Check availability 
        /// </summary>
        public bool SearchEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.SearchEnable);
            }
            set
            {
                this.SetValByKey(BtnAttr.SearchEnable, value);
            }
        }
        /// <summary>
        ///  Transfer 
        /// </summary>
        public string ShiftLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.ShiftLab);
            }
            set
            {
                this.SetValByKey(BtnAttr.ShiftLab, value);
            }
        }
        /// <summary>
        ///  Whether the transfer is enabled 
        /// </summary>
        public bool ShiftEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.ShiftEnable);
            }
            set
            {
                this.SetValByKey(BtnAttr.ShiftEnable, value);
            }
        }
        /// <summary>
        ///  Select recipient 
        /// </summary>
        public string SelectAccepterLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SelectAccepterLab);
            }
        }
        /// <summary>
        ///  Select the recipient type 
        /// </summary>
        public int SelectAccepterEnable
        {
            get
            {
                return this.GetValIntByKey(BtnAttr.SelectAccepterEnable);
            }
        }
        /// <summary>
        ///  Save 
        /// </summary>
        public string SaveLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SaveLab);
            }
        }
        /// <summary>
        ///  Whether saving enabled 
        /// </summary>
        public bool SaveEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.SaveEnable);
            }
        }
        /// <summary>
        ///  Child thread button labels 
        /// </summary>
        public string ThreadLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.ThreadLab);
            }
        }
        /// <summary>
        ///  Child thread button is enabled 
        /// </summary>
        public bool ThreadEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.ThreadEnable);
            }
        }

        /// <summary>
        ///  Subprocess button labels 
        /// </summary>
        public string SubFlowLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SubFlowLab);
            }
        }
        /// <summary>
        ///  Subprocess button is enabled 
        /// </summary>
        public SubFlowCtrlRole SubFlowCtrlRole
        {
            get
            {
                return (SubFlowCtrlRole)this.GetValIntByKey(BtnAttr.SubFlowCtrlRole);
            }
        }
        /// <summary>
        ///  Jump label 
        /// </summary>
        public string JumpWayLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.JumpWayLab);
            }
        }
        public JumpWay JumpWayEnum
        {
            get
            {
                return (JumpWay)this.GetValIntByKey(NodeAttr.JumpWay);
            }
        }
        /// <summary>
        ///  Whether Jump enabled 
        /// </summary>
        public bool JumpWayEnable
        {
            get
            {
                return this.GetValBooleanByKey(NodeAttr.JumpWay);
            }
        }
        /// <summary>
        ///  Return label 
        /// </summary>
        public string ReturnLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.ReturnLab);
            }
        }
        /// <summary>
        ///  Returned to the field 
        /// </summary>
        public string ReturnField
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.ReturnField);
            }
        }
        /// <summary>
        ///  Jump is enabled 
        /// </summary>
        public bool ReturnEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.ReturnRole);
            }
        }
        /// <summary>
        ///  Hang tag 
        /// </summary>
        public string HungLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.HungLab);
            }
        }
        /// <summary>
        ///  Whether to enable hang 
        /// </summary>
        public bool HungEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.HungEnable);
            }
        }
        /// <summary>
        ///  Print labels 
        /// </summary>
        public string PrintDocLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.PrintDocLab);
            }
        }
        /// <summary>
        ///  Whether printing is enabled 
        /// </summary>
        public bool PrintDocEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.PrintDocEnable);
            }
        }
        /// <summary>
        ///  Send labels 
        /// </summary>
        public string SendLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SendLab);
            }
        }
        /// <summary>
        ///  Whether to send Enabled ?
        /// </summary>
        public bool SendEnable
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        ///  Sent Js Code 
        /// </summary>
        public string SendJS
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.SendJS).Replace("~", "'");
            }
        }
        /// <summary>
        ///  Locus tag 
        /// </summary>
        public string TrackLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.TrackLab);
            }
        }
        /// <summary>
        ///  Whether the track is enabled 
        /// </summary>
        public bool TrackEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.TrackEnable);
            }
        }
        /// <summary>
        ///  Cc Label 
        /// </summary>
        public string CCLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.CCLab);
            }
        }
        /// <summary>
        ///  CC rules 
        /// </summary>
        public CCRole CCRole
        {
            get
            {
                return (CCRole)this.GetValIntByKey(BtnAttr.CCRole);
            }
        }
        /// <summary>
        ///  Remove label 
        /// </summary>
        public string DeleteLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.DelLab);
            }
        }
        /// <summary>
        ///  Delete Type 
        /// </summary>
        public int DeleteEnable
        {
            get
            {
                return this.GetValIntByKey(BtnAttr.DelEnable);
            }
        }
        /// <summary>
        ///  End Process 
        /// </summary>
        public string EndFlowLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.EndFlowLab);
            }
        }
        /// <summary>
        ///  Whether to enable the end of the process 
        /// </summary>
        public bool EndFlowEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.EndFlowEnable);
            }
        }
          /// <summary>
        ///  Whether circulation Custom enabled 
        /// </summary>
        public string TCLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.TCLab);
            }
        }
        /// <summary>
        ///  Whether circulation Custom enabled 
        /// </summary>
        public bool TCEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.TCEnable);
            }
            set
            {
                this.SetValByKey(BtnAttr.TCEnable, value);
            }
        }
        
        /// <summary>
        ///  Audit tag 
        /// </summary>
        public string WorkCheckLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.WorkCheckLab);
            }
        }
        /// <summary>
        ///  Audit is available 
        /// </summary>
        public bool WorkCheckEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.WorkCheckEnable);
            }
            set
            {
                this.SetValByKey(BtnAttr.WorkCheckEnable, value);
            }
        }
        /// <summary>
        ///  Batch processing is available 
        /// </summary>
        public bool BatchEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.BatchEnable);
            }
        }
        /// <summary>
        ///  Batch Label 
        /// </summary>
        public string BatchLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.BatchLab);
            }
        }
        /// <summary>
        ///  Plus sign 
        /// </summary>
        public bool AskforEnable
        {
            get
            {
                return this.GetValBooleanByKey(BtnAttr.AskforEnable);
            }
        }
        /// <summary>
        ///  Plus sign 
        /// </summary>
        public string AskforLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.AskforLab);
            }
        }
        /// <summary>
        /// Whether the document is enabled 
        /// </summary>
        public int WebOfficeEnable
        {
            get
            {
                return this.GetValIntByKey(BtnAttr.WebOfficeEnable);
            }
        }
        /// <summary>
        ///  Document button labels 
        /// </summary>
        public string WebOfficeLab
        {
            get
            {
                return this.GetValStringByKey(BtnAttr.WebOfficeLab);
            }
        }
        /// <summary>
        ///  Open local files 
        /// </summary>
        public bool OfficeOpenEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeOpenEnable); }
        }
        /// <summary>
        ///  Open the local label       
        /// </summary>
        public string OfficeOpenLab
        {
            get { return this.GetValStrByKey(BtnAttr.OfficeOpen); }
        }
        /// <summary>
        ///  Open the template 
        /// </summary>
        public bool OfficeOpenTemplateEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeOpenTemplateEnable); }
        }
        /// <summary>
        ///  Open the template tag 
        /// </summary>
        public string OfficeOpenTemplateLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeOpenTemplate); }
        }
        /// <summary>
        ///  Save button 
        /// </summary>
        public bool OfficeSaveEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeSaveEnable); }
        }
        /// <summary>
        ///  Save the label 
        /// </summary>
        public string OfficeSaveLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeSave); }
        }
        /// <summary>
        ///  Accept Change 
        /// </summary>
        public bool OfficeAcceptEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeAcceptEnable); }
        }
        /// <summary>
        ///  Accept the revised label 
        /// </summary>
        public string OfficeAcceptLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeAccept); }
        }
        /// <summary>
        ///  Reject Changes 
        /// </summary>
        public bool OfficeRefuseEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeRefuseEnable); }
        }
        /// <summary>
        ///  Reject Changes Label 
        /// </summary>
        public string OfficeRefuseLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeRefuse); }
        }
        /// <summary>
        ///  Whether Taohong 
        /// </summary>
        public bool OfficeOverEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeOverEnable); }
        }
        /// <summary>
        ///  Tao Hong button labels 
        /// </summary>
        public string OfficeOVerLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeOver); }
        }
        /// <summary>
        ///  Whether to print 
        /// </summary>
        public bool OfficePrintEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficePrintEnable); }
        }
        /// <summary>
        ///  View whether a user traces 
        /// </summary>
        public bool OfficeMarks
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeMarks); }
        }
        /// <summary>
        ///  Print button labels 
        /// </summary>
        public string OfficePrintLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficePrint); }
        }
        /// <summary>
        ///  Is read-only 
        /// </summary>
        public bool OfficeReadOnly
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeReadOnly); }
        }

        /// <summary>
        ///  Signature button 
        /// </summary>
        public bool OfficeSealEnable
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeSealEnabel); }
        }
        /// <summary>
        ///  Signature label 
        /// </summary>
        public string OfficeSealLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeSeal); }
        }

        /// <summary>
        /// Insertion process 
        /// </summary>
        public bool OfficeInsertFlowEnabel
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeInsertFlowEnabel); }
        }
        /// <summary>
        ///  Process tag 
        /// </summary>
        public string OfficeInsertFlowLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeInsertFlow); }
        }

        /// <summary>
        ///  Risk insertion point button 
        /// </summary>
        public bool OfficeInsertFengXianEnabel
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeInsertFengXianEnabel); }
        }
        /// <summary>
        ///  Risk insertion point labels 
        /// </summary>
        public string OfficeInsertFengXianLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeInsertFengXian); }
        }
        /// <summary>
        ///  Whether to automatically record the node information 
        /// </summary>
        public bool OfficeNodeInfo
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeNodeInfo); }
        }

        /// <summary>
        ///  Whether to automatically record the node information 
        /// </summary>
        public bool OfficeReSavePDF
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeReSavePDF); }
        }

        /// <summary>
        ///  Whether to enter the traces mode 
        /// </summary>
        public bool OfficeIsMarks
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeIsMarks); }
        }

        /// <summary>
        ///  Download button labels 
        /// </summary>
        public String OfficeDownLab
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeDownLab); }
        }

        /// <summary>
        ///  Whether to download enabled 
        /// </summary>
        public bool OfficeIsDown
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeIsDown); }
        }

        /// <summary>
        ///  Specify the document template 
        /// </summary>
        public String OfficeTemplate
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeTemplate); }
        }


        /// <summary>
        ///  Risk point template 
        /// </summary>
        public String OfficeFengXianTemplate
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeFengXianTemplate); }
        }

        /// <summary>
        ///  Whether the parent process documentation 
        /// </summary>
        public bool OfficeIsParent
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeIsParent); }
        }

        /// <summary>
        ///  Whether automatic Taohong 
        /// </summary>
        public bool OfficeIsTrueTH
        {
            get { return this.GetValBooleanByKey(BtnAttr.OfficeIsTrueTH); }
        }
        /// <summary>
        ///  Tao Hong template automatically 
        /// </summary>
        public string OfficeTHTemplate
        {
            get { return this.GetValStringByKey(BtnAttr.OfficeTHTemplate); }
        }

        #endregion

        #region  Constructor 
        /// <summary>
        /// Btn
        /// </summary>
        public BtnLab() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeid"></param>
        public BtnLab(int nodeid)
        {
            this.NodeID = nodeid;
            this.RetrieveFromDBSources();
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_Node");
                map.EnDesc = " Node label ";

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBIntPK(BtnAttr.NodeID, 0, "NodeID", true, false);

                map.AddTBString(BtnAttr.SendLab, " Send ", " Send button labels ", true, false, 0, 200, 10);
                map.AddTBString(BtnAttr.SendJS, "", " Push button JS Function ", true, false, 0, 200, 10);

                //map.AddBoolean(BtnAttr.SendEnable, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.JumpWayLab, " Jump ", " Jump button labels ", true, false, 0, 200, 10);
                map.AddBoolean(NodeAttr.JumpWay, false, " Whether to enable ", true, true);


                map.AddTBString(BtnAttr.SaveLab, " Save ", " Save button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.SaveEnable, true, " Whether to enable ", true, true);


                map.AddTBString(BtnAttr.ThreadLab, " Child thread ", " Child thread button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.ThreadEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.SubFlowLab, " Subprocess ", " Subprocess button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.SubFlowCtrlRole, 0, " Control rules ", true, true, BtnAttr.SubFlowCtrlRole, "@0=None@1= You can not delete sub-processes @2= You can delete the sub-processes ");


                map.AddTBString(BtnAttr.ReturnLab, " Return ", " Return button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.ReturnRole, true, " Whether to enable ", true, true);
                map.AddTBString(BtnAttr.ReturnField, "", " Return information to fill in the fields ", true, false, 0, 200, 10, true);


                map.AddTBString(BtnAttr.CCLab, " Cc ", " Cc button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.CCRole, 0, " CC rules ", true, true, BtnAttr.CCRole);

                //  map.AddBoolean(BtnAttr, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.ShiftLab, " Transfer ", " Transfer button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.ShiftEnable, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.DelLab, " Delete Process ", " Delete Process button label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.DelEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.EndFlowLab, " End Process ", " End Process button label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.EndFlowEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.HungLab, " Pending ", " Suspend button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.HungEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.PrintDocLab, " Printing documents ", " Print Documents button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.PrintDocEnable, false, " Whether to enable ", true, true);

                //map.AddTBString(BtnAttr.AthLab, " Accessory ", " Accessories button labels ", true, false, 0, 200, 10);
                //map.AddBoolean(BtnAttr.FJOpen, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.TrackLab, " Locus ", " Track button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.TrackEnable, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.SelectAccepterLab, " Recipient ", " Recipient button labels ", true, false, 0, 200, 10);
                map.AddDDLSysEnum(BtnAttr.SelectAccepterEnable, 0, " The way ",
          true, true, BtnAttr.SelectAccepterEnable);

                // map.AddBoolean(BtnAttr.SelectAccepterEnable, false, " Whether to enable ", true, true);
                //map.AddTBString(BtnAttr.OptLab, " Options ", " Options button labels ", true, false, 0, 200, 10);
                //map.AddBoolean(BtnAttr.OptEnable, true, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.SearchLab, " Inquiry ", " Query button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.SearchEnable, true, " Whether to enable ", true, true);

                // 
                map.AddTBString(BtnAttr.WorkCheckLab, " Check ", " Audit button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.WorkCheckEnable, false, " Whether to enable ", true, true);

                // 
                map.AddTBString(BtnAttr.BatchLab, " Batch review ", " Batch audit labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.BatchEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.AskforLab, " Plus sign ", " Endorsement label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.AskforEnable, false, " Whether to enable ", true, true);

                // add by stone 2014-11-21.  Allows users to define their own circulation .
                map.AddTBString(BtnAttr.TCLab, " Circulation Custom ", " Circulation Custom ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.TCEnable, false, " Whether to enable ", true, true);


                map.AddTBString(BtnAttr.WebOfficeLab, " Document ", " Official label ", true, false, 0, 200, 10);
                //map.AddBoolean(BtnAttr.WebOfficeEnable, false, " Whether to enable ", true, true);
                map.AddDDLSysEnum(BtnAttr.WebOfficeEnable, 0, " Activating document ", true, true, BtnAttr.WebOfficeEnable,
                 "@0= Not enabled @1= Button mode @2= Tab way ");

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

                map.AddTBString(BtnAttr.OfficeOver, " Tao Hong ", " Tao Hong tag ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeOverEnable, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeMarks, true, " View whether a user traces ", true, true);
                map.AddBoolean(BtnAttr.OfficeReadOnly, false, " Is read-only ", true, true);

                map.AddTBString(BtnAttr.OfficePrint, " Print ", " Print labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficePrintEnable, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeSeal, " Signature ", " Signature label ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeSealEnabel, false, " Whether to enable ", true, true);

                map.AddTBString(BtnAttr.OfficeInsertFlow, " Insertion process ", " Insertion process tag ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeInsertFlowEnabel, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeNodeInfo, false, " Whether the record node information ", true, true);
                map.AddBoolean(BtnAttr.OfficeReSavePDF, false, " Whether the automatically saved as PDF", true, true);


                map.AddTBString(BtnAttr.OfficeDownLab, " Download ", " Download button labels ", true, false, 0, 200, 10);
                map.AddBoolean(BtnAttr.OfficeIsDown, false, " Whether to enable ", true, true);

                map.AddBoolean(BtnAttr.OfficeIsMarks, true, " Whether to enter the traces mode ", true, true);
                map.AddTBString(BtnAttr.OfficeTemplate, "", " Specify the document template ", true, false, 0, 100, 10);
                map.AddBoolean(BtnAttr.OfficeIsParent, true, " Whether the parent process documentation ", true, true);

                if (Glo.IsEnableZhiDu)
                {
                    map.AddTBString(BtnAttr.OfficeFengXianTemplate, "", " Risk point template ", true, false, 0, 100, 10);

                    map.AddTBString(BtnAttr.OfficeInsertFengXian, " Risk insertion point ", " Risk insertion point labels ", true, false, 0, 200, 10);
                    map.AddBoolean(BtnAttr.OfficeInsertFengXianEnabel, false, " Whether to enable ", true, true);
                }
                map.AddBoolean(BtnAttr.OfficeIsTrueTH, false, " Whether automatic Taohong ", true, true);
                map.AddTBString(BtnAttr.OfficeTHTemplate, "", " Tao Hong template automatically ", true, false, 0, 200, 10);

                #endregion

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    /// Btn
    /// </summary>
    public class BtnLabs : Entities
    {
        /// <summary>
        /// Btn
        /// </summary>
        public BtnLabs()
        {
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new BtnLab();
            }
        }
    }
}
