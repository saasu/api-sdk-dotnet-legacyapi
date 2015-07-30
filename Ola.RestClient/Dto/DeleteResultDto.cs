namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;
	
	
	public class DeleteResultDto
	{
		[XmlAttribute(AttributeName = "uid")]
		public int			Uid;
	}
}
