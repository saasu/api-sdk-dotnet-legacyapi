using Ola.RestClient.Dto;
using System;
using System.Xml.Serialization;


namespace Ola.RestClient.Tasks
{
	[XmlRoot(ElementName = "checkout")]
	public class CheckoutTask : ITask
	{
		/// <summary>
		/// Which payment gateway connection uid to use?
		/// </summary>
		[XmlElement(ElementName = "pgcUid")]
		public int PgcUid;

		[XmlElement(ElementName = "billingContact")]
		public ContactDto BillingContact = new ContactDto();

		[XmlElement(ElementName = "shippingContact")]
		public ContactDto ShippingContact = new ContactDto();

		[XmlElement(ElementName = "sale")]
		public InvoiceDto Sale = new InvoiceDto();

		[XmlElement(ElementName = "ccPayment")]
		public CCPaymentDto CCPayment = new CCPaymentDto();

		[XmlElement(ElementName = "emailReceipt")]
		public bool EmailReceipt;

		[XmlElement(ElementName = "emailReceiptUsingTemplateUid")]
		public int EmailReceiptUsingTemplateUid;
	}
}
