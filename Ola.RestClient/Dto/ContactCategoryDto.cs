namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;
	
	
	[XmlRoot(ElementName = "contactCategory")]
	public class ContactCategoryDto : EntityDto
	{
		[XmlElement(ElementName = "type")]
		public string		Type;
		
		[XmlElement(ElementName = "name")]
		public string		Name;
	}
}
