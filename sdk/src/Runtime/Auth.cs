namespace Omnigage.Runtime
{
    /// <summary>
    /// Authentication and request context
    /// </summary>
    public class AuthContext
    {
        public string JWT { get; set; }
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
                if (this.JWT != null)
                {
                    return $"Bearer {this.JWT}";
                }

                byte[] authBytes = System.Text.Encoding.UTF8.GetBytes($"{this.TokenKey}:{this.TokenSecret}");
                return $"Basic {System.Convert.ToBase64String(authBytes)}";
            }
        }
    }
}