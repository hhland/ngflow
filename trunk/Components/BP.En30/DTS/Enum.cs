using System;

namespace BP.DTS
{
	/// <summary>
	///  Run Type го
	/// </summary>
	public enum RunTimeType
	{
		/// <summary>
		///  Per minute 
		/// </summary>
		Minute,
		/// <summary>
		///  Per hour 
		/// </summary>
		Hour,
		/// <summary>
		///  Every day 
		/// </summary>
		Day,
		/// <summary>
		///  Per month 
		/// </summary>
		Month,		
		/// <summary>
		///  Not specified 
		/// </summary>
		UnName
	}
	/// <summary>
	///  Run Type 
	/// </summary>
    public enum RunType
    {
        /// <summary>
        ///  The middle layer method 
        /// </summary>
        Method,
        /// <summary>
        /// SQL Text 
        /// </summary>
        SQL,
        /// <summary>
        ///  Stored Procedures 
        /// </summary>
        SP,
        /// <summary>
        ///  Data scheduling 
        /// </summary>
        DataIO
    }
}
