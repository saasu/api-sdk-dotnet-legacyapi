namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "checkoutResult")]
	public class CheckoutResult : TaskResult
	{
		[XmlAttribute("invoiceUid")]
		public int InvoiceUid;

		[XmlAttribute("invoiceLastUpdatedUid")]
		public string InvoiceLastUpdatedUid = "";

		[XmlAttribute("billingContactUid")]
		public int BillingContactUid = 0;

		[XmlAttribute("billingContactLastUpdatedUid")]
		public string BillingContactLastUpdatedUid = "";

		[XmlAttribute("shippingContactUid")]
		public int ShippingContactUid = 0;

		[XmlAttribute("shippingContactLastUpdatedUid")]
		public string ShippingContactLastUpdatedUid = "";

		[XmlAttribute("ccTxRef")]
		public string CCTxRef = "";
	}
}
