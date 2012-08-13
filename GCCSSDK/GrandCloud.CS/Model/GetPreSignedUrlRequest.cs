
using System;
using System.Xml.Serialization;

namespace GrandCloud.CS.Model
{
    /// <summary>
    /// The GetPreSignedUrlRequest contains the parameters used to create
    /// a pre signed URL.
    /// <br />Required Parameters: BucketName, Expires
    /// <br />Optional Parameters: Key, Verb: default is GET
    /// </summary>
    public class GetPreSignedUrlRequest : CSRequest
    {
        #region Private Members

        ResponseHeaderOverrides _responseHeaders;
        string bucketName;
        string contentType;
        string key;
        DateTime? expires;
        Protocol protocol;
        HttpVerb verb;

        #endregion

        #region BucketName

        /// <summary>
        /// Gets and sets the BucketName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "BucketName")]
        public string BucketName
        {
            get { return this.bucketName; }
            set { this.bucketName = value; }
        }

        /// <summary>
        /// Sets the BucketName property for this request.
        /// This is the CS Bucket that contains the CS Object
        /// you want to create a pre signed URL for.
        /// </summary>
        /// <param name="bucketName">The value that BucketName is set to</param>
        /// <returns>this instance</returns>
        public GetPreSignedUrlRequest WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        /// <summary>
        /// Checks if BucketName property is set.
        /// </summary>
        /// <returns>true if BucketName property is set.</returns>
        internal bool IsSetBucketName()
        {
            return !System.String.IsNullOrEmpty(this.bucketName);
        }

        #endregion

        #region Key

        /// <summary>
        /// Gets and sets the Key property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Key")]
        public string Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Sets the Key property for this request.
        /// This is the Key for the CS Object you want to create a 
        /// pre signed URL for.
        /// </summary>
        /// <param name="key">The value that Key is set to</param>
        /// <returns>this instance</returns>
        public GetPreSignedUrlRequest WithKey(string key)
        {
            this.key = key;
            return this;
        }

        /// <summary>
        /// Checks if Key property is set.
        /// </summary>
        /// <returns>true if Key property is set.</returns>
        internal bool IsSetKey()
        {
            return !System.String.IsNullOrEmpty(this.key);
        }

        #endregion

        #region ContentType
        /// <summary>
        /// Gets and sets the ContentType property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ContentType")]
        public string ContentType
        {
            get { return this.contentType; }
            set { this.contentType = value; }
        }

        /// <summary>
        /// Sets the ContentType property for this request.
        /// This property defaults to "binary/octet-stream",
        /// but if you require something else you can set this property.
        /// </summary>
        /// <param name="contentType">the value the ContentType to be set to</param>
        /// <returns>The request with the ContentType set</returns>
        public GetPreSignedUrlRequest WithContentType(string contentType)
        {
            this.contentType = contentType;
            return this;
        }

        /// <summary>
        /// Checks if ContentType property is set.
        /// </summary>
        /// <returns>true if ContentType property is set.</returns>
        internal bool IsSetContentType()
        {
            return !System.String.IsNullOrEmpty(this.contentType);
        }

        #endregion

        #region Expires
        /// <summary>
        /// Gets and sets the Expires property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Expires")]
        public DateTime Expires
        {
            get { return this.expires.GetValueOrDefault(); }
            set { this.expires = value; }
        }

        /// <summary>
        /// Sets the Expires property for this request.
        /// The time when the signature expires, specified as a DateTime 
        /// Object. 
        /// </summary>
        /// <param name="expires">The value that Expires is set to</param>
        /// <returns>this instance</returns>
        public GetPreSignedUrlRequest WithExpires(DateTime expires)
        {
            this.expires = expires;
            return this;
        }

        /// <summary>
        /// Checks if Expires property is set.
        /// </summary>
        /// <returns>true if Expires property is set.</returns>
        public bool IsSetExpires()
        {
            return this.expires.HasValue;
        }

        #endregion

        #region Protocol

        /// <summary>
        /// Gets and sets the Protocol property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Protocol")]
        public Protocol Protocol
        {
            get { return this.protocol; }
            set { this.protocol = value; }
        }

        /// <summary>
        /// Sets the Protocol property for this request.
        /// Specifies whether the pre signed URL will use 
        /// http or https. Defaults to https unless otherwise 
        /// set.
        /// </summary>
        /// <param name="protocol">The value that Protocol is set to</param>
        /// <returns>the response with the Protocol set</returns>
        public GetPreSignedUrlRequest WithProtocol(Protocol protocol)
        {
            this.protocol = protocol;
            return this;
        }

        #endregion

        #region Verb
        /// <summary>
        /// Gets and sets the Verb property.
        /// Specifies the verb used in the pre-signed URL.
        /// Accepted verbs are GET, PUT and HEAD.
        /// Default is GET.
        /// </summary>
        [XmlElementAttribute(ElementName = "Verb")]
        public HttpVerb Verb
        {
            get { return this.verb; }
            set { this.verb = value; }
        }

        /// <summary>
        /// Sets the Verb property for this request.
        /// Specifies which verb to use in the pre-signed URL.
        /// Accepted verbs are GET, PUT and HEAD.
        /// Default value is GET.
        /// </summary>
        /// <param name="verb">The value that Verb is set to</param>
        /// <returns>the response with the Verb set</returns>
        public GetPreSignedUrlRequest WithVerb(HttpVerb verb)
        {
            Verb = verb;
            return this;
        }
        #endregion

        #region Response Headers


        /// <summary>
        /// Gets and sets the response headers to be returned back with the response of the object.
        /// </summary>
        public ResponseHeaderOverrides ResponseHeaderOverrides
        {
            get
            {
                if (this._responseHeaders == null)
                {
                    this._responseHeaders = new ResponseHeaderOverrides();
                }
                return this._responseHeaders;
            }
            set
            {
                this._responseHeaders = value;
            }
        }

        /// <summary>
        /// Sets the ResponseHeaderOverrides property and returns back this instance for method chaining.
        /// </summary>
        /// <param name="responseHeaderOverrides">The response headers</param>
        /// <returns>this instance</returns>
        public GetPreSignedUrlRequest WithResponseHeaderOverrides(ResponseHeaderOverrides responseHeaderOverrides)
        {
            this.ResponseHeaderOverrides = responseHeaderOverrides;
            return this;
        }

        #endregion
    }
}