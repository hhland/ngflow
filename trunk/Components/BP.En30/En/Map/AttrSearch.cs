using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace BP.En
{
    /// <summary>
    ///  Query Properties 
    /// </summary>
    public class AttrSearch
    {
        /// <summary>
        ///  Query Properties 
        /// </summary>
        public Attr HisAttr = null;
        /// <summary>
        ///  Whether to show all 
        /// </summary>
        public bool IsShowAll = true;
        /// <summary>
        ///  And associated sub-menu 
        /// </summary>
        public string RelationalDtlKey = null;
        public string Key = null;
        public AttrSearch()
        {
        }
    }
    /// <summary>
    ///  Query Properties s
    /// </summary>
    public class AttrSearchs : CollectionBase
    {
        public AttrSearchs()
        {
        }
        public void Add(Attr attr, bool isShowSelectedAll, string relationalDtlKey)
        {
            AttrSearch en = new AttrSearch();
            en.HisAttr = attr;
            en.IsShowAll = isShowSelectedAll;
            en.RelationalDtlKey = relationalDtlKey;
            en.Key = attr.Key;
            this.InnerList.Add(en);
        }

        public void Add(AttrSearch attr)
        {
            this.InnerList.Add(attr);
        }
    }
}
