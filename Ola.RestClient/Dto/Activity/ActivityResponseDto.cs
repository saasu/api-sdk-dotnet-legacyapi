using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace Ola.RestClient.Dto.Activity
{
	[XmlRoot(ElementName = "activityResponse")]
	public class ActivityResponseDto : CrudResponseDto
	{
		[XmlElement(ElementName = "activity")]
		public ActivityDto Activity;
	}
}
