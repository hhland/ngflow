using System;
using System.IO;
using System.Collections;
using BP.DA;
using System.Data;
using BP.En;

namespace BP.XML
{
    /// <summary>
    /// XmlEn  The summary .
    /// </summary>
    abstract public class XmlEn
    {
        #region  Gets the value of 
        private Row _row = null;
        public Row Row
        {
            get
            {
                if (this._row == null)
                    throw new Exception("xmlEn  Has not been instantiated .");
                return this._row;
            }
            set
            {
                this._row = value;
            }
        }
        /// <summary>
        ///  Gets an object 
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public Object GetValByKey(string attrKey)
        {
            if (this._row == null)
                return null;

            return this.Row[attrKey];
        }
        public int GetValIntByKey(string key)
        {
            try
            {
                return int.Parse(this.GetValByKey(key).ToString().Trim());
            }
            catch
            {
                throw new Exception("key=" + key + " Not to int  Type Conversion .val=" + this.GetValByKey(key));
            }
        }
        public decimal GetValDecimalByKey(string key)
        {
            return (decimal)this.GetValByKey(key);
        }
        /// <summary>
        ///  Gets an object 
        /// </summary>
        /// <param name="attrKey"></param>
        /// <returns></returns>
        public string GetValStringByKey(string attrKey)
        {
            if (this._row == null)
                return "";

            try
            {
                return this.Row[attrKey].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(" @XMLEN Error Attr=[" + attrKey + "], ClassName= " + this.ToString() + " , File =" + this.GetNewEntities.File + " , Error = " + ex.Message);
            }
        }
        public string GetValStringHtmlByKey(string attrKey)
        {
            return this.GetValStringByKey(attrKey).Replace("\n", "<BR>").Replace(" ", "&nbsp;");
        }
        /// <summary>
        ///  Gets an object 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetValBoolByKey(string key)
        {
            string val = this.GetValStringByKey(key);
            if (val == "1" || val.ToUpper() == "TRUE")
                return true;
            else
                return false;
        }
        #endregion  Gets the value of 

        #region  Constructor 
        /// <summary>
        ///  Constructor 
        /// </summary>
        public XmlEn()
        {
        }

        /// <summary>
        ///  Specify attribute query 
        /// </summary>
        /// <param name="key"> Property Value </param>
        /// <param name="val"> Property </param>
        public int RetrieveBy_del(string key, string val)
        {
            XmlEns ens = this.GetNewEntities;
            ens.RetrieveAll();

            ens.RetrieveBy(key, val);
            if (ens.Count == 0)
                return 0;

            this.Row = ens[0].Row;
            return ens.Count;
        }
        public int RetrieveByPK(string key, string val)
        {
            XmlEns ens = null; // Cash.GetObj(this.GetNewEntities.ToString(), Depositary.Application) as XmlEns;
            if (ens == null)
            {
                ens = this.GetNewEntities;
                ens.RetrieveAll();
            }

            int i = 0;
            foreach (XmlEn en in ens)
            {
                if (en.GetValStringByKey(key) == val)
                {
                    this.Row = en.Row;
                    i++;
                }
            }
            if (i == 1)
                return 1;

            if (i > 1)
            {
               // BP.SystemConfig.DoClearCash();
                throw new Exception("@XML = " + this.ToString() + " 中 PK=" + val + " Not only ....");
            }
            return 0;
        }
        #endregion  Constructor 

        #region  Subclass method needs to be achieved 
        public abstract XmlEns GetNewEntities { get; }
        #endregion  Subclass method needs to be achieved 
    }
    abstract public class XmlEnNoName : XmlEn
    {
        public string No
        {
            get
            {
                return this.GetValStringByKey("No");
            }
        }
        public string Name
        {
            get
            {
                return this.GetValStringByKey("Name");
            }
        }
        public XmlEnNoName()
        {
        }
        public XmlEnNoName(string no)
        {
            this.RetrieveByPK("No", no);
        }
    }
    /// <summary>
    /// XmlEn  The summary .
    /// </summary>
    abstract public class XmlEns : System.Collections.CollectionBase
    {

        #region  Structure 
        /// <summary>
        ///  Structure 
        /// </summary>
        public XmlEns()
        {

        }
        #endregion  Structure 

        #region  Query methods 
        public string Tname
        {
            get
            {
                string tname = this.File.Replace(".TXT", "").Replace(".txt", "");
                tname = tname.Substring(tname.LastIndexOf("\\") + 1) + this.TableName + "_X";
                return tname;
            }
        }

        private DataTable GetTableTxt(DataTable dt, FileInfo file)
        {
            StreamReader sr = new StreamReader(file.FullName, System.Text.ASCIIEncoding.GetEncoding("GB2312"));
            Hashtable ht = new Hashtable();
            string key = "";
            string val = "";
            while (true)
            {
                if (sr.EndOfStream)
                    break;
                string lin = sr.ReadLine();
                if (lin == "" || lin == null)
                    continue;


                if (lin.IndexOf("*") == 0)
                {
                    /*  Encountered annotation file  */
                    continue;
                }

                if (lin.IndexOf("=") == 0 || sr.EndOfStream)
                {


                    /*  Rows agreed ,  Start with  =  That it is the beginning of a new record . */
                    //  Treatment table structure .
                    foreach (string ojbkey in ht.Keys)
                    {
                        if (dt.Columns.Contains(ojbkey) == false)
                        {
                            dt.Columns.Add(new DataColumn(ojbkey, typeof(string)));
                        }
                    }

                    DataRow dr = dt.NewRow();
                    foreach (string ojbkey in ht.Keys)
                    {
                        dr[ojbkey] = ht[ojbkey];
                    }

                    if (ht.Keys.Count > 1)
                        dt.Rows.Add(dr);


                    ht.Clear(); // clear hashtable.
                    if (sr.EndOfStream)
                        break;
                    continue;
                }

                int idx = lin.IndexOf("=");
                if (idx == -1)
                {
                    throw new Exception(this.File + "@ Does not comply with the rules  key =val  Rules .");
                }

                key = lin.Substring(0, idx);
                if (key == "")
                    continue;

                val = lin.Substring(idx + 1);
                ht.Add(key, val);
            }


            return dt;
        }
        public DataTable GetTable()
        {

            DataTable cdt = null; // BP.DA.Cash.GetObj(this.Tname, Depositary.Application) as DataTable;
            if (cdt != null)
                return cdt;

            if (this.File.ToLower().IndexOf(".xml") > 0)
            {
                DataSet ds1 = new DataSet();
                ds1.ReadXml(this.File);
                DataTable mdt = ds1.Tables[this.TableName];
                if (mdt == null)
                    mdt = new DataTable();

                //BP.DA.Cash.AddObj(this.Tname,
                //    Depositary.Application, mdt);
                return ds1.Tables[this.TableName];
            }

            /*  Description This is a directory  */
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(this.File);
            if (di.Exists == false)
                throw new Exception(" File does not exist :" + this.File);

            FileInfo[] fis = di.GetFiles("*.xml");
          
            DataTable dt = new DataTable(this.TableName);
            if (fis.Length == 0)
                return dt;

            DataTable tempDT = new DataTable();
            foreach (FileInfo fi in fis)
            {

                DataSet ds = new DataSet("myds");
                try
                {
                    ds.ReadXml(this.File + "\\" + fi.Name);
                }
                catch (Exception ex)
                {
                    throw new Exception(" Read the file :" + fi.Name + " Error .Exception=" + ex.Message);
                }
                try
                {
                    //ds.
                    if (dt.Columns.Count == 0)
                    {
                        /*  If the table is empty , No Structure .*/
                        try
                        {
                            dt = ds.Tables[this.TableName];
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(" May not be in " + fi.Name + " File, find the table :" + this.TableName + " exception=" + ex.Message);
                        }
                        tempDT = dt.Clone();
                        continue;
                    }

                    DataTable mydt = ds.Tables[this.TableName];
                    if (mydt == null)
                        throw new Exception(" No such table :" + this.TableName);

                    if (mydt.Rows.Count == 0)
                        continue;

                    foreach (DataRow mydr in mydt.Rows)
                    {
                        //dt.ImportRow(mydr);
                        DataRow dr = dt.NewRow();

                        foreach (DataColumn dc in tempDT.Columns)
                        {
                            //string "sd".Clone();
                            if (dc.ColumnName.IndexOf("_Id") != -1)
                                continue;

                            try
                            {
                                object obj = mydr[dc.ColumnName];
                                dr[dc.ColumnName] = obj;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("xml  Configuration error , Multiple file attributes asymmetry ." + ex.Message);
                            }
                        }

                        dt.Rows.Add(dr);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(" Get data errors :fileName=" + fi.Name + " clasName=" + this.ToString() + " MoreInfo=" + ex.Message);
                }
            }
            //BP.DA.Cash.AddObj(this.Tname,
            //    Depositary.Application,
            //    dt);
            return dt;
        }
        public virtual int RetrieveAllFromDBSource()
        {
           // BP.DA.Cash.RemoveObj(this.Tname);
            return this.RetrieveAll();
        }
        /// <summary>
        ///  Load XML
        /// </summary>
        public virtual int RetrieveAll()
        {
            this.Clear(); //  Clear all information .
            XmlEns ens = null;// BP.DA.Cash.GetObj(this.ToString(), Depositary.Application) as XmlEns;
            if (ens != null)
            {
                foreach (XmlEn en in ens)
                    this.Add(en);
                return ens.Count;
            }

            //  Looking from memory .
            DataTable dt = this.GetTable();
            foreach (DataRow dr in dt.Rows)
            {
                XmlEn en = this.GetNewEntity;
                en.Row = new Row(dt, dr);
                this.Add(en);
            }

           // BP.DA.Cash.AddObj(this.ToString(), Depositary.Application, this);
            return dt.Rows.Count;
        }
       

        public int RetrieveByLength(string key, int len)
        {
            this.Clear(); // Clear all information 
            DataTable dt = this.GetTable();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[key].ToString().Length == len)
                {
                    XmlEn en = this.GetNewEntity;
                    en.Row = new Row(dt, dr);
                    this.Add(en);
                    i++;
                }
            }
            return i;
        }

        /// <summary>
        ///  According to the key inquiry 
        /// </summary>
        /// <param name="key"> To query the health </param>
        /// <param name="val">值</param>
        /// <returns> Returns the number of queries </returns>
        public int RetrieveBy(string key, object val)
        {
            if (val == null)
                return 0;

            this.Clear(); // Clear all information 
            DataTable dt = this.GetTable();
            if (dt == null)
                throw new Exception("@ Error :类" + this.GetNewEntity.ToString() + " File= " + this.File + " Table=" + this.TableName + " , Did not get to the data .");

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[key].ToString() == val.ToString())
                {
                    XmlEn en = this.GetNewEntity;
                    en.Row = new Row(dt, dr);
                    this.Add(en);
                    i++;
                }
            }
            return i;
        }

        public int RetrieveBy(string key, object val, string orderByAttr)
        {
            DataTable dt = this.GetTable();
            DataView dv = new DataView(dt, orderByAttr, orderByAttr, DataViewRowState.Unchanged);

            this.Clear(); // Clear all information .
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[key].ToString() == val.ToString())
                {
                    XmlEn en = this.GetNewEntity;
                    en.Row = new Row(dt, dr);
                    this.Add(en);
                    i++;
                }
            }
            return i;
        }
        #endregion

        #region  Public Methods 
        public XmlEn Find(string key, object val)
        {
            foreach (XmlEn en in this)
            {
                if (en.GetValStringByKey(key) == val.ToString())
                    return en;
            }
            return null;

        }
        public bool IsExits(string key, object val)
        {
            foreach (XmlEn en in this)
            {
                if (en.GetValStringByKey(key) == val.ToString())
                    return true;
            }
            return false;
        }
        #endregion


        #region   Increase   Convenient access 
        public XmlEn GetEnByKey(string key, string val)
        {
            foreach (XmlEn en in this)
            {
                if (en.GetValStringByKey(key) == val)
                    return en;
            }
            return null;

        }
        /// <summary>
        ///  Access to data based on location 
        /// </summary>
        public XmlEn this[int index]
        {
            get
            {
                return (XmlEn)this.InnerList[index];
            }
        }
        /// <summary>
        ///  Get Data 
        /// </summary>
        public XmlEn this[string key, string val]
        {
            get
            {
                foreach (XmlEn en in this)
                {
                    if (en.GetValStringByKey(key) == val)
                        return en;
                }
                throw new Exception("在[" + this.TableName + "," + this.File + "," + this.ToString() + "] Not found key=" + key + ", val=" + val + " Examples .");
            }
        }
        /// <summary>
        ///  Adding a xml en to Ens.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int Add(XmlEn entity)
        {
            return this.InnerList.Add(entity);
        }
        #endregion


        #region  Subclass implementation xml  Description information .
        public abstract XmlEn GetNewEntity 
        { get; }
        /// <summary>
        ///  File 
        /// </summary>
        protected string _File = null;
        /// <summary>
        ///  Get it in the xml file  Location .
        /// </summary>
        public abstract string File
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        protected string _TableName = null;
        /// <summary>
        ///  Physical table name ( Probably a xml File has n个Table.)
        /// </summary>
        public abstract string TableName
        {
            get;
        }
        #endregion
    }



}
