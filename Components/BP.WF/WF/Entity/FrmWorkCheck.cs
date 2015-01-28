using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.WF.Template;
using BP.WF;
namespace BP.Sys
{
    public enum FWCType
    {
        /// <summary>
        ///  Audit Components 
        /// </summary>
        Check,
        /// <summary>
        ///  Logging component 
        /// </summary>
        Log
    }
    /// <summary>
    ///  Display Format 
    /// </summary>
    public enum FrmWorkShowModel
    {
        /// <summary>
        ///  Form 
        /// </summary>
        Table,
        /// <summary>
        ///  Freedom display 
        /// </summary>
        Free
    }
    /// <summary>
    ///  Audit Component Status 
    /// </summary>
    public enum FrmWorkCheckSta
    {
        /// <summary>
        ///  Unavailable 
        /// </summary>
        Disable,
        /// <summary>
        ///  Available 
        /// </summary>
        Enable,
        /// <summary>
        ///  Read-only 
        /// </summary>
        Readonly
    }
    /// <summary>
    ///  Audit Components 
    /// </summary>
    public class FrmWorkCheckAttr : EntityNoAttr
    {
        /// <summary>
        ///  Can approval 
        /// </summary>
        public const string FWCSta = "FWCSta";
        /// <summary>
        /// X
        /// </summary>
        public const string FWC_X = "FWC_X";
        /// <summary>
        /// Y
        /// </summary>
        public const string FWC_Y = "FWC_Y";
        /// <summary>
        /// H
        /// </summary>
        public const string FWC_H = "FWC_H";
        /// <summary>
        /// W
        /// </summary>
        public const string FWC_W = "FWC_W";
        /// <summary>
        ///  Application Type 
        /// </summary>
        public const string FWCType = "FWCType";
        /// <summary>
        ///  Display mode .
        /// </summary>
        public const string FWCShowModel = "FWCShowModel";
        /// <summary>
        ///  Trajectories is displayed ?
        /// </summary>
        public const string FWCTrackEnable = "FWCTrackEnable";
        /// <summary>
        ///  Historical audit information is displayed ?
        /// </summary>
        public const string FWCListEnable = "FWCListEnable";
        /// <summary>
        ///  Are all steps show ?
        /// </summary>
        public const string FWCIsShowAllStep = "FWCIsShowAllStep";
        /// <summary>
        ///  Default audit information 
        /// </summary>
        public const string FWCDefInfo = "FWCDefInfo";
        /// <summary>
        ///  If the user does not audit opinion is populated by default ?
        /// </summary>
        public const string FWCIsFullInfo = "FWCIsFullInfo";
        /// <summary>
        ///  Operating nouns ( Check , Audited , Review , Instructions )
        /// </summary>
        public const string FWCOpLabel = "FWCOpLabel";
        /// <summary>
        ///  The operator whether to display digital signature 
        /// </summary>
        public const string SigantureEnabel = "SigantureEnabel";
    }
    /// <summary>
    ///  Audit Components 
    /// </summary>
    public class FrmWorkCheck : Entity
    {
        #region  Property 
        public string No
        {
            get
            {
                return "ND" + this.NodeID;
            }
            set
            {
                string nodeID = value.Replace("ND", "");
                this.NodeID = int.Parse(nodeID);
            }
        }
        /// <summary>
        ///  Node ID
        /// </summary>
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
        ///  Status 
        /// </summary>
        public FrmWorkCheckSta HisFrmWorkCheckSta
        {
            get
            {
                return (FrmWorkCheckSta)this.GetValIntByKey(FrmWorkCheckAttr.FWCSta);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCSta, (int)value);
            }
        }
        /// <summary>
        ///  Display Format (0= Form ,1= Free .)
        /// </summary>
        public FrmWorkShowModel HisFrmWorkShowModel
        {
            get
            {
                return (FrmWorkShowModel)this.GetValIntByKey(FrmWorkCheckAttr.FWCShowModel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCShowModel, (int)value);
            }
        }
        /// <summary>
        ///  Component Type 
        /// </summary>
        public FWCType HisFrmWorkCheckType
        {
            get
            {
                return (FWCType)this.GetValIntByKey(FrmWorkCheckAttr.FWCType);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCType, (int)value);
            }
        }
        /// <summary>
        /// Y
        /// </summary>
        public float FWC_Y
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_Y);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_Y, value);
            }
        }
        /// <summary>
        /// X
        /// </summary>
        public float FWC_X
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_X);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_X, value);
            }
        }
        /// <summary>
        /// W
        /// </summary>
        public float FWC_W
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_W);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_W, value);
            }
        }
        /// <summary>
        /// H
        /// </summary>
        public float FWC_H
        {
            get
            {
                return this.GetValFloatByKey(FrmWorkCheckAttr.FWC_H);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWC_H, value);
            }
        }
        /// <summary>
        ///  Trajectories is displayed ?
        /// </summary>
        public bool FWCTrackEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCTrackEnable);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCTrackEnable, value);
            }
        }
        /// <summary>
        ///  Historical audit information is displayed ?
        /// </summary>
        public bool FWCListEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCListEnable);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCListEnable, value);
            }
        }
        /// <summary>
        ///  All the steps in the track list is displayed ?
        /// </summary>
        public bool FWCIsShowAllStep
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCIsShowAllStep);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCIsShowAllStep, value);
            }
        }
        /// <summary>
        ///  If the user does not audit opinion is populated by default ?
        /// </summary>
        public bool FWCIsFullInfo
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.FWCIsFullInfo);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCIsFullInfo, value);
            }
        }
        /// <summary>
        ///  Default audit information 
        /// </summary>
        public string FWCDefInfo
        {
            get
            {
                return this.GetValStringByKey(FrmWorkCheckAttr.FWCDefInfo);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCDefInfo, value);
            }
        }
        /// <summary>
        ///  Operating nouns ( Check , Audited , Review , Instructions )
        /// </summary>
        public string FWCOpLabel
        {
            get
            {
                return this.GetValStringByKey(FrmWorkCheckAttr.FWCOpLabel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.FWCOpLabel, value);
            }
        }
        /// <summary>
        ///  Whether to display digital signature ?
        /// </summary>
        public bool SigantureEnabel
        {
            get
            {
                return this.GetValBooleanByKey(FrmWorkCheckAttr.SigantureEnabel);
            }
            set
            {
                this.SetValByKey(FrmWorkCheckAttr.SigantureEnabel, value);
            }
        }
        #endregion

        #region  Constructor 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                uac.IsDelete = false;
                uac.IsInsert = false;
                return uac;
            }
        }
        public override string PK
        {
            get
            {
                return "NodeID";
            }
        }
        /// <summary>
        ///  Audit Components 
        /// </summary>
        public FrmWorkCheck()
        {
        }
        /// <summary>
        ///  Audit Components 
        /// </summary>
        /// <param name="no"></param>
        public FrmWorkCheck(string mapData)
        {
            if (mapData.Contains("ND") == false)
            {
                this.HisFrmWorkCheckSta = FrmWorkCheckSta.Disable;
                return;
            }

            string mapdata = mapData.Replace("ND", "");
            if (DataType.IsNumStr(mapdata) == false)
            {
                this.HisFrmWorkCheckSta = FrmWorkCheckSta.Disable;
                return;
            }

            try
            {
                this.NodeID = int.Parse(mapdata);
            }
            catch
            {
                return;
            }
            this.Retrieve();
        }
        /// <summary>
        ///  Audit Components 
        /// </summary>
        /// <param name="no"></param>
        public FrmWorkCheck(int nodeID)
        {
            this.NodeID = nodeID;
            this.Retrieve();
        }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;
                Map map = new Map("WF_Node");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Audit Components ";
                map.EnType = EnType.Sys;

                map.AddTBIntPK(NodeAttr.NodeID, 0, " Node ID", true, true);

                #region  Here changed  NodeSheet Class ,map  This section also describes changes .
                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCSta, (int)FrmWorkCheckSta.Disable, " Audit Component Status ",
                   true, true, FrmWorkCheckAttr.FWCSta, "@0= Disable @1= Enable @2= Read-only ");
                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCShowModel, (int)FrmWorkShowModel.Free, " Display mode ",
                    true, true, FrmWorkCheckAttr.FWCShowModel, "@0= Tabular form @1= Free Mode "); // This property has no use .

                map.AddDDLSysEnum(FrmWorkCheckAttr.FWCType, (int)FWCType.Check, " Audit Components ", true, true, FrmWorkCheckAttr.FWCType, "@0= Audit Components @1= Logging component ");

                map.AddBoolean(FrmWorkCheckAttr.FWCTrackEnable, true, " Trajectories is displayed ?", true, true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCListEnable, true, " Historical audit information is displayed ?(No, Historical information appears only comments box )", true, true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCIsShowAllStep, false, " All the steps in the track list is displayed ?", true, true);

                map.AddTBString(FrmWorkCheckAttr.FWCOpLabel, " Check ", " Operating nouns ( Check / Review / Instructions )", true, false, 0, 200, 10);
                map.AddTBString(FrmWorkCheckAttr.FWCDefInfo, " Agree ", " Default audit information ", true, false, 0, 200, 10);
                map.AddBoolean(FrmWorkCheckAttr.SigantureEnabel, false, " The operator is displayed as a picture signature ?", true, true);
                map.AddBoolean(FrmWorkCheckAttr.FWCIsFullInfo, true, " If the user does not audit opinion is populated by default ?", true, true, true);


                map.AddTBFloat(FrmWorkCheckAttr.FWC_X, 5, " Location X", true, false);
                map.AddTBFloat(FrmWorkCheckAttr.FWC_Y, 5, " Location Y", true, false);


                map.AddTBFloat(FrmWorkCheckAttr.FWC_H, 300, " Height ", true, false);
                map.AddTBFloat(FrmWorkCheckAttr.FWC_W, 400, " Width ", true, false);

                #endregion  Here changed  NodeSheet Class ,map  This section also describes changes .

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Audit Components s
    /// </summary>
    public class FrmWorkChecks : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Audit Components s
        /// </summary>
        public FrmWorkChecks()
        {
        }
        /// <summary>
        ///  Audit Components s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmWorkChecks(string fk_mapdata)
        {
            if (SystemConfig.IsDebug)
                this.Retrieve("No", fk_mapdata);
            else
                this.RetrieveFromCash("No", (object)fk_mapdata);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmWorkCheck();
            }
        }
        #endregion
    }
}
