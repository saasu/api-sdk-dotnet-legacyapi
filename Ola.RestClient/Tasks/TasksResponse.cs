using Ola.RestClient.Tasks.Inventory;
using Ola.RestClient.Tasks.Journals;


namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Collections;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "tasksResponse")]
	public class TasksResponse
	{
		[XmlArray(ElementName = "errors")]
		[XmlArrayItem(ElementName = "error", Type = typeof(ErrorInfo))]
		public ArrayList Errors = new ArrayList();
		
		[XmlElement(ElementName = "insertTransactionCategoryResult", Type = typeof(InsertTransactionCategoryResult))]
		[XmlElement(ElementName = "updateTransactionCategoryResult", Type = typeof(UpdateTransactionCategoryResult))]
		[XmlElement(ElementName = "insertContactResult", Type = typeof(InsertContactResult))]
		[XmlElement(ElementName = "updateContactResult", Type = typeof(UpdateContactResult))]
		[XmlElement(ElementName = "insertContactCategoryResult", Type = typeof(InsertContactCategoryResult))]
		[XmlElement(ElementName = "updateContactCategoryResult", Type = typeof(UpdateContactCategoryResult))]
		[XmlElement(ElementName = "insertBankAccountResult", Type = typeof(InsertBankAccountResult))]
		[XmlElement(ElementName = "updateBankAccountResult", Type = typeof(UpdateBankAccountResult))]
		[XmlElement(ElementName = "insertInventoryItemResult", Type = typeof(InsertInventoryItemResult))]
		[XmlElement(ElementName = "updateInventoryItemResult", Type = typeof(UpdateInventoryItemResult))]
		[XmlElement(ElementName = "insertInvoiceResult", Type = typeof(InsertInvoiceResult))]
		[XmlElement(ElementName = "updateInvoiceResult", Type = typeof(UpdateInvoiceResult))]
		[XmlElement(ElementName = "insertInvoicePaymentResult", Type = typeof(InsertInvoicePaymentResult))]
		[XmlElement(ElementName = "updateInvoicePaymentResult", Type = typeof(UpdateInvoicePaymentResult))]
		[XmlElement(ElementName = "emailPdfInvoiceResult", Type = typeof(EmailPdfInvoiceResult))]
		[XmlElement(ElementName = "checkoutResult", Type = typeof(CheckoutResult))]
        [XmlElement(ElementName = "insertInventoryAdjustmentResult", Type = typeof(InsertInventoryAdjustmentResult))]
        [XmlElement(ElementName = "updateInventoryAdjustmentResult", Type = typeof(UpdateInventoryAdjustmentResult))]
        [XmlElement(ElementName = "insertInventoryTransferResult", Type = typeof(InsertInventoryTransferResult))]
        [XmlElement(ElementName = "updateInventoryTransferResult", Type = typeof(UpdateInventoryTransferResult))]

		[XmlElement(ElementName = "insertJournalResult", Type = typeof(InsertJournalResult))]
		[XmlElement(ElementName = "updateJournalResult", Type = typeof(UpdateJournalResult))]

		[XmlElement(ElementName = "insertActivityResult", Type = typeof(InsertActivityResult))]
		[XmlElement(ElementName = "updateActivityResult", Type = typeof(UpdateActivityResult))]


		[XmlElement(ElementName = "insertComboItemResult", Type = typeof(InsertComboItemResult))]
		[XmlElement(ElementName = "updateComboItemResult", Type = typeof(UpdateComboItemResult))]
        [XmlElement(ElementName = "buildComboItemResult", Type = typeof(BuildComboItemResult))]
        public ArrayList Results = new ArrayList();
	}
}
