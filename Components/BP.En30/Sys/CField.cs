using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.Sys
{
	/// <summary>
	///  Property 
	/// </summary>
	public class CFieldAttr
	{
		/// <summary>
		///  Property Key
		/// </summary>
		public const string EnsName="EnsName";
		/// <summary>
		///  Staff 
		/// </summary>
		public const string FK_Emp="FK_Emp";
		/// <summary>
		///  Column selection 
		/// </summary>
		public const string Attrs="Attrs";
	}
	/// <summary>
	///  Column selection 
	/// </summary>
	public class CField: Entity
	{
		#region  Basic properties 
		/// <summary>
		///  Column selection 
		/// </summary>
		public string Attrs
		{
			get
			{
				return this.GetValStringByKey(CFieldAttr.Attrs ) ; 
			}
			set
			{
				this.SetValByKey(CFieldAttr.Attrs,value) ; 
			}
		}
		/// <summary>
		///  The operator ID
		/// </summary>
		public string FK_Emp
		{
			get
			{
				return this.GetValStringByKey(CFieldAttr.FK_Emp ) ; 
			}
			set
			{
				this.SetValByKey(CFieldAttr.FK_Emp,value) ; 
			}
		}
		/// <summary>
		///  Property 
		/// </summary>
		public string EnsName
		{
			get
			{
				return this.GetValStringByKey(CFieldAttr.EnsName ) ; 
			}
			set
			{
				this.SetValByKey(CFieldAttr.EnsName,value) ; 
			}
		}
		#endregion

		#region  Constructor 
		/// <summary>
		///  Column selection 
		/// </summary>
		public CField()
		{
		}
		/// <summary>
		///  Column selection 
		/// </summary>
		/// <param name="FK_Emp"> Staff ID</param>
		/// <param name="className"> The class name </param>
		/// <param name="attrKey"> Property </param>
		/// <param name="Attrs">ֵ</param>
        public CField(string FK_Emp, string className)
        {
            int i = this.Retrieve(CFieldAttr.FK_Emp, FK_Emp,
                   CFieldAttr.EnsName, className);
            if (i == 0)
            {
                this.EnsName = className;
                this.FK_Emp = FK_Emp;
                this.Insert();
            }
        }
		/// <summary>
		/// map
		/// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null) return this._enMap;
                Map map = new Map("Sys_CField");
                map.EnType = EnType.Sys;
                map.EnDesc = " Column selection ";
                map.DepositaryOfEntity = Depositary.None;
                map.AddTBStringPK(CFieldAttr.EnsName, null, " Entity class name ", false, true, 1, 100, 10);
                map.AddTBStringPK(CFieldAttr.FK_Emp, Web.WebUser.No, " Staff ", false, true, 1, 100, 10);
                map.AddTBStringDoc(CFieldAttr.Attrs, null, " Property s", true, false);
                this._enMap = map;
                return this._enMap;
            }
        }
		#endregion 
	
        public static Attrs GetMyAttrs(Entities ens, Map map)
        {
            string vals = SystemConfig.GetConfigXmlEns("ListAttrs", ens.ToString());
            if (vals == null)
                return map.Attrs;
            Attrs attrs=new Attrs();
            foreach (Attr attr in map.Attrs)
            {
                if ( vals.Contains(","+attr.Key+",") )
                    attrs.Add(attr);
            }
            return attrs;

            //string no = Web.WebUser.No;
            //if (no == null)
            //    throw new Exception("@ Your landing time is too long ...");

            //CField cf = new CField(no, ens.ToString());
            //if (cf.Attrs == "")
            //    return ens.GetNewEntity.EnMap.Attrs;

            //Attrs myattrs = new Attrs();
            //Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            //foreach (Attr attr in attrs)
            //{
            //    if (attr.IsPK)
            //    {
            //        myattrs.Add(attr);
            //        continue;
            //    }
            //    if (cf.Attrs.IndexOf("@" + attr.Key + "@") >= 0)
            //        myattrs.Add(attr);
            //}
            //return myattrs;
        }
	}
	/// <summary>
	///  Column selection s
	/// </summary>
	public class CFields : Entities
	{
		/// <summary>
		///  Column selection s
		/// </summary>
		public CFields()
		{
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new CField();
			}
		}
	}
}
