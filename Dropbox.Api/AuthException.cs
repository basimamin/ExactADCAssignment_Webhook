// <auto-generated>
// Auto-generated by BabelAPI, do not modify.
// </auto-generated>

namespace Dropbox.Api
{
    using sys = System;

    using Dropbox.Api.Auth;

    /// <summary>
    /// <para>An HTTP exception that is caused by the server reporting an authentication
    /// problem.</para>
    /// </summary>
    public sealed partial class AuthException : StructuredException<AuthError>
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AuthException"/> class.</para>
        /// </summary>
        public AuthException()
        {
        }

        /// <summary>
        /// <para>Decode from given json.</para>
        /// </summary>
        internal static AuthException Decode(string json)
        {
            return StructuredException<AuthError>.Decode<AuthException>(json, AuthError.Decoder);
        }
    }
}
