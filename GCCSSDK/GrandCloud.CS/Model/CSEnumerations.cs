
namespace GrandCloud.CS.Model
{

    internal enum CSQueryParameter
    {
        Action,
        Authorization,
        BucketVersion,
        CanonicalizedResource,
        ContentBody,
        ContentLength,
        ContentType,
        DestinationBucket,
        Expires,
        Key,
        Query,
        QueryToSign,
        Range,
        RequestAddress,
        RequestTimeout,
        RequestReadWriteTimeout,
        Url,
        Verb,
        VerifyChecksum,
        MaxUploads,
        KeyMarker,
        UploadIdMarker
    }

    /// <summary>
    /// An enumeration of all possible CS Bucket region possibilities. For
    /// more information, refer:
    /// <see href="http://www.grandcloud.cn/help/lists/107"/>
    /// </summary>
    public enum CSRegion
    {
        /// <summary>
        /// Specifies that the CS Bucket should use default locality.
        /// This is the default value, which is decided by the server side.
        /// </summary>
        DEFAULT,
        /// <summary>
        /// Specifies that the CS Bucket should use HUADONG1 locality.
        /// </summary>
        HUADONG1,
        /// <summary>
        /// Specifies that the CS Bucket should use HUABEI1 locality.
        /// </summary>
        HUABEI1
    }

    /// <summary>
    /// An enumeration of all protocols that the pre-signed
    /// URL can be created against.
    /// </summary>
    public enum Protocol
    {
        /// <summary>
        /// https protocol will be used in the pre-signed URL.
        /// </summary>
        HTTPS,
        /// <summary>
        /// http protocol will be used in the pre-signed URL.
        /// </summary>
        HTTP
    }

    /// <summary>
    /// An enumeration of supported HTTP verbs
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        /// The GET HTTP verb.
        /// </summary>
        GET,
        /// <summary>
        /// The HEAD HTTP verb.
        /// </summary>
        HEAD,
        /// <summary>
        /// The PUT HTTP verb.
        /// </summary>
        PUT,
        /// <summary>
        /// The DELETE HTTP verb.
        /// </summary>
        DELETE
    }

    /// <summary>
    /// Specifies the Storage Class of of an CS object. Possible values
    /// are: <list type="bullet">
    /// <item>ReducedRedundancy: provides a 99.99% durability guarantee</item>
    /// <item>Standard: provides a 99.999999999% durability guarantee</item>
    /// </list>
    /// </summary>
    public enum CSStorageClass
    {
        /// <summary>
        /// The STANDARD storage class, which is the default
        /// storage class for CS objects. Provides a 99.999999999%
        /// durability guarantee.
        /// </summary>
        Standard,
        /// <summary>
        /// The REDUCED_REDUNDANCY storage class for CS objects. This
        /// provides a reduced (99.99%) durability guarantee at a lower
        /// cost as compared to the STANDARD storage class. Use this
        /// storage class for non-mission critical data or for data 
        /// that doesn’t require the higher level of durability that CS
        /// provides with the STANDARD storage class.
        /// </summary>
        ReducedRedundancy
    }

}