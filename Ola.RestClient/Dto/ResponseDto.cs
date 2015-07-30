namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;
	
	
	public abstract class ResponseDto
	{
		[XmlArray(ElementName = "errors")]
		[XmlArrayItem(ElementName = "error", Type=typeof(ErrorInfo))]
		public ArrayList Errors = new ArrayList();
	}
}
