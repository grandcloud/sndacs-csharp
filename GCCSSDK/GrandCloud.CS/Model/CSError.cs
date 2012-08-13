
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// Represents an error returned by the CS service. Exposes 
    /// an error code, a message, a host ID and a request ID for
    /// debugging purposes.
    /// </summary>
    public class CSError
    {
        private string code;
        private string message;
        private string requestId;
        private string etag;
        private string resource;

        /// <summary>
        /// Gets and sets the Code property.
        /// </summary>
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        /// <summary>
        /// Gets and sets the Message property.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        /// <summary>
        /// Gets and sets the RequestId property.
        /// </summary>
        public string RequestId
        {
            get { return this.requestId; }
            set { this.requestId = value; }
        }

        /// <summary>
        /// Gets and sets the ETag property.
        /// </summary>
        public string ETag
        {
            get { return this.etag; }
            set { this.etag = value; }
        }

        /// <summary>
        /// Gets and sets the Resource property.
        /// </summary>
        public string Resource
        {
            get { return this.resource; }
            set { this.resource = value; }
        }
    }
}
