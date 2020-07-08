namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserListRequest : MicModel
    {
        /// <summary>
        /// The attributes you want to get.
        /// </summary>
        public MicUserAttributes? Attributes { get; set; }
        /// <summary>
        /// A string to filter results on.
        /// </summary>
        public string? FreeTextFilter { get; set; }
        /// <summary>
        /// Filter for the category of the user. Defaults to <see cref="MicUserCategory.Active"/>.
        /// </summary>
        public MicUserCategory? Category { get; set; }
        /// <summary>
        /// Page to list, i.e. pagination support.
        /// </summary>
        public int? Page { get; set; }
        /// <summary>
        /// Number of users to retrieve per page. If not set all users are returned.
        /// </summary>
        public int? Size { get; set; }
        /// <summary>
        /// Name of attribute to sort by, defaults to the user name.
        /// </summary>
        public string? SortProp { get; set; }
    }
}
