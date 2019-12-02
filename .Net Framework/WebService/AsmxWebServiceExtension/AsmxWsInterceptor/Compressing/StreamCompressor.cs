using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace AsmxWsInterceptor.Compressing
{
    /// <summary>
    /// 用于对发送的数据进行压缩和对从网络接收过来的数据进行解压操作，该类属于抽象基类，
    /// 是AlwaysStreanCompressor、AutoStreamCompressor和NeverStreamCompressor类型
    /// 的基类，并作为上述类型的工厂类，通过其静态方法Create可以获得所需要的制定类型。
    /// 系统中定义了3种类型的StreamCompressor可以通过指定相应的字符串标识获得对应类型
    /// 的对象。
    /// </summary>
    internal abstract class StreamCompressor : StreamProcessor
    {

        /// <summary>
        /// 存储流中数据的缓冲区。
        /// 抽象类自身作为产生具体类型的工厂类，根据输入的字符串标识，返回已有的三种类型的数据流压缩器。
        /// </summary>
        /// <param name="compressor">只接受auto、always和never作为合法值，字符串标识不区分大小写</param>
        /// <returns>返回具体的压缩对象</returns>
        public static StreamCompressor Create(string compressor)
        {
            if (compressor == null)
            {
                throw new ArgumentNullException("Parameter compressor is null");
            }

            if (compressor.Equals("always", StringComparison.OrdinalIgnoreCase))
            {
                return new AlwaysStreamCompressor();
            }

            if (compressor.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                return new AutoStreamCompressor();
            }

            if (compressor.Equals("never", StringComparison.OrdinalIgnoreCase))
            {
                return new NeverStreamCompressor();
            }

            throw new NotImplementedException("No such Stream Compressor type");
        }


        
        protected abstract void CompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer);
        protected abstract void DecompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer);



        /// <summary>
        /// 集中处理所有数据流的压缩或者解压缩，具体的压缩和解压功能
        /// 在具体的子类中通过重写CompressStream和DecompressStream
        /// 方法来实现。
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="mode"></param>
        /// <param name="buffer"></param>
        protected override void ProcessModeStream(Stream inputStream, Stream outputStream, StreamProcessMode mode, byte[] buffer)
        {
            if (mode == StreamProcessMode.Compress)
            {
                CompressStream(inputStream, outputStream, buffer);
            }
            else if (mode == StreamProcessMode.Decompress)
            {
                DecompressStream(inputStream, outputStream, buffer);
            }
            else
            {
                throw new NotSupportedException("Not support compression mode");
            }
        }


    }
}
