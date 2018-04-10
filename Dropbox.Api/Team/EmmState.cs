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
    /// <para>The emm state object</para>
    /// </summary>
    public class EmmState
    {
        #pragma warning disable 108

        /// <summary>
        /// <para>The encoder instance.</para>
        /// </summary>
        internal static enc.StructEncoder<EmmState> Encoder = new EmmStateEncoder();

        /// <summary>
        /// <para>The decoder instance.</para>
        /// </summary>
        internal static enc.StructDecoder<EmmState> Decoder = new EmmStateDecoder();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="EmmState" /> class.</para>
        /// </summary>
        public EmmState()
        {
        }

        /// <summary>
        /// <para>Gets a value indicating whether this instance is Disabled</para>
        /// </summary>
        public bool IsDisabled
        {
            get
            {
                return this is Disabled;
            }
        }

        /// <summary>
        /// <para>Gets this instance as a Disabled, or <c>null</c>.</para>
        /// </summary>
        public Disabled AsDisabled
        {
            get
            {
                return this as Disabled;
            }
        }

        /// <summary>
        /// <para>Gets a value indicating whether this instance is Optional</para>
        /// </summary>
        public bool IsOptional
        {
            get
            {
                return this is Optional;
            }
        }

        /// <summary>
        /// <para>Gets this instance as a Optional, or <c>null</c>.</para>
        /// </summary>
        public Optional AsOptional
        {
            get
            {
                return this as Optional;
            }
        }

        /// <summary>
        /// <para>Gets a value indicating whether this instance is Required</para>
        /// </summary>
        public bool IsRequired
        {
            get
            {
                return this is Required;
            }
        }

        /// <summary>
        /// <para>Gets this instance as a Required, or <c>null</c>.</para>
        /// </summary>
        public Required AsRequired
        {
            get
            {
                return this as Required;
            }
        }

        /// <summary>
        /// <para>Gets a value indicating whether this instance is Other</para>
        /// </summary>
        public bool IsOther
        {
            get
            {
                return this is Other;
            }
        }

        /// <summary>
        /// <para>Gets this instance as a Other, or <c>null</c>.</para>
        /// </summary>
        public Other AsOther
        {
            get
            {
                return this as Other;
            }
        }

        #region Encoder class

        /// <summary>
        /// <para>Encoder for  <see cref="EmmState" />.</para>
        /// </summary>
        private class EmmStateEncoder : enc.StructEncoder<EmmState>
        {
            /// <summary>
            /// <para>Encode fields of given value.</para>
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="writer">The writer.</param>
            public override void EncodeFields(EmmState value, enc.IJsonWriter writer)
            {
                if (value is Disabled)
                {
                    WriteProperty(".tag", "disabled", writer, enc.StringEncoder.Instance);
                    Disabled.Encoder.EncodeFields((Disabled)value, writer);
                    return;
                }
                if (value is Optional)
                {
                    WriteProperty(".tag", "optional", writer, enc.StringEncoder.Instance);
                    Optional.Encoder.EncodeFields((Optional)value, writer);
                    return;
                }
                if (value is Required)
                {
                    WriteProperty(".tag", "required", writer, enc.StringEncoder.Instance);
                    Required.Encoder.EncodeFields((Required)value, writer);
                    return;
                }
                if (value is Other)
                {
                    WriteProperty(".tag", "other", writer, enc.StringEncoder.Instance);
                    Other.Encoder.EncodeFields((Other)value, writer);
                    return;
                }
                throw new sys.InvalidOperationException();
            }
        }

        #endregion

        #region Decoder class

        /// <summary>
        /// <para>Decoder for  <see cref="EmmState" />.</para>
        /// </summary>
        private class EmmStateDecoder : enc.UnionDecoder<EmmState>
        {
            /// <summary>
            /// <para>Create a new instance of type <see cref="EmmState" />.</para>
            /// </summary>
            /// <returns>The struct instance.</returns>
            protected override EmmState Create()
            {
                return new EmmState();
            }

            /// <summary>
            /// <para>Decode based on given tag.</para>
            /// </summary>
            /// <param name="tag">The tag.</param>
            /// <param name="reader">The json reader.</param>
            /// <returns>The decoded object.</returns>
            protected override EmmState Decode(string tag, enc.IJsonReader reader)
            {
                switch (tag)
                {
                    case "disabled":
                        return Disabled.Decoder.DecodeFields(reader);
                    case "optional":
                        return Optional.Decoder.DecodeFields(reader);
                    case "required":
                        return Required.Decoder.DecodeFields(reader);
                    default:
                        return Other.Decoder.DecodeFields(reader);
                }
            }
        }

        #endregion

        /// <summary>
        /// <para>Emm token is disabled</para>
        /// </summary>
        public sealed class Disabled : EmmState
        {
            #pragma warning disable 108

            /// <summary>
            /// <para>The encoder instance.</para>
            /// </summary>
            internal static enc.StructEncoder<Disabled> Encoder = new DisabledEncoder();

            /// <summary>
            /// <para>The decoder instance.</para>
            /// </summary>
            internal static enc.StructDecoder<Disabled> Decoder = new DisabledDecoder();

            /// <summary>
            /// <para>Initializes a new instance of the <see cref="Disabled" /> class.</para>
            /// </summary>
            private Disabled()
            {
            }

            /// <summary>
            /// <para>A singleton instance of Disabled</para>
            /// </summary>
            public static readonly Disabled Instance = new Disabled();

            #region Encoder class

            /// <summary>
            /// <para>Encoder for  <see cref="Disabled" />.</para>
            /// </summary>
            private class DisabledEncoder : enc.StructEncoder<Disabled>
            {
                /// <summary>
                /// <para>Encode fields of given value.</para>
                /// </summary>
                /// <param name="value">The value.</param>
                /// <param name="writer">The writer.</param>
                public override void EncodeFields(Disabled value, enc.IJsonWriter writer)
                {
                }
            }

            #endregion

            #region Decoder class

            /// <summary>
            /// <para>Decoder for  <see cref="Disabled" />.</para>
            /// </summary>
            private class DisabledDecoder : enc.StructDecoder<Disabled>
            {
                /// <summary>
                /// <para>Create a new instance of type <see cref="Disabled" />.</para>
                /// </summary>
                /// <returns>The struct instance.</returns>
                protected override Disabled Create()
                {
                    return new Disabled();
                }

                /// <summary>
                /// <para>Decode fields without ensuring start and end object.</para>
                /// </summary>
                /// <param name="reader">The json reader.</param>
                /// <returns>The decoded object.</returns>
                public override Disabled DecodeFields(enc.IJsonReader reader)
                {
                    return Disabled.Instance;
                }
            }

            #endregion
        }

        /// <summary>
        /// <para>Emm token is optional</para>
        /// </summary>
        public sealed class Optional : EmmState
        {
            #pragma warning disable 108

            /// <summary>
            /// <para>The encoder instance.</para>
            /// </summary>
            internal static enc.StructEncoder<Optional> Encoder = new OptionalEncoder();

            /// <summary>
            /// <para>The decoder instance.</para>
            /// </summary>
            internal static enc.StructDecoder<Optional> Decoder = new OptionalDecoder();

            /// <summary>
            /// <para>Initializes a new instance of the <see cref="Optional" /> class.</para>
            /// </summary>
            private Optional()
            {
            }

            /// <summary>
            /// <para>A singleton instance of Optional</para>
            /// </summary>
            public static readonly Optional Instance = new Optional();

            #region Encoder class

            /// <summary>
            /// <para>Encoder for  <see cref="Optional" />.</para>
            /// </summary>
            private class OptionalEncoder : enc.StructEncoder<Optional>
            {
                /// <summary>
                /// <para>Encode fields of given value.</para>
                /// </summary>
                /// <param name="value">The value.</param>
                /// <param name="writer">The writer.</param>
                public override void EncodeFields(Optional value, enc.IJsonWriter writer)
                {
                }
            }

            #endregion

            #region Decoder class

            /// <summary>
            /// <para>Decoder for  <see cref="Optional" />.</para>
            /// </summary>
            private class OptionalDecoder : enc.StructDecoder<Optional>
            {
                /// <summary>
                /// <para>Create a new instance of type <see cref="Optional" />.</para>
                /// </summary>
                /// <returns>The struct instance.</returns>
                protected override Optional Create()
                {
                    return new Optional();
                }

                /// <summary>
                /// <para>Decode fields without ensuring start and end object.</para>
                /// </summary>
                /// <param name="reader">The json reader.</param>
                /// <returns>The decoded object.</returns>
                public override Optional DecodeFields(enc.IJsonReader reader)
                {
                    return Optional.Instance;
                }
            }

            #endregion
        }

        /// <summary>
        /// <para>Emm token is required</para>
        /// </summary>
        public sealed class Required : EmmState
        {
            #pragma warning disable 108

            /// <summary>
            /// <para>The encoder instance.</para>
            /// </summary>
            internal static enc.StructEncoder<Required> Encoder = new RequiredEncoder();

            /// <summary>
            /// <para>The decoder instance.</para>
            /// </summary>
            internal static enc.StructDecoder<Required> Decoder = new RequiredDecoder();

            /// <summary>
            /// <para>Initializes a new instance of the <see cref="Required" /> class.</para>
            /// </summary>
            private Required()
            {
            }

            /// <summary>
            /// <para>A singleton instance of Required</para>
            /// </summary>
            public static readonly Required Instance = new Required();

            #region Encoder class

            /// <summary>
            /// <para>Encoder for  <see cref="Required" />.</para>
            /// </summary>
            private class RequiredEncoder : enc.StructEncoder<Required>
            {
                /// <summary>
                /// <para>Encode fields of given value.</para>
                /// </summary>
                /// <param name="value">The value.</param>
                /// <param name="writer">The writer.</param>
                public override void EncodeFields(Required value, enc.IJsonWriter writer)
                {
                }
            }

            #endregion

            #region Decoder class

            /// <summary>
            /// <para>Decoder for  <see cref="Required" />.</para>
            /// </summary>
            private class RequiredDecoder : enc.StructDecoder<Required>
            {
                /// <summary>
                /// <para>Create a new instance of type <see cref="Required" />.</para>
                /// </summary>
                /// <returns>The struct instance.</returns>
                protected override Required Create()
                {
                    return new Required();
                }

                /// <summary>
                /// <para>Decode fields without ensuring start and end object.</para>
                /// </summary>
                /// <param name="reader">The json reader.</param>
                /// <returns>The decoded object.</returns>
                public override Required DecodeFields(enc.IJsonReader reader)
                {
                    return Required.Instance;
                }
            }

            #endregion
        }

        /// <summary>
        /// <para>The other object</para>
        /// </summary>
        public sealed class Other : EmmState
        {
            #pragma warning disable 108

            /// <summary>
            /// <para>The encoder instance.</para>
            /// </summary>
            internal static enc.StructEncoder<Other> Encoder = new OtherEncoder();

            /// <summary>
            /// <para>The decoder instance.</para>
            /// </summary>
            internal static enc.StructDecoder<Other> Decoder = new OtherDecoder();

            /// <summary>
            /// <para>Initializes a new instance of the <see cref="Other" /> class.</para>
            /// </summary>
            private Other()
            {
            }

            /// <summary>
            /// <para>A singleton instance of Other</para>
            /// </summary>
            public static readonly Other Instance = new Other();

            #region Encoder class

            /// <summary>
            /// <para>Encoder for  <see cref="Other" />.</para>
            /// </summary>
            private class OtherEncoder : enc.StructEncoder<Other>
            {
                /// <summary>
                /// <para>Encode fields of given value.</para>
                /// </summary>
                /// <param name="value">The value.</param>
                /// <param name="writer">The writer.</param>
                public override void EncodeFields(Other value, enc.IJsonWriter writer)
                {
                }
            }

            #endregion

            #region Decoder class

            /// <summary>
            /// <para>Decoder for  <see cref="Other" />.</para>
            /// </summary>
            private class OtherDecoder : enc.StructDecoder<Other>
            {
                /// <summary>
                /// <para>Create a new instance of type <see cref="Other" />.</para>
                /// </summary>
                /// <returns>The struct instance.</returns>
                protected override Other Create()
                {
                    return new Other();
                }

                /// <summary>
                /// <para>Decode fields without ensuring start and end object.</para>
                /// </summary>
                /// <param name="reader">The json reader.</param>
                /// <returns>The decoded object.</returns>
                public override Other DecodeFields(enc.IJsonReader reader)
                {
                    return Other.Instance;
                }
            }

            #endregion
        }
    }
}
