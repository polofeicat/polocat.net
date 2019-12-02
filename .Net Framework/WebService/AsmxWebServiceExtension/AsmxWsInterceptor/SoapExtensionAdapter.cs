using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmxWsInterceptor
{

    /// <summary>
    /// 
    /// </summary>
    public class SoapExtensionAdapter : AbstractSoapExtension
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageBeforeDeserialize(System.Web.Services.Protocols.SoapMessage message)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageAfterDeserialize(System.Web.Services.Protocols.SoapMessage message)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageBeforeSerialize(System.Web.Services.Protocols.SoapMessage message)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessageAfterSerialize(System.Web.Services.Protocols.SoapMessage message)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public override object GetInitializer(Type serviceType)
        {
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public override object GetInitializer(System.Web.Services.Protocols.LogicalMethodInfo methodInfo, System.Web.Services.Protocols.SoapExtensionAttribute attribute)
        {
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="initializer"></param>
        public override void Initialize(object initializer)
        {
        }
    }
}
