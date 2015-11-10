using System;
using System.Data;
using System.Collections;
namespace BP.En
{
	/// <summary>
	/// Row  The summary .
	///  To deal with a record 
	/// </summary>
	public class Row : Hashtable
	{
        public Row():base(System.StringComparer.Create(System.Globalization.CultureInfo.CurrentCulture, true))
        {
        }
        /// <summary>
        ///  Initialization data .
        /// </summary>
        /// <param name="attrs"></param>
        public void LoadAttrs(Attrs attrs)
        {
            this.Clear();
            foreach (Attr attr in attrs)
            {
                switch (attr.MyDataType)
                {
                    case BP.DA.DataType.AppInt:
                        if (attr.IsNull)
                            this.Add(attr.Key, DBNull.Value);
                        else
                            this.Add(attr.Key, int.Parse(attr.DefaultVal.ToString()));
                        break;
                    case BP.DA.DataType.AppFloat:
                        if (attr.IsNull)
                            this.Add(attr.Key, DBNull.Value);
                        else
                            this.Add(attr.Key, float.Parse(attr.DefaultVal.ToString()));
                        break;
                    case BP.DA.DataType.AppMoney:
                        if (attr.IsNull)
                            this.Add(attr.Key, DBNull.Value);
                        else
                            this.Add(attr.Key, decimal.Parse(attr.DefaultVal.ToString()));
                        break;
                    case BP.DA.DataType.AppDouble:
                        if (attr.IsNull)
                            this.Add(attr.Key, DBNull.Value);
                        else
                            this.Add(attr.Key, double.Parse(attr.DefaultVal.ToString()));
                        break;
                    default:
                        this.Add(attr.Key, attr.DefaultVal);
                        break;
                }
            }
        }
        /// <summary>
        /// LoadAttrs
        /// </summary>
        /// <param name="attrs"></param>
        public void LoadDataTable(DataTable dt, DataRow dr)
        {
            this.Clear();
            foreach (DataColumn dc in dt.Columns)
            {
                this.Add(dc.ColumnName, dr[dc.ColumnName]);
            }
        }

		/// <summary>
		///  Setting a value by key . 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
        public void SetValByKey(string key, object val)
        {
            if (val == null)
            {
                this[key] = val;
                return;
            }

            if (val.GetType() == typeof(TypeCode))
                this[key] = (int)val;
            else
                this[key] = val;
        }


        public bool GetBoolenByKey(string key)
        {
            object obj = this[key];
            if (obj == null || string.IsNullOrEmpty(obj.ToString())==true || obj.ToString()=="0")
                return false;
            return true;
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
