using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;

namespace AsmxWsInterceptor
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractSoapExtension : SoapExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override sealed void ProcessMessage(SoapMessage message)
        {
            try
            {
                switch (message.Stage)
                {
                    case SoapMessageStage.BeforeDeserialize:
                        ProcessMessageBeforeDeserialize(message);
                        break;
                    case SoapMessageStage.AfterDeserialize:
                        ProcessMessageAfterDeserialize(message);
                        break;
                    case SoapMessageStage.BeforeSerialize:
                        ProcessMessageBeforeSerialize(message);
                        break;
                    case SoapMessageStage.AfterSerialize:
                        ProcessMessageAfterSerialize(message);
                        break;
                    default:
                        throw new SoapException("Invalid Soap Message Stage", SoapException.ServerFaultCode);
                }
            }
            catch (Exception e)
            {
                if (e is SoapException)
                {
                    message.Exception = (SoapException)e;
                }
                else
                {
                    Exception realex = e.InnerException == null ? e : e.InnerException;
                    message.Exception = new SoapException(e.Message, SoapException.ServerFaultCode, realex);
                }
            }
        }

        public abstract void ProcessMessageBeforeDeserialize(SoapMessage message);
        public abstract void ProcessMessageAfterDeserialize(SoapMessage message);
        public abstract void ProcessMessageBeforeSerialize(SoapMessage message);
        public abstract void ProcessMessageAfterSerialize(SoapMessage message);

    }
}
