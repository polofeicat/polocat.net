using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace AsmxWsInterceptor.Compressing
{
    internal class AlwaysStreamCompressor : StreamCompressor
    {

        /// <summary>
        /// 对即将通过网络发送出去的数据流进行压缩处理。

        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <param name="emptyBuffer">数据缓存</param>
        protected override void CompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            int rdbytes = -1;

            using (GZipStream gzips = new GZipStream(outputStream, CompressionMode.Compress, true))
            {
                while ((rdbytes = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
                {
                    gzips.Write(emptyBuffer, 0, rdbytes);
                }
            }

        }

        /// <summary>
        /// 对从网络中接收过来的数据流进行解压缩处理。

        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <param name="emptyBuffer">数据缓存</param>
        protected override void DecompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            int rdbytes = -1;

            using (GZipStream gzips = new GZipStream(inputStream, CompressionMode.Decompress, true))
            {
                while ((rdbytes = gzips.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
                {
                    outputStream.Write(emptyBuffer, 0, rdbytes);
                }
                outputStream.Flush();
            }
        }
    }
}
