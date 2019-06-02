namespace Shared.Framework.Security
{
    /// <summary>
    /// Identity storage contract.
    /// </summary>
    public interface IIdentityStorage
    {
        /// <summary>
        /// Sets given identity.
        /// </summary>
        /// <param name="userInfo"></param>
        void SaveIdentity(UserInfo userInfo);

        /// <summary>
        /// Clears principal info.
        /// </summary>
        void ClearIdentity();

        /// <summary>
        /// Retrieves current principal information
        /// </summary>
        /// <returns></returns>
        IAutoGenePrincipal GetPrincipal();
    }
}