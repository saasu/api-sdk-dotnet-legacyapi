namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	

	[XmlRoot(ElementName = "invoicePaymentResponse")]
	public class InvoicePaymentResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "invoicePayment")]
		public InvoicePaymentDto InvoicePayment;
	}
}
