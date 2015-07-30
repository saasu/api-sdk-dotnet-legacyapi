namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	

	[XmlRoot(ElementName = "emailMessage")]
	public class EmailMessageDto
	{
		[XmlElement(ElementName = "from")]
		public string From;

		[XmlElement(ElementName = "to")]
		public string To;

		[XmlElement(ElementName = "cc")]
		public string Cc;

		[XmlElement(ElementName = "bcc")]
		public string Bcc;

		[XmlElement(ElementName = "subject")]
		public string Subject;
		
		[XmlElement(ElementName = "body")]
		public string Body;
	}
}
