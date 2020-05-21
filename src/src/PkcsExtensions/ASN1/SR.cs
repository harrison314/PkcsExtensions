using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkcsExtensions.ASN1
{
    internal static class SR
    {
        public const string Argument_InvalidOidValue = "The OID value was invalid.";
        public const string Cryptography_Asn_NamedBitListRequiresFlagsEnum = "Named bit list operations require an enum with the [Flags] attribute.";
        public const string Cryptography_Der_Invalid_Encoding = "ASN1 corrupted data.";
        public const string Cryptography_Asn_UnusedBitCountRange = "Unused bit count must be between 0 and 7, inclusive.";
        public const string Cryptography_WriteEncodedValue_OneValueAtATime = "The input to WriteEncodedValue must represent a single encoded value with no trailing data.";
        public const string Cryptography_Asn_EnumeratedValueRequiresNonFlagsEnum = "ASN.1 Enumerated values only apply to enum types without the [Flags] attribute.";
        public const string Cryptography_AsnWriter_EncodeUnbalancedStack = "Encode cannot be called while a Sequence or SetOf is still open.";
        public const string Cryptography_AsnWriter_PopWrongTag = "Cannot pop the requested tag as it is not currently in progress.";
        public const string Cryptography_Asn_UniversalValueIsFixed = "Tags with TagClass Universal must have the appropriate TagValue value for the data type being read or written.";
        public const string Cryptography_Asn_NamedBitListValueTooBig = "The encoded named bit list value is larger than the value size of the '{0}' enum.";
        public const string Cryptography_AsnSerializer_AmbiguousFieldType = "Field '{0}' of type '{1}' has ambiguous type '{2}', an attribute derived from AsnTypeAttribute is required.";
        public const string Cryptography_AsnSerializer_Choice_AllowNullNonNullable = "[Choice].AllowNull=true is not valid because type '{0}' cannot have a null value.";
        public const string Cryptography_AsnSerializer_Choice_NonNullableField = "Field '{0}' on [Choice] type '{1}' can not be assigned a null value.";
        public const string Cryptography_AsnSerializer_Choice_TypeCycle = "Field '{0}' on [Choice] type '{1}' has introduced a type chain cycle.";
        public const string Cryptography_AsnSerializer_Choice_DefaultValueDisallowed = "Field '{0}' on [Choice] type '{1}' has a default value, which is not permitted.";
        public const string Cryptography_AsnSerializer_Choice_ConflictingTagMapping = "The tag ({0} {1}) for field '{2}' on type '{3}' already is associated in this context with field '{4}' on type '{5}'.";
        public const string Cryptography_AsnSerializer_Choice_TooManyValues = "Fields '{0}' and '{1}' on type '{2}' are both non-null when only one value is permitted.";
        public const string Cryptography_AsnSerializer_Choice_NoChoiceWasMade = "An instance of [Choice] type '{0}' has no non-null fields.";
        public const string Cryptography_AsnSerializer_SetValueException = "Unable to set field {0} on type {1}.";
        public const string Cryptography_AsnSerializer_NoOpenTypes = "Type '{0}' cannot be serialized or deserialized because it is not sealed or has unbound generic parameters.";
        public const string Cryptography_AsnSerializer_NoMultiDimensionalArrays = "Type '{0}' cannot be serialized or deserialized because it is a multi-dimensional array.";
        public const string Cryptography_AsnSerializer_NoJaggedArrays = "Type '{0}' cannot be serialized or deserialized because it is an array of arrays.";
        public const string Cryptography_AsnSerializer_UnhandledType = "Could not determine how to serialize or deserialize type '{0}'.";
        public const string Cryptography_AsnSerializer_PopulateFriendlyNameOnString = "Field '{0}' on type '{1}' has [ObjectIdentifier].PopulateFriendlyName set to true, which is not applicable to a string.  Change the field to '{2}' or set PopulateFriendlyName to false.";
        public const string Cryptography_AsnSerializer_UnexpectedTypeForAttribute = "Field '{0}' of type '{1}' has an effective type of '{2}' when one of ({3}) was expected.";
        public const string Cryptography_AsnSerializer_Optional_NonNullableField = "Field '{0}' on type '{1}' is declared [OptionalValue], but it can not be assigned a null value.";
        public const string Cryptography_AsnSerializer_SpecificTagChoice = "Field '{0}' on type '{1}' has specified an implicit tag value via [ExpectedTag] for [Choice] type '{2}'. ExplicitTag must be true, or the [ExpectedTag] attribute removed.";
        public const string Cryptography_AsnSerializer_UtcTimeTwoDigitYearMaxTooSmall = "Field '{0}' on type '{1}' has a [UtcTime] TwoDigitYearMax value ({2}) smaller than the minimum (99).";

        public static string Format(string str, params object[] args)
            => string.Format(str, args);
    }
}
