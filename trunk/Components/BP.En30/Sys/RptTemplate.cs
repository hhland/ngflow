using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP;
namespace BP.Sys
{
	/// <summary>
	///  Percentage display 
	/// </summary>
	public enum PercentModel
	{
		/// <summary>
		///  Do not show 
		/// </summary>
		None,
		/// <summary>
		///  Longitudinal 
		/// </summary>
		Vertical,
		/// <summary>
		///  Lateral 
		/// </summary>
		Transverse,
	}
	/// <summary>
	/// RptTemplateAttr
	/// </summary>
    public class RptTemplateAttr : EntityOIDAttr
    {
        /// <summary>
        ///  The class name 
        /// </summary>
        public const string EnsName = "EnsName";
        /// <summary>
        ///  Description 
        /// </summary>
        public const string MyPK = "MyPK";
        /// <summary>
        /// D1
        /// </summary> 
        public const string D1 = "D1";
        /// <summary>
        /// d2
        /// </summary>
        public const string D2 = "D2";
        /// <summary>
        /// d3
        /// </summary>
        public const string D3 = "D3";
        /// <summary>
        ///  Objects to be analyzed s
        /// </summary>
        public const string AlObjs = "AlObjs";
        /// <summary>
        ///  Record people 
        /// </summary>
        public const string Height = "Height";
        /// <summary>
        /// EnsName
        /// </summary>
        public const string Width = "Width";
        /// <summary>
        ///  Whether to display a large sum 
        /// </summary>
        public const string IsSumBig = "IsSumBig";
        /// <summary>
        ///  Whether to display the small total 
        /// </summary>
        public const string IsSumLittle = "IsSumLittle";
        /// <summary>
        ///  Total is displayed right 
        /// </summary>
        public const string IsSumRight = "IsSumRight";
        /// <summary>
        ///  Ratio shows the way 
        /// </summary>
        public const string PercentModel = "PercentModel";
        /// <summary>
        ///  Staff 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
    }
	/// <summary>
	///  Report Template 
	/// </summary>
	public class RptTemplate: Entity
	{
		#region  Basic properties 
		/// <summary>
		///  Collection class name 
		/// </summary>
		public string EnsName
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.EnsName) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.EnsName,value) ; 
			}		
		}
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(RptTemplateAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(RptTemplateAttr.FK_Emp, value);
            }
        }
		/// <summary>
		///  Description 
		/// </summary>
		public string MyPK
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.MyPK ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.MyPK,value) ; 
			}
		}		 
		/// <summary>
		/// D1
		/// </summary>
		public string D1
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.D1) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.D1,value) ; 
			}
		}
		/// <summary>
		/// D2
		/// </summary>
		public string D2
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.D2) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.D2,value) ; 
			}
		}
		/// <summary>
		/// D3
		/// </summary>
		public string D3
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.D3) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.D3,value) ; 
			}
		}
		public string AlObjsText
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.AlObjs ) ; 
			}
		}
		/// <summary>
		///  Object analysis 
		///  Data Format  @ Analysis of the object 1@ Analysis of the object 2@ Analysis of the object 3@
		/// </summary>
		public string AlObjs
		{
			get
			{
				return this.GetValStringByKey(RptTemplateAttr.AlObjs) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.AlObjs,value) ; 
			}
		}
		public int Height
		{
			get
			{
				return this.GetValIntByKey(RptTemplateAttr.Height ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.Height,value) ; 
			}
		}
		public int Width
		{
			get
			{
				return this.GetValIntByKey(RptTemplateAttr.Width ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.Width,value) ; 
			}
		}
		/// <summary>
		///  Whether to display a large sum 
		/// </summary>
		public bool IsSumBig
		{
			get
			{
				return this.GetValBooleanByKey(RptTemplateAttr.IsSumBig ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.IsSumBig,value) ; 
			}
		}
		/// <summary>
		///  Small total 
		/// </summary>
		public bool IsSumLittle
		{
			get
			{
				return this.GetValBooleanByKey(RptTemplateAttr.IsSumLittle ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.IsSumLittle,value) ; 
			}
		}
		/// <summary>
		///  Is it realistic right aggregate .
		/// </summary>
		public bool IsSumRight
		{
			get
			{
				return this.GetValBooleanByKey(RptTemplateAttr.IsSumRight ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.IsSumRight,value) ; 
			}
		}
		public PercentModel PercentModel
		{
			get
			{
				return (PercentModel)this.GetValIntByKey(RptTemplateAttr.PercentModel ) ; 
			}
			set
			{
				this.SetValByKey(RptTemplateAttr.PercentModel,(int)value) ; 
			}
		}
		#endregion

		#region  Constructor 

		public override UAC HisUAC
		{
			get
			{
				UAC uac = new UAC();
				uac.IsUpdate=true;
				uac.IsView=true;
				return base.HisUAC;
			}
		}
        /// <summary>
        /// 
        /// </summary>
		public RptTemplate()
		{
		}
        /// <summary>
        /// ¿‡
        /// </summary>
        /// <param name="EnsName"></param>
        public RptTemplate(string ensName)
        {
            this.EnsName = ensName;
            this.FK_Emp = Web.WebUser.No;

            this.MyPK = Web.WebUser.No + "@" + EnsName;
            
            try
            {
                this.Retrieve();
            }
            catch
            {
                this.Insert();
            }
        }
		 
		/// <summary>
        ///  Report Template 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				Map map = new Map("Sys_RptTemplate");
				map.DepositaryOfEntity=Depositary.Application;
				map.EnDesc=" Report Template ";
				map.EnType=EnType.Sys;

                map.AddMyPK();
				map.AddTBString(RptTemplateAttr.EnsName,null," Class name ",false,false,0,500,20);
                map.AddTBString(RptTemplateAttr.FK_Emp, null, " The operator ", true, false, 0, 20, 20);


				map.AddTBString(RptTemplateAttr.D1,null,"D1",false,true,0,90,10);
				map.AddTBString(RptTemplateAttr.D2,null,"D2",false,true,0,90,10);
				map.AddTBString(RptTemplateAttr.D3,null,"D3",false,true,0,90,10);

				map.AddTBString(RptTemplateAttr.AlObjs,null," Objects to be analyzed ",false,true,0,90,10);

				map.AddTBInt(RptTemplateAttr.Height,600,"Height",false,true);
				map.AddTBInt(RptTemplateAttr.Width,800,"Width",false,true);

				map.AddBoolean(RptTemplateAttr.IsSumBig,false," Whether to display a large sum ",false,true);
				map.AddBoolean(RptTemplateAttr.IsSumLittle,false," Whether to display the small total ",false,true);
				map.AddBoolean(RptTemplateAttr.IsSumRight,false," Total is displayed right ",false,true);

				map.AddTBInt(RptTemplateAttr.PercentModel,0," Ratio shows the way ",false,true);
				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion 
	}
	
	/// <summary>
	///  Entity set 
	/// </summary>
	public class RptTemplates : Entities
	{		
		#region  Structure 
		public RptTemplates()
		{
		}
		
		/// <summary>
		///  Inquiry 
		/// </summary>
		/// <param name="EnsName"></param>
		public RptTemplates(string EnsName)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(RptTemplateAttr.EnsName, EnsName);			 
			qo.DoQuery();
		}
		/// <summary>
		///  Get it  Entity
		/// </summary>
		public override Entity GetNewEntity 
		{
			get
			{
				return new RptTemplate();
			}

		}
		#endregion

		#region  Query methods 
		 
		#endregion
		
	}
}
