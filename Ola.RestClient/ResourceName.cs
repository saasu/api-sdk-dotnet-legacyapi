namespace Ola.RestClient
{
	using System;


	public class ResourceName
	{
		public static readonly string TransactionCategory		= "TransactionCategory";
		public static readonly string TransactionCategoryList	= "TransactionCategoryList";
		public static readonly string BankAccount				= "BankAccount";
		public static readonly string BankAccountList			= "BankAccountList";
		public static readonly string Contact					= "Contact";
		public static readonly string ContactList				= "ContactList";
		public static readonly string ContactCategory			= "ContactCategory";
		public static readonly string ContactCategoryList		= "ContactCategoryList";
		public static readonly string Invoice					= "Invoice";
		public static readonly string Journal					= "Journal";
		public static readonly string JournalList				= "JournalList";
		public static readonly string InvoiceList				= "InvoiceList";
		public static readonly string InvoicePayment			= "InvoicePayment";
		public static readonly string InvoicePaymentList		= "InvoicePaymentList";
		public static readonly string InventoryItem				= "InventoryItem";
		public static readonly string InventoryItemList			= "InventoryItemList"; //Depricated. Use FullInventoryItemList instead.
		public static readonly string FullInventoryItemList		= "FullInventoryItemList"; 
        public static readonly string ComboItem                 = "ComboItem";
		public static readonly string ComboItemList				= "ComboItemList"; //Depricated. Use FullComboItemList instead.
		public static readonly string FullComboItemList			= "FullComboItemList";
		public static readonly string EmailPdfInvoice			= "EmailPdfInvoice";
        public static readonly string InventoryAdjustment       = "InventoryAdjustment";
        public static readonly string InventoryAdjustmentList   = "InventoryAdjustmentList";
        public static readonly string InventoryTransfer         = "InventoryTransfer";
        public static readonly string InventoryTransferList     = "InventoryTransferList";
		public static readonly string Activity					= "Activity";
		public static readonly string ActivityList				= "ActivityList";
		public static readonly string TagList					= "TagList";


		private ResourceName()
		{
		}
	}
}
