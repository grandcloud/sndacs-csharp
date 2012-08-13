
namespace GrandCloud.CS
{
    /// <summary>
    /// The GrandCloud Web Services SDK provides devlopers with a coherent and unified interface to the
    /// suite of GrandCloud Web Services. The intent is to facilitate the rapid building of
    /// applications that leverage multiple GrandCloud Web Services.
    /// <para>
    /// To get started, request an instance of the CSClientFactory via this class's static Instance
    /// member. Use the factory instance to create clients for all the Web Services needed by
    /// the application.</para>
    /// </summary>

    public static class CSClientFactory
    {

 
        /// <summary>
        /// Create a client for the GrandCloud CS Service with the credentials defined in the App.config.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="CSAccessKey" value="********************"/&gt;
        ///         &lt;add key="CSSecretKey" value="****************************************"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        /// </summary>
        /// <returns>An GrandCloud CS client</returns>
        /// <remarks>
        /// </remarks>
        public static GrandCloudCS CreateGrandCloudCSClient()
        {
            return new GrandCloudCSClient();
        }

        /// <summary>
        /// Create a client for the GrandCloud CS Service with the credentials defined in the App.config.
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
        /// </summary>
        /// <param name="config">Configuration options for the service like HTTP Proxy, # of connections, etc</param>
        /// <returns>An GrandCloud CS client</returns>
        public static GrandCloudCS CreateGrandCloudCSClient(GrandCloudCSConfig config)
        {
            return new GrandCloudCSClient(config);
        }

        /// <summary>
        /// Create a client for the GrandCloud CS service with the default configuration
        /// </summary>
        /// <param name="accessKey">The Access Key associated with the account</param>
        /// <param name="secretAccessKey">The Secret Access Key associated with the account</param>
        /// <returns>An GrandCloud CS client</returns>
        /// <remarks>
        /// </remarks>
        public static GrandCloudCS CreateGrandCloudCSClient(
            string accessKey,
            string secretAccessKey
            )
        {
            return new GrandCloudCSClient(accessKey, secretAccessKey);
        }

        /// <summary>
        /// Create a client for the GrandCloud CS service with the specified configuration
        /// </summary>
        /// <param name="accessKey">The Access Key associated with the account</param>
        /// <param name="secretAccessKey">The Secret Access Key associated with the account</param>
        /// <param name="config">Configuration options for the service like HTTP Proxy, # of connections, etc
        /// </param>
        /// <returns>An GrandCloud CS client</returns>
        /// <remarks>
        /// </remarks>
        public static GrandCloudCS CreateGrandCloudCSClient(
            string accessKey,
            string secretAccessKey,
            GrandCloudCSConfig config
            )
        {
            return new GrandCloudCSClient(accessKey, secretAccessKey, config);
        }

    }
}