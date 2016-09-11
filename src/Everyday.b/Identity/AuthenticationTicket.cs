using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;

namespace Everyday.b.Identity
{
    /// <summary>
    /// Contains user identity information as well as additional authentication state.
    /// </summary>
    public class AuthenticationTicket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationTicket"/> class
        /// </summary>
        /// <param name="principal">the <see cref="ClaimsPrincipal"/> that represents the authenticated user.</param>
        /// <param name="properties">additional properties that can be consumed by the user or runtime.</param>
        /// <param name="authenticationScheme">the authentication middleware that was responsible for this ticket.</param>
        public AuthenticationTicket(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            AuthenticationScheme = authenticationScheme;
            Principal = principal;
            Properties = properties ?? new AuthenticationProperties();
        }

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        public string AuthenticationScheme { get; private set; }

        /// <summary>
        /// Gets the claims-principal with authenticated user identities.
        /// </summary>
        public ClaimsPrincipal Principal { get; private set; }

        /// <summary>
        /// Additional state values for the authentication session.
        /// </summary>
        public AuthenticationProperties Properties { get; private set; }
    }
}
