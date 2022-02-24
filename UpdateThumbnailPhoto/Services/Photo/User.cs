namespace UpdateThumbnailPhoto.Services.Photo
{
    using System;
    using System.Text.Json.Serialization;

    /// <summary>
    /// A user object from the Azure AD graph
    /// </summary>
    internal class User
    {
        /// <summary>
        /// Gets or sets the user's Object ID
        /// </summary>
        [JsonPropertyName("objectId")]
        public string ObjectId { get; set; }

        /// <summary>
        /// Get or sets the user's user principal name (UPN)
        /// </summary>
        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [JsonPropertyName("mail")]
        public string Mail { get; set; }
    }
}
