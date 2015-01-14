using System;
using System.Collections;

namespace BP.En
{
	/// <summary>
	///  Property 
	/// </summary>
    public class AttrFile
    {
        public string FileNo = null;
        public string FileName = null;
        public AttrFile(string fileno,string filename)
        {
            this.FileNo = fileno;
            this.FileName = filename;
        }
        public AttrFile()
        {
        }
    }
	/// <summary>
	///  Properties collection 
	/// </summary>
    [Serializable]
    public class AttrFiles : CollectionBase
    {
        public AttrFiles()
        {
        }
        /// <summary>
        ///  Add file 
        /// </summary>
        /// <param name="fileNo"></param>
        /// <param name="fileName"></param>
        public void Add(string fileNo, string fileName)
        {
            this.InnerList.Add(new AttrFile(fileNo, fileName));
        }
    }	
}
