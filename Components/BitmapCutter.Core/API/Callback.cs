using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BitmapCutter.Core.API
{
    /// <summary>
    /// Bitmap operations
    /// </summary>
    public class Callback
    {
        /// <summary>
        /// create a new BitmapCutter.Core.API.BitmapOPS instance
        /// </summary>
        public Callback() { }

        /// <summary>
        /// rotate bitmap with any angle
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public string RotateBitmap(string src)
        {
            try
            {
                src = src.Replace("/", "\\");
                src = src.Replace("..\\", string.Empty);
                HttpContext context = HttpContext.Current;
                float angle = float.Parse(context.Request["angle"]);
                Image oldImage = Bitmap.FromFile(src);
                Bitmap newBmp = Helper.RotateImage(oldImage, angle);
                oldImage.Dispose();
                int nw = newBmp.Width;
                int nh = newBmp.Height;
                newBmp.Save(src);
                newBmp.Dispose();
                return "{msg:'success',size:{width:" + nw.ToString() + ",height:" + nh.ToString() + "}}";
            }
            catch (Exception ex)
            {
                return "{msg:'" + ex.Message+"'}";
            }
        }

        public string GenerateBitmap(string src)
        {
            try
            {
                src=src.Replace("/","\\");
                src = src.Replace("..\\", string.Empty);
                FileInfo fi = new FileInfo(src);
                string ext = fi.Extension;

                string newfileName = "DataUser/ImgAth/Temp/" + DateTime.Now.ToString("yyMMddHHmmssffff") + ".png";

                //Image.GetThumbnailImageAbort abort = null;
                Bitmap oldBitmap = new Bitmap(src);
               
                HttpContext context = HttpContext.Current;
                Cutter cut = new Cutter(
                    double.Parse(context.Request["zoom"]),
                    -int.Parse(context.Request["x"]),
                    -int.Parse(context.Request["y"]),
                    int.Parse(context.Request["width"]),
                    int.Parse(context.Request["height"]),
                    oldBitmap.Width,
                    oldBitmap.Height);

                Bitmap bmp = Helper.GenerateBitmap(oldBitmap, cut);
                oldBitmap.Dispose();
                
                string temp = Path.Combine(context.Server.MapPath("~/"), newfileName);
                bmp.Save(temp, ImageFormat.Png);
                bmp.Dispose();
                return "{msg:'success',src:'../../" + newfileName + "'}";
            }
            catch (Exception ex)
            {
                return "{msg:'" + ex.Message + "'}";
            }
        }
    }
}
