namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "checkout")]
	public class CheckoutDto : EntityDto
	{
		public CheckoutDto()
		{
		}

		[XmlElement(ElementName = "billingContact")]
		public ContactDto BillingContact = new ContactDto();

		[XmlElement(ElementName = "shippingContact")]
		public ContactDto ShippingContact = new ContactDto();

		[XmlElement(ElementName = "sale")]
		public InvoiceDto Sale = new InvoiceDto();

		[XmlElement(ElementName = "paymentAmount")]
		public decimal PaymentAmount;

		[XmlElement(ElementName = "emailReceipt")]
		public bool EmailReceipt;

		[XmlElement(ElementName = "emailReceiptUsingTemplateUid")]
		public int EmailReceiptUsingTemplateUid;
	}
}
