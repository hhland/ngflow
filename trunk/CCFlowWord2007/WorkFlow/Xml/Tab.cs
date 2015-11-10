using System;
using System.Collections.Generic;
using System.Text;

namespace BP.WF
{
    /// <summary>
    ///  Function 
    /// </summary>
    public class Tab : BP.XML.XmlEnNoName
    {
      
        public override BP.XML.XmlEns GetNewEntities
        {
            get
            {
                return new Tabs();
            }
        }
    }
    /// <summary>
    ///  Feature set 
    /// </summary>
    public class Tabs : BP.XML.XmlEns
    {
        public override BP.XML.XmlEn GetNewEntity
        {
            get { return new Tab(); }
        }
        /// <summary>
        ///  File 
        /// </summary>
        public override string File
        {
            get
            {
                return @"D:\ccflow\value-added\CCFlowWord2007\Toolbar.xml";
            }
        }
        /// <summary>
        /// 表
        /// </summary>
        public override string TableName
        {
            get { return "Tab"; }
        }
    }
}
