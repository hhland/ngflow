using System;
using System.Collections.Generic;
using System.Text;
//   Author : MEDICINE IN 
//  2005-8-8
// // // // // //
using System;
using System.Text;
using System.IO;
namespace BP.DA
{
    /// <summary>
    ///  For acquiring encoding of a text file (Encoding).
    /// </summary>
    public class TxtFileEncoding
    {
        public TxtFileEncoding()
        {
            //
            // TODO:  Add constructor logic here 
            //
        }
        /// <summary>
        ///  Obtaining encoding of a text file . If you can not find a valid preamble in the header ,Encoding.Default Will be returned .
        /// </summary>
        /// <param name="fileName"> File name .</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }
        /// <summary>
        ///  Obtain a text file encoding streams .
        /// </summary>
        /// <param name="stream"> Text file stream .</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }
        /// <summary>
        ///  Obtaining encoding of a text file .
        /// </summary>
        /// <param name="fileName"> File name .</param>
        /// <param name="defaultEncoding"> Default encoding . When this method can not obtain a valid preamble from the top of the file when , Will return the encoding .</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }
        /// <summary>
        ///  Obtain a text file encoding streams .
        /// </summary>
        /// <param name="stream"> Text file stream .</param>
        /// <param name="defaultEncoding"> Default encoding . When this method can not obtain a valid preamble from the top of the file when , Will return the encoding .</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                // Before saving the file stream 4 Bytes 
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                // Save the current Seek Location 
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }

                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                // According to pre-file stream 4 Bytes judge Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }
                // Recovery Seek Location        
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
    }
}
     