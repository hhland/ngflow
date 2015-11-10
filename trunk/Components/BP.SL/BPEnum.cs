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

namespace BP.En
{
    /// <summary>
    ///  Button type 
    /// </summary>
    public enum EventType
    {
        /// <summary>
        ///  Disable 
        /// </summary>
        Disable = 0,
        /// <summary>
        ///  Run the stored procedure 
        /// </summary>
        RunSP = 1,
        /// <summary>
        ///  Run sql
        /// </summary>
        RunSQL = 2,
        /// <summary>
        ///  Carried out URL
        /// </summary>
        RunURL = 3,
        /// <summary>
        ///  Run webservices
        /// </summary>
        RunWS = 4,
        /// <summary>
        ///  Run Exe File .
        /// </summary>
        RunExe =5,
        /// <summary>
        ///  Run JS
        /// </summary>
        RunJS
    }
    /// <summary>
    ///  Button type 
    /// </summary>
    public enum BtnType
    {
        /// <summary>
        ///  Save 
        /// </summary>
        Save = 0,
        /// <summary>
        ///  Print 
        /// </summary>
        Print = 1,
        /// <summary>
        ///  Delete 
        /// </summary>
        Delete = 2,
        /// <summary>
        ///  Increase 
        /// </summary>
        Add = 3,
        /// <summary>
        ///  Custom 
        /// </summary>
        Self = 100
    }
    /// <summary>
    ///  Edit Type 
    /// </summary>
    public enum EditType
    {
        /// <summary>
        ///  Editable 
        /// </summary>
        Edit,
        /// <summary>
        ///  Can not be deleted 
        /// </summary>
        UnDelete,
        /// <summary>
        ///  Read-only , Can not be deleted 
        /// </summary>
        Readonly
    }
}
