// <auto-generated>
// Auto-generated by BabelAPI, do not modify.
// </auto-generated>

namespace Dropbox.Api.Team
{
    using sys = System;
    using col = System.Collections.Generic;
    using re = System.Text.RegularExpressions;

    using enc = Dropbox.Api.Babel;

    /// <summary>
    /// <para>Basic member profile.</para>
    /// </summary>
    /// <seealso cref="GroupMemberInfo" />
    /// <seealso cref="TeamMemberProfile" />
    public class MemberProfile
    {
        #pragma warning disable 108

        /// <summary>
        /// <para>The encoder instance.</para>
        /// </summary>
        internal static enc.StructEncoder<MemberProfile> Encoder = new MemberProfileEncoder();

        /// <summary>
        /// <para>The decoder instance.</para>
        /// </summary>
        internal static enc.StructDecoder<MemberProfile> Decoder = new MemberProfileDecoder();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MemberProfile" /> class.</para>
        /// </summary>
        /// <param name="teamMemberId">ID of user as a member of a team.</param>
        /// <param name="email">Email address of user.</param>
        /// <param name="emailVerified">Is true if the user's email is verified to be owned by
        /// the user.</param>
        /// <param name="status">The user's status as a member of a specific team.</param>
        /// <param name="name">Representations for a person's name.</param>
        /// <param name="externalId">External ID that a team can attach to the user. An
        /// application using the API may find it easier to use their own IDs instead of
        /// Dropbox IDs like account_id or team_member_id.</param>
        public MemberProfile(string teamMemberId,
                             string email,
                             bool emailVerified,
                             TeamMemberStatus status,
                             Dropbox.Api.Users.Name name,
                             string externalId = null)
        {
            if (teamMemberId == null)
            {
                throw new sys.ArgumentNullException("teamMemberId");
            }

            if (email == null)
            {
                throw new sys.ArgumentNullException("email");
            }

            if (status == null)
            {
                throw new sys.ArgumentNullException("status");
            }

            if (name == null)
            {
                throw new sys.ArgumentNullException("name");
            }

            this.TeamMemberId = teamMemberId;
            this.Email = email;
            this.EmailVerified = emailVerified;
            this.Status = status;
            this.Name = name;
            this.ExternalId = externalId;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MemberProfile" /> class.</para>
        /// </summary>
        /// <remarks>This is to construct an instance of the object when
        /// deserializing.</remarks>
        public MemberProfile()
        {
        }

        /// <summary>
        /// <para>ID of user as a member of a team.</para>
        /// </summary>
        public string TeamMemberId { get; protected set; }

        /// <summary>
        /// <para>Email address of user.</para>
        /// </summary>
        public string Email { get; protected set; }

        /// <summary>
        /// <para>Is true if the user's email is verified to be owned by the user.</para>
        /// </summary>
        public bool EmailVerified { get; protected set; }

        /// <summary>
        /// <para>The user's status as a member of a specific team.</para>
        /// </summary>
        public TeamMemberStatus Status { get; protected set; }

        /// <summary>
        /// <para>Representations for a person's name.</para>
        /// </summary>
        public Dropbox.Api.Users.Name Name { get; protected set; }

        /// <summary>
        /// <para>External ID that a team can attach to the user. An application using the API
        /// may find it easier to use their own IDs instead of Dropbox IDs like account_id or
        /// team_member_id.</para>
        /// </summary>
        public string ExternalId { get; protected set; }

        #region Encoder class

        /// <summary>
        /// <para>Encoder for  <see cref="MemberProfile" />.</para>
        /// </summary>
        private class MemberProfileEncoder : enc.StructEncoder<MemberProfile>
        {
            /// <summary>
            /// <para>Encode fields of given value.</para>
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="writer">The writer.</param>
            public override void EncodeFields(MemberProfile value, enc.IJsonWriter writer)
            {
                WriteProperty("team_member_id", value.TeamMemberId, writer, enc.StringEncoder.Instance);
                WriteProperty("email", value.Email, writer, enc.StringEncoder.Instance);
                WriteProperty("email_verified", value.EmailVerified, writer, enc.BooleanEncoder.Instance);
                WriteProperty("status", value.Status, writer, Dropbox.Api.Team.TeamMemberStatus.Encoder);
                WriteProperty("name", value.Name, writer, Dropbox.Api.Users.Name.Encoder);
                if (value.ExternalId != null)
                {
                    WriteProperty("external_id", value.ExternalId, writer, enc.StringEncoder.Instance);
                }
            }
        }

        #endregion


        #region Decoder class

        /// <summary>
        /// <para>Decoder for  <see cref="MemberProfile" />.</para>
        /// </summary>
        private class MemberProfileDecoder : enc.StructDecoder<MemberProfile>
        {
            /// <summary>
            /// <para>Create a new instance of type <see cref="MemberProfile" />.</para>
            /// </summary>
            /// <returns>The struct instance.</returns>
            protected override MemberProfile Create()
            {
                return new MemberProfile();
            }

            /// <summary>
            /// <para>Set given field.</para>
            /// </summary>
            /// <param name="value">The field value.</param>
            /// <param name="fieldName">The field name.</param>
            /// <param name="reader">The json reader.</param>
            protected override void SetField(MemberProfile value, string fieldName, enc.IJsonReader reader)
            {
                switch (fieldName)
                {
                    case "team_member_id":
                        value.TeamMemberId = enc.StringDecoder.Instance.Decode(reader);
                        break;
                    case "email":
                        value.Email = enc.StringDecoder.Instance.Decode(reader);
                        break;
                    case "email_verified":
                        value.EmailVerified = enc.BooleanDecoder.Instance.Decode(reader);
                        break;
                    case "status":
                        value.Status = Dropbox.Api.Team.TeamMemberStatus.Decoder.Decode(reader);
                        break;
                    case "name":
                        value.Name = Dropbox.Api.Users.Name.Decoder.Decode(reader);
                        break;
                    case "external_id":
                        value.ExternalId = enc.StringDecoder.Instance.Decode(reader);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }

        #endregion
    }
}
