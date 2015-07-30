using System;

namespace Ola.RestClient.Dto
{
	/// <summary>
	/// Notes: This is zone sensitive. Too see a list of Invoice Types that you could use for your zone,
	/// sign in to your file, then go to Sales > Add or Purchases > Add.
	/// </summary>
	public class InvoiceType
	{
		public const string TaxInvoice = "Tax Invoice";
		public const string SaleInvoice = "Sale Invoice";
		public const string PurchaseInvoice = "Purchase Invoice";
		public const string AdjustmentNote = "Adjustment Note";
		public const string CreditNote = "Credit Note";
		public const string DebitNote = "Debit Note";
		public const string PaymentInvoice = "Payment Invoice";
		public const string RctInvoice = "RCT Invoice";
		public const string MoneyIn = "Money In (Income)";
		public const string MoneyOut = "Money Out (Expense)";
		public const string PurchaseOrder = "Purchase Order";
		public const string SaleOrder = "Sale Order";
		public const string Quote = "Quote";
		public const string PreQuoteOpportunity = "Pre-Quote Opportunity";
		public const string SelfBilling = "Self-Billing";
		public const string Consignment = "Consignment";		// Reserved, not used.
		
		private InvoiceType()
		{
		}
	}
}
