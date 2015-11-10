using System;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Sys;
using BP.XML;

namespace BP.Web.Port.Xml
{
    public class HelpAttr
    {
        /// <summary>
        ///  Serial number 
        /// </summary>
        public const string No = "No";
        /// <summary>
        ///  Name 
        /// </summary>
        public const string Name = "Name";
        public const string HelpFile = "HelpFile";
        public const string Img = "Img";

    }
	/// <summary>
	/// 
	/// </summary>
	public class Help:XmlEn
	{
		#region  Property 
        public string Parent
        {
            get
            {
                switch (this.Grade)
                {
                    case 1:
                        return null;
                    case 2:
                        return this.No.Substring(0, 2);
                    case 3:
                        return this.No.Substring(0, 4);
                    default:
                        return null;
                }
            }
        }
        public int Grade
        {
            get
            {
                return this.No.Length / 2;
            }
        }
        public string Img
        {
            get
            {
                string str = this.GetValStringByKey(HelpAttr.Img);
                if (str == null || str == "")
                    if (this.No.Length >= 4)
                        return "/WF/Images/Pub/BillOpen.gif";
                    else
                        return "/WF/Images/Pub/Bill.gif";
                else
                    return str;
            }
        }
        public string HelpFile
        {
            get
            {
                return this.GetValStringByKey(HelpAttr.HelpFile);
            }
        }
		/// <summary>
		///  Serial number 
		/// </summary>
		public string No
		{
			get
			{
				return this.GetValStringByKey(HelpAttr.No).Trim();
			}
		}
		/// <summary>
		///  Name 
		/// </summary>
		public string Name
		{
			get
			{
				return this.GetValStringByKey(HelpAttr.Name);
			}
		}
		#endregion

		#region  Structure 
		public Help()
		{
		}
		/// <summary>
		///  Serial number 
		/// </summary>
		/// <param name="no"></param>
		public Help(string no)
		{
		}
		/// <summary>
		///  Get an instance of 
		/// </summary>
        public override XmlEns GetNewEntities
        {
            get
            {
                return new Helps();
            }
        }
		#endregion
	}
	/// <summary>
	/// 
	/// </summary>
	public class Helps:XmlEns
	{
		#region  Structure 
		/// <summary>
		///  Assessment rate data elements 
		/// </summary>
		public Helps(){}
		#endregion

		#region  Override the base class property or method .
		/// <summary>
		///  Get it  Entity 
		/// </summary>
		public override XmlEn GetNewEntity
		{
			get
			{
				return new Help();
			}
		}
		public override string File
		{
			get
			{
                return SystemConfig.PathOfWebApp + "\\Helper\\Help.xml";
			}
		}
		/// <summary>
		///  Physical table name 
		/// </summary>
		public override string TableName
		{
			get
			{
				return "Help";
			}
		}
		public override Entities RefEns
		{
			get
			{
				return null; //new BP.ZF1.Helps();
			}
		}
		#endregion
		 
	}
}
