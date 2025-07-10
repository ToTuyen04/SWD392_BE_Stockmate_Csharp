using Microsoft.AspNetCore.Cors;

namespace SWD392_BE_MOBILE.Attributes
{
    /// <summary>
    /// Custom CORS attribute for easy application to controllers
    /// Matches Java @CrossOrigin annotation functionality
    /// </summary>
    public class AllowCorsAttribute : EnableCorsAttribute
    {
        public AllowCorsAttribute() : base("AllowCredentials")
        {
        }
    }
    
    /// <summary>
    /// CORS attribute for public APIs (no credentials)
    /// </summary>
    public class AllowPublicCorsAttribute : EnableCorsAttribute
    {
        public AllowPublicCorsAttribute() : base("AllowAll")
        {
        }
    }
}
