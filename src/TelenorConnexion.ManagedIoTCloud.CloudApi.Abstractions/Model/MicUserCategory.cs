using System.Runtime.Serialization;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    /// <summary/>
    public enum MicUserCategory
    {
        /// <summary/>
        [EnumMember(Value = "all")]
        All,
        /// <summary>
        /// Active users have access to login.
        /// </summary>
        [EnumMember(Value = "active")]
        Active,
        /// <summary>
        /// Pending users have not been assigned domain name and role name.
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending,
        /// <summary>
        /// Unconfirmed users have signed up but not visited the link in the confirmation email and set a new password.
        /// </summary>
        [EnumMember(Value = "unconfirmed")]
        Unconfirmed
    }
}
