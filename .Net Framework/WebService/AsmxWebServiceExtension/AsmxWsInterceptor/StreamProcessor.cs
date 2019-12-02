using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmxWsInterceptor
{

    /// <summary>
    /// 
    /// </summary>
    internal abstract class StreamProcessor
    {
        protected const int BufferSize = 4096;


        /// <summary>
        /// 实现两个数据流对等拷贝，简单的把数据从输入流中读出后再写入输出流。
        /// 中间不做任何过滤处理。
        /// </summary>
        /// <param name="inputStream">输入流</param>
        /// <param name="outputStream">输出流</param>
        /// <param name="emptyBuffer">数据缓存</param>
        protected void InternalNormalProcessStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            Array.Clear(emptyBuffer, 0, emptyBuffer.Length);
            int rdbytes = -1;
            while ((rdbytes = inputStream.Read(emptyBuffer, 0, emptyBuffer.Length)) > 0)
            {
                outputStream.Write(emptyBuffer, 0, rdbytes);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="mode"></param>
        protected abstract void ProcessModeStream(Stream inputStream, Stream outputStream, StreamProcessMode mode, byte[] buffer);



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="mode"></param>
        public void ProcessStream(Stream inputStream, Stream outputStream, StreamProcessMode mode)
        {
            // 当输入流不可读时应该抛出异常终止程序。

            if (!inputStream.CanRead)
            {
                throw new NotSupportedException("Input stream not support reading");
            }

            // 当输出流不可写时应该抛出异常终止程序。

            if (!outputStream.CanWrite)
            {
                throw new NotSupportedException("Output stream not support writting");
            }

            long outputStreamCurrentPosition = 0;
            if (outputStream.CanSeek)
            {
                outputStreamCurrentPosition = outputStream.Position;
            }

            // 将上一个输出流（上一个SOAP扩展的输出流就是当前SOAP扩展的输入流）复位再后读取经过上一个SOAP扩展解析的数据流。

            if (inputStream.CanSeek)
            {
                inputStream.Position = 0;
            }

            byte[] buffer = new byte[BufferSize];

            try
            {
                ProcessModeStream(inputStream, outputStream, mode, buffer);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("数据处理错误 : " + ex.Message, ex);
            }
            finally
            {
                // 在整个数据流链表中，必需手动关闭输入流，但不能关闭输出流。
                inputStream.Close();

                /* 当优先级最低的SOAP扩展（即最后一个过滤处理器）在处理完从网络中接收过来的数据时，
                 * 必需将输出流复位到经当前SOAP扩展处理后前的位置上，否则系统从这个流中读取数据时
                 * 必定遇到EOF异常。
                 * */
                if (outputStream.CanSeek)
                {
                    outputStream.Position = outputStreamCurrentPosition;
                }
            }
        }

    }
}
