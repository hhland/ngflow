using System;
using System.Net;
using System.Windows;
using System.IO;

namespace BP.CY
{
    public class StringHandler
    {
        /// <summary>
        ///  The stream into a string 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(Stream stream)
        {
            byte[] b = (stream as MemoryStream).GetBuffer();
            return Convert.ToBase64String(b);
        }

        /// <summary>
        ///  Convert a string into a stream 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Stream ToStream(string str)
        {
            byte[] b = Convert.FromBase64String(str);

            return (new MemoryStream(b));
        }
    }
}
