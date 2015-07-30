namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	
	
	public class InsertUpdateResultDto
	{
		[XmlAttribute(AttributeName = "uid")]
		public int		Uid;
		
		[XmlAttribute(AttributeName = "lastUpdatedUid")]
		public string	LastUpdatedUid;
	}
}
