namespace Services.Objects
{
    /// <summary>
    /// User Object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password (already encrypted).
        /// </summary>
        public string Password { get; set; }
    }
}