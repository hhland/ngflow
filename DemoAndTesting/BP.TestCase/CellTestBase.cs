using System;
using System.Threading;
using System.Collections;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web.Controls;
using BP.Web;

namespace BP.CT
{
    public enum EditState
    {
        /// <summary>
        ///  Has been completed 
        /// </summary>
        Passed,
        /// <summary>
        ///  Edit 
        /// </summary>
        Editing,
        /// <summary>
        ///  Unfinished 
        /// </summary>
        UnOK
    }
	/// <summary>
	///  Test base class 
	/// </summary>
    abstract public class TestBase
    {
        public EditState EditState = EditState.Editing;
        /// <summary>
        ///  Step information 
        /// </summary>
        public int TestStep = 0;
        public string Note = "";
        /// <summary>
        ///  Increase test content .
        /// </summary>
        /// <param name="note"> A detailed description of test content .</param>
        public void AddNote(string note)
        {
            TestStep++;
            if (Note == "")
            {
                Note += "\t\n  Get on :" + TestStep + " Tests ";
                Note += "\t\n" + note;

            }
            else
            {
                Note += "\t\n Tested .";
                Note += "\t\n  Get on :" + TestStep + " Tests ";
                Note += "\t\n" + note;
            }
        }
        public string sql = "";
        public DataTable dt = null;
        /// <summary>
        ///  Let subclasses override 
        /// </summary>
        public virtual void Do()
        {
        }

        #region  Basic properties .
        /// <summary>
        ///  Title 
        /// </summary>
        public string Title = " Unnamed unit testing ";
        public string DescIt = " Description ";
        /// <summary>
        ///  Error Messages 
        /// </summary>
        public string ErrInfo = "";
        #endregion
        /// <summary>
        ///  Test base class 
        /// </summary>
        public TestBase() { }
    }

}
