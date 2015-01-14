using System;
using System.Collections;
using BP.DA;
using BP.En;
namespace BP.Sys
{
    /// <summary>
    ///  Hyperlinks 
    /// </summary>
    public class FrmLinkAttr : EntityMyPKAttr
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
        /// Target
        /// </summary>
        public const string Target = "Target";
        public const string URL = "URL";
        /// <summary>
        /// X
        /// </summary>
        public const string X = "X";
        /// <summary>
        /// Y
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// X2
        /// </summary>
        public const string X2 = "X2";
        /// <summary>
        /// Y2
        /// </summary>
        public const string Y2 = "Y2";
        /// <summary>
        ///  Width 
        /// </summary>
        public const string FontSize = "FontSize";
        /// <summary>
        ///  Color 
        /// </summary>
        public const string FontColor = "FontColor";
        /// <summary>
        /// FontName
        /// </summary>
        public const string FontName = "FontName";
        /// <summary>
        ///  Font style 
        /// </summary>
        public const string FontStyle = "FontStyle";
        /// <summary>
        /// GUID
        /// </summary>
        public const string GUID = "GUID";
    }
    /// <summary>
    ///  Hyperlinks 
    /// </summary>
    public class FrmLink : EntityMyPK
    {
        #region  Property 
        /// <summary>
        /// FontStyle
        /// </summary>
        public string FontStyle
        {
            get
            {
                return this.GetValStringByKey(FrmLinkAttr.FontStyle);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.FontStyle, value);
            }
        }
        public string FontColorHtml
        {
            get
            {
                return PubClass.ToHtmlColor(this.FontColor);
            }
        }
        /// <summary>
        /// FontColor
        /// </summary>
        public string FontColor
        {
            get
            {
                return this.GetValStringByKey(FrmLinkAttr.FontColor);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.FontColor, value);
            }
        }
        public string URL
        {
            get
            {
                return this.GetValStringByKey(FrmLinkAttr.URL).Replace("#","@");
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.URL, value);
            }
        }
        /// <summary>
        /// Font
        /// </summary>
        public string FontName
        {
            get
            {
                return this.GetValStringByKey(FrmLinkAttr.FontName);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.FontName, value);
            }
        }
        /// <summary>
        /// Y
        /// </summary>
        public float Y
        {
            get
            {
                return this.GetValFloatByKey(FrmLinkAttr.Y);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.Y, value);
            }
        }
        /// <summary>
        /// X
        /// </summary>
        public float X
        {
            get
            {
                return this.GetValFloatByKey(FrmLinkAttr.X);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.X, value);
            }
        }
        /// <summary>
        /// FontSize
        /// </summary>
        public int FontSize
        {
            get
            {
                return this.GetValIntByKey(FrmLinkAttr.FontSize);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.FontSize, value);
            }
        }
        /// <summary>
        /// FK_MapData
        /// </summary>
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(FrmLinkAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.FK_MapData, value);
            }
        }
        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get
            {
                return this.GetValStrByKey(FrmLinkAttr.Text);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.Text, value);
            }
        }
        public string Target
        {
            get
            {
                return this.GetValStringByKey(FrmLinkAttr.Target);
            }
            set
            {
                this.SetValByKey(FrmLinkAttr.Target, value);
            }
        }
        public bool IsBold
        {
            get
            {
                return this.GetValBooleanByKey(FrmLabAttr.IsBold);
            }
            set
            {
                this.SetValByKey(FrmLabAttr.IsBold, value);
            }
        }
        public bool IsItalic
        {
            get
            {
                return this.GetValBooleanByKey(FrmLabAttr.IsItalic);
            }
            set
            {
                this.SetValByKey(FrmLabAttr.IsItalic, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Hyperlinks 
        /// </summary>
        public FrmLink()
        {
        }
        /// <summary>
        ///  Hyperlinks 
        /// </summary>
        /// <param name="mypk"></param>
        public FrmLink(string mypk)
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
                Map map = new Map("Sys_FrmLink");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Hyperlinks ";
                map.EnType = EnType.Sys;

                map.AddMyPK();
                map.AddTBString(FrmLinkAttr.FK_MapData, null, "FK_MapData", true, false, 1, 30, 20);
                map.AddTBString(FrmLinkAttr.Text, "New Link", "Label", true, false, 0, 500, 20);

                map.AddTBString(FrmLinkAttr.URL, null, "URL", true, false, 0, 500, 20);

                map.AddTBString(FrmLinkAttr.Target, "_blank", "Target", true, false, 0, 20, 20);

                map.AddTBFloat(FrmLinkAttr.X, 5, "X", true, false);
                map.AddTBFloat(FrmLinkAttr.Y, 5, "Y", false, false);

                map.AddTBInt(FrmLinkAttr.FontSize, 12, "FontSize", false, false);
                map.AddTBString(FrmLinkAttr.FontColor, "black", "FontColor", true, false, 0, 200, 20);
                map.AddTBString(FrmLinkAttr.FontName, null, "FontName", true, false, 0, 200, 20);
                map.AddTBString(FrmLinkAttr.FontStyle, "normal", "FontStyle", true, false, 0, 200, 20);

                map.AddTBInt(FrmLabAttr.IsBold, 0, "IsBold", false, false);
                map.AddTBInt(FrmLabAttr.IsItalic, 0, "IsItalic", false, false);

                map.AddTBString(FrmBtnAttr.GUID, null, "GUID", true, false, 0, 128, 20);


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Hyperlinks s
    /// </summary>
    public class FrmLinks : EntitiesMyPK
    {
        #region  Structure 
        /// <summary>
        ///  Hyperlinks s
        /// </summary>
        public FrmLinks()
        {
        }
        /// <summary>
        ///  Hyperlinks s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public FrmLinks(string fk_mapdata)
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
                return new FrmLink();
            }
        }
        #endregion
    }
}
