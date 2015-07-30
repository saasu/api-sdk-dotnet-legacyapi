namespace Ola.RestClient.Dto
{
	using System;
	using System.Collections;
	using System.Xml.Serialization;
	
	
	public class CrudResponseDto : ResponseDto
	{
		[XmlElement(ElementName = "deleteResult")]
		public DeleteResultDto			DeleteResult;
	}
}
