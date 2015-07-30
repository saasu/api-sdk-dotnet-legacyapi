namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "updateInvoiceResult")]
	public class UpdateInvoiceResult : UpdateResult
	{
		[XmlAttribute("sentToContact")]
		public bool SentToContact;
	}
}
