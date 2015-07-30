namespace Ola.RestClient.Tasks
{
	using System;
	using System.Xml.Serialization;


	public class EmailPdfInvoiceResult : TaskResult
	{
		[XmlAttribute("sentToContact")]
		public bool SentToContact;
	}
}
