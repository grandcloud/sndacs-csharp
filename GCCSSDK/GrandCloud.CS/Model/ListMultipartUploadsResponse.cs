
using System;
using System.Collections.Generic;
using System.Text;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The ListMultipartUploadsResponse contains all the information about the
    /// ListMultipartUploads method.
    /// </summary>
    public class ListMultipartUploadsResponse : CSResponse
    {
        private string bucketName;
        private string keyMarker;
        private string uploadIdMarker;
        private string nextKeyMarker;
        private string nextUploadIdMarker;
        private int maxUploads;
        private bool isTruncated;
        private List<MultipartUpload> multipartUploads;
        private string delimiter;
        private List<string> commonPrefixes;
        private string prefix;


        /// <summary>
        /// Gets and sets the BucketName property.
        /// </summary>
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Gets and sets the KeyMarker which is the key at or after which the listing began.
        /// </summary>
        public string KeyMarker
        {
            get { return this.keyMarker; }
            set { this.keyMarker = value; }
        }

        /// <summary>
        /// Gets and sets the UploadIdMarker property.
        /// </summary>
        public string UploadIdMarker
        {
            get { return this.uploadIdMarker; }
            set { this.uploadIdMarker = value; }
        }

        /// <summary>
        /// Gets and sets the NextKeyMarker property.
        /// <para>
        /// When a list is truncated, specifies the last multipart upload that should
        /// be skipped over to resume listing. Use this value for the KeyMarker
        /// request parameter in a subsequent request.        
        /// </para>
        /// </summary>
        public string NextKeyMarker
        {
            get { return this.nextKeyMarker; }
            set { this.nextKeyMarker = value; }
        }

        /// <summary>
        /// Gets and sets the NextUploadIdMarker property.
        /// <para>
        /// When a list is truncated, specifies the last multipart upload that should
        /// be skipped over to resume listing. Use this value for the
        /// UploadIdMarker request parameter in a subsequent request.
        /// </para>
        /// </summary>
        public string NextUploadIdMarker
        {
            get { return this.nextUploadIdMarker; }
            set { this.nextUploadIdMarker = value; }
        }

        /// <summary>
        /// Gets and sets the MaxUploads property.
        /// </summary>
        public int MaxUploads
        {
            get { return this.maxUploads; }
            set { this.maxUploads = value; }
        }

        /// <summary>
        /// Gets and sets the IsTruncated property.
        /// <para>
        /// Indicates whether the returned list of multipart uploads is truncated. A
        /// value true indicates the list was truncated. The list may be truncated if
        /// the number of multipart uploads exceeds the limit specified by
        /// MaxUploads.
        /// </para>
        /// </summary>
        public bool IsTruncated
        {
            get { return this.isTruncated; }
            set { this.isTruncated = value; }
        }

        /// <summary>
        /// Gets and sets the MultipartUploads property.
        /// <para>
        /// Container for elements related to a particular multipart upload. A response
        /// can contain zero or more Upload elements.
        /// </para>
        /// </summary>
        public List<MultipartUpload> MultipartUploads
        {
            get { return this.multipartUploads; }
            set { this.multipartUploads = value; }
        }

        /// <summary>
        /// Gets and sets the Prefix property.
        /// </summary>
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        /// <summary>
        /// Gets and sets the Delimiter property.
        /// </summary>
        public string Delimiter
        {
            get { return this.delimiter; }
            set { this.delimiter = value; }
        }

        /// <summary>
        /// Gets the CommonPrefixes property. 
        /// A response can contain CommonPrefixes only if you specify a delimiter. 
        /// When you do, CommonPrefixes contains all (if there are any) keys between 
        /// Prefix and the next occurrence of the string specified by delimiter. In effect, 
        /// CommonPrefixes lists keys that act like subdirectories in the directory specified 
        /// by Prefix. For example, if prefix is notes/ and delimiter is a slash (/), in 
        /// notes/summer/july, the common prefix is notes/summer/.
        /// </summary>
        public List<string> CommonPrefixes
        {
            get
            {
                if (this.commonPrefixes == null)
                {
                    this.commonPrefixes = new List<string>();
                }
                return this.commonPrefixes;
            }
        }

    }
}
