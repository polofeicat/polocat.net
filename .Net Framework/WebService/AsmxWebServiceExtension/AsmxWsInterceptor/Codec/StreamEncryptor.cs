using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmxWsInterceptor.Codec
{

    /// <summary>
    /// 
    /// </summary>
    internal abstract class StreamEncryptor : StreamProcessor
    {

        private static readonly AesStreamEncryptor AES = new AesStreamEncryptor();
        private static readonly TripleDesStreamEncryptor TripleDES = new TripleDesStreamEncryptor();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptor"></param>
        /// <returns></returns>
        public static StreamEncryptor Create(string encryptor)
        {
            if (encryptor == null)
            {
                throw new ArgumentNullException("Parameter encryptor is null");
            }

            if (encryptor.Equals("3des", StringComparison.OrdinalIgnoreCase))
            {
                return TripleDES;
            }

            if (encryptor.Equals("aes", StringComparison.OrdinalIgnoreCase))
            {
                return AES;
            }

            throw new NotImplementedException("No such Stream Encryptor type");
        }

        protected abstract void EncryptStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer);
        protected abstract void DecryptStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="mode"></param>
        /// <param name="buffer"></param>
        protected override void ProcessModeStream(Stream inputStream, Stream outputStream, StreamProcessMode mode, byte[] buffer)
        {
            if (mode == StreamProcessMode.Encrypt)
            {
                EncryptStream(inputStream, outputStream, buffer);
            }
            else if (mode == StreamProcessMode.Decrypt)
            {
                DecryptStream(inputStream, outputStream, buffer);
            }
            else
            {
                throw new NotSupportedException("Not support encryption mode");
            }
        }
    }
}
