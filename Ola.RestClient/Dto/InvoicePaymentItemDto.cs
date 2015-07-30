namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	
	public class InvoicePaymentItemDto
	{
		[XmlElement(ElementName = "invoiceUid")]
		public int InvoiceUid;
		
		[XmlElement(ElementName = "amount")]
		public decimal Amount;
	}
}
