using Newtonsoft.Json;
using System;

namespace Telekom
{
    public class JSON_
    {
        #region Product Report
        public partial class ProductReport
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
            public string Label { get; set; }

            [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
            public string Category { get; set; }

            [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
            public string Description { get; set; }

            [JsonProperty("additionalDescription", NullValueHandling = NullValueHandling.Ignore)]
            public string AdditionalDescription { get; set; }

            [JsonProperty("contractExpires", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? ContractExpires { get; set; }

            [JsonProperty("simIdentifier", NullValueHandling = NullValueHandling.Ignore)]
            public string SimIdentifier { get; set; }

            [JsonProperty("simIdentifierName", NullValueHandling = NullValueHandling.Ignore)]
            public string SimIdentifierName { get; set; }

            [JsonProperty("activeAddonsSummary", NullValueHandling = NullValueHandling.Ignore)]
            public ActiveAddonsSummary ActiveAddonsSummary { get; set; }

            [JsonProperty("actualSpending", NullValueHandling = NullValueHandling.Ignore)]
            public ActualSpending ActualSpending { get; set; }

            [JsonProperty("consumptionGroups", NullValueHandling = NullValueHandling.Ignore)]
            public ConsumptionGroup[] ConsumptionGroups { get; set; }

            [JsonProperty("valueAddedServices", NullValueHandling = NullValueHandling.Ignore)]
            public ValueAddedService[] ValueAddedServices { get; set; }

            [JsonProperty("billingAccountId", NullValueHandling = NullValueHandling.Ignore)]
            public string BillingAccountId { get; set; }

            [JsonProperty("creditBalance", NullValueHandling = NullValueHandling.Ignore)]
            public CreditBalance CreditBalance { get; set; }
        }

        public partial class ActiveAddonsSummary
        {
            [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
            public long? Count { get; set; }
        }

        public partial class CreditBalance
        {
            [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
            public Total Total { get; set; }
        }

        public partial class ActualSpending
        {
            [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
            public Item[] Items { get; set; }

            [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
            public Total Total { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("cost", NullValueHandling = NullValueHandling.Ignore)]
            public Total Cost { get; set; }
        }

        public partial class Total
        {
            [JsonProperty("currencyCode", NullValueHandling = NullValueHandling.Ignore)]
            public string CurrencyCode { get; set; }

            [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
            public double? Amount { get; set; }
        }

        public partial class ConsumptionGroup
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }

            [JsonProperty("consumptions", NullValueHandling = NullValueHandling.Ignore)]
            public Consumption[] Consumptions { get; set; }
        }

        public partial class Consumption
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("remaining", NullValueHandling = NullValueHandling.Ignore)]
            public Max Remaining { get; set; }

            [JsonProperty("used", NullValueHandling = NullValueHandling.Ignore)]
            public Max Used { get; set; }

            [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
            public Max Max { get; set; }

            [JsonProperty("remainingPercentage", NullValueHandling = NullValueHandling.Ignore)]
            public long? RemainingPercentage { get; set; }

            [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
            public string Level { get; set; }

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }

            [JsonProperty("boostAction", NullValueHandling = NullValueHandling.Ignore)]
            public string BoostAction { get; set; }

            [JsonProperty("updatedTime", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? UpdatedTime { get; set; }

            [JsonProperty("validUntil", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? ValidUntil { get; set; }

            [JsonProperty("refills", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Refills { get; set; }

            [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
            public long? Priority { get; set; }

            [JsonProperty("categories", NullValueHandling = NullValueHandling.Ignore)]
            public Category[] Categories { get; set; }

            [JsonProperty("bundledBuckets", NullValueHandling = NullValueHandling.Ignore)]
            public object[] BundledBuckets { get; set; }

            [JsonProperty("usageTargets", NullValueHandling = NullValueHandling.Ignore)]
            public object[] UsageTargets { get; set; }

            [JsonProperty("unlimitedDataBackup", NullValueHandling = NullValueHandling.Ignore)]
            public bool? UnlimitedDataBackup { get; set; }
        }

        public partial class Category
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }
        }

        public partial class Max
        {
            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public double? Value { get; set; }

            [JsonProperty("unit", NullValueHandling = NullValueHandling.Ignore)]
            public string Unit { get; set; }
        }

        public partial class SelfCare
        {
            [JsonProperty("enableCallLogs", NullValueHandling = NullValueHandling.Ignore)]
            public bool? EnableCallLogs { get; set; }
        }

        public partial class Swap
        {
            [JsonProperty("swapEligible", NullValueHandling = NullValueHandling.Ignore)]
            public bool? SwapEligible { get; set; }
        }

        public partial class ValueAddedService
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("isAsync", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsAsync { get; set; }

            [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
            public string[] Category { get; set; }

            [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
            public string State { get; set; }

            [JsonProperty("serviceCharacteristics", NullValueHandling = NullValueHandling.Ignore)]
            public ServiceCharacteristic[] ServiceCharacteristics { get; set; }
        }

        public partial class ServiceCharacteristic
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public string Value { get; set; }
        }
        #endregion

        #region Login
        public partial class Login
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            [JsonProperty("detailedStatus", NullValueHandling = NullValueHandling.Ignore)]
            public string DetailedStatus { get; set; }

            [JsonProperty("individual", NullValueHandling = NullValueHandling.Ignore)]
            public Individual Individual { get; set; }

            [JsonProperty("characteristics", NullValueHandling = NullValueHandling.Ignore)]
            public Characteristic[] Characteristics { get; set; }

            [JsonProperty("relatedParties", NullValueHandling = NullValueHandling.Ignore)]
            public RelatedParty[] RelatedParties { get; set; }

            [JsonProperty("contactMediums", NullValueHandling = NullValueHandling.Ignore)]
            public ContactMediums[] ContactMediums { get; set; }

            [JsonProperty("manageableAssets", NullValueHandling = NullValueHandling.Ignore)]
            public ManageableAsset[] ManageableAssets { get; set; }

            [JsonProperty("userProfileRelatedPartyId", NullValueHandling = NullValueHandling.Ignore)]
            public string UserProfileRelatedPartyId { get; set; }

            [JsonProperty("centralToken", NullValueHandling = NullValueHandling.Ignore)]
            public CentralToken CentralToken { get; set; }
        }

        public partial class CentralToken
        {
            [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
            public string Token { get; set; }

            [JsonProperty("expiresIn", NullValueHandling = NullValueHandling.Ignore)]
            public long? ExpiresIn { get; set; }
        }

        public partial class Characteristic
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
            public string Value { get; set; }
        }

        public partial class ContactMediums
        {
            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }

            [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
            public Medium Medium { get; set; }
        }

        public partial class Medium
        {
            [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
            public string Number { get; set; }

            [JsonProperty("emailAddress", NullValueHandling = NullValueHandling.Ignore)]
            public string EmailAddress { get; set; }
        }

        public partial class Individual
        {
            [JsonProperty("givenName", NullValueHandling = NullValueHandling.Ignore)]
            public string GivenName { get; set; }

            [JsonProperty("familyName", NullValueHandling = NullValueHandling.Ignore)]
            public string FamilyName { get; set; }
        }

        public partial class ManageableAsset
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("segment", NullValueHandling = NullValueHandling.Ignore)]
            public string Segment { get; set; }

            [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
            public string Category { get; set; }

            [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
            public string Label { get; set; }

        }

        public partial class RelatedParty
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
            public string Role { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }
        }
        #endregion

        #region Patch Profile
        public partial class PatchProfileResult
        {
            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            [JsonProperty("detailedStatus", NullValueHandling = NullValueHandling.Ignore)]
            public string DetailedStatus { get; set; }
        }
        public partial class PatchSIMResult
        {
            [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
            public string Label { get; set; }
        }
        #endregion

        #region Invoices and bills
        public partial class UnpaidBills
        {
            [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
            public long? Count { get; set; }

            [JsonProperty("cost", NullValueHandling = NullValueHandling.Ignore)]
            public Cost Cost { get; set; }

            [JsonProperty("bills", NullValueHandling = NullValueHandling.Ignore)]
            public Bill[] Bills { get; set; }
        }

        public partial class Bill
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("paymentDueDate", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? PaymentDueDate { get; set; }

            [JsonProperty("billingAccount", NullValueHandling = NullValueHandling.Ignore)]
            public BillingAccount BillingAccount { get; set; }
        }

        public partial class Cost
        {
            [JsonProperty("currencyCode", NullValueHandling = NullValueHandling.Ignore)]
            public string CurrencyCode { get; set; }

            [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
            public double? Amount { get; set; }
        }

        public partial class BillingMonths
        {
            [JsonProperty("month", NullValueHandling = NullValueHandling.Ignore)]
            public long? Month { get; set; }

            [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
            public long? Year { get; set; }
        }

        public partial class CustomerBills
        {
            [JsonProperty("appliedPayment", NullValueHandling = NullValueHandling.Ignore)]
            public object[] AppliedPayment { get; set; }

            [JsonProperty("billDate", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? BillDate { get; set; }

            [JsonProperty("billDocument", NullValueHandling = NullValueHandling.Ignore)]
            public object[] BillDocument { get; set; }

            [JsonProperty("billingAccount", NullValueHandling = NullValueHandling.Ignore)]
            public BillingAccount BillingAccount { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("isBillUnpayable", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsBillUnpayable { get; set; }

            [JsonProperty("relatedParty", NullValueHandling = NullValueHandling.Ignore)]
            public object[] RelatedParty { get; set; }

            [JsonProperty("taxItem", NullValueHandling = NullValueHandling.Ignore)]
            public object[] TaxItem { get; set; }

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
        }

        public partial class BillingAccount
        {
            [JsonProperty("businessId", NullValueHandling = NullValueHandling.Ignore)]
            public string BusinessId { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }
        }

        public partial class BillView
        {
            [JsonProperty("amountDue", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue AmountDue { get; set; }

            [JsonProperty("appliedPayment", NullValueHandling = NullValueHandling.Ignore)]
            public object[] AppliedPayment { get; set; }

            [JsonProperty("billDate", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? BillDate { get; set; }

            [JsonProperty("billDocument", NullValueHandling = NullValueHandling.Ignore)]
            public BillDocument[] BillDocument { get; set; }

            [JsonProperty("billNo", NullValueHandling = NullValueHandling.Ignore)]
            public string BillNo { get; set; }

            [JsonProperty("billingAccount", NullValueHandling = NullValueHandling.Ignore)]
            public BillingAccount BillingAccount { get; set; }

            [JsonProperty("billingRates", NullValueHandling = NullValueHandling.Ignore)]
            public BillingRate[] BillingRates { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("invoiceAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue InvoiceAmount { get; set; }

            [JsonProperty("isBillUnpayable", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsBillUnpayable { get; set; }

            [JsonProperty("isItemizedBill", NullValueHandling = NullValueHandling.Ignore)]
            public bool? IsItemizedBill { get; set; }

            [JsonProperty("paymentDueDate", NullValueHandling = NullValueHandling.Ignore)]
            public DateTimeOffset? PaymentDueDate { get; set; }

            [JsonProperty("relatedParty", NullValueHandling = NullValueHandling.Ignore)]
            public RelatedParty[] RelatedParty { get; set; }

            [JsonProperty("remainingAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue RemainingAmount { get; set; }

            [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
            public string State { get; set; }

            [JsonProperty("taxExcludedAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue TaxExcludedAmount { get; set; }

            [JsonProperty("taxIncludedAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue TaxIncludedAmount { get; set; }

            [JsonProperty("taxItem", NullValueHandling = NullValueHandling.Ignore)]
            public TaxItem[] TaxItem { get; set; }

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
        }

        public partial class AmountDue
        {
            [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
            public double? Amount { get; set; }

            [JsonProperty("units", NullValueHandling = NullValueHandling.Ignore)]
            public string Units { get; set; }
        }

        public partial class BillDocument
        {
            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
            public string Url { get; set; }
        }

        public partial class BillingRate
        {
            [JsonProperty("characteristic", NullValueHandling = NullValueHandling.Ignore)]
            public object[] Characteristic { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string Id { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("taxIncludedAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue TaxIncludedAmount { get; set; }

            [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
            public string Type { get; set; }
        }

        public partial class TaxItem
        {
            [JsonProperty("taxAmount", NullValueHandling = NullValueHandling.Ignore)]
            public AmountDue TaxAmount { get; set; }

            [JsonProperty("taxCategory", NullValueHandling = NullValueHandling.Ignore)]
            public string TaxCategory { get; set; }

            [JsonProperty("taxRate", NullValueHandling = NullValueHandling.Ignore)]
            public long? TaxRate { get; set; }
        }
        #endregion
    }
}
