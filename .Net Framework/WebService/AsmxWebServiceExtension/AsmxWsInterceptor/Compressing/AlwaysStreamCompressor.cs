using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace AsmxWsInterceptor.Compressing
{
    /// <summary>
    /// 
    /// </summary>
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

            MemoryStream msA = new MemoryStream();      //压缩前的数据
            MemoryStream msB = new MemoryStream();      //压缩后的数据


            while ((rdbytes = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
            {
                msA.Write(emptyBuffer, 0, rdbytes);
            }

            long sizeOfOrgData = msA.Length;            //压缩前大小
            msA.Position = 0;                           //定位流到开始处

            rdbytes = -1;
            Array.Clear(emptyBuffer, 0, emptyBuffer.Length);
            using (GZipStream gzips = new GZipStream(msB, CompressionMode.Compress, true))
            {
                while ((rdbytes = msA.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
                {
                    gzips.Write(emptyBuffer, 0, rdbytes);
                }
                gzips.Flush();
            }

            long sizeOfZipData = msB.Length;            //压缩后大小

            byte[] zipData = msB.GetBuffer();           //压缩后的数据
            outputStream.Write(zipData, 0, zipData.Length);
        }

            //using (GZipStream gzips = new GZipStream(outputStream, CompressionMode.Compress, true))
            //{
            //    while ((rdbytes = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
            //    {
            //        gzips.Write(emptyBuffer, 0, rdbytes);
            //    }
            //}

        //}

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
