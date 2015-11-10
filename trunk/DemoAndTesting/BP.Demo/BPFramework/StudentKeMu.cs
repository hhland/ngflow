using System;
using System.Data;
using BP.DA;
using BP.En;

namespace BP.Demo
{
	/// <summary>
	///  Student subjects corresponding  - Property 
	/// </summary>
	public class StudentKeMuAttr  
	{
		#region  Basic properties 
		/// <summary>
		///  Student 
		/// </summary>
		public const  string FK_Student="FK_Student";
		/// <summary>
		///  Subject 
		/// </summary>
		public const  string FK_KeMu="FK_KeMu";		 
		#endregion	
	}
	/// <summary>
    ///  Student subjects corresponding 
	/// </summary>
	public class StudentKeMu :EntityMM
	{
		#region  Basic properties 
		/// <summary>
		///  Student 
		/// </summary>
		public string FK_Student
		{
			get
			{
				return this.GetValStringByKey(StudentKeMuAttr.FK_Student);
			}
			set
			{
				SetValByKey(StudentKeMuAttr.FK_Student,value);
			}
		}
        /// <summary>
        ///  Account Name 
        /// </summary>
        public string FK_KeMuT
        {
            get
            {
                return this.GetValRefTextByKey(StudentKeMuAttr.FK_KeMu);
            }
        }
		/// <summary>
		/// Subject 
		/// </summary>
		public string FK_KeMu
		{
			get
			{
				return this.GetValStringByKey(StudentKeMuAttr.FK_KeMu);
			}
			set
			{
				SetValByKey(StudentKeMuAttr.FK_KeMu,value);
			}
		}		  
		#endregion
		 

		#region  Constructor 
		/// <summary>
		///  Student subjects corresponding 
		/// </summary> 
		public StudentKeMu(){}
		/// <summary>
		///  Correspondence courses for working students 
		/// </summary>
		/// <param name="_empoid"> Student </param>
        /// <param name="fk_km"> Account Number </param> 	
		public StudentKeMu(string fk_student,string fk_km)
		{
            this.FK_Student = fk_student;
            this.FK_KeMu = fk_km;
            this.Retrieve();
		}
		/// <summary>
		///  Override the base class methods 
		/// </summary>
		public override Map EnMap
		{
			get
			{
				if (this._enMap!=null) 
					return this._enMap;
				
				Map map = new Map("Demo_StudentKeMu");
				map.EnDesc=" Correspondence courses for working students ";	
				map.EnType=EnType.Dot2Dot;

                map.AddTBStringPK(StudentKeMuAttr.FK_Student, null, " Student ", false, false, 1, 20, 1);
				map.AddDDLEntitiesPK(StudentKeMuAttr.FK_KeMu,null," Subject ",new BP.Demo.KeMus(),true);

				this._enMap=map;
				return this._enMap;
			}
		}
		#endregion

		#region  Override the base class methods 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                {
                    uac.IsView = true;
                    uac.IsDelete = true;
                    uac.IsInsert = true;
                    uac.IsUpdate = true;
                    uac.IsAdjunct = true;
                }
                return uac;
            }
        }
		/// <summary>
		///  Before inserting the work done 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeInsert()
		{
			return base.beforeInsert();			
		}
		/// <summary>
		///  Work done before update 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeUpdate()
		{
			return base.beforeUpdate(); 
		}
		/// <summary>
		///  Work done before deleting 
		/// </summary>
		/// <returns>true/false</returns>
		protected override bool beforeDelete()
		{
			return base.beforeDelete(); 
		}
		#endregion 
	}
	/// <summary>
    ///  Student subjects corresponding s - Set  
	/// </summary>
	public class StudentKeMus : EntitiesMM
	{
		#region  Structure 
		/// <summary>
        ///  Student subjects corresponding s
		/// </summary>
		public StudentKeMus(){}
		/// <summary>
        ///  Student subjects corresponding s
		/// </summary>
		/// <param name="FK_Student">FK_Student</param>
		public StudentKeMus(string  FK_Student)
		{
			QueryObject qo = new QueryObject(this);
			qo.AddWhere(StudentKeMuAttr.FK_Student, FK_Student);
			qo.DoQuery();
		}
		#endregion

		#region  Overriding methods 
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override Entity GetNewEntity
		{
			get
			{
				return new StudentKeMu();
			}
		}	
		#endregion 
	}
	
}
