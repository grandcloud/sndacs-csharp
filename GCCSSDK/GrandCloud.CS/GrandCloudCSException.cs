
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

using GrandCloud.CS.Model;

namespace GrandCloud.CS
{
    /// <summary>
    /// GrandCloud CS Exception provides details of errors returned by GrandCloud CS service.
    /// 
    /// In particular, this class provides access to CS's extended request ID, the Date,
    /// and the host ID which are required debugging information in the odd case that 
    /// you need to contact GrandCloud about an issue where GrandCloud CS is incorrectly handling 
    /// a request.
    /// 
    /// The ResponseHeaders property of the GrandCloudCSException contains all the HTTP headers
    /// in the Error Response returned by the CS service.
    /// </summary>
    [Serializable]
    public class GrandCloudCSException : Exception, ISerializable
    {
        private HttpStatusCode statusCode = default(HttpStatusCode);
        private string errorCode;
        private string message;
        private string requestId;
        private string xml;
        private string requestAddr;
        private WebHeaderCollection responseHeaders;

        /// <summary>
        /// Initializes a new GrandCloudCSException with default values.
        /// </summary>
        public GrandCloudCSException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new GrandCloudCSException with a specified error message
        /// </summary>
        /// <param name="message">A message that describes the error</param>
        public GrandCloudCSException(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Initializes a new GrandCloudCSException from the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="innerException">The nested exception that caused the GrandCloudCSException</param>
        public GrandCloudCSException(Exception innerException)
            : this(innerException.Message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new GrandCloudCSException with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.
        /// </param>
        /// <param name="context">The contextual information about the source or destination.
        /// </param>
        protected GrandCloudCSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.statusCode = (HttpStatusCode)info.GetValue("statusCode", typeof(HttpStatusCode));
            this.errorCode = info.GetString("errorCode");
            this.message = info.GetString("message");
            this.requestId = info.GetString("requestId");
            this.xml = info.GetString("xml");
            this.requestAddr = info.GetString("requestAddr");
            this.responseHeaders = (WebHeaderCollection)info.GetValue("responseHeaders", typeof(WebHeaderCollection));
        }

        /// <summary>
        /// Serializes this instance of GrandCloudCSException.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.
        /// </param>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("statusCode", statusCode, typeof(HttpStatusCode));
            info.AddValue("errorCode", errorCode, typeof(string));
            info.AddValue("message", message, typeof(string));
            info.AddValue("requestId", requestId, typeof(string));
            info.AddValue("xml", xml, typeof(string));
            info.AddValue("requestAddr", requestAddr, typeof(string));
            info.AddValue("responseHeaders", responseHeaders, typeof(WebHeaderCollection));
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Initializes a new GrandCloudCSException with a specific error message and the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the innerException parameter is not a null reference, the current exception is
        /// raised in a catch block that handles the inner exception.</param>
        public GrandCloudCSException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
            GrandCloudCSException ex = innerException as GrandCloudCSException;
            if (ex != null)
            {
                this.statusCode = ex.StatusCode;
                this.errorCode = ex.ErrorCode;
                this.requestId = ex.RequestId;
                this.message = ex.Message;
                this.xml = ex.XML;
            }
        }

        /// <summary>
        /// Initializes an GrandCloudCSException with a specific message and
        /// HTTP status code
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="statusCode">HTTP status code for error response</param>
        public GrandCloudCSException(string message, HttpStatusCode statusCode)
            : this(message)
        {
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Initializes an GrandCloudCSException with error information provided in an
        /// GrandCloudCS response.
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="statusCode">HTTP status code for error response</param>
        /// <param name="errorCode">Error Code returned by the service</param>
        /// <param name="requestId">Request ID returned by the service</param>
        /// <param name="xml">Compete xml found in response</param>
        /// <param name="requestAddr">The CS request url</param>
        /// <param name="responseHeaders">The response headers containing CS specific information
        /// </param>
        public GrandCloudCSException(
            string message,
            HttpStatusCode statusCode,
            string errorCode,
            string requestId,
            string xml,
            string requestAddr,
            WebHeaderCollection responseHeaders)
            : this(message, statusCode)
        {
            this.errorCode = errorCode;
            this.requestId = requestId;
            this.xml = xml;
            this.requestAddr = requestAddr;
            this.responseHeaders = responseHeaders;
        }

        /// <summary>
        /// Initializes an GrandCloudCSException with error information provided in an
        /// GrandCloudCS response and the inner exception that is the cause of the exception
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="statusCode">HTTP status code for error response</param>
        /// <param name="requestAddr">The CS request url</param>
        /// <param name="responseHeaders">The response headers containing CS specific information
        /// <param name="innerException">The nested exception that caused the GrandCloudCSException</param>
        /// </param>
        public GrandCloudCSException(
            string message,
            HttpStatusCode statusCode,
            string requestAddr,
            WebHeaderCollection responseHeaders,
            Exception innerException)
            : this(message, innerException)
        {
            this.statusCode = statusCode;
            this.requestAddr = requestAddr;
            this.responseHeaders = responseHeaders;
        }

        /// <summary>
        /// Initializes an GrandCloudCSException with error information provided in an
        /// GrandCloudCS response and the inner exception that is the cause of the exception
        /// </summary>
        /// <param name="statusCode">HTTP status code for error response</param>
        /// <param name="xml">Compete xml found in response</param>
        /// <param name="requestAddr">The CS request url</param>
        /// <param name="responseHeaders">The response headers containing CS specific information
        /// <param name="error">The nested exception that caused the GrandCloudCSException</param>
        /// </param>
        public GrandCloudCSException(
            HttpStatusCode statusCode,
            string xml,
            string requestAddr,
            WebHeaderCollection responseHeaders,
            CSError error)
        {
            this.xml = xml;
            this.statusCode = statusCode;
            this.requestAddr = requestAddr;
            this.responseHeaders = responseHeaders;
            if (error != null)
            {
                this.errorCode = error.Code;
                this.requestId = error.RequestId;
                this.message = error.Message;
            }
        }

        /// <summary>
        /// Gets the ErrorCode property.
        /// </summary>
        public string ErrorCode
        {
            get { return this.errorCode; }
        }

        /// <summary>
        /// Gets error message
        /// </summary>
        public override string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Gets status code returned by the service if available. If status
        /// code is set to -1, it means that status code was unavailable at the
        /// time exception was thrown
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        /// <summary>
        /// Gets XML returned by the service if available.
        /// </summary>
        public string XML
        {
            get { return this.xml; }
        }

        /// <summary>
        /// Gets Request ID returned by the service if available.
        /// </summary>
        public string RequestId
        {
            get { return this.requestId; }
        }

        /// <summary>
        /// Gets the CS service address used to make this request.
        /// </summary>
        public string CSRequestAddress
        {
            get { return this.requestAddr; }
        }

        /// <summary>
        /// Gets the error response HTTP headers so that CS specific information
        /// can be retrieved for debugging. Interesting field is:
        ///  Date
        ///
        /// A value can be retrieved from this HeaderCollection via:
        /// ResponseHeaders.Get("key");
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return this.responseHeaders; }
        }
    }
}