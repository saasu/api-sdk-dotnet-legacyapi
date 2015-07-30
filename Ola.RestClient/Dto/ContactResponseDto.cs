namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "contactResponse")]
	public class ContactResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "contact")]
		public ContactDto Contact;
	}
}
