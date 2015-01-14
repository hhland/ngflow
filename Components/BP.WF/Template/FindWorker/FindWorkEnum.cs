using System;
using System.Collections.Generic;
using System.Text;

namespace BP.WF.Template
{
    /// <summary>
    ///  Looking for leadership type 
    /// </summary>
    public enum FindLeaderType
    {
        /// <summary>
        ///  Author 
        /// </summary>
        Submiter,
        /// <summary>
        ///  The author of the specified node 
        /// </summary>
        SpecNodeSubmiter,
        /// <summary>
        ///  The author of a particular field 
        /// </summary>
        BySpecField
    }
    /// <summary>
    ///  Looking for Leadership 
    /// </summary>
    public enum FindLeaderModel
    {
        /// <summary>
        ///  Direct leadership 
        /// </summary>
        DirLeader,
        /// <summary>
        ///  Specifies the level of leadership positions 
        /// </summary>
        SpecDutyLevelLeader,
        /// <summary>
        ///  Specific leadership positions 
        /// </summary>
        DutyLeader,
        /// <summary>
        ///  Specific job 
        /// </summary>
        SpecStation
    }
}
