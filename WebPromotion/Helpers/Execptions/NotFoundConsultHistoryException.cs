using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WebPromotion.Helpers.Execptions
{
    public class NotFoundConsultHistoryException : Exception
    {
        public NotFoundConsultHistoryException() { }
        public NotFoundConsultHistoryException(string message) : base(message) { }
        public NotFoundConsultHistoryException(string message, Exception inner) : base(message, inner) { }
        protected NotFoundConsultHistoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}