namespace Ola.RestClient.Dto
{
	using System;
	using System.Xml.Serialization;

	
	[XmlRoot(ElementName = "error")]
	public class ErrorInfo
	{
		[XmlElement(ElementName = "type")]
		public string Type = "";
		
		[XmlElement(ElementName = "message")]
		public string Message = "";
	}
}
