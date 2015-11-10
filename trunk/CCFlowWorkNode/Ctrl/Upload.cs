using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace WorkNode
{
    /// <summary>
    ///  Accessories upload type 
    /// </summary>
    public enum AttachmentUploadType
    {
        /// <summary>
        ///  Single 
        /// </summary>
        Single,
        /// <summary>
        ///  Multiple 
        /// </summary>
        Multi,
        /// <summary>
        ///  Specified 
        /// </summary>
        Specifically
    }
    /// <summary>
    /// 
    /// </summary>
    public class FilesClass
    {
        public FilesClass()
        {

        }

        string strStatus = "";
        string strNo = "";
        FileInfo strFileName = null;

        public string PropNumber
        {
            get
            {
                return strNo;
            }
            set
            {
                strNo = value;
            }
        }

        public FileInfo PropFileName
        {
            get
            {
                return strFileName;
            }
            set
            {
                strFileName = value;
            }
        }

        public string PropStatus
        {
            get
            {
                return strStatus;
            }
            set
            {
                strStatus = value;
            }
        }
    }
}
