using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BP.DA;
using BP.Port; 
using BP.En;

namespace BP.PRJ
{
    /// <summary>
    ///  File Data Description 
    /// </summary>
    public class FileDataAttr : EntityNoNameAttr
    {
        /// <summary>
        /// My_PK
        /// </summary>
        public const string My_PK = "My_PK";
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string OID = "OID";
        /// <summary>
        ///  File Type 
        /// </summary>
        public const string FileType = "FileType";
        /// <summary>
        ///  File name 
        /// </summary>
        public const string FileName = "FileName";
        /// <summary>
        ///  Path 
        /// </summary>
        public const string AbsolutionPath = "AbsolutionPath";
        /// <summary>
        ///  File Format 
        /// </summary>
        public const string FileFormat = "FileFormat";
        /// <summary>
        ///  File size 
        /// </summary>
        public const string FileSize = "FileSize";
        /// <summary>
        ///  Uploaded 
        /// </summary>
        public const string UpLoadDate = "UpLoadDate";
        /// <summary>
        ///  Upload people 
        /// </summary>
        public const string UpLoadPerson = "UpLoadPerson";
        /// <summary>
        ///  Upload node name 
        /// </summary>
        public const string NodeAttrbute = "NodeAttrbute";
        /// <summary>
        ///  Process ID
        /// </summary>
        public const string FlowID = "FlowID";
    }
    /// <summary>
    ///  File Data Sheet 
    /// </summary>
    public class FileData : EntityNoName
    {
        /// <summary>
        /// FileData
        /// </summary>
        public FileData()
        {
        }
        /// <summary>
        ///  File Description 
        /// </summary>
        public FileData(string no)
        {
            this.No = no;
            this.Retrieve();
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

                Map map = new Map("PRJ_FileData");
                map.EnDesc = " File Data Sheet ";
                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.CodeStruct = "2";
                map.IsAutoGenerNo = true;

                map.AddTBStringPK(FileDataAttr.My_PK, null, "My_PK", true, false, 10, 10, 10);
                map.AddTBInt(FileDataAttr.OID, 0, "OID", true, false);
                map.AddTBString(FileDataAttr.FileName, null, " File name ", true, false, 0, 60, 500);
                map.AddTBString(FileDataAttr.AbsolutionPath, null, " Path ", true, false, 0, 60, 500);
                map.AddTBString(FileDataAttr.FileFormat, null, " File Format ", true, false, 0, 60, 500);
                map.AddTBString(FileDataAttr.FileSize, null, " File size ", true, false, 0, 60, 500);
                map.AddTBDate(FileDataAttr.UpLoadDate, null, " Uploaded ", true, false);
                map.AddTBString(FileDataAttr.UpLoadPerson, null, " Upload people ", true, false, 0, 60, 500);
                map.AddTBString(FileDataAttr.NodeAttrbute, null, " Upload node name ", true, false, 0, 60, 500);
                map.AddTBInt(FileDataAttr.FlowID, 0, "FlowID", true, false);

                this._enMap = map;
                return this._enMap;
            }
        }
    }
}
