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

namespace BP.SL
{
    public delegate void CallBack();
    /// <summary>
    ///  After the successful implementation of a callback function 
    /// </summary>
    public interface ICallBack
    {
        /// <summary>
        ///  After the successful implementation of a callback function 
        /// </summary>
        event CallBack DoCompleted;
    }
}
