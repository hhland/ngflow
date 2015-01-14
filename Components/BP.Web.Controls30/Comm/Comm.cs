using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace BP.Web.Comm
{
    public enum UIRowStyleGlo
    {
        /// <summary>
        ///  No style 
        /// </summary>
        None,
        /// <summary>
        ///  Alternate style 
        /// </summary>
        Alternately,
        /// <summary>
        ///  Mouse movement 
        /// </summary>
        Mouse,
        /// <summary>
        ///  Mouse movements and alternately 
        /// </summary>
        MouseAndAlternately
    }
    public enum ActionType
    {
        /// <summary>
        ///  Do not do anything 
        /// </summary>
        None = 0,
        /// <summary>
        ///  Delete files 
        /// </summary>
        DeleteFile = 1,
        /// <summary>
        ///  Print documents only one entity 
        /// </summary>
        PrintEnBill = 2
    }

    public enum ShowWay
    {
        /// <summary>
        ///  Reduction off the map 
        /// </summary>
        Cards,
        /// <summary>
        ///  List 
        /// </summary>
        List,
        /// <summary>
        ///  Details 
        /// </summary>
        Dtl
    }

}
 