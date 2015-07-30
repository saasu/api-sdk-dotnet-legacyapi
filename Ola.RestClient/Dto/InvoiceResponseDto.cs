namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "invoiceResponse")]
	public class InvoiceResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "invoice")]
		public InvoiceDto Invoice;
	}
}
