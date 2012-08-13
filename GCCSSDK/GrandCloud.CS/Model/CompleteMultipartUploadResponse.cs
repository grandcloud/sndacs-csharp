
using System;
using System.Collections.Generic;
using System.Text;

using GrandCloud.CS.Util;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The CompleteMultipartUploadResponse contains all the information about the
    /// CompleteMultipartUpload method.
    /// </summary>
    public class CompleteMultipartUploadResponse : CSResponse 
    {
        private string location;
        private string bucketName;
        private string key;
        private string etag;

        /// <summary>
        /// Gets and sets the URI that identifies the newly created object.
        /// </summary>
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        /// <summary>
        /// Gets and sets the name of the bucket that contains the newly created object.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Gets and sets the object key of the newly created object.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets and sets Entity tag that identifies the newly created object's data. Objects with different
        /// object data will have different entity tags. The entity tag is an opaque string.
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
            }
        }

    }
}
