using AsmxWsInterceptor.Codec;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace AsmxWsInterceptor
{

    /// <summary>
    /// 
    /// </summary>
    public class EncryptionExtension : SoapExtensionAdapter
    {
        private Stream previousStream;
        private Stream currentStream;


        //public const string EncryptionMode = "aes";
        public const string EncryptionMode = "3des";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageAfterSerialize(System.Web.Services.Protocols.SoapMessage message)
        {
            StreamEncryptor compressor = StreamEncryptor.Create(EncryptionMode);

            try
            {
                compressor.ProcessStream(currentStream, previousStream, StreamProcessMode.Encrypt);
            }
            catch (Exception e)
            {
                throw new SoapException(e.Message, SoapException.ServerFaultCode, e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageBeforeDeserialize(System.Web.Services.Protocols.SoapMessage message)
        {
            StreamEncryptor compressor = StreamEncryptor.Create(EncryptionMode);

            try
            {
                compressor.ProcessStream(previousStream, currentStream, StreamProcessMode.Decrypt);
            }
            catch (Exception e)
            {
                throw new SoapException(e.Message, SoapException.ServerFaultCode, e);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override System.IO.Stream ChainStream(System.IO.Stream stream)
        {
            previousStream = stream;
            currentStream = new MemoryStream();
            return currentStream;
        }

    }
}
