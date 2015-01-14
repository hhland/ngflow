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

namespace CCForm
{
    /// <summary>
    ///  Database Type 
    /// </summary>
    public class DBType
    {
        /// <summary>
        /// MSSQL
        /// </summary>
        public const string MSSQL = "MSSQL";
        /// <summary>
        /// Oracle
        /// </summary>
        public const string Oracle = "Oracle";
        /// <summary>
        /// MySQL
        /// </summary>
        public const string MySQL = "MySQL";
        /// <summary>
        /// Infomix
        /// </summary>
        public const string Infomix = "Infomix";
    }
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
    ///  Type 
    /// </summary>
    public enum TBType
    {
        String,
        Int,
        Float,
        Money,
        DateTime,
        Date,
        Boolean
    }

    /// <summary>
    ///  Cursor state 
    /// </summary>
    public enum MousePosition
    {
        None = 0, //'无      
        SizeRight = 1, //' Stretching right border       
        SizeLeft = 2, //' Stretch the left border       
        SizeTop = 4, //' Tension on the border       
        SizeTopLeft = 5, //' Stretch the upper left corner       
        SizeTopRight = 6, //' Stretch the upper right corner    
        SizeBottom = 3, //' Stretching under the border    
        SizeBottomLeft = 7, //' Stretching the lower left corner       
        SizeBottomRight = 8, //' Stretching the lower right corner       
        Drag = 9   // ' Drag the mouse     
    }

    public class EEleTableNames
    {
        public const string
            Sys_FrmLine = "Sys_FrmLine",
            Sys_FrmBtn = "Sys_FrmBtn",
            Sys_FrmLab = "Sys_FrmLab",
            Sys_FrmLink = "Sys_FrmLink",
            Sys_FrmImg = "Sys_FrmImg",
            Sys_FrmEle = "Sys_FrmEle",
            Sys_FrmImgAth = "Sys_FrmImgAth",
            Sys_FrmRB = "Sys_FrmRB",
            Sys_FrmAttachment = "Sys_FrmAttachment",

            Sys_MapData = "Sys_MapData",
            Sys_MapAttr = "Sys_MapAttr",
            Sys_MapDtl = "Sys_MapDtl",
            Sys_MapM2M = "Sys_MapM2M",

            WF_Node = "WF_Node";//BPWorkCheck
    }

    // 1） With respect to the designer UI, Operating status of all new elements are added 
    // 2） If the design is first loaded , After loading the state have changed Default, Not processed when saving 
    // 3） If it is the new element , Element status quo , Traversal generates belt saved  insert  Statement 
    // 4） Elements deleted , Now delete the implementation element method , Carried out  delete  Statement , Update  UI
    // 5） Editing elements , Element status Update, Traversal generated when you save  update  Statement 
    //public enum OperateState
    //{
    //    Added,
    //    Updated,
    //    Deleted,
    //    Default
    //}
}
