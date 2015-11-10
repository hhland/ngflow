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

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace WorkNode.Classes
{
    public static class Constants
    {
        /// <summary>
        ///  Possible states 
        /// </summary>
        public enum FileStates
        {
            /// <summary>
            ///  Time out 
            /// </summary>
            Pending = 0,
            /// <summary>
            ///  Uploading 
            /// </summary>
            Uploading = 1,
            /// <summary>
            ///  End 
            /// </summary>
            Finished = 2,
            /// <summary>
            ///  Remove 
            /// </summary>
            Deleted = 3,
            /// <summary>
            ///  Error 
            /// </summary>
            Error = 4
        }
    }
}
