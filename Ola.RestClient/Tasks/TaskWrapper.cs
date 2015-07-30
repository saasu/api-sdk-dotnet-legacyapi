using Ola.RestClient.Tasks.Inventory;
using Ola.RestClient.Tasks.Journals;


namespace Ola.RestClient.Tasks
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "tasks")]
	public class TaskWrapper
	{
		[XmlElement("insertTransactionCategory", typeof(InsertTransactionCategoryTask))]
		[XmlElement("updateTransactionCategory", typeof(UpdateTransactionCategoryTask))]
		
        [XmlElement("insertContact", typeof(InsertContactTask))]
		[XmlElement("updateContact", typeof(UpdateContactTask))]
		[XmlElement("insertContactCategory", typeof(InsertContactCategoryTask))]
		[XmlElement("updateContactCategory", typeof(UpdateContactCategoryTask))]

		[XmlElement("insertBankAccount", typeof(InsertBankAccountTask))]
		[XmlElement("updateBankAccount", typeof(UpdateBankAccountTask))]
		
        [XmlElement("insertInventoryItem", typeof(InsertInventoryItemTask))]
		[XmlElement("updateInventoryItem", typeof(UpdateInventoryItemTask))]

		[XmlElement("insertComboItem", typeof(InsertComboItemTask))]
		[XmlElement("updateComboItem", typeof(UpdateComboItemTask))]
        [XmlElement("buildComboItem", typeof(BuildComboItemTask))]
		
		[XmlElement("insertInvoice", typeof(InsertInvoiceTask))]
		[XmlElement("updateInvoice", typeof(UpdateInvoiceTask))]

		[XmlElement("insertInvoicePayment", typeof(InsertInvoicePaymentTask))]
		[XmlElement("updateInvoicePayment", typeof(UpdateInvoicePaymentTask))]
		
        [XmlElement("emailPdfInvoice", typeof(EmailPdfInvoiceTask))]
		
        [XmlElement("checkout", typeof(CheckoutTask))]
        
        [XmlElement("insertInventoryAdjustment", typeof(InsertInventoryAdjustmentTask))]
        [XmlElement("updateInventoryAdjustment", typeof(UpdateInventoryAdjustmentTask))]
        [XmlElement("insertInventoryTransfer", typeof(InsertInventoryTransferTask))]
		[XmlElement("updateInventoryTransfer", typeof(UpdateInventoryTransferTask))]
		
		[XmlElement("insertJournal", typeof(InsertJournalTask))]
		[XmlElement("updateJournal", typeof(UpdateJournalTask))]

		[XmlElement("insertActivity", typeof(InsertActivityTask))]
		[XmlElement("updateActivity", typeof(UpdateActivityTask))]
		public ArrayList Tasks = new ArrayList();
	}
}
