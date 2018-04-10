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
    /// <para>The list folders args object</para>
    /// </summary>
    public class ListFoldersArgs
    {
        #pragma warning disable 108

        /// <summary>
        /// <para>The encoder instance.</para>
        /// </summary>
        internal static enc.StructEncoder<ListFoldersArgs> Encoder = new ListFoldersArgsEncoder();

        /// <summary>
        /// <para>The decoder instance.</para>
        /// </summary>
        internal static enc.StructDecoder<ListFoldersArgs> Decoder = new ListFoldersArgsDecoder();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ListFoldersArgs" />
        /// class.</para>
        /// </summary>
        /// <param name="limit">The maximum number of results to return per request.</param>
        /// <param name="actions">Folder actions to query.</param>
        public ListFoldersArgs(uint limit = 1000,
                               col.IEnumerable<FolderAction> actions = null)
        {
            if (limit < 1U)
            {
                throw new sys.ArgumentOutOfRangeException("limit", "Value should be greater or equal than 1");
            }
            if (limit > 1000U)
            {
                throw new sys.ArgumentOutOfRangeException("limit", "Value should be less of equal than 1000");
            }

            var actionsList = enc.Util.ToList(actions);

            this.Limit = limit;
            this.Actions = actionsList;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ListFoldersArgs" />
        /// class.</para>
        /// </summary>
        /// <remarks>This is to construct an instance of the object when
        /// deserializing.</remarks>
        public ListFoldersArgs()
        {
            this.Limit = 1000;
        }

        /// <summary>
        /// <para>The maximum number of results to return per request.</para>
        /// </summary>
        public uint Limit { get; protected set; }

        /// <summary>
        /// <para>Folder actions to query.</para>
        /// </summary>
        public col.IList<FolderAction> Actions { get; protected set; }

        #region Encoder class

        /// <summary>
        /// <para>Encoder for  <see cref="ListFoldersArgs" />.</para>
        /// </summary>
        private class ListFoldersArgsEncoder : enc.StructEncoder<ListFoldersArgs>
        {
            /// <summary>
            /// <para>Encode fields of given value.</para>
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="writer">The writer.</param>
            public override void EncodeFields(ListFoldersArgs value, enc.IJsonWriter writer)
            {
                WriteProperty("limit", value.Limit, writer, enc.UInt32Encoder.Instance);
                if (value.Actions.Count > 0)
                {
                    WriteListProperty("actions", value.Actions, writer, Dropbox.Api.Sharing.FolderAction.Encoder);
                }
            }
        }

        #endregion


        #region Decoder class

        /// <summary>
        /// <para>Decoder for  <see cref="ListFoldersArgs" />.</para>
        /// </summary>
        private class ListFoldersArgsDecoder : enc.StructDecoder<ListFoldersArgs>
        {
            /// <summary>
            /// <para>Create a new instance of type <see cref="ListFoldersArgs" />.</para>
            /// </summary>
            /// <returns>The struct instance.</returns>
            protected override ListFoldersArgs Create()
            {
                return new ListFoldersArgs();
            }

            /// <summary>
            /// <para>Set given field.</para>
            /// </summary>
            /// <param name="value">The field value.</param>
            /// <param name="fieldName">The field name.</param>
            /// <param name="reader">The json reader.</param>
            protected override void SetField(ListFoldersArgs value, string fieldName, enc.IJsonReader reader)
            {
                switch (fieldName)
                {
                    case "limit":
                        value.Limit = enc.UInt32Decoder.Instance.Decode(reader);
                        break;
                    case "actions":
                        value.Actions = ReadList<FolderAction>(reader, Dropbox.Api.Sharing.FolderAction.Decoder);
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
