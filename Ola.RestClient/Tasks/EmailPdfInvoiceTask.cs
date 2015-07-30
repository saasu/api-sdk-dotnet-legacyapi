namespace Ola.RestClient.Tasks
{
	using Ola.RestClient.Dto;

	using System;
	using System.Xml.Serialization;


	[XmlRoot(ElementName = "emailPdfInvoice")]
	public class EmailPdfInvoiceTask : ITask
	{
		[XmlElement(ElementName = "invoiceUid")]
		public int InvoiceUid;
		
		[XmlElement(ElementName = "templateUid")]
		public int TemplateUid;

		[XmlElement(ElementName = "emailMessage")]
		public EmailMessageDto EmailMessage = new EmailMessageDto();
	}
}
