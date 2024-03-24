using EdiEngine.Standards.X12_004010.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utility
{
    internal static class SD
    {
        public enum ISAProperties
        {
            AuthorizationInformationQualifier,

            AuthorizationInformation,

            SecurityInformationQualifier,

            SecurityInformation,

            InterchangeIDQualifier1,

            InterchangeSenderID,

            InterchangeIDQualifier2,

            InterchangeReceiverID,

            InterchangeDate,

            InterchangeTime,

            InterchangeControlStandardsIdentifier,

            InterchangeControlVersionNumber,

            InterchangeControlNumber,

            AcknowledgmentRequested,

            UsageIndicator,

            ComponentElementSeparator
        }

        public enum BPRProperties
        {
            TransactionHandlingCode,

            MonetaryAmount,

            CreditDebitFlagCode,

            PaymentMethodCode,

            PaymentFormatCode,

            DFIIDNumberQualifier1,

            DFIIdentificationNumber1,

            AccountNumberQualifier1,

            AccountNumber1,

            OriginatingCompanyIdentifier,

            OriginatingCompanySupplementalCode,

            DFIIDNumberQualifier2,

            DFIIdentificationNumber2,

            AccountNumberQualifier2,

            AccountNumber2,

            Date
        }

        public enum TRNProperties
        {
            TraceTypeCode,

            ReferenceIdentification,

            OriginatingCompanyIdentifier
        }

        public enum DTMProperties
        {
            DateTimeQualifier,

            Date
        }

        public enum LXProperties
        {
            AssignedNumber
        }

        public enum CLPProperties
        {
            ClaimSubmitterIdentifier,
            ClaimStatusCode,
            MonetaryAmount1,
            MonetaryAmount2,
            MonetaryAmount3,
            ClaimFilingIndicatorCode,
            ReferenceIdentification,
            FacilityCodeValue
        }

        public enum NM1Properties
        {
            EntityIdentifierCode,

            EntityTypeQualifier,

            NameLastorOrganizationName,

            NameFirst,

            NameMiddle,

            NamePrefix,

            NameSuffix,

            IdentificationCodeQualifier,

            IdentificationCode


        }

        public enum REFProperties
        {
            ReferenceIdentificationQualifier,
            ReferenceIdentification
        }

        public enum SVCProperties
        {
            ProductServiceIDQualifier,

            MonetaryAmount1,

            MonetaryAmount2,

            ProductServiceID,

            Quantity

        }

        public enum CASProperties
        {

            ClaimAdjustmentGroupCode,

            ClaimAdjustmentReasonCode,

            MonetaryAmount,

            Quantity,

            ClaimAdjustmentReasonCode1,

            MonetaryAmount1,

            Quantity1,

            ClaimAdjustmentReasonCode2,

            MonetaryAmount2,

            Quantity2



        }

        public enum LQProperties
        {
            CodeListQualifierCode,
            IndustryCode

        }

        public enum N1Properties
        {
            EntityIdentifierCode,

            Name,

            IdentificationCodeQualifier,

            IdentificationCode

        }

        public enum N3Properties
        {
            AddressInformation
        }

        public enum N4Properties
        {
            CityName,

            StateorProvinceCode,

            PostalCode
        }
    }
}
