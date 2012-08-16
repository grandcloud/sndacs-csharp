
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The ListPartsResponse contains all the information about the
    /// ListParts method.
    /// </summary>
    public class ListPartsResponse : CSResponse
    {
        private string bucketName;
        private string key;
        private string uploadId;
        private string partNumberMarker;
        private string nextPartNumberMarker;
        private int maxParts;
        private bool isTruncated;
        private List<PartDetail> parts;

        /// <summary>
        /// Gets and sets the name of the bucket to which the multipart upload was initiated.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Gets and sets the object key for which the multipart upload was initiated.
        /// </summary>
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets and sets the upload ID identifying the multipart upload whose parts are being listed.
        /// </summary>
        public string UploadId
        {
            get { return this.uploadId; }
            set { this.uploadId = value; }
        }

        /// <summary>
        /// Gets and sets the part number after which listing begins.
        /// </summary>
        public string PartNumberMarker
        {
            get { return this.partNumberMarker; }
            set { this.partNumberMarker = value; }
        }

        /// <summary>
        /// Gets and sets the NextPartNumberMarker property.
        /// <para>
        /// When a list is truncated, specifies the last part that should be skipped
        /// over to resume listing. Use this value for the PartNumberMarker
        /// request property in a subsequent request.
        /// </para>
        /// </summary>
        public string NextPartNumberMarker
        {
            get { return this.nextPartNumberMarker; }
            set { this.nextPartNumberMarker = value; }
        }

        /// <summary>
        /// Gets and sets the maximum number of parts allowed in a response.
        /// </summary>
        public int MaxParts
        {
            get { return this.maxParts; }
            set { this.maxParts = value; }
        }

        /// <summary>
        /// Gets and sets the IsTruncated property.
        /// <para>
        /// Indicates whether the returned list of parts is truncated. A value true
        /// indicates the list was truncated. A list may be truncated if the number of
        /// parts exceeds the limit specified by MaxParts.
        /// </para>
        /// </summary>
        public bool IsTruncated
        {
            get { return this.isTruncated; }
            set { this.isTruncated = value; }
        }

        /// <summary>
        /// Gets and sets the Parts property.
        /// <para>
        /// PartDetails is a container for elements related to a particular part. A response can contain
        /// zero or more Part elements.
        /// </para>
        /// </summary>
        public List<PartDetail> Parts
        {
            get { return this.parts; }
            set { this.parts = value; }
        }

    }
}
