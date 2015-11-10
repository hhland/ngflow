using System;
using System.Data;
using System.Collections;
using BP.En;

namespace BP.En
{
	/// <summary>
	/// Row  The summary .
	///  To deal with a record store , Issue .
	/// </summary>
	public class Row : Hashtable
	{
        /// <summary>
        /// 
        /// </summary>
		public Row()
		{

		}
		/// <summary>
		///  Data description constructs a table ( For xmlEn)
		/// </summary>
		/// <param name="dt"></param>
		public Row(DataTable dt, DataRow dr)
		{
			this.Clear();
			foreach( DataColumn dc in dt.Columns ) 
			{
				this.Add(dc.ColumnName, dr[dc.ColumnName] );
			}
		}

		/// <summary>
		///  Setting a value by key . 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public void SetValByKey(string key, object val)
		{
			//this.Values[].Add(key,val);
			if (val.GetType()== typeof( TypeCode ) )
				this[key]=(int)val;
			else
				this[key]=val;
		}
        public object GetValByKey(string key)
        {
            return this[key];

            /*
            if (SystemConfig.IsDebug)
            {
                try
                {
                    return this[key];
                }
                catch(Exception ex)
                {
                    throw new Exception("@GetValByKey Not found key="+key+" Properties Vale ,  Please confirm Map  Inside whether this property ."+ex.Message);
                }
            }
            else
            {
                return this[key];

            }
            */
        }

	}
	/// <summary>
	/// row  Set 
	/// </summary>
	public class Rows : System.Collections.CollectionBase
	{
		public Rows()
		{
		}
		public Row this[int index]
		{
			get 
			{
				return (Row)this.InnerList[index];
			}
		}	 
		/// <summary>
		///  Adding a Row .
		/// </summary>
		/// <param name="r">row</param>
		public void Add(Row r)
		{
			this.InnerList.Add(r);
		}
	}
}
