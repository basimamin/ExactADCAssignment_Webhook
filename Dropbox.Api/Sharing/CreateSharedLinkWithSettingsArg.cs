// <auto-generated>
// Auto-generated by BabelAPI, do not modify.
// </auto-generated>

namespace Dropbox.Api.Sharing
{
    using sys = System;
    using col = System.Collections.Generic;
    using re = System.Text.RegularExpressions;

    using enc = Dropbox.Api.Babel;

    /// <summary>
    /// <para>The create shared link with settings arg object</para>
    /// </summary>
    public class CreateSharedLinkWithSettingsArg
    {
        #pragma warning disable 108

        /// <summary>
        /// <para>The encoder instance.</para>
        /// </summary>
        internal static enc.StructEncoder<CreateSharedLinkWithSettingsArg> Encoder = new CreateSharedLinkWithSettingsArgEncoder();

        /// <summary>
        /// <para>The decoder instance.</para>
        /// </summary>
        internal static enc.StructDecoder<CreateSharedLinkWithSettingsArg> Decoder = new CreateSharedLinkWithSettingsArgDecoder();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="CreateSharedLinkWithSettingsArg"
        /// /> class.</para>
        /// </summary>
        /// <param name="path">The path to be shared by the shared link</param>
        /// <param name="settings">The requested settings for the newly created shared
        /// link</param>
        public CreateSharedLinkWithSettingsArg(string path,
                                               SharedLinkSettings settings = null)
        {
            if (path == null)
            {
                throw new sys.ArgumentNullException("path");
            }
            if (!re.Regex.IsMatch(path, @"\A(?:((/|id:).*)|(rev:[0-9a-f]{9,}))\z"))
            {
                throw new sys.ArgumentOutOfRangeException("path", @"Value should match pattern '\A(?:((/|id:).*)|(rev:[0-9a-f]{9,}))\z'");
            }

            this.Path = path;
            this.Settings = settings;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="CreateSharedLinkWithSettingsArg"
        /// /> class.</para>
        /// </summary>
        /// <remarks>This is to construct an instance of the object when
        /// deserializing.</remarks>
        public CreateSharedLinkWithSettingsArg()
        {
        }

        /// <summary>
        /// <para>The path to be shared by the shared link</para>
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// <para>The requested settings for the newly created shared link</para>
        /// </summary>
        public SharedLinkSettings Settings { get; protected set; }

        #region Encoder class

        /// <summary>
        /// <para>Encoder for  <see cref="CreateSharedLinkWithSettingsArg" />.</para>
        /// </summary>
        private class CreateSharedLinkWithSettingsArgEncoder : enc.StructEncoder<CreateSharedLinkWithSettingsArg>
        {
            /// <summary>
            /// <para>Encode fields of given value.</para>
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="writer">The writer.</param>
            public override void EncodeFields(CreateSharedLinkWithSettingsArg value, enc.IJsonWriter writer)
            {
                WriteProperty("path", value.Path, writer, enc.StringEncoder.Instance);
                if (value.Settings != null)
                {
                    WriteProperty("settings", value.Settings, writer, Dropbox.Api.Sharing.SharedLinkSettings.Encoder);
                }
            }
        }

        #endregion


        #region Decoder class

        /// <summary>
        /// <para>Decoder for  <see cref="CreateSharedLinkWithSettingsArg" />.</para>
        /// </summary>
        private class CreateSharedLinkWithSettingsArgDecoder : enc.StructDecoder<CreateSharedLinkWithSettingsArg>
        {
            /// <summary>
            /// <para>Create a new instance of type <see cref="CreateSharedLinkWithSettingsArg"
            /// />.</para>
            /// </summary>
            /// <returns>The struct instance.</returns>
            protected override CreateSharedLinkWithSettingsArg Create()
            {
                return new CreateSharedLinkWithSettingsArg();
            }

            /// <summary>
            /// <para>Set given field.</para>
            /// </summary>
            /// <param name="value">The field value.</param>
            /// <param name="fieldName">The field name.</param>
            /// <param name="reader">The json reader.</param>
            protected override void SetField(CreateSharedLinkWithSettingsArg value, string fieldName, enc.IJsonReader reader)
            {
                switch (fieldName)
                {
                    case "path":
                        value.Path = enc.StringDecoder.Instance.Decode(reader);
                        break;
                    case "settings":
                        value.Settings = Dropbox.Api.Sharing.SharedLinkSettings.Decoder.Decode(reader);
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
