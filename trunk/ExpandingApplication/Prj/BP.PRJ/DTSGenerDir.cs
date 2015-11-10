using System;
using System.IO;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
namespace BP.PRJ
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class RepariDB : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public RepariDB()
        {
            this.Title = " Rebuild the project data tree ";
            this.Help = "Cleanup PRJ_FileDir Data  , Again according to the directory to initialize .";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            //Dirs dirs = new Dirs();
            //dirs.ClearTable();

            //string path = @"D:\ccflow\VisualFlow\Data\PrjData\Templete";
            //string[] strs = Directory.GetDirectories(path);
            //foreach (string str in strs)
            //{
            //    Dir dir = new Dir();
            //    dir.No = str.Substring(0, 2);
            //    dir.Name = str.Substring(3);
            //    dir.DirPath = str;
            //    dir.Insert();
            //}
            return " Successful implementation ...";
        }
        private void GetFolder(string pPath)
        {
            //string[] str_Directorys;
            //str_Directorys = Directory.GetDirectories(pPath);
            //foreach (string pstr in str_Directorys)
            //{
            //    Dir dir = new Dir();
            //    //dir.No = str.Substring(0, 2);
            //}
        }

    }
}
