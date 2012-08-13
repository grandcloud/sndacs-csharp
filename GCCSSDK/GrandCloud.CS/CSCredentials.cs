
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;


namespace GrandCloud.CS
{
    /// <summary>
    /// Immutable representation of GrandCloud credentials.
    /// </summary>
    public class ImmutableCredentials : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the AccessKey property for the current credentials.
        /// </summary>
        public string AccessKey { get; private set; }

        /// <summary>
        /// Gets the ClearSecretKey property for the current credentials.
        /// </summary>
        public string ClearSecretKey { get; private set; }

        /// <summary>
        /// Gets the SecureSecretKey property for the current credentials.
        /// </summary>
        public SecureString SecureSecretKey { get; private set; }

        /// <summary>
        /// Gets the UseSecureStringForSecretKey property for the current credentials.
        /// Specifies if secretKey is stored as SecureString or String.
        /// </summary>
        public bool UseSecureStringForSecretKey { get { return SecureSecretKey != null; } }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructs an ImmutableCredentials object with supplied accessKey, secretKey,
        /// optional token and flag to determine if secretKey should be stored in a SecureString
        /// </summary>
        /// <param name="csAccessKeyId"></param>
        /// <param name="csSecretAccessKey"></param>
        /// <param name="token">Optional. Can be set to null or empty for non-session credentials.</param>
        /// <param name="useSecureStringForSecretKey">
        /// True if secretKey should be stored in SecureString. False if secretKey should be stored as clear string.
        /// </param>
        public ImmutableCredentials(string csAccessKeyId, string csSecretAccessKey, string token, bool useSecureStringForSecretKey)
        {
            if (string.IsNullOrEmpty(csAccessKeyId)) throw new ArgumentNullException("csAccessKeyId");
            if (string.IsNullOrEmpty(csSecretAccessKey)) throw new ArgumentNullException("csSecretAccessKey");

            AccessKey = csAccessKeyId;
            if (useSecureStringForSecretKey)
            {
                SetSecretAccessKey(csSecretAccessKey);
                ClearSecretKey = null;
            }
            else
            {
                ClearSecretKey = csSecretAccessKey;
                SecureSecretKey = null;
            }
        }

        /// <summary>
        /// Constructs an ImmutableCredentials object with supplied accessKey, secretKey as a SecureString
        /// and an optional session token.
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="token">Optional. Can be set to null or empty for non-session credentials.</param>
        public ImmutableCredentials(string accessKey, SecureString secretKey, string token)
        {
            if (string.IsNullOrEmpty(accessKey)) throw new ArgumentNullException("accessKey");
            if (secretKey == null) throw new ArgumentNullException("secretKey");

            AccessKey = accessKey;
            ClearSecretKey = null;
            SecureSecretKey = secretKey.Copy();
        }
        private ImmutableCredentials() { }

        #endregion


        #region Public methods

        /// <summary>
        /// Returns a copy of the current credentials.
        /// </summary>
        /// <returns></returns>
        public ImmutableCredentials Copy()
        {
            ImmutableCredentials credentials = new ImmutableCredentials
            {
                AccessKey = this.AccessKey,
                ClearSecretKey = this.ClearSecretKey,
                SecureSecretKey = (this.SecureSecretKey == null ? null : this.SecureSecretKey.Copy()),
            };
            return credentials;
        }

        #endregion


        #region Private methods

        // Sets SecureSecretKey equal to the passed-in string.
        private void SetSecretAccessKey(string secretAccessKey)
        {
            SecureSecretKey = new SecureString();
            foreach (char ch in secretAccessKey.ToCharArray())
            {
                SecureSecretKey.AppendChar(ch);
            }
            SecureSecretKey.MakeReadOnly();
        }

        #endregion


        #region IDisposable Members

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (SecureSecretKey != null)
                    {
                        SecureSecretKey.Dispose();
                        SecureSecretKey = null;
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// Abstract class that represents a credentials object for AWS services.
    /// </summary>
    public abstract class CSCredentials : IDisposable
    {
        /// <summary>
        /// Returns a copy of ImmutableCredentials
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableCredentials GetCredentials();

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }

    /// <summary>
    /// Basic set of credentials consisting of an AccessKey and SecretKey
    /// </summary>
    public class BasicCSCredentials : CSCredentials
    {
        #region Private members

        private ImmutableCredentials _credentials;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructs a BasicCSCredentials object for the specified accessKey and secretKey
        /// SecretKey is stored as a clear string
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public BasicCSCredentials(string accessKey, string secretKey)
            : this(accessKey, secretKey, false) { }

        /// <summary>
        /// Constructs a BasicCSCredentials object for the specified accessKey and secretKey,
        /// with the useSecureString flag to signal if the secretKey should be stored as SecureString
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="useSecureString">
        /// True if secretKey should be stored in SecureString. False if secretKey should be stored as clear string.
        /// </param>
        public BasicCSCredentials(string accessKey, string secretKey, bool useSecureString)
        {
            _credentials = new ImmutableCredentials(accessKey, secretKey, null, useSecureString);
        }

        /// <summary>
        /// Constructs a BasicCSCredentials object for the specified accessKey and secretKey
        /// SecretKey is stored in SecureString
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public BasicCSCredentials(string accessKey, SecureString secretKey)
        {
            _credentials = new ImmutableCredentials(accessKey, secretKey, null);
        }

        #endregion


        #region Abstract class overrides

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            return _credentials.Copy();
        }

        #endregion


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_credentials != null)
                    {
                        _credentials.Dispose();
                        _credentials = null;
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// Credentials that are retrieved from ConfigurationManager.AppSettings
    /// </summary>
    public class EnvironmentCSCredentials : CSCredentials
    {
        #region Private members

        private const string ACCESSKEY = "CSAccessKey";
        private const string SECRETKEY = "CSSecretKey";

        private ImmutableCredentials _wrappedCredentials;

        #endregion


        #region Public constructors

        /// <summary>
        /// Constructs an instance of EnvironmentCSCredentials and attempts
        /// to load AccessKey and SecretKey from ConfigurationManager.AppSettings
        /// </summary>
        public EnvironmentCSCredentials()
        {
            //NameValueCollection appConfig = ConfigurationManager.AppSettings;
            NameValueCollection appConfig = null;
            var accessKey = appConfig[ACCESSKEY];
            var secretKey = appConfig[SECRETKEY];

            if (string.IsNullOrEmpty(accessKey))
                throw new ArgumentException(string.Format("Access Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your access key.", ACCESSKEY));
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentException(string.Format("Secret Key could not be found.  Add an appsetting to your App.config with the name {0} with a value of your secret key.", SECRETKEY));

            this._wrappedCredentials = new ImmutableCredentials(accessKey, secretKey, null, false);
        }

        #endregion


        #region Abstract class overrides

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            return this._wrappedCredentials.Copy();
        }

        #endregion


        #region IDisposable Members

        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_wrappedCredentials != null)
                    {
                        _wrappedCredentials.Dispose();
                        _wrappedCredentials = null;
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }


    // Credentials fallback mechanism
    internal static class FallbackCredentialsFactory
    {
        static FallbackCredentialsFactory()
        {
            Reset();
        }

        public delegate CSCredentials CredentialsGenerator();
        public static List<CredentialsGenerator> CredentialsGenerators { get; set; }
        public static void Reset()
        {
            CredentialsGenerators = new List<CredentialsGenerator>
            {
                () => new EnvironmentCSCredentials()               
            };
        }

        internal static CSCredentials GetCredentials()
        {
            CSCredentials credentials = null;

            foreach (CredentialsGenerator generator in CredentialsGenerators)
            {
                try
                {
                    credentials = generator();
                }
                catch
                {
                    credentials = null;
                }

                if (credentials != null)
                    return credentials;
            }

            if (credentials == null)
                throw new GrandCloudServiceException("Unable to find credentials");

            return credentials;
        }
    }
}
