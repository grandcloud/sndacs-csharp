
using System;

using GrandCloud.CS.Model;

namespace GrandCloud.CS
{
    /// <summary>
    /// Interface for GrandCloud CS Clients.
    /// For more information about GrandCloud CS, go to <see href="http://www.grandcloud.cn/product/ecs"/>
    /// </summary>
    public interface GrandCloudCS : IDisposable
    {
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
        string GetPreSignedURL(GetPreSignedUrlRequest request);

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
        ListBucketsResponse ListAllBuckets();

        /// <summary>
        /// The ListAllBuckets operation returns a list of all of the buckets
        /// owned by the authenticated sender of the request.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListBucketsResponse with the response from CS.</returns>
        IAsyncResult BeginListAllBuckets(AsyncCallback callback, object state);

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
        IAsyncResult BeginListAllBuckets(ListBucketsRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the ListAllBuckets operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListAllBuckets.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListBucketsResponse from CS.</returns>
        ListBucketsResponse EndListAllBuckets(IAsyncResult asyncResult);

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
        ListBucketsResponse ListAllBuckets(ListBucketsRequest request);

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
        IAsyncResult BeginGetBucketLocation(GetBucketLocationRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the GetBucketLocation operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetBucketLocation.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetBucketLocationResponse from CS.</returns>
        GetBucketLocationResponse EndGetBucketLocation(IAsyncResult asyncResult);

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
        GetBucketLocationResponse GetBucketLocation(GetBucketLocationRequest request);

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
        IAsyncResult BeginGetBucketPolicy(GetBucketPolicyRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the GetBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetBucketPolicyResponse from CS.</returns>
        GetBucketPolicyResponse EndGetBucketPolicy(IAsyncResult asyncResult);

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
        /// For more information on forming bucket polices, 
        /// refer: <see href="http://docs.amazonwebservices.com/GrandCloudCS/latest/dev/"/>
        /// </para>
        /// </summary>
        /// <param name="request">The GetBucketPolicyRequest that defines the parameters of the operation.</param>
        /// <returns>Returns a GetBucketPolicyResponse from CS.</returns>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        GetBucketPolicyResponse GetBucketPolicy(GetBucketPolicyRequest request);

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
        IAsyncResult BeginSetBucketPolicy(PutBucketPolicyRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the SetBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginSetBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutBucketPolicyResponse from CS.</returns>
        PutBucketPolicyResponse EndSetBucketPolicy(IAsyncResult asyncResult);

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
        PutBucketPolicyResponse SetBucketPolicy(PutBucketPolicyRequest request);

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
        IAsyncResult BeginDeleteBucketPolicy(DeleteBucketPolicyRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteBucketPolicy operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteBucketPolicy.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteBucketPolicyResponse from CS.</returns>
        DeleteBucketPolicyResponse EndDeleteBucketPolicy(IAsyncResult asyncResult);

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
        DeleteBucketPolicyResponse DeleteBucketPolicy(DeleteBucketPolicyRequest request);

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
        IAsyncResult BeginListObjects(ListObjectsRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the ListObjects operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListObjects.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListObjectsResponse from CS.</returns>
        ListObjectsResponse EndListObjects(IAsyncResult asyncResult);

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
        ListObjectsResponse ListObjects(ListObjectsRequest request);

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
        IAsyncResult BeginCreateBucket(PutBucketRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the CreateBucket operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCreateBucket.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutBucketResponse from CS.</returns>
        PutBucketResponse EndCreateBucket(IAsyncResult asyncResult);

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
        /// <para>
        /// Buckets are similar to Internet domain names. Just as GrandCloud is the only owner
        /// of the domain name GrandCloud.com, only one person or organization can own a bucket
        /// within GrandCloud CS. The similarities between buckets and domain names is not a
        /// coincidence - there is a direct mapping between GrandCloud CS buckets and subdomains
        /// of s3.amazonaws.com. Objects stored in GrandCloud CS are addressable using the REST API
        /// under the domain bucketname.s3.amazonaws.com. For example, the object homepage.html
        /// stored in the GrandCloud CS bucket mybucket can be addressed as
        /// http://mybucket.s3.amazonaws.com/homepage.html.</para>
        /// To conform with DNS requirements, the following constraints apply:
        /// <list type="bullet">
        /// <item>Bucket names should not contain underscores (_)</item>
        /// <item>Bucket names should be between 3 and 63 characters long</item>
        /// <item>Bucket names should not end with a dash</item>
        /// <item>Bucket names cannot contain two, adjacent periods</item>
        /// <item>Bucket names cannot contain dashes next to periods
        ///   (e.g., "my-.bucket.com" and "my.-bucket" are invalid)</item>
        /// <item>Bucket names cannot contain uppercase characters</item>
        /// </list>
        /// There is no limit to the number of objects that can be stored in a bucket and no
        /// variation in performance when using many buckets or just a few. You can store all
        /// of your objects in a single bucket or organize them across several buckets.
        /// </remarks>
        /// <seealso cref="T:GrandCloud.CS.Model.CSRegion"/>
        PutBucketResponse CreateBucket(PutBucketRequest request);

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
        IAsyncResult BeginDeleteBucket(DeleteBucketRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteBucket operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteBucket.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteBucketResponse from CS.</returns>
        DeleteBucketResponse EndDeleteBucket(IAsyncResult asyncResult);

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
        DeleteBucketResponse DeleteBucket(DeleteBucketRequest request);


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
        IAsyncResult BeginGetObject(GetObjectRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the GetObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectResponse from CS.</returns>
        GetObjectResponse EndGetObject(IAsyncResult asyncResult);

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
        GetObjectResponse GetObject(GetObjectRequest request);

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
        IAsyncResult BeginHeadObject(GetObjectMetadataRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the HeadObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginHeadObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a GetObjectMetadataResponse from CS.</returns>
        GetObjectMetadataResponse EndHeadObject(IAsyncResult asyncResult);

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
        GetObjectMetadataResponse HeadObject(GetObjectMetadataRequest request);

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
        IAsyncResult BeginPutObject(PutObjectRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the PutObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginPutObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a PutObjectResponse from CS.</returns>
        PutObjectResponse EndPutObject(IAsyncResult asyncResult);

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
        PutObjectResponse PutObject(PutObjectRequest request);

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
        IAsyncResult BeginDeleteObject(DeleteObjectRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the DeleteObject operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteObject.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a DeleteObjectResponse from CS.</returns>
        DeleteObjectResponse EndDeleteObject(IAsyncResult asyncResult);

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
        DeleteObjectResponse DeleteObject(DeleteObjectRequest request);

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
        IAsyncResult BeginInitiateMultipartUpload(InitiateMultipartUploadRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the InitiateMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginInitiateMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a InitiateMultipartUploadResponse from CS.</returns>
        InitiateMultipartUploadResponse EndInitiateMultipartUpload(IAsyncResult asyncResult);

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
        InitiateMultipartUploadResponse InitiateMultipartUpload(InitiateMultipartUploadRequest request);

        #endregion

        #region UploadPart

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
        IAsyncResult BeginUploadPart(UploadPartRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the UploadPart operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginUploadPart.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a UploadPartResponse from CS.</returns>
        UploadPartResponse EndUploadPart(IAsyncResult asyncResult);

        /// <summary>
        /// This method uploads a part in a multipart upload.  You must initiate a 
        /// multipart upload before you can upload any part.
        /// <para>
        /// Your UploadPart request must include an upload ID and a part number. 
        /// The upload ID is the ID returned by GrandCloud CS in response to your 
        /// Initiate Multipart Upload request. Part number can be any number between 1 and
        /// 10,000, inclusive. A part number uniquely identifies a part and also 
        /// defines its position within the object being uploaded. If you 
        /// upload a new part using the same part number as an existing
        /// part, that part is overwritten.
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
        UploadPartResponse UploadPart(UploadPartRequest request);

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
        IAsyncResult BeginListParts(ListPartsRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the ListParts operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListParts.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListPartsResponse from CS.</returns>
        ListPartsResponse EndListParts(IAsyncResult asyncResult);

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
        ListPartsResponse ListParts(ListPartsRequest request);

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
        IAsyncResult BeginAbortMultipartUpload(AbortMultipartUploadRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the AbortMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginAbortMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a AbortMultipartUploadResponse from CS.</returns>
        AbortMultipartUploadResponse EndAbortMultipartUpload(IAsyncResult asyncResult);

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
        AbortMultipartUploadResponse AbortMultipartUpload(AbortMultipartUploadRequest request);

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
        IAsyncResult BeginCompleteMultipartUpload(CompleteMultipartUploadRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the CompleteMultipartUpload operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginCompleteMultipartUpload.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a CompleteMultipartUploadResponse from CS.</returns>
        CompleteMultipartUploadResponse EndCompleteMultipartUpload(IAsyncResult asyncResult);

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
        CompleteMultipartUploadResponse CompleteMultipartUpload(CompleteMultipartUploadRequest request);

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
        IAsyncResult BeginListMultipartUploads(ListMultipartUploadsRequest request, AsyncCallback callback, object state);

        /// <summary>
        /// Finishes the asynchronous execution of the ListMultipartUploads operation.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginListMultipartUploads.</param>
        /// <exception cref="T:System.ArgumentNullException"></exception>
        /// <exception cref="T:System.Net.WebException"></exception>
        /// <exception cref="T:GrandCloud.CS.GrandCloudCSException"></exception>
        /// <returns>Returns a ListMultipartUploadsResponse from CS.</returns>
        ListMultipartUploadsResponse EndListMultipartUploads(IAsyncResult asyncResult);

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
        ListMultipartUploadsResponse ListMultipartUploads(ListMultipartUploadsRequest request);

        #endregion

    }
}