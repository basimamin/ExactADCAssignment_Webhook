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
    /// <para>The members add arg object</para>
    /// </summary>
    public class MembersAddArg
    {
        #pragma warning disable 108

        /// <summary>
        /// <para>The encoder instance.</para>
        /// </summary>
        internal static enc.StructEncoder<MembersAddArg> Encoder = new MembersAddArgEncoder();

        /// <summary>
        /// <para>The decoder instance.</para>
        /// </summary>
        internal static enc.StructDecoder<MembersAddArg> Decoder = new MembersAddArgDecoder();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MembersAddArg" /> class.</para>
        /// </summary>
        /// <param name="newMembers">Details of new members to be added to the team.</param>
        /// <param name="forceAsync">Whether to force the add to happen asynchronously.</param>
        public MembersAddArg(col.IEnumerable<MemberAddArg> newMembers,
                             bool forceAsync = false)
        {
            var newMembersList = enc.Util.ToList(newMembers);

            if (newMembers == null)
            {
                throw new sys.ArgumentNullException("newMembers");
            }

            this.NewMembers = newMembersList;
            this.ForceAsync = forceAsync;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="MembersAddArg" /> class.</para>
        /// </summary>
        /// <remarks>This is to construct an instance of the object when
        /// deserializing.</remarks>
        public MembersAddArg()
        {
            this.ForceAsync = false;
        }

        /// <summary>
        /// <para>Details of new members to be added to the team.</para>
        /// </summary>
        public col.IList<MemberAddArg> NewMembers { get; protected set; }

        /// <summary>
        /// <para>Whether to force the add to happen asynchronously.</para>
        /// </summary>
        public bool ForceAsync { get; protected set; }

        #region Encoder class

        /// <summary>
        /// <para>Encoder for  <see cref="MembersAddArg" />.</para>
        /// </summary>
        private class MembersAddArgEncoder : enc.StructEncoder<MembersAddArg>
        {
            /// <summary>
            /// <para>Encode fields of given value.</para>
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="writer">The writer.</param>
            public override void EncodeFields(MembersAddArg value, enc.IJsonWriter writer)
            {
                WriteListProperty("new_members", value.NewMembers, writer, Dropbox.Api.Team.MemberAddArg.Encoder);
                WriteProperty("force_async", value.ForceAsync, writer, enc.BooleanEncoder.Instance);
            }
        }

        #endregion


        #region Decoder class

        /// <summary>
        /// <para>Decoder for  <see cref="MembersAddArg" />.</para>
        /// </summary>
        private class MembersAddArgDecoder : enc.StructDecoder<MembersAddArg>
        {
            /// <summary>
            /// <para>Create a new instance of type <see cref="MembersAddArg" />.</para>
            /// </summary>
            /// <returns>The struct instance.</returns>
            protected override MembersAddArg Create()
            {
                return new MembersAddArg();
            }

            /// <summary>
            /// <para>Set given field.</para>
            /// </summary>
            /// <param name="value">The field value.</param>
            /// <param name="fieldName">The field name.</param>
            /// <param name="reader">The json reader.</param>
            protected override void SetField(MembersAddArg value, string fieldName, enc.IJsonReader reader)
            {
                switch (fieldName)
                {
                    case "new_members":
                        value.NewMembers = ReadList<MemberAddArg>(reader, Dropbox.Api.Team.MemberAddArg.Decoder);
                        break;
                    case "force_async":
                        value.ForceAsync = enc.BooleanDecoder.Instance.Decode(reader);
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
