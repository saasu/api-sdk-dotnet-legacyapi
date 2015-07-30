namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;


	public class EntityDto : IDto
	{	
		[XmlAttribute(AttributeName = "uid")]
		public int Uid;

		[XmlAttribute(AttributeName = "lastUpdatedUid")]
		public string LastUpdatedUid;

		[XmlElement(ElementName = "utcLastModified")] 
		public DateTime UtcLastModified; 
	}
}
