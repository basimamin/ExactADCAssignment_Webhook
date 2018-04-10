using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ExactAssignment.BLL
{
    public class Common
    {
        /// <summary>
        ///*** Fn to Convert Stream to Byte Array
        /// </summary>
        /// <param name="input"></param>
        /// <returns>return Byte Array</returns>
        public static byte[] ConvertStreamtoByteArr(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
