using System;
using System.Collections;
using BP.DA;
using BP.En;

namespace BP.YS
{
    /// <summary>
    ///  Item type attribute 
    /// </summary>
    public class ProjectSortAttr : EntityNoNameAttr
    {
        public const string StaGrade = "StaGrade";
    }
    /// <summary>
    ///  Project Type 
    /// </summary>
    public class ProjectSort : EntityNoName
    {
        #region  Achieve the basic method 
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                uac.OpenForSysAdmin();
                return uac;
            }
        }
        public new string Name
        {
            get
            {
                return this.GetValStrByKey("Name");
            }
        }
        public int Grade
        {
            get
            {
                return this.No.Length / 2;
            }
        }
        public int StaGrade
        {
            get
            {
                return this.GetValIntByKey(ProjectSortAttr.StaGrade);
            }
            set
            {
                this.SetValByKey(ProjectSortAttr.StaGrade,value);
            }
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Project Type 
        /// </summary> 
        public ProjectSort()
        {
        }
        /// <summary>
        ///  Project Type 
        /// </summary>
        /// <param name="_No"></param>
        public ProjectSort(string _No) : base(_No) { }
        /// <summary>
        /// EnMap
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("YS_ProjectSort");
                map.EnDesc = " Project Type "; // " Project Type ";
                map.EnType = EnType.Admin;
                map.DepositaryOfMap = Depositary.Application;
                map.DepositaryOfEntity = Depositary.Application;
                map.CodeStruct = "2"; //  The maximum level is  7 .
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(ProjectSortAttr.No, null, " Serial number ", true, true, 2, 2, 2);
                map.AddTBString(ProjectSortAttr.Name, null, " Name ", true, false, 0, 100, 100);

                //map.AddDDLSysEnum(ProjectSortAttr.StaGrade, 0, " Type ", true, true, ProjectSortAttr.StaGrade,
                //    "@1= Senior post @2= Middle Gang @3= Execution Gang ");


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion
    }
    /// <summary>
    ///  Project Type s
    /// </summary>
    public class ProjectSorts : EntitiesNoName
    {
        /// <summary>
        ///  Project Type 
        /// </summary>
        public ProjectSorts() { }
        /// <summary>
        ///  Get it  Entity
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new ProjectSort();
            }
        }
    }
}
