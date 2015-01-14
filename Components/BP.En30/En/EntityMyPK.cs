using System;
using System.Collections;
using BP.DA;

namespace BP.En
{
	/// <summary>
	/// EntityMyPKAttr
	/// </summary>
    public class EntityMyPKAttr
    {
        /// <summary>
        /// className
        /// </summary>
        public const string MyPK = "MyPK";
        public const string NoInt = "NoInt";
    }
	/// <summary>
	/// NoEntity  The summary .
	/// </summary>
    abstract public class EntityMyPK : Entity
    {
        #region  Basic properties 
        public override string PK
        {
            get
            {
                return "MyPK";
            }
        }
        /// <summary>
        ///  Collection class name 
        /// </summary>
        public string MyPK
        {
            get
            {
                return this.GetValStringByKey(EntityMyPKAttr.MyPK);
            }
            set
            {
                this.SetValByKey(EntityMyPKAttr.MyPK, value);
            }
        }
        #endregion

        #region  Structure 
        public EntityMyPK()
        {
        }
        /// <summary>
        /// class Name 
        /// </summary>
        /// <param name="_MyPK">_MyPK</param>
        protected EntityMyPK(string _MyPK)
        {
            this.MyPK = _MyPK;
            this.Retrieve();
        }
        #endregion
    }
    /// <summary>
    /// EntityMyPKMyFile
    /// </summary>
    abstract public class EntityMyPKMyFile : EntityMyPK
    {
        #region  Property 
        public override string PK
        {
            get
            {
                return "MyPK";
            }
        }
        public string MyFilePath
        {
            get
            {
                return this.GetValStringByKey("MyFilePath");
            }
            set
            {
                this.SetValByKey("MyFilePath", value);
            }
        }
        public string MyFileExt
        {
            get
            {
                return this.GetValStringByKey("MyFileExt");
            }
            set
            {
                this.SetValByKey("MyFileExt", value.Replace(".","") );
            }
        }
        public string MyFileName
        {
            get
            {
                return this.GetValStringByKey("MyFileName");
            }
            set
            {
                this.SetValByKey("MyFileName", value);
            }
        }
        public Int64 MyFileSize
        {
            get
            {
                return this.GetValInt64ByKey("MyFileSize");
            }
            set
            {
                this.SetValByKey("MyFileSize", value);
            }
        }
        public int MyFileH
        {
            get
            {
                return this.GetValIntByKey("MyFileH");
            }
            set
            {
                this.SetValByKey("MyFileH", value);
            }
        }
        public int MyFileW
        {
            get
            {
                return this.GetValIntByKey("MyFileW");
            }
            set
            {
                this.SetValByKey("MyFileW", value);
            }
        }
        public bool IsImg
        {
            get
            {
                return DataType.IsImgExt(this.MyFileExt);
            }
        }
        #endregion

        #region  Structure 
        public EntityMyPKMyFile()
		{
		}
		/// <summary>
		/// class Name 
		/// </summary>
		/// <param name="_MyPK">_MyPK</param>
        protected EntityMyPKMyFile(string _MyPK)
        {
            this.MyPK = _MyPK;
            this.Retrieve();
        }
		#endregion
    }
	/// <summary>
	///  Entity set class name 
	/// </summary>
    abstract public class EntitiesMyPK : Entities
    {
        /// <summary>
        ///  Entity set 
        /// </summary>
        public EntitiesMyPK()
        {
        }
    }
}
