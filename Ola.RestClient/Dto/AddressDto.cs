namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	

	[XmlRoot(ElementName = "address")]
	public class AddressDto : IDto
	{
		[XmlElement(ElementName = "street")]	
		public string	Street;

		[XmlElement(ElementName = "city")]
		public string	City;

		[XmlElement(ElementName = "state")]	
		public string	State;

		[XmlElement(ElementName = "postCode")]	
		public string	PostCode;

		[XmlElement(ElementName = "country")]	
		public string	Country;
	}
}
