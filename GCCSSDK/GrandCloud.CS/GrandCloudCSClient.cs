
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;



using GrandCloud.CS.Model;
using GrandCloud.CS.Util;


using Map = System.Collections.Generic.IDictionary<GrandCloud.CS.Model.CSQueryParameter, string>;

namespace GrandCloud.CS
{
    public class GrandCloudCSClient : GrandCloudCS
    {
        #region Private Members

        private GrandCloudCSConfig config;
        private bool disposed;
        private Type myType;
        private bool ownCredentials;
        private CSCredentials credentials;

        #endregion

        static Logger LOGGER = new Logger(typeof(GrandCloudCSClient));
        static MethodInfo ADD_RANGE_METHODINFO;


        #region Dispose Pattern

        /// <summary>
        /// Implements the Dispose pattern for the GrandCloudCSClient
        /// </summary>
        /// <param name="fDisposing">Whether this object is being disposed via a call to Dispose
        /// or garbage collected.</param>
        protected virtual void Dispose(bool fDisposing)
        {
            if (!this.disposed)
            {
                if (fDisposing && credentials != null)
                {
                    if (ownCredentials)
                    {
                        credentials.Dispose();
                    }
                    credentials = null;
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Disposes of all managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The destructor for the client class.
        /// </summary>
        ~GrandCloudCSClient()
        {
            this.Dispose(false);
        }

        #endregion


        #region Constructors

        static GrandCloudCSClient()
        {
            Type t = typeof(HttpWebRequest);
            ADD_RANGE_METHODINFO = t.GetMethod("AddRange", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), typeof(string), typeof(string) }, null);
        }

        /// <summary>
        /// Constructs GrandCloudCSClient with the credentials defined in the App.config.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSAccessKey" value="********************"/&gt;
        ///         &lt;add key="AWSSecretKey" value="****************************************"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        public GrandCloudCSClient()
            : this(FallbackCredentialsFactory.GetCredentials(), new GrandCloudCSConfig(), true) { }

        /// <summary>
        /// Constructs GrandCloudCSClient with the credentials defined in the App.config.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSAccessKey" value="********************"/&gt;
        ///         &lt;add key="AWSSecretKey" value="****************************************"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        /// <param name="config">The GrandCloudCSClient Configuration Object</param>
        public GrandCloudCSClient(GrandCloudCSConfig config)
            : this(FallbackCredentialsFactory.GetCredentials(), config, true) { }

        /// <summary>
        /// Constructs GrandCloudCSClient with AWS Access Key ID and AWS Secret Key
        /// </summary>
        /// <param name="csAccessKeyId">AWS Access Key ID</param>
        /// <param name="csSecretAccessKey">AWS Secret Access Key</param>
        public GrandCloudCSClient(string awsAccessKeyId, string awsSecretAccessKey)
            : this(CreateCredentials(awsAccessKeyId, awsSecretAccessKey), new GrandCloudCSConfig(), true) { }

        /// <summary>
        /// Constructs GrandCloudCSClient with AWS Access Key ID, AWS Secret Key and an
        /// GrandCloudCS Configuration object. If the config object's
        /// UseSecureStringForGrandCloudSecretKey is false, the AWS Secret Key
        /// is stored as a clear-text string. Please use this option only
        /// if the application environment doesn't allow the use of SecureStrings.
        /// </summary>
        /// <param name="csAccessKeyId">AWS Access Key ID</param>
        /// <param name="csSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="config">The CS Configuration Object</param>
        public GrandCloudCSClient(string awsAccessKeyId, string awsSecretAccessKey, GrandCloudCSConfig config)
            : this(CreateCredentials(awsAccessKeyId, awsSecretAccessKey), config, true) { }

        /// <summary>
        /// Constructs an GrandCloudCSClient with AWS Access Key ID, AWS Secret Key and an
        /// GrandCloud CS Configuration object
        /// </summary>
        /// <param name="csAccessKeyId">AWS Access Key ID</param>
        /// <param name="csSecretAccessKey">AWS Secret Access Key as a SecureString</param>
        /// <param name="config">The CS Configuration Object</param>
        public GrandCloudCSClient(string awsAccessKeyId, SecureString awsSecretAccessKey, GrandCloudCSConfig config)
            : this(CreateCredentials(awsAccessKeyId, awsSecretAccessKey), config, true) { }

        /// <summary>
        /// Constructs an GrandCloudCSClient with CSCredentials
        /// </summary>
        /// <param name="credentials"></param>
        public GrandCloudCSClient(CSCredentials credentials)
            : this(credentials, new GrandCloudCSConfig()) { }

        /// <summary>
        /// Constructs an GrandCloudCSClient with CSCredentials and an
        /// GrandCloud CS Configuration object
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="config"></param>
        public GrandCloudCSClient(CSCredentials credentials, GrandCloudCSConfig config)
            : this(credentials, config, false) { }

        private GrandCloudCSClient(CSCredentials credentials, GrandCloudCSConfig config, bool ownCredentials)
        {
            this.config = config;
            this.myType = this.GetType();
            this.credentials = credentials;
            this.ownCredentials = ownCredentials;
        }

        #endregion

        #region GetPreSignedURL

        /// <summary>
        /// The GetPreSignedURL operations creates a signed http request.
        /// Query string authentication is useful for giving HTTP or browser
        /// access to resources that would normally require authentication.
        /// When using query string authentication, you create a query,
        /// specify an expiration time for the query, sign it with your
        /// signature, place the data in an HTTP request, and distribute
        /// the request to a user or embed the request in a web page.
        /// A PreSigned URL can be generated for GET, PUT and HEAD
        /// operations on your bucket, keys, and versions.
        /// </summary>
        /// <param name="request">The GetPreSignedUrlRequest that defines the
        /// parameters of the operation.</param>
        /// <returns>A string that is the signed http request.</returns>
        /// <exception cref="T:System.ArgumentException" />
        /// <exception cref="T:System.ArgumentNullException" />
        public string GetPreSignedURL(GetPreSignedUrlRequest request)
        {
            if (credentials == null)
            {
                throw new GrandCloudCSException("Credentials must be specified, cannot call method anonymously");
            }

            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The PreSignedUrlRequest specified is null!");
            }

            if (!request.IsSetExpires())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Expires Specified is null!");
            }

            if (request.Verb > HttpVerb.PUT)
            {
                throw new ArgumentException(
                    CSConstants.RequestParam,
                    "An Invalid HttpVerb was specified for the GetPreSignedURL request. Valid - GET, HEAD, PUT"                    
                    );
            }

            if (!config.ServiceURL.Equals("storage.sdcloud.cn"))
            {
                throw new ArgumentException(CSConstants.RequestParam, "Pre-signed URL is supported by storage.sdcloud.cn only.");
            }

            ConvertGetPreSignedUrl(request);
            return request.parameters[CSQueryParameter.Url];
        }

        #endregion

        #region ListAllBuckets
        /// <summary>
        /// The ListAllBuckets operation returns a list of all of the buckets
        /// owned by the authenticated sender of the request.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListBucketsResponse with the response from CS.</returns>
        public ListBucketsResponse ListAllBuckets()
        {
            return ListAllBuckets(new ListBucketsRequest());
        }

        /// <summary>
        /// Initiates the asynchronous execution of the ListAllBuckets operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.EnableBucketLogging"/>
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndListAllBuckets.</returns>
        public IAsyncResult BeginListAllBuckets(AsyncCallback callback, object state)
        {
            return BeginListAllBuckets(new ListBucketsRequest(), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the ListAllBuckets operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.EnableBucketLogging"/>
        /// </summary>
        /// <param name="request">The ListBucketsRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndListAllBuckets.</returns>
        public IAsyncResult BeginListAllBuckets(ListBucketsRequest request, AsyncCallback callback, object state)
        {
            return invokeListBuckets(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the ListAllBuckets operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListAllBuckets.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListBucketsResponse from CS.</returns>
        public ListBucketsResponse EndListAllBuckets(IAsyncResult asyncResult)
        {
            return endOperation<ListBucketsResponse>(asyncResult);
        }

        /// <summary>
        /// The ListAllBuckets operation returns a list of all of the buckets
        /// owned by the authenticated sender of the request.
        /// </summary>
        /// <param name="request">The ListBucketsRequest that defines the parameters
        /// of this operation</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListBucketsResponse with the response from CS.</returns>
        public ListBucketsResponse ListAllBuckets(ListBucketsRequest request)
        {
            IAsyncResult asyncResult = invokeListBuckets(request, null, null, true);
            return EndListAllBuckets(asyncResult);
        }

        IAsyncResult invokeListBuckets(ListBucketsRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The ListObjectsRequest is null!");
            }

            ConvertListBuckets(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<ListBucketsResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region GetBucketLocation

        /// <summary>
        /// Initiates the asynchronous execution of the GetBucketLocation operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.EnableBucketLogging"/>
        /// </summary>
        /// <param name="request">The GetBucketLocationRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndGetBucketLocation.</returns>
        public IAsyncResult BeginGetBucketLocation(GetBucketLocationRequest request, AsyncCallback callback, object state)
        {
            return invokeGetBucketLocation(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the GetBucketLocation operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetBucketLocation.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetBucketLocationResponse from CS.</returns>
        public GetBucketLocationResponse EndGetBucketLocation(IAsyncResult asyncResult)
        {
            return endOperation<GetBucketLocationResponse>(asyncResult);
        }

        /// <summary>
        /// The GetBucketLocation operation takes in a bucket's name and lists the location
        /// of the bucket. This information can be used to determine the bucket's geographical
        /// location.
        /// To determine the location of a bucket, you must be the bucket owner.
        /// </summary>
        /// <param name="request">The GetBucketLocationRequest that defines the parameters of the operation.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetBucketLocationResponse from CS.</returns>
        public GetBucketLocationResponse GetBucketLocation(GetBucketLocationRequest request)
        {
            IAsyncResult asyncResult = invokeGetBucketLocation(request, null, null, true);
            return EndGetBucketLocation(asyncResult);
        }

        IAsyncResult invokeGetBucketLocation(GetBucketLocationRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The GetBucketLocationRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertGetBucketLocation(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<GetBucketLocationResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region GetBucketPolicy

        /// <summary>
        /// Initiates the asynchronous execution of the GetBucketPolicy operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.GetBucketPolicy"/>
        /// </summary>
        /// <param name="request">The GetBucketPolicyRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndGetBucketPolicy.</returns>
        public IAsyncResult BeginGetBucketPolicy(GetBucketPolicyRequest request, AsyncCallback callback, object state)
        {
            return invokeGetBucketPolicy(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the GetBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetBucketPolicyResponse from CS.</returns>
        public GetBucketPolicyResponse EndGetBucketPolicy(IAsyncResult asyncResult)
        {
            try
            {
                return endOperation<GetBucketPolicyResponse>(asyncResult);
            }
            catch (GrandCloudCSException e)
            {
                if (e.ErrorCode == CSConstants.NoSuchBucketPolicy)
                {
                    return new GetBucketPolicyResponse();
                }

                throw;
            }
        }

        /// <summary>
        /// <para>
        /// Retrieves the policy for the specified bucket. Only the owner of the
        /// bucket can retrieve the policy. If no policy has been set for the bucket,
        /// then an error will be thrown.
        /// </para>
        /// <para>
        /// Bucket policies provide access control management at the bucket level for
        /// both the bucket resource and contained object resources. Only one policy
        /// can be specified per-bucket.
        /// </para>
        /// <para>
        /// For more information on forming bucket polices, please check grandcloud ecs documents.        
        /// </para>
        /// </summary>
        /// <param name="request">The GetBucketPolicyRequest that defines the parameters of the operation.</param>
        /// <returns>Returns a GetBucketPolicyResponse from CS.</returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        public GetBucketPolicyResponse GetBucketPolicy(GetBucketPolicyRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The GetBucketPolicyRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertGetBucketPolicy(request);
            IAsyncResult asyncResult = invokeGetBucketPolicy(request, null, null, true);
            return EndGetBucketPolicy(asyncResult);
        }

        IAsyncResult invokeGetBucketPolicy(GetBucketPolicyRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The GetBucketPolicyRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertGetBucketPolicy(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<GetBucketPolicyResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region SetBucketPolicy

        /// <summary>
        /// Initiates the asynchronous execution of the SetBucketPolicy operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.SetBucketPolicy"/>
        /// </summary>
        /// <param name="request">The PutBucketPolicyRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndSetBucketPolicy.</returns>
        public IAsyncResult BeginSetBucketPolicy(PutBucketPolicyRequest request, AsyncCallback callback, object state)
        {
            return invokePutBucketPolicy(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the SetBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginSetBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutBucketPolicyResponse from CS.</returns>
        public PutBucketPolicyResponse EndSetBucketPolicy(IAsyncResult asyncResult)
        {
            return endOperation<PutBucketPolicyResponse>(asyncResult);
        }

        /// <summary>
        /// <para>
        /// Sets the policy associated with the specified bucket. Only the owner of
        /// the bucket can set a bucket policy. If a policy already exists for the
        /// specified bucket, the new policy will replace the existing policy.
        /// </para>
        /// <para>
        /// Bucket policies provide access control management at the bucket level for
        /// both the bucket resource and contained object resources. Only one policy
        /// may be specified per-bucket.
        /// </para>
        /// <para>
        /// For more information on forming bucket polices, 
        /// refer: <see href="http://docs.amazonwebservices.com/GrandCloudCS/latest/dev/"/>
        /// </para>
        /// </summary>
        /// <param name="request">The PutBucketPolicyRequest that defines the parameters of the operation.</param>
        /// <returns>Returns a PutBucketPolicyResponse from CS.</returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        public PutBucketPolicyResponse SetBucketPolicy(PutBucketPolicyRequest request)
        {
            IAsyncResult asyncResult = invokePutBucketPolicy(request, null, null, true);
            return EndSetBucketPolicy(asyncResult);
        }

        IAsyncResult invokePutBucketPolicy(PutBucketPolicyRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The PutBucketPolicyRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            if (!request.IsSetPolicy())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The policy specified is null or empty!");
            }

            ConvertPutBucketPolicy(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<PutBucketPolicyResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region DeleteBucketPolicy

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteBucketPolicy operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.DeleteBucketPolicy"/>
        /// </summary>
        /// <param name="request">The DeleteBucketPolicyRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndDeleteBucketPolicy.</returns>
        public IAsyncResult BeginDeleteBucketPolicy(DeleteBucketPolicyRequest request, AsyncCallback callback, object state)
        {
            return invokeDeleteBucketPolicy(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteBucketPolicyResponse from CS.</returns>
        public DeleteBucketPolicyResponse EndDeleteBucketPolicy(IAsyncResult asyncResult)
        {
            return endOperation<DeleteBucketPolicyResponse>(asyncResult);
        }

        /// <summary>
        /// <para>
        /// Deletes the policy associated with the specified bucket. Only the owner
        /// of the bucket can delete the bucket policy.
        /// </para>
        /// <para>
        /// If you delete a policy that does not exist, GrandCloud CS will return a
        /// success (not an error message).
        /// </para>
        /// <para>
        /// Bucket policies provide access control management at the bucket level for
        /// both the bucket resource and contained object resources. Only one policy
        /// may be specified per-bucket.
        /// </para>
        /// <para>
        /// For more information on forming bucket polices, 
        /// refer: <see href="http://docs.amazonwebservices.com/GrandCloudCS/latest/dev/"/>
        /// </para>
        /// </summary>
        /// <param name="request">The DeleteBucketPolicyRequest that defines the parameters of the operation.</param>
        /// <returns>Returns a DeleteBucketPolicyResponse from CS.</returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        public DeleteBucketPolicyResponse DeleteBucketPolicy(DeleteBucketPolicyRequest request)
        {
            IAsyncResult asyncResult = invokeDeleteBucketPolicy(request, null, null, true);
            return EndDeleteBucketPolicy(asyncResult);
        }

        IAsyncResult invokeDeleteBucketPolicy(DeleteBucketPolicyRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The DeleteBucketPolicyRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertDeleteBucketPolicy(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<DeleteBucketPolicyResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region ListObjects

        /// <summary>
        /// Initiates the asynchronous execution of the ListObjects operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.ListObjects"/>
        /// </summary>
        /// <param name="request">The ListObjectsRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndListObjects.</returns>
        public IAsyncResult BeginListObjects(ListObjectsRequest request, AsyncCallback callback, object state)
        {
            return invokeListObjects(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the ListObjects operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListObjects.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListObjectsResponse from CS.</returns>
        public ListObjectsResponse EndListObjects(IAsyncResult asyncResult)
        {
            return endOperation<ListObjectsResponse>(asyncResult);
        }

        /// <summary>
        /// The ListObjects operation lists the objects/keys in a bucket ordered
        /// lexicographically (from a-Z). The list can be filtered via the Marker
        /// property of the ListObjectsRequest.
        /// In order to List Objects, you must have READ access to the bucket.
        /// </summary>
        /// <param name="request">
        /// The ListObjectsRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListObjectsResponse from CS with a list of CSObjects,
        /// headers and request parameters used to filter the list.</returns>
        /// <remarks>
        /// Since buckets can contain a virtually unlimited number of objects, the complete
        /// results of a list query can be extremely large. To manage large result sets,
        /// GrandCloud CS uses pagination to split them into multiple responses. Callers should
        /// always check the <see cref="P:GrandCloud.CS.Model.ListObjectsResponse.IsTruncated" />
        /// to see if the returned listing
        /// is complete, or if callers need to make additional calls to get more results.
        /// The marker parameter allows callers to specify where to start the object listing.
        /// List performance is not substantially affected by the total number of keys in your
        /// bucket, nor by the presence or absence of any additional query parameters.
        /// </remarks>
        public ListObjectsResponse ListObjects(ListObjectsRequest request)
        {
            IAsyncResult asyncResult = invokeListObjects(request, null, null, true);
            return EndListObjects(asyncResult);
        }

        IAsyncResult invokeListObjects(ListObjectsRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The ListObjectsRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertListObjects(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<ListObjectsResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region CreateBucket

        /// <summary>
        /// Initiates the asynchronous execution of the CreateBucket operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.CreateBucket"/>
        /// </summary>
        /// <param name="request">The PutBucketRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndCreateBucket.</returns>
        public IAsyncResult BeginCreateBucket(PutBucketRequest request, AsyncCallback callback, object state)
        {
            return invokePutBucket(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the CreateBucket operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCreateBucket.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutBucketResponse from CS.</returns>
        public PutBucketResponse EndCreateBucket(IAsyncResult asyncResult)
        {
            return endOperation<PutBucketResponse>(asyncResult);
        }

        /// <summary>
        /// The CreateBucket operation creates a new CS Bucket.
        /// Depending on your latency and legal requirements, you can specify a location
        /// constraint that will affect where your data physically resides.
        /// </summary>
        /// <param name="request">
        /// The PutBucketRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutBucketResponse from CS.</returns>
        /// <remarks>
        /// Every object stored in GrandCloud CS is contained in a bucket. Buckets
        /// partition the namespace of objects stored in GrandCloud CS at the top level.
        /// Within a bucket, you can use any names for your objects, but bucket names
        /// must be unique across all of GrandCloud CS.
        /// There is no limit to the number of objects that can be stored in a bucket and no
        /// variation in performance when using many buckets or just a few. You can store all
        /// of your objects in a single bucket or organize them across several buckets.
        /// </remarks>
        /// <seealso cref="T:GrandCloud.CS.Model.CSRegion"/>
        public PutBucketResponse CreateBucket(PutBucketRequest request)
        {
            IAsyncResult asyncResult = invokePutBucket(request, null, null, true);
            return EndCreateBucket(asyncResult);
        }

        IAsyncResult invokePutBucket(PutBucketRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The PutBucketRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertPutBucket(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<PutBucketResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region DeleteBucket

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteBucket operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.DeleteBucket"/>
        /// </summary>
        /// <param name="request">The DeleteBucketRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndDeleteBucket.</returns>
        public IAsyncResult BeginDeleteBucket(DeleteBucketRequest request, AsyncCallback callback, object state)
        {
            return invokeDeleteBucket(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteBucket operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteBucket.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteBucketResponse from CS.</returns>
        public DeleteBucketResponse EndDeleteBucket(IAsyncResult asyncResult)
        {
            return endOperation<DeleteBucketResponse>(asyncResult);
        }

        /// <summary>
        /// The DeleteBucket operation deletes the bucket named in the request.
        /// All objects in the bucket must be deleted before the bucket itself can be deleted.
        /// Only the owner of a bucket can delete it, regardless of the bucket's access control policy.
        /// </summary>
        /// <param name="request">
        /// The DeleteBucketRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteBucketResponse from CS.</returns>
        public DeleteBucketResponse DeleteBucket(DeleteBucketRequest request)
        {
            IAsyncResult asyncResult = invokeDeleteBucket(request, null, null, true);
            return EndDeleteBucket(asyncResult);
        }

        IAsyncResult invokeDeleteBucket(DeleteBucketRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The DeleteBucketRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }

            ConvertDeleteBucket(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<DeleteBucketResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region GetObject

        /// <summary>
        /// Initiates the asynchronous execution of the GetObject operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.GetObject"/>
        /// </summary>
        /// <param name="request">The GetObjectRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndGetObject.</returns>
        public IAsyncResult BeginGetObject(GetObjectRequest request, AsyncCallback callback, object state)
        {
            return invokeGetObject(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the GetObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectResponse from CS.</returns>
        public GetObjectResponse EndGetObject(IAsyncResult asyncResult)
        {
            GetObjectResponse response = endOperation<GetObjectResponse>(asyncResult);

            CSAsyncResult csAsyncResult = asyncResult as CSAsyncResult;
            GetObjectRequest request = csAsyncResult.CSRequest as GetObjectRequest;
            response.BucketName = request.BucketName;
            response.Key = request.Key;

            return response;
        }

        /// <summary>
        /// The GetObject operation fetches the most recent version of an CS object
        /// from the specified CS bucket. You must have READ access to the object.
        /// If READ access is granted to an anonymous user, an object can be retrieved
        /// without an authorization header. Providing a version-id for the object will
        /// fetch the specific version from CS instead of the most recent one.
        /// </summary>
        /// <param name="request">
        /// The GetObjectRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectResponse from CS.</returns>
        /// <remarks>
        /// Please wrap the response you get from calling GetObject in a using clause.
        /// This ensures that all underlying IO resources allocated for the response
        /// are disposed once the response has been processed. This is one way to
        /// call GetObject:
        /// <code>
        /// using (GetObjectResponse response = csClient.GetObject(request))
        /// {
        ///     ... Process the response:
        ///     Get the Stream, get the content-length, write contents to disk, etc
        /// }
        /// </code>
        /// To see what resources are cleaned up at the end of the using block, please
        /// see <see cref="M:GrandCloud.CS.Model.CSResponse.Dispose"/>
        /// </remarks>
        public GetObjectResponse GetObject(GetObjectRequest request)
        {
            IAsyncResult asyncResult = invokeGetObject(request, null, null, true);
            GetObjectResponse response = EndGetObject(asyncResult);
            ((CSAsyncResult)asyncResult).FinalResponse = null;
            return response;
        }

        IAsyncResult invokeGetObject(GetObjectRequest request, AsyncCallback callback, object state, bool synchronzied)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The GetObjectRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Key Specified is null or empty!");
            }

            ConvertGetObject(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronzied);
            invoke<GetObjectResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region HeadObject

        /// <summary>
        /// Initiates the asynchronous execution of the HeadObject operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.HeadObject"/>
        /// </summary>
        /// <param name="request">The GetObjectMetadataRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndHeadObject.</returns>
        public IAsyncResult BeginHeadObject(GetObjectMetadataRequest request, AsyncCallback callback, object state)
        {
            return invokeGetObjectMetadata(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the HeadObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginHeadObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectMetadataResponse from CS.</returns>
        public GetObjectMetadataResponse EndHeadObject(IAsyncResult asyncResult)
        {
            return endOperation<GetObjectMetadataResponse>(asyncResult);
        }

        /// <summary>
        /// The HeadObject operation is used to retrieve information about a specific object
        /// or object size, without actually fetching the object itself. This is useful if you're
        /// only interested in the object metadata, and don't want to waste bandwidth on the object data.
        /// The response is identical to the GetObject response, except that there is no response body.
        /// </summary>
        /// <param name="request">
        /// The GetObjectMetadataRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectMetadataResponse from CS.</returns>
        public GetObjectMetadataResponse HeadObject(GetObjectMetadataRequest request)
        {
            IAsyncResult asyncResult = invokeGetObjectMetadata(request, null, null, true);
            return EndHeadObject(asyncResult);
        }

        IAsyncResult invokeGetObjectMetadata(GetObjectMetadataRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The GetObjectMetadataRequest specified is null!");
            }

            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Key Specified is null or empty!");
            }

            ConvertGetObjectMetadata(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<GetObjectMetadataResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region PutObject

        /// <summary>
        /// Initiates the asynchronous execution of the PutObject operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.PutObject"/>
        /// </summary>
        /// <param name="request">The PutObjectRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndPutObject.</returns>
        public IAsyncResult BeginPutObject(PutObjectRequest request, AsyncCallback callback, object state)
        {
            return invokePutObject(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the PutObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginPutObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutObjectResponse from CS.</returns>
        public PutObjectResponse EndPutObject(IAsyncResult asyncResult)
        {
            // from the checks made, it is guaranteed that if a filename is not specified
            // and the flow of execution gets this far, there has to be either an InputStream
            // or a ContentBody with a Key
            try
            {
                PutObjectResponse response = endOperation<PutObjectResponse>(asyncResult);
                return response;
            }
            finally
            {
                try
                {
                    CSAsyncResult csAsyncResult = asyncResult as CSAsyncResult;
                    PutObjectRequest request = csAsyncResult.CSRequest as PutObjectRequest;
                    if (request.InputStream != null && (request.IsSetFilePath() || request.AutoCloseStream))
                    {
                        request.InputStream.Close();
                    }
                }
                catch (Exception e)
                {
                    if (LOGGER.IsDebugEnabled)
                        LOGGER.Error("Error closing stream after PutObject.", e);
                }
            }
        }

        /// <summary>
        /// The PutObject operation adds an object to an CS Bucket.
        /// The response indicates that the object has been successfully stored.
        /// GrandCloud CS never stores partial objects: if you receive a successful
        /// response, then you can be confident that the entire object was stored.
        ///
        /// To ensure data is not corrupted over the network, use the Content-MD5
        /// header. When you use the Content-MD5 header, GrandCloud CS checks the object
        /// against the provided MD5 value. If they do not match, GrandCloud CS returns an error.
        /// Additionally, you can calculate the MD5 while putting an object to
        /// GrandCloud CS and compare the returned Etag to the calculated MD5 value.
        ///
        /// If an object already exists in a bucket, the new object will overwrite
        /// it because GrandCloud CS stores the last write request. However, GrandCloud CS
        /// is a distributed system. If GrandCloud CS receives multiple write requests
        /// for the same object nearly simultaneously, all of the objects might be
        /// stored, even though only one wins in the end. GrandCloud CS does not provide
        /// object locking; if you need this, make sure to build it into your application
        /// layer.
        ///
        /// If you specify a location constraint when creating a bucket, all objects
        /// added to the bucket are stored in the bucket's location.
        ///
        /// You must have WRITE access to the bucket to add an object.
        /// </summary>
        /// <param name="request">
        /// The PutObjectRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentException"></exception>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <exception cref="T:System.IO.FileNotFoundException"></exception>
        /// <returns>Returns a PutObjectResponse from CS.</returns>
        public PutObjectResponse PutObject(PutObjectRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokePutObject(request, null, null, true);
            return EndPutObject(asyncResult);
        }

        IAsyncResult invokePutObject(PutObjectRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The PutObjectRequest specified is null!");
            }

            // The BucketName and one of either the Key or the FilePath needs to be set
            if (!request.IsSetBucketName())
            {
                throw new ArgumentException("An CS Bucket must be specified for CS PUT object.");
            }

            if (!(request.IsSetKey() || request.IsSetFilePath()))
            {
                throw new ArgumentException(
                    "Either a Key or a Filename need to be specified for CS PUT object.");
            }

            // Either:
            // 1. A file is being transferred - so a filename or a stream needs to be provided
            // 2. The content body needs to be set
            if (!request.IsSetFilePath() &&
                !request.IsSetInputStream() &&
                !request.IsSetContentBody())
            {
                throw new ArgumentException(
                    "Please specify either a Filename, provide a FileStream or provide a ContentBody to PUT an object into CS.");
            }

            if (request.IsSetInputStream() && request.IsSetContentBody())
            {
                throw new ArgumentException(
                    "Please specify one of either an Input FileStream or the ContentBody to be PUT as an CS object.");
            }

            if (request.IsSetInputStream() && request.IsSetFilePath())
            {
                throw new ArgumentException(
                    "Please specify one of either an Input FileStream or a Filename to be PUT as an CS object.");
            }

            if (request.IsSetFilePath() && request.IsSetContentBody())
            {
                throw new ArgumentException(
                    "Please specify one of either a Filename or the ContentBody to be PUT as an CS object.");
            }

            if (request.IsSetFilePath())
            {
                // Create a stream from the filename specified
                if (File.Exists(request.FilePath))
                {
                    request.InputStream = new FileStream(request.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    throw new FileNotFoundException("The specified file does not exist");
                }

                if (!request.IsSetKey())
                {
                    string name = request.FilePath;
                    // Set the key to be the name of the file sans directories
                    request.Key = name.Substring(name.LastIndexOf(@"\") + 1);
                }
            }

            ConvertPutObject(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<PutObjectResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region DeleteObject

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteObject operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.DeleteObject"/>
        /// </summary>
        /// <param name="request">The DeleteObjectRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndDeleteObject.</returns>
        public IAsyncResult BeginDeleteObject(DeleteObjectRequest request, AsyncCallback callback, object state)
        {
            return invokeDeleteObject(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteObjectResponse from CS.</returns>
        public DeleteObjectResponse EndDeleteObject(IAsyncResult asyncResult)
        {
            return endOperation<DeleteObjectResponse>(asyncResult);
        }

        /// <summary>
        /// The DeleteObject operation removes the specified object from GrandCloud CS.
        /// Once deleted, there is no method to restore or undelete an object.
        ///
        /// If you delete an object that does not exist, GrandCloud CS will return a
        /// success (not an error message).
        /// </summary>
        /// <param name="request">
        /// The DeleteObjectRequest that defines the parameters of the operation.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteObjectResponse from CS.</returns>
        public DeleteObjectResponse DeleteObject(DeleteObjectRequest request)
        {
            IAsyncResult asyncResult = invokeDeleteObject(request, null, null, true);
            return EndDeleteObject(asyncResult);
        }

        IAsyncResult invokeDeleteObject(DeleteObjectRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The DeleteObjectRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CS Key Specified is null or empty!");
            }

            ConvertDeleteObject(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<DeleteObjectResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region InitiateMultipartUpload

        /// <summary>
        /// Initiates the asynchronous execution of the InitiateMultipartUpload operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.InitiateMultipartUpload"/>
        /// </summary>
        /// <param name="request">The InitiateMultipartUploadRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndInitiateMultipartUpload.</returns>
        public IAsyncResult BeginInitiateMultipartUpload(InitiateMultipartUploadRequest request, AsyncCallback callback, object state)
        {
            return invokeInitiateMultipartUpload(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the InitiateMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginInitiateMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a InitiateMultipartUploadResponse from CS.</returns>
        public InitiateMultipartUploadResponse EndInitiateMultipartUpload(IAsyncResult asyncResult)
        {
            return endOperation<InitiateMultipartUploadResponse>(asyncResult);
        }

        /// <summary>
        /// This method initiates a multipart upload and returns an InitiateMultipartUploadResponse 
        /// which contains an upload ID. This upload ID associates all the
        /// parts in the specific upload. You specify this upload ID in each of 
        /// your subsequent Upload Part requests. You also include
        /// this upload ID in the final request to either complete, or abort
        /// the multipart upload request.
        /// </summary>
        /// <param name="request">
        /// The CopyObjectRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a InitiateMultipartUploadResponse from CS.</returns>
        public InitiateMultipartUploadResponse InitiateMultipartUpload(InitiateMultipartUploadRequest request)
        {
            IAsyncResult asyncResult = invokeInitiateMultipartUpload(request, null, null, true);
            return EndInitiateMultipartUpload(asyncResult);
        }

        IAsyncResult invokeInitiateMultipartUpload(InitiateMultipartUploadRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The InitiateMultipartUploadRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Key Specified is null or empty!");
            }

            ConvertInitiateMultipartUpload(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<InitiateMultipartUploadResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region UploadPart

        const string ORIGINAL_STREAM_PARAM = "ORIGINAL_STREAM_PARAM";
        const string FILE_STREAM_PARAM = "FILE_STREAM_PARAM";

        /// <summary>
        /// Initiates the asynchronous execution of the UploadPart operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.UploadPart"/>
        /// </summary>
        /// <param name="request">The UploadPartRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndUploadPart.</returns>
        public IAsyncResult BeginUploadPart(UploadPartRequest request, AsyncCallback callback, object state)
        {
            return invokeUploadPart(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the UploadPart operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginUploadPart.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a UploadPartResponse from CS.</returns>
        public UploadPartResponse EndUploadPart(IAsyncResult asyncResult)
        {
            CSAsyncResult s3AsyncResult = asyncResult as CSAsyncResult;
            if (s3AsyncResult == null)
                return null;

            Stream orignalStream = s3AsyncResult.Parameters[ORIGINAL_STREAM_PARAM] as Stream;
            Stream fileStream = s3AsyncResult.Parameters[FILE_STREAM_PARAM] as Stream;

            UploadPartRequest request = s3AsyncResult.CSRequest as UploadPartRequest;
            try
            {
                UploadPartResponse response = endOperation<UploadPartResponse>(asyncResult);
                response.PartNumber = request.PartNumber;
                return response;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }

                request.InputStream = orignalStream;
            }
        }

        /// <summary>
        /// This method uploads a part in a multipart upload.  You must initiate a 
        /// multipart upload before you can upload any part.
        /// <para>
        /// Your UploadPart request must include an upload ID and a part number. 
        /// The upload ID is the ID returned by GrandCloud CS in response to your 
        /// Initiate Multipart Upload request. For more information on initiating a
        /// multipart upload. Part number can be any number between 1 and
        /// 10,000, inclusive. A part number uniquely identifies a part and also 
        /// defines its position within the object being uploaded. If you 
        /// upload a new part using the same part number as an existing part, 
        /// that part is overwritten.
        /// </para>
        /// <para>
        /// To ensure data is not corrupted traversing the network, specify the 
        /// Content-MD5 header in the Upload Part request. GrandCloud CS checks 
        /// the part data against the provided MD5 value. If they do not match,
        /// GrandCloud CS returns an error.
        /// </para>
        /// <para>
        /// When you upload a part, the UploadPartResponse response contains an ETag property.
        /// You should record this ETag property value and the part 
        /// number. After uploading all parts, you must send a CompleteMultipartUpload
        /// request. At that time GrandCloud CS constructs a complete object by 
        /// concatenating all the parts you uploaded, in ascending order based on 
        /// the part numbers. The CompleteMultipartUpload request requires you to
        /// send all the part numbers and the corresponding ETag values.
        /// </para>
        /// </summary>
        /// <param name="request">
        /// The UploadPartRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a UploadPartResponse from CS.</returns>
        public UploadPartResponse UploadPart(UploadPartRequest request)
        {
            IAsyncResult asyncResult = invokeUploadPart(request, null, null, true);
            return EndUploadPart(asyncResult);
        }

        IAsyncResult invokeUploadPart(UploadPartRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The UploadPartUpload is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentException("The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentException("The Key Specified is null or empty!");
            }
            if (!request.IsSetUploadId())
            {
                throw new ArgumentException("The UploadId Specified is null or empty!");
            }
            if (!request.IsSetInputStream() && !request.IsSetFilePath())
            {
                throw new ArgumentException("Either InputStream or FilePath must be set.");
            }
            if (request.IsSetInputStream() && request.IsSetFilePath())
            {
                throw new ArgumentException("Both InputStream and FilePath can not be set.");
            }
            if (request.IsSetFilePath() && !request.IsSetFilePosition())
            {
                throw new ArgumentException("FilePosition is not set which is required when using FilePath.");
            }

            Stream fileStream = null;
            Stream orignalStream = request.InputStream;
            try
            {
                if (request.IsSetInputStream())
                {
                    request.InputStream = new PartStreamWrapper(request.InputStream, request.PartSize);
                }
                else
                {
                    fileStream = File.OpenRead(request.FilePath);
                    fileStream.Position = request.FilePosition;
                    request.InputStream = new PartStreamWrapper(fileStream, request.PartSize);
                }

                ConvertUploadPart(request);
                CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
                asyncResult.Parameters[ORIGINAL_STREAM_PARAM] = orignalStream;
                asyncResult.Parameters[FILE_STREAM_PARAM] = fileStream;
                invoke<UploadPartResponse>(asyncResult);
                return asyncResult;
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                request.InputStream = orignalStream;
                throw;
            }
        }


        #endregion

        #region ListParts

        /// <summary>
        /// Initiates the asynchronous execution of the ListParts operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.ListParts"/>
        /// </summary>
        /// <param name="request">The ListPartsRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndListParts.</returns>
        public IAsyncResult BeginListParts(ListPartsRequest request, AsyncCallback callback, object state)
        {
            return invokeListParts(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the ListParts operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListParts.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListPartsResponse from CS.</returns>
        public ListPartsResponse EndListParts(IAsyncResult asyncResult)
        {
            return endOperation<ListPartsResponse>(asyncResult);
        }

        /// <summary>
        /// This method lists the parts that have been uploaded 
        /// for a particular multipart upload.
        /// <para>
        /// This method must include the upload ID, returned by 
        /// the InitiateMultipartUpload request. This request 
        /// returns a maximum of 1000 uploaded parts by default. You can
        /// restrict the number of parts returned by specifying the 
        /// MaxParts property on the ListPartsRequest. If your multipart
        /// upload consists of more parts than allowed in the 
        /// ListParts response, the response returns a IsTruncated
        /// field with value true, and a NextPartNumberMarker property. 
        /// In subsequent ListParts request you can include the 
        /// PartNumberMarker property and set its value to the
        /// NextPartNumberMarker property value from the previous response.
        /// </para>
        /// </summary>
        /// <param name="request">
        /// The ListPartsRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a ListPartsResponse from CS.</returns>
        public ListPartsResponse ListParts(ListPartsRequest request)
        {
            IAsyncResult asyncResult = invokeListParts(request, null, null, true);
            return EndListParts(asyncResult);
        }

        IAsyncResult invokeListParts(ListPartsRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The ListPartsRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Key Specified is null or empty!");
            }
            if (!request.IsSetUploadId())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The UploadId Specified is null or empty!");
            }

            ConvertListParts(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<ListPartsResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region AbortMultipartUpload

        /// <summary>
        /// Initiates the asynchronous execution of the AbortMultipartUpload operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.AbortMultipartUpload"/>
        /// </summary>
        /// <param name="request">The AbortMultipartUploadRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndAbortMultipartUpload.</returns>
        public IAsyncResult BeginAbortMultipartUpload(AbortMultipartUploadRequest request, AsyncCallback callback, object state)
        {
            return invokeAbortMultipartUpload(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the AbortMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginAbortMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a AbortMultipartUploadResponse from CS.</returns>
        public AbortMultipartUploadResponse EndAbortMultipartUpload(IAsyncResult asyncResult)
        {
            return endOperation<AbortMultipartUploadResponse>(asyncResult);
        }

        /// <summary>
        /// This method aborts a multipart upload. After a multipart upload is 
        /// aborted, no additional parts can be uploaded using that upload ID. 
        /// The storage consumed by any previously uploaded parts will be freed.
        /// However, if any part uploads are currently in progress, those part 
        /// uploads may or may not succeed. As a result, it may be necessary to 
        /// abort a given multipart upload multiple times in order to completely free
        /// all storage consumed by all parts.
        /// </summary>
        /// <param name="request">
        /// The AbortMultipartUploadRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a AbortMultipartUploadResponse from CS.</returns>
        public AbortMultipartUploadResponse AbortMultipartUpload(AbortMultipartUploadRequest request)
        {
            IAsyncResult asyncResult = invokeAbortMultipartUpload(request, null, null, true);
            return EndAbortMultipartUpload(asyncResult);
        }

        IAsyncResult invokeAbortMultipartUpload(AbortMultipartUploadRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The AbortMultipartUploadRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The Key Specified is null or empty!");
            }
            if (!request.IsSetUploadId())
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The UploadId Specified is null or empty!");
            }

            ConvertAbortMultipartUpload(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<AbortMultipartUploadResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region CompleteMultipartUpload

        /// <summary>
        /// Initiates the asynchronous execution of the CompleteMultipartUpload operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.CompleteMultipartUpload"/>
        /// </summary>
        /// <param name="request">The CompleteMultipartUploadRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndCompleteMultipartUpload.</returns>
        public IAsyncResult BeginCompleteMultipartUpload(CompleteMultipartUploadRequest request, AsyncCallback callback, object state)
        {
            return invokeCompleteMultipartUpload(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the CompleteMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCompleteMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a CompleteMultipartUploadResponse from CS.</returns>
        public CompleteMultipartUploadResponse EndCompleteMultipartUpload(IAsyncResult asyncResult)
        {
            return endOperation<CompleteMultipartUploadResponse>(asyncResult);
        }

        /// <summary>
        /// This operation completes a multipart upload by assembling 
        /// previously uploaded parts.
        /// <para>
        /// You first upload all parts using the UploadPart method. 
        /// After successfully uploading all relevant parts of an upload, 
        /// you call this operation to complete the upload. Upon receiving
        /// this request, GrandCloud CS concatenates all the parts in ascending 
        /// order by part number to create a new object. In the 
        /// CompleteMultipartUpload request, you must provide the 
        /// parts list. For each part in the list, you provide the 
        /// part number and the ETag header value, returned after that 
        /// part was uploaded.
        /// </para>
        /// <para>
        /// Processing of a CompleteMultipartUpload request may take 
        /// several minutes to complete.
        /// </para>
        /// </summary>
        /// <param name="request">
        /// The CompleteMultipartUploadRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a CompleteMultipartUploadResponse from CS.</returns>
        public CompleteMultipartUploadResponse CompleteMultipartUpload(CompleteMultipartUploadRequest request)
        {
            IAsyncResult asyncResult = invokeCompleteMultipartUpload(request, null, null, true);
            return EndCompleteMultipartUpload(asyncResult);
        }

        IAsyncResult invokeCompleteMultipartUpload(CompleteMultipartUploadRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The CompleteMultipartUploadRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentException("The CS BucketName specified is null or empty!");
            }
            if (!request.IsSetKey())
            {
                throw new ArgumentException("The Key Specified is null or empty!");
            }
            if (!request.IsSetUploadId())
            {
                throw new ArgumentException("The UploadId Specified is null or empty!");
            }
            if (request.PartETags.Count == 0)
            {
                throw new ArgumentException("No part etags were added to the request!");
            }

            ConvertCompleteMultipartUpload(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<CompleteMultipartUploadResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region ListMultipartUploads

        /// <summary>
        /// Initiates the asynchronous execution of the ListMultipartUploads operation. 
        /// <seealso cref="M:GrandCloud.CS.GrandCloudCS.ListMultipartUploads"/>
        /// </summary>
        /// <param name="request">The ListMultipartUploadsRequest that defines the parameters of
        /// the operation.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; 
        /// this value is also needed when invoking EndListMultipartUploads.</returns>
        public IAsyncResult BeginListMultipartUploads(ListMultipartUploadsRequest request, AsyncCallback callback, object state)
        {
            return invokeListMultipartUploads(request, callback, state, false);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the ListMultipartUploads operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListMultipartUploads.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListMultipartUploadsResponse from CS.</returns>
        public ListMultipartUploadsResponse EndListMultipartUploads(IAsyncResult asyncResult)
        {
            return endOperation<ListMultipartUploadsResponse>(asyncResult);
        }

        /// <summary>
        /// This operation lists in-progress multipart uploads. An in-progress 
        /// multipart upload is a multipart upload that has been initiated, 
        /// using the InitiateMultipartUpload request, but has not yet been 
        /// completed or aborted.
        /// <para>
        /// This operation returns at most 1,000 multipart uploads in the 
        /// response by default. The number of multipart uploads can be further 
        /// limited using the MaxUploads property on the request parameter. If there are 
        /// additional multipart uploads that satisfy the list criteria, the 
        /// response will contain an IsTruncated property with the value set to true.
        /// To list the additional multipart uploads use the KeyMarker and 
        /// UploadIdMarker properties on the request parameters.
        /// </para>
        /// </summary>
        /// <param name="request">
        /// The ListMultipartUploadsRequest that defines the parameters of the operation.
        /// </param>
        /// <returns>Returns a ListMultipartUploadsResponse from CS.</returns>
        public ListMultipartUploadsResponse ListMultipartUploads(ListMultipartUploadsRequest request)
        {
            IAsyncResult asyncResult = invokeListMultipartUploads(request, null, null, true);
            return EndListMultipartUploads(asyncResult);
        }

        IAsyncResult invokeListMultipartUploads(ListMultipartUploadsRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(CSConstants.RequestParam, "The ListMultipartUploadsRequest is null!");
            }
            if (!request.IsSetBucketName())
            {
                throw new ArgumentException("The CS BucketName specified is null or empty!");
            }

            ConvertListMultipartUploads(request);
            CSAsyncResult asyncResult = new CSAsyncResult(request, state, callback, synchronized);
            invoke<ListMultipartUploadsResponse>(asyncResult);
            return asyncResult;
        }

        #endregion

        #region Private ConvertXXX Methods

        /**
        * Convert ListBucketsRequest to key/value pairs
        */
        private void ConvertListBuckets(ListBucketsRequest request)
        {
            Map parameters = request.parameters;

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "ListAllBuckets";
            request.RequestDestinationBucket = null;
        }

        /**
         * Convert GetBucketLocationRequest to key/value pairs.
         */
        private void ConvertGetBucketLocation(GetBucketLocationRequest request)
        {
            Map parameters = request.parameters;

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "GetBucketLocation";
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = "?location";

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert ListObjectsRequest to key/value pairs
         */
        private void ConvertListObjects(ListObjectsRequest request)
        {
            Map parameters = request.parameters;

            //Create query string if any of the values are set.
            StringBuilder sb = new StringBuilder("?", 256);
            if (request.IsSetPrefix())
            {
                sb.Append(String.Concat("prefix=", GrandCloudCSUtil.UrlEncode(request.Prefix, false), "&"));
            }
            if (request.IsSetMarker())
            {
                sb.Append(String.Concat("marker=", GrandCloudCSUtil.UrlEncode(request.Marker, false), "&"));
            }
            if (request.IsSetDelimiter())
            {
                sb.Append(String.Concat("delimiter=", GrandCloudCSUtil.UrlEncode(request.Delimiter, false), "&"));
            }
            if (request.IsSetMaxKeys())
            {
                sb.Append(String.Concat("max-keys=", request.MaxKeys, "&"));
            }

            string query = sb.ToString();

            // Remove trailing & character
            if (query.EndsWith("&"))
            {
                query = query.Remove(query.Length - 1);
            }

            // We initialized the query with a "?". If none of
            // Prefix, Marker, Delimiter, MaxKeys is set, there
            // is no query
            if (query.Length > 1)
            {
                parameters[CSQueryParameter.Query] = query;
            }

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "ListObjects";
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Converts the PutBucketRequest to key/value pairs
         */
        private void ConvertPutBucket(PutBucketRequest request)
        {
            Map parameters = request.parameters;

            parameters[CSQueryParameter.Verb] = CSConstants.PutVerb;
            parameters[CSQueryParameter.Action] = "CreateBucket";
            //TODO: check this part code
            string regionCode = "";
            if (!string.IsNullOrEmpty(request.BucketRegionName))
            {
                if (string.Equals(request.BucketRegionName, CSConstants.REGION_HUABEI_1) ||
                    string.Equals(request.BucketRegionName, CSConstants.REGION_HUADONG_1))
                {
                    regionCode = request.BucketRegionName;
                }
            }
            else
            {
                regionCode = CSConstants.LocationConstraints[(int)request.BucketRegion];
            }

            if (!string.IsNullOrEmpty(regionCode))
            {
                string content = String.Format(
                    "<CreateBucketConfiguration><LocationConstraint>{0}</LocationConstraint></CreateBucketConfiguration>",
                    regionCode
                    );
                parameters[CSQueryParameter.ContentBody] = content;                
                parameters[CSQueryParameter.ContentType] = GrandCloudCSUtil.MimeTypeFromExtension(".xml");
            }

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert DeleteBucketRequest to key/value pairs
         */
        private void ConvertDeleteBucket(DeleteBucketRequest request)
        {
            Map parameters = request.parameters;

            parameters[CSQueryParameter.Verb] = CSConstants.DeleteVerb;
            parameters[CSQueryParameter.Action] = "DeleteBucket";
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert GetBucketPolicyRequest to key/value pairs.
         */
        private void ConvertGetBucketPolicy(GetBucketPolicyRequest request)
        {
            Map parameters = request.parameters;
            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "GetBucketPolicy";
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = "?policy";

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert PutBucketPolicyRequest to key/value pairs.
         */
        private void ConvertPutBucketPolicy(PutBucketPolicyRequest request)
        {
            Map parameters = request.parameters;
            parameters[CSQueryParameter.Verb] = CSConstants.PutVerb;
            parameters[CSQueryParameter.Action] = "SetBucketPolicy";
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = "?policy";

            parameters[CSQueryParameter.ContentBody] = request.Policy;
            parameters[CSQueryParameter.ContentType] = GrandCloudCSUtil.MimeTypeFromExtension(".txt");

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert DeleteBucketPolicyRequest to key/value pairs.
         */
        private void ConvertDeleteBucketPolicy(DeleteBucketPolicyRequest request)
        {
            Map parameters = request.parameters;
            parameters[CSQueryParameter.Verb] = CSConstants.DeleteVerb;
            parameters[CSQueryParameter.Action] = "DeleteBucketPolicy";
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = "?policy";

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
          * Convert GetObjectRequest to key/value pairs.
          */
        private void ConvertGetObject(GetObjectRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "GetObject";
            parameters[CSQueryParameter.Key] = request.Key;

            if (request.IsSetByteRange())
            {
                parameters[CSQueryParameter.Range] = String.Concat(
                    request.ByteRangeLong.First,
                    ":",
                    request.ByteRangeLong.Second
                    );
            }

            // Add the necessary get object specific headers to the request.Headers object
            if (request.IsSetETagToMatch())
            {
                setIfMatchHeader(webHeaders, request.ETagToMatch);
            }
            if (request.IsSetETagToNotMatch())
            {
                setIfNoneMatchHeader(webHeaders, request.ETagToNotMatch);
            }
            if (request.IsSetModifiedSinceDate())
            {
                setIfModifiedSinceHeader(webHeaders, request.ModifiedSinceDate);
            }
            if (request.IsSetUnmodifiedSinceDate())
            {
                setIfUnmodifiedSinceHeader(webHeaders, request.UnmodifiedSinceDate);
            }

            StringBuilder queryStr = new StringBuilder();

            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_CACHE_CONTROL, request.ResponseHeaderOverrides.CacheControl);
            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_CONTENT_DISPOSITION, request.ResponseHeaderOverrides.ContentDisposition);
            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_CONTENT_ENCODING, request.ResponseHeaderOverrides.ContentEncoding);
            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_CONTENT_LANGUAGE, request.ResponseHeaderOverrides.ContentLanguage);
            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_CONTENT_TYPE, request.ResponseHeaderOverrides.ContentType);
            addParameter(queryStr, ResponseHeaderOverrides.RESPONSE_EXPIRES, request.ResponseHeaderOverrides.Expires);


            if (queryStr.Length > 0)
            {
                parameters[CSQueryParameter.Query] = "?" + queryStr.ToString();
                parameters[CSQueryParameter.QueryToSign] = parameters[CSQueryParameter.Query];
            }

            // Add the Timeout parameter
            parameters[CSQueryParameter.RequestTimeout] = request.Timeout.ToString();
            parameters[CSQueryParameter.RequestReadWriteTimeout] = request.ReadWriteTimeout.ToString();

            request.RequestDestinationBucket = request.BucketName;
        }

        void addParameter(StringBuilder queryStr, string key, string value)
        {
            addParameter(queryStr, null, key, value);
        }
        void addParameter(StringBuilder queryStr, StringBuilder encodedQueryStr, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (queryStr != null && queryStr.Length > 0)
                    queryStr.Append("&");
                if (encodedQueryStr != null && encodedQueryStr.Length > 0)
                    encodedQueryStr.Append("&");

                if (queryStr != null)
                    queryStr.AppendFormat("{0}={1}", key, value);
                if (encodedQueryStr != null)
                    encodedQueryStr.AppendFormat("{0}={1}", key, GrandCloudCSUtil.UrlEncode(value, true));
            }
        }

        /**
         * Convert GetObjectMetadataRequest to key/value pairs.
         */
        private void ConvertGetObjectMetadata(GetObjectMetadataRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.HeadVerb;
            parameters[CSQueryParameter.Action] = "HeadObject";
            parameters[CSQueryParameter.Key] = request.Key;

            if (request.IsSetETagToNotMatch())
            {
                setIfNoneMatchHeader(webHeaders, request.ETagToNotMatch);
            }
            if (request.IsSetModifiedSinceDate())
            {
                setIfModifiedSinceHeader(webHeaders, request.ModifiedSinceDate);
            }
            if (request.IsSetUnmodifiedSinceDate())
            {
                setIfUnmodifiedSinceHeader(webHeaders, request.UnmodifiedSinceDate);
            }

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert PutObjectRequest to key/value pairs.
         */
        protected internal void ConvertPutObject(PutObjectRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.PutVerb;
            parameters[CSQueryParameter.Action] = "PutObject";
            parameters[CSQueryParameter.Key] = request.Key;
            if (request.IsSetMD5Digest())
            {
                webHeaders[CSSDKUtils.ContentMD5Header] = request.MD5Digest;
            }
            else if (request.GenerateMD5Digest)
            {
                string checksum = null;
                if (request.IsSetInputStream())
                {
                    checksum = GrandCloudCSUtil.GenerateChecksumForStream(request.InputStream, true);
                }
                else
                {
                    // If there isn't an input stream, there has to be a content body.
                    checksum = GrandCloudCSUtil.GenerateChecksumForContent(request.ContentBody, true);
                }

                webHeaders[CSSDKUtils.ContentMD5Header] = checksum;
            }

            // Add the Content Type
            if (request.IsSetContentType())
            {
                parameters[CSQueryParameter.ContentType] = request.ContentType;
            }
            else if (request.IsSetFilePath() ||
                request.IsSetKey())
            {
                // Get the extension of the file from the path.
                // Try the key as well.
                string ext = Path.GetExtension(request.FilePath);
                if (String.IsNullOrEmpty(ext) &&
                    request.IsSetKey())
                {
                    ext = Path.GetExtension(request.Key);
                }
                // Use the extension to get the mime-type
                if (!String.IsNullOrEmpty(ext))
                {
                    parameters[CSQueryParameter.ContentType] = GrandCloudCSUtil.MimeTypeFromExtension(ext);
                }
            }

            // Set the Content Length based on whether there is a stream
            if (request.IsSetInputStream())
            {
                parameters[CSQueryParameter.ContentLength] = request.InputStream.Length.ToString();
            }

            if (request.IsSetContentBody())
            {
                // The content length is determined based on the number of bytes
                // needed to represent the content string - check invoke<T>
                parameters[CSQueryParameter.ContentBody] = request.ContentBody;
                // Since a content body was set, let's determine whether a content type was set
                if (!parameters.ContainsKey(CSQueryParameter.ContentType))
                {
                    parameters[CSQueryParameter.ContentType] = CSSDKUtils.UrlEncodedContent;
                }
            }

            // Add the Timeout parameter
            parameters[CSQueryParameter.RequestTimeout] = request.Timeout.ToString();
            parameters[CSQueryParameter.RequestReadWriteTimeout] = request.ReadWriteTimeout.ToString();

            // Add the Put Object specific headers to the request
            // 2. The MetaData
            if (request.IsSetMetaData())
            {
                // Add headers of type x-amz-meta-<key> to the request
                foreach (string key in request.metaData)
                {
                    string prefixedKey;
                    if (!key.StartsWith(CSConstants.MetaHeaderPrefix))
                    {
                        prefixedKey = String.Concat(CSConstants.MetaHeaderPrefix, key);
                    }
                    else
                    {
                        prefixedKey = key;
                    }

                    webHeaders[prefixedKey] = request.metaData[key];
                }
            }

            // Add the storage class header
            webHeaders[CSConstants.StorageClassHeader] = CSConstants.StorageClasses[(int)request.StorageClass];

            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert GetPreSignedUrlRequest to key/value pairs.
         */
        private void ConvertGetPreSignedUrl(GetPreSignedUrlRequest request)
        {
            using (ImmutableCredentials immutableCredentials = credentials.GetCredentials())
            {
                Map parameters = request.parameters;

                parameters[CSQueryParameter.Verb] = CSConstants.Verbs[(int)request.Verb];
                parameters[CSQueryParameter.Action] = "GetPreSignedUrl";
                StringBuilder queryStr = new StringBuilder("?", 512);                

                queryStr.Append("SNDAAccessKeyId=" + immutableCredentials.AccessKey);
                if (request.IsSetKey())
                {
                    parameters[CSQueryParameter.Key] = request.Key;
                }

                if (request.IsSetContentType())
                {
                    parameters[CSQueryParameter.ContentType] = request.ContentType;
                }

                if (queryStr.Length != 0)
                {
                    queryStr.Append("&");
                }
                queryStr.Append("Expires=");

                string value = Convert.ToInt64((request.Expires.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds).ToString();
                queryStr.Append(value);
                parameters[CSQueryParameter.Expires] = value;


                StringBuilder encodedQueryStrToSign = new StringBuilder();
                StringBuilder queryStrToSign = new StringBuilder();

                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_CACHE_CONTROL, request.ResponseHeaderOverrides.CacheControl);
                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_CONTENT_DISPOSITION, request.ResponseHeaderOverrides.ContentDisposition);
                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_CONTENT_ENCODING, request.ResponseHeaderOverrides.ContentEncoding);
                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_CONTENT_LANGUAGE, request.ResponseHeaderOverrides.ContentLanguage);
                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_CONTENT_TYPE, request.ResponseHeaderOverrides.ContentType);
                addParameter(queryStrToSign, encodedQueryStrToSign, ResponseHeaderOverrides.RESPONSE_EXPIRES, request.ResponseHeaderOverrides.Expires);

                if (queryStrToSign.Length > 0)
                {
                    parameters[CSQueryParameter.QueryToSign] = "?" + queryStrToSign.ToString();
                    queryStr.Append("&" + encodedQueryStrToSign.ToString());
                }

                parameters[CSQueryParameter.Query] = queryStr.ToString();
                request.RequestDestinationBucket = request.BucketName;
                addCSQueryParameters(request, immutableCredentials);

                // the url needs to be modified so that:
                // 1. The right http protocol is used
                // 2. The auth string is added to the url
                string url = request.parameters[CSQueryParameter.Url];

                // the url's protocol prefix is generated using the config's
                // CommunicationProtocol property. If the request's
                // protocol differs from that set in the config, make the
                // necessary string replacements.
                if (request.Protocol != config.CommunicationProtocol)
                {
                    switch (config.CommunicationProtocol)
                    {
                        case Protocol.HTTP:
                            url = url.Replace("http://", "https://");
                            break;
                        case Protocol.HTTPS:
                            url = url.Replace("https://", "http://");
                            break;
                    }
                }
               

                //sign the request
                string toSign = buildSigningString(parameters, request.Headers);
                string auth;
                if (immutableCredentials.UseSecureStringForSecretKey)
                {
                    KeyedHashAlgorithm algorithm = new HMACSHA1();
                    auth = CSSDKUtils.HMACSign(toSign, immutableCredentials.SecureSecretKey, algorithm);
                }
                else
                {
                    KeyedHashAlgorithm algorithm = new HMACSHA1();
                    auth = CSSDKUtils.HMACSign(toSign, immutableCredentials.ClearSecretKey, algorithm);
                }
                parameters[CSQueryParameter.Authorization] = auth;

                parameters[CSQueryParameter.Url] = String.Concat(
                    url,
                    "&Signature=",
                    GrandCloudCSUtil.UrlEncode(request.parameters[CSQueryParameter.Authorization], false)
                    );
            }
        }

        /**
         * Convert DeleteObjectRequest to key/value pairs.
         */
        private void ConvertDeleteObject(DeleteObjectRequest request)
        {
            Map parameters = request.parameters;

            parameters[CSQueryParameter.Verb] = CSConstants.DeleteVerb;
            parameters[CSQueryParameter.Action] = "DeleteObject";
            parameters[CSQueryParameter.Key] = request.Key;            

            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert InitiateMultipartUpload to key/value pairs.
         */
        private void ConvertInitiateMultipartUpload(InitiateMultipartUploadRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.PostVerb;
            parameters[CSQueryParameter.Action] = "InitiateMultipartUpload";
            parameters[CSQueryParameter.Key] = request.Key;
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = "?uploads";

            // Add the Content Type
            if (request.IsSetContentType())
            {
                parameters[CSQueryParameter.ContentType] = request.ContentType;
            }
            else if (request.IsSetKey())
            {
                // Get the extension of the file from the path.
                // Try the key as well.
                string ext = Path.GetExtension(request.Key);
                // Use the extension to get the mime-type
                if (!String.IsNullOrEmpty(ext))
                {
                    parameters[CSQueryParameter.ContentType] = GrandCloudCSUtil.MimeTypeFromExtension(ext);
                }
            }

            // Add the Put Object specific headers to the request
            if (request.IsSetMetaData())
            {
                // Add headers of type x-snda-meta-<key> to the request
                foreach (string key in request.metaData)
                {
                    if (!key.StartsWith(CSConstants.MetaHeaderPrefix))
                    {
                        webHeaders[String.Concat(CSConstants.MetaHeaderPrefix, key)] = request.metaData[key];
                    }
                    else
                    {
                        webHeaders[key] = request.metaData[key];
                    }
                }
            }
     
            // Add the storage class header
            webHeaders[CSConstants.StorageClassHeader] = CSConstants.StorageClasses[(int)request.StorageClass];

            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert AbortMultipartUploadRequest for enable logging, to key/value pairs.
         */
        private void ConvertAbortMultipartUpload(AbortMultipartUploadRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.DeleteVerb;
            parameters[CSQueryParameter.Action] = "AbortMultipartUpload";
            parameters[CSQueryParameter.Key] = request.Key;
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = string.Format("?uploadId={0}", request.UploadId);


            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert ListMultipartUploadsRequest for enable logging, to key/value pairs.
         */
        private void ConvertListMultipartUploads(ListMultipartUploadsRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "ListMultipartUploads";
            parameters[CSQueryParameter.QueryToSign] = "?uploads";

            //Create query string if any of the values are set.
            StringBuilder sb = new StringBuilder("?uploads&", 256);
            if (request.IsSetMaxUploads())
            {
                sb.Append(String.Concat("max-uploads=", request.MaxUploads.ToString(), "&"));
            }
            if (request.IsSetKeyMarker())
            {
                sb.Append(String.Concat("key-marker=", GrandCloudCSUtil.UrlEncode(request.KeyMarker, false), "&"));
            }
            if (request.IsSetUploadIdMarker())
            {
                sb.Append(String.Concat("upload-idmarker=", GrandCloudCSUtil.UrlEncode(request.UploadIdMarker, false), "&"));
            }
            if (request.IsSetPrefix())
            {
                sb.Append(String.Concat("prefix=", GrandCloudCSUtil.UrlEncode(request.Prefix, false), "&"));
            }
            if (request.IsSetDelimiter())
            {
                sb.Append(String.Concat("delimiter=", GrandCloudCSUtil.UrlEncode(request.Delimiter, false), "&"));
            }

            string query = sb.ToString();

            // Remove trailing & character
            if (query.EndsWith("&"))
            {
                query = query.Remove(query.Length - 1);
            }

            parameters[CSQueryParameter.Query] = query;

            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert ListPartsRequest for enable logging, to key/value pairs.
         */
        private void ConvertListParts(ListPartsRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.GetVerb;
            parameters[CSQueryParameter.Action] = "ListParts";
            parameters[CSQueryParameter.Key] = request.Key;
            parameters[CSQueryParameter.QueryToSign] = string.Format("?uploadId={0}", request.UploadId);

            //Create query string if any of the values are set.
            StringBuilder sb = new StringBuilder(string.Format("?uploadId={0}&", request.UploadId), 256);
            if (request.IsSetMaxParts())
            {
                sb.Append(String.Concat("max-parts=", request.MaxParts.ToString(), "&"));
            }
            if (request.IsSetPartNumberMarker())
            {
                sb.Append(String.Concat("part-number-marker=", GrandCloudCSUtil.UrlEncode(request.PartNumberMarker, false), "&"));
            }

            string query = sb.ToString();

            // Remove trailing & character
            if (query.EndsWith("&"))
            {
                query = query.Remove(query.Length - 1);
            }

            parameters[CSQueryParameter.Query] = query;

            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert CompleteMultipartUploadRequest for enable logging, to key/value pairs.
         */
        private void ConvertCompleteMultipartUpload(CompleteMultipartUploadRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.PostVerb;
            parameters[CSQueryParameter.Action] = "CompleteMultipartUpload";
            parameters[CSQueryParameter.Key] = request.Key;
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = string.Format("?uploadId={0}", request.UploadId);
            parameters[CSQueryParameter.ContentBody] = request.ContentXML;
            parameters[CSQueryParameter.ContentType] = "application/xml";


            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        /**
         * Convert SetBucketLoggingRequest for enable logging, to key/value pairs.
         */
        private void ConvertUploadPart(UploadPartRequest request)
        {
            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[CSQueryParameter.Verb] = CSConstants.PutVerb;
            parameters[CSQueryParameter.Action] = "UploadPart";
            parameters[CSQueryParameter.Key] = request.Key;
            parameters[CSQueryParameter.Query] = parameters[CSQueryParameter.QueryToSign] = string.Format("?partNumber={1}&uploadId={0}", request.UploadId, request.PartNumber);

            if (request.IsSetMD5Digest())
            {
                webHeaders[CSSDKUtils.ContentMD5Header] = request.MD5Digest;
            }
            else if (request.GenerateMD5Digest)
            {
                string checksum = null;
                if (request.IsSetInputStream())
                {
                    checksum = GrandCloudCSUtil.GenerateChecksumForStream(request.InputStream, true);
                }

                webHeaders[CSSDKUtils.ContentMD5Header] = checksum;
            }

            // InputStream is a PartStreamWrapper that will take care of computing the length for the part.
            parameters[CSQueryParameter.ContentLength] = request.InputStream.Length.ToString();


            // Add the Timeout parameter
            parameters[CSQueryParameter.RequestTimeout] = request.Timeout.ToString();
            parameters[CSQueryParameter.RequestReadWriteTimeout] = request.ReadWriteTimeout.ToString();


            // Finally, add the CS specific parameters and headers
            request.RequestDestinationBucket = request.BucketName;
        }

        #endregion

        #region Private Methods

        T endOperation<T>(IAsyncResult result) where T : class
        {
            CSAsyncResult csAsyncResult = result as CSAsyncResult;
            if (csAsyncResult == null)
                return default(T);

            if (!csAsyncResult.IsCompleted)
            {
                csAsyncResult.AsyncWaitHandle.WaitOne();
            }

            if (csAsyncResult.Exception != null)
            {
                CSSDKUtils.PreserveStackTrace(csAsyncResult.Exception);
                throw csAsyncResult.Exception;
            }

            T response = csAsyncResult.FinalResponse as T;
            //csAsyncResult.FinalResponse = null;
            return response;
        }

        void invoke<T>(CSAsyncResult csAsyncResult) where T : CSResponse, new()
        {
            invoke<T>(csAsyncResult, false);
        }

        void invoke<T>(CSAsyncResult csAsyncResult, bool isRedirect) where T : CSResponse, new()
        {
            if (csAsyncResult.CSRequest == null)
            {
                throw new GrandCloudCSException("No request specified for the CS operation!");
            }

            string userAgent = config.UserAgent + " " + (csAsyncResult.CompletedSynchronously ? "CSSync" : "CSAsync");
            csAsyncResult.CSRequest.Headers[CSSDKUtils.UserAgentHeader] = userAgent;            

            ImmutableCredentials immutableCredentials = credentials == null ? null : credentials.GetCredentials();
            try
            {
                if (!isRedirect)
                {
                    addCSQueryParameters(csAsyncResult.CSRequest, immutableCredentials);
                }

                WebHeaderCollection headers = csAsyncResult.CSRequest.Headers;
                Map parameters = csAsyncResult.CSRequest.parameters;
                Stream fStream = csAsyncResult.CSRequest.InputStream;

                // if credentials are present (non-anonymous) sign the request
                if (immutableCredentials != null)
                {
                    string toSign = buildSigningString(parameters, headers);
                    string auth;
                    if (immutableCredentials.UseSecureStringForSecretKey)
                    {
                        KeyedHashAlgorithm algorithm = new HMACSHA1();
                        auth = CSSDKUtils.HMACSign(toSign, immutableCredentials.SecureSecretKey, algorithm);
                    }
                    else
                    {
                        KeyedHashAlgorithm algorithm = new HMACSHA1();
                        auth = CSSDKUtils.HMACSign(toSign, immutableCredentials.ClearSecretKey, algorithm);
                    }
                    parameters[CSQueryParameter.Authorization] = auth;
                }

                string actionName = parameters[CSQueryParameter.Action];
                string verb = parameters[CSQueryParameter.Verb];

                if (LOGGER.IsDebugEnabled)
                    LOGGER.DebugFormat("Starting request (id {0}) for {0}", csAsyncResult.CSRequest.Id, actionName);

                // Variables that pertain to PUT requests
                byte[] requestData = Encoding.UTF8.GetBytes("");
                long reqDataLen = 0;

                validateVerb(verb);

                if (verb.Equals(CSConstants.PutVerb) || verb.Equals(CSConstants.PostVerb))
                {
                    if (parameters.ContainsKey(CSQueryParameter.ContentBody))
                    {
                        string reqBody = parameters[CSQueryParameter.ContentBody];
                        csAsyncResult.CSRequest.BytesProcessed = reqBody.Length;
                        if (LOGGER.IsDebugEnabled)
                            LOGGER.DebugFormat("Request (id {0}) body's length [{1}]", csAsyncResult.CSRequest.Id, reqBody.Length);
                        requestData = Encoding.UTF8.GetBytes(reqBody);

                        // Since there is a request body, determine the length of the
                        // data that will be sent to the server.
                        reqDataLen = requestData.Length;
                        parameters[CSQueryParameter.ContentLength] = reqDataLen.ToString();
                    }

                    if (parameters.ContainsKey(CSQueryParameter.ContentLength))
                    {
                        reqDataLen = Int64.Parse(parameters[CSQueryParameter.ContentLength]);
                    }
                }

                int maxRetries = config.IsSetMaxErrorRetry() ? config.MaxErrorRetry : CSSDKUtils.DefaultMaxRetry;

                if (fStream != null)
                {
                    csAsyncResult.OrignalStreamPosition = fStream.Position;
                }

                HttpWebRequest request = configureWebRequest(csAsyncResult.CSRequest, reqDataLen, immutableCredentials);

                parameters[CSQueryParameter.RequestAddress] = request.Address.ToString();

                try
                {
                    csAsyncResult.RequestState = new RequestState(request, parameters, fStream, requestData, reqDataLen, csAsyncResult.CSRequest.StopWatch.ElapsedTicks);
                    if (reqDataLen > 0)
                    {
                        if (csAsyncResult.CompletedSynchronously)
                        {
                            this.getRequestStreamCallback<T>(csAsyncResult);
                        }
                        else
                        {
                            IAsyncResult httpResult = request.BeginGetRequestStream(new AsyncCallback(this.getRequestStreamCallback<T>), csAsyncResult);
                            if (httpResult.CompletedSynchronously)
                            {
                                if (!csAsyncResult.RequestState.GetRequestStreamCallbackCalled)
                                {
                                    getRequestStreamCallback<T>(httpResult);
                                }
                                csAsyncResult.SetCompletedSynchronously(true);
                            }
                        }
                    }
                    else
                    {
                        if (csAsyncResult.CompletedSynchronously)
                        {
                            this.getResponseCallback<T>(csAsyncResult);
                        }
                        else
                        {
                            IAsyncResult httpResult = request.BeginGetResponse(new AsyncCallback(this.getResponseCallback<T>), csAsyncResult);
                            if (httpResult.CompletedSynchronously)
                            {
                                if (!csAsyncResult.RequestState.GetResponseCallbackCalled)
                                {
                                    getResponseCallback<T>(httpResult);
                                }
                                csAsyncResult.SetCompletedSynchronously(true);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LOGGER.Error("Error starting async http operation", e);
                    throw;
                }
            }
            finally
            {
                if (immutableCredentials != null)
                {
                    immutableCredentials.Dispose();
                }
            }
        }

        void validateVerb(string verb)
        {
            // The HTTP operation specified has to be one of the operations
            // the GrandCloud CS service explicitly supports
            if (!(verb.Equals(CSConstants.PutVerb) ||
                verb.Equals(CSConstants.GetVerb) ||
                verb.Equals(CSConstants.DeleteVerb) ||
                verb.Equals(CSConstants.HeadVerb) ||
                verb.Equals(CSConstants.PostVerb)))
            {
                throw new GrandCloudCSException("Invalid HTTP Operation attempted! Supported operations - GET, HEAD, PUT, DELETE, POST");
            }
        }

        void getRequestStreamCallback<T>(IAsyncResult result) where T : CSResponse, new()
        {
            CSAsyncResult csAsyncResult;
            if (result is CSAsyncResult)
                csAsyncResult = result as CSAsyncResult;
            else
                csAsyncResult = result.AsyncState as CSAsyncResult;

            csAsyncResult.RequestState.GetRequestStreamCallbackCalled = true;
            try
            {
                RequestState state = csAsyncResult.RequestState;
                bool shouldRetry = false;
                try
                {
                    Stream requestStream;
                    if (csAsyncResult.CompletedSynchronously)
                        requestStream = state.WebRequest.GetRequestStream();
                    else
                        requestStream = state.WebRequest.EndGetRequestStream(result);

                    using (requestStream)
                    {
                        Stream stream = state.InputStream != null ? state.InputStream : new MemoryStream(state.RequestData);
                        writeStreamToService(csAsyncResult.CSRequest, state.RequestDataLength, stream, requestStream);
                    }
                }
                catch (IOException e)
                {
                    shouldRetry = handleIOException(csAsyncResult.CSRequest, csAsyncResult.RequestState.WebRequest, null, e, csAsyncResult.RetriesAttempt);
                }

                if (shouldRetry)
                {
                    csAsyncResult.RetriesAttempt++;
                    handleRetry(csAsyncResult.CSRequest, csAsyncResult.RequestState.WebRequest, null, csAsyncResult.OrignalStreamPosition,
                        csAsyncResult.RetriesAttempt, HttpStatusCode.OK, null);
                    invoke<T>(csAsyncResult);
                }
                else
                {
                    if (csAsyncResult.CompletedSynchronously)
                    {
                        this.getResponseCallback<T>(csAsyncResult);
                    }
                    else
                    {
                        IAsyncResult httpResult = state.WebRequest.BeginGetResponse(new AsyncCallback(this.getResponseCallback<T>), csAsyncResult);
                        if (httpResult.CompletedSynchronously)
                        {
                            if (!csAsyncResult.RequestState.GetResponseCallbackCalled)
                            {
                                getResponseCallback<T>(httpResult);
                            }
                            csAsyncResult.SetCompletedSynchronously(true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                csAsyncResult.RequestState.WebRequest.Abort();
                LOGGER.Error("Error for GetRequestStream", e);
                csAsyncResult.Exception = e;

                csAsyncResult.SignalWaitHandle();
                if (csAsyncResult.Callback != null)
                    csAsyncResult.Callback(csAsyncResult);
            }
        }

        void getResponseCallback<T>(IAsyncResult result) where T : CSResponse, new()
        {
            CSAsyncResult csAsyncResult;
            if (result is CSAsyncResult)
                csAsyncResult = result as CSAsyncResult;
            else
                csAsyncResult = result.AsyncState as CSAsyncResult;

            csAsyncResult.RequestState.GetResponseCallbackCalled = true;
            bool shouldRetry = false;
            try
            {
                Exception cause = null;
                HttpStatusCode statusCode = HttpStatusCode.OK;
                RequestState state = csAsyncResult.RequestState;
                HttpWebResponse httpResponse = null;
                T response = null;
                try
                {
                    if (csAsyncResult.CompletedSynchronously)
                        httpResponse = state.WebRequest.GetResponse() as HttpWebResponse;
                    else
                        httpResponse = state.WebRequest.EndGetResponse(result) as HttpWebResponse;

                    long lengthOfRequest = csAsyncResult.CSRequest.StopWatch.ElapsedTicks - state.WebRequestStart;
                    csAsyncResult.CSRequest.ResponseTime = lengthOfRequest;
                    shouldRetry = handleHttpResponse<T>(
                        csAsyncResult.CSRequest,
                        state.WebRequest,
                        httpResponse,
                        csAsyncResult.RetriesAttempt,
                        lengthOfRequest,
                        out response, out cause, out statusCode);
                    if (!shouldRetry)
                    {
                        csAsyncResult.FinalResponse = response;
                    }
                }
                catch (WebException we)
                {
                    shouldRetry = handleHttpWebErrorResponse(csAsyncResult.CSRequest, we, csAsyncResult.RequestState.WebRequest, httpResponse, out cause, out statusCode);
                }
                catch (IOException e)
                {
                    shouldRetry = handleIOException(csAsyncResult.CSRequest, csAsyncResult.RequestState.WebRequest, httpResponse, e, csAsyncResult.RetriesAttempt);
                }

                if (shouldRetry)
                {
                    csAsyncResult.RetriesAttempt++;
                    WebHeaderCollection respHeaders = null;
                    if (response != null)
                    {
                        respHeaders = response.Headers;
                    }

                    handleRetry(csAsyncResult.CSRequest, csAsyncResult.RequestState.WebRequest, respHeaders, csAsyncResult.OrignalStreamPosition,
                        csAsyncResult.RetriesAttempt, statusCode, cause);

                    invoke<T>(csAsyncResult, (httpResponse == null) ? false : isRedirect(httpResponse));
                }
                else if (cause != null)
                {
                    csAsyncResult.Exception = cause;
                }
            }
            catch (Exception e)
            {
                LOGGER.Error("Error for GetResponse", e);
                csAsyncResult.Exception = e;
                shouldRetry = false;
            }
            finally
            {
                if (!shouldRetry)
                {
                    csAsyncResult.SignalWaitHandle();
                    if (csAsyncResult.Callback != null)
                        csAsyncResult.Callback(csAsyncResult);
                }
            }
        }

        /**
         * Add authentication related and version parameters
         */
        void addCSQueryParameters(CSRequest request, ImmutableCredentials immutableCredentials)
        {
            if (request == null)
            {
                return;
            }

            string destinationBucket = request.RequestDestinationBucket;

            Map parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            if (webHeaders != null)
            {                
                webHeaders[CSConstants.DateHeader] = GrandCloudCSUtil.FormattedCurrentTimestamp;
            }

            StringBuilder canonicalResource = new StringBuilder("/", 512);
            if (!String.IsNullOrEmpty(destinationBucket))
            {
                parameters[CSQueryParameter.DestinationBucket] = destinationBucket;
                canonicalResource.Append(destinationBucket);
                if (!destinationBucket.EndsWith("/"))
                {
                    canonicalResource.Append("/");
                }
            }

            // The canonical resource doesn't need the query because it is added
            // in the configureWebRequest function directly to the URL
            if (parameters.ContainsKey(CSQueryParameter.Key))
            {                
                canonicalResource.Append(parameters[CSQueryParameter.Key]);
            }

            parameters[CSQueryParameter.CanonicalizedResource] = canonicalResource.ToString();

            // Has the user added the Content-Type header to the request?
            string value = webHeaders[CSSDKUtils.ContentTypeHeader];
            if (!String.IsNullOrEmpty(value))
            {
                // Remove the header from the webHeaders collection
                // and add it to the parameters
                parameters[CSQueryParameter.ContentType] = value;
                webHeaders.Remove(CSSDKUtils.ContentTypeHeader);
            }

            // Insert the CS Url into the parameters
            addUrlToParameters(request, config);
        }

        void writeStreamToService(CSRequest request, long reqDataLen, Stream inputStream, Stream requestStream)
        {
            if (inputStream != null)
            {
                long current = 0;
                // Reset the file stream's position to the starting point
                inputStream.Position = 0;
                byte[] buffer = new byte[this.config.BufferSize];
                int bytesRead = 0;
                while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    current += bytesRead;
                    requestStream.Write(buffer, 0, bytesRead);
                    if (request != null)
                    {
                        request.OnRaiseProgressEvent(bytesRead, current, reqDataLen);
                    }
                }
            }
        }


        void handleRetry(CSRequest userRequest, HttpWebRequest request, WebHeaderCollection respHdrs, long orignalStreamPosition, int retries, HttpStatusCode statusCode, Exception cause)
        {
            string actionName = userRequest.parameters[CSQueryParameter.Action];
            string requestAddr = request.Address.ToString();

            if (retries <= this.config.MaxErrorRetry)
            {
                if (LOGGER.IsInfoEnabled)
                    LOGGER.InfoFormat("Retry number {0} for request {1}.", retries, actionName);
            }
            pauseOnRetry(retries, this.config.MaxErrorRetry, statusCode, requestAddr, respHdrs, cause);
            // Reset the request so that streams are recreated,
            // removed headers are added back, etc
            prepareRequestForRetry(userRequest, orignalStreamPosition);
        }

        bool handleIOException(CSRequest userRequest, HttpWebRequest request, HttpWebResponse httpResponse, IOException e, int retries)
        {
            if (isInnerExceptionThreadAbort(e))
                throw e;

            string actionName = userRequest.parameters[CSQueryParameter.Action];
            if (LOGGER.IsErrorEnabled)
                LOGGER.Error(string.Format("Error making request {0}.", actionName), e);
            if (httpResponse != null)
            {
                httpResponse.Close();
                httpResponse = null;
            }
            // Abort the unsuccessful request
            request.Abort();

            return retries <= this.config.MaxErrorRetry;
        }

        bool isInnerExceptionThreadAbort(Exception e)
        {
            if (e.InnerException is ThreadAbortException)
                return true;
            if (e.InnerException != null)
                return isInnerExceptionThreadAbort(e.InnerException);
            return false;
        }

        bool handleHttpWebErrorResponse(CSRequest userRequest, WebException we, HttpWebRequest request, HttpWebResponse httpResponse, out Exception cause, out HttpStatusCode statusCode)
        {
            WebHeaderCollection respHdrs;
            string actionName = userRequest.parameters[CSQueryParameter.Action];
            string requestAddr = request.Address.ToString();

            if (LOGGER.IsDebugEnabled)
                LOGGER.Debug(string.Format("Error making request {0}.", actionName), we);


            bool shouldRetry;
            using (HttpWebResponse errorResponse = we.Response as HttpWebResponse)
            {
                shouldRetry = processRequestError(actionName, request, we, errorResponse, requestAddr, out respHdrs, typeof(GrandCloudCSClient), out cause);

                if (httpResponse != null)
                {
                    httpResponse.Close();
                    httpResponse = null;
                }
                // Abort the unsuccessful request regardless of whether we should
                // or shouldn't retry.
                request.Abort();

                if (errorResponse != null)
                {
                    statusCode = errorResponse.StatusCode;
                }
                else
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
            }

            return shouldRetry;
        }

        bool handleHttpResponse<T>(CSRequest userRequest, HttpWebRequest request, HttpWebResponse httpResponse,
            int retries,
            long lengthOfRequest, out T response, out Exception cause, out HttpStatusCode statusCode)
            where T : CSResponse, new()
        {
            response = null;
            cause = null;
            WebHeaderCollection respHdrs = httpResponse.Headers;
            statusCode = httpResponse.StatusCode;
            Map parameters = userRequest.parameters;
            string actionName = parameters[CSQueryParameter.Action];
            string requestAddr = request.Address.ToString();

            bool shouldRetry;
            respHdrs = httpResponse.Headers;
            if (LOGGER.IsInfoEnabled)
                LOGGER.InfoFormat("Received response for {0} (id {1}) with status code {2} in {3}.", actionName, userRequest.Id, httpResponse.StatusCode, lengthOfRequest);

            statusCode = httpResponse.StatusCode;
            if (!isRedirect(httpResponse))
            {
                // The request submission has completed. Retrieve the response.
                shouldRetry = processRequestResponse<T>(httpResponse, userRequest, myType, out response, out cause);
            }
            else
            {
                if (httpResponse != null && string.IsNullOrEmpty(httpResponse.Headers["Location"]))
                {
                    throw new WebException(
                        "A redirect was returned without a new location.  This can be caused by attempting to access buckets with periods in the name in a different region then the client is configured for.",
                        WebExceptionStatus.ProtocolError
                        );
                }

                shouldRetry = true;

                processRedirect(userRequest, httpResponse);
                if (LOGGER.IsInfoEnabled)
                    LOGGER.InfoFormat("Request for {0} is being redirect to {1}.", actionName, userRequest.parameters[CSQueryParameter.Url]);

                pauseOnRetry(retries + 1, this.config.MaxErrorRetry, statusCode, requestAddr, httpResponse.Headers, cause);

                // The HTTPResponse object needs to be closed. Once this is done, the request
                // is gracefully terminated. Mind you, if this response object is not closed,
                // the client will start getting timeout errors.
                // P.S. This sequence of close-response followed by abort-request
                // will be repeated through the exception handlers for this try block
                httpResponse.Close();
                httpResponse = null;
                request.Abort();
            }

            return shouldRetry;
        }

        static void processRedirect(CSRequest userRequest, HttpWebResponse httpResponse)
        {
            if (httpResponse == null)
            {
                throw new WebException(
                    "The Web Response for a redirected request is null!",
                    WebExceptionStatus.ProtocolError
                    );
            }

            // This is a redirect. Get the URL from the location header
            WebHeaderCollection respHeaders = httpResponse.Headers;
            string value;
            if (!String.IsNullOrEmpty(value = respHeaders["Location"]))
            {
                // This should be the new location for the request
                userRequest.parameters[CSQueryParameter.Url] = value;
            }
        }

        static bool isRedirect(HttpWebResponse httpResponse)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse", "Input parameter is null");
            }

            HttpStatusCode statusCode = httpResponse.StatusCode;

            return (statusCode >= HttpStatusCode.MovedPermanently &&
                statusCode < HttpStatusCode.BadRequest);
        }

        /*
         * 1. Add removed headers back to the request's headers
         * 2. If the InputStream is not-null, reset its position to 0
         */
        void prepareRequestForRetry(CSRequest request, long orignalStreamPosition)
        {
            if (request.InputStream != null)
            {
                request.InputStream.Position = orignalStreamPosition;
            }

            if (request.removedHeaders.Count > 0)
            {
                request.Headers.Add(request.removedHeaders);
            }
        }

        bool processRequestResponse<T>(HttpWebResponse httpResponse, CSRequest request, Type t, out T response, out Exception cause)
            where T : CSResponse, new()
        {
            response = default(T);
            cause = null;
            IDictionary<CSQueryParameter, string> parameters = request.parameters;
            string actionName = parameters[CSQueryParameter.Action];
            bool shouldRetry = false;

            if (httpResponse == null)
            {
                throw new WebException(
                    "The Web Response for a successful request is null!",
                    WebExceptionStatus.ProtocolError
                    );
            }

            WebHeaderCollection headerCollection = httpResponse.Headers;
            HttpStatusCode statusCode = httpResponse.StatusCode;
            string responseBody = null;

            try
            {
                if (actionName.Equals("GetObject"))
                {
                    response = new T();
                    Stream respStr = httpResponse.GetResponseStream();
                    request.BytesProcessed = httpResponse.ContentLength;

                    if (parameters.ContainsKey(CSQueryParameter.VerifyChecksum))
                    {
                        try
                        {
                            // The md5Digest needs to be verified
                            string checksumFromS3 = headerCollection[CSSDKUtils.ETagHeader];
                            checksumFromS3 = checksumFromS3.Replace("\"", String.Empty);
                            if (respStr.CanSeek)
                            {
                                response.ResponseStream = respStr;
                            }
                            else
                            {
                                response.ResponseStream = GrandCloudCSUtil.MakeStreamSeekable(respStr);
                            }
                            string calculatedCS = GrandCloudCSUtil.GenerateChecksumForStream(response.ResponseStream, false);

                            // do a case-insensitive comparison of the 2 strings.
                            if (0 != StringComparer.OrdinalIgnoreCase.Compare(calculatedCS, checksumFromS3))
                            {
                                string msg = String.Concat(
                                    "The calculated md5Digest '",
                                    calculatedCS,
                                    "' differs from the md5Digest returned by CS '",
                                    checksumFromS3
                                    );

                                throw new GrandCloudCSException(
                                    msg,
                                    HttpStatusCode.BadRequest,
                                    "BadDigest",
                                    "",
                                    "",
                                    parameters[CSQueryParameter.RequestAddress],
                                    headerCollection
                                    );
                            }
                        }
                        catch (Exception)
                        {
                            // Handle this error gracefully by setting the response object
                            // to be null. The outer finally block will catch the exception
                            // and close the httpResponse if the response object is null
                            response = null;
                            throw;
                        }
                    }
                    else
                    {
                        response.ResponseStream = respStr;
                    }
                }
                else
                {
                    using (httpResponse)
                    {
                        long streamRead = request.StopWatch.ElapsedTicks;

                        using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            responseBody = reader.ReadToEnd();
                        }

                        request.BytesProcessed = responseBody.Length;
                        responseBody = responseBody.Trim();

                        if (responseBody.EndsWith("/Error>"))
                        {
                            // Even though we received a 200 OK, there is a possibility of receiving an error
                            string transformed = transform(responseBody, "CSError", t);
                            // Attempt to deserialize response into S3ErrorResponse type
                            CSError error;
                            XmlSerializer serializer = new XmlSerializer(typeof(CSError));
                            using (XmlTextReader sr = new XmlTextReader(new StringReader(transformed)))
                            {
                                error = (CSError)serializer.Deserialize(sr);
                            }
                            cause = new GrandCloudCSException(statusCode, responseBody, parameters[CSQueryParameter.RequestAddress], headerCollection, error);
                            shouldRetry = true;
                        }

                        // Perform response transformation
                        else if (responseBody.EndsWith(">"))
                        {
                            string transformed = transform(responseBody, actionName, t);

                            long streamParsed = request.StopWatch.ElapsedTicks;

                            // Attempt to deserialize response into <Action> Response type
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            using (XmlTextReader sr = new XmlTextReader(new StringReader(transformed)))
                            {
                                response = (T)serializer.Deserialize(sr);
                            }
                            long objectCreated = request.StopWatch.ElapsedTicks;
                            request.ResponseReadTime = streamParsed - streamRead;
                            request.ResponseProcessingTime = objectCreated - streamParsed;
                            if (LOGGER.IsInfoEnabled)
                                LOGGER.InfoFormat("Done reading response stream for request (id {0}). Stream read: {1}. Object create: {2}. Length of body: {3}",
                                    request.Id,
                                    request.ResponseReadTime,
                                    request.ResponseProcessingTime,
                                    request.BytesProcessed);
                        }
                        else
                        {
                            // We can receive responses that have no response body.
                            // All responses have headers so at a future point,
                            // we "do" attach the headers to the response.
                            response = new T();
                            response.ProcessResponseBody(responseBody);

                            long streamParsed = request.StopWatch.ElapsedTicks;
                            request.ResponseReadTime = streamParsed - streamRead;
                        }
                    }

                    // We are done with our use of the httpResponse object
                    httpResponse = null;
                }
            }
            finally
            {
                if (actionName.Equals("GetObject") &&
                    response != null)
                {
                    // Save the http response object so that it can be closed
                    // gracefully when the GetObjectResponse object is either
                    // garbage-collected or disposed
                    response.httpResponse = httpResponse;
                }
                else if (httpResponse != null)
                {
                    httpResponse.Close();
                    httpResponse = null;
                }

                // Store the headers in the response for all successful service requests
                if (response != null)
                {
                    // Add the header key/value pairs to our <Action> Response type
                    response.Headers = headerCollection;
                    response.ResponseXml = responseBody;
                }
            }

            return shouldRetry;
        }

        private bool processRequestError(string actionName, HttpWebRequest request, WebException we, HttpWebResponse errorResponse, string requestAddr, out WebHeaderCollection respHdrs, Type t, out Exception cause)
        {
            bool shouldRetry = false;
            HttpStatusCode statusCode = default(HttpStatusCode);
            string responseBody = null;

            // Initialize the out parameter to null
            // in case there is no errorResponse
            respHdrs = null;

            if (errorResponse == null)
            {
                if (LOGGER.IsErrorEnabled)
                    LOGGER.Error(string.Format("Error making request {0}.", actionName), we);
                throw we;
            }

            // Set the response headers for future use
            respHdrs = errorResponse.Headers;

            // Obtain the HTTP status code
            statusCode = errorResponse.StatusCode;

            using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream(), Encoding.UTF8))
            {
                responseBody = reader.ReadToEnd();
            }

            if (request.Method.Equals("HEAD"))
            {
                string message = we.Message;
                string errorCode = statusCode.ToString();
                if (statusCode == HttpStatusCode.NotFound)
                {
                    message = "The specified key does not exist";
                    errorCode = "NoSuchKey";
                }

                GrandCloudCSException excep = new GrandCloudCSException(
                    message,
                    statusCode,
                    errorCode,
                    respHdrs[CSConstants.RequestIdHeader],
                    "",
                    requestAddr,
                    respHdrs
                    );

                if (LOGGER.IsErrorEnabled)
                    LOGGER.Error(string.Format("Error making request {0}.", actionName), excep);
                throw excep;
            }

            if (statusCode == HttpStatusCode.InternalServerError ||
                statusCode == HttpStatusCode.ServiceUnavailable)
            {
                shouldRetry = true;
                cause = we;
            }
            else
            {
                // Attempt to deserialize response into ErrorResponse type
                using (XmlTextReader sr = new XmlTextReader(new StringReader(transform(responseBody, "CSError", t))))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(CSError));
                    CSError error = (CSError)serializer.Deserialize(sr);

                    // Throw formatted exception with information available from the error response
                    GrandCloudCSException excep = new GrandCloudCSException(
                        error.Message,
                        statusCode,
                        error.Code,
                        error.RequestId,
                        responseBody,
                        requestAddr,
                        respHdrs
                        );

                    if (LOGGER.IsErrorEnabled)
                        LOGGER.Error(string.Format("Error making request {0}.", actionName), excep);
                    throw excep;
                }
            }

            return shouldRetry;
        }

        /**
         * Build the Url from the parameters passed in.
         * Component parts are:
         * - ServiceURL from the Config
         * - Bucket
         * - Key
         * - urlPrefix
         * - Query
         */
        void addUrlToParameters(CSRequest request, GrandCloudCSConfig config)
        {
            Map parameters = request.parameters;

            if (!config.IsSetServiceURL())
            {
                throw new GrandCloudCSException("The GrandCloud CS Service URL is either null or empty");
            }

            string url = config.ServiceURL;
 
            if (parameters.ContainsKey(CSQueryParameter.DestinationBucket))
            {
                string bucketName = parameters[CSQueryParameter.DestinationBucket];
                if (!string.IsNullOrEmpty(bucketName))
                {
                    url = String.Concat(url, "/", bucketName, "/");
                    
                }

                if (parameters.ContainsKey(CSQueryParameter.Key))
                {
                    if (!url.EndsWith("/"))
                    {
                        url = String.Concat(url, "/", parameters[CSQueryParameter.Key]);
                    }
                    else
                    {
                        url = String.Concat(url, parameters[CSQueryParameter.Key]);
                    }
                }
            }

            string urlPrefix = "https://";
            if (config.CommunicationProtocol == Protocol.HTTP)
            {
                urlPrefix = "http://";
            }
            url = String.Concat(urlPrefix, url);

            // Encode the URL
            url = GrandCloudCSUtil.UrlEncode(url, true);

            if (parameters.ContainsKey(CSQueryParameter.Query))
            {
                url = String.Concat(url, parameters[CSQueryParameter.Query]);
            }

            // Add the Url to the parameters
            parameters[CSQueryParameter.Url] = url;
        }

        /**
         * Code for fixing a bug in C#
         * The problem is when the url is like http://storage.grandcloud.cn/test./ , Uri class will remove the last "." in the url,
         * this function is to fix this problem.
         */ 
        private void CodeForUrlWithDot()
        {
            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic); 
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {     
                foreach (string scheme in new[] { "http", "https" })
                {         
                    UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
                    if (parser != null)
                    {
                        int flagsValue = (int)flagsField.GetValue(parser);
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }     
                } 
            } 
        }
        /**
         * Configure HttpClient with set of defaults as well as configuration
         * from AmazonEC2Config instance
         */
        HttpWebRequest configureWebRequest(CSRequest request, long contentLength, ImmutableCredentials credentials)
        {
            WebHeaderCollection headers = request.Headers;
            Map parameters = request.parameters;

            if (!parameters.ContainsKey(CSQueryParameter.Url))
            {
                throw new GrandCloudCSException("The Amazon CS URL is either null or empty");
            }

            string url = parameters[CSQueryParameter.Url];

            CodeForUrlWithDot();

            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                httpRequest.ServicePoint.ConnectionLimit = this.config.ConnectionLimit;

                if (config.IsSetProxyHost() && config.IsSetProxyPort())
                {
                    WebProxy proxy = new WebProxy(config.ProxyHost, config.ProxyPort);
                    if (config.IsSetProxyUsername())
                    {
                        proxy.Credentials = new NetworkCredential(
                            config.ProxyUsername,
                            config.ProxyPassword ?? String.Empty
                            );
                        if (LOGGER.IsDebugEnabled)
                            LOGGER.DebugFormat("Configured request to use proxy with host {0} and port {1} for user {2}.", config.ProxyHost, config.ProxyPort, config.ProxyUsername);
                    }
                    else
                    {
                        if (LOGGER.IsDebugEnabled)
                            LOGGER.DebugFormat("Configured request to use proxy with host {0} and port {1}.", config.ProxyHost, config.ProxyPort);
                    }
                    httpRequest.Proxy = proxy;
                }

                string value = headers[CSSDKUtils.IfModifiedSinceHeader];
                if (!String.IsNullOrEmpty(value))
                {
                    DateTime date = DateTime.ParseExact(value, CSSDKUtils.GMTDateFormat, null);
                    httpRequest.IfModifiedSince = date;
                    headers.Remove(CSSDKUtils.IfModifiedSinceHeader);
                    request.removedHeaders[CSSDKUtils.IfModifiedSinceHeader] = value;
                }

                // The Content-Type header could have been specified using
                // the CSRequest.AddHeader method. If Content-Type was specified,
                // it needs to be removed and set as an explicit property
                // of the HttpWebRequest.
                value = headers[CSSDKUtils.ContentTypeHeader];
                if (!String.IsNullOrEmpty(value))
                {
                    httpRequest.ContentType = value;
                    headers.Remove(CSSDKUtils.ContentTypeHeader);
                    request.removedHeaders[CSSDKUtils.ContentTypeHeader] = value;
                }

                // The User-Agent header could have been specified using
                // the CSRequest.AddHeader method. If User-Agent was specified,
                // it needs to be removed and set as an explicit property
                // of the HttpWebRequest.
                value = headers[CSSDKUtils.UserAgentHeader];
                if (!String.IsNullOrEmpty(value))
                {
                    httpRequest.UserAgent = value;
                    headers.Remove(CSSDKUtils.UserAgentHeader);
                    request.removedHeaders[CSSDKUtils.UserAgentHeader] = value;
                }

                if (parameters.ContainsKey(CSQueryParameter.ContentType))
                {
                    httpRequest.ContentType = parameters[CSQueryParameter.ContentType];
                }

                if (parameters.ContainsKey(CSQueryParameter.Range))
                {
                    string rangeHeader = parameters[CSQueryParameter.Range];
                    char[] splitter = { ':' };
                    string[] myRange = rangeHeader.Split(splitter);
                    addHttpRange(httpRequest, long.Parse(myRange[0]), long.Parse(myRange[1]));
                }

                // Add the AWS Authorization header.
                if (credentials != null && !string.IsNullOrEmpty(credentials.AccessKey))
                {
                    httpRequest.Headers[CSConstants.AuthorizationHeader] = String.Concat(
                        "SNDA ",
                        credentials.AccessKey,
                        ":",
                        parameters[CSQueryParameter.Authorization]);
                }

                // Let's enable Expect100Continue only for PutObject requests
                httpRequest.ServicePoint.Expect100Continue = request.Expect100Continue;

                // While checking the Action, for Get, Put and Copy Object, set
                // the timeout to the value specified in the request.
                if (request.SupportTimeout)
                {
                    int timeout = 0;
                    Int32.TryParse(parameters[CSQueryParameter.RequestTimeout], out timeout);
                    if (timeout > 0 || timeout == System.Threading.Timeout.Infinite)
                    {
                        httpRequest.Timeout = timeout;
                        httpRequest.ReadWriteTimeout = timeout; // set both for backwards compatibility
                    }
                }

                // While checking the Action, for Get, Put and Copy Object, set
                // the read/write timeout to the value specified in the request.
                if (request.SupportReadWriteTimeout)
                {
                    int readWriteTimeout = 0;
                    Int32.TryParse(parameters[CSQueryParameter.RequestReadWriteTimeout], out readWriteTimeout);
                    if (readWriteTimeout > 0 || readWriteTimeout == System.Threading.Timeout.Infinite)
                    {
                        httpRequest.ReadWriteTimeout = readWriteTimeout;
                    }
                }

                httpRequest.Headers.Add(headers);
                httpRequest.Method = parameters[CSQueryParameter.Verb];
                httpRequest.ContentLength = contentLength;
                httpRequest.AllowWriteStreamBuffering = false;
                httpRequest.AllowAutoRedirect = false;
            }
            return httpRequest;
        }

        /**
         * Exponential sleep on failed request
         */
        void pauseOnRetry(int retries, int maxRetries, HttpStatusCode status, string requestAddr, WebHeaderCollection headers, Exception cause)
        {
            if (retries <= maxRetries)
            {
                int delay = (int)Math.Pow(4, retries) * 100;
                System.Threading.Thread.Sleep(delay);
            }
            else
            {
                throw new GrandCloudCSException(
                    String.Concat("Maximum number of retry attempts reached : ", (retries - 1)),
                    status,
                    requestAddr,
                    headers,
                    cause
                    );
            }
        }

        /*
         *  Transforms response based on xslt template
         */
        string transform(string responseBody, string actionName, Type t)
        {
            XslCompiledTransform transformer = new XslCompiledTransform();
            char[] seps = { ',' };
            Assembly assembly = Assembly.GetAssembly(t);

            string assemblyName = assembly.FullName;
            assemblyName = assemblyName.Split(seps)[0];

            // Build the name of the xslt transform to apply to the response
            string ns = t.Namespace;
            string resourceName = String.Concat(
                assemblyName,
                ".",
                ns,
                ".Model.",
                actionName,
                "Response.xslt"
                );

            using (XmlTextReader xmlReader = new XmlTextReader(assembly.GetManifestResourceStream(resourceName)))
            {
                transformer.Load(xmlReader);

                using (XmlTextReader xmlR = new XmlTextReader(new StringReader(responseBody)))
                {
                    StringBuilder sb = new StringBuilder(1024);
                    using (StringWriter sw = new StringWriter(sb))
                    {
                        transformer.Transform(xmlR, null, sw);
                        return sb.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the If-Match Header in the specified header collection.
        ///
        /// Return the object only if its entity tag (ETag) is the same as the one
        /// specified, otherwise return a 412 (precondition failed).
        /// </summary>
        /// <param name="headers">The header collection to add the new header to</param>
        /// <param name="eTag">The ETag to match against</param>
        void setIfMatchHeader(WebHeaderCollection headers, string eTag)
        {
            headers[CSSDKUtils.IfMatchHeader] = eTag;
        }

        /// <summary>
        /// Set the If-None-Match Header in the specified header collection.
        ///
        /// Return the object only if its entity tag (ETag) is different from the one
        /// specified, otherwise return a 304 (not modified).
        /// </summary>
        /// <param name="headers">The header collection to add the new header to</param>
        /// <param name="eTag">The ETag to match against</param>
        void setIfNoneMatchHeader(WebHeaderCollection headers, string eTag)
        {
            headers["If-None-Match"] = eTag;
        }

        /// <summary>
        /// Sets the If-Modifed-Since Header in the specified header collection.
        ///
        /// Return the object only if it has been modified since the specified time,
        /// otherwise return a 304 (not modified).
        /// </summary>
        /// <param name="headers">The header collection to add the new header to</param>
        /// <param name="date">DateTime Object representing the date to use</param>
        void setIfModifiedSinceHeader(WebHeaderCollection headers, DateTime date)
        {
            headers[CSSDKUtils.IfModifiedSinceHeader] = date.ToUniversalTime().ToString(CSSDKUtils.GMTDateFormat);
        }

        /// <summary>
        /// Sets the If-Unmodifed-Since Header in the specified header collection.
        ///
        /// Return the object only if it has not been modified since the specified time,
        /// otherwise return a 412 (precondition failed).
        /// </summary>
        /// <param name="headers">The header collection to add the new header to</param>
        /// <param name="date">DateTime Object representing the date to use</param>
        void setIfUnmodifiedSinceHeader(WebHeaderCollection headers, DateTime date)
        {
            headers["If-Unmodified-Since"] = date.ToUniversalTime().ToString(CSSDKUtils.GMTDateFormat);
        }

        /**
         * Creates a string based on the parameters and encrypts it using
         * key. Returns the encrypted string.
         */
        string buildSigningString(IDictionary<CSQueryParameter, string> parameters, WebHeaderCollection webHeaders)
        {
            StringBuilder sb = new StringBuilder("", 256);
            string value = null;

            sb.Append(parameters[CSQueryParameter.Verb]);
            sb.Append("\n");

            if (webHeaders != null)
            {
                if (!String.IsNullOrEmpty(value = webHeaders[CSSDKUtils.ContentMD5Header]))
                {
                    sb.Append(value);
                }
                sb.Append("\n");

                if (parameters.ContainsKey(CSQueryParameter.ContentType))
                {
                    sb.Append(parameters[CSQueryParameter.ContentType]);
                }
                sb.Append("\n");
            }
            else
            {
                // The headers are null, but we still need to append
                // the 2 newlines that are required by CS.
                // Without these, CS rejects the signature.
                sb.Append("\n\n");
            }

            if (parameters.ContainsKey(CSQueryParameter.Expires))
            {
                sb.Append(parameters[CSQueryParameter.Expires]);
                webHeaders.Remove(CSConstants.DateHeader);
            }
            sb.Append("\n");

            sb.Append(buildCanonicalizedHeaders(webHeaders));

            if (parameters.ContainsKey(CSQueryParameter.CanonicalizedResource))
            {
                sb.Append(GrandCloudCSUtil.UrlEncode(parameters[CSQueryParameter.CanonicalizedResource], true));
            }

            //string action = parameters[CSQueryParameter.Action];

            if (parameters.ContainsKey(CSQueryParameter.QueryToSign))
            {
                sb.Append(parameters[CSQueryParameter.QueryToSign]);
            }

            return sb.ToString();
        }

        /**
         * Returns a string of all x-amz headers sorted by Ordinal.
         */
        StringBuilder buildCanonicalizedHeaders(WebHeaderCollection headers)
        {
            // Build a sorted list of headers that start with x-snda
            List<string> list = new List<string>(headers.Count);
            foreach (string key in headers.AllKeys)
            {
                string lowerKey = key.ToLower();
                if (lowerKey.StartsWith("x-snda-"))
                {
                    list.Add(lowerKey);
                }
            }
            // Using the recommendations from:
            // http://msdn.microsoft.com/en-us/library/ms973919.aspx
            list.Sort(StringComparer.Ordinal);

            // Create the canonicalized header string to return.
            StringBuilder sb = new StringBuilder(256);
            foreach (string key in list)
            {
                sb.Append(String.Concat(key, ":", headers[key], "\n"));
            }

            return sb;
        }

        void addHttpRange(HttpWebRequest request, long start, long end)
        {
            if (ADD_RANGE_METHODINFO != null)
            {
                string rangeSpecifier = "bytes";
                string fromString = start.ToString();
                string toString = end.ToString();

                object[] args = new object[3];

                args[0] = rangeSpecifier;
                args[1] = fromString;
                args[2] = toString;

                ADD_RANGE_METHODINFO.Invoke(request, args);
            }
            else
            {
                request.AddRange(Convert.ToInt32(start), Convert.ToInt32(end));
            }
        }

        private static CSCredentials CreateCredentials(string awsAccessKeyId, SecureString awsSecretAccessKey)
        {
            if (string.IsNullOrEmpty(awsAccessKeyId))
            {
                return null; // anonymous access, no credentials specified
            }
            else
            {
                return new BasicCSCredentials(awsAccessKeyId, awsSecretAccessKey);
            }
        }

        private static CSCredentials CreateCredentials(string awsAccessKeyId, string awsSecretAccessKey)
        {
            if (string.IsNullOrEmpty(awsAccessKeyId))
            {
                return null; // anonymous access, no credentials specified
            }
            else
            {
                return new BasicCSCredentials(awsAccessKeyId, awsSecretAccessKey);
            }
        }

        #endregion

        #region Async Classes
        class CSAsyncResult : IAsyncResult
        {
            private static Logger _logger = new Logger(typeof(CSAsyncResult));

            bool _isComplete;
            bool _completedSynchronously;
            ManualResetEvent _waitHandle;
            CSRequest _csRequest;
            AsyncCallback _callback;
            RequestState _requestState;
            long _orignalStreamPosition;
            object _state;
            int _retiresAttempt;
            Exception _exception;
            CSResponse _finalResponse;
            Dictionary<string, object> _parameters;
            object _lockObj;

            private long _startTime;

            internal CSAsyncResult(CSRequest csRequest, object state, AsyncCallback callback, bool completeSynchronized)
            {
                this._csRequest = csRequest;
                this._callback = callback;
                this._state = state;
                this._completedSynchronously = completeSynchronized;

                this._lockObj = new object();

                this.CSRequest.StopWatch = Stopwatch.StartNew();
                this.CSRequest.StopWatch.Start();
                this._startTime = this.CSRequest.StopWatch.ElapsedTicks;
            }

            internal CSRequest CSRequest
            {
                get { return this._csRequest; }
                set { this._csRequest = value; }
            }

            internal Exception Exception
            {
                get { return this._exception; }
                set { this._exception = value; }
            }

            internal long OrignalStreamPosition
            {
                get { return this._orignalStreamPosition; }
                set { this._orignalStreamPosition = value; }
            }

            internal int RetriesAttempt
            {
                get { return this._retiresAttempt; }
                set { this._retiresAttempt = value; }
            }

            internal AsyncCallback Callback
            {
                get { return this._callback; }
            }

            internal void SignalWaitHandle()
            {
                this._isComplete = true;

                if (this._waitHandle != null)
                {
                    this._waitHandle.Set();
                }
            }

            internal object State
            {
                get { return this._state; }
            }

            public bool CompletedSynchronously
            {
                get { return this._completedSynchronously; }
            }

            internal void SetCompletedSynchronously(bool completedSynchronously)
            {
                this._completedSynchronously = completedSynchronously;
            }

            public bool IsCompleted
            {
                get { return this._isComplete; }
            }

            internal void SetIsComplete(bool isComplete)
            {
                this._isComplete = isComplete;
            }

            public object AsyncState
            {
                get { return this._state; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    if (this._waitHandle != null)
                    {
                        return this._waitHandle;
                    }

                    lock (this._lockObj)
                    {
                        if (this._waitHandle == null)
                        {
                            this._waitHandle = new ManualResetEvent(this._isComplete);
                        }
                    }

                    return this._waitHandle;
                }
            }

            internal RequestState RequestState
            {
                get { return this._requestState; }
                set { this._requestState = value; }
            }


            internal CSResponse FinalResponse
            {
                get { return this._finalResponse; }
                set
                {
                    this._finalResponse = value;
                    long endTime = this._csRequest.StopWatch.ElapsedTicks;
                    long timeToComplete = endTime - this._startTime;
                    this._csRequest.TotalRequestTime = timeToComplete;
                    if (_logger.IsDebugEnabled)
                        _logger.InfoFormat("CS request completed: {0}", this._csRequest);
                }
            }

            internal Dictionary<string, object> Parameters
            {
                get
                {
                    if (this._parameters == null)
                    {
                        this._parameters = new Dictionary<string, object>();
                    }

                    return this._parameters;
                }
            }
        }


        class RequestState
        {
            Stream _inputStream;
            byte[] _requestData;
            long _requestDataLength;
            HttpWebRequest _webRequest;
            Map _parameters;
            long _webRequestStart;
            bool _getRequestStreamCallbackCalled;
            bool _getResponseCallbackCalled;


            public RequestState(HttpWebRequest webRequest, Map parameters, Stream inputStream, byte[] requestData, long requestDataLength, long startTime)
            {
                this._webRequest = webRequest;
                this._parameters = parameters;
                this._inputStream = inputStream;
                this._requestData = requestData;
                this._requestDataLength = requestDataLength;
                this._webRequestStart = startTime;
            }

            internal HttpWebRequest WebRequest
            {
                get { return this._webRequest; }
            }

            internal Map Parameters
            {
                get { return this._parameters; }
            }

            internal Stream InputStream
            {
                get { return this._inputStream; }
            }

            internal byte[] RequestData
            {
                get { return this._requestData; }
            }

            internal long RequestDataLength
            {
                get { return this._requestDataLength; }
            }

            internal long WebRequestStart
            {
                get { return this._webRequestStart; }
                set { this._webRequestStart = value; }
            }

            internal bool GetRequestStreamCallbackCalled
            {
                get { return this._getRequestStreamCallbackCalled; }
                set { this._getRequestStreamCallbackCalled = value; }
            }

            internal bool GetResponseCallbackCalled
            {
                get { return this._getResponseCallbackCalled; }
                set { this._getResponseCallbackCalled = value; }
            }
        }
        #endregion
    }
}
