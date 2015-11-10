using System;
 

namespace BP.Web.Controls
{
	/// <summary>
	///  Button type 
	/// </summary>
	public enum BtnType
	{
		/// <summary>
		///  Confirm  , Need to give 　hit  Assignment . 
		/// ＸＸＸ　 Confirm horse ?
		/// </summary>
		ConfirmHit,
		/// <summary>
		///  Normal 
		/// </summary>
		Normal,			
		/// <summary>
		///  Determine 
		/// </summary>
		Confirm,
		/// <summary>
		///  Save 
		/// </summary>
		Save,
		/// <summary>
		///  Save and New 
		/// </summary>
		SaveAndNew,
		/// <summary>
		///  Find 
		/// </summary>
		Search,
		/// <summary>
		///  Cancel 
		/// </summary>
		Cancel,
		/// <summary>
		///  Delete 
		/// </summary>
		Delete,
		/// <summary>
		///  Update 
		/// </summary>
		Update,
		/// <summary>
		///  Insert 
		/// </summary>
		Insert,
		/// <summary>
		///  Editor 
		/// </summary>
		Edit,
		/// <summary>
		///  New 
		/// </summary>
		New,
		/// <summary>
		///  Browse 
		/// </summary>
		View,
		/// <summary>
		///  Shut down 
		/// </summary>
		Close,
		/// <summary>
		///  Export 
		/// </summary>
		Export,
		/// <summary>
		///  Print 
		/// </summary>
		Print,
		/// <summary>
		///  Increase 
		/// </summary>
		Add,
		/// <summary>
		///  One 
		/// </summary>
		Reomve,
		/// <summary>
		///  Return 
		/// </summary>
		Back,
		/// <summary>
		///  Refresh 
		/// </summary>
		Refurbish,
		/// <summary>
		///  Application task 
		/// </summary>
		ApplyTask,
		/// <summary>
		///  All selected candidates 
		/// </summary>
		SelectAll,
		/// <summary>
		///  Clear All 
		/// </summary>
		SelectNone
	} 
	/// <summary>
	/// TaxBox  Type 
	/// </summary>
	public enum TBType
	{
		/// <summary>
		/// Entities 的DataHelp,  If here described , He is Ens ,  Would indicate DataHelpKey. 
		///  This , The system will appear in his right help .
		/// </summary>
		Ens,
		/// <summary>
		/// Entities 的DataHelp,  If here described , He is Ens ,  Would indicate DataHelpKey. 
		///  This , The system will appear in his right help .
		///  Probably need more than worthy choice . When you select multiple values of time , To use ','  Return them apart . 
		/// </summary>
		EnsOfMany,
		/// <summary>
		///  Custom Types .
		/// </summary>
		Self,
		/// <summary>
		///  Normal 
		/// </summary>
		TB,
		/// <summary>
		/// Num
		/// </summary>
		Num,
		/// <summary>
		/// Int
		/// </summary>
		Int,		 
		/// <summary>
		/// Float
		/// </summary>
		Float,
		/// <summary>
		/// Decimal
		/// </summary>
		Decimal,
		/// <summary>
		/// Moneny
		/// </summary>
		Moneny,
		/// <summary>
		/// Date
		/// </summary>
		Date,
		/// <summary>
		/// DateTime
		/// </summary>
		DateTime,
		/// <summary>
		/// Email
		/// </summary>
		Email,		
		/// <summary>
		/// Key
		/// </summary>
		Key,
		Area

	} 
	/// <summary>
	/// AddAllLocation
	/// </summary>
	public enum AddAllLocation
	{
		/// <summary>
		///  Displayed above 
		/// </summary>
		Top,
		/// <summary>
		///  Displayed below 
		/// </summary>
		End,
		/// <summary>
		///  Displayed above and below 
		/// </summary>
		TopAndEnd,
		/// <summary>
		///  Do not show 
		/// </summary>
		None,
        /// <summary>
        ///  Multiple choice 
        /// </summary>
        TopAndEndWithMVal
	} 
	/// <summary>
	/// DDLShowType
	/// </summary>
	public enum DDLShowType
	{
		/// <summary>
		/// None
		/// </summary>
		None,
		/// <summary>
		/// Gender
		/// </summary>
		Gender,
		/// <summary>
		/// Boolean
		/// </summary>
		Boolean,
		/// <summary>
		/// 
		/// </summary> 
		SysEnum,
		/// <summary>
		/// Self
		/// </summary>
		Self,
		/// <summary>
		///  Entity set 
		/// </summary>
		Ens,
		/// <summary>
		/// 与Table  Associated 
		/// </summary>
		BindTable
	}
	/// <summary>
	/// DDLShowType
	/// </summary>
	public enum DDLShowType_del
	{
		/// <summary>
		/// None
		/// </summary>
		None,
		/// <summary>
		/// Gender
		/// </summary>
		Gender,
		/// <summary>
		/// Boolean
		/// </summary>
		Boolean,
		/// <summary>
		/// Year
		/// </summary>
		Year,
		/// <summary>
		/// Month
		/// </summary>
		Month,
		/// <summary>
		/// Day
		/// </summary>
		Day,
		/// <summary>
		/// hh
		/// </summary>
		hh,
		/// <summary>
		/// mm
		/// </summary>
		mm,
		/// <summary>
		///  Quarter 
		/// </summary>
		Quarter,
		/// <summary>
		/// Week
		/// </summary>
		Week,
		/// <summary>
		///  System enumerated type  SelfBindKey=" System enumeration Key"
		/// </summary>
		SysEnum,
		/// <summary>
		/// Self
		/// </summary>
		Self,
		/// <summary>
		///  Entity set 
		/// </summary>
		Ens,
		/// <summary>
		/// 与Table  Associated 
		/// </summary>
		BindTable
	}
    /// <summary>
    ///  Display mode 
    /// </summary>
    public enum FormShowType
    {
        /// <summary>
        ///  Not set 
        /// </summary>
        NotSet,
        /// <summary>
        ///  Fool form 
        /// </summary>
        FixForm,
        /// <summary>
        ///  Freedom Form 
        /// </summary>
        FreeForm
    }
}