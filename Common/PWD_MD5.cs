using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class PWD_MD5
    {
        /// <summary>
        /// 传入文本进行加密获取MD5值
        /// </summary>
        /// <param name="text">未加密的文本</param>
        /// <returns></returns>
        public static string Encryption(string text)
        {
            byte[] textByte = MD5.Create().ComputeHash(Encoding.Default.GetBytes(text));
            return BitConverter.ToString(textByte).Replace("-", "");
        }

    }
}
