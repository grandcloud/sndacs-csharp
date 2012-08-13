
using System;
using GrandCloud.CS.Util;


namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The GetObjectMetadataResponse contains any headers returned by CS.
    /// </summary>
    public class GetObjectMetadataResponse : CSResponse
    {
        private DateTime? lastModified;
        private string etag;
        private long contentLength;
        private string contentType;

        /// <summary>
        /// Gets and sets the lastModified property.
        /// </summary>
        public DateTime LastModified
        {
            get { return this.lastModified.GetValueOrDefault(); }
            set { this.lastModified = value; }
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
        /// Gets and sets the ContentType property.
        /// </summary>
        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        /// <summary>
        /// Gets and sets the ContentLength property.
        /// </summary>
        public long ContentLength
        {
            get { return this.contentLength; }
            set { this.contentLength = value; }
        }

        /// <summary>
        /// Gets and sets the Headers property.
        /// </summary>
        public override System.Net.WebHeaderCollection Headers
        {
            set
            {
                base.Headers = value;

                string hdr = null;
                if (!String.IsNullOrEmpty(hdr = value.Get("Last-Modified")))
                {
                    this.LastModified = DateTime.SpecifyKind(DateTime.ParseExact(hdr,
                                                                                 CSSDKUtils.GMTDateFormat, 
                                                                                 System.Globalization.CultureInfo.InvariantCulture),
                                                             DateTimeKind.Utc);
                }

                if (!String.IsNullOrEmpty(hdr = value.Get(CSSDKUtils.ETagHeader)))
                {
                    this.ETag = hdr;
                }

                if (!String.IsNullOrEmpty(hdr = value.Get(CSSDKUtils.ContentTypeHeader)))
                {
                    this.ContentType = hdr;
                }

                if (!String.IsNullOrEmpty(hdr = value.Get(CSSDKUtils.ContentLengthHeader)))
                {
                    this.ContentLength = System.Convert.ToInt64(hdr);
                }
            }
        }
    }
}