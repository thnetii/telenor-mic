namespace TelenorConnexion.ManagedIoTCloud.Model
{
    /// <summary>
    /// Represents basic details of the logged in user returned from a
    /// successful login operation.
    /// </summary>
    public interface IMicAuthUserDetails : IMicUserBasicDetails
    {
    }

    /// <inheritdoc />
    public class MicAuthUserDetails : MicUserBasicDetails, IMicAuthUserDetails
    {
    }
}
