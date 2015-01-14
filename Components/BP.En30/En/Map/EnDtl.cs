using System; 
using System.Collections;
using BP.DA; 
using BP.Web.Controls;

namespace BP.En
{
	/// <summary>
	/// EnDtl  The summary .
	/// </summary>
	public class EnDtl
	{

	
		/// <summary>
		///  Details 
		/// </summary>
		public EnDtl()
		{			  
		}
		/// <summary>
		///  Details 
		/// </summary>
		/// <param name="className"> The class name </param>
		public EnDtl(string className)
		{
			this.Ens=ClassFactory.GetEns(className);
		}
		/// <summary>
		///  The class name 
		/// </summary>
		public string EnsName
		{
			get
			{
				return this.Ens.ToString();
			}
		}
		/// <summary>
		///  Details 
		/// </summary>
		public Entities _Ens=null;
		/// <summary>
		///  Gets or sets   His collection 
		/// </summary>
		public Entities Ens
		{
			get
			{
				return _Ens;
			}
			set			
			{
				_Ens=value;
			}
		}
		/// <summary>
		///  He connected the key
		/// </summary>
		private string _refKey=null;
		/// <summary>
		///  He connected the  key
		/// </summary>
		public string RefKey
		{
			get
			{
				return _refKey;
			}
			set
			{
				this._refKey =value; 
			}
		}
		/// <summary>
		///  Description 
		/// </summary>
		private string _Desc=null;
		/// <summary>
		///  Description 
		/// </summary>
		public string Desc
		{
			get
			{
				if (this._Desc==null)
					this._Desc=this.Ens.GetNewEntity.EnDesc;
				return _Desc;
			}
			set
			{
				_Desc=value;
			}
		}
	}
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class EnDtls: CollectionBase
	{
		/// <summary>
		///  Is not included className
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public bool IsContainKey(string className)		
		{
			foreach(EnDtl ed in this)
			{
				if (ed.EnsName==className)
					return true;
			}
			return false;
		}	 
		/// <summary>
		///  Join 
		/// </summary>
		/// <param name="attr">attr</param>
		public void Add(EnDtl en)
		{
			 if (this.IsExits(en))
				 return ;
			this.InnerList.Add(en);
		}
		/// <summary>
		///  Is there a collection of inside 
		/// </summary>
		/// <param name="en"> To check the EnDtl</param>
		/// <returns>true/false</returns>
		public bool IsExits(EnDtl en)
		{
			foreach (EnDtl dtl in this )
			{
				if (dtl.Ens== en.Ens )
				{
					return true;
				}
			}
			return false;
		}
	 
		/// <summary>
		///  Through a key  Get its property values .
		/// </summary>
		/// <param name="key">key</param>
		/// <returns>EnDtl</returns>
		public EnDtl GetEnDtlByKey(string key)
		{		
			foreach (EnDtl dtl in this )
			{
				if (dtl.RefKey.Equals(key))
				{
					return dtl;
				}
			}
			throw new Exception("@ Not found  key=["+key+"] Properties , Please check map File .");
		}
		/// <summary>
		///  According to the index to access elements within the collection Attr.
		/// </summary>
		public EnDtl this[int index]
		{			
			get
			{	
				return (EnDtl)this.InnerList[index];
			}
		}
		/// <summary>
		/// className
		/// </summary>
		/// <param name="className"> The class name </param>
		/// <returns></returns>
		public EnDtl GetEnDtlByEnsName(string className)
		{
			foreach( EnDtl en in this)
			{
				if (en.EnsName==className)
					return en;
			}
			throw new Exception("@ He did not find the detail :"+className);
		}
		
	}
}
