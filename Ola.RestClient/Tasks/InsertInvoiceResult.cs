namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "insertInvoiceResult")]
	public class InsertInvoiceResult : InsertResult
	{	
		[XmlAttribute("sentToContact")]
		public bool SentToContact;
		
		[XmlAttribute("generatedInvoiceNumber")]
		public string GeneratedInvoiceNumber;

		[XmlAttribute("generatedPurchaseOrderNumber")]
		public string GeneratedPurchaseOrderNumber;
	}
}
