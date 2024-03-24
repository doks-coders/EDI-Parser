using Project.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Constants._835ClassProperties;

namespace Project.Constants
{
    internal class _835ClassProperties
    {
        /// <summary>
        /// These are the names of the properties of 835. They placed in positions relative to their index
        /// </summary>
        public static class Properties
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
        /// <summary>
        /// These are the key value pairs of the segment's names and it's properties
        /// </summary>
        public static Dictionary<string, Type> KeyTypePairs = new()
        {
            {"BPR",typeof(Properties.BPRProperties)},
            {"TRN",typeof(Properties.TRNProperties)},
            {"DTM",typeof(Properties.DTMProperties)},

            {"N1",typeof(Properties.N1Properties)},
            {"N3", typeof(Properties.N3Properties)},
            {"N4",typeof(Properties.N4Properties)},
            {"REF", typeof(Properties.REFProperties) },

            {"LX", typeof(Properties.LXProperties) },
            {"CLP", typeof(Properties.CLPProperties) },
            {"NM1", typeof(Properties.NM1Properties) },

            {"SVC", typeof(Properties.SVCProperties) },
            {"CAS", typeof(Properties.CASProperties) },
            {"LQ", typeof(Properties.LQProperties) }

        };
    }
}
