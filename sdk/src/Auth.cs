namespace Omnigage.Auth
{
    /// <summary>
    /// Authentication and request context
    /// </summary>
    public class AuthContext
    {
        public string TokenKey { get; set; }
        public string TokenSecret { get; set; }

        /// <summary>
        /// Create Authorization token following RFC 2617 
        /// </summary>
        /// <returns>Base64 encoded string</returns>
        public string Authorization
        {
            get
            {
                byte[] authBytes = System.Text.Encoding.UTF8.GetBytes($"{this.TokenKey}:{this.TokenSecret}");
                return System.Convert.ToBase64String(authBytes);
            }
        }
    }
}