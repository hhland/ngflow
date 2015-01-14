using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	/// <summary>
	///  User-defined table 
	/// </summary>
    public class SFTableAttr : EntityNoNameAttr
    {
        /// <summary>
        ///  Whether you can delete 
        /// </summary>
        public const string IsDel = "IsDel";
        /// <summary>
        ///  Field 
        /// </summary>
        public const string FK_Val = "FK_Val";
        /// <summary>
        ///  The data sheet describes 
        /// </summary>
        public const string TableDesc = "TableDesc";
        /// <summary>
        ///  Defaults 
        /// </summary>
        public const string DefVal = "DefVal";
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public const string DBSrc = "DBSrc";
        /// <summary>
        ///  Is the tree 
        /// </summary>
        public const string IsTree = "IsTree";

        /// <summary>
        ///  Dictionary table type 
        /// </summary>
	    public const string SFTableType = "SFTableType";

        #region  Links to other systems to obtain attribute data .
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public const string FK_SFDBSrc = "FK_SFDBSrc";
        /// <summary>
        ///  Data source table 
        /// </summary>
        public const string SrcTable = "SrcTable";
        /// <summary>
        ///  The value displayed 
        /// </summary>
        public const string ColumnValue = "ColumnValue";
        /// <summary>
        ///  Text display 
        /// </summary>
        public const string ColumnText = "ColumnText";
        /// <summary>
        ///  Parent node value 
        /// </summary>
        public const string ParentValue = "ParentValue";
        /// <summary>
        ///  Query 
        /// </summary>
	    public const string SelectStatement = "SelectStatement";
        #endregion  Links to other systems to obtain attribute data .

    }

	/// <summary>
	///  User-defined table 
	/// </summary>
    public class SFTable : EntityNoName
    {
        #region  Links to other systems to obtain attribute data 
        /// <summary>
        ///  Data Sources 
        /// </summary>
        public string FK_SFDBSrc
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.FK_SFDBSrc);
            }
            set
            {
                this.SetValByKey(SFTableAttr.FK_SFDBSrc, value);
            }
        }
        /// <summary>
        ///  Physical table name 
        /// </summary>
        public string SrcTable
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.SrcTable);
            }
            set
            {
                this.SetValByKey(SFTableAttr.SrcTable, value);
            }
        }
        /// <summary>
        /// 值/ The primary key field name 
        /// </summary>
        public string ColumnValue
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.ColumnValue);
            }
            set
            {
                this.SetValByKey(SFTableAttr.ColumnValue, value);
            }
        }
        /// <summary>
        ///  Display field / Display field name 
        /// </summary>
        public string ColumnText
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.ColumnText);
            }
            set
            {
                this.SetValByKey(SFTableAttr.ColumnText, value);
            }
        }
        /// <summary>
        ///  Parent node field name 
        /// </summary>
        public string ParentValue
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.ParentValue);
            }
            set
            {
                this.SetValByKey(SFTableAttr.ParentValue, value);
            }
        }

        /// <summary>
        ///  Query 
        /// </summary>
        public string SelectStatement
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.SelectStatement);
            }
            set
            {
                this.SetValByKey(SFTableAttr.SelectStatement, value);
            }
        }
        #endregion

        #region  Property 
        /// <summary>
        ///  Is class 
        /// </summary>
        public bool IsClass
        {
            get
            {
                if (this.No.Contains("."))
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Whether the entity is a tree ?
        /// </summary>
        public bool IsTree
        {
            get
            {
                return this.GetValBooleanByKey(SFTableAttr.IsTree);
            }
            set
            {
                this.SetValByKey(SFTableAttr.IsTree, value);
            }
        }

        /// <summary>
        ///  Dictionary table type 
        /// <remarks>0:NoName Type </remarks>
        /// <remarks>1:NoNameTree Type </remarks>
        /// <remarks>2:NoName The type of administrative divisions </remarks>
        /// </summary>
	    public int SFTableType
	    {
	        get
	        {
	            return this.GetValIntByKey(SFTableAttr.SFTableType);
	        }
            set
            {
                this.SetValByKey(SFTableAttr.SFTableType, value);
            }
	    }

        /// <summary>
        /// 值
        /// </summary>
        public string FK_Val
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.FK_Val);
            }
            set
            {
                this.SetValByKey(SFTableAttr.FK_Val, value);
            }
        }
        public string TableDesc
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.TableDesc);
            }
            set
            {
                this.SetValByKey(SFTableAttr.TableDesc, value);
            }
        }
        public string DefVal
        {
            get
            {
                return this.GetValStringByKey(SFTableAttr.DefVal);
            }
            set
            {
                this.SetValByKey(SFTableAttr.DefVal, value);
            }
        }
        public EntitiesNoName HisEns
        {
            get
            {
                if (this.IsClass)
                {
                    EntitiesNoName ens = (EntitiesNoName)BP.En.ClassFactory.GetEns(this.No);
                    ens.RetrieveAll();
                    return ens;
                }

                BP.En.GENoNames ges = new GENoNames(this.No, this.Name);
                ges.RetrieveAll();
                return ges;
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
                uac.IsInsert = false;
                return uac;
            }
        }
        /// <summary>
        ///  User-defined table 
        /// </summary>
        public SFTable()
        {
        }
        public SFTable(string mypk)
        {
            this.No = mypk;
            try
            {
                this.Retrieve();
            }
            catch (Exception ex)
            {
                switch (this.No)
                {
                    case "BP.Pub.NYs":
                        this.Name = " Years ";
                      //  this.HisSFTableType = SFTableType.ClsLab;
                        this.FK_Val = "FK_NY";
                     //   this.IsEdit = true;
                        this.Insert();
                        break;
                    case "BP.Pub.YFs":
                        this.Name = "月";
                      //  this.HisSFTableType = SFTableType.ClsLab;
                        this.FK_Val = "FK_YF";
                       // this.IsEdit = true;
                        this.Insert();
                        break;
                    case "BP.Pub.Days":
                        this.Name = "天";
                     //   this.HisSFTableType = SFTableType.ClsLab;
                        this.FK_Val = "FK_Day";
                        //this.IsEdit = true;
                        this.Insert();
                        break;
                    case "BP.Pub.NDs":
                        this.Name = "年";
                     //   this.HisSFTableType = SFTableType.ClsLab;
                        this.FK_Val = "FK_ND";
                       // this.IsEdit = true;
                        this.Insert();
                        break;
                    default:
                        throw new Exception(ex.Message);
                }
            }
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
                Map map = new Map("Sys_SFTable");
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Dictionary table ";
                map.EnType = EnType.Sys;

                map.AddTBStringPK(SFTableAttr.No, null, " Table English name ", true, false, 1, 20, 20);
                map.AddTBString(SFTableAttr.Name, null, " Table Chinese name ", true, false, 0, 30, 20);
                map.AddTBString(SFTableAttr.FK_Val, null, " The default field names created ", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.TableDesc, null, " The table below describes ", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.DefVal, null, " Defaults ", true, false, 0, 200, 20);
                map.AddBoolean(SFTableAttr.IsTree, false, " Whether the entity tree ", true, true);
                map.AddDDLSysEnum(SFTableAttr.SFTableType, 0, " Dictionary table type ", true, false, SFTableAttr.SFTableType, "@0=NoName Type @1=NoNameTree Type @2=NoName The type of administrative divisions ");

                // Data Sources .
                map.AddDDLEntities(SFTableAttr.FK_SFDBSrc, "local", " Data Sources ", new BP.Sys.SFDBSrcs(), true);
                map.AddTBString(SFTableAttr.SrcTable, null, " Data source table ", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.ColumnValue, null, " The value displayed ( Number column )", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.ColumnText, null, " Text display ( Name column )", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.ParentValue, null, " The value of the parent ( Parent column )", true, false, 0, 200, 20);
                map.AddTBString(SFTableAttr.SelectStatement, null, " Query ", true, false, 0, 1000, 600, true);

                // Find .
                map.AddSearchAttr(SFTableAttr.FK_SFDBSrc);

                RefMethod rm = new RefMethod();
                rm.Title = " Edit Data "; 
                rm.ClassMethodName = this.ToString() + ".DoEdit";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                rm.IsForEns = false;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Create Table Guide ";
                rm.ClassMethodName = this.ToString() + ".DoGuide";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                rm.IsForEns = false;
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Data Source Administrator ";
                rm.ClassMethodName = this.ToString() + ".DoMangDBSrc";
                rm.RefMethodType = RefMethodType.RightFrameOpen;
                rm.IsForEns = false;
                map.AddRefMethod(rm);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
        /// <summary>
        ///  Data Source Administrator 
        /// </summary>
        /// <returns></returns>
        public string DoMangDBSrc()
        {
            return "/WF/Comm/Search.aspx?EnsName=BP.Sys.SFDBSrcs";
        }
        /// <summary>
        ///  Create Table wizard 
        /// </summary>
        /// <returns></returns>
        public string DoGuide()
        {
            return "/WF/Comm/Sys/SFGuide.aspx";
        }
        /// <summary>
        ///  Edit Data 
        /// </summary>
        /// <returns></returns>
        public string DoEdit()
        {
            if (this.IsClass)
                return "/WF/Comm/Ens.aspx?EnsName=" + this.No;
            else
                return "/WF/MapDef/SFTableEditData.aspx?RefNo=" + this.No;
        }
        protected override bool beforeDelete()
        {
            MapAttrs attrs = new MapAttrs();
            attrs.Retrieve(MapAttrAttr.UIBindKey, this.No);
            if (attrs.Count != 0)
            {
                string err = "";
                foreach (MapAttr item in attrs)
                    err += " @ " + item.MyPK + " " + item.Name ;
                throw new Exception("@ The following fields in the referenced entity :"+err);
            }
            return base.beforeDelete();
        }
    }
	/// <summary>
	///  User-defined table s
	/// </summary>
    public class SFTables : EntitiesNoName
	{		
		#region  Structure 
        /// <summary>
        ///  User-defined table s
        /// </summary>
		public SFTables()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity 
		{
			get
			{
				return new SFTable();
			}
		}
		#endregion
	}
}
