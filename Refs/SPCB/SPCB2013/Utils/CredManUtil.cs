using CredentialManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Represents utility class for Windows Credential Manager (CredMan).
    /// </summary>
    /// <seealso cref="http://credentialmanagement.codeplex.com/"/>
    /// <seealso cref="https://www.nuget.org/packages/CredentialManagement/"/>
    class CredManUtil
    {
        private const CredentialType CREDENTIAL_TYPE = CredentialType.Generic;

        /// <summary>
        /// Tries to retrieve the password from the Credential Manager (CredMan).
        /// </summary>
        /// <param name="url">URL of site or tenant related to the password.</param>
        /// <param name="password">Retrieved password when successfull.</param>
        /// <param name="storageType">What type is stored?</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool TryGetPassword(string url, out string password, StorageType storageType)
        {
            bool isSuccess = false;
            Credential credential = null;
            password = string.Empty;

            try
            {
                // Try to retrieve credentials
                credential = new Credential { Target = GetTargetKey(url, storageType), Type = CREDENTIAL_TYPE };

                if (credential.Load())
                {
                    password = credential.Password;
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while trying to retrieve credentials for {0}.", url), ex);
            }
            finally
            {
                if (credential != null)
                    credential.Dispose();
            }

            return isSuccess;
        }

        /// <summary>
        /// Saves or updates the credential in the Credential Manager (CredMan).
        /// </summary>
        /// <param name="url">URL of site or tenant to save.</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="storageType">What type is stored?</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool SaveCredentials(string url, string username, string password, StorageType storageType)
        {
            bool isSuccess = false;
            Credential credential = null;

            try
            {
                // Save credentials
                credential = new Credential(username, password, GetTargetKey(url, storageType), CREDENTIAL_TYPE);
                credential.PersistanceType = PersistanceType.LocalComputer;
                credential.Description = "Credentials stored by SharePoint Client Browser";

                isSuccess = credential.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while trying to save credentials for {0}.", url), ex);
            }
            finally
            {
                if (credential != null)
                    credential.Dispose();
            }

            return isSuccess;
        }

        /// <summary>
        /// Deletes credential in Credential Manager (CredMan).
        /// </summary>
        /// <param name="url">URL of site or tenant to delete.</param>
        /// <param name="storageType">What type is stored?</param>
        /// <returns>Returns true if successful, otherwise false.</returns>
        public static bool DeleteCredentials(string url, StorageType storageType)
        {
            bool isSuccess = false;
            Credential credential = null;

            try
            {
                // Delete credentials
                credential = new Credential { Target = GetTargetKey(url, storageType), Type = CREDENTIAL_TYPE };
                if (credential.Exists())
                {
                    isSuccess = credential.Delete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while trying to delete credentials for {0}.", url), ex);
            }
            finally
            {
                if (credential != null)
                    credential.Dispose();
            }

            return isSuccess;
        }

        private static string GetTargetKey(string url, StorageType storageType)
        {
            return string.Format("SPCB.{0}:{1}", storageType, url);
        }
    }

    /// <summary>
    /// Indicates the storage type for the Credential Manager, used for prefixing the target.
    /// </summary>
    public enum StorageType
    {
        SiteCollection,
        Tenant
    }
}
