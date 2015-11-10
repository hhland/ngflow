using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    /// <summary>
    ///  Button event type  - ”Îsl  To the same set .
    /// </summary>
    public enum BtnEventType
    {
        /// <summary>
        ///  Disable 
        /// </summary>
        Disable = 0,
        /// <summary>
        ///  Run the stored procedure 
        /// </summary>
        RunSP = 1,
        /// <summary>
        ///  Run sql
        /// </summary>
        RunSQL = 2,
        /// <summary>
        ///  Carried out URL
        /// </summary>
        RunURL = 3,
        /// <summary>
        ///  Run webservices
        /// </summary>
        RunWS = 4,
        /// <summary>
        ///  Run Exe File .
        /// </summary>
        RunExe = 5,
        /// <summary>
        ///  Run JS
        /// </summary>
        RunJS =6
    }    /// <summary>
    ///  Button to access 
    /// </summary>
    public enum BtnUAC
    {
        /// <summary>
        ///  Does not deal with 
        /// </summary>
        None,
        /// <summary>
        ///  By staff 
        /// </summary>
        ByEmp,
        /// <summary>
        ///  By post 
        /// </summary>
        ByStation,
        /// <summary>
        ///  By sector 
        /// </summary>
        ByDept,
        /// <summary>
        /// ∞¥sql
        /// </summary>
        BySQL
    }
    /// <summary>
    ///  Button type 
    /// </summary>
    public enum BtnType
    {
        /// <summary>
        ///  Save 
        /// </summary>
        Save=0,
        /// <summary>
        ///  Print 
        /// </summary>
        Print=1,
        /// <summary>
        ///  Delete 
        /// </summary>
        Delete=2,
        /// <summary>
        ///  Increase 
        /// </summary>
        Add=3,
        /// <summary>
        ///  Custom 
        /// </summary>
        Self=100
    }
    /// <summary>
    ///  Push button 
    /// </summary>
    public class FrmBtnAttr : EntityMyPKAttr
    {
        /// <summary>
        /// Text
        /// </summary>
        public const string Text = "Text";
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// X
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        ///  Width 
        /// </summary>
        public const string BtnType = "BtnType";
        /// <summary>
        ///  Color 
        /// </summary>
        public const string IsView = "IsView";
        /// <summary>
        ///  Style 
        /// </summary>
        public const string IsEnable = "IsEnable";
        /// <summary>
        ///  Font style 
        /// </summary>
        public const string EventContext = "EventContext";
        /// <summary>
        ///  Fonts 
        /// </summary>
        public const string UACContext = "UACContext";
        /// <summary>
        ///  Whether bold 
        /// </summary>
        public const string EventType = "EventType";
        /// <summary>
        ///  Type of control 
        /// </summary>
        public const string UAC = "UAC";
        /// <summary>
        /// MsgOK
        /// </summary>
        public const string MsgOK = "MsgOK";
        /// <summary>
        /// MsgErr
        /// </summary>
        public const string MsgErr = "MsgErr";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
        
    }
    /// <summary>
    ///  Push button 
    /// </summary>
    public class FrmBtn : EntityMyPK
    {
        #region  Property 
      
        public string MsgOK
        {
            get
            {
                return this.GetValStringByKey(FrmBtnAttr.MsgOK);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.MsgOK, value);
            }
        }
        public string MsgErr
        {
            get
            {
                return this.GetValStringByKey(FrmBtnAttr.MsgErr);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.MsgErr, value);
            }
        }
        /// <summary>
        /// EventContext
        /// </summary>
        public string EventContext
        {
            get
            {
                return this.GetValStringByKey(FrmBtnAttr.EventContext).Replace("#", "@");
                //return this.GetValStringByKey(FrmBtnAttr.EventContext);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.EventContext, value);
            }
        }
        public string IsViewHtml
        {
            get
            {
                return PubClass.ToHtmlColor(this.IsView);
            }
        }
        /// <summary>
        /// IsView
        /// </summary>
        public string IsView
        {
            get
            {
                return this.GetValStringByKey(FrmBtnAttr.IsView);
            }
            set
            {
                switch (value)
                {
                    case "#FF000000":
                        this.SetValByKey(FrmBtnAttr.IsView, "Red");
                        return;
                    default:
                        break;
                }
                this.SetValByKey(FrmBtnAttr.IsView, value);
            }
        }
        public string UACContext
        {
            get
            {
                return this.GetValStringByKey(FrmBtnAttr.UACContext);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.UACContext, value);
            }
        }
        public bool EventType
        {
            get
            {
                return this.GetValBooleanByKey(FrmBtnAttr.EventType);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.EventType, value);
            }
        }
        public bool UAC
        {
            get
            {
                return this.GetValBooleanByKey(FrmBtnAttr.UAC);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.UAC, value);
            }
        }
        /// <summary>
        /// IsEnable
        /// </summary>
        public bool IsEnable
        {
            get
            {
                return this.GetValBooleanByKey(FrmBtnAttr.IsEnable);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.IsEnable, value);
            }
        }
        /// <summary>
        /// Y
        /// </summary>
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmBtnAttr.Y);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.Y, value);
            }
        }
        /// <summary>
        /// X
        /// </summary>
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmBtnAttr.X);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.X, value);
            }
        }
        public BtnEventType HisBtnEventType
        {
            get
            {
                return (BtnEventType)this.GetValIntByKey(FrmBtnAttr.EventType);
            }
        }
        /// <summary>
        /// BtnType
        /// </summary>
        public int BtnType
        {
            get
            {
                return this.GetValIntByKey(FrmBtnAttr.BtnType);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.BtnType, value);
            }
        }
        /// <summary>
        /// FK_MapData
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmBtnAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.FK_MapData, value);
            }
        }
        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                return this.GetValStrByKey(FrmBtnAttr.Text);
            }
            set
            {
                this.SetValByKey(FrmBtnAttr.Text, value);
            }
        }
        public string TextHtml
        {
            get
            {
                if (this.EventType)
                    return "<b>" + this.GetValStrByKey(FrmBtnAttr.Text).Replace("@","<br>") + "</b>";
                else
                    return this.GetValStrByKey(FrmBtnAttr.Text).Replace("@", "<br>");
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Push button 
        /// </summary>
        public FrmBtn()
        {
        }
        /// <summary>
        ///  Push button 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmBtn(string mypk)
        {
            this.MyPK = mypk;
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

                Map map = new Map("Sys_FrmBtn");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Push button ";
                map.EnType = EnType.Sys;

                map.AddMyPK();
                map.AddTBString(FrmBtnAttr.FK_MapData, null, "FK_MapData", true, false, 1, 30, 20);
                map.AddTBString(FrmBtnAttr.Text, null, " Label ", true, false, 0, 3900, 20);

                map.AddTBFloat(FrmBtnAttr.X, 5, "X", true, false);
                map.AddTBFloat(FrmBtnAttr.Y, 5, "Y", false, false);

                map.AddTBInt(FrmBtnAttr.IsView, 0, " Is visible ", false, false);
                map.AddTBInt(FrmBtnAttr.IsEnable, 0, " Whether hired ", false, false);

                map.AddTBInt(FrmBtnAttr.BtnType, 0, " Type ", false, false);

                map.AddTBInt(FrmBtnAttr.UAC, 0, " Type of control ", false, false);
                map.AddTBString(FrmBtnAttr.UACContext, null, " Control content ", true, false, 0, 3900, 20);

                map.AddTBInt(FrmBtnAttr.EventType, 0, " Event Type ", false, false);
                map.AddTBString(FrmBtnAttr.EventContext, null, " Event Content ", true, false, 0, 3900, 20);

                map.AddTBString(FrmBtnAttr.MsgOK, null, " Run success tips ", true, false, 0, 500, 20);
                map.AddTBString(FrmBtnAttr.MsgErr, null, " Fail tips ", true, false, 0, 500, 20);

                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);
             
                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Push button s
    /// </summary>
    public class FrmBtns : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Push button s
        /// </summary>
        public FrmBtns()
        {
        }
        /// <summary>
        ///  Push button s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmBtns(string fk_mapdata)
        {
            if (SystemConfig.IsDebug)
                this.Retrieve(FrmLineAttr.FK_MapData, fk_mapdata);
            else
                this.RetrieveFromCash(FrmLineAttr.FK_MapData, (object)fk_mapdata);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FrmBtn();
            }
        }
        #endregion
    }
}
