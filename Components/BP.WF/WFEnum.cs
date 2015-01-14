using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  User Information Display Format 
    /// </summary>
    public enum UserInfoShowModel
    {
        /// <summary>
        ///  User ID, Username 
        /// </summary>
        UserIDUserName = 0,
        /// <summary>
        ///  User ID
        /// </summary>
        UserIDOnly = 1,
        /// <summary>
        ///  Username 
        /// </summary>
        UserNameOnly = 2
    }
    /// <summary>
    ///  Organizational structure model 
    /// </summary>
    public enum OSModel
    {
        WorkFlow = 0,
        BPM = 1
    }
}
