using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace AsmxWsInterceptor.Codec
{

    /// <summary>
    /// 
    /// </summary>
    internal class AesStreamEncryptor : StreamEncryptor
    {

        private string key;     //32 * 8位
        private string iv;      //16 * 8位
        private CipherMode cMode;
        private PaddingMode pMode;


        /// <summary>
        /// 
        /// </summary>
        public AesStreamEncryptor()
        {
            cMode = CipherMode.CBC;
            pMode = PaddingMode.ISO10126;
            key = "706ae1e2-d8c8-4098-ab26-2f237add";
            iv = "7c328d55-4044-42";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void DecryptStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            int readCount = 0;
            MemoryStream ms = new MemoryStream();
            while((readCount = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
            {
                ms.Write(emptyBuffer, 0, readCount);
            }
            ms.Flush();

            RijndaelManaged aes = new RijndaelManaged()
            {
                Mode = cMode,
                Padding = pMode,
                Key = Encoding.ASCII.GetBytes(key),
                IV = Encoding.ASCII.GetBytes(iv)
            };

            ICryptoTransform cipher = aes.CreateDecryptor();
            byte[] cipherData = ms.ToArray();
            byte[] orgData = cipher.TransformFinalBlock(cipherData, 0, cipherData.Length);

            outputStream.Write(orgData, 0, orgData.Length);
            outputStream.Flush();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void EncryptStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            int readCount = 0;
            MemoryStream ms = new MemoryStream();
            while ((readCount = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
            {
                ms.Write(emptyBuffer, 0, readCount);
            }
            ms.Flush();

            RijndaelManaged aes = new RijndaelManaged()
            {
                Mode = cMode,
                Padding = pMode,
                Key = Encoding.ASCII.GetBytes(key),
                IV = Encoding.ASCII.GetBytes(iv)
            };

            ICryptoTransform cipher = aes.CreateEncryptor();
            byte[] orgData = ms.ToArray();
            byte[] cipherData = cipher.TransformFinalBlock(orgData, 0, orgData.Length);

            outputStream.Write(cipherData, 0, cipherData.Length);
            outputStream.Flush();
        }
    }
}
