using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
    /// <summary>
    ///  Expand 
    /// </summary>
    public class MapExtAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Main table 
        /// </summary>
        public const string FK_MapData = "FK_MapData";
        /// <summary>
        /// ExtType
        /// </summary>
        public const string ExtType = "ExtType";
        /// <summary>
        ///  The insertion position of the form 
        /// </summary>
        public const string RowIdx = "RowIdx";
        /// <summary>
        /// GroupID
        /// </summary>
        public const string GroupID = "GroupID";
        /// <summary>
        ///  Height 
        /// </summary>
        public const string H = "H";
        /// <summary>
        ///  Width 
        /// </summary>
        public const string W = "W";
        /// <summary>
        ///  Can adaptive size 
        /// </summary>
        public const string IsAutoSize = "IsAutoSize";
        /// <summary>
        ///  Properties set 
        /// </summary>
        public const string AttrOfOper = "AttrOfOper";
        /// <summary>
        ///  Activation properties 
        /// </summary>
        public const string AttrsOfActive = "AttrsOfActive";
        /// <summary>
        ///  Implementation modalities 
        /// </summary>
        public const string DoWay = "DoWay";
        /// <summary>
        /// Tag
        /// </summary>
        public const string Tag = "Tag";
        /// <summary>
        /// Tag1
        /// </summary>
        public const string Tag1 = "Tag1";
        /// <summary>
        /// Tag2
        /// </summary>
        public const string Tag2 = "Tag2";
        /// <summary>
        /// Tag3
        /// </summary>
        public const string Tag3 = "Tag3";
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public const string DBSrc = "DBSrc";
        /// <summary>
        /// Doc
        /// </summary>
        public const string Doc = "Doc";
        /// <summary>
        ///  Parameters 
        /// </summary>
        public const string AtPara = "AtPara";
        /// <summary>
        ///  Calculating priority 
        /// </summary>
        public const string PRI = "PRI";
    }
    /// <summary>
    ///  Expand 
    /// </summary>
    public class MapExt : EntityMyPK
    {
        #region  With respect to  at  Parameters 
        /// <summary>
        /// Pop Parameters .
        /// </summary>
        public int PopValFormat
        {
            get
            {
                return this.GetParaInt("PopValFormat");
            }
            set
            {
                this.SetPara("PopValFormat", value);
            }
        }
        /// <summary>
        /// pop  Select mode 
        /// 0, Multiple choice ,1= Radio .
        /// </summary>
        public int PopValSelectModel
        {
            get
            {
                return this.GetParaInt("PopValSelectModel");
            }
            set
            {
                this.SetPara("PopValSelectModel", value);
            }
        }

        /// <summary>
        ///  Mode 
        /// 0=url, 1= Internal .
        /// </summary>
        public int PopValWorkModel
        {
            get
            {
                return this.GetParaInt("PopValWorkModel");
            }
            set
            {
                this.SetPara("PopValWorkModel", value);
            }
        }


        /// <summary>
        /// pop  Presentation 
        /// 0, Form ,1= Table of Contents .
        /// </summary>
        public int PopValShowModel
        {
            get
            {
                return this.GetParaInt("PopValShowModel");
            }
            set
            {
                this.SetPara("PopValShowModel", value);
            }
        }
        #endregion

        #region  Property 
        public string ExtDesc
        {
            get
            {
                string dec = "";
                switch (this.ExtType)
                {
                    case MapExtXmlList.ActiveDDL:
                        dec += " Field " + this.AttrOfOper;
                        break;
                    case MapExtXmlList.TBFullCtrl:
                        dec += this.AttrOfOper;
                        break;
                    case MapExtXmlList.DDLFullCtrl:
                        dec += "" + this.AttrOfOper;
                        break;
                    case MapExtXmlList.InputCheck:
                        dec += " Field :" + this.AttrOfOper + "  Check the contents :" + this.Tag1;
                        break;
                    case MapExtXmlList.PopVal:
                        dec += " Field :" + this.AttrOfOper + " Url:" + this.Tag;
                        break;
                    default:
                        break;
                }
                return dec;
            }
        }
        /// <summary>
        ///  Whether adaptive size 
        /// </summary>
        public bool IsAutoSize
        {
            get
            {
                return this.GetValBooleanByKey(MapExtAttr.IsAutoSize);
            }
            set
            {
                this.SetValByKey(MapExtAttr.IsAutoSize, value);
            }
        }
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public string DBSrc
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.DBSrc);
            }
            set
            {
                this.SetValByKey(MapExtAttr.DBSrc, value);
            }
        }
        public string AtPara
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.AtPara);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AtPara, value);
            }
        }
      
        public string ExtType
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.ExtType);
            }
            set
            {
                this.SetValByKey(MapExtAttr.ExtType, value);
            }
        }
        public int DoWay
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.DoWay);
            }
            set
            {
                this.SetValByKey(MapExtAttr.DoWay, value);
            }
        }
        /// <summary>
        ///  Operations attrs
        /// </summary>
        public string AttrOfOper
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.AttrOfOper);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AttrOfOper, value);
            }
        }
        /// <summary>
        ///  Activated attrs
        /// </summary>
        public string AttrsOfActive
        {
            get
            {
              //  return this.GetValStrByKey(MapExtAttr.AttrsOfActive).Replace("~", "'");
                return this.GetValStrByKey(MapExtAttr.AttrsOfActive);
            }
            set
            {
                this.SetValByKey(MapExtAttr.AttrsOfActive, value);
            }
        }
        public string FK_MapData
        {
            get
            {
                return this.GetValStrByKey(MapExtAttr.FK_MapData);
            }
            set
            {
                this.SetValByKey(MapExtAttr.FK_MapData, value);
            }
        }
        /// <summary>
        /// Doc
        /// </summary>
        public string Doc
        {
            get
            {
                return  this.GetValStrByKey("Doc").Replace("~","'");
            }
            set
            {
                this.SetValByKey("Doc", value);
            }
        }
        public string TagOfSQL_autoFullTB
        {
            get
            {
                if (string.IsNullOrEmpty(this.Tag))
                    return this.DocOfSQLDeal;
                

                string sql = this.Tag;
                sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                return sql;
            }
        }

        public string DocOfSQLDeal
        {
            get
            {
                string sql = this.Doc;
                sql = sql.Replace("@WebUser.No", BP.Web.WebUser.No);
                sql = sql.Replace("@WebUser.Name", BP.Web.WebUser.Name);
                sql = sql.Replace("@WebUser.FK_Dept", BP.Web.WebUser.FK_Dept);
                sql = sql.Replace("@WebUser.FK_DeptName", BP.Web.WebUser.FK_DeptName);
                return sql;
            }
        }
        public string Tag
        {
            get
            {
                string s= this.GetValStrByKey("Tag").Replace("~", "'");

                s = s.Replace("\\\\", "\\");
                s = s.Replace("\\\\", "\\");

                s = s.Replace(@"CCFlow\Data\", @"CCFlow\WF\Data\");

                return s;
            }
            set
            {
                this.SetValByKey("Tag", value);
            }
        }
        public string Tag1
        {
            get
            {
                return this.GetValStrByKey("Tag1").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag1", value);
            }
        }
        public string Tag2
        {
            get
            {
                return this.GetValStrByKey("Tag2").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag2", value);
            }
        }
        public string Tag3
        {
            get
            {
                return this.GetValStrByKey("Tag3").Replace("~", "'");
            }
            set
            {
                this.SetValByKey("Tag3", value);
            }
        }
        public int H
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.H);
            }
            set
            {
                this.SetValByKey(MapExtAttr.H, value);
            }
        }
        public int W
        {
            get
            {
                return this.GetValIntByKey(MapExtAttr.W);
            }
            set
            {
                this.SetValByKey(MapExtAttr.W, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Expand 
        /// </summary>
        public MapExt()
        {
        }
        /// <summary>
        ///  Expand 
        /// </summary>
        /// <param name="no"></param>
        public MapExt(string mypk)
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

                Map map = new Map("Sys_MapExt");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Expand ";
                map.EnType = EnType.Sys;

                map.AddMyPK();

                map.AddTBString(MapExtAttr.FK_MapData, null, " Main table ", true, false, 0, 30, 20);
                map.AddTBString(MapExtAttr.ExtType, null, " Type ", true, false, 0, 30, 20);
                map.AddTBInt(MapExtAttr.DoWay, 0, " Implementation modalities ", true, false);

                map.AddTBString(MapExtAttr.AttrOfOper, null, " Operations Attr", true, false, 0, 30, 20);
                map.AddTBString(MapExtAttr.AttrsOfActive, null, " Activation of the field ", true, false, 0, 900, 20);
                map.AddTBStringDoc();
                map.AddTBString(MapExtAttr.Tag, null, "Tag", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag1, null, "Tag1", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag2, null, "Tag2", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.Tag3, null, "Tag3", true, false, 0, 2000, 20);

                map.AddTBString(MapExtAttr.AtPara, null, " Parameters ", true, false, 0, 2000, 20);
                map.AddTBString(MapExtAttr.DBSrc, null, " Data Sources ", true, false, 0, 20, 20);


                map.AddTBInt(MapExtAttr.H, 500, " Height ", false, false);
                map.AddTBInt(MapExtAttr.W, 400, " Width ", false, false);

                // add by stone 2013-12-21  Calculating priority , For js Calculation .
                map.AddTBInt(MapExtAttr.PRI, 0, "PRI", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

    }
    /// <summary>
    ///  Expand s
    /// </summary>
    public class MapExts : Entities
    {
        #region  Structure 
        /// <summary>
        ///  Expand s
        /// </summary>
        public MapExts()
        {
        }
        /// <summary>
        ///  Expand s
        /// </summary>
        /// <param name="fk_mapdata">s</param>
        public MapExts(string fk_mapdata)
        {
            this.Retrieve(MapExtAttr.FK_MapData, fk_mapdata, MapExtAttr.PRI);
        }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new MapExt();
            }
        }
        #endregion
    }
}
