using System;
using System.Diagnostics;

namespace PkcsExtenions.ASN1
{
    // Represents a BER-family encoded tag.
    // T-REC-X.690-201508 sec 8.1.2
    public struct Asn1Tag : IEquatable<Asn1Tag>
    {
        private const byte ClassMask = 0b1100_0000;
        private const byte ConstructedMask = 0b0010_0000;
        private const byte ControlMask = ClassMask | ConstructedMask;
        private const byte TagNumberMask = 0b0001_1111;

        internal static readonly Asn1Tag EndOfContents = new Asn1Tag(0, (int)UniversalTagNumber.EndOfContents);
        internal static readonly Asn1Tag Boolean = new Asn1Tag(0, (int)UniversalTagNumber.Boolean);
        internal static readonly Asn1Tag Integer = new Asn1Tag(0, (int)UniversalTagNumber.Integer);
        internal static readonly Asn1Tag PrimitiveBitString = new Asn1Tag(0, (int)UniversalTagNumber.BitString);
        internal static readonly Asn1Tag ConstructedBitString = new Asn1Tag(ConstructedMask, (int)UniversalTagNumber.BitString);
        internal static readonly Asn1Tag PrimitiveOctetString = new Asn1Tag(0, (int)UniversalTagNumber.OctetString);
        internal static readonly Asn1Tag ConstructedOctetString = new Asn1Tag(ConstructedMask, (int)UniversalTagNumber.OctetString);
        internal static readonly Asn1Tag Null = new Asn1Tag(0, (int)UniversalTagNumber.Null);
        internal static readonly Asn1Tag ObjectIdentifier = new Asn1Tag(0, (int)UniversalTagNumber.ObjectIdentifier);
        internal static readonly Asn1Tag Enumerated = new Asn1Tag(0, (int)UniversalTagNumber.Enumerated);
        internal static readonly Asn1Tag Sequence = new Asn1Tag(ConstructedMask, (int)UniversalTagNumber.Sequence);
        internal static readonly Asn1Tag SetOf = new Asn1Tag(ConstructedMask, (int)UniversalTagNumber.SetOf);
        internal static readonly Asn1Tag UtcTime = new Asn1Tag(0, (int)UniversalTagNumber.UtcTime);
        internal static readonly Asn1Tag GeneralizedTime = new Asn1Tag(0, (int)UniversalTagNumber.GeneralizedTime);

        private readonly byte _controlFlags;

        public TagClass TagClass => (TagClass)(_controlFlags & ClassMask);
        public bool IsConstructed => (_controlFlags & ConstructedMask) != 0;
        public int TagValue { get; }

        private Asn1Tag(byte controlFlags, int tagValue)
        {
            _controlFlags = (byte)(controlFlags & ControlMask);
            TagValue = tagValue;
        }

        public Asn1Tag(UniversalTagNumber universalTagNumber, bool isConstructed = false)
            : this(isConstructed ? ConstructedMask : (byte)0, (int)universalTagNumber)
        {
            // T-REC-X.680-201508 sec 8.6 (Table 1)
            const UniversalTagNumber ReservedIndex = (UniversalTagNumber)15;

            if (universalTagNumber < UniversalTagNumber.EndOfContents
                || universalTagNumber > UniversalTagNumber.RelativeObjectIdentifierIRI
                || universalTagNumber == ReservedIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(universalTagNumber));
            }
        }

        public Asn1Tag(TagClass tagClass, int tagValue, bool isConstructed = false)
            : this((byte)((byte)tagClass | (isConstructed ? ConstructedMask : 0)), tagValue)
        {
            if (tagClass < TagClass.Universal || tagClass > TagClass.Private)
            {
                throw new ArgumentOutOfRangeException(nameof(tagClass));
            }

            if (tagValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tagValue));
            }
        }

        public Asn1Tag AsConstructed()
        {
            return new Asn1Tag((byte)(_controlFlags | ConstructedMask), TagValue);
        }

        public Asn1Tag AsPrimitive()
        {
            return new Asn1Tag((byte)(_controlFlags & ~ConstructedMask), TagValue);
        }

        public static bool TryParse(ReadOnlySpan<byte> source, out Asn1Tag tag, out int bytesRead)
        {
            tag = default(Asn1Tag);
            bytesRead = 0;

            if (source.IsEmpty)
            {
                return false;
            }

            byte first = source[bytesRead];
            bytesRead++;
            uint tagValue = (uint)(first & TagNumberMask);

            if (tagValue == TagNumberMask)
            {
                // Multi-byte encoding
                // T-REC-X.690-201508 sec 8.1.2.4
                const byte ContinuationFlag = 0x80;
                const byte ValueMask = ContinuationFlag - 1;

                tagValue = 0;
                byte current;

                do
                {
                    if (source.Length <= bytesRead)
                    {
                        bytesRead = 0;
                        return false;
                    }

                    current = source[bytesRead];
                    byte currentValue = (byte)(current & ValueMask);
                    bytesRead++;

                    // If TooBigToShift is shifted left 7, the content bit shifts out.
                    // So any value greater than or equal to this cannot be shifted without loss.
                    const int TooBigToShift = 0b00000010_00000000_00000000_00000000;

                    if (tagValue >= TooBigToShift)
                    {
                        bytesRead = 0;
                        return false;
                    }

                    tagValue <<= 7;
                    tagValue |= currentValue;

                    // The first byte cannot have the value 0 (T-REC-X.690-201508 sec 8.1.2.4.2.c)
                    if (tagValue == 0)
                    {
                        bytesRead = 0;
                        return false;
                    }
                }
                while ((current & ContinuationFlag) == ContinuationFlag);

                // This encoding is only valid for tag values greater than 30.
                // (T-REC-X.690-201508 sec 8.1.2.3, 8.1.2.4)
                if (tagValue <= 30)
                {
                    bytesRead = 0;
                    return false;
                }

                // There's not really any ambiguity, but prevent negative numbers from showing up.
                if (tagValue > int.MaxValue)
                {
                    bytesRead = 0;
                    return false;
                }
            }

            Debug.Assert(bytesRead > 0);
            tag = new Asn1Tag(first, (int)tagValue);
            return true;
        }

        public int CalculateEncodedSize()
        {
            const int SevenBits = 0b0111_1111;
            const int FourteenBits = 0b0011_1111_1111_1111;
            const int TwentyOneBits = 0b0001_1111_1111_1111_1111_1111;
            const int TwentyEightBits = 0b0000_1111_1111_1111_1111_1111_1111_1111;

            if (TagValue < TagNumberMask)
                return 1;
            if (TagValue <= SevenBits)
                return 2;
            if (TagValue <= FourteenBits)
                return 3;
            if (TagValue <= TwentyOneBits)
                return 4;
            if (TagValue <= TwentyEightBits)
                return 5;

            return 6;
        }

        public bool TryWrite(Span<byte> destination, out int bytesWritten)
        {
            int spaceRequired = CalculateEncodedSize();

            if (destination.Length < spaceRequired)
            {
                bytesWritten = 0;
                return false;
            }

            if (spaceRequired == 1)
            {
                byte value = (byte)(_controlFlags | TagValue);
                destination[0] = value;
                bytesWritten = 1;
                return true;
            }

            byte firstByte = (byte)(_controlFlags | TagNumberMask);
            destination[0] = firstByte;

            int remaining = TagValue;
            int idx = spaceRequired - 1;

            while (remaining > 0)
            {
                int segment = remaining & 0x7F;

                // The last byte doesn't get the marker, which we write first.
                if (remaining != TagValue)
                {
                    segment |= 0x80;
                }

                Debug.Assert(segment <= byte.MaxValue);
                destination[idx] = (byte)segment;
                remaining >>= 7;
                idx--;
            }

            Debug.Assert(idx == 0);
            bytesWritten = spaceRequired;
            return true;
        }

        public bool Equals(Asn1Tag other)
        {
            return _controlFlags == other._controlFlags && TagValue == other.TagValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Asn1Tag && Equals((Asn1Tag)obj);
        }

        public override int GetHashCode()
        {
            // Most TagValue values will be in the 0-30 range,
            // the GetHashCode value only has collisions when TagValue is
            // between 2^29 and uint.MaxValue
            return (_controlFlags << 24) ^ TagValue;
        }

        public static bool operator ==(Asn1Tag left, Asn1Tag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Asn1Tag left, Asn1Tag right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            const string ConstructedPrefix = "Constructed ";
            string classAndValue;

            if (TagClass == TagClass.Universal)
            {
                classAndValue = ((UniversalTagNumber)TagValue).ToString();
            }
            else
            {
                classAndValue = TagClass + "-" + TagValue;
            }

            if (IsConstructed)
            {
                return ConstructedPrefix + classAndValue;
            }

            return classAndValue;
        }
    }
}
