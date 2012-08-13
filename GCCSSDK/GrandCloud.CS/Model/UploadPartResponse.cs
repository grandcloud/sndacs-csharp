
using System;
using System.Collections.Generic;
using System.Text;


using GrandCloud.CS.Util;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The UploadPartResponse contains all the information about the
    /// UploadPart method.
    /// </summary>
    public class UploadPartResponse : CSResponse
    {
        private int partNumber;
        private string etag;

        /// <summary>
        /// Gets and sets the part number specified for the part upload.  This is needed when
        /// completing the multipart upload.
        /// </summary>
        public int PartNumber
        {
            get { return this.partNumber; }
            set { this.partNumber = value; }
        }

        /// <summary>
        /// Gets and sets the Entity tag returned when the part was uploaded.  This is needed 
        /// when completing the multipart upload.
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
