using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Data;
using BP.DA;

namespace CCFlowWord2007.WorkFlow
{
    /// <summary>
    ///  Process Node 
    /// </summary>
    public class WFNode
    {
        public WFNode()
        { }
        #region Model

        /// <summary>
        /// 
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        ///  Process Steps 
        /// </summary>
        public int? Step { get; set; }

        /// <summary>
        ///  Name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  No work permit allocation ?
        /// </summary>
        public bool? IsTask { get; set; }
        /// <summary>
        ///  Whether the process can be forced to remove ( Confluence point for effective )
        /// </summary>
        public bool? IsForceKill { get; set; }
        /// <summary>
        ///  By Rate 
        /// </summary>
        public decimal? PassRate { get; set; }
        /// <summary>
        ///  Run mode ( Effective for ordinary nodes ), Enumerated type :0  General ;1  Confluence ;2  Bypass ;3  Confluence points ;
        /// </summary>
        public int? RunModel { get; set; }
        /// <summary>
        ///  Focus field 
        /// </summary>
        public string FocusField { get; set; }
        /// <summary>
        ///  Delivery Rules 
        /// </summary>
        public int? DeliveryWay { get; set; }
        /// <summary>
        ///  Recipient SQL
        /// </summary>
        public string RecipientSQL { get; set; }
        /// <summary>
        ///  Form type , Enumerated type :0  Fool form ;1  Freedom Form ;2  Custom Form ;3 SDK Form ;9  Disable ( Form for more efficient processes );
        /// </summary>
        public int? FormType { get; set; }
        /// <summary>
        ///  Form URL
        /// </summary>
        public string FormUrl { get; set; }
        /// <summary>
        ///  After the completion of treatment SQL
        /// </summary>
        public string DoWhat { get; set; }
        /// <summary>
        ///  Steering handle 
        /// </summary>
        public int? TurnToDeal { get; set; }
        /// <summary>
        ///  After sending the message 
        /// </summary>
        public string TurnToDealDoc { get; set; }
        /// <summary>
        ///  Lifecycle from 
        /// </summary>
        public string DTFrom { get; set; }
        /// <summary>
        ///  Life cycle to 
        /// </summary>
        public string DTTo { get; set; }
        /// <summary>
        ///  Send button labels 
        /// </summary>
        public string SendLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? SendEnable { get; set; }
        /// <summary>
        ///  Save button labels 
        /// </summary>
        public string SaveLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? SaveEnable { get; set; }
        /// <summary>
        ///  Return button labels 
        /// </summary>
        public string ReturnLab { get; set; }
        /// <summary>
        ///  Return rules 
        /// </summary>
        public ReturnRoleKind ReturnRole { get; set; }
        /// <summary>
        ///  Cc button labels 
        /// </summary>
        public string CCLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? CCEnable { get; set; }
        /// <summary>
        ///  Transfer button labels 
        /// </summary>
        public string ShiftLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? ShiftEnable { get; set; }
        /// <summary>
        ///  Delete Process button label 
        /// </summary>
        public string DelLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? DelEnable { get; set; }
        /// <summary>
        ///  End Process button label 
        /// </summary>
        public string EndFlowLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? EndFlowEnable { get; set; }
        /// <summary>
        ///  Report button labels 
        /// </summary>
        public string RptLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? RptEnable { get; set; }
        /// <summary>
        ///  Print Documents button labels 
        /// </summary>
        public string PrintDocLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? PrintDocEnable { get; set; }
        /// <summary>
        ///  Accessories button labels 
        /// </summary>
        public string AthLab { get; set; }
        /// <summary>
        ///  Accessories Permissions 
        /// </summary>
        public AttachmentRoleKind FJOpen { get; set; }
        /// <summary>
        ///  Track button labels 
        /// </summary>
        public string TrackLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? TrackEnable { get; set; }
        /// <summary>
        ///  Options button labels 
        /// </summary>
        public string OptLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? OptEnable { get; set; }
        /// <summary>
        ///  Recipient button labels 
        /// </summary>
        public string SelectAccepterLab { get; set; }
        /// <summary>
        ///  Whether to enable 
        /// </summary>
        public bool? SelectAccepterEnable { get; set; }
        /// <summary>
        ///  Warning period (0 Without warning )
        /// </summary>
        public int? WarningDays { get; set; }
        /// <summary>
        ///  Deadline (天)
        /// </summary>
        public int? DeductDays { get; set; }
        /// <summary>
        ///  Deduction ( Each extension 1 Days buckle )
        /// </summary>
        public decimal? DeductCent { get; set; }
        /// <summary>
        ///  Maximum deduction 
        /// </summary>
        public decimal? MaxDeductCent { get; set; }
        /// <summary>
        ///  Working score 
        /// </summary>
        public decimal? SwinkCent { get; set; }
        /// <summary>
        ///  Timeout Handling , Enumerated type :0  Does not deal with ;1  Automatically transferred to the next step ;2  Automatically go to designated personnel ;3  Send a message to the designated officer ;4  Delete Process ;5  Carried out SQL;
        /// </summary>
        public int? OutTimeDeal { get; set; }
        /// <summary>
        ///  Processing content 
        /// </summary>
        public string DoOutTime { get; set; }
        /// <summary>
        /// flow
        /// </summary>
        public string FK_Flow { get; set; }
        /// <summary>
        ///  Node Type 
        /// </summary>
        public int? NodeWorkType { get; set; }
        /// <summary>
        ///  Process name 
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FK_FlowSort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FK_FlowSortT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FrmAttr { get; set; }
        /// <summary>
        ///  Description 
        /// </summary>
        public string Doc { get; set; }
        /// <summary>
        ///  Can CC 
        /// </summary>
        public bool? IsCanCC { get; set; }
        /// <summary>
        ///  Can I view Report ?
        /// </summary>
        public bool? IsCanRpt { get; set; }
        /// <summary>
        ///  Whether the process can be terminated 
        /// </summary>
        public bool? IsCanOver { get; set; }
        /// <summary>
        ///  Is secrecy step 
        /// </summary>
        public bool? IsSecret { get; set; }
        /// <summary>
        ///  Can I delete process 
        /// </summary>
        public bool? IsCanDelFlow { get; set; }
        /// <summary>
        ///  Can I transfer 
        /// </summary>
        public bool? IsHandOver { get; set; }
        /// <summary>
        ///  Audit Mode ( Effective audit node ), Enumerated type :0  Single sign ;1  Meeting sign ;
        /// </summary>
        public int? SignType { get; set; }
        /// <summary>
        ///  Diversion rules 
        /// </summary>
        public int? FLRole { get; set; }
        /// <summary>
        ///  Process Node Type 
        /// </summary>
        public int? FNType { get; set; }
        /// <summary>
        ///  Location 
        /// </summary>
        public int? NodePosType { get; set; }
        /// <summary>
        ///  Are there nodes closing conditions 
        /// </summary>
        public bool? IsCCNode { get; set; }
        /// <summary>
        ///  Are there conditions to complete the process 
        /// </summary>
        public bool? IsCCFlow { get; set; }
        /// <summary>
        ///  Post 
        /// </summary>
        public string HisStas { get; set; }
        /// <summary>
        ///  Department 
        /// </summary>
        public string HisDeptStrs { get; set; }
        /// <summary>
        ///  Go to the node 
        /// </summary>
        public string HisToNDs { get; set; }
        /// <summary>
        ///  Invoice IDs
        /// </summary>
        public string HisBillIDs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HisEmps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HisSubFlows { get; set; }
        /// <summary>
        ///  Physical table 
        /// </summary>
        public string PTable { get; set; }
        /// <summary>
        ///  Form display 
        /// </summary>
        public string ShowSheets { get; set; }
        /// <summary>
        ///  Post grouping node 
        /// </summary>
        public string GroupStaNDs { get; set; }
        /// <summary>
        /// X Coordinate 
        /// </summary>
        public int? X { get; set; }
        /// <summary>
        /// Y Coordinate 
        /// </summary>
        public int? Y { get; set; }

        #endregion Model


        #region  Method

        /// <summary>
        ///  Get a target entity 
        /// </summary>
        public WFNode(int nodeId)
        {
            var strSql = new StringBuilder();
            strSql.Append("SELECT * FROM WF_Node WHERE NodeID = " + nodeId);
            var dt = DBAccess.RunSQLReturnTable(strSql.ToString());
            if (dt.Rows.Count == 0)
                return;
            try
            {
                if (dt.Rows[0]["NodeID"].ToString() != "")
                {
                    NodeID = int.Parse(dt.Rows[0]["NodeID"].ToString());
                }
                if (dt.Rows[0]["Step"].ToString() != "")
                {
                    Step = int.Parse(dt.Rows[0]["Step"].ToString());
                }
                Name = dt.Rows[0]["Name"].ToString();
                if (dt.Rows[0]["IsTask"].ToString() != "")
                {
                    IsTask = Convert.ToBoolean(dt.Rows[0]["IsTask"]);
                }
                if (dt.Rows[0]["IsForceKill"].ToString() != "")
                {
                    IsForceKill = Convert.ToBoolean(dt.Rows[0]["IsForceKill"]);
                }
                if (dt.Rows[0]["PassRate"].ToString() != "")
                {
                    PassRate = decimal.Parse(dt.Rows[0]["PassRate"].ToString());
                }
                if (dt.Rows[0]["RunModel"].ToString() != "")
                {
                    RunModel = int.Parse(dt.Rows[0]["RunModel"].ToString());
                }
                FocusField = dt.Rows[0]["FocusField"].ToString();
                if (dt.Rows[0]["DeliveryWay"].ToString() != "")
                {
                    DeliveryWay = int.Parse(dt.Rows[0]["DeliveryWay"].ToString());
                }

                SendLab = dt.Rows[0]["SendLab"].ToString();
                SendEnable = true;

                SaveLab = dt.Rows[0]["SaveLab"].ToString();
                SaveEnable = Convert.ToBoolean(dt.Rows[0]["SaveEnable"]);

                ReturnLab = dt.Rows[0]["ReturnLab"].ToString();
                if (dt.Rows[0]["ReturnRole"].ToString() != "")
                {
                    ReturnRole = (ReturnRoleKind)Convert.ToInt32(dt.Rows[0]["ReturnRole"]);
                }

                CCLab = dt.Rows[0]["CCLab"].ToString();
                if (dt.Rows[0][BP.WF.BtnAttr.CCRole].ToString() != "")
                {
                    CCEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.CCRole]);
                }
                ShiftLab = dt.Rows[0]["ShiftLab"].ToString();
                if (dt.Rows[0][BP.WF.BtnAttr.ShiftEnable].ToString() != "")
                {
                    ShiftEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.ShiftEnable]);
                }
                DelLab = dt.Rows[0]["DelLab"].ToString();
                if (dt.Rows[0]["DelEnable"].ToString() != "")
                {
                    DelEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.DelEnable]);
                }
                EndFlowLab = dt.Rows[0]["EndFlowLab"].ToString();
                if (dt.Rows[0]["EndFlowEnable"].ToString() != "")
                {
                    EndFlowEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.EndFlowEnable]);
                }
               
                TrackLab = dt.Rows[0]["TrackLab"].ToString();
                if (dt.Rows[0]["TrackEnable"].ToString() != "")
                {
                    TrackEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.TrackEnable]);
                }
               
                SelectAccepterLab = dt.Rows[0]["SelectAccepterLab"].ToString();
                if (dt.Rows[0][BP.WF.BtnAttr.SelectAccepterEnable].ToString() != "")
                {
                    SelectAccepterEnable = Convert.ToBoolean(dt.Rows[0][BP.WF.BtnAttr.SelectAccepterEnable]);
                }
               
                FK_Flow = dt.Rows[0]["FK_Flow"].ToString();
                FlowName = dt.Rows[0]["FlowName"].ToString();
                FK_FlowSort = dt.Rows[0]["FK_FlowSort"].ToString();
                FK_FlowSortT = dt.Rows[0]["FK_FlowSortT"].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Error ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion  Method
    }

    /// <summary>
    ///  Return rules 
    /// </summary>
    public enum ReturnRoleKind
    {
        /// <summary>
        ///  Can not be returned 
        /// </summary>
        UnEnable = 0,

        /// <summary>
        ///  Return on a node 
        /// </summary>
        BackOne = 1,

        /// <summary>
        ///  Return any node 
        /// </summary>
        BackEvery = 2,

        /// <summary>
        ///  Return the specified node 
        /// </summary>
        BackAppointed = 3
    }

    /// <summary>
    ///  Accessories Permissions 
    /// </summary>
    public enum AttachmentRoleKind
    {
        /// <summary>
        ///  Close Accessories 
        /// </summary>
        Close = 0,

        /// <summary>
        ///  The operator 
        /// </summary>
        User = 1,

        /// <summary>
        ///  The work ID
        /// </summary>
        Work = 2,

        /// <summary>
        ///  Process ID
        /// </summary>
        WorkFlow = 3
    }
}
