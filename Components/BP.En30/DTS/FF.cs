using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls ; 

namespace BP.DTS
{ 
	/// <summary>
	///  Property 
	/// </summary>
	public class FF
	{
		/// <summary>
		///  From the field 
		/// </summary>
		public string FromField=null;
		/// <summary>
		///  To field 
		/// </summary>
		public string ToField=null;
		/// <summary>
		///  Data Source Type 
		/// </summary>
		public int DataType=1;//DataType.AppString;		
		/// <summary>
		///  Whether it is the primary key 
		/// </summary>
		public bool IsPK=false;
		public FF()
		{
		}
		/// <summary>
		///  Structure 
		/// </summary>
		/// <param name="from">´Ó</param>
		/// <param name="to">µ½</param>
		/// <param name="datatype"> Data Types </param>
		/// <param name="isPk"> Whether PK</param>
		public FF(string from,string to,int datatype, bool isPk)
		{
			this.FromField=from;
			this.ToField=to;
			this.DataType=datatype;
			this.IsPK=isPk;
		}
	}
	/// <summary>
	///  Properties collection 
	/// </summary>
	[Serializable]
	public class FFs: CollectionBase
	{
        public int PKCount
        {
            get
            {
                int i = 0;
                foreach (FF ff in this)
                {
                    if (ff.IsPK)
                        i++;
                }
                if (i == 0)
                    throw new Exception(" Not Set PK.  Please check map  Error .");
                return i;
            }
        }
		/// <summary>
		///  Properties collection 
		/// </summary>
		public FFs(){}
		/// <summary>
		///  Join a property 
		/// </summary>
		/// <param name="attr"></param>
		public void Add(FF ff)
		{
			this.InnerList.Add(ff);
		}
		/// <summary>
		///  Adding a data mapping 
		/// </summary>
		/// <param name="fromF"></param>
		/// <param name="toF"></param>
		/// <param name="dataType"></param>
		/// <param name="isPk"></param>
		public void Add(string fromF,string toF, int dataType, bool isPk)
		{
			this.Add( new FF( fromF , toF , dataType ,isPk ) );
		}
		 

		/// <summary>
		///  According to the index to access elements within the collection Attr.
		/// </summary>
		public FF this[int index]
		{			
			get
			{	
				return (FF)this.InnerList[index];
			}
		}
	}	
}
