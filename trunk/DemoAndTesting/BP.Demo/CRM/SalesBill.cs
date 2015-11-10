using System;
using System.Data;
using BP.DA;
using BP.En;
using BP.WF;
using BP.Port;

namespace BP.Demo
{
    /// <summary>
    ///  Sales Billing   Property 
    /// </summary>
    public class SaleBillAttr
    {
        #region  Basic properties 
        /// <summary>
        ///  Work node 
        /// </summary>
        public const string FK_Emp = "FK_Emp";
        /// <summary>
        ///  Department 
        /// </summary>
        public const string FK_Dept = "FK_Dept";
        /// <summary>
        ///  Year 
        /// </summary>
        public const string FK_ND = "FK_ND";
        /// <summary>
        ///  Money 
        /// </summary>
        public const string JE = "JE";
        /// <summary>
        ///  Years 
        /// </summary>
        public const string FK_NY = "FK_NY";
        /// <summary>
        ///  Product 
        /// </summary>
        public const string FK_Product = "FK_Product";
        #endregion
    }
    /// <summary>
    ///  Sales Billing 
    /// </summary>
    public class SaleBill : EntityOID
    {
        #region  Property 
        /// <summary>
        ///  Years 
        /// </summary>
        public string FK_NY
        {
            get
            {
                return this.GetValStringByKey(SaleBillAttr.FK_NY);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.FK_NY, value);
            }
        }
        public string FK_Product
        {
            get
            {
                return this.GetValStringByKey(SaleBillAttr.FK_Product);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.FK_Product, value);
            }
        }
        /// <summary>
        ///  Staff 
        /// </summary>
        public string FK_Emp
        {
            get
            {
                return this.GetValStringByKey(SaleBillAttr.FK_Emp);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.FK_Emp, value);
            }
        }
        /// <summary>
        ///  Workplace 
        /// </summary>
        public string FK_Dept
        {
            get
            {
                return this.GetValStringByKey(SaleBillAttr.FK_Dept);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.FK_Dept, value);
            }
        }
        /// <summary>
        ///  Year 
        /// </summary>
        public string FK_ND
        {
            get
            {
                return this.GetValStringByKey(SaleBillAttr.FK_ND);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.FK_ND, value);
            }
        }
        /// <summary>
        ///  Money 
        /// </summary>
        public decimal JE
        {
            get
            {
                return this.GetValDecimalByKey(SaleBillAttr.JE);
            }
            set
            {
                this.SetValByKey(SaleBillAttr.JE, value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Sales Billing 
        /// </summary>
        public SaleBill()
        {
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
                Map map = new Map("Demo_SaleBill");
                map.EnDesc = " Sales Billing ";
                map.AddTBIntPKOID();
                map.AddDDLEntities(SaleBillAttr.FK_Dept, null, " Department ", new BP.Port.Depts(), false);
                map.AddDDLEntities(SaleBillAttr.FK_Emp, null, " Staff ", new BP.Port.Emps(),false);
                
                map.AddDDLEntities(SaleBillAttr.FK_ND, null, " Year ", new BP.Pub.NDs(), false);
                map.AddDDLEntities(SaleBillAttr.FK_NY, null, " Years ", new BP.Pub.NYs(), false);

                map.AddDDLEntities(SaleBillAttr.FK_Product, null, " Product ", new BP.Demo.Products(), false);
                map.AddTBDecimal(SaleBillAttr.JE, 0, " Sales Amount ", true, false);
                map.AddTBString(ProductAttr.Addr, null, " Production Address ", true, false, 0, 200, 200);

                // The query mapping .
                map.AddSearchAttr(SaleBillAttr.FK_Dept);
                map.AddSearchAttr(SaleBillAttr.FK_NY);
                map.AddSearchAttr(SaleBillAttr.FK_Product);

                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Sales Billing s
    /// </summary>
    public class SaleBills : Entities
    {
        #region  Method 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new SaleBill();
            }
        }
        /// <summary>
        ///  Sales Billing s
        /// </summary>
        public SaleBills() { }
        #endregion
    }
}
