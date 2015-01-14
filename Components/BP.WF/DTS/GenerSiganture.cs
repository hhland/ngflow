using System;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;

namespace BP.WF.DTS
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class GenerSiganture : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public GenerSiganture()
        {
            this.Title = " Set the default digital signature for a digital signature is not set user ";
            this.Help = " This feature requires users  "+ BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\  Has read and write permissions , Otherwise fails .";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
            //this.Warning = " Are you sure you want to perform ?";
            //HisAttrs.AddTBString("P1", null, " Old Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P2", null, " New Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P3", null, " Confirm ", true, false, 0, 10, 10);
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
            try
            {
                BP.Port.Emps emps = new Emps();
                emps.RetrieveAllFromDBSource();
                string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\T.JPG";
                string fontName = " Times New Roman ";
                string empOKs = "";
                string empErrs = "";
                foreach (Emp emp in emps)
                {
                    string pathMe = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.No + ".JPG";
                    if (System.IO.File.Exists(pathMe))
                        continue;

                    File.Copy(BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\Templete.JPG",
                        path, true);

                    System.Drawing.Image img = System.Drawing.Image.FromFile(path);
                    Font font = new Font(fontName, 15);
                    Graphics g = Graphics.FromImage(img);
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical);// Text 
                    g.DrawString(emp.Name, font, drawBrush, 3, 3);
                    img.Save(pathMe);
                    img.Dispose();
                    g.Dispose();

                    File.Copy(pathMe,
                    BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + emp.Name + ".JPG", true);
                }
                return " Successful implementation ...";
            }
            catch(Exception ex)
            {
                return " Execution failed , Make sure the right  " + BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\  Directory have access ? Exception Information :"+ex.Message;
            }
        }
    }
}
