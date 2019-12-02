using AsmxWsInterceptor.Compressing;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace AsmxWsInterceptor
{

    /// <summary>
    /// 
    /// </summary>
    public class CompressionExtension : SoapExtensionAdapter
    {
        private Stream previousStream;
        private Stream currentStream;

        // CompressorMode 只有Auto、Always和Never这三种。
        public const string CompressorMode = "always";

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageAfterSerialize(System.Web.Services.Protocols.SoapMessage message)
        {
            StreamCompressor compressor = StreamCompressor.Create(CompressorMode);
            try
            {
                //对于处理AfterSerialize过程时，因为系统按优先级从低到高逐个调用SOAP扩展的ProcessMessage方法，
                //因此输入流必须是currentStream指向的对象，该对象就是在ChainStream方法中定义的MemoryStream对象，
                //而输出流是previousStream所指向的对象，该对象是更高优先级的SOAP扩展的输入流对象或者当前SOAP扩展
                //自身是最高优先级别的扩展时，其previousStream所指向的对象是个SoapExternalStream对象，这个对象只
                //是一个包装类对象而已，它底层包含了一个用于将构成Http/Soap协议所必要的信息发送到网络中输出流。
                compressor.ProcessStream(currentStream, previousStream, StreamProcessMode.Compress);
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
            StreamCompressor compressor = StreamCompressor.Create(CompressorMode);
            try
            {
                //对于处理BeforeDeserialize过程时，因为系统会按照优先级从高到低逐个调用SOAP扩展（跟上述过程刚好相反）
                //的ProcessMessage方法，因此输入流必须是previousStream所指向的对象，该对象是优先级别更高的SOAP扩展的
                //currentStream所指向的内存流对象，或者当前SOAP扩展自身是个最高优先级别的扩展时，其previousStream所
                //指向的对象是个来自网络的输入流对象，而该函数所接受的输出流对象则应该是currentStream所指向的流对象。
                compressor.ProcessStream(previousStream, currentStream, StreamProcessMode.Decompress);
            }
            catch (Exception e)
            {
                throw new SoapException(e.Message, SoapException.ServerFaultCode, e);
            }
        }


        /// <summary>
        /// 该函数准遵循固定格式的写法，在多个SOAP扩展中创建一个单向的流链表结构，
        /// 优先级低的SOAP扩展的previousStream字段指向优先级高的SOAP扩展的currentStream
        /// 字段所指向的Stream对象。
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
