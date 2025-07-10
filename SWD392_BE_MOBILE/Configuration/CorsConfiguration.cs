namespace SWD392_BE_MOBILE.Configuration
{
    /// <summary>
    /// CORS Configuration - matching Java project SecurityConfig
    /// </summary>
    public static class CorsConfiguration
    {
        public const string AllowAllPolicy = "AllowAll";
        public const string AllowCredentialsPolicy = "AllowCredentials";
        
        /// <summary>
        /// Configure CORS policies to match Java project configuration
        /// Java equivalent: corsConfigurationSource() method in SecurityConfig.java
        /// </summary>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                // Policy 1: Allow all origins, methods, headers (no credentials)
                // Best for public APIs that don't need authentication
                options.AddPolicy(AllowAllPolicy, policy =>
                {
                    policy.AllowAnyOrigin()        // Java: setAllowedOriginPatterns(List.of("*"))
                          .AllowAnyMethod()        // Java: setAllowedMethods(List.of("*"))
                          .AllowAnyHeader();       // Java: setAllowedHeaders(List.of("*"))
                });
                
                // Policy 2: Allow credentials with any origin (matches Java exactly)
                // Java: setAllowCredentials(true) + setAllowedOriginPatterns(List.of("*"))
                options.AddPolicy(AllowCredentialsPolicy, policy =>
                {
                    policy.SetIsOriginAllowed(origin => true)  // Allow any origin (equivalent to AllowedOriginPatterns("*"))
                          .AllowAnyMethod()                    // Java: setAllowedMethods(List.of("*"))
                          .AllowAnyHeader()                    // Java: setAllowedHeaders(List.of("*"))
                          .AllowCredentials();                 // Java: setAllowCredentials(true)
                });
                
                // Policy 3: Specific origins for production (recommended)
                options.AddPolicy("Production", policy =>
                {
                    policy.WithOrigins(
                              "http://localhost:3000",      // React dev server
                              "http://localhost:3001",      // Alternative React port
                              "http://localhost:4200",      // Angular dev server
                              "https://yourdomain.com",     // Production domain
                              "https://www.yourdomain.com"  // Production www domain
                          )
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });
        }
        
        /// <summary>
        /// Apply CORS middleware to the application pipeline
        /// </summary>
        public static void UseCorsConfiguration(this WebApplication app)
        {
            // Use the policy that matches Java configuration
            app.UseCors(AllowCredentialsPolicy);
        }
    }
}
