using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AsmxWsInterceptor.Compressing
{

    /// <summary>
    /// 
    /// </summary>
    internal class NeverStreamCompressor : StreamCompressor
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void CompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            InternalNormalProcessStream(inputStream, outputStream, emptyBuffer);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="emptyBuffer"></param>
        protected override void DecompressStream(Stream inputStream, Stream outputStream, byte[] emptyBuffer)
        {
            InternalNormalProcessStream(inputStream, outputStream, emptyBuffer);
        }
    }
}
