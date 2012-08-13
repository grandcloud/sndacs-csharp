
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GrandCloud.CS
{
    /// <summary>
    /// A base exception for some GrandCloud Web Services.
    /// <para>
    /// Most exceptions thrown to client code will be service-specific exceptions, though some services
    /// may throw this exception if there is a problem which is caught in the core client code.
    /// </para>
    /// </summary>
    public class GrandCloudServiceException : Exception
    {
        private ErrorType errorType;
        private string errorCode;
        private string requestId;
        private HttpStatusCode statusCode;

        protected internal GrandCloudServiceException()
            : base()
        {
        }

        protected internal GrandCloudServiceException(string message)
            : base(message)
        {
        }

        protected internal GrandCloudServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected internal GrandCloudServiceException(string message, Exception innerException, HttpStatusCode statusCode)
            : base(message, innerException)
        {
            this.statusCode = statusCode;
        }

        protected internal GrandCloudServiceException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        protected internal GrandCloudServiceException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
            : base(message)
        {
            this.errorCode = errorCode;
            this.errorType = errorType;
            this.requestId = requestId;
            this.statusCode = statusCode;
        }

        protected internal GrandCloudServiceException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
            : base(message, innerException)
        {
            this.errorCode = errorCode;
            this.errorType = errorType;
            this.requestId = requestId;
            this.statusCode = statusCode;
        }

        /// <summary>
        /// Whether the error was attributable to <c>Sender</c> or <c>Reciever</c>.
        /// </summary>
        public ErrorType ErrorType
        {
            get { return this.errorType; }
            protected set { this.errorType = value; }
        }

        /// <summary>
        /// The error code returned by the service
        /// </summary>
        public string ErrorCode
        {
            get { return this.errorCode; }
            protected set { this.errorCode = value; }
        }

        /// <summary>
        /// The id of the request which generated the exception.
        /// </summary>
        public string RequestId
        {
            get { return this.requestId; }
            protected set { this.requestId = value; }
        }

        /// <summary>
        /// The HTTP status code from the service response
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
            protected set { this.statusCode = value; }
        }


        #region Internal Set methods

        internal void SetErrorType(ErrorType errorType)
        {
            this.ErrorType = errorType;
        }
        internal void SetErrorCode(string errorCode)
        {
            this.ErrorCode = errorCode;
        }
        internal void SetRequestId(string requestId)
        {
            this.RequestId = requestId;
        }
        internal void SetStatusCode(HttpStatusCode StatusCode)
        {
            this.StatusCode = StatusCode;
        }

        #endregion
    }
}
