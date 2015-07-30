namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml;
	using System.Xml.Serialization;
	
	
	[XmlRoot(ElementName = "contactCategoryResponse")]
	public class ContactCategoryResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "contactCategory")]
		public ContactCategoryDto ContactCategory;
	}
}
