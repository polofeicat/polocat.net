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
    internal class AutoStreamCompressor : StreamCompressor
    {
        private const string CompressedIndicator = "MustDecompressed";
        private const int NeededCompressionMinSize = 400;

        /// <summary>
        /// 对即将发送到网络上的数据进行压缩处理，自动判断本次所发送的
        /// 数据量是否有必要进行压缩处理，若传送数据量较大需要进行压缩时，
        /// 在有效数据里开头部分加入压缩标记的长度和压缩标记（一个字符串），
        /// 以便接受端在收到数据时能进行识别进入的数据流是否为已压缩流。
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void CompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            if (inputStream.Length >= NeededCompressionMinSize)
            {
                byte[] bIndicator = Encoding.ASCII.GetBytes(CompressedIndicator);
                byte[] bIndicatorSize = BitConverter.GetBytes(bIndicator.Length);

                // 写入数据压缩标记和标记的字节长度
                outputStream.Write(bIndicatorSize, 0, bIndicatorSize.Length);
                outputStream.Write(bIndicator, 0, bIndicator.Length);

                int rdbytes = -1;

                // 对数据流进行压缩处理使用的是Gzip压缩方式
                using (GZipStream gzips = new GZipStream(outputStream, CompressionMode.Compress, true))
                {
                    while ((rdbytes = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
                    {
                        gzips.Write(emptyBuffer, 0, rdbytes);
                    }
                }
            }
            else
            {
                // 简单的将输入流拷贝到输出流中去。
                InternalNormalProcessStream(inputStream, outputStream, emptyBuffer);
            }
        }

        /// <summary>
        /// 将从网络中接收过来的数据流解压缩，自动识别数据流流的标头，
        /// 若标头含有表示符号”MustDecompressed“和其长度则需要将
        /// 接收到的数据流进行解压处理，若标头并不含有压缩识别符号，
        /// 则不进行压缩处理。
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void DecompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            /* 输入流有几种类型，有些是不能够定位的，如来自网络的数据流，

             * 有些是可以定位的如内存中的数据流，对于那些不能够重定位的
             * 数据流，先将所有数据读入内存流后再做处理。

             * */
            Stream orgInputStream = null;
            try
            {
                if (inputStream.CanSeek)
                {
                    orgInputStream = inputStream;
                }
                else
                {
                    orgInputStream = new MemoryStream();
                    byte[] buffer = new byte[BufferSize];
                    InternalNormalProcessStream(inputStream, orgInputStream, buffer);
                    orgInputStream.Position = 0;
                }

                byte[] bIndicatorSize = new byte[4];
                int nrd = orgInputStream.Read(bIndicatorSize, 0, bIndicatorSize.Length);
                if (nrd != bIndicatorSize.Length)
                {
                    throw new ApplicationException("Unrecognized input stream format");
                }
                int indicatorSize = BitConverter.ToInt32(bIndicatorSize, 0);

                // 识别出数据流压缩标记长度。
                if (indicatorSize == CompressedIndicator.Length)
                {
                    byte[] bIndicator = new byte[indicatorSize];
                    if (orgInputStream.Read(bIndicator, 0, indicatorSize) != indicatorSize)
                    {
                        throw new ApplicationException("Unrecognized stream format");
                    }
                    string indicator = Encoding.ASCII.GetString(bIndicator);

                    // 识别出数据流压缩标记。
                    if (indicator.Equals(CompressedIndicator, StringComparison.Ordinal))
                    {

                        int rdbytes = -1;
                        using (GZipStream gzips = new GZipStream(orgInputStream, CompressionMode.Decompress, true))
                        {
                            while ((rdbytes = gzips.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
                            {
                                outputStream.Write(emptyBuffer, 0, rdbytes);
                            }
                        }

                        // 压缩处理完毕，此时必须返回。
                        return;
                    }
                }

                orgInputStream.Position = 0;
                InternalNormalProcessStream(orgInputStream, outputStream, emptyBuffer);

            }
            finally
            {
                if (orgInputStream != null)
                {
                    orgInputStream.Close();
                }
            }
        }
    }
}
