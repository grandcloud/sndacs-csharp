
using System;

using GrandCloud.CS.Util;


namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The PutObjectResponse contains any headers returned by CS.
    /// </summary>
    public class PutObjectResponse : CSResponse
    {
        private string etag;

        /// <summary>
        /// Gets and sets the ETag property.
        /// </summary>
        public string ETag
        {
            get { return this.etag; }
            set { this.etag = value; }
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
                if (!System.String.IsNullOrEmpty(hdr = value.Get(CSSDKUtils.ETagHeader)))
                {
                    this.ETag = hdr;
                }

            }
        }
    }
}